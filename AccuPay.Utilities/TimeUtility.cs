using System;

namespace AccuPay.Utilities
{
    public static class TimeUtility
    {
        public static DateTime RangeStart(DateTime dateToday, TimeSpan startTime)
        {
            return Combine(dateToday, startTime);
        }

        public static DateTime RangeEnd(DateTime dateToday, TimeSpan startTime, TimeSpan endTime)
        {
            var dateTomorrow = dateToday.AddDays(1);
            return Combine(
                endTime > startTime ? dateToday : dateTomorrow,
                endTime
            );
        }

        public static DateTime Combine(DateTime day, TimeSpan time)
        {
            var timestampString = $"{day.ToString("yyyy-MM-dd")} {time.ToString(@"hh\:mm\:ss")}";

            return DateTime.Parse(timestampString);
        }

        public static DateTime? ToDateTime(TimeSpan? timeSpan, DateTime date)
        {
            if (!timeSpan.HasValue) return null;

            var dateTime = new DateTime(
                date.Year, date.Month, date.Day,
                timeSpan.Value.Hours, timeSpan.Value.Minutes, 0);

            return dateTime;
        }
    }
}