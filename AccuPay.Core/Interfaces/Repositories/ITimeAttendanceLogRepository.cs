using AccuPay.Core.Entities;
using AccuPay.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ITimeAttendanceLogRepository
    {
        Task<ICollection<TimeAttendanceLog>> GetByDateAndEmployeeAsync(DateTime date, int employeeId);

        Task<ICollection<TimeAttendanceLog>> GetByTimePeriodAsync(int organizationId, TimePeriod timePeriod);
    }
}
