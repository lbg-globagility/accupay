using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class LoanTransactionRepository
    {
        public async Task<IEnumerable<LoanTransaction>> GetByPayPeriodAsync(int payPeriodId)
        {
            using (var context = new PayrollContext())
            {
                return await context.LoanTransactions.
                                Where(x => x.PayPeriodID == payPeriodId).
                                ToListAsync();
            }
        }
    }
}