using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Data.Entities
{
    [Table("paystubemail")]
    public class PaystubEmail
    {
        public const string StatusWaiting = "WAITING";
        public const string StatusProcessing = "PROCESSING";
        public const string StatusFailed = "FAILED";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RowID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public int PaystubID { get; set; }
        public DateTime? ProcessingStarted { get; set; }
        public string ErrorLogMessage { get; set; }
        public string Status { get; set; }

        [ForeignKey("PaystubID")]
        public virtual Paystub Paystub { get; set; }
    }
}