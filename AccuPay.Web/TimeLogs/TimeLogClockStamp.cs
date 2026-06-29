using AccuPay.Core.Entities;
using System;

namespace AccuPay.Web.TimeLogs
{
    public class TimeLogClockStamp
    {
        public int EmployeeId { get; set; }
        public string EmployeeNo { get; set; }
        public DateTime ClockStamp { get; set; }
        public bool StampTag { get; set; }

        public TimeAttendanceLog ToTimeAttendanceLog(int userId, int organizationId)
            => TimeAttendanceLog.NewTimeAttendanceLog(userId: userId,
                organizationId: organizationId,
                importNumber: EmployeeNo,
                timeStamp: ClockStamp,
                workDay: ClockStamp.Date,
                employeeID: EmployeeId,
                isTimeIn: StampTag);
    }
}
