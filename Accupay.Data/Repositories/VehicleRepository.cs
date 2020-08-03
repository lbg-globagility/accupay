using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class VehicleRepository
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
    }
}
