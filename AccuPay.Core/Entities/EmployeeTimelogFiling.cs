using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace AccuPay.Core.Entities
{
    [Table("employeetimelogfiling")]
    public class EmployeeTimelogFiling : EmployeeDataEntity
    {
        public const string StatusApproved = "Approved";

        public const string StatusPending = "Pending";

        public const string StatusRejected = "Rejected";

        [Column("LogDate")]
        public DateTime LogDate { get; set; }

        public TimeSpan? TimeIn { get; set; }

        public TimeSpan? LunchOut { get; set; }

        public TimeSpan? LunchIn { get; set; }

        public TimeSpan? TimeOut { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }

        public string DecidedBy { get; set; }

        public bool IsApproved => Status == StatusApproved;

        public bool IsNotifyEmail { get; set; }
        public DateTime? NotifyEmailSentAt { get; set; }

        [NotMapped]
        public bool HasAnyTime => TimeIn.HasValue || LunchOut.HasValue || LunchIn.HasValue || TimeOut.HasValue;

        [NotMapped]
        public DateTime? TimeInFull => ToFullDate(TimeIn);

        [NotMapped]
        public DateTime? LunchOutFull => ToFullDate(LunchOut);

        [NotMapped]
        public DateTime? LunchInFull => ToFullDate(LunchIn);

        [NotMapped]
        public DateTime? TimeOutFull => ToFullDate(TimeOut);

        private DateTime? ToFullDate(TimeSpan? time) => time == null ? (DateTime?)null : LogDate.Date.Add(time.Value);

    }
}
