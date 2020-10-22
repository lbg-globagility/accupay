using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Data.Services.Policies
{
    public class ShiftBasedAutomaticOvertimePolicy
    {
        private const int STANDARD_LABOR_HOURS = 8;
        private const int STANDARD_LABOR_HOURS_WOUT_BREAK = 9;

        private const int MINUTES_PER_HOUR = 60;
        private const int SECONDS_PER_HOUR = 3600;

        private const string POLICY_TYPE = "DutyShift";
        private readonly ListOfValueCollection _settings;

        public ShiftBasedAutomaticOvertimePolicy(ListOfValueCollection settings) => _settings = settings;

        public bool Enabled => _settings.GetBoolean($"{POLICY_TYPE}.ShiftBasedAutomaticOvertime");

        public decimal Denominator => _settings.GetDecimal($"{POLICY_TYPE}.DivisibleBy");

        public decimal Minimum => _settings.GetDecimal($"{POLICY_TYPE}.MinimumDuration");

        public decimal DefaultWorkHours => _settings.GetDecimal($"{POLICY_TYPE}.DefaultWorkHours", STANDARD_LABOR_HOURS);

        public decimal DefaultShiftHours => _settings.GetDecimal($"{POLICY_TYPE}.DefaultShiftHours", STANDARD_LABOR_HOURS_WOUT_BREAK);

        public decimal DefaultBreakLength => DefaultShiftHours - DefaultWorkHours;

        public bool Validate(DateTime? startTimeSpan, DateTime? endTimeSpan)
        {
            if (startTimeSpan.HasValue && endTimeSpan.HasValue)
            {
                var expectedEndTime = GetDefaultEndTime(startTimeSpan).Value;
                var minimumOTEndTime = expectedEndTime.AddMinutes(Convert.ToDouble(Minimum));

                var endTimeSpanValue = endTimeSpan.Value;
                if (startTimeSpan.Value.Hour > endTimeSpanValue.Hour) endTimeSpanValue = endTimeSpanValue.AddDays(1);
                var eightHoursExact = endTimeSpanValue.Subtract(expectedEndTime).TotalSeconds == 0;
                var isMinimumOTTimeOnwards = endTimeSpanValue.Subtract(minimumOTEndTime).TotalSeconds >= 0;

                if (!isMinimumOTTimeOnwards)
                    if (!eightHoursExact)
                        return false;

                return true;
            }
            return false;
        }

        public DateTime? GetDefaultEndTime(DateTime? startTimeSpan)
        {
            if (startTimeSpan == null) return default(DateTime?);

            var standardLaborSecondsWoutBreak = STANDARD_LABOR_HOURS_WOUT_BREAK * SECONDS_PER_HOUR;
            var expectedEndTimeSpan = startTimeSpan.Value.AddSeconds(standardLaborSecondsWoutBreak);

            return expectedEndTimeSpan;
        }

        private decimal ConvertHourToMinute(decimal valueHours) => MINUTES_PER_HOUR * valueHours;

        public decimal TrimOvertimeHoursWroked(decimal overtimeHoursWorked)
        {
            if (Denominator > 1)
            {
                var remainder = ConvertHourToMinute(overtimeHoursWorked) % Denominator;
                return overtimeHoursWorked - remainder;
            }
            return overtimeHoursWorked;
        }
    }
}