using AccuPay.Core.Entities.LeaveReset;
using AccuPay.Core.Services.LeaveBalanceReset;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ILeaveBalanceResetCalculator
    {
        Task<LeaveResetResult> Start(int organizationId,
            int userId,
            int employeeId,
            LeaveReset leaveReset,
            ILeaveResetResources resources);
    }
}
