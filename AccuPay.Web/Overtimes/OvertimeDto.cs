using System;

namespace AccuPay.Web.Overtimes
{
    public class OvertimeDto
    {
        public int Id { get; set; }

        public string EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; }

        public string Reason { get; set; }

        public string Comments { get; set; }
    }
}
