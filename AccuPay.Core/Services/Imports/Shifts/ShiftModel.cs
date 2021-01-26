using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using System;

namespace AccuPay.Core.Services
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

        public Shift ToShift(int organizationId)
        {
            var shift = new Shift()
            {
                EmployeeID = EmployeeId,
                OrganizationID = organizationId,
                DateSched = Date,
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
