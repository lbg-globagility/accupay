using System;

namespace AccuPay.Web.Shifts.Models
{
    public class ShiftDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeType { get; set; }

        public DateTime Date { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? BreakStartTime { get; set; }

        public decimal BreakLength { get; set; }

        public bool IsOffset { get; set; }
    }
}
