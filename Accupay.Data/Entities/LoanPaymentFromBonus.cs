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

        public int LoanId { get; set; }
        public int BonusId { get; set; }
        public int? PaystubId { get; set; }
        public decimal AmountPayment { get; set; }

        [ForeignKey("LoanId")]
        public virtual LoanSchedule LoanSchedule { get; set; }

        [ForeignKey("BonusId")]
        public virtual Bonus Bonus { get; set; }

        public decimal DeductionAmount { get; set; }

        [ForeignKey("PaystubId")]
        public virtual Paystub Paystub { get; set; }
    }
}