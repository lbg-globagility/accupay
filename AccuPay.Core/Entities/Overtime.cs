using AccuPay.Core.Interfaces;
using AccuPay.Utilities.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("employeeovertime")]
    public class Overtime : EmployeeDataEntity, IPayrollEntity
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

        private Overtime()
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

        public static Overtime NewOvertime(IShift shift, int organizationId)
        {
            return NewOvertime(
                organizationId: organizationId,
                employeeId: shift.EmployeeId.Value,
                startDate: shift.Date,
                startTime: null,
                endTime: null,
                status: StatusApproved);
        }

        public static Overtime NewOvertime(
            int organizationId,
            int employeeId,
            DateTime startDate,
            TimeSpan? startTime,
            TimeSpan? endTime,
            string status = StatusApproved,
            string reason = null,
            string comments = null)
        {
            var overtime = new Overtime()
            {
                OrganizationID = organizationId,
                EmployeeID = employeeId,
                OTStartDate = startDate,
                OTStartTime = startTime,
                OTEndTime = endTime,
                Status = status,
                Reason = reason,
                Comments = comments
            };

            overtime.UpdateEndDate();

            return overtime;
        }
    }
}
