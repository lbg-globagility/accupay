using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class AgencyRepository
    {
        private readonly PayrollContext _context;

        public AgencyRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Agency>> GetAllAsync(int organizationId)
        {
            return await _context.Agencies
                .Where(x => x.OrganizationID == organizationId)
                .ToListAsync();
        }
    }
}
