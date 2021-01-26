using AccuPay.Core.Entities;
using AccuPay.Core.Services.Imports.Salaries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ISalaryDataService : IBaseSavableDataService<Salary>
    {
        Task<List<Salary>> BatchApply(IReadOnlyCollection<SalaryImportModel> validRecords, int organizationId, int currentlyLoggedInUserId);
    }
}
