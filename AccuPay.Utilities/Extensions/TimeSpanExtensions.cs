using System;

namespace AccuPay.Utilities.Extensions
{
    public static class TimeSpanExtensions
    {
        private const int MINUTES_PER_HOUR = 60;

        public static string ToStringFormat(
                                this TimeSpan timeSpanInput,
                                string format,
                                DateTime? currentDate = null)
        {
            currentDate = currentDate == null ? DateTime.Now.ToMinimumHourValue() :
                                                currentDate.Value.ToMinimumHourValue();

            return currentDate.Value.Add(timeSpanInput).ToString(format);
        }

        public static string ToStringFormat(
                                this TimeSpan? timeSpanInput,
                                string format,
                                DateTime? currentDate = null)
        {
            currentDate = currentDate == null ? DateTime.Now.ToMinimumHourValue() :
                                                currentDate.Value.ToMinimumHourValue();

            if (!timeSpanInput.HasValue) return string.Empty;
            return currentDate.Value.Add(timeSpanInput.Value).ToString(format);
        }

        public static TimeSpan AddHours(this TimeSpan timeSpanInput, int value)
        {
            return timeSpanInput.Add(new TimeSpan(0, value * MINUTES_PER_HOUR, 0));
        }
    }
}