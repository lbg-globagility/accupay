using AccuPay.Core.Entities;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IAgencyFeeRepository
    {
        Task<ICollection<AgencyFee>> GetByDatePeriodAsync(int organizationId, TimePeriod timePeriod);

        decimal GetPaystubAmount(int organizationId, TimePeriod timePeriod, int employeeId);
    }
}
