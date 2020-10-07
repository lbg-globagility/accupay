using System;

namespace AccuPay.Data.Services.Policies
{
    public class DayValue
    {
        public bool DayIsLastDayOfTheMonth { get; set; }
        public bool MonthIsLastMonth { get; set; }
        public int Value { get; set; }

        private DayValue(int value, bool isLastDayOfTheMonth = false, bool monthIsLastMonth = false)
        {
            this.Value = value;
            this.DayIsLastDayOfTheMonth = isLastDayOfTheMonth;
            this.MonthIsLastMonth = monthIsLastMonth;
        }

        public static DayValue Create(int value, bool isLastDayOfTheMonth = false, bool monthIsLastMonth = false)
        {
            return new DayValue(
                value,
                isLastDayOfTheMonth: isLastDayOfTheMonth,
                monthIsLastMonth: monthIsLastMonth);
        }

        public DateTime GetDate(int month, int year)
        {
            var date = new DateTime(year, month, 1);

            if (MonthIsLastMonth)
            {
                date = date.AddMonths(-1);
                month = date.Month;
                year = date.Year;
            }

            int day = DayIsLastDayOfTheMonth ? DateTime.DaysInMonth(year, month) : Value;

            return new DateTime(year, month, day);
        }
    }
}