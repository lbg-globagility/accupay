using AccuPay.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Repositories
{
    public class AwardRepository : SavableRepository<Award>
    {
        public AwardRepository(PayrollContext context) : base(context)
        {
        }

        public async Task<ICollection<Award>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.Awards
                .Where(l => l.EmployeeID == employeeId)
                .ToListAsync();
        }
    }
}
