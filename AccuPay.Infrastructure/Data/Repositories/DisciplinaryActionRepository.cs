using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class DisciplinaryActionRepository : SavableRepository<DisciplinaryAction>, IDisciplinaryActionRepository
    {
        public DisciplinaryActionRepository(PayrollContext context) : base(context)
        {
        }

        public async Task<ICollection<DisciplinaryAction>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.DisciplinaryActions
                .Include(x => x.Finding)
                .Where(l => l.EmployeeID == employeeId)
                .ToListAsync();
        }
    }
}
