using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class EducationalBackgroundRepository
    {
        private readonly PayrollContext _context;

        public EducationalBackgroundRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EducationalBackground>> GetListByEmployeeAsync(int employeeId)
        {
            return await _context.EducationalBackgrounds.
                                Where(l => l.EmployeeID == employeeId).
                                ToListAsync();
        }

        public async Task DeleteAsync(EducationalBackground educBg)
        {
            _context.EducationalBackgrounds.Remove(educBg);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(EducationalBackground educBg)
        {
            _context.EducationalBackgrounds.Add(educBg);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EducationalBackground educBg)
        {
            _context.Entry(educBg).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}