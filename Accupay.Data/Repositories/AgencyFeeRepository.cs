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

        public ICollection<AgencyFee> GetByDatePeriod(int organizationId, TimePeriod timePeriod)
        {
            return CreateBaseQueryGetByDatePeriod(organizationId, timePeriod)
                        .ToList();
        }

        public decimal GetPaystubAmount(int organizationId, TimePeriod timePeriod, int employeeId)
        {
            return CreateBaseQueryGetByDatePeriod(organizationId, timePeriod)
                        .Where(x => x.EmployeeID == employeeId)
                        .Sum(x => x.Amount);
        }

        public IQueryable<AgencyFee> CreateBaseQueryGetByDatePeriod(int organizationId, TimePeriod timePeriod)
        {
            return _context.AgencyFees
                    .Where(x => x.OrganizationID == organizationId)
                    .Where(x => timePeriod.Start <= x.Date)
                    .Where(x => x.Date <= timePeriod.End);
        }
    }
}