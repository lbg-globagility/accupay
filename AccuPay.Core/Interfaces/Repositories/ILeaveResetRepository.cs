using AccuPay.Core.Entities.LeaveReset;
using AccuPay.Core.ValueObjects;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces.Repositories
{
    public interface ILeaveResetRepository
    {
        Task<LeaveReset> GetByOrganizationIdAndDate(int organizationId, TimePeriod timePeriod);

        Task<ILeaveResetPolicy> GetLeaveResetPolicyAsync();
    }
}
