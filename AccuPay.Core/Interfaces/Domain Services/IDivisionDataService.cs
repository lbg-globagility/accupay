using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IDivisionDataService : IBaseSavableDataService<Division>
    {
        Task<Division> GetOrCreateDefaultDivisionAsync(int organizationId, int changedByUserId);

        Task<ICollection<string>> GetSchedulesAsync();
    }
}
