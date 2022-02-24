using AccuPay.Core.Entities.LeaveReset;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;

namespace AccuPay.Infrastructure.Data
{
    public class LeavePolicy: ILeavePolicy
    {
        private readonly ListOfValueCollection _settings;

        public LeavePolicy(ListOfValueCollection settings)
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
