using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PromotionRepository
    {
        private readonly PayrollContext _context;

        public PromotionRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Promotion>> GetListByEmployeeAsync(int employeeId)
        {
            return await _context.Promotions.
                                Include(x => x.SalaryEntity).
                                Where(l => l.EmployeeID == employeeId).
                                ToListAsync();
        }

        public async Task CreateAsync(Promotion promotion)
        {
            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Promotion promotion)
        {
            _context.Entry(promotion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Promotion promotion)
        {
            _context.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();
        }
    }
}