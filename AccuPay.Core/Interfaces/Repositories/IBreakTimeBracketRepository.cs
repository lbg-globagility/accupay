using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IBreakTimeBracketRepository
    {
        Task<ICollection<BreakTimeBracket>> GetAllAsync(int organizationId);
    }
}
