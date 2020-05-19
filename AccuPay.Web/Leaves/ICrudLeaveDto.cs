using System;

namespace AccuPay.Web.Leaves
{
    public interface ICrudLeaveDto
    {
        string Comments { get; set; }

        DateTime? EndTime { get; set; }

        string LeaveType { get; set; }

        string Reason { get; set; }

        DateTime StartDate { get; set; }

        DateTime? StartTime { get; set; }

        string Status { get; set; }
    }
}
