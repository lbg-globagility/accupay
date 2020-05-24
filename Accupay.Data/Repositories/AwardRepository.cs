using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class AwardRepository
    {
        private readonly PayrollContext _context;

        public AwardRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(Award award)
        {
            _context.Awards.Remove(award);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Award award)
        {
            _context.Awards.Add(award);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Award award)
        {
            _context.Entry(award).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Award>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.Awards.
                                Where(l => l.EmployeeID == employeeId).
                                ToListAsync();
        }
    }
}