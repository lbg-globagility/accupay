using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Salaries.Models
{
    public abstract class CrudSalaryDto
    {
        private const double MinimumAmount = 0.01;
        private const double MaximumAmount = 99999999.99;

        [Required]
        public DateTime EffectiveFrom { get; set; }

        [Required]
        [Range(MinimumAmount, MaximumAmount)]
        public decimal BasicSalary { get; set; }

        [Required]
        [Range(0, MaximumAmount)]
        public decimal AllowanceSalary { get; set; }

        public bool DoPaySSSContribution { get; set; }

        public bool AutoComputeHDMFContribution { get; set; }

        [Range(0, MaximumAmount)]
        public decimal HDMFDeduction { get; set; }

        public bool AutoComputePhilHealthContribution { get; set; }

        [Range(0, MaximumAmount)]
        public decimal PhilHealthDeduction { get; set; }
    }
}
