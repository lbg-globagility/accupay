using AccuPay.Core.Entities;
using AccuPay.Core.Services.Imports.Overtimes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IOvertimeDataService : IBaseSavableDataService<Overtime>
    {
        Task<List<Overtime>> BatchApply(IReadOnlyCollection<OvertimeImportModel> validRecords, int organizationId, int currentlyLoggedInUserId);

        Task DeleteManyAsync(IEnumerable<int> overtimeIds, int currentlyLoggedInUserId);

        Task GenerateOvertimeByShift(IEnumerable<IShift> modifiedShifts, List<int> employeeIds, int organizationId, int currentlyLoggedInUserId);
    }
}
