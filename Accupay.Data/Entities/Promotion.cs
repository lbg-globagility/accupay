using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("employeepromotions")]
    public class Promotion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int EmployeeID { get; set; }

        public string PositionFrom { get; set; }

        public string PositionTo { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public string CompensationChange { get; set; }

        public decimal? CompensationValue { get; set; }

        [ForeignKey("EmployeeSalaryID")]
        public virtual Salary SalaryEntity { get; set; }

        public int? EmployeeSalaryID { get; set; }

        public string Reason { get; set; }

        public decimal? NewAmount { get; set; }

        public string CompensationToYesNo => CompensationChange == "0" ? "No" : "Yes";

        public decimal? BasicPay => SalaryEntity?.BasicSalary;
    }
}