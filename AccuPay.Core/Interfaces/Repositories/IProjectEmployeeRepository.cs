using AccuPay.Core.Entities;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IProjectEmployeeRepository : ISavableRepository<ProjectEmployee>
    {
        Task<ICollection<ProjectEmployee>> GetAllByProjectIdAsync(int projectId);

        /// <summary>
        /// Gets the ids of the organization's employees that are assigned to a contractor's project
        /// during the time period. A blank begin or end date on the assignment or on the project
        /// means it is open-ended.
        /// </summary>
        Task<ICollection<int>> GetContractorEmployeeIdsAsync(int organizationId, TimePeriod timePeriod);
    }
}
