using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class WithholdingTaxBracketRepository
    {
        public async Task<IEnumerable<WithholdingTaxBracket>> GetAllAsync()
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.WithholdingTaxBrackets.ToListAsync();
            }
        }
    }
}