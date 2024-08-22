using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IOfficialBusinessRepository : ISavableRepository<OfficialBusiness>
    {
        Task<ICollection<OfficialBusiness>> GetAllApprovedByDatePeriodAsync(int organizationId, TimePeriod datePeriod);

        Task<ICollection<OfficialBusiness>> GetAllApprovedByEmployeeAndDatePeriodAsync(int organizationId, int employeeId, TimePeriod datePeriod);

        Task<ICollection<OfficialBusiness>> GetByEmployeeAsync(int employeeId);

        Task<List<OfficialBusiness>> GetByEmployeeIdsBetweenDatesAsync(int organizationId, List<int> employeeIds, TimePeriod timePeriod);

        Task<OfficialBusiness> GetByIdWithEmployeeAsync(int id);

        Task<PaginatedList<OfficialBusiness>> GetPaginatedListAsync(OfficialBusinessPageOptions options, int organizationId);

        List<string> GetStatusList();

        Task<ICollection<OfficialBusiness>> GetOBWithEmployee();
    }
}
