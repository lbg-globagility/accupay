using AccuPay.Core.Helpers;
using AccuPay.Core.ValueObjects;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ITimeEntryGenerator
    {
        Task<EmployeeResult> Start(int employeeId, ITimeEntryResources resources, int currentlyLoggedInUserId, TimePeriod payPeriod, bool isMorningSun = false);
    }
}
