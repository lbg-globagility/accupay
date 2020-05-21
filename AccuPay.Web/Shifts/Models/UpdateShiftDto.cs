using System;

namespace AccuPay.Web.Shifts.Models
{
    public class UpdateShiftDto : ICrudShiftDto
    {
        public DateTime DateSched { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public TimeSpan? BreakStartTime { get; set; }

        public decimal BreakLength { get; set; }

        public bool IsRestDay { get; set; }

        public decimal ShiftHours { get; set; }

        public decimal WorkHours { get; set; }
    }
}
