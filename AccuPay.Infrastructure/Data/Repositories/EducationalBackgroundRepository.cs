using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class EducationalBackgroundRepository : SavableRepository<EducationalBackground>, IEducationalBackgroundRepository
    {
        public EducationalBackgroundRepository(PayrollContext context) : base(context)
        {
        }

        public async Task<ICollection<EducationalBackground>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.EducationalBackgrounds
                .Where(l => l.EmployeeID == employeeId)
                .ToListAsync();
        }
    }
}
