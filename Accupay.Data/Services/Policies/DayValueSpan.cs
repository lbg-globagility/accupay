using System;

namespace AccuPay.Data.Services.Policies
{
    public class DayValueSpan
    {
        private static readonly DayValue DefaultFirstHalfStartDay = DayValue.Create(1);
        private static readonly DayValue DefaultFirstHalfEndDay = DayValue.Create(15);
        private static readonly DayValue DefaultEndOfTheMonthStartDay = DayValue.Create(16);
        private static readonly DayValue DefaultEndOfTheMonthEndDay = DayValue.Create(30, isLastDayOfTheMonth: true);

        public DayValue From { get; set; }
        public DayValue To { get; set; }

        private DayValueSpan(DayValue from, DayValue to)
        {
            From = from;
            To = to;
        }

        public static DayValueSpan Create(DayValue from, DayValue to)
        {
            return new DayValueSpan(from, to);
        }

        public static DayValueSpan DefaultFirstHalf => new DayValueSpan(DefaultFirstHalfStartDay, DefaultFirstHalfEndDay);

        public static DayValueSpan DefaultEndOfTheMonth => new DayValueSpan(DefaultEndOfTheMonthStartDay, DefaultEndOfTheMonthEndDay);

        public bool Contains(DateTime date, int month, int year)
        {
            DateTime fromDate = From.GetDate(month: month, year: year);
            DateTime toDate = To.GetDate(month: month, year: year);

            return date >= fromDate && date <= toDate;
        }
    }
}