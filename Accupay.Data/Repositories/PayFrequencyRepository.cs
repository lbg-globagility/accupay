using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PayFrequencyRepository
    {
        private readonly PayrollContext _context;

        public PayFrequencyRepository(PayrollContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<PayFrequency>> GetAllAsync()
        {
            return await _context.PayFrequencies.ToListAsync();
        }
    }
}