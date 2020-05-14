using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PositionViewQueryBuilder
    {
        private PayrollContext _context;
        private IQueryable<PositionView> _query;

        public PositionViewQueryBuilder(PayrollContext context)
        {
            _context = context;
            _query = _context.PositionViews;
        }

        public PositionViewQueryBuilder ByOrganizationId(int organizationId)
        {
            _query = _query.Where(u => u.OrganizationID == organizationId);
            return this;
        }

        public PositionViewQueryBuilder ByPositionId(int positionId)
        {
            _query = _query.Where(u => u.PositionID == positionId);
            return this;
        }

        public PositionViewQueryBuilder IncludeView()
        {
            _query = _query.Include(p => p.View);
            return this;
        }

        public PositionViewQueryBuilder IncludePosition()
        {
            _query = _query.Include(p => p.Position);
            return this;
        }

        public async Task<IEnumerable<PositionView>> ToListAsync()
        {
            return await _query.ToListAsync();
        }
    }
}