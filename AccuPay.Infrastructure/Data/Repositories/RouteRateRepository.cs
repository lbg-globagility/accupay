using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class RouteRateRepository : IRouteRateRepository
    {
        private readonly PayrollContext _context;

        public RouteRateRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<ICollection<RoutePayRate>> GetAll(int? routeId)
        {
            var routeRates = await _context.RoutePayRates
                .Where(t => t.RouteID == routeId)
                .ToListAsync();

            return routeRates;
        }

        public async Task<ICollection<RoutePayRate>> GetAllAsync()
        {
            var routeRates = await _context.RoutePayRates.ToListAsync();

            return routeRates;
        }

        public async Task SaveMany(ICollection<RoutePayRate> routeRates)
        {
            var added = routeRates.Where(t => t.IsNew);
            var updated = routeRates.Where(t => !t.IsNew);

            added.ToList().ForEach(t => _context.RoutePayRates.Add(t));
            updated.ToList().ForEach(t => _context.Entry(t).State = EntityState.Modified);

            await _context.SaveChangesAsync();
        }
    }
}
