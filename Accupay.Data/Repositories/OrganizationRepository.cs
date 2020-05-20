using AccuPay.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class OrganizationRepository
    {
        private readonly PayrollContext _context;

        public OrganizationRepository(PayrollContext context)
        {
            _context = context;
        }

        public Organization GetById(int id)
        {
            return _context.Organizations.FirstOrDefault(x => x.RowID == id);
        }

        public async Task Create(Organization organization)
        {
            _context.Organizations.Add(organization);
            await _context.SaveChangesAsync();
        }
    }
}
