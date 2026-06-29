using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("employeetimeattendancelog")]
    public partial class TimeAttendanceLog : OrganizationalEntity
    {
        public string ImportNumber { get; set; }

        public DateTime TimeStamp { get; set; }

        public bool? IsTimeIn { get; set; }

        public DateTime WorkDay { get; set; }

        public int EmployeeID { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        public string IsTimeInDescription
        {
            get
            {
                if (IsTimeIn == null)
                    return "";

                return IsTimeIn == true ? "IN" : "OUT";
            }
        }
    }

    public partial class TimeAttendanceLog
    {
        public TimeAttendanceLog()
        {
        }

        public TimeAttendanceLog(int userId,
            int organizationId,
            DateTime timeStamp,
            DateTime workDay,
            int employeeID,
            bool? isTimeIn = null,
            string importNumber = null)
        {
            OrganizationID= organizationId;
            ImportNumber = importNumber;
            TimeStamp = timeStamp;
            WorkDay = workDay;
            EmployeeID = employeeID;
            IsTimeIn = isTimeIn;

            if (IsNewEntity) CreatedBy = userId;
            if (!IsNewEntity) LastUpdBy = userId;
        }

        public static TimeAttendanceLog NewTimeAttendanceLog(int userId,
            int organizationId,
            DateTime timeStamp,
            DateTime workDay,
            int employeeID,
            bool? isTimeIn = null,
            string importNumber = null)
            => new TimeAttendanceLog(userId: userId,
                organizationId: organizationId,
                timeStamp: timeStamp,
                workDay: workDay,
                employeeID: employeeID,
                isTimeIn: isTimeIn,
                importNumber: importNumber);
    }
}
