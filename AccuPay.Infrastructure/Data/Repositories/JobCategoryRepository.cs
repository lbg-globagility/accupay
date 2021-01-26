using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Infrastructure.Data
{
    public class JobCategoryRepository : IJobCategoryRepository
    {
        private readonly PayrollContext _context;

        public JobCategoryRepository(PayrollContext context)
        {
            _context = context;
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
