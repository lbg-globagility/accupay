using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class AwardRepository : SavableRepository<Award>, IAwardRepository
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
