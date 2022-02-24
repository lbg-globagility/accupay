using AccuPay.Core.Entities.LeaveReset;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;

namespace AccuPay.Infrastructure.Data
{
    public class LeaveResetPolicy: ILeaveResetPolicy
    {
        private const string LEAVE_RESET_POLICY = "LeaveResetPolicy";
        private readonly ListOfValueCollection _settings;

        public LeaveResetPolicy(ListOfValueCollection settings)
        {
            _settings = settings;
        }

        public bool IsLeaveResetEnable => _settings.GetBoolean($"{LEAVE_RESET_POLICY}.Enable");
        public LeaveResetBaseScheme GetLeaveResetBaseScheme(int? organizationId = null)
        {
            return _settings.GetEnum(name: $"{LEAVE_RESET_POLICY}.BaseScheme",
                @default: LeaveResetBaseScheme.EmployeeProfile,
                findByOrganization: false,
                organizationId: organizationId);
        }
    }
}
