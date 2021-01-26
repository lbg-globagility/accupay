using AccuPay.Core.Entities;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IActualTimeEntryRepository
    {
        Task<ICollection<ActualTimeEntry>> GetByDatePeriodAsync(int organizationId, TimePeriod timePeriod);
    }
}
