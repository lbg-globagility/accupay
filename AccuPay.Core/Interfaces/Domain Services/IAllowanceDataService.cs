using AccuPay.Core.Entities;
using AccuPay.Core.Services.Imports.Allowances;
using System;
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
            DateTime startDate,
            DateTime? endDate = null,
            string allowanceFrequency = Allowance.FREQUENCY_DAILY,
            decimal amount = 0);

        Task<bool> CheckIfAlreadyUsedInClosedPayPeriodAsync(int allowanceId);

        Task<Allowance> CreateEcola(
            int employeeId,
            int organizationId,
            int currentlyLoggedInUserId,
            DateTime startDate,
            string allowanceFrequency,
            decimal amount);
    }
}
