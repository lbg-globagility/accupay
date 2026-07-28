using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IContractorDataService : IBaseSavableDataService<Contractor>
    {
        Task<ICollection<Contractor>> GetAllAsync();
    }
}
