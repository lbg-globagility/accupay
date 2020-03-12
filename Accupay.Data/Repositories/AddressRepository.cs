using Accupay.Data.Entities;
using System.Linq;

namespace Accupay.Data.Repositories
{
    public class AddressRepository
    {
        public Address FindById(int id)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.Addresses.FirstOrDefault(x => x.RowID == id);
            }
        }
    }
}