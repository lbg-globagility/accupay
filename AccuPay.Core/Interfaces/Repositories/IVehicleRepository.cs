using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IVehicleRepository
    {
        ICollection<Vehicle> GetAll();

        Task<ICollection<Vehicle>> GetAllAsync();

        Task<IEnumerable<Vehicle>> GetAllAsync(int organizationId);

        Task CreateMany(IEnumerable<Vehicle> vehicles);
    }
}
