using AccuPay.Core.Entities;
using System.Collections.Generic;

namespace AccuPay.Core.Interfaces
{
    public interface IJobCategoryRepository
    {
        JobCategory FindById(int id);

        IEnumerable<JobLevel> GetAll();
    }
}
