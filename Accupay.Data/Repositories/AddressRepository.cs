using AccuPay.Data.Entities;
using System.Linq;

namespace AccuPay.Data.Repositories
{
    public class AddressRepository
    {
        public Address GetById(int id)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.Addresses.FirstOrDefault(x => x.RowID == id);
            }
        }
    }
}