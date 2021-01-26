using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class LoanPaymentFromBonusRepository : ILoanPaymentFromBonusRepository
    {
        private readonly PayrollContext _context;

        public LoanPaymentFromBonusRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task SaveManyAsync(List<LoanPaymentFromBonus> loanPaymentFromBonuses)
        {
            var updated = loanPaymentFromBonuses.Where(e => e.Id != 0).ToList();
            if (updated.Any())
            {
                var deletables = updated.Where(x => x.AmountPayment == 0).ToList();
                deletables.ForEach(x => _context.Entry(x).State = EntityState.Deleted);

                var updateables = updated.Where(x => x.AmountPayment > 0).ToList();
                updateables.ForEach(x => _context.Entry(x).State = EntityState.Modified);
            }

            var added = loanPaymentFromBonuses.Where(e => e.Id == 0).ToList();
            if (added.Any())
            {
                _context.LoanPaymentFromBonuses.AddRange(added);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<LoanPaymentFromBonus>> GetByBonusIdAsync(int bonusId)
        {
            return await _context.LoanPaymentFromBonuses
                .Include(b => b.Items)
                .Where(b => b.BonusId == bonusId)
                .ToListAsync();
        }
    }
}
