using AccuPay.Core.Entities;
using System;

namespace AccuPay.Web.TimeLogs
{
    public class TimeLogClockStamp
    {
        public int EmployeeId { get; set; }
        public string EmployeeNo { get; set; }
        public DateTime? ClockStamp { get; set; } = null;
        public bool StampTag { get; set; }

        public TimeAttendanceLog ToTimeAttendanceLog(int userId, int organizationId, int employeeId)
            => TimeAttendanceLog.NewTimeAttendanceLog(userId: userId,
                organizationId: organizationId,
                timeStamp: ClockStamp ?? DateTime.UtcNow,
                workDay: (ClockStamp ?? DateTime.UtcNow).Date,
                employeeID: employeeId,
                isTimeIn: StampTag);
    }
}
