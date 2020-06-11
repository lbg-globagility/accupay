using System;

namespace AccuPay.Web.TimeEntries.Models
{
    public class DatePeriodDto
    {
        public DateTime? Start { get; }
        public DateTime? End { get; }

        public DatePeriodDto(DateTime? start, DateTime? end)
        {
            if (start != null || end != null)
            {
                Start = start;
                End = end;
            }
        }
    }
}
