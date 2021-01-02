using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task CreateMany(IEnumerable<Vehicle> vehicles)
        {
            _context.Vehicles.AddRange(vehicles);
            await _context.SaveChangesAsync();
        }

        public ICollection<Vehicle> GetAll()
        {
            var vehicles = _context.Vehicles.ToList();

            return vehicles;
        }

        public async Task<ICollection<Vehicle>> GetAllAsync()
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
