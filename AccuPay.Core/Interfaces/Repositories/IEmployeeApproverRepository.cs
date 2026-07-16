using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IEmployeeApproverRepository
    {
        Task<ICollection<EmployeeApprover>> GetByEmployeeIdAsync(int employeeId);
    }
}
