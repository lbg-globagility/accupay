using AccuPay.Data.Entities;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class TimeEntryRepository
    {
        private readonly PayrollContext _context;

        public TimeEntryRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task DeleteByEmployeeAsync(int employeeId, int payPeriodId)
        {
            var payPeriod = await _context.PayPeriods
                .FirstOrDefaultAsync(x => x.RowID == payPeriodId);

            if (payPeriod == null) return;

            var timeEntries = await _context.TimeEntries
                .Where(x => x.EmployeeID == employeeId)
                .Where(x => x.Date >= payPeriod.PayFromDate)
                .Where(x => x.Date <= payPeriod.PayToDate)
                .ToListAsync();

            var actualTimeEntries = await _context.ActualTimeEntries
                .Where(x => x.EmployeeID == employeeId)
                .Where(x => x.Date >= payPeriod.PayFromDate)
                .Where(x => x.Date <= payPeriod.PayToDate)
                .ToListAsync();

            _context.RemoveRange(timeEntries);
            _context.RemoveRange(actualTimeEntries);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteByPayPeriodAsync(int payPeriodId)
        {
            var payPeriod = await _context.PayPeriods
                .FirstOrDefaultAsync(x => x.RowID == payPeriodId);

            if (payPeriod == null) return;

            var timeEntries = await _context.TimeEntries
                .Where(x => x.OrganizationID == payPeriod.OrganizationID)
                .Where(x => x.Date >= payPeriod.PayFromDate)
                .Where(x => x.Date <= payPeriod.PayToDate)
                .ToListAsync();

            var actualTimeEntries = await _context.ActualTimeEntries
                .Where(x => x.OrganizationID == payPeriod.OrganizationID)
                .Where(x => x.Date >= payPeriod.PayFromDate)
                .Where(x => x.Date <= payPeriod.PayToDate)
                .ToListAsync();

            _context.RemoveRange(timeEntries);
            _context.RemoveRange(actualTimeEntries);

            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<TimeEntry>> GetByDatePeriodAsync(
            int organizationId,
            TimePeriod datePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, datePeriod).ToListAsync();
        }

        public async Task<ICollection<TimeEntry>> GetByEmployeeAndDatePeriodAsync(
            int organizationId,
            int employeeId,
            TimePeriod datePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, datePeriod)
                .Where(x => x.EmployeeID == employeeId)
                .OrderBy(x => x.Date)
                .ToListAsync();
        }

        private IQueryable<TimeEntry> CreateBaseQueryByDatePeriod(int organizationId, TimePeriod datePeriod)
        {
            return _context.TimeEntries
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => datePeriod.Start <= x.Date)
                .Where(x => x.Date <= datePeriod.End);
        }
    }
}
