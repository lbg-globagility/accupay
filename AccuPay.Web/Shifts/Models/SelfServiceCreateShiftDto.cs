using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Shifts.Models
{
    public class SelfServiceCreateShiftDto
    {
        [Required]
        public string EmployeeNumber { get; set; }

        [Required]
        public DateTime DateFrom { get; set; }

        [Required]
        public DateTime DateTo { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public DateTime? BreakStartTime { get; set; }

        public decimal BreakLength { get; set; }

        public bool IsRestDay { get; set; }

        public bool RequiresLunchInOut { get; set; }
    }
}
