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
        public IEnumerable<ActualTimeEntry> GetByDatePeriod(int organizationId, TimePeriod timePeriod)
        {
            using (var context = new PayrollContext())
            {
                return CreateBaseQueryByDatePeriod(organizationId, timePeriod, context).
                        ToList();
            }
        }

        public async Task<IEnumerable<ActualTimeEntry>> GetByDatePeriodAsync(int organizationId, TimePeriod timePeriod)
        {
            using (var context = new PayrollContext())
            {
                return await CreateBaseQueryByDatePeriod(organizationId, timePeriod, context).
                            ToListAsync();
            }
        }

        private IQueryable<ActualTimeEntry> CreateBaseQueryByDatePeriod(int organizationId,
                                                                    TimePeriod timePeriod,
                                                                    PayrollContext context)
        {
            return context.ActualTimeEntries.
                    Where(x => x.OrganizationID == organizationId).
                    Where(x => timePeriod.Start <= x.Date).
                    Where(x => x.Date <= timePeriod.End);
        }
    }
}