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
        /// Deletes the leaves a self-service filing group no longer needs and saves the rest in a
        /// single transaction, so a failure while saving rolls back the deletes.
        /// </summary>
        Task ReplaceSelfServiceFilingGroupAsync(List<Leave> toDelete, List<Leave> toSave, int changedByUserId);
    }
}
