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

        public static DateTime? ToDateTime(TimeSpan? timeSpan)
        {
            if (!timeSpan.HasValue) return null;

            var today = DateTime.Now;
            var value = timeSpan.GetValueOrDefault();

            var dateTime = new DateTime(today.Year, today.Month, today.Day,
                                        value.Hours, value.Minutes, 0);

            return dateTime;
        }
    }
}