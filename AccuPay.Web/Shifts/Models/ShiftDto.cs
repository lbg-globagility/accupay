using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Web.Shifts.Models
{
    public class ShiftDto
    {
        public int Id { get; set; }

        public string EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

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
