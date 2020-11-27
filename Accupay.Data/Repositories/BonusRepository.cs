using AccuPay.Data.Entities;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class BonusRepository
    {
        private readonly PayrollContext _context;

        public BonusRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(Bonus currentBonus)
        {
            _context.Bonuses.Remove(currentBonus);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Bonus bonus)
        {
            _context.Bonuses.Add(bonus);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Bonus bonus)
        {
            _context.Entry(bonus).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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

        public async Task<IEnumerable<Bonus>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.Bonuses
                .Include(x => x.Product)
                .Where(x => x.EmployeeID == employeeId)
                .ToListAsync();
        }

        public async Task<ICollection<Bonus>> GetByPayPeriodAsync(int organizationId, TimePeriod timePeriod)
        {
            return await CreateBaseQueryByTimePeriod(organizationId, timePeriod).
                ToListAsync();
        }

        public async Task<ICollection<Bonus>> GetByEmployeeAndPayPeriodAsync(int organizationId, int employeeId, TimePeriod timePeriod)
        {
            return await CreateBaseQueryByTimePeriod(organizationId, timePeriod)
                .Where(b => b.EmployeeID == employeeId)
                .ToListAsync();
        }

        public async Task<ICollection<Bonus>> GetByEmployeeAndPayPeriodForLoanPaymentAsync(int organizationId, int employeeId, TimePeriod timePeriod)
        {
            return await CreateBaseQueryByTimePeriod(organizationId, timePeriod)
                .Include(b => b.LoanPaymentFromBonuses)
                    .ThenInclude(l => l.LoanSchedule)
                .Include(b => b.LoanPaymentFromBonuses)
                    .ThenInclude(l => l.Items)
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
    }
}
