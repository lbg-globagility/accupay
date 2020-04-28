using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities;
using System;

namespace AccuPay.Data.Services
{
    public class DayCalculator
    {
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
    }
}