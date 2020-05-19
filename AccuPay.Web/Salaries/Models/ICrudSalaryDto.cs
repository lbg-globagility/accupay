using System;

namespace AccuPay.Web.Salaries.Models
{
    public interface ICrudSalaryDto
    {
        DateTime EffectiveFrom { get; set; }

        decimal BasicSalary { get; set; }

        decimal AllowanceSalary { get; set; }

        bool DoPaySSSContribution { get; set; }

        bool AutoComputeHDMFContribution { get; set; }

        decimal HDMFDeduction { get; set; }

        bool AutoComputePhilHealthContribution { get; set; }

        decimal PhilHealthDeduction { get; set; }
    }
}
