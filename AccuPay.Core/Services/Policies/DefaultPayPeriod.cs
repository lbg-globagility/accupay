using AccuPay.Core.ValueObjects;
using System;

namespace AccuPay.Core.Services.Policies
{
    public class DefaultPayPeriod
    {
        public int StartDay { get; set; }
        public int EndDay { get; set; }
        public bool StartDayIsLastDayOfTheMonth { get; set; }
        public bool EndDayIsLastDayOfTheMonth { get; set; }
        public bool StartDayIsPreviousMonth { get; set; }
        public bool EndDayIsPreviousMonth { get; set; }

        public DefaultPayPeriod(TimePeriod period)
        {
            StartDay = period.Start.Day;
            EndDay = period.End.Day;
            StartDayIsLastDayOfTheMonth = period.Start.Day == DateTime.DaysInMonth(period.Start.Year, period.Start.Month);
            EndDayIsLastDayOfTheMonth = period.End.Day == DateTime.DaysInMonth(period.End.Year, period.End.Month); ;
            StartDayIsPreviousMonth = period.Start.Month == 12;
            EndDayIsPreviousMonth = period.End.Month == 12;
        }

        public override string ToString()
        {
            return $"{StartDay},{EndDay},{StartDayIsLastDayOfTheMonth},{EndDayIsLastDayOfTheMonth},{StartDayIsPreviousMonth},{EndDayIsPreviousMonth}";
        }
    }
}