using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class JobLevelRepository
    {
        public IEnumerable<JobLevel> GetAll(int organizationId)
        {
            using (var context = new PayrollContext())
            {
                return context.JobLevels.
                                Where(x => x.OrganizationID == organizationId).
                                ToList();
            }
        }

        public async Task<IEnumerable<JobLevel>> GetAllAsync(int organizationId)
        {
            using (var context = new PayrollContext())
            {
                return await context.JobLevels.
                                        Where(x => x.OrganizationID == organizationId).
                                        ToListAsync();
            }
        }

        public void Delete(JobLevel jobLevel)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.JobLevels.Remove(jobLevel);
                context.SaveChanges();
            }
        }
    }
}