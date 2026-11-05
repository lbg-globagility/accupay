using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IProjectEmployeeDataService : IBaseSavableDataService<ProjectEmployee>
    {
        Task<ICollection<ProjectEmployee>> GetAllByProjectIdAsync(int projectId);
    }
}
