using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Core.Services
{
    public class TimeEntryCalculator
    {
        public decimal ComputeRegularHours(
            TimePeriod workPeriod,
            CurrentShift currentShift,
            bool computeBreakTimeLate)
        {
            var shiftPeriod = currentShift.ShiftPeriod;
            var coveredPeriod = workPeriod.Overlap(shiftPeriod);

            if (currentShift.HasBreaktime && computeBreakTimeLate == false)
            {
                var breakPeriod = currentShift.BreakPeriod;
                var coveredPeriods = coveredPeriod.Difference(breakPeriod);

                return coveredPeriods.Sum(c => c.TotalHours);
            }

            return coveredPeriod.TotalHours;
        }

        public decimal ComputeLateHours(
            TimePeriod workPeriod,
            CurrentShift currentShift,
            bool computeBreakTimeLate)
        {
            var shiftPeriod = currentShift.ShiftPeriod;

            if (workPeriod.EarlierThan(shiftPeriod))
                return 0;

            var latePeriod = new TimePeriod(shiftPeriod.Start, workPeriod.Start);

            if (currentShift.HasBreaktime && computeBreakTimeLate == false)
            {
                var breakPeriod = currentShift.BreakPeriod;
                var latePeriods = latePeriod.Difference(breakPeriod);

                return latePeriods.Sum(l => l.TotalHours);
            }

            return latePeriod.TotalHours;
        }

        public decimal ComputeBreakTimeLateHours(
            TimePeriod workPeriod,
            CurrentShift currentShift,
            IList<TimeAttendanceLog> timeAttendanceLogs,
            IList<BreakTimeBracket> breakTimeBrackets)
        {
            var shiftPeriod = currentShift.ShiftPeriod;

            var startTime = workPeriod.Start >= shiftPeriod.Start ? workPeriod.Start : shiftPeriod.Start;
            var endTime = workPeriod.End >= shiftPeriod.End ? shiftPeriod.End : workPeriod.End;

            var logs = timeAttendanceLogs
                .Where(l => l.TimeStamp >= startTime)
                .Where(l => l.TimeStamp <= endTime)
                .OrderBy(l => l.TimeStamp)
                .ToList();

            var totalBreakTimeLateHours = GetTotalBreakTimeLateHours(logs);

            if (breakTimeBrackets == null || breakTimeBrackets.Count == 0)
                return totalBreakTimeLateHours;

            var breakTimeDuration = BreakTimeBracketHelper.GetBreakTimeDuration(breakTimeBrackets, shiftPeriod.Length.TotalHours);

            var finalBreakTimeLateHours = totalBreakTimeLateHours - breakTimeDuration;

            if (finalBreakTimeLateHours < 0)
                return 0;

            return finalBreakTimeLateHours;
        }

        public decimal ComputeUndertimeHours(TimePeriod workPeriod, CurrentShift currentShift, bool computeBreakTimeLate)
        {
            var shiftPeriod = currentShift.ShiftPeriod;

            if (workPeriod.LaterThan(shiftPeriod))
                return 0;

            var undertimePeriod = new TimePeriod(workPeriod.End, shiftPeriod.End);

            if (currentShift.HasBreaktime && computeBreakTimeLate == false)
            {
                var breakPeriod = currentShift.BreakPeriod;
                var undertimePeriods = undertimePeriod.Difference(breakPeriod);

                return undertimePeriods.Sum(u => u.TotalHours);
            }

            return undertimePeriod.TotalHours;
        }

        public decimal ComputeNightDiffHours(TimePeriod workPeriod, CurrentShift currentShift, TimePeriod nightDiffPeriod, bool shouldBreakTime, bool computeBreakTimeLate)
        {
            if (!workPeriod.Intersects(nightDiffPeriod))
                return 0M;

            var nightWorked = workPeriod.Overlap(nightDiffPeriod);

            var nightDiffHours = 0M;

            if (shouldBreakTime && currentShift.HasBreaktime && computeBreakTimeLate == false)
            {
                var breakPeriod = currentShift.BreakPeriod;
                var nightWorkedPeriods = nightWorked.Difference(breakPeriod);

                nightDiffHours = nightWorkedPeriods.Sum(n => n.TotalHours);
            }
            else
                nightDiffHours = nightWorked.TotalHours;

            return nightDiffHours;
        }

        public decimal ComputeOvertimeHours(TimePeriod workPeriod, Overtime overtime, CurrentShift shift, TimePeriod breaktime)
        {
            var overtimeWorked = GetOvertimeWorked(workPeriod, overtime, shift);

            decimal? overtimeHours;
            if (breaktime != null)
            {
                overtimeHours = overtimeWorked?.Difference(breaktime).Sum(o => o.TotalHours);
            }
            else
            {
                overtimeHours = overtimeWorked?.TotalHours;
            }

            return overtimeHours ?? 0;
        }

        public decimal ComputeNightDiffOTHours(TimePeriod workPeriod, Overtime overtime, CurrentShift shift, TimePeriod nightDiffPeriod, TimePeriod breaktime)
        {
            var overtimeWorked = GetOvertimeWorked(workPeriod, overtime, shift);

            var nightOvertimeWorked = overtimeWorked?.Overlap(nightDiffPeriod);

            decimal? nightDiffOTHours;
            if (breaktime != null)
            {
                nightDiffOTHours = nightOvertimeWorked?.Difference(breaktime).Sum(o => o.TotalHours);
            }
            else
            {
                nightDiffOTHours = nightOvertimeWorked?.TotalHours;
            }

            return nightDiffOTHours ?? 0;
        }

        private TimePeriod GetOvertimeWorked(TimePeriod workPeriod, Overtime overtime, CurrentShift shift)
        {
            var otStartTime = overtime.OTStartTime ?? shift.End.TimeOfDay;
            var otEndTime = overtime.OTEndTime ?? shift.Start.TimeOfDay;

            var overtimeStart = overtime.OTStartDate.Add(otStartTime);
            var nextDay = overtime.OTStartDate.AddDays(1);

            if (otEndTime == otStartTime)
                return null;

            var overtimeEnd = otEndTime > otStartTime ? overtime.OTStartDate.Add(otEndTime) : nextDay.Add(otEndTime);

            var overtimeRecognized = new TimePeriod(overtimeStart, overtimeEnd);

            var overtimeWorked = workPeriod.Overlap(overtimeRecognized);

            // If overtime worked is nothing, perhaps the overtime was meant to happen in the next day past midnight.
            if (overtimeWorked == null)
            {
                overtimeStart = nextDay.Add(otStartTime);
                overtimeEnd = nextDay.Add(otEndTime);

                overtimeRecognized = new TimePeriod(overtimeStart, overtimeEnd);

                overtimeWorked = workPeriod.Overlap(overtimeRecognized);
            }

            return overtimeWorked;
        }

        public static decimal ComputeLeaveHours(TimePeriod leavePeriod, CurrentShift currentShift, bool computeBreakTimeLate)
        {
            if (currentShift.HasBreaktime && computeBreakTimeLate == false)
            {
                var breakPeriod = currentShift.BreakPeriod;

                return leavePeriod.Difference(breakPeriod).Sum(l => l.TotalHours);
            }

            return leavePeriod.TotalHours;
        }

        private decimal GetTotalBreakTimeLateHours(List<TimeAttendanceLog> logs)
        {
            // get the late periods
            List<TimePeriod> latePeriods = new List<TimePeriod>();

            DateTime lastOut = new DateTime();
            DateTime firstIn;
            bool isLookingForBreakTimeOut = true;

            foreach (var log in logs)
            {
                if (isLookingForBreakTimeOut)
                {
                    if (log.IsTimeIn == false)
                    {
                        lastOut = log.TimeStamp;

                        isLookingForBreakTimeOut = false;
                    }
                }
                else
                {
                    // still get the time out to get the last OUT
                    if (log.IsTimeIn == false)
                    {
                        lastOut = log.TimeStamp;
                    }
                    else
                    {
                        // here in else, we found the first IN since the last employee OUT for breaktime
                        // this will be the late timeperiod
                        firstIn = log.TimeStamp;

                        isLookingForBreakTimeOut = true;

                        latePeriods.Add(new TimePeriod(lastOut, firstIn));
                    }
                }
            }

            // compute the late hours
            return latePeriods.Sum(l => l.TotalHours);
        }
    }
}