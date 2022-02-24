using AccuPay.Core.Entities.LeaveReset;

namespace AccuPay.Core.Interfaces
{
    public interface ILeaveResetPolicy
    {
        bool IsLeaveResetEnable { get; }
        LeaveResetBaseScheme GetLeaveResetBaseScheme(int? organizationId = null);
    }
}
