using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class JobLevelRepository : IJobLevelRepository
    {
        private readonly PayrollContext _context;

        public JobLevelRepository(PayrollContext context)
        {
            _context = context;
        }

        public void Delete(JobLevel jobLevel)
        {
            _context.JobLevels.Remove(jobLevel);
            _context.SaveChanges();
        }

        public IEnumerable<JobLevel> GetAll(int organizationId)
        {
            return _context.JobLevels.
                            Where(x => x.OrganizationID == organizationId).
                            ToList();
        }

        public async Task<IEnumerable<JobLevel>> GetAllAsync(int organizationId)
        {
            return await _context.JobLevels.
                                Where(x => x.OrganizationID == organizationId).
                                ToListAsync();
        }
    }
}
