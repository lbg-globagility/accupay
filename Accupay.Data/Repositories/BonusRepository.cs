using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class BonusRepository
    {
        private readonly PayrollContext _context;

        public BonusRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(Bonus currentBonus)
        {
            _context.Bonuses.Remove(currentBonus);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Bonus bonus)
        {
            _context.Bonuses.Add(bonus);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Bonus bonus)
        {
            _context.Entry(bonus).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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
            return await _context.Bonuses.
                                    Include(x => x.Product).
                                    Where(x => x.EmployeeID == employeeId).
                                    ToListAsync();
        }
    }
}