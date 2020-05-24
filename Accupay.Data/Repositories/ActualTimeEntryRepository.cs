using AccuPay.Data.Entities;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class ActualTimeEntryRepository
    {
        private readonly PayrollContext _context;

        public ActualTimeEntryRepository(PayrollContext context)
        {
            _context = context;
        }

        public IEnumerable<ActualTimeEntry> GetByDatePeriod(int organizationId, TimePeriod timePeriod)
        {
            return CreateBaseQueryByDatePeriod(organizationId, timePeriod).
                    ToList();
        }

        public async Task<IEnumerable<ActualTimeEntry>> GetByDatePeriodAsync(int organizationId, TimePeriod timePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, timePeriod).
                        ToListAsync();
        }

        private IQueryable<ActualTimeEntry> CreateBaseQueryByDatePeriod(int organizationId,
                                                                    TimePeriod timePeriod)
        {
            return _context.ActualTimeEntries.
                    Where(x => x.OrganizationID == organizationId).
                    Where(x => timePeriod.Start <= x.Date).
                    Where(x => x.Date <= timePeriod.End);
        }
    }
}