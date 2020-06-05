using System;

namespace AccuPay.Web.TimeEntries.Models
{
    public class HolidayDto
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string[] branches { get; set; }
    }
}
