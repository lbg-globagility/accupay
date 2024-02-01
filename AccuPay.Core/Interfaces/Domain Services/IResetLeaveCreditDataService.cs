
using AccuPay.Core.Entities;
using AccuPay.Core.Services.Imports.ResetLeaveCredits;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IResetLeaveCreditDataService : IBaseSavableDataService<ResetLeaveCredit>
    {
        Task SaveManyAsync2(int organizationId, int userId, IReadOnlyCollection<ResetLeaveCreditModel> resetLeaveCreditModels);
        Task ApplyLeaveCredits(int organizationId, int userId, IList<ResetLeaveCreditItemModel> resetLeaveCreditItems);
    }
}
