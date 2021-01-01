using AccuPay.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Repositories
{
    public class PayFrequencyRepository
    {
        private readonly PayrollContext _context;

        public PayFrequencyRepository(PayrollContext context)
        {
            _context = context;
        }

        public ICollection<PayFrequency> GetAll()
        {
            return _context.PayFrequencies.ToList();
        }

        public async Task<ICollection<PayFrequency>> GetAllAsync()
        {
            return await _context.PayFrequencies.ToListAsync();
        }
    }
}