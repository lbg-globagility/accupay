using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class RouteQueryBuilder : IRouteQueryBuilder
    {
        private readonly PayrollContext _context;

        private IQueryable<Route> _query;

        public RouteQueryBuilder(PayrollContext context)
        {
            _context = context;
            _query = _context.Routes;
        }

        public List<Route> ToList(int organizationId)
        {
            return _query.ToList();
        }

        public async Task<List<Route>> ToListAsync(int? organizationId)
        {
            return await _query.ToListAsync();
        }
    }
}
