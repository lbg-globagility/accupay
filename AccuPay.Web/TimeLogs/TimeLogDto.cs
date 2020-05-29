using System;

namespace AccuPay.Web.TimeLogs
{
    public class TimeLogDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeType { get; set; }

        public DateTime Date { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? BranchId { get; set; }

        public string BranchName { get; set; }
    }
}
