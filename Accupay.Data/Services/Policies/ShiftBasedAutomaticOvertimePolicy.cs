using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.ValueObjects;
using System;

namespace AccuPay.Data.Services.Policies
{
    public class ShiftBasedAutomaticOvertimePolicy
    {
        private const decimal STANDARD_LABOR_HOURS = CurrentShift.StandardWorkingHours;

        private const int MINUTES_PER_HOUR = TimeConstants.MinutesPerHour;

        private const string POLICY_TYPE = "DutyShift";
        private readonly ListOfValueCollection _settings;

        public ShiftBasedAutomaticOvertimePolicy(ListOfValueCollection settings) => _settings = settings;

        public bool Enabled => _settings.GetBoolean($"{POLICY_TYPE}.ShiftBasedAutomaticOvertime");

        public decimal Denominator => _settings.GetDecimal($"{POLICY_TYPE}.DivisibleBy");

        public decimal MinimumMinutes => _settings.GetDecimal($"{POLICY_TYPE}.MinimumDuration", 30);

        public decimal MinimumHours => ConvertMinuteToHour(MinimumMinutes);

        public decimal DefaultWorkHours => DefaultShiftHours - DefaultBreakLength;

        public decimal DefaultShiftHours => _settings.GetDecimal($"{POLICY_TYPE}.DefaultShiftHour", STANDARD_LABOR_HOURS);

        public decimal DefaultBreakLength => _settings.GetDecimal($"{POLICY_TYPE}.BreakHour");

        public decimal DefaultWorkHoursAndMinimumOTHours => DefaultWorkHours + MinimumHours;

        public bool IsValidDefaultShiftPeriod(DateTime? startDate, TimeSpan? endTime, decimal breakLength)
        {
            if (startDate.HasValue && endTime.HasValue)
            {
                var shiftPeriod = new TimePeriod(
                    startDate.Value,
                    startDate.Value.Date.Add(endTime.Value));

                if (!(shiftPeriod.TotalHours <= DefaultWorkHours))
                {
                    var expectedEndTime = GetExpectedEndTime(startDate, breakLength).Value;
                    var minimumOTEndTime = expectedEndTime.AddMinutes(Convert.ToDouble(MinimumMinutes));

                    if (shiftPeriod.End > expectedEndTime && shiftPeriod.End <= minimumOTEndTime)
                        return false;
                }

                return true;
            }

            return false;
        }

        public DateTime? GetExpectedEndTime(DateTime? shiftStart, decimal breakLength)
        {
            if (shiftStart == null) return null;

            var userLaborHours = DefaultWorkHours + breakLength;
            var expectedEndTimeSpan = shiftStart.Value.AddHours(Convert.ToDouble(userLaborHours));

            return expectedEndTimeSpan;
        }

        public DateTime? GetExpectedEndTime(Shift shift)
        {
            if (shift.WorkHours < DefaultWorkHours)
            {
                return shift.EndTimeFull;
            }

            return GetExpectedEndTime(shift.StartTimeFull, shift.BreakLength);
        }

        private decimal ConvertHourToMinute(decimal valueHours) => MINUTES_PER_HOUR * valueHours;

        private decimal ConvertMinuteToHour(decimal valueMinutes) => valueMinutes / MINUTES_PER_HOUR;

        public decimal TrimOvertimeHoursWorked(decimal overtimeHoursWorked)
        {
            if (Denominator > 1)
            {
                var remainder = ConvertHourToMinute(overtimeHoursWorked) % Denominator;
                return overtimeHoursWorked - ConvertMinuteToHour(remainder);
            }
            return overtimeHoursWorked;
        }
    }
}
