using AccuPay.Data.Entities;

namespace AccuPay.Data.Repositories
{
    public class AddressRepository : SavableRepository<Address>
    {
        public AddressRepository(PayrollContext context) : base(context)
        {
        }
    }
}