using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class AwardRepository
    {
        public async Task<IEnumerable<Award>> GetListByEmployeeAsync(int employeeId)
        {
            using (var context = new PayrollContext())
            {
                return await context.Awards.Where(l => l.EmployeeID == employeeId).ToListAsync();
            }
        }

        public async Task DeleteAsync(Award award)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Awards.Attach(award);
                context.Awards.Remove(award);
                await context.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(Award award)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Awards.Add(award);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Award award)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Entry(award).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
