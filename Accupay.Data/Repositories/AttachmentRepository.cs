using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class AttachmentRepository
    {
        public async Task<IEnumerable<Attachment>> GetListByEmployeeAsync(int employeeId)
        {
            using (var context = new PayrollContext())
            {
                return await context.Attachments.Where(l => l.EmployeeID == employeeId).ToListAsync();
            }
        }

        public async Task DeleteAsync(Attachment attachment)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Attachments.Remove(attachment);
                await context.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(Attachment attachment)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Attachments.Add(attachment);
                await context.SaveChangesAsync();
            }
        }
    }
}
