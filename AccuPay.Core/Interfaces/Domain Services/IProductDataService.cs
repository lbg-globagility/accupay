using AccuPay.Core.Entities;
using AccuPay.Core.ValueObjects;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IProductDataService
    {
        Task<bool> CheckIfAlreadyUsedInAllowancesAsync(string allowanceType);

        Task<Allowance> GetOrCreateEmployeeEcola(int employeeId, int organizationId, int currentlyLoggedInUserId, TimePeriod timePeriod, string allowanceFrequency = "Semi-monthly", decimal amount = 0);
    }
}
