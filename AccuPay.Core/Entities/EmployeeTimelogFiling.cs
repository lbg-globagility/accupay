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

        public const string CheckInType = "CheckIn";

        public const string CheckOutType = "CheckOut";
        public string EntryType { get; set; }

        [Column("LogDate")]
        public DateTime LogDate { get; set; }

        [Column("Time")]
        public TimeSpan Time { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }

        public string ApproverEmail { get; set; }

        public bool IsApproved => Status == StatusApproved;


        public string TimeStamp => Time.ToString(@"hh\:mm");

    }
}
