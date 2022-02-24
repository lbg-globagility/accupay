using AccuPay.Core.Entities.LeaveReset;

namespace AccuPay.Core.Interfaces
{
    public interface ILeavePolicy
    {
        decimal GetLeavePrematureYear { get; }
        bool IsAllowedPrematureLeave { get; }
        BasisStartDateEnum AnniversaryDateBasis();
    }
}
