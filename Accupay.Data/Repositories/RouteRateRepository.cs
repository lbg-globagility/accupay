using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class RouteRateRepository
    {
        private readonly PayrollContext _context;

        public RouteRateRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<ICollection<RoutePayRate>> GetAll()
        {
            var routes = await _context.RoutePayRates.ToListAsync();

            return routes;
        }
    }
}
