using AccuPay.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Repositories
{
    public class JobCategoryRepository
    {
        public IEnumerable<JobLevel> GetAll()
        {
            using (var context = new PayrollContext())
            {
                return context.JobLevels.ToList();
            }
        }

        public JobCategory FindById(int id)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.JobCategories.FirstOrDefault(x => x.RowID == id);
            }
        }
    }
}