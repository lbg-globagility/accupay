using AccuPay.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Repositories
{
    public class AgencyRepository
    {
        public IEnumerable<Agency> GetAll(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.Agencies.
                                Where(x => x.OrganizationID == organizationId).
                                ToList();
            }
        }
    }
}