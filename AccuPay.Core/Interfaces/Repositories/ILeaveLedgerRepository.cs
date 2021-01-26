using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ILeaveLedgerRepository
    {
        Task CreateBeginningBalanceAsync(int employeeId, int leaveTypeId, int userId, int organizationId, decimal balance);

        Task<ICollection<LeaveLedger>> GetAllByEmployee(int? employeeId);

        Task<IEnumerable<LeaveLedger>> GetLeaveBalancesAsync(int organizationId, string searchTerm = null);

        Task<ICollection<LeaveTransaction>> GetTransactionsByLedger(int? leaveLedgerId);

        Task<PaginatedList<LeaveTransaction>> ListTransactionsAsync(PageOptions options, int organizationId, int id, string type = null);
    }
}
