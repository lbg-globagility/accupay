using AccuPay.Utilities.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Data.Entities
{
    [Table("employeeofficialbusiness")]
    public class OfficialBusiness
    {
        public const string StatusApproved = "Approved";

        public const string StatusPending = "Pending";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? LastUpdBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? EmployeeID { get; set; }

        [Column("OffBusType")]
        public string Type { get; set; }

        [Column("OffBusStatus")]
        public string Status { get; set; }

        [Column("OffBusStartTime")]
        public TimeSpan? StartTime { get; set; }

        [Column("OffBusEndTime")]
        public TimeSpan? EndTime { get; set; }

        [Column("OffBusStartDate")]
        public DateTime? StartDate { get; set; }

        [Column("OffBusEndDate")]
        public DateTime? EndDate { get; set; }

        public string Reason { get; set; }

        public string Comments { get; set; }

        public virtual Employee Employee { get; set; }

        [NotMapped]
        public DateTime? StartTimeFull
        {
            get => StartTime == null ?
                        (DateTime?)null :
                        ProperStartDate.Date.ToMinimumHourValue().Add(StartTime.Value);

            set => StartTime = value == null ? null : value?.TimeOfDay;
        }

        [NotMapped]
        public DateTime? EndTimeFull
        {
            get => EndTime == null ?
                        (DateTime?)null :
                        ProperEndDate.Date.ToMinimumHourValue().Add(EndTime.Value);

            set => EndTime = value == null ? null : value?.TimeOfDay;
        }

        // Start Date that is not nullable since it should not be nullable
        [NotMapped]
        public DateTime ProperStartDate
        {
            get => StartDate ?? DateTime.Now.Date;
            set => StartDate = value;
        }

        // End Date that is not nullable since it should not be nullable
        [NotMapped]
        public DateTime ProperEndDate
        {
            get => EndDate ?? ProperStartDate;
            set => EndDate = value;
        }

        public void UpdateEndDate()
        {
            if (StartTime == null || EndTime == null)
            {
                EndDate = ProperEndDate;
            }
            else
            {
                EndDate = EndTime < StartTime ? ProperStartDate.AddDays(1) : StartDate;
            }
        }

        public string Validate()
        {
            if (StartDate == null && EndDate == null)
                return "Start Date and End Date cannot be both empty.";

            if (StartDate != null && StartTime == null)
                return "Start Time cannot be empty if Start Date has value.";

            if (EndDate != null && EndTime == null)
                return "End Time cannot be empty if End Date has value.";

            if (StartTime != null && StartDate == null)
                return "Start Date cannot be empty if Start Time has value.";

            if (EndTime != null && EndDate == null)
                return "End Date cannot be empty if End Time has value.";

            if (StartDate != null && EndDate != null && StartDate.Value.Date.Add(StartTime.Value.StripSeconds()) >= EndDate.Value.Date.Add(EndTime.Value.StripSeconds()))
                return "Start date and time cannot be greater than or equal to End date and time.";

            if (new string[] { StatusPending, StatusApproved }.Contains(Status) == false)
            {
                return "Status is not valid.";
            }

            // Means no error
            return null;
        }
    }
}