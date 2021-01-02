using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class CertificationRepository : SavableRepository<Certification>, ICertificationRepository
    {
        public CertificationRepository(PayrollContext context) : base(context)
        {
        }

        public async Task<ICollection<Certification>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.Certifications
                .Where(l => l.EmployeeID == employeeId)
                .ToListAsync();
        }
    }
}
