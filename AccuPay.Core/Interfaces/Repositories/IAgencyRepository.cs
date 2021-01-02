using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IAgencyRepository
    {
        Task<ICollection<Agency>> GetAllAsync(int organizationId);
    }
}
