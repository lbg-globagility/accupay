using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("loanpaidbonus")]
    public class LoanPaymentFromBonus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }
        public int LoanId { get; set; }
        public int BonusId { get; set; }
        public decimal AmountPayment { get; set; }

        [ForeignKey("LoanId")]
        public virtual Loan Loan { get; set; }

        [ForeignKey("BonusId")]
        public virtual Bonus Bonus { get; set; }

        public virtual ICollection<LoanPaymentFromBonusItem> Items { get; set; }
    }
}
