using AccuPay.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Core.Repositories
{
    public class VehicleQueryBuilder
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