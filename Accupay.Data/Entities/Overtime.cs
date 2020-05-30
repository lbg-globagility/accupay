using AccuPay.Utilities.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("employeeovertime")]
    public class Overtime
    {
        public const string StatusApproved = "Approved";

        public const string StatusPending = "Pending";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int? EmployeeID { get; set; }

        [Column("OTType")]
        public string Type { get; set; }

        public TimeSpan? OTStartTime { get; set; }

        public TimeSpan? OTEndTime { get; set; }

        public DateTime OTStartDate { get; set; }

        // TODO: make this readonly. Domain methods should only be the one to set this.
        public DateTime OTEndDate { get; set; }

        [Column("OTStatus")]
        public string Status { get; set; }

        public string Reason { get; set; }

        public string Comments { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        public Overtime()
        {
            Status = StatusPending;
        }

        [NotMapped]
        public DateTime? OTStartTimeFull
        {
            get => OTStartTime == null ?
                        (DateTime?)null :
                        OTStartDate.Date.ToMinimumHourValue().Add(OTStartTime.Value);

            set => OTStartTime = value == null ? null : value?.TimeOfDay;
        }

        [NotMapped]
        public DateTime? OTEndTimeFull
        {
            get => OTEndTime == null ?
                        (DateTime?)null :
                        OTEndDate.Date.ToMinimumHourValue().Add(OTEndTime.Value);

            set => OTEndTime = value == null ? null : value?.TimeOfDay;
        }

        public void UpdateEndDate()
        {
            if (OTStartTime == null || OTEndTime == null)
            {
                OTEndDate = OTStartDate;
            }
            else
            {
                OTEndDate = OTEndTime < OTStartTime ? OTStartDate.AddDays(1) : OTStartDate;
            }
        }
    }
}