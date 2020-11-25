using AccuPay.Utilities.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("employeetimeentrydetails")]
    public class TimeLog : BaseEntity
    {
        public int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int? EmployeeID { get; set; }

        [Column("Date")]
        public DateTime LogDate { get; set; }

        public TimeSpan? TimeIn { get; set; }

        public TimeSpan? TimeOut { get; set; }

        public DateTime? TimeStampIn { get; set; }

        public DateTime? TimeStampOut { get; set; }

        public string TimeentrylogsImportID { get; set; }

        public int? BranchID { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("BranchID")]
        public virtual Branch Branch { get; set; }

        // Eventually just use TimeStampIn and TimeStampOut instead
        // of using TimeInFull and TimeOutFull. But since TimeStampIn and TimeStampOut
        // is created by Lambert, its data is not reliable.
        [NotMapped]
        public DateTime? TimeInFull
        {
            get => TimeIn == null ?
                        (DateTime?)null :
                        LogDate.Date.ToMinimumHourValue().Add(TimeIn.Value);

            set => TimeIn = value == null ? null : value?.TimeOfDay;
        }

        [NotMapped]
        public DateTime? TimeOutFull
        {
            get => TimeOut == null ?
                        (DateTime?)null :
                        LogDate.Date.ToMinimumHourValue().Add(TimeOut.Value);

            set => TimeOut = value == null ? null : value?.TimeOfDay;
        }
    }
}
