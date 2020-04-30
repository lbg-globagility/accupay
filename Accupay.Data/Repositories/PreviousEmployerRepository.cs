using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PreviousEmployerRepository
    {
        public async Task<IEnumerable<PreviousEmployer>> GetListByEmployeeAsync(int employeeId)
        {
            using (var context = new PayrollContext())
            {
                return await context.PreviousEmployers.Where(l => l.EmployeeID == employeeId).ToListAsync();
            }
        }

        public async Task DeleteAsync(PreviousEmployer previousEmployer)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.PreviousEmployers.Remove(previousEmployer);
                await context.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(PreviousEmployer previousEmployer)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.PreviousEmployers.Add(previousEmployer);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(PreviousEmployer previousEmployer)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Entry(previousEmployer).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
