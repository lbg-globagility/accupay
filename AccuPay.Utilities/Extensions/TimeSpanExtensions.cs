using System;

namespace AccuPay.Utilities.Extensions
{
    public static class TimeSpanExtensions
    {
        public const int HoursPerDay = 24;
        private const int MinutesPerHour = 60;
        private const int SecondsPerMinutes = 60;

        public static string ToStringFormat(
            this TimeSpan timeSpanInput,
            string format,
            DateTime? currentDate = null)
        {
            currentDate = currentDate == null ?
                DateTime.Now.ToMinimumHourValue() :
                currentDate.Value.ToMinimumHourValue();

            return currentDate.Value.Add(timeSpanInput).ToString(format);
        }

        public static string ToStringFormat(
                this TimeSpan? timeSpanInput,
                string format,
                DateTime? currentDate = null)
        {
            if (!timeSpanInput.HasValue) return string.Empty;

            currentDate = currentDate == null ?
                DateTime.Now.ToMinimumHourValue() :
                currentDate.Value.ToMinimumHourValue();

            return currentDate.Value.Add(timeSpanInput.Value).ToString(format);
        }

        public static TimeSpan AddHours(this TimeSpan timeSpanInput, decimal value)
        {
            return timeSpanInput.Add(
                new TimeSpan(0, 0, (int)(value * MinutesPerHour * SecondsPerMinutes)));
        }

        public static TimeSpan AddOneDay(this TimeSpan timeSpanInput)
        {
            return timeSpanInput.Add(new TimeSpan(HoursPerDay, 0, 0));
        }

        public static TimeSpan StripSeconds(this TimeSpan timeSpanInput)
        {
            return new TimeSpan(timeSpanInput.Hours, timeSpanInput.Minutes, 0);
        }

        public static TimeSpan? StripSeconds(this TimeSpan? timeSpanInput)
        {
            if (timeSpanInput == null) return null;

            return new TimeSpan(timeSpanInput.Value.Hours, timeSpanInput.Value.Minutes, 0);
        }
    }
}
