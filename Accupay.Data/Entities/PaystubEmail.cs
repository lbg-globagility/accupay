using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Data.Entities
{
    [Table("paystubemail")]
    public class PaystubEmail
    {
        // TODO: make the statuses enum, to be make them strongly-typed
        public const string StatusWaiting = "WAITING";

        public const string StatusProcessing = "PROCESSING";
        public const string StatusFailed = "FAILED";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RowID { get; private set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; private set; }

        public int CreatedBy { get; private set; }

        public int PaystubID { get; private set; }
        public DateTime? ProcessingStarted { get; private set; }
        public string ErrorLogMessage { get; private set; }
        public string Status { get; private set; }
        public bool IsActual { get; private set; }

        [ForeignKey("PaystubID")]
        public virtual Paystub Paystub { get; private set; }

        private PaystubEmail()
        {
        }

        public void SetStatusToFailed(string errorLogMessage)
        {
            Status = StatusFailed;
            ErrorLogMessage = errorLogMessage;
        }

        public void SetStatusToProcessing()
        {
            Status = StatusProcessing;
            ProcessingStarted = DateTime.Now;
        }

        public void ResetStatus()
        {
            Status = StatusWaiting;
            ProcessingStarted = null;
        }

        public static PaystubEmail Create(int createdByUserId, int paystubId, bool isActual)
        {
            return new PaystubEmail()
            {
                CreatedBy = createdByUserId,
                PaystubID = paystubId,
                Status = StatusWaiting,
                IsActual = isActual
            };
        }
    }
}