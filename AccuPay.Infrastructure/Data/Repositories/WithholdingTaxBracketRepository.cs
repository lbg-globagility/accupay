using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class WithholdingTaxBracketRepository : IWithholdingTaxBracketRepository
    {
        private readonly PayrollContext _context;

        public WithholdingTaxBracketRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<ICollection<WithholdingTaxBracket>> GetAllAsync()
        {
            return await _context
                .WithholdingTaxBrackets
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
