using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class SoloParentBeneficiaryRepository : SavableRepository<SoloParentBeneficiary>, ISoloParentBeneficiaryRepository
    {
        public SoloParentBeneficiaryRepository(PayrollContext context) : base(context)
        {
        }

        public async Task<ICollection<SoloParentBeneficiary>> GetAllByOrganizationIdAsync(int orgId)
            => await _context.SoloParentBeneficiaries
                .AsNoTracking()
                .Include(spb => spb.Employee)
                .Where(spb => spb.OrganizationID == orgId)
                .ToListAsync();

        public async Task<SoloParentBeneficiary> GetByEmployeeIdAsync(int employeeId)
            => await _context.SoloParentBeneficiaries
                .AsNoTracking()
                .FirstOrDefaultAsync(spb => spb.EmployeeId == employeeId);

        public async Task<bool> IsEmployeeBeneficiaryAsync(int employeeId)
            => await GetByEmployeeIdAsync(employeeId) != null;
    }
}
