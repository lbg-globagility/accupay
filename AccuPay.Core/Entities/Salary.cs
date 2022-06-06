using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace AccuPay.Core.Entities
{
    [Table("employeesalary")]
    public class Salary : EmployeeDataEntity
    {
        public int? PositionID { get; set; }

        public decimal PhilHealthDeduction { get; set; }

        public decimal HDMFAmount { get; set; }

        [Column("Salary")]
        public decimal BasicSalary { get; set; }

        [Column("UndeclaredSalary")]
        public decimal AllowanceSalary { get; set; }

        // TODO: make this readonly. Domain methods should only be the one to set this.
        [Column("TrueSalary")]
        public decimal TotalSalary { get; internal set; }

        public string MaritalStatus { get; set; }

        [Column("EffectiveDateFrom")]
        public DateTime EffectiveFrom { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        public bool DoPaySSSContribution { get; set; }

        public bool AutoComputePhilHealthContribution { get; set; }

        public bool AutoComputeHDMFContribution { get; set; }

        public bool IsMinimumWage { get; set; }

        /// <summary>
        /// Updates TotalSalary. Call this everytime BasicSalary or AllowanceSalary has changed.
        /// </summary>
        public void UpdateTotalSalary() => TotalSalary = BasicSalary + AllowanceSalary;

        public string AutoComputeSSSDisplay => DoPaySSSContribution ? "AUTO" : "NO";

        public string AutoComputePhilHealthDisplay => AutoComputePhilHealthContribution ? "AUTO" : PhilHealthDeduction.ToString("N", CultureInfo.CurrentCulture);

        public string AutoComputeHDMFDisplay => AutoComputeHDMFContribution ? "AUTO" : HDMFAmount.ToString("N", CultureInfo.CurrentCulture);

        [NotMapped]
        public DateTime? EffectiveTo { get; set; }

        public static Salary NewSalary(int employeeId,
                int organizationId,
                int? positionID,
                decimal philHealthDeduction,
                decimal hDMFAmount,
                decimal basicSalary,
                decimal allowanceSalary,
                decimal totalSalary,
                DateTime effectiveFrom,
                int doPaySSSContribution,
                int autoComputePhilHealthContribution,
                int autoComputeHDMFContribution,
                int isMinimumWage,
                DateTime? effectiveTo = null)
        {
            return new Salary()
            {
                EmployeeID = employeeId,
                OrganizationID = organizationId,
                PositionID = positionID,
                PhilHealthDeduction = philHealthDeduction,
                HDMFAmount = hDMFAmount,
                BasicSalary = basicSalary,
                AllowanceSalary = allowanceSalary,
                TotalSalary = totalSalary,
                EffectiveFrom = effectiveFrom,
                DoPaySSSContribution = doPaySSSContribution == 1,
                AutoComputePhilHealthContribution = autoComputePhilHealthContribution == 1,
                AutoComputeHDMFContribution = autoComputeHDMFContribution == 1,
                IsMinimumWage = isMinimumWage == 1,
                EffectiveTo = effectiveTo
            };
        }
    }
}
