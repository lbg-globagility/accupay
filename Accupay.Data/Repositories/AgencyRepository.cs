using AccuPay.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Repositories
{
    public class AgencyRepository
    {
        private readonly PayrollContext _context;

        public AgencyRepository(PayrollContext context)
        {
            _context = context;
        }

        public IEnumerable<Agency> GetAll(int organizationId)
        {
            return _context.Agencies.
                            Where(x => x.OrganizationID == organizationId).
                            ToList();
        }
    }
}