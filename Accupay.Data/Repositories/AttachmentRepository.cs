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
        private readonly PayrollContext _context;

        public AttachmentRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attachment>> GetListByEmployeeAsync(int employeeId)
        {
            return await _context.Attachments.
                                Where(l => l.EmployeeID == employeeId).
                                ToListAsync();
        }

        public async Task DeleteAsync(Attachment attachment)
        {
            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Attachment attachment)
        {
            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();
        }
    }
}