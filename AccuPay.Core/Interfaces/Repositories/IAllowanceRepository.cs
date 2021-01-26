using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IAllowanceRepository : ISavableRepository<Allowance>
    {
        Task<ICollection<AllowanceItem>> GetAllowanceItemsWithPayPeriodAsync(int[] ids);

        Task<ICollection<Allowance>> GetByEmployeeWithProductAsync(int employeeId);

        Task<Allowance> GetByIdWithEmployeeAndProductAsync(int id);

        ICollection<Allowance> GetByPayPeriodWithProduct(int organizationId, TimePeriod timePeriod);

        Task<ICollection<Allowance>> GetByPayPeriodWithProductAsync(int organizationId, TimePeriod timePeriod);

        Task<Allowance> GetEmployeeEcolaAsync(int employeeId, int organizationId, TimePeriod timePeriod);

        Task<List<Allowance>> GetByEmployeeIdsBetweenDatesByAllowanceTypesAsync(List<int> employeeIds, List<string> allowanceTypeNames, TimePeriod timePeriod);

        List<string> GetFrequencyList();

        Task<PaginatedList<Allowance>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "");

        Task<ICollection<PayPeriod>> GetPayPeriodsAsync(int id);

        Task<ICollection<PayPeriod>> GetPayPeriodsAsync(int[] ids);
    }
}
