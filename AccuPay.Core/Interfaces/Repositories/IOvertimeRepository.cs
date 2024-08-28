using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Helpers;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IOvertimeRepository : ISavableRepository<Overtime>
    {
        Task DeleteManyAsync(IEnumerable<int> ids);

        Task<ICollection<Overtime>> GetByDatePeriodAsync(int organizationId, TimePeriod datePeriod, OvertimeStatus overtimeStatus = OvertimeStatus.All);

        Task<ICollection<Overtime>> GetByEmployeeAndDatePeriod(int organizationId, int employeeId, TimePeriod datePeriod, OvertimeStatus overtimeStatus = OvertimeStatus.All);

        Task<ICollection<Overtime>> GetByEmployeeAsync(int employeeId);

        ICollection<Overtime> GetByEmployeeIDsAndDatePeriod(int organizationId, List<int> employeeIdList, TimePeriod datePeriod, OvertimeStatus overtimeStatus = OvertimeStatus.All);

        Task<ICollection<Overtime>> GetByEmployeeIdsBetweenDatesAsync(int organizationId, List<int> employeeIds, TimePeriod timePeriod);

        Task<Overtime> GetByIdWithEmployeeAsync(int id);

        Task<PaginatedList<Overtime>> GetPaginatedListAsync(OvertimePageOptions options, int organizationId);

        List<string> GetStatusList();

        Task<ICollection<Overtime>> GetOTWithEmployee();

        Task<ICollection<Overtime>> GetPendingOTWithEmployee();

        Task ApproveOvertimes(List<int> overtimeIds);
    }
}
