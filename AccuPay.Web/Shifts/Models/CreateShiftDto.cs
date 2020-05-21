using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Web.Shifts.Models
{
    public class CreateShiftDto : ICrudShiftDto
    {
        public int EmployeeId { get; set; }

        public DateTime DateSched { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public TimeSpan? BreakStartTime { get; set; }

        public decimal BreakLength { get; set; }

        public bool IsRestDay { get; set; }

        public decimal ShiftHours { get; set; }

        public decimal WorkHours { get; set; }
    }
}
