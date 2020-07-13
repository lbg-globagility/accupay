using System;

namespace AccuPay.Data.Services.Policies
{
    public class DaysSpan
    {
        private static readonly DayValue DefaultFirstHalfStartDay = DayValue.Create(1);
        private static readonly DayValue DefaultFirstHalfEndDay = DayValue.Create(15);
        private static readonly DayValue DefaultEndOfTheMonthStartDay = DayValue.Create(16);
        private static readonly DayValue DefaultEndOfTheMonthEndDay = DayValue.Create(30, isLastDayOfTheMonth: true);

        public DayValue From { get; set; }
        public DayValue To { get; set; }

        private DaysSpan(DayValue from, DayValue to)
        {
            From = from;
            To = to;
        }

        public static DaysSpan Create(DayValue from, DayValue to)
        {
            return new DaysSpan(from, to);
        }

        public static DaysSpan DefaultFirstHalf => new DaysSpan(DefaultFirstHalfStartDay, DefaultFirstHalfEndDay);

        public static DaysSpan DefaultEndOfTheMonth => new DaysSpan(DefaultEndOfTheMonthStartDay, DefaultEndOfTheMonthEndDay);

        public bool IsBetween(DateTime date)
        {
            int month = date.Month;
            int year = date.Year;

            DateTime fromDate = From.GetDate(month: month, year: year);
            DateTime toDate = To.GetDate(month: month, year: year);

            return date >= fromDate && date <= toDate;
        }
    }
}