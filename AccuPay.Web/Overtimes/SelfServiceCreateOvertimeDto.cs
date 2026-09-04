using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Overtimes
{
    public class SelfServiceCreateOvertimeDto
    {
        [Required]
        public string EmployeeNumber { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }
    }
}
