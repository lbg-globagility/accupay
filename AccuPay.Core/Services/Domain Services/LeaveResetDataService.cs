using AccuPay.Core.Entities.LeaveReset;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Interfaces.Domain_Services;
using AccuPay.Core.Interfaces.Repositories;
using AccuPay.Core.ValueObjects;
using System.Threading.Tasks;

namespace AccuPay.Core.Services.Domain_Services
{
    public class LeaveResetDataService : ILeaveResetDataService
    {
        private readonly ILeaveResetRepository _leaveResetRepository;

        public LeaveResetDataService(ILeaveResetRepository leaveResetRepository)
        {
            _leaveResetRepository = leaveResetRepository;
        }

        public async Task<LeaveReset> GetByOrganizationIdAndDate(int organizationId, TimePeriod timePeriod)
        {
            return await _leaveResetRepository.GetByOrganizationIdAndDate(organizationId: organizationId, timePeriod: timePeriod);
        }

        public async Task<ILeaveResetPolicy> GetLeaveResetPolicy()
        {
            return await _leaveResetRepository.GetLeaveResetPolicyAsync();
        }
    }
}
