using AccuPay.Data.Interfaces;
using AccuPay.Data.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Data.Services.Policies
{
    public class ShiftBasedAutomaticOvertimePolicy
    {
        private const int STANDARD_LABOR_HOURS = 8;

        private const int MINUTES_PER_HOUR = 60;

        private const int NO_DEFAULT_BREAK = 0;

        private const string POLICY_TYPE = "DutyShift";
        private readonly ListOfValueCollection _settings;

        public ShiftBasedAutomaticOvertimePolicy(ListOfValueCollection settings) => _settings = settings;

        public bool Enabled => _settings.GetBoolean($"{POLICY_TYPE}.ShiftBasedAutomaticOvertime");

        public decimal Denominator => _settings.GetDecimal($"{POLICY_TYPE}.DivisibleBy");

        public decimal Minimum => _settings.GetDecimal($"{POLICY_TYPE}.MinimumDuration");

        public decimal DefaultWorkHours => _settings.GetDecimal($"{POLICY_TYPE}.DefaultWorkHours", STANDARD_LABOR_HOURS);

        public decimal DefaultBreakLength => NO_DEFAULT_BREAK;

        public bool IsValidDefaultShiftPeriod(DateTime? startDate, TimeSpan? endTime, decimal breakLength)
        {
            if (startDate.HasValue && endTime.HasValue)
            {
                var expectedEndTime = GetExpectedEndTime(startDate, breakLength).Value;
                var minimumOTEndTime = expectedEndTime.AddMinutes(Convert.ToDouble(Minimum));

                var endDate = startDate.Value.Date.Add(endTime.Value);
                if (startDate.Value.Hour >= endDate.Hour) endDate = endDate.AddDays(1);

                var atLeastDefaultWorkHoursOrMore = endDate.Subtract(expectedEndTime).TotalSeconds == 0;
                var isMinimumOTTimeOnwards = endDate.Subtract(minimumOTEndTime).TotalSeconds >= 0;

                if (!isMinimumOTTimeOnwards)
                    if (!atLeastDefaultWorkHoursOrMore)
                        return false;

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

        private decimal ConvertHourToMinute(decimal valueHours) => MINUTES_PER_HOUR * valueHours;

        private decimal ConvertMinuteToHour(decimal valueHours) => valueHours / MINUTES_PER_HOUR;

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