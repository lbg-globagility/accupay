using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IRouteQueryBuilder
    {
        List<Route> ToList(int organizationId);

        Task<List<Route>> ToListAsync(int? organizationId);
    }
}
