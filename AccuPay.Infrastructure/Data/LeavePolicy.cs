using AccuPay.Core.Entities.LeaveReset;
using AccuPay.Core.Interfaces;

namespace AccuPay.Infrastructure.Data
{
    public class LeavePolicy : ILeavePolicy
    {
        private readonly IListOfValueCollection _settings;

        public LeavePolicy(IListOfValueCollection settings)
        {
            _settings = settings;
        }

        public decimal GetLeavePrematureYear => _settings.GetDecimal("LeavePolicy.PrematureYear");
        public bool IsAllowedPrematureLeave => _settings.GetBoolean("LeavePolicy.AllowPrematureLeave");

        public BasisStartDateEnum AnniversaryDateBasis()
        {
            return _settings.GetEnum(name: "LeavePolicy.AnniversaryDateBasis",
                @default: BasisStartDateEnum.StartDate);
        }
    }
}
