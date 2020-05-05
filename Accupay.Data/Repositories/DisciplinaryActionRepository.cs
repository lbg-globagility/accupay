using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class DisciplinaryActionRepository
    {
        public async Task<IEnumerable<DisciplinaryAction>> GetListByEmployeeAsync(int employeeId)
        {
            using (var context = new PayrollContext())
            {
                return await context.DisciplinaryActions.Include(x => x.Finding).Where(l => l.EmployeeID == employeeId).ToListAsync();
            }
        }

        public async Task DeleteAsync(DisciplinaryAction disciplinaryAction)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.DisciplinaryActions.Remove(disciplinaryAction);
                await context.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(DisciplinaryAction disciplinaryAction)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.DisciplinaryActions.Add(disciplinaryAction);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(DisciplinaryAction disciplinaryAction)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Entry(disciplinaryAction).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
