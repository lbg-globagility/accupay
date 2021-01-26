using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class PayFrequencyRepository : IPayFrequencyRepository
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
