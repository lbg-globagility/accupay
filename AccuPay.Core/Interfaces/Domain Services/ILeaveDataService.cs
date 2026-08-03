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

        /// <summary>
        /// Deletes an existing self-service filing group and saves its replacement leaves in a
        /// single transaction, so a failure creating the replacement rolls back the deletes.
        /// </summary>
        Task ReplaceSelfServiceFilingGroupAsync(List<Leave> toDelete, List<Leave> toCreate, int changedByUserId);
    }
}
