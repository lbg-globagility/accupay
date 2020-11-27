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
    }
}
