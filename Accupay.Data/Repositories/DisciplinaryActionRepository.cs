using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class DisciplinaryActionRepository : SavableRepository<DisciplinaryAction>
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
