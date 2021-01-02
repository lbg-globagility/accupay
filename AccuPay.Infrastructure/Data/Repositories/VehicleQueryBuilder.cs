using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class VehicleQueryBuilder : IVehicleQueryBuilder
    {
        private readonly PayrollContext _context;

        private IQueryable<Vehicle> _query;

        public VehicleQueryBuilder(PayrollContext context)
        {
            _context = context;
            _query = _context.Vehicles;
        }

        public List<Vehicle> ToList(int organizationId)
        {
            return _query.ToList();
        }

        public async Task<List<Vehicle>> ToListAsync(int? organizationId)
        {
            return await _query.ToListAsync();
        }
    }
}
