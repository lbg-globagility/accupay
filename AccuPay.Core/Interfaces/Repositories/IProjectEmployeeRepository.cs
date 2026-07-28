using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IProjectEmployeeRepository : ISavableRepository<ProjectEmployee>
    {
        Task<ICollection<ProjectEmployee>> GetAllByProjectIdAsync(int projectId);
    }
}
