using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IEmployeeApproverRepository : ISavableRepository<EmployeeApprover>
    {
        Task<ICollection<EmployeeApprover>> GetByEmployeeIdAsync(int employeeId);

        Task<EmployeeApprover> GetByIdAsync(int id);

        Task DeleteManyAsync(IEnumerable<int> ids);
    }
}
