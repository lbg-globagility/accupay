using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IContractorProjectDataService : IBaseSavableDataService<ContractorProject>
    {
        Task<ICollection<ContractorProject>> GetAllAsync();
    }
}
