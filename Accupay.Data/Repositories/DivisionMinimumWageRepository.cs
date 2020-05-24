using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class DivisionMinimumWageRepository
    {
        private readonly PayrollContext _context;

        public DivisionMinimumWageRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DivisionMinimumWage>> GetByDateAsync(int organizationId,
                                                                            DateTime date)
        {
            return await _context.DivisionMinimumWages.
                            Where(t => t.OrganizationID == organizationId).
                            Where(t => t.EffectiveDateFrom <= date).
                            Where(t => date <= t.EffectiveDateTo).
                            ToListAsync();
        }
    }
}