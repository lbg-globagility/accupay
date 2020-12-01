using AccuPay.Utilities.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("employeeofficialbusiness")]
    public class OfficialBusiness : BaseEmployeeDataEntity, IPayrollEntity
    {
        public const string StatusApproved = "Approved";

        public const string StatusPending = "Pending";

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

        // TODO: make this readonly. Domain methods should only be the one to set this.
        [Column("OffBusEndDate")]
        public DateTime? EndDate { get; set; }

        public string Reason { get; set; }

        public string Comments { get; set; }

        public Employee Employee { get; set; }

        public DateTime? PayrollDate => StartDate;

        public OfficialBusiness()
        {
            Status = StatusPending;
        }

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
    }
}
