using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("employeesalary")]
    public class Salary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

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

        [Column("TrueSalary")]
        public decimal TotalSalary { get; set; }

        [Column("BasicDailyPay")]
        public decimal DailyRate { get; set; }

        [Column("BasicHourlyPay")]
        public decimal HourlyRate { get; set; }

        public int NoOfDependents { get; set; }

        public string MaritalStatus { get; set; }

        [Column("EffectiveDateFrom")]
        public DateTime EffectiveFrom { get; set; }

        [Column("EffectiveDateTo")]
        public DateTime? EffectiveTo { get; set; }

        //[ForeignKey("EmployeeID")]
        //public virtual Employee Employee { get; set; }

        public bool DoPaySSSContribution { get; set; }

        public bool AutoComputePhilHealthContribution { get; set; }

        public bool AutoComputeHDMFContribution { get; set; }

        public bool IsIndefinite
        {
            get
            {
                return !EffectiveTo.HasValue;
            }
        }
    }
}