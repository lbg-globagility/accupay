using AccuPay.Core.Entities;
using AccuPay.Core.Services;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ITimeEntryRepository
    {
        Task<(IReadOnlyCollection<TimeEntry> timeEntries, IReadOnlyCollection<ActualTimeEntry> actualTimeEntries)> DeleteByEmployeeAsync(int employeeId, int payPeriodId);

        Task<(IReadOnlyCollection<TimeEntry> timeEntries, IReadOnlyCollection<ActualTimeEntry> actualTimeEntries)> DeleteByPayPeriodAsync(int payPeriodId);

        Task<ICollection<TimeEntry>> GetByDatePeriodAsync(int organizationId, TimePeriod datePeriod);

        Task<ICollection<TimeEntry>> GetByEmployeeAndDatePeriodAsync(int organizationId, int employeeId, TimePeriod datePeriod);

        Task<TimeEntryPolicy> GetTimeEntryPolicy();
    }
}
