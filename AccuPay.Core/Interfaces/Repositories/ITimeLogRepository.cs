using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ITimeLogRepository : ISavableRepository<TimeLog>
    {
        Task<ICollection<TimeLog>> GetByDatePeriodAsync(int organizationId, TimePeriod datePeriod);

        Task<ICollection<TimeLog>> GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(IEnumerable<int> employeeIds, TimePeriod datePeriod);

        Task<ICollection<TimeLog>> GetLatestByEmployeeAndDatePeriodAsync(int employeeId, TimePeriod datePeriod);

        Task<(ICollection<Employee> employees, int total, ICollection<TimeLog> timeLogs)> ListByEmployeeAsync(int organizationId, TimeLogsByEmployeePageOptions options);

        Task CreateFilingAsync(EmployeeTimelogFiling filing);

        Task<EmployeeTimelogFiling> GetFilingByIdAsync(int filingId);

        Task<ICollection<EmployeeTimelogFiling>> GetFilingsByIdsAsync(IEnumerable<int> filingIds);

        Task UpdateFilingAsync(EmployeeTimelogFiling filing);

        Task<ICollection<EmployeeTimelogFiling>> GetLatestFilingByEmployeeAndDatePeriodAsync(int employeeId, TimePeriod datePeriod);

        Task<ICollection<EmployeeTimelogFiling>> GetPendingByEmployeeAsync(int employeeId);
    }
}
