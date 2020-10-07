using AccuPay.Data.Services.Policies;
using System;

namespace AccuPay.Data.Helpers
{
    internal class PayPeriodHelper
    {
        public static (DayValueSpan currentDaySpan, int month, int year) GetCutOffDayValueSpan(
            DateTime date,
            DayValueSpan firstHalf,
            DayValueSpan endOfTheMonth)
        {
            DayValueSpan currentDaySpan = DayValueSpan.DefaultFirstHalf;

            int month = date.Month;
            int year = date.Year;

            if (firstHalf.Contains(date, month: month, year: year))
            {
                currentDaySpan = firstHalf;
            }
            else if (endOfTheMonth.Contains(date, month: month, year: year))
            {
                currentDaySpan = endOfTheMonth;
            }
            else
            {
                // this means the date is not within the initially computed default first half or end of the month
                // mostly likely because the first half starts on previous month like in goldwings
                var adjustedDate = date.AddMonths(1);
                month = adjustedDate.Month;
                year = adjustedDate.Year;

                if (firstHalf.Contains(date, month: month, year: year))
                {
                    currentDaySpan = firstHalf;
                }
                else if (endOfTheMonth.Contains(date, month: month, year: year))
                {
                    currentDaySpan = endOfTheMonth;
                }
            }

            return (currentDaySpan, month: month, year: year);
        }
    }
}