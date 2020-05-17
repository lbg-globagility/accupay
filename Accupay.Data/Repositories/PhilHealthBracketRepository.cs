using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PhilHealthBracketRepository
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