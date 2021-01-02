using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;

namespace AccuPay.Infrastructure.Data
{
    public class AddressRepository : SavableRepository<Address>, ISavableRepository<Address>
    {
        public AddressRepository(PayrollContext context) : base(context)
        {
        }
    }
}
