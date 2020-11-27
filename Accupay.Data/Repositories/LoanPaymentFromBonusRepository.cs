using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class LoanPaymentFromBonusRepository
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
    }
}
