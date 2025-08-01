using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Helpers;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ILeaveDataService : IBaseSavableDataService<Leave>
    {
        Task<decimal> ForceUpdateLeaveAllowanceAsync(int employeeId, int organizationId, int userId, LeaveType selectedLeaveType, decimal newAllowance);

        Task<PaginatedList<LeaveLedger>> GetLeaveBalancesAsync(PageOptions options, int organizationId, string searchTerm);
    }
}
