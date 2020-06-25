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
            return await _context.Bonuses.
                                    Include(x => x.Product).
                                    Where(x => x.EmployeeID == employeeId).
                                    ToListAsync();
        }

        public async Task<IEnumerable<Bonus>> GetBetweenDatesByEmployeeId(int employeeId, DateTime startDate)
        {
            var paystub = await _context.Paystubs.
                Include(p => p.PayPeriod).
                Where(p => p.EmployeeID == employeeId).
                OrderByDescending(p => p.PayPeriod.Year).
                ThenByDescending(p => p.PayPeriod.OrdinalValue).
                FirstOrDefaultAsync();

            var payperiod = paystub.PayPeriod;

            var remainingPeriods = await _context.PayPeriods.
                Where(pp => pp.OrganizationID == payperiod.OrganizationID).
                Where(pp => pp.Year == payperiod.Year).
                Where(pp => pp.PayFrequencyID == payperiod.PayFrequencyID).
                Where(pp => pp.OrdinalValue > payperiod.OrdinalValue).
                ToListAsync();

            var minDate = remainingPeriods.Min(pp => pp.PayFromDate);
            var maxDate = remainingPeriods.Max(pp => pp.PayToDate);

            var bonuses = await _context.Bonuses.
                Where(b => b.EmployeeID == employeeId).
                Where(b => b.EffectiveEndDate >= minDate).
                Where(b => b.EffectiveEndDate <= maxDate).
                ToListAsync();

            return bonuses;
        }

        public async Task<IEnumerable<Bonus>> GetBetweenDatesNotFromIDs(int employeeId, DateTime startDate, int[] bonusIds)
        {
            var paystub = await _context.Paystubs.
                Include(p => p.PayPeriod).
                Where(p => p.EmployeeID == employeeId).
                OrderByDescending(p => p.PayPeriod.Year).
                ThenByDescending(p => p.PayPeriod.OrdinalValue).
                FirstOrDefaultAsync();

            var payperiod = paystub.PayPeriod;

            var remainingPeriods = await _context.PayPeriods.
                Where(pp => pp.OrganizationID == payperiod.OrganizationID).
                Where(pp => pp.Year == payperiod.Year).
                Where(pp => pp.PayFrequencyID == payperiod.PayFrequencyID).
                Where(pp => pp.OrdinalValue > payperiod.OrdinalValue).
                ToListAsync();

            var minDate = remainingPeriods.Min(pp => pp.PayFromDate);
            var maxDate = remainingPeriods.Max(pp => pp.PayToDate);

            var bonuses = await _context.Bonuses.
                Where(b => !bonusIds.Contains(b.RowID.Value)).
                Where(b => b.EmployeeID == employeeId).
                Where(b => b.EffectiveEndDate >= minDate).
                Where(b => b.EffectiveEndDate <= maxDate).
                ToListAsync();

            return bonuses;
        }

        public async Task<ICollection<Bonus>> GetByPayPeriodAsync(int organizationId, TimePeriod timePeriod)
        {
            return await CreateBaseQueryByTimePeriod(organizationId, timePeriod).
                ToListAsync();
        }

        private IQueryable<Bonus> CreateBaseQueryByTimePeriod(int organizationId,
                                                                TimePeriod timePeriod)
        {
            return _context.Bonuses.
                Include(a => a.Product).
                Where(a => a.OrganizationID == organizationId).
                Where(a => a.EffectiveStartDate <= timePeriod.End).
                Where(a => a.EffectiveEndDate == null ? true : timePeriod.Start <= a.EffectiveEndDate);
        }
    }
}