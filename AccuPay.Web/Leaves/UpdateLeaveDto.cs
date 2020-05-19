using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Leaves
{
    public class UpdateLeaveDto : ICrudLeaveDto
    {
        [Required]
        public string LeaveType { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string Reason { get; set; }

        public string Comments { get; set; }
    }
}
