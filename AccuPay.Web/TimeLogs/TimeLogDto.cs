using AccuPay.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.TimeLogs
{
    public class TimeLogDto
    {
        public int? RowID { get; set; }

        public int? EmployeeID { get; set; }

        public DateTime LogDate { get; set; }

        public TimeSpan? TimeIn { get; set; }

        public TimeSpan? TimeOut { get; set; }

        public DateTime? TimeStampIn { get; set; }

        public DateTime? TimeStampOut { get; set; }

        public string TimeentrylogsImportID { get; set; }

        public int? BranchID { get; set; }

        internal static TimeLogDto Convert(TimeLog timeLog)
        {
            return new TimeLogDto()
            {
                BranchID = timeLog.BranchID,
                EmployeeID = timeLog.EmployeeID,
                LogDate = timeLog.LogDate,
                RowID = timeLog.RowID,
                TimeentrylogsImportID = timeLog.TimeentrylogsImportID,
                TimeIn = timeLog.TimeIn,
                TimeOut = timeLog.TimeOut,
                TimeStampIn = timeLog.TimeStampIn,
                TimeStampOut = timeLog.TimeStampOut
            };
        }
    }
}
