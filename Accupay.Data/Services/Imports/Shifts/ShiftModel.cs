using AccuPay.Data.Entities;
using AccuPay.Data.Interfaces;
using System;

namespace AccuPay.Data.Services
{
    public class ShiftModel : IShift
    {
        public int? EmployeeId { get; set; }

        public DateTime Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public TimeSpan? BreakTime { get; set; }
        public decimal BreakLength { get; set; }
        public bool IsRestDay { get; set; }

        public EmployeeDutySchedule ToEmployeeDutySchedule(int organizationId, int changedByUserId)
        {
            var shift = new EmployeeDutySchedule()
            {
                EmployeeID = EmployeeId,
                OrganizationID = organizationId,
                DateSched = Date,
                CreatedBy = changedByUserId,
                Created = DateTime.Now,
                LastUpdBy = changedByUserId,
                LastUpd = DateTime.Now,
                StartTime = StartTime,
                EndTime = EndTime,
                BreakStartTime = BreakTime,
                BreakLength = BreakLength,
                IsRestDay = IsRestDay,
            };

            return shift;
        }
    }
}
