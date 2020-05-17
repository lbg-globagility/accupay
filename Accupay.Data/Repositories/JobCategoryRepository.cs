using AccuPay.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Repositories
{
    public class JobCategoryRepository
    {
        private readonly PayrollContext _context;

        public JobCategoryRepository(PayrollContext context)
        {
            this._context = context;
        }

        public IEnumerable<JobLevel> GetAll()
        {
            return _context.JobLevels.ToList();
        }

        public JobCategory FindById(int id)
        {
            return _context.JobCategories.FirstOrDefault(x => x.RowID == id);
        }
    }
}