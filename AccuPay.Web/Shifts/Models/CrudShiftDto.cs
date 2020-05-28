using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Shifts.Models
{
    public abstract class CrudShiftDto
    {
        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public DateTime BreakStartTime { get; set; }

        [Required]
        public decimal BreakLength { get; set; }

        [Required]
        public bool isOffset { get; set; }
    }
}
