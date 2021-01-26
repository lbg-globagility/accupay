using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class PhilHealthBracketRepository : IPhilHealthBracketRepository
    {
        private readonly PayrollContext _context;

        public PhilHealthBracketRepository(PayrollContext context)
        {
            _context = context;
        }

        public IEnumerable<PhilHealthBracket> GetAll()
        {
            return _context.PhilHealthBrackets.ToList();
        }

        public async Task<IEnumerable<PhilHealthBracket>> GetAllAsync()
        {
            return await _context.PhilHealthBrackets.ToListAsync();
        }
    }
}
