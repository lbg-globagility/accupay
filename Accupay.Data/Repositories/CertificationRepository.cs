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
        public async Task<IEnumerable<Certification>> GetListByEmployeeAsync(int employeeId)
        {
            using (var context = new PayrollContext())
            {
                return await context.Certifications.Where(l => l.EmployeeID == employeeId).ToListAsync();
            }
        }

        public async Task DeleteAsync(Certification certification)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Certifications.Attach(certification);
                context.Certifications.Remove(certification);
                await context.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(Certification certification)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Certifications.Add(certification);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Certification certification)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Entry(certification).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
