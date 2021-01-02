using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IVehicleRepository
    {
        Task<ICollection<Vehicle>> GetAll();

        Task<IEnumerable<Vehicle>> GetAllAsync(int organizationId);
    }
}
