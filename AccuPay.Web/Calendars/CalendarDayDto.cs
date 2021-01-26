using System;

namespace AccuPay.Web.Calendars
{
    public class CalendarDayDto
    {
        public int? Id { get; set; }

        public DateTime Date { get; set; }

        public string DayType { get; set; }

        public string Description { get; set; }
    }
}
