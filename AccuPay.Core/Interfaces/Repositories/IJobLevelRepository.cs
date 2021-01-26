using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IJobLevelRepository
    {
        void Delete(JobLevel jobLevel);

        IEnumerable<JobLevel> GetAll(int organizationId);

        Task<IEnumerable<JobLevel>> GetAllAsync(int organizationId);
    }
}
