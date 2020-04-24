using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("paysocialsecurity")]
    public class SocialSecurityBracket
    {
        [Key]
        public int? RowID { get; set; }

        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public decimal RangeFromAmount { get; set; }

        public decimal RangeToAmount { get; set; }

        public decimal MonthlySalaryCredit { get; set; }

        public decimal EmployeeContributionAmount { get; set; }

        public decimal EmployerContributionAmount { get; set; }

        public decimal EmployeeECAmount { get; set; }

        public bool HiddenData { get; set; }

        public DateTime EffectiveDateFrom { get; set; }

        public DateTime EffectiveDateTo { get; set; }
    }
}