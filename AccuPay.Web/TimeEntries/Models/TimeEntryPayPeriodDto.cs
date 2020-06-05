using AccuPay.Web.Payroll;
using System.Collections.Generic;

namespace AccuPay.Web.TimeEntries.Models
{
    public class TimeEntryPayPeriodDto : PayperiodDto
    {
        public int TimeEntryCount { get; set; }
        public int ShiftCount { get; set; }
        public int TimeLogCount { get; set; }
        public int LeaveCount { get; set; }
        public int OfficialBusinessCount { get; set; }
        public int OvertimeCount { get; set; }
        public int AbsentCount { get; set; }
        public int LateCount { get; set; }
        public int UndertimeCount { get; set; }

        public ICollection<HolidayDto> Holidays { get; set; }

        //public void CreateHolidayList()
    }
}
