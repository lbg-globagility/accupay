using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;

namespace AccuPay.Infrastructure.Data
{
    public class ContractorRepository : SavableRepository<Contractor>, IContractorRepository
    {
        public ContractorRepository(PayrollContext context) : base(context)
        {
        }
    }
}
