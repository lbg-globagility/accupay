using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class WithholdingTaxBracketRepository
    {
        private readonly PayrollContext _context;

        public WithholdingTaxBracketRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WithholdingTaxBracket>> GetAllAsync()
        {
            return await _context.WithholdingTaxBrackets.ToListAsync();
        }
    }
}