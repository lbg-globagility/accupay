using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Leaves
{
    public class SelfServiceCreateLeaveDto
    {
        [Required]
        public string LeaveType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime EndDate { get; set; }

        public string Reason { get; set; }
    }
}
