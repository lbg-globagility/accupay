using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IRouteRateRepository
    {
        Task<ICollection<RoutePayRate>> GetAll(int? routeId);

        Task<ICollection<RoutePayRate>> GetAllAsync();

        Task SaveMany(ICollection<RoutePayRate> routeRates);
    }
}
