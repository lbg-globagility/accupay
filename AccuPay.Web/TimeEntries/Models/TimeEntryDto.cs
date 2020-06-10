using System;

namespace AccuPay.Web.TimeEntries.Models
{
    public class TimeEntryDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal WorkHours { get; set; }

        public decimal OvertimeHours { get; set; }

        public decimal NightDiffHours { get; set; }

        public decimal NightDiffOTHours { get; set; }

        public decimal LateHours { get; set; }

        public decimal UndertimeHours { get; set; }

        public decimal AbsentHours { get; set; }
    }
}
