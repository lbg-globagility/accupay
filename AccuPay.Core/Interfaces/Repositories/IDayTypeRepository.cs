using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IDayTypeRepository
    {
        Task<ICollection<DayType>> GetAllAsync();

        Task<DayType> GetOrCreateRegularDayAsync();

        Task SaveAsync(DayType dayType);
    }
}
