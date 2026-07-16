using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class EmployeeApproverRepository : IEmployeeApproverRepository
    {
        private readonly PayrollContext _context;

        public EmployeeApproverRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<ICollection<EmployeeApprover>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.Set<EmployeeApprover>()
                .Include(ea => ea.Approver)
                .Where(ea => ea.EmployeeID == employeeId)
                .ToListAsync();
        }
    }
}
