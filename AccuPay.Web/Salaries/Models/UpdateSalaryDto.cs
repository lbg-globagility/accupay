using System;

namespace AccuPay.Web.Salaries.Models
{
    public class UpdateSalaryDto : ICrudSalaryDto
    {
        public DateTime EffectiveFrom { get; set; }

        public decimal BasicSalary { get; set; }

        public decimal AllowanceSalary { get; set; }

        public bool DoPaySSSContribution { get; set; }

        public bool AutoComputeHDMFContribution { get; set; }

        public decimal HDMFDeduction { get; set; }

        public bool AutoComputePhilHealthContribution { get; set; }

        public decimal PhilHealthDeduction { get; set; }
    }
}
