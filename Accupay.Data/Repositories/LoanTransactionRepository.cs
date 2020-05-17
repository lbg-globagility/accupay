using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class LoanTransactionRepository
    {
        private readonly PayrollContext _context;

        public LoanTransactionRepository(PayrollContext context)
        {
            _context = context;
        }

        // TODO: this should be fetched from paystub
        public async Task<IEnumerable<LoanTransaction>> GetByPayPeriodAsync(int payPeriodId)
        {
            return await _context.LoanTransactions.
                            Where(x => x.PayPeriodID == payPeriodId).
                            ToListAsync();
        }
    }
}