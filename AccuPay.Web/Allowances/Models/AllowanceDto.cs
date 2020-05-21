using System;

namespace AccuPay.Web.Allowances.Models
{
    public class AllowanceDto
    {
        public int Id { get; set; }

        public string EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeType { get; set; }

        public string AllowanceType { get; set; }

        public string Frequency { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal Amount { get; set; }
    }
}
