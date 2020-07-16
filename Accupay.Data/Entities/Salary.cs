using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("employeesalary")]
    public class Salary : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int? EmployeeID { get; set; }

        public int? OrganizationID { get; set; }

        public int? FilingStatusID { get; set; }

        public int? PositionID { get; set; }

        public int? PayPhilHealthID { get; set; }

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

        // TODO: delete this
        [Column("EffectiveDateTo")]
        public DateTime? EffectiveTo { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        public bool DoPaySSSContribution { get; set; }

        public bool AutoComputePhilHealthContribution { get; set; }

        public bool AutoComputeHDMFContribution { get; set; }

        public bool IsIndefinite => !EffectiveTo.HasValue;

        /// <summary>
        /// Updates TotalSalary. Call this everytime BasicSalary or AllowanceSalary has changed.
        /// </summary>
        public void UpdateTotalSalary() => TotalSalary = BasicSalary + AllowanceSalary;
    }
}