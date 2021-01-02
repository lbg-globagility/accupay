using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ILeaveDataService : IBaseSavableDataService<Leave>
    {
        Task<decimal> ForceUpdateLeaveAllowanceAsync(int employeeId, int organizationId, int userId, LeaveType selectedLeaveType, decimal newAllowance);

        Task<PaginatedList<LeaveLedger>> GetLeaveBalancesAsync(PageOptions options, int organizationId, string searchTerm);

        Task<Leave> SaveAsync(Leave leave, int changedByUserId);

        Task SaveManyAsync(List<Leave> leaves, int changedByUserId);
    }
}
