using System;
using System.Collections.Generic;

namespace AccuPay.Data.Helpers
{
    public class CalendarHelper
    {
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            var day = from.Date;
            while (day.Date <= thru.Date)
            {
                yield return day;
                day = day.AddDays(1);
            }
        }
    }
}