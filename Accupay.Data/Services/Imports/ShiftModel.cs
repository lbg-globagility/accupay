using AccuPay.Data.Entities;
using System;

namespace AccuPay.Data.Services.Imports
{
    public class ShiftModel
    {
        public int? EmployeeId { get; set; }

        public DateTime Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public TimeSpan? BreakTime { get; set; }
        public decimal BreakLength { get; set; }
        public bool IsRestDay { get; set; }

        public EmployeeDutySchedule ToEmployeeDutySchedule(int organizationId, int userId)
        {
            var shift = new EmployeeDutySchedule()
            {
                EmployeeID = EmployeeId,
                OrganizationID = organizationId,
                DateSched = Date,
                CreatedBy = userId,
                Created = DateTime.Now,
                LastUpdBy = userId,
                LastUpd = DateTime.Now,
                StartTime = StartTime,
                EndTime = EndTime,
                BreakStartTime = BreakTime,
                BreakLength = BreakLength,
                IsRestDay = IsRestDay,
            };

            shift.ComputeShiftHours();

            return shift;
        }
    }
}