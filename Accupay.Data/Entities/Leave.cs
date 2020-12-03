using AccuPay.Utilities.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Data.Entities
{
    [Table("employeeleave")]
    public class Leave : EmployeeDataEntity, IPayrollEntity
    {
        public const string StatusApproved = "Approved";

        public const string StatusPending = "Pending";

        public const string StatusRejected = "Rejected";

        public string LeaveType { get; set; }

        [Column("LeaveStartTime")]
        public TimeSpan? StartTime { get; set; }

        [Column("LeaveEndTime")]
        public TimeSpan? EndTime { get; set; }

        [Column("LeaveStartDate")]
        public DateTime StartDate { get; set; }

        // TODO: make this readonly. Domain methods should only be the one to set this.
        [Column("LeaveEndDate")]
        public DateTime? EndDate { get; set; }

        public string Reason { get; set; }

        public string Comments { get; set; }

        public byte[] Image { get; set; }

        public string Status { get; set; }

        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; }

        public DateTime? PayrollDate => StartDate;

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
                        ProperEndDate.Date.ToMinimumHourValue().Add(EndTime.Value);

            set => EndTime = value == null ? null : value?.TimeOfDay;
        }

        // End Date that is not nullable since it should not be nullable
        [NotMapped]
        public DateTime ProperEndDate
        {
            get => EndDate ?? StartDate;
            set => EndDate = value;
        }

        [NotMapped]
        public bool IsWholeDay => StartTime == null && EndTime == null;

        public void UpdateEndDate()
        {
            if (StartTime == null || EndTime == null)
            {
                EndDate = ProperEndDate;
            }
            else
            {
                EndDate = EndTime < StartTime ? StartDate.AddDays(1) : StartDate;
            }
        }

        public string Validate()
        {
            if (EndDate == null)
                return "End Date cannot be empty.";

            if ((StartTime.HasValue && EndTime == null) || (EndTime.HasValue && StartTime == null))
                return "Both Start Time and End Time should have value or both should be empty.";

            if (StartTime == null && EndTime == null)
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
