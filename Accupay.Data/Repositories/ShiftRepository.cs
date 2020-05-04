using AccuPay.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Repositories
{
    public class ShiftRepository
    {
        public IEnumerable<Shift> GetAll(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.Shifts.
                                Where(x => x.OrganizationID == organizationId).
                                ToList();
            }
        }
    }
}