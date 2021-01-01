using AccuPay.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Core.Repositories
{
    public class RouteQueryBuilder
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