using System;
using System.Collections.Generic;

namespace AccuPay.Web.Leaves
{
    public class LeaveDto
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public string EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeType { get; set; }

        public string LeaveType { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; }

        public string Reason { get; set; }

        public string Comments { get; set; }

        public string ApproverEmail { get; set; }

        public DateTime Created { get; set; }

        public DateTime? LastUpd { get; set; }

        public int? CreatedBy { get; set; }

        public int? LastUpdBy { get; set; }

        public bool IsNotifyEmail { get; set; }

        public DateTime? NotifyEmailSentAt { get; set; }

        public DateTime? FilingGroupDate { get; set; }

        public List<DateTime>? DateTimes { get; set; }
    }
}
