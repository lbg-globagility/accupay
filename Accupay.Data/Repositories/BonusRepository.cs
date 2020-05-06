using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class BonusRepository
    {
        public async Task DeleteAsync(Bonus currentBonus)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Bonuses.Remove(currentBonus);
                await context.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(Bonus bonus)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Bonuses.Add(bonus);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Bonus bonus)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Entry(bonus).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public List<string> GetFrequencyList()
        {
            return new List<string>()
            {
                Bonus.FREQUENCY_ONE_TIME,
                Bonus.FREQUENCY_DAILY,
                Bonus.FREQUENCY_SEMI_MONTHLY,
                Bonus.FREQUENCY_MONTHLY
            };
        }

        public async Task<IEnumerable<Bonus>> GetByEmployeeAsync(int employeeId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Bonuses.Include(x => x.Product).Where(x => x.EmployeeID == employeeId).ToListAsync();
            }
        }
    }
}