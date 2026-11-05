using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;

namespace AccuPay.Infrastructure.Data
{
    public class ContractorProjectRepository : SavableRepository<ContractorProject>, IContractorProjectRepository
    {
        public ContractorProjectRepository(PayrollContext context) : base(context)
        {
        }
    }
}
