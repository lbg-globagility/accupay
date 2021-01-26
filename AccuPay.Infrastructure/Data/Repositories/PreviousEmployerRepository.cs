using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class PreviousEmployerRepository : SavableRepository<PreviousEmployer>, IPreviousEmployerRepository
    {
        public PreviousEmployerRepository(PayrollContext context) : base(context)
        {
        }

        public async Task<ICollection<PreviousEmployer>> GetListByEmployeeAsync(int employeeId)
        {
            return await _context.PreviousEmployers
                .Where(l => l.EmployeeID == employeeId)
                .ToListAsync();
        }
    }
}
