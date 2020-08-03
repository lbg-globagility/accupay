using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
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
    }
}
