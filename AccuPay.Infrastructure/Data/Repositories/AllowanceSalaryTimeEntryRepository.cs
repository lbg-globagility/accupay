using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data.Repositories
{
    public class AllowanceSalaryTimeEntryRepository : SavableRepository<AllowanceSalaryTimeEntry>, IAllowanceSalaryTimeEntryRepository
    {
        public AllowanceSalaryTimeEntryRepository(PayrollContext context) : base(context)
        {

        }

        public async Task<ICollection<AllowanceSalaryTimeEntry>> GetByDatePeriodAsync(int organizationId, TimePeriod timePeriod) => await _context.AllowanceSalaryTimeEntries
            .Where(x => x.OrganizationID == organizationId)
            .Where(x => timePeriod.Start <= x.Date)
            .Where(x => x.Date <= timePeriod.End)
            .ToListAsync();
    }
}
