using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("paystubemailhistory")]
    public class PaystubEmailHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RowID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int SentBy { get; set; }

        public int PaystubID { get; set; }
        public DateTime? SentDateTime { get; set; }
        public string ReferenceNumber { get; set; }
        public string EmailAddress { get; set; }
        public bool IsActual { get; set; }

        [ForeignKey("PaystubID")]
        public virtual Paystub Paystub { get; set; }
    }
}