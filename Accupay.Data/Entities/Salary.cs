using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace AccuPay.Data.Entities
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
    }
}
