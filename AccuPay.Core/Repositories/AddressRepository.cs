using AccuPay.Core.Entities;

namespace AccuPay.Core.Repositories
{
    public class AddressRepository : SavableRepository<Address>
    {
        public AddressRepository(PayrollContext context) : base(context)
        {
        }
    }
}