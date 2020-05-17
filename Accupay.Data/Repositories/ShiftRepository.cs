using AccuPay.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Repositories
{
    public class ShiftRepository
    {
        private readonly PayrollContext _context;

        public ShiftRepository(PayrollContext context)
        {
            _context = context;
        }

        public IEnumerable<Shift> GetAll(int organizationId)
        {
            return _context.Shifts.
                            Where(x => x.OrganizationID == organizationId).
                            ToList();
        }
    }
}