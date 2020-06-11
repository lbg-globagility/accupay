using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
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
            var employmentPolicy = await _context.EmploymentPolicies.FindAsync(employmentPolicyId);

            return employmentPolicy;
        }

        public async Task Create(EmploymentPolicy employmentPolicy)
        {
            _context.EmploymentPolicies.Add(employmentPolicy);
            await _context.SaveChangesAsync();
        }

        public async Task Update(EmploymentPolicy employmentPolicy)
        {
            _context.Entry(employmentPolicy).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
