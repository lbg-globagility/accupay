using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class AgencyFeeRepository : IAgencyFeeRepository
    {
        private readonly PayrollContext _context;

        public AgencyFeeRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<ICollection<AgencyFee>> GetByDatePeriodAsync(int organizationId, TimePeriod timePeriod)
        {
            return await CreateBaseQueryGetByDatePeriod(organizationId, timePeriod)
                .ToListAsync();
        }

        public decimal GetPaystubAmount(int organizationId, TimePeriod timePeriod, int employeeId)
        {
            return CreateBaseQueryGetByDatePeriod(organizationId, timePeriod)
                .Where(x => x.EmployeeID == employeeId)
                .Sum(x => x.Amount);
        }

        private IQueryable<AgencyFee> CreateBaseQueryGetByDatePeriod(int organizationId, TimePeriod timePeriod)
        {
            return _context.AgencyFees
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => timePeriod.Start <= x.Date)
                .Where(x => x.Date <= timePeriod.End);
        }
    }
}
