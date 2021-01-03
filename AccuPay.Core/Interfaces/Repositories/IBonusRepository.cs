using AccuPay.Core.Entities;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IBonusRepository : ISavableRepository<Bonus>
    {
        Task<ICollection<Bonus>> GetByEmployeeAndPayPeriodAsync(int organizationId, int employeeId, TimePeriod timePeriod);

        Task<ICollection<Bonus>> GetByEmployeeAndPayPeriodForLoanPaymentAsync(int organizationId, int employeeId, TimePeriod timePeriod);

        Task<ICollection<Bonus>> GetByEmployeeAsync(int employeeId);

        Task<ICollection<Bonus>> GetByPayPeriodAsync(int organizationId, TimePeriod timePeriod);

        List<string> GetFrequencyList();
    }
}
