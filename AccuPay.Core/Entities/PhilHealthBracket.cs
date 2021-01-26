using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("payphilhealth")]
    public class PhilHealthBracket
    {
        [Key]
        public int? RowID { get; set; }

        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int SalaryBracket { get; set; }

        public decimal SalaryRangeFrom { get; set; }

        public decimal SalaryRangeTo { get; set; }

        public decimal SalaryBase { get; set; }

        public decimal TotalMonthlyPremium { get; set; }

        public decimal EmployeeShare { get; set; }

        public decimal EmployerShare { get; set; }

        public bool HiddenData { get; set; }
    }
}