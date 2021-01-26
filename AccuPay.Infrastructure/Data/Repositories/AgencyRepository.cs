using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class AgencyRepository : IAgencyRepository
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
