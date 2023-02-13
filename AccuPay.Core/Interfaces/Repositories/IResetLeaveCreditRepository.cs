
using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces.Repositories
{
    public interface IResetLeaveCreditRepository : ISavableRepository<ResetLeaveCredit>
    {
        Task<ICollection<ResetLeaveCredit>> GetResetLeaveCredits(int organizationId);
    }
}
