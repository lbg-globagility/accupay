using AccuPay.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Repositories
{
    public class RouteRepository
    {
        private readonly PayrollContext _context;

        public RouteRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Route>> GetAll()
        {
            var routes = await _context.Routes.ToListAsync();

            return routes;
        }

        public async Task<IEnumerable<Route>> GetAllAsync(int organizationId)
        {
            var builder = new RouteQueryBuilder(_context);
            return await builder.
                ToListAsync(organizationId);
        }
    }
}