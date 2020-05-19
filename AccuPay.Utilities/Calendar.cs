using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AccuPay.Utilities
{
    public class Calendar
    {
        private static int HoursInAClock = 12;

        private static Regex TimeRegex = new Regex(@"^(2[0-3]|1[0-9]|0?[0-9])(?:\s|\.|\:)*(5[0-9]|4[0-9]|3[0-9]|2[0-9]|1[0-9]|0[0-9]|[0-5])?\s*(a|am|p|pm)?$", RegexOptions.IgnoreCase);

        public static DateTime ToDate(string text)
        {
            return default(DateTime);
        }

        public static TimeSpan? ToTimespan(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return default(TimeSpan?);

            var results = TimeRegex.Match(text);
            var groups = results.Groups;

            if (groups.Count < 2)
                return default(TimeSpan?);

            int hour = groups.Count > 1 ? System.Convert.ToInt32(groups[1].Value) : 0;
            int minute = groups.Count > 2 ? ParseMinute(groups[2].Value) : 0;
            string clock = groups.Count > 3 ? groups[3].Value : string.Empty;

            if (string.IsNullOrEmpty(clock))
                return new TimeSpan(hour, minute, 0);

            if (IsAm(clock))
            {
                if (hour > HoursInAClock)
                    return default(TimeSpan?);

                hour = hour == HoursInAClock ? 0 : hour;

                return new TimeSpan(hour, minute, 0);
            }
            else if (IsPm(clock))
            {
                hour = hour == HoursInAClock ? hour : hour + HoursInAClock;

                return new TimeSpan(hour, minute, 0);
            }
            else
                return default(TimeSpan?);
        }

        private static int ParseMinute(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            if (value.Length == 1)
                return System.Convert.ToInt32($"{value}0");
            else
                return System.Convert.ToInt32(value);
        }

        private static bool IsAm(string clock)
        {
            return string.Equals(clock, "am", StringComparison.OrdinalIgnoreCase) | string.Equals(clock, "a", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsPm(string clock)
        {
            return string.Equals(clock, "pm", StringComparison.OrdinalIgnoreCase) | string.Equals(clock, "p", StringComparison.OrdinalIgnoreCase);
        }

        public static DateTime Create(DateTime datePortion, TimeSpan? timePortion)
        {
            if (timePortion == null)
                return datePortion;

            return datePortion.Add(timePortion.Value);
        }

        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            var day = from.Date;
            while (day.Date <= thru.Date)
            {
                yield return day;
                day = day.AddDays(1);
            }
        }

        public static int DaysBetween(DateTime a, DateTime b)
        {
            return (b - a).Duration().Days + 1;
        }

        public static int MonthsBetween(DateTime a, DateTime b)
        {
            var start = new[] { a, b }.Min();
            var end = new[] { a, b }.Max();

            var months = ((end.Year * 12) + end.Month) - ((start.Year * 12) + start.Month);

            return months;
        }
    }
}
