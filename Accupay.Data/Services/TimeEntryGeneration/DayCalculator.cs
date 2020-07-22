using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class DayCalculator
    {
        private const int DEFAULT_WORK_HOURS = 8;
        private readonly ListOfValueCollection _settings;
        private readonly Organization _organization;
        private readonly Employee _employee;
        private readonly IEmploymentPolicy _employmentPolicy;
        private readonly TimeEntryPolicy _policy;

        public DayCalculator(
            Organization organization,
            ListOfValueCollection settings,
            Employee employee,
            IEmploymentPolicy employmentPolicy)
        {
            _settings = settings;
            _organization = organization;
            _employee = employee;
            _policy = new TimeEntryPolicy(settings);
            _employmentPolicy = employmentPolicy;
        }

        public TimeEntry Compute(DateTime currentDate,
                                Salary salary,
                                IList<TimeEntry> oldTimeEntries,
                                ShiftSchedule employeeShift,
                                EmployeeDutySchedule shiftSched,
                                TimeLog timeLog,
                                IList<Overtime> overtimes,
                                OfficialBusiness officialBusiness,
                                IList<Leave> leaves,
                                IList<TimeAttendanceLog> timeAttendanceLogs,
                                IList<BreakTimeBracket> breakTimeBrackets,
                                IPayrate payrate,
                                CalendarCollection calendarCollection,
                                int? branchId)
        {
            var timeEntry = oldTimeEntries.Where(t => t.Date == currentDate).SingleOrDefault();

            if (timeEntry == null)
                timeEntry = new TimeEntry()
                {
                    EmployeeID = _employee.RowID,
                    OrganizationID = _organization.RowID,
                    Date = currentDate
                };

            timeEntry.Reset();

            bool isBetweenSalaryDates = false;
            if (salary != null)
                isBetweenSalaryDates = currentDate.Date >= salary.EffectiveFrom &&
                            (salary.EffectiveTo == null || currentDate.Date <= salary.EffectiveTo.Value);

            // TODO: return this as one the list of errors of Time entry generation
            // No covered salary for this date
            if (isBetweenSalaryDates == false)
                return timeEntry;

            var currentShift = GetCurrentShift(currentDate, employeeShift, shiftSched, _policy.UseShiftSchedule, _policy.RespectDefaultRestDay, _employee.DayOfRest);

            timeEntry.BranchID = branchId;
            timeEntry.IsRestDay = currentShift.IsRestDay;
            timeEntry.HasShift = currentShift.HasShift;

            if (timeEntry.HasShift)
            {
                timeEntry.WorkHours = currentShift.WorkingHours;
                timeEntry.ShiftHours = currentShift.ShiftHours;
            }

            var hasWorkedLastDay = PayrollTools.HasWorkedLastWorkingDay(currentDate, oldTimeEntries.ToList(), calendarCollection);

            ComputeHours(currentDate, timeEntry, timeLog, officialBusiness, leaves, overtimes, timeAttendanceLogs, breakTimeBrackets, currentShift, hasWorkedLastDay, payrate);
            ComputePay(timeEntry, currentDate, currentShift, salary, payrate, hasWorkedLastDay);

            return timeEntry;
        }

        private void ComputeHours(DateTime currentDate,
                                TimeEntry timeEntry,
                                TimeLog timeLog,
                                OfficialBusiness officialBusiness,
                                IList<Leave> leaves,
                                IList<Overtime> overtimes,
                                IList<TimeAttendanceLog> timeAttendanceLogs,
                                IList<BreakTimeBracket> breakTimeBrackets,
                                CurrentShift currentShift,
                                bool hasWorkedLastDay,
                                IPayrate payrate)
        {
            var previousDay = currentDate.AddDays(-1);
            var calculator = new TimeEntryCalculator();

            timeEntry.EmployeeShiftID = currentShift.ShiftSchedule?.RowID;

            var hasTimeLog = (timeLog?.TimeIn != null && timeLog?.TimeOut != null) ||
                                officialBusiness != null;

            TimePeriod logPeriod = null;
            if (hasTimeLog)
            {
                logPeriod = GetLogPeriod(timeLog, officialBusiness, currentShift, currentDate);
            }

            if (_policy.PaidAsLongAsPresent)
            {
                var atLeastHasTimeLogs = timeLog?.TimeIn != null || timeLog?.TimeOut != null;
                if (atLeastHasTimeLogs)
                {
                    timeEntry.RegularHours = DEFAULT_WORK_HOURS;
                }
            }

            if (logPeriod != null)
            {
                TimePeriod dutyPeriod = null;

                var shiftPeriod = currentShift.ShiftPeriod;

                if (shiftPeriod != null)
                {
                    dutyPeriod = shiftPeriod.Overlap(logPeriod);
                }

                if (dutyPeriod != null)
                {
                    timeEntry.RegularHours = calculator.ComputeRegularHours(dutyPeriod, currentShift, _policy.ComputeBreakTimeLate);

                    var coveredPeriod = dutyPeriod;

                    TimePeriod leavePeriod = null;
                    if (leaves.Any())
                    {
                        var leave = leaves.FirstOrDefault();
                        leavePeriod = GetLeavePeriod(leave, currentShift);

                        coveredPeriod = new TimePeriod(
                                    new DateTime[] { dutyPeriod.Start, leavePeriod.Start }.Min(),
                                    new DateTime[] { dutyPeriod.End, leavePeriod.End }.Max());

                        var leaveHours = TimeEntryCalculator.ComputeLeaveHours(
                                                                        leavePeriod,
                                                                        currentShift,
                                                                        _policy.ComputeBreakTimeLate);

                        timeEntry.SetLeaveHours(leave.LeaveType, leaveHours);
                    }

                    timeEntry.LateHours = calculator.ComputeLateHours(coveredPeriod, currentShift, _policy.ComputeBreakTimeLate);

                    timeEntry.UndertimeHours = calculator.ComputeUndertimeHours(coveredPeriod, currentShift, _policy.ComputeBreakTimeLate);

                    OverrideLateAndUndertimeHoursComputations(timeEntry, currentShift, dutyPeriod, leavePeriod, _policy);

                    if (_policy.ComputeBreakTimeLate)
                        timeEntry.LateHours += calculator.ComputeBreakTimeLateHours(coveredPeriod, currentShift, timeAttendanceLogs, breakTimeBrackets);

                    if (_policy.LateHoursRoundingUp)
                    {
                        var lateHours = timeEntry.LateHours;

                        if (lateHours > 0.5M && lateHours <= 1)
                            timeEntry.LateHours = 1;
                        else if (lateHours >= 2 && lateHours <= 4)
                            timeEntry.LateHours = 4;
                    }

                    timeEntry.RegularHours = currentShift.WorkingHours -
                                                (timeEntry.LateHours + timeEntry.UndertimeHours);

                    if (leavePeriod != null)
                    {
                        var coveredLeavePeriod = new TimePeriod(
                                    new DateTime[] { currentShift.Start, leavePeriod.Start }.Max(),
                                    new DateTime[] { currentShift.End, leavePeriod.End }.Min());

                        timeEntry.RegularHours -= calculator.ComputeRegularHours(coveredLeavePeriod, currentShift, _policy.ComputeBreakTimeLate);
                    }

                    TimePeriod nightBreaktime = null;
                    if (_policy.HasNightBreaktime)
                        nightBreaktime = new TimePeriod(
                                                currentDate.Add(TimeSpan.Parse("21:00")),
                                                currentDate.Add(TimeSpan.Parse("22:00")));

                    timeEntry.OvertimeHours = overtimes.Sum(o => calculator.ComputeOvertimeHours(logPeriod, o, currentShift, nightBreaktime));

                    ComputeNightDiffHours(calculator, timeEntry, currentShift, dutyPeriod, logPeriod, currentDate, previousDay, overtimes, nightBreaktime);
                }

                ComputeHolidayHours(payrate, timeEntry);
                ComputeRestDayHours(currentShift, timeEntry, logPeriod);

                timeEntry.BasicHours = timeEntry.RegularHours +
                                        timeEntry.RestDayHours +
                                        timeEntry.RegularHolidayHours +
                                        timeEntry.SpecialHolidayHours;
            }

            ComputeAbsentHours(timeEntry, payrate, hasWorkedLastDay, currentShift, leaves);
            ComputeLeaveHours(hasTimeLog, leaves, currentShift, timeEntry, payrate);
        }

        private TimePeriod GetLogPeriod(TimeLog timeLog,
                                    OfficialBusiness officialBusiness,
                                    CurrentShift currentShift,
                                    DateTime currentDate)
        {
            var appliedIn = new TimeSpan?[] { timeLog?.TimeIn, officialBusiness?.StartTime }.
                                            Where(i => i.HasValue).
                                            Min();

            var appliedOut = new TimeSpan?[] { timeLog?.TimeOut, officialBusiness?.EndTime }.
                                            Where(i => i.HasValue).
                                            Max();

            if (!appliedIn.HasValue || !appliedOut.HasValue)
                return null;

            var logPeriod = TimePeriod.FromTime(appliedIn.Value, appliedOut.Value, currentDate);

            if (currentShift.HasShift)
            {
                var graceTime = _employmentPolicy.GracePeriod;

                var shiftStart = currentShift.ShiftPeriod.Start;
                var gracePeriod = new TimePeriod(shiftStart, shiftStart.AddMinutes((double)graceTime));

                if (gracePeriod.Contains(logPeriod.Start))
                    logPeriod = TimePeriod.FromTime(shiftStart.TimeOfDay, appliedOut.Value, currentDate);
            }

            return logPeriod;
        }

        private void OverrideLateAndUndertimeHoursComputations(TimeEntry timeEntry,
                                                                CurrentShift currentShift,
                                                                TimePeriod dutyPeriod,
                                                                TimePeriod leavePeriod,
                                                                TimeEntryPolicy policy)
        {
            var output = ComputeLateAndUndertimeHours(currentShift.ShiftPeriod, dutyPeriod, leavePeriod, currentShift.BreakPeriod, policy.ComputeBreakTimeLate);

            if (output.Item1 != null && output.Item2 != null)
            {
                timeEntry.LateHours = output.Item1.Value;
                timeEntry.UndertimeHours = output.Item2.Value;
            }
        }

        // TODO: transfer this to TimeEntryCalculator
        public static Tuple<decimal?, decimal?> ComputeLateAndUndertimeHours(
                                                                    TimePeriod shiftPeriod,
                                                                    TimePeriod dutyPeriod,
                                                                    TimePeriod leavePeriod,
                                                                    TimePeriod breakPeriod,
                                                                    bool computeBreakTimeLatePolicy)
        {
            decimal lateHours, undertimeHours;

            var shiftAndDutyDifference = new List<TimePeriod>();

            if (shiftPeriod == null)
                return new Tuple<decimal?, decimal?>(null, null);

            if (dutyPeriod != null)
                shiftAndDutyDifference = shiftPeriod.Difference(dutyPeriod).ToList();
            else
                shiftAndDutyDifference.Add(shiftPeriod);

            var latePeriods = GetLateAndUndertimePeriods(shiftPeriod, dutyPeriod, leavePeriod, shiftAndDutyDifference);

            TimePeriod latePeriod = latePeriods.Item1;
            TimePeriod inBetweenUndertimePeriod = latePeriods.Item2;
            TimePeriod undertimePeriod = latePeriods.Item3;

            decimal inBetweenUndertimeHours = 0;

            if (inBetweenUndertimePeriod != null)
                inBetweenUndertimeHours = ComputeHoursNotCoveredByLeave(breakPeriod, computeBreakTimeLatePolicy, inBetweenUndertimePeriod, leavePeriod);

            lateHours = ComputeHoursNotCoveredByLeave(breakPeriod, computeBreakTimeLatePolicy, latePeriod, leavePeriod);
            undertimeHours = ComputeHoursNotCoveredByLeave(breakPeriod, computeBreakTimeLatePolicy, undertimePeriod, leavePeriod);

            undertimeHours += inBetweenUndertimeHours;

            return new Tuple<decimal?, decimal?>(lateHours, undertimeHours);
        }

        private static Tuple<TimePeriod, TimePeriod, TimePeriod> GetLateAndUndertimePeriods(
                                                        TimePeriod shiftPeriod,
                                                        TimePeriod dutyPeriod,
                                                        TimePeriod leavePeriod,
                                                        List<TimePeriod> shiftAndDutyDifference)
        {
            TimePeriod undertimePeriod = null;
            TimePeriod latePeriod = null;
            TimePeriod inBetweenUndertimePeriod = null;

            if (shiftAndDutyDifference.Any())
            {
                if (shiftAndDutyDifference.Count == 2)
                {
                    latePeriod = shiftAndDutyDifference[0];
                    undertimePeriod = shiftAndDutyDifference[1];

                    if (leavePeriod != null)
                    {
                        // check if there is undertime after leave
                        // ex 9am-6pm shift / 10am-3pm leave / 4pm-5pm duty period / 9am-10am Late - 3pm-4pm UT & 5pm-6pm UT
                        var latePeriods = latePeriod.Difference(leavePeriod);
                        if (latePeriods.Count == 2)
                        {
                            latePeriod = latePeriods[0];

                            // this is an in between undertime and will
                            // be added on to the end of the day undertime
                            inBetweenUndertimePeriod = latePeriods[1];
                        }
                        else if (latePeriods.Count == 1)
                        {
                            // check if the late is a late or undertime
                            if (leavePeriod.Start <= shiftPeriod.Start ||
                                (dutyPeriod != null && dutyPeriod.Start <= shiftPeriod.Start))
                            {
                                latePeriod = null;
                                // this is an in between undertime and will
                                // be added on to the end of the day undertime
                                inBetweenUndertimePeriod = latePeriods[0];
                            }
                            else
                            {
                                latePeriod = latePeriods[0];
                            }
                        }
                    }
                }
                else if (shiftAndDutyDifference.Count == 1)
                {
                    if ((leavePeriod != null && leavePeriod.Start <= shiftPeriod.Start) ||
                        (dutyPeriod != null && dutyPeriod.Start <= shiftPeriod.Start))

                        // ex. 9am-6pm shift / 9am-12pm leave
                        undertimePeriod = shiftAndDutyDifference[0];
                    else
                    {
                        // ex. 9am-6pm shift / 3pm-6pm leave
                        latePeriod = shiftAndDutyDifference[0];

                        if (leavePeriod != null)
                        {
                            // check if there is undertime after leave
                            // ex 9am-6pm shift / 3pm-5pm leave / 9am-3pm Late - 5pm-6pm Undertime
                            var latePeriods = latePeriod.Difference(leavePeriod);
                            if (latePeriods.Count == 2)
                            {
                                latePeriod = latePeriods[0];

                                undertimePeriod = latePeriods[1];
                            }
                        }
                    }
                }
            }

            return new Tuple<TimePeriod, TimePeriod, TimePeriod>(latePeriod,
                                                                inBetweenUndertimePeriod,
                                                                undertimePeriod);
        }

        private static decimal ComputeHoursNotCoveredByLeave(TimePeriod breakPeriod,
                                                            bool computeBreakTimeLatePolicy,
                                                            TimePeriod notCoveredByLeavePeriod,
                                                            TimePeriod leavePeriod)
        {
            decimal hours = 0;

            if (notCoveredByLeavePeriod == null)
                return hours;

            var notCoveredByLeavePeriods = new List<TimePeriod>();

            if (leavePeriod != null)
            {
                notCoveredByLeavePeriods = notCoveredByLeavePeriod.Difference(leavePeriod).ToList();
            }
            else
            {
                notCoveredByLeavePeriods.Add(notCoveredByLeavePeriod);
            }

            if (notCoveredByLeavePeriods.Any())
            {
                if (notCoveredByLeavePeriods.Count == 2)
                {
                    var periodBeforeLeave = notCoveredByLeavePeriods[0];
                    var periodAfterLeave = notCoveredByLeavePeriods[1];

                    hours = ComputeHoursWithBreaktime(breakPeriod, computeBreakTimeLatePolicy, periodBeforeLeave);
                    hours += ComputeHoursWithBreaktime(breakPeriod, computeBreakTimeLatePolicy, periodAfterLeave);
                }
                else
                    hours = ComputeHoursWithBreaktime(breakPeriod, computeBreakTimeLatePolicy, notCoveredByLeavePeriods[0]);
            }

            return hours;
        }

        private static decimal ComputeHoursWithBreaktime(TimePeriod breakPeriod,
                                                        bool computeBreakTimeLatePolicy,
                                                        TimePeriod hoursPeriod)
        {
            decimal hours = 0;

            if (breakPeriod != null && computeBreakTimeLatePolicy == false)
            {
                var hoursWithoutBreaktimes = hoursPeriod.Difference(breakPeriod);
                hours = hoursWithoutBreaktimes.Sum(l => l.TotalHours);
            }
            else
                hours = hoursPeriod.TotalHours;

            return hours;
        }

        private void ComputeNightDiffHours(TimeEntryCalculator calculator,
                                            TimeEntry timeEntry,
                                            CurrentShift currentShift,
                                            TimePeriod dutyPeriod,
                                            TimePeriod logPeriod,
                                            DateTime currentDate,
                                            DateTime previousDay,
                                            IList<Overtime> overtimes,
                                            TimePeriod nightBreaktime)
        {
            if (_employmentPolicy.ComputeNightDiff && currentShift.IsNightShift)
            {
                var nightDiffPeriod = GetNightDiffPeriod(currentDate);
                var dawnDiffPeriod = GetNightDiffPeriod(previousDay);

                timeEntry.NightDiffHours = calculator.ComputeNightDiffHours(dutyPeriod, currentShift, nightDiffPeriod, _policy.HasNightBreaktime, _policy.ComputeBreakTimeLate) + calculator.ComputeNightDiffHours(dutyPeriod, currentShift, dawnDiffPeriod, _policy.HasNightBreaktime, _policy.ComputeBreakTimeLate);

                timeEntry.NightDiffOTHours = overtimes.Sum(o => calculator.ComputeNightDiffOTHours(logPeriod, o, currentShift, nightDiffPeriod, nightBreaktime) + calculator.ComputeNightDiffOTHours(logPeriod, o, currentShift, dawnDiffPeriod, nightBreaktime));
            }
        }

        private TimePeriod GetNightDiffPeriod(DateTime date)
        {
            var nightDiffTimeFrom = _organization.NightDifferentialTimeFrom;
            var nightDiffTimeTo = _organization.NightDifferentialTimeTo;

            return TimePeriod.FromTime(nightDiffTimeFrom, nightDiffTimeTo, date);
        }

        private void ComputeHolidayHours(IPayrate payrate, TimeEntry timeEntry)
        {
            if (!((_employmentPolicy.ComputeRegularHoliday && payrate.IsRegularHoliday) ||
                (_employmentPolicy.ComputeSpecialHoliday && payrate.IsSpecialNonWorkingHoliday)))
                return;

            if (payrate.IsRegularHoliday)
            {
                timeEntry.RegularHolidayHours = timeEntry.RegularHours;
                timeEntry.RegularHolidayOTHours = timeEntry.OvertimeHours;
            }
            else if (payrate.IsSpecialNonWorkingHoliday)
            {
                timeEntry.SpecialHolidayHours = timeEntry.RegularHours;
                timeEntry.SpecialHolidayOTHours = timeEntry.OvertimeHours;
            }

            timeEntry.RegularHours = 0;
            timeEntry.OvertimeHours = 0;

            timeEntry.LateHours = 0;
            timeEntry.UndertimeHours = 0;
        }

        private void ComputeRestDayHours(CurrentShift currentShift,
                                        TimeEntry timeEntry,
                                        TimePeriod logPeriod)
        {
            if (!(currentShift.IsRestDay && _employmentPolicy.ComputeRestDay))
                return;

            if (_policy.IgnoreShiftOnRestDay)
            {
                timeEntry.RegularHours = logPeriod.TotalHours;
            }

            timeEntry.RestDayHours = timeEntry.RegularHours;
            timeEntry.RegularHours = 0;

            timeEntry.RestDayOTHours = timeEntry.OvertimeHours;
            timeEntry.OvertimeHours = 0;

            timeEntry.UndertimeHours = 0;
            timeEntry.LateHours = 0;
        }

        private void ComputeAbsentHours(TimeEntry timeEntry,
                                        IPayrate payrate,
                                        bool hasWorkedLastDay,
                                        CurrentShift currentshift,
                                        IList<Leave> leaves)
        {
            if (leaves.Any())
                return;

            if (timeEntry.BasicHours > 0)
                return;

            if (payrate.IsRegularDay && (currentshift.IsRestDay || (!currentshift.HasShift)))
                return;

            if (IsExemptDueToHoliday(payrate, hasWorkedLastDay))
                return;

            timeEntry.AbsentHours = currentshift.WorkingHours;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="payrate"></param>
        /// <param name="hasWorkedLastDay"></param>
        /// <returns>True means no absence, false means possibly absent.</returns>
        private bool IsExemptDueToHoliday(IPayrate payrate, bool hasWorkedLastDay)
        {
            // If it's not a holiday, then employee is not exempt
            if (!payrate.IsHoliday)
                return false;

            if (_employee.IsDaily)
                return true;

            if (IsHolidayExempt(payrate))
                return true;

            if (!_policy.AbsencesOnHoliday)
                return true;

            return ((!_policy.RequiredToWorkLastDay) || hasWorkedLastDay);
        }

        private bool IsHolidayExempt(IPayrate payrate)
        {
            var isCalculatingRegularHoliday = payrate.IsRegularHoliday && _employmentPolicy.ComputeRegularHoliday;
            var isCalculatingSpecialHoliday = payrate.IsSpecialNonWorkingHoliday && _employmentPolicy.ComputeSpecialHoliday;

            return !(isCalculatingRegularHoliday || isCalculatingSpecialHoliday);
        }

        /// <summary>
        /// Updates timeentry. This sets the leave hours only if there is no time logs. This will also increment the undertime hours if its is a working day (with timelogs or not).
        /// </summary>
        /// <param name="leaves">The list of leave for this day.</param>
        /// <param name="currentShift">The shift for this day.</param>
        /// <param name="timeEntry">The timeEntry object that will be updated.</param>
        /// <param name="payrate">Used to check if current day is a holiday.</param>
        private void ComputeLeaveHours(bool hasTimeLog,
                                        IList<Leave> leaves,
                                        CurrentShift currentShift,
                                        TimeEntry timeEntry,
                                        IPayrate payrate)
        {
            if (!hasTimeLog && leaves.Any())
            {
                var leave = leaves.FirstOrDefault();
                decimal leaveHours = ComputeLeaveHoursWithoutTimelog(currentShift,
                                                                    leave,
                                                                    _policy.ComputeBreakTimeLate);

                timeEntry.SetLeaveHours(leave.LeaveType, leaveHours);
            }

            if (leaves.Any() && currentShift.IsWorkingDay)
            {
                var requiredHours = currentShift.WorkingHours;
                var missingHours = requiredHours - (timeEntry.TotalLeaveHours +
                                                    timeEntry.RegularHours +
                                                    timeEntry.LateHours +
                                                    timeEntry.UndertimeHours);

                if (missingHours > 0 && !payrate.IsHoliday)
                {
                    timeEntry.UndertimeHours += missingHours;
                }
            }
        }

        private void ComputePay(TimeEntry timeEntry,
                                DateTime currentDate,
                                CurrentShift currentShift,
                                Salary salary,
                                IPayrate payrate,
                                bool hasWorkedLastDay)
        {
            // TODO: return this as one the list of errors of Time entry generation
            // Employee has not started yet according to the employee's Start Date
            if (currentDate < _employee.StartDate)
            {
                timeEntry.Reset();

                return;
            }

            var dailyRate = PayrollTools.GetDailyRate(salary, _employee);
            var hourlyRate = PayrollTools.GetHourlyRateByDailyRate(dailyRate);

            timeEntry.BasicDayPay = timeEntry.BasicHours * hourlyRate;
            timeEntry.RegularPay = timeEntry.RegularHours * hourlyRate;

            timeEntry.LateDeduction = timeEntry.LateHours * hourlyRate;
            timeEntry.UndertimeDeduction = timeEntry.UndertimeHours * hourlyRate;
            timeEntry.AbsentDeduction = timeEntry.AbsentHours * hourlyRate;

            timeEntry.OvertimePay = timeEntry.OvertimeHours * hourlyRate * payrate.OvertimeRate;

            var restDayRate = payrate.RestDayRate;
            if (_policy.RestDayInclusive && _employee.IsPremiumInclusive)
                restDayRate -= 1;
            timeEntry.RestDayPay = timeEntry.RestDayHours * hourlyRate * restDayRate;

            timeEntry.RestDayOTPay = timeEntry.RestDayOTHours * hourlyRate * payrate.RestDayOTRate;
            timeEntry.LeavePay = timeEntry.TotalLeaveHours * hourlyRate;

            if (currentShift.IsWorkingDay)
            {
                var nightDiffRate = payrate.NightDiffRate - payrate.RegularRate;
                var nightDiffOTRate = payrate.NightDiffOTRate - payrate.OvertimeRate;

                var notEntitledForLegalHolidayRate = _employmentPolicy.ComputeRegularHoliday == false && payrate.IsSpecialNonWorkingHoliday;
                var notEntitledForSpecialNonWorkingHolidayRate = _employmentPolicy.ComputeSpecialHoliday == false && payrate.IsSpecialNonWorkingHoliday;

                if (notEntitledForLegalHolidayRate || notEntitledForSpecialNonWorkingHolidayRate)
                {
                    nightDiffRate = (payrate.NightDiffRate / payrate.RegularRate) % 1;
                    nightDiffOTRate = (payrate.NightDiffOTRate / payrate.OvertimeRate) % 1;
                }

                timeEntry.NightDiffPay = timeEntry.NightDiffHours * hourlyRate * nightDiffRate;
                timeEntry.NightDiffOTPay = timeEntry.NightDiffOTHours * hourlyRate * nightDiffOTRate;
            }
            else if (currentShift.IsRestDay)
            {
                var restDayNDRate = payrate.RestDayNDRate - payrate.RestDayRate;
                var restDayNDOTRate = payrate.RestDayNDOTRate - payrate.RestDayOTRate;

                timeEntry.NightDiffPay = timeEntry.NightDiffHours * hourlyRate * restDayNDRate;
                timeEntry.NightDiffOTPay = timeEntry.NightDiffOTHours * hourlyRate * restDayNDOTRate;
            }

            if (payrate.IsHoliday)
            {
                var holidayRate = 0M;
                var holidayOTRate = 0M;

                if (currentShift.IsWorkingDay)
                {
                    holidayRate = payrate.RegularRate;
                    holidayOTRate = payrate.OvertimeRate;
                }
                else if (currentShift.IsRestDay)
                {
                    holidayRate = payrate.RestDayRate;
                    holidayOTRate = payrate.RestDayOTRate;
                }

                timeEntry.SpecialHolidayOTPay = timeEntry.SpecialHolidayOTHours * hourlyRate * holidayOTRate;
                timeEntry.RegularHolidayOTPay = timeEntry.RegularHolidayOTHours * hourlyRate * holidayOTRate;

                var isHolidayPayInclusive = _employee.IsPremiumInclusive;

                if (_employmentPolicy.ComputeSpecialHoliday && payrate.IsSpecialNonWorkingHoliday)
                {
                    holidayRate = isHolidayPayInclusive ? holidayRate - 1 : holidayRate;

                    timeEntry.SpecialHolidayPay = timeEntry.SpecialHolidayHours * hourlyRate * holidayRate;
                    timeEntry.LeavePay = timeEntry.TotalLeaveHours * hourlyRate * holidayRate;
                }
                else if (_employmentPolicy.ComputeRegularHoliday && payrate.IsRegularHoliday)
                {
                    timeEntry.RegularHolidayPay = timeEntry.RegularHolidayHours * hourlyRate * (holidayRate - 1);

                    var holidayCalculationType = _settings.GetStringOrDefault("Payroll Policy.HolidayPay", "Daily");

                    var basicHolidayPay = 0M;
                    if (holidayCalculationType == "Hourly")
                        basicHolidayPay = currentShift.WorkingHours * hourlyRate;
                    else if (holidayCalculationType == "Daily")
                        basicHolidayPay = dailyRate;

                    var isPaidToday = timeEntry.GetTotalDayPay() > 0;

                    var isEntitledToHolidayPay = (isHolidayPayInclusive == false) &&
                                                    (hasWorkedLastDay ||
                                                    isPaidToday ||
                                                    _policy.RequiredToWorkLastDayForHolidayPay == false);

                    if (isEntitledToHolidayPay)
                    {
                        // If it's a `Double Holiday', then the employee is entitled to twice their holiday pay.
                        if (payrate.IsDoubleHoliday)
                            timeEntry.BasicRegularHolidayPay = basicHolidayPay * 2;
                        else
                            timeEntry.BasicRegularHolidayPay = basicHolidayPay;
                    }
                }
            }

            timeEntry.ComputeTotalHours();
            timeEntry.ComputeTotalPay();
        }

        #region Public static methods

        public static CurrentShift GetCurrentShift(DateTime currentDate,
                                            ShiftSchedule employeeShift,
                                            EmployeeDutySchedule shiftSched,
                                            bool useShiftSchedule,
                                            bool respectDefaultRestDay,
                                            int? employeeDayOfRest)
        {
            var currentShift = useShiftSchedule ? new CurrentShift(shiftSched, currentDate) :
                                                new CurrentShift(employeeShift, currentDate);

            if (respectDefaultRestDay)
            {
                currentShift.SetDefaultRestDay(employeeDayOfRest);
            }

            return currentShift;
        }

        public static decimal ComputeLeaveHoursWithoutTimelog(CurrentShift currentShift,
                                                            Leave leave,
                                                            bool computeBreakTimeLate)
        {
            var leaveHours = 0M;
            if (currentShift.HasShift == false && (leave.StartTime == null || leave.EndTime == null))
            {
                leaveHours = 8;
            }
            else
            {
                var leavePeriod = GetLeavePeriod(leave, currentShift);
                leaveHours = TimeEntryCalculator.ComputeLeaveHours(leavePeriod, currentShift, computeBreakTimeLate);
            }

            leaveHours = AccuMath.CommercialRound(leaveHours);
            return leaveHours;
        }

        private static TimePeriod GetLeavePeriod(Leave leave, CurrentShift currentShift)
        {
            var startTime = leave.StartTime ?? currentShift.StartTime.Value;
            var endTime = leave.EndTime ?? currentShift.EndTime.Value;

            var leavePeriod = TimePeriod.FromTime(startTime, endTime, currentShift.Date);

            if (!currentShift.HasShift)
                return leavePeriod;

            if (!currentShift.ShiftPeriod.Intersects(leavePeriod))
            {
                var nextDay = currentShift.Date.AddDays(1);
                leavePeriod = TimePeriod.FromTime(startTime, endTime, nextDay);
            }

            return leavePeriod;
        }

        #endregion Public static methods
    }
}