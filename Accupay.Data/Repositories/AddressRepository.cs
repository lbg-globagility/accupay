using AccuPay.Data.Entities;
using System.Linq;

namespace AccuPay.Data.Repositories
{
    public class AddressRepository
    {
        private readonly PayrollContext _context;

        public AddressRepository(PayrollContext context)
        {
            _context = context;
        }

        public Address GetById(int id)
        {
            return _context.Addresses.FirstOrDefault(x => x.RowID == id);
        }
    }
}