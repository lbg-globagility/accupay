using System;

namespace AccuPay.Utilities.Extensions
{
    public static class DateExtensions
    {
        public static string ToStringFormatOrNull(this DateTime? input, string format)
        {
            if (input == null) return null;
            return input.Value.ToString(format);
        }

        public static DateTime ToMinimumDateValue(this DateTime input)
        {
            return new DateTime(input.Year, input.Month, 1, 0, 0, 0);
        }

        public static DateTime ToMinimumHourValue(this DateTime input)
        {
            return new DateTime(input.Year, input.Month, input.Day, 0, 0, 0);
        }

        public static DateTime ToMaximumHourValue(this DateTime input)
        {
            return new DateTime(input.Year, input.Month, input.Day, 23, 59, 59);
        }

        public static DateTime? ToMinimumHourValue(this DateTime? input)
        {
            if (input == null) return null;

            return new DateTime(input.Value.Year, input.Value.Month, input.Value.Day, 0, 0, 0);
        }

        public static DateTime? ToMaximumHourValue(this DateTime? input)
        {
            if (input == null) return null;

            return new DateTime(input.Value.Year, input.Value.Month, input.Value.Day, 23, 59, 59);
        }

        public static DateTime StripSeconds(this DateTime input)
        {
            return new DateTime(input.Year, input.Month, input.Day, input.Hour, input.Minute, 0);
        }
    }
}