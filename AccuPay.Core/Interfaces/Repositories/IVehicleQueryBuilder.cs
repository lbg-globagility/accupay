using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IVehicleQueryBuilder
    {
        List<Vehicle> ToList(int organizationId);

        Task<List<Vehicle>> ToListAsync(int? organizationId);
    }
}
