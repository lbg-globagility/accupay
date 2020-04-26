using AccuPay.Utilities.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Data.Entities
{
    [Table("employeeleave")]
    public class Leave
    {
        public const string StatusApproved = "Approved";

        public const string StatusPending = "Pending";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int? RowID { get; set; }

        public virtual int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual DateTime Created { get; set; }

        public virtual int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime? LastUpd { get; set; }

        public virtual int? LastUpdBy { get; set; }

        public virtual int? EmployeeID { get; set; }

        public virtual string LeaveType { get; set; }

        public virtual decimal LeaveHours { get; set; }

        [Column("LeaveStartTime")]
        public virtual TimeSpan? StartTime { get; set; }

        [Column("LeaveEndTime")]
        public virtual TimeSpan? EndTime { get; set; }

        [Column("LeaveStartDate")]
        public virtual DateTime StartDate { get; set; }

        [Column("LeaveEndDate")]
        public virtual DateTime? EndDate { get; set; }

        public virtual string Reason { get; set; }

        public virtual string Comments { get; set; }

        public virtual byte[] Image { get; set; }

        public virtual string Status { get; set; }

        [NotMapped]
        public bool IsNew { get; set; } // Delete this. This is only used on ImportLeaveForm and other codes may use this and get a wrong result

        [NotMapped]
        public DateTime? StartTimeFull
        {
            get => StartTime == null ?
                    (DateTime?)null :
                    StartDate.Date.ToMinimumHourValue().Add(StartTime.Value);

            set => StartTime = value == null ? null : value?.TimeOfDay;
        }

        [NotMapped]
        public DateTime? EndTimeFull
        {
            get => EndTime == null ?
                        (DateTime?)null :
                        EndDate == null ?
                                DateTime.Now.ToMinimumHourValue().Add(EndTime.Value) :
                                EndDate.Value.Date.ToMinimumHourValue().Add(EndTime.Value);

            set => EndTime = value == null ? null : value?.TimeOfDay;
        }

        // End Date that is not nullable since it should not be nullable
        [NotMapped]
        public DateTime ProperEndDate
        {
            get => EndDate ?? DateTime.Now.Date;
            set => EndDate = value;
        }

        public string Validate()
        {
            if (EndDate == null)
                return "End Date cannot be empty.";

            if ((StartTime.HasValue && EndTime == null) || (EndTime.HasValue && StartTime == null))
                return "Both Start Time and End Time should have value or both should be empty.";

            if (StartTime == null & EndTime == null)
            {
                if (StartDate.Date > EndDate.Value.Date)
                    return "Start Date cannot be greater than End Date (empty start and end times)";
            }
            else if (StartDate.Date.Add(StartTime.Value.StripSeconds()) >= EndDate.Value.Date.Add(EndTime.Value.StripSeconds()))
                return "Start date and time cannot be greater than or equal to End date and time.";

            if (string.IsNullOrWhiteSpace(LeaveType))
                return "Leave Type cannot be empty.";

            if (new string[] { StatusPending, StatusApproved }.Contains(Status) == false)
            {
                return "Status is not valid.";
            }

            // Means no error
            return null;
        }
    }
}