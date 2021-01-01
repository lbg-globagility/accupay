using AccuPay.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Repositories
{
    public class BreakTimeBracketRepository
    {
        private readonly PayrollContext _context;

        public BreakTimeBracketRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<ICollection<BreakTimeBracket>> GetAllAsync(int organizationId)
        {
            return await _context.BreakTimeBrackets
                .Include(x => x.Division)
                .Where(x => x.Division.OrganizationID == organizationId)
                .ToListAsync();
        }
    }
}
