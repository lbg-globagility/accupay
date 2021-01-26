using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IRouteRepository
    {
        Task<ICollection<Route>> GetAll();

        Task<IEnumerable<Route>> GetAllAsync(int organizationId);
    }
}
