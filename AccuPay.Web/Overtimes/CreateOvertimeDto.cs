using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Overtimes
{
    public class CreateOvertimeDto : ICrudOvertimeDto
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public string Reason { get; set; }

        public string Comments { get; set; }
    }
}
