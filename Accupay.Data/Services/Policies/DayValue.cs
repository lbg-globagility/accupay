using System;

namespace AccuPay.Data.Services.Policies
{
    public class DayValue
    {
        public bool IsLastDayOfTheMonth { get; set; }
        public int Value { get; set; }

        private DayValue(int value, bool isLastDayOfTheMonth = false)
        {
            this.IsLastDayOfTheMonth = isLastDayOfTheMonth;
            this.Value = value;
        }

        public static DayValue Create(int value, bool isLastDayOfTheMonth = false)
        {
            return new DayValue(value, isLastDayOfTheMonth);
        }

        public DateTime GetDate(int month, int year)
        {
            int day = IsLastDayOfTheMonth ? DateTime.DaysInMonth(year, month) : Value;

            return new DateTime(year, month, day);
        }
    }
}