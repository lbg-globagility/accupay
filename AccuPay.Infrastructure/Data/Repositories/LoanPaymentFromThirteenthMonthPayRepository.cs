using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class LoanPaymentFromThirteenthMonthPayRepository : ILoanPaymentFromThirteenthMonthPayRepository
    {
        private readonly PayrollContext _context;

        public LoanPaymentFromThirteenthMonthPayRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task SaveManyAsync(List<LoanPaymentFromThirteenthMonthPay> LoanPaymentFromThirteenthMonthPays)
        {
            var updated = LoanPaymentFromThirteenthMonthPays.Where(e => e.Id != 0).ToList();
            if (updated.Any())
            {
                var deletables = updated.Where(x => x.AmountPayment == 0).ToList();
                deletables.ForEach(x => _context.Entry(x).State = EntityState.Deleted);

                var updateables = updated.Where(x => x.AmountPayment > 0).ToList();
                updateables.ForEach(x => _context.Entry(x).State = EntityState.Modified);
            }

            var added = LoanPaymentFromThirteenthMonthPays.Where(e => e.Id == 0).ToList();
            if (added.Any())
            {
                _context.LoanPaymentFromThirteenthMonthPays.AddRange(added);
            }

            await _context.SaveChangesAsync();
        }
    }
}
