using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("paywithholdingtax")]
    public class WithholdingTaxBracket
    {
        [Key]
        public int? RowID { get; set; }

        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int? PayFrequencyID { get; set; }

        public int? FilingStatusID { get; set; }

        public DateTime EffectiveDateFrom { get; set; }

        public DateTime EffectiveDateTo { get; set; }

        public decimal ExemptionAmount { get; set; }

        public decimal ExemptionInExcessAmount { get; set; }

        public decimal TaxableIncomeFromAmount { get; set; }

        public decimal TaxableIncomeToAmount { get; set; }

        public bool IsSemiMonthly => PayFrequencyID == PayFrequency.SemiMonthlyTypeId;
        public bool IsMonthly => PayFrequencyID == PayFrequency.MonthlyTypeId;
    }
}
