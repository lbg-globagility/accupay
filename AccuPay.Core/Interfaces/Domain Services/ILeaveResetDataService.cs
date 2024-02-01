using AccuPay.Core.Entities.LeaveReset;
using AccuPay.Core.ValueObjects;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces.Domain_Services
{
    public interface ILeaveResetDataService
    {
        Task<LeaveReset> GetByOrganizationIdAndDate(int organizationId, TimePeriod timePeriod);
        Task<ILeaveResetPolicy> GetLeaveResetPolicy();
    }
}
