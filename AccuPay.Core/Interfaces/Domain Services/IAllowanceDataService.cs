using AccuPay.Core.Entities;
using AccuPay.Core.Services.Imports.Allowances;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IAllowanceDataService : IBaseSavableDataService<Allowance>
    {
        Task BatchApply(IReadOnlyCollection<AllowanceImportModel> validRecords, int organizationId, int currentlyLoggedInUserId);

        Task<Allowance> GetOrCreateEmployeeEcola(
            int employeeId,
            int organizationId,
            int currentlyLoggedInUserId,
            TimePeriod timePeriod,
            string allowanceFrequency = Allowance.FREQUENCY_SEMI_MONTHLY,
            decimal amount = 0);

        Task<bool> CheckIfAlreadyUsedInClosedPayPeriodAsync(int allowanceId);
    }
}
