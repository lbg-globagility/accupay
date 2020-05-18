using AccuPay.Data.Entities;
using System;

namespace AccuPay.Web.Salaries.Models
{
    class SalaryDto
    {
        public static SalaryDto Produce(Salary salary)
        {
            return FromSalaryToDto(salary);
        }
        
        internal static SalaryDto FromSalaryToDto(Salary _salary)
        {
            return new SalaryDto()
            {
                RowID = _salary.RowID,
                EmployeeID = _salary.EmployeeID,
                FilingStatusID = _salary.FilingStatusID,
                PositionID = _salary.PositionID,
                PayPhilHealthID = _salary.PayPhilHealthID,
                PhilHealthDeduction = _salary.PhilHealthDeduction,
                HDMFAmount = _salary.HDMFAmount,
                BasicSalary = _salary.BasicSalary,
                AllowanceSalary = _salary.AllowanceSalary,
                TotalSalary = _salary.TotalSalary,
                //DailyRate = _salary.DailyRate,
                //HourlyRate = _salary.HourlyRate,
                NoOfDependents = _salary.NoOfDependents,
                MaritalStatus = _salary.MaritalStatus,
                EffectiveFrom = _salary.EffectiveFrom,
                EffectiveTo = _salary.EffectiveTo,
                DoPaySSSContribution = _salary.DoPaySSSContribution,
                AutoComputeHDMFContribution = _salary.AutoComputeHDMFContribution,
                AutoComputePhilHealthContribution = _salary.AutoComputePhilHealthContribution
            };
        }


        public int? RowID { get; set; }

        public int? EmployeeID { get; set; }

        public int? OrganizationID { get; set; }

        public int? FilingStatusID { get; set; }

        public int? PositionID { get; set; }

        public int? PayPhilHealthID { get; set; }

        public decimal PhilHealthDeduction { get; set; }

        public decimal HDMFAmount { get; set; }

        public decimal BasicSalary { get; set; }

        public decimal AllowanceSalary { get; set; }

        public decimal TotalSalary { get; set; }

        public decimal DailyRate { get; set; }

        public decimal HourlyRate { get; set; }

        public int NoOfDependents { get; set; }

        public string MaritalStatus { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public bool DoPaySSSContribution { get; set; }

        public bool AutoComputePhilHealthContribution { get; set; }

        public bool AutoComputeHDMFContribution { get; set; }
    }
}
