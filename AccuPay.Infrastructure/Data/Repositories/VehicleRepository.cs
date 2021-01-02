using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly PayrollContext _context;

        public VehicleRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Vehicle>> GetAll()
        {
            var vehicles = await _context.Vehicles.ToListAsync();

            return vehicles;
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync(int organizationId)
        {
            var builder = new VehicleQueryBuilder(_context);
            return await builder.
                ToListAsync(organizationId);
        }
    }
}
