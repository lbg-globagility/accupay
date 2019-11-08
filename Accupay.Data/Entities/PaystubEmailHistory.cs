using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Accupay.Data.Entities
{
    [Table("paystubemailhistory")]
    public class PaystubEmailHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RowID { get; set; }

        public int PaystubID { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime? SentDateTime { get; set; }
        public int SentBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        [ForeignKey("PaystubID")]
        public virtual Paystub Paystub { get; set; }
    }
}