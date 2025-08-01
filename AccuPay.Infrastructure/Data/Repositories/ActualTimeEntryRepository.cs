using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class ActualTimeEntryRepository : IActualTimeEntryRepository
    {
        private readonly PayrollContext _context;

        public ActualTimeEntryRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ActualTimeEntry>> GetByDatePeriodAsync(int organizationId, TimePeriod timePeriod)
        {
            return await _context.ActualTimeEntries
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => timePeriod.Start <= x.Date)
                .Where(x => x.Date <= timePeriod.End)
                .ToListAsync();
        }
    }
}
