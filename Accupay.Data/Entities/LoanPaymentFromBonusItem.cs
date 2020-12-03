using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("loanpaidbonusitem")]
    public class LoanPaymentFromBonusItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int LoanPaidBonusId { get; set; }
        public int PaystubId { get; set; }

        [ForeignKey("LoanPaidBonusId")]
        public virtual LoanPaymentFromBonus LoanPaymentFromBonus { get; set; }

        [ForeignKey("PaystubId")]
        public virtual Paystub Paystub { get; internal set; }

        public static LoanPaymentFromBonusItem CreateNew(int loanPaidBonusId, Paystub paystub)
        {
            var item = new LoanPaymentFromBonusItem() { LoanPaidBonusId = loanPaidBonusId };
            if (paystub.RowID.HasValue)
            {
                item.PaystubId = paystub.RowID.Value;
            }
            else
            {
                item.Paystub = paystub;
            }

            return item;
        }
    }
}
