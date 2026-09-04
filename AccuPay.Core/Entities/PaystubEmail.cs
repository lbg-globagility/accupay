using AccuPay.Utilities.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Core.Entities
{
    [Table("paystubemail")]
    public class PaystubEmail
    {
        // TODO: make the statuses enum, to be make them strongly-typed
        public const string StatusWaiting = "WAITING";

        public const string StatusProcessing = "PROCESSING";
        public const string StatusFailed = "FAILED";

        public const string TypePayslip = "Payslip";
        public const string TypeDailyAttendanceReport = "DailyAttendanceReport";
        public const string TypeAccessOffshoringPayslip = "AccessOffshoringPayslip";

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
        [ColumnName("type")]
        public string Type { get; private set; }

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

        public static PaystubEmail Create(int createdByUserId, int paystubId, bool isActual, string type)
        {
            return new PaystubEmail()
            {
                CreatedBy = createdByUserId,
                PaystubID = paystubId,
                Status = StatusWaiting,
                IsActual = isActual,
                Type = type

            };
        }
    }
}
