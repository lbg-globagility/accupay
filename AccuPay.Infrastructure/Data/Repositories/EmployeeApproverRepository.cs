using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class EmployeeApproverRepository : SavableRepository<EmployeeApprover>, IEmployeeApproverRepository
    {

        public EmployeeApproverRepository(PayrollContext context) : base(context)
        {
        }

        public async Task<ICollection<EmployeeApprover>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.Set<EmployeeApprover>()
                .Include(ea => ea.Approver)
                .Where(ea => ea.EmployeeID == employeeId)
                .ToListAsync();
        }
        public async Task DeleteManyAsync(IEnumerable<int> ids)
        {
            var empApprover = await _context.EmployeeApprovers
                .Where(x => ids.Contains(x.RowID.Value))
                .ToListAsync();

            _context.RemoveRange(empApprover);

            await _context.SaveChangesAsync();
        }
    }
}
