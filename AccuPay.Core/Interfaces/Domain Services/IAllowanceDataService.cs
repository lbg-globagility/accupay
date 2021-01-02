using AccuPay.Core.Entities;
using AccuPay.Core.Services.Imports.Allowances;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IAllowanceDataService : IBaseSavableDataService<Allowance>
    {
        Task BatchApply(IReadOnlyCollection<AllowanceImportModel> validRecords, int organizationId, int currentlyLoggedInUserId);

        Task<bool> CheckIfAlreadyUsedInClosedPayPeriodAsync(int allowanceId);
    }
}
