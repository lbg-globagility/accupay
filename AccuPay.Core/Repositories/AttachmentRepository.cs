using AccuPay.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Repositories
{
    public class AttachmentRepository : SavableRepository<Attachment>
    {
        public AttachmentRepository(PayrollContext context) : base(context)
        {
        }

        public async Task<ICollection<Attachment>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.Attachments
                .Where(l => l.EmployeeID == employeeId)
                .ToListAsync();
        }
    }
}
