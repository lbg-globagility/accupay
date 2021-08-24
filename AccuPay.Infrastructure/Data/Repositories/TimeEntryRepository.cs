using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class TimeEntryRepository : ITimeEntryRepository
    {
        private readonly PayrollContext _context;

        public TimeEntryRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<(IReadOnlyCollection<TimeEntry> timeEntries, IReadOnlyCollection<ActualTimeEntry> actualTimeEntries)> DeleteByEmployeeAsync(
            int employeeId,
            int payPeriodId)
        {
            var payPeriod = await _context.PayPeriods
                .FirstOrDefaultAsync(x => x.RowID == payPeriodId);

            if (payPeriod == null) return (null, null);

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

            return (
                timeEntries: timeEntries,
                actualTimeEntries: actualTimeEntries);
        }

        public async Task<(IReadOnlyCollection<TimeEntry> timeEntries, IReadOnlyCollection<ActualTimeEntry> actualTimeEntries)> DeleteByPayPeriodAsync(
            int payPeriodId)
        {
            var payPeriod = await _context.PayPeriods
                .FirstOrDefaultAsync(x => x.RowID == payPeriodId);

            if (payPeriod == null) return (null, null);

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

            return (
                timeEntries: timeEntries,
                actualTimeEntries: actualTimeEntries);
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
                .AsNoTracking()
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => datePeriod.Start <= x.Date)
                .Where(x => x.Date <= datePeriod.End);
        }
    }
}
