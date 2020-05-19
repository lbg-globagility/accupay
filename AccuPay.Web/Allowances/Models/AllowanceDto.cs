using System;

namespace AccuPay.Web.Allowances.Models
{
    public class AllowanceDto
    {
        public int Id { get; set; }

        public string EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

        public string AllowanceType { get; set; }

        public DateTime EffectiveStartDate { get; set; }

        public string AllowanceFrequency { get; set; }

        public DateTime? EffectiveEndDate { get; set; }

        public decimal Amount { get; set; }
    }
}
