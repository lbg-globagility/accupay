using AccuPay.Data.Entities;
using AccuPay.Data.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Repositories
{
    public class AgencyFeeRepository
    {
        private readonly PayrollContext _context;

        public AgencyFeeRepository(PayrollContext context)
        {
            _context = context;
        }

        public IEnumerable<AgencyFee> GetByDatePeriod(int organizationId, TimePeriod timePeriod)
        {
            return _context.AgencyFees.
                    Where(x => x.OrganizationID == organizationId).
                    Where(x => timePeriod.Start <= x.Date).
                    Where(x => x.Date <= timePeriod.End).
                    ToList();
        }
    }
}