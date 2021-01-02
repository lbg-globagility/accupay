using AccuPay.Core.Entities;
using AccuPay.Core.Services;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ITimeEntryDataService
    {
        Task DeleteByEmployeeAsync(int employeeId, int payPeriodId, int currentlyLoggedInUserId);

        Task<ICollection<TimeEntryData>> GetEmployeeTimeEntries(int organizationId, int employeeId, TimePeriod datePeriod);

        Task RecordCreateByEmployee(int currentlyLoggedInUserId, IReadOnlyCollection<TimeEntry> timeEntries);

        Task RecordDeleteByEmployee(int currentlyLoggedInUserId, IReadOnlyCollection<TimeEntry> timeEntries);

        Task RecordEditByEmployee(int currentlyLoggedInUserId, IReadOnlyCollection<TimeEntry> timeEntries);
    }
}
