using AccuPay.Core.Entities;
using AccuPay.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IShiftDataService : IBaseSavableDataService<Shift>
    {
        Task<BatchApplyResult<Shift>> BatchApply(IEnumerable<ShiftModel> shiftModels, int organizationId, int currentlyLoggedInUserId);
    }
}
