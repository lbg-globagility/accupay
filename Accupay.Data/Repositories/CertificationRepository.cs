using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class CertificationRepository
    {
        private readonly PayrollContext _context;

        public CertificationRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(Certification certification)
        {
            _context.Certifications.Remove(certification);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Certification certification)
        {
            _context.Certifications.Add(certification);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Certification certification)
        {
            _context.Entry(certification).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Certification>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.Certifications.
                                Where(l => l.EmployeeID == employeeId).
                                ToListAsync();
        }
    }
}