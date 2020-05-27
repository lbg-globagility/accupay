using System;

namespace AccuPay.Web.Shifts.Models
{
    public class UpdateShiftDto : ICrudShiftDto
    {
        public DateTime DateSched { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? BreakStartTime { get; set; }

        public decimal BreakLength { get; set; }

        public bool IsRestDay { get; set; }
    }
}
