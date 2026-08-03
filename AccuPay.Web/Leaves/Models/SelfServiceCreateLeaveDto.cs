using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Leaves
{
    public class SelfServiceCreateLeaveDto
    {
        public const string TimingDay = "Day";

        public const string TimingHour = "Hour";
        [Required]
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        [Required]
        public List<DateTime> DateTimes { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string LeaveTiming { get; set; }
        public string Reason { get; set; }
    }
}
