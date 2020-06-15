using System;

namespace AccuPay.Web.TimeEntries.Models
{
    public class DatePeriodDto
    {
        public DateTime? Start { get; }
        public DateTime? End { get; }
        public bool IsWholeDay { get; }

        private DatePeriodDto(DateTime? start, DateTime? end, bool isWholeDay)
        {
            IsWholeDay = isWholeDay;
            Start = start;
            End = end;
        }

        public static DatePeriodDto Create(DateTime? start, DateTime? end, bool isWholeDay = false)
        {
            if (start == null && end == null && isWholeDay == false)
            {
                return null;
            }

            return new DatePeriodDto(start, end, isWholeDay);
        }
    }
}
