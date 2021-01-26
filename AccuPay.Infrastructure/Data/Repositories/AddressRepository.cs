using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;

namespace AccuPay.Infrastructure.Data
{
    public class AddressRepository : SavableRepository<Address>, IAddressRepository
    {
        public AddressRepository(PayrollContext context) : base(context)
        {
        }
    }
}
