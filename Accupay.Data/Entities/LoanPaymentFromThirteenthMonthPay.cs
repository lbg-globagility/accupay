using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("loanpaidthirteenthmonth")]
    public class LoanPaymentFromThirteenthMonthPay
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
        public int PaystubId { get; set; }
        public decimal AmountPayment { get; set; }

        [ForeignKey("PaystubId")]
        public virtual Paystub Paystub { get; set; }
    }
}
