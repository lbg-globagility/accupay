using AccuPay.Core.Entities;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IAllowanceSalaryTimeEntryRepository : ISavableRepository<AllowanceSalaryTimeEntry>
    {
        Task<ICollection<AllowanceSalaryTimeEntry>> GetByDatePeriodAsync(int organizationId, TimePeriod timePeriod);
    }
}
