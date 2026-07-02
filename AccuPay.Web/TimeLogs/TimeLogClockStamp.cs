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
        {
            var val = ClockStamp ?? DateTime.UtcNow;

            return TimeAttendanceLog.NewTimeAttendanceLog(userId: userId,
                organizationId: organizationId,
                timeStamp: val,
                workDay: val.Date,
                employeeID: employeeId,
                isTimeIn: StampTag);
        }
    }
}
