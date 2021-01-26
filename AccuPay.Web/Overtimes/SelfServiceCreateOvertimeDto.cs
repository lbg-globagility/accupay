using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Overtimes
{
    public class SelfServiceCreateOvertimeDto
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public string Reason { get; set; }
    }
}
