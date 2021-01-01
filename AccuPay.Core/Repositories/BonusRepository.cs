using AccuPay.Core.Entities;
using AccuPay.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Repositories
{
    public class BonusRepository : SavableRepository<Bonus>
    {
        public BonusRepository(PayrollContext context) : base(context)
        {
        }

        public override async Task<Bonus> GetByIdAsync(int id)
        {
            return await _context.Bonuses
                .AsNoTracking()
                .Include(b => b.LoanPaymentFromBonuses)
                    .ThenInclude(lb => lb.Items)
                .Where(b => b.RowID == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<Bonus>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.Bonuses
                .Include(x => x.Product)
                .Where(x => x.EmployeeID == employeeId)
                .ToListAsync();
        }

        public async Task<ICollection<Bonus>> GetByPayPeriodAsync(int organizationId, TimePeriod timePeriod)
        {
            return await CreateBaseQueryByTimePeriod(organizationId, timePeriod)
                .ToListAsync();
        }

        public async Task<ICollection<Bonus>> GetByEmployeeAndPayPeriodAsync(int organizationId, int employeeId, TimePeriod timePeriod)
        {
            return await CreateBaseQueryByTimePeriod(organizationId, timePeriod)
                .Where(b => b.EmployeeID == employeeId)
                .ToListAsync();
        }

        public async Task<ICollection<Bonus>> GetByEmployeeAndPayPeriodForLoanPaymentAsync(int organizationId, int employeeId, TimePeriod timePeriod)
        {
            return await _context.Bonuses
                .AsNoTracking()
                .Include(b => b.Product)
                .Include(b => b.LoanPaymentFromBonuses)
                    .ThenInclude(l => l.Loan)
                .Include(b => b.LoanPaymentFromBonuses)
                    .ThenInclude(l => l.Items)
                .Where(b => b.OrganizationID == organizationId)
                //.Where(b => timePeriod.Start <= b.EffectiveStartDate).Where(b => b.EffectiveStartDate <= timePeriod.End)
                .Where(b => (timePeriod.Start <= b.EffectiveStartDate && b.EffectiveStartDate <= timePeriod.End) ||
                    (timePeriod.Start <= b.EffectiveEndDate && b.EffectiveEndDate <= timePeriod.End))
                .Where(b => b.EmployeeID == employeeId)
                .Where(b => b.BonusAmount > 0)
                .ToListAsync();
        }

        private IQueryable<Bonus> CreateBaseQueryByTimePeriod(int organizationId, TimePeriod timePeriod)
        {
            return _context.Bonuses
                .Include(a => a.Product)
                .Where(a => a.OrganizationID == organizationId)
                .Where(a => a.EffectiveStartDate <= timePeriod.End)
                .Where(a => a.EffectiveEndDate == null ? true : timePeriod.Start <= a.EffectiveEndDate);
        }

        public List<string> GetFrequencyList()
        {
            return new List<string>()
            {
                Bonus.FREQUENCY_ONE_TIME,
                Bonus.FREQUENCY_DAILY,
                Bonus.FREQUENCY_SEMI_MONTHLY,
                Bonus.FREQUENCY_MONTHLY
            };
        }
    }
}
