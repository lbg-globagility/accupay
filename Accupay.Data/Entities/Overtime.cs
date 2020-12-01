using AccuPay.Data.Interfaces;
using AccuPay.Utilities.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("employeeovertime")]
    public class Overtime : BaseEmployeeDataEntity, IPayrollEntity
    {
        public const string StatusApproved = "Approved";

        public const string StatusPending = "Pending";

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

        public DateTime? PayrollDate => OTStartDate;

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

        public static Overtime NewOvertime(IShift shift, int organizationId, int userId)
        {
            return new Overtime()
            {
                Created = DateTime.Now,
                CreatedBy = userId,
                EmployeeID = shift.EmployeeId,
                OrganizationID = organizationId,
                OTStartDate = shift.Date,
                OTEndDate = shift.Date,
                Status = StatusApproved
            };
        }
    }
}
