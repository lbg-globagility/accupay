using System;

namespace AccuPay.Web.Salaries.Models
{
    public class SalaryDto
    {
        public int Id { get; set; }

        public string EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeType { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public decimal BasicSalary { get; set; }

        public decimal AllowanceSalary { get; set; }

        public decimal TotalSalary { get; set; }

        public bool DoPaySSSContribution { get; set; }

        public bool AutoComputePhilHealthContribution { get; set; }

        public decimal PhilHealthDeduction { get; set; }

        public bool AutoComputeHDMFContribution { get; set; }

        public decimal HDMFDeduction { get; set; }
    }
}
