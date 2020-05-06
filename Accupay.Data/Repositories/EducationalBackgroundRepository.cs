using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class EducationalBackgroundRepository
    {
        public async Task<IEnumerable<EducationalBackground>> GetListByEmployeeAsync(int employeeId)
        {
            using (var context = new PayrollContext())
            {
                return await context.EducationalBackgrounds.Where(l => l.EmployeeID == employeeId).ToListAsync();
            }
        }

        public async Task DeleteAsync(EducationalBackground educBg)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.EducationalBackgrounds.Remove(educBg);
                await context.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(EducationalBackground educBg)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.EducationalBackgrounds.Add(educBg);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(EducationalBackground educBg)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Entry(educBg).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
