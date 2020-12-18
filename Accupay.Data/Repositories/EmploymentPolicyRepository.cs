using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class EmploymentPolicyRepository
    {
        private readonly PayrollContext _context;

        public EmploymentPolicyRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<EmploymentPolicy> GetById(int employmentPolicyId)
        {
            var employmentPolicy = await _context.EmploymentPolicies
                .Include(t => t.Items)
                    .ThenInclude(t => t.Type)
                .FirstOrDefaultAsync(t => t.Id == employmentPolicyId);

            return employmentPolicy;
        }

        public async Task<ICollection<EmploymentPolicy>> GetAllAsync()
        {
            var employmentPolicies = await _context.EmploymentPolicies
                .Include(t => t.Items)
                    .ThenInclude(t => t.Type)
                .ToListAsync();

            return employmentPolicies;
        }

        public async Task<ICollection<EmploymentPolicyType>> GetAllTypes()
        {
            return await _context.EmploymentPolicyTypes.ToListAsync();
        }

        public async Task Create(EmploymentPolicy employmentPolicy)
        {
            _context.EmploymentPolicies.Add(employmentPolicy);

            // Detach the EmployeePolicyTypes as they're not supposed to saved alongside the employee policy.
            employmentPolicy.Items.ToList().ForEach(t => _context.Entry(t.Type).State = EntityState.Detached);

            await _context.SaveChangesAsync();
        }

        public async Task Update(EmploymentPolicy employmentPolicy)
        {
            _context.Entry(employmentPolicy).State = EntityState.Modified;

            employmentPolicy.Items.Where(t => t.IsNew).ToList().ForEach(t => _context.Entry(t).State = EntityState.Added);
            employmentPolicy.Items.Where(t => !t.IsNew).ToList().ForEach(t => _context.Entry(t).State = EntityState.Modified);

            await _context.SaveChangesAsync();
        }

        public async Task<(ICollection<EmploymentPolicy> employmentPolicies, int total)> List(PageOptions options)
        {
            var query = _context.EmploymentPolicies.AsQueryable();

            var employmentPolicies = await query.Page(options).ToListAsync();
            var total = await query.CountAsync();

            return (employmentPolicies, total);
        }
    }
}
