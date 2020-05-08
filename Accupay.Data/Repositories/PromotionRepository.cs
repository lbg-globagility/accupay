using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PromotionRepository
    {
        public async Task<IEnumerable<Promotion>> GetListByEmployeeAsync(int employeeId)
        {
            using (var context = new PayrollContext())
            {
                return await context.Promotions.Include(x => x.SalaryEntity).Where(l => l.EmployeeID == employeeId).ToListAsync();
            }
        }

        public async Task CreateAsync(Promotion promotion)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Promotions.Add(promotion);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Promotion promotion)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Entry(promotion).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Promotion promotion)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Promotions.Remove(promotion);
                await context.SaveChangesAsync();
            }
        }
    }
}
