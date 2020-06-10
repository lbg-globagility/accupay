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

        public IEnumerable<TimeEntry> GetByDatePeriod(int organizationId, TimePeriod timePeriod)
        {
            return CreateBaseQueryByDatePeriod(organizationId, timePeriod).
                    ToList();
        }

        public async Task<IEnumerable<IGrouping<int?, TimeEntry>>> GetTimexzEntriesByEmployeeIds(
            int organizationId,
            TimePeriod timePeriod,
            IEnumerable<int?> employeeIds)
        {
            var query = CreateBaseQueryByDatePeriod(organizationId, timePeriod)
                .Where(t => employeeIds.Contains(t.EmployeeID));

            var timeEntries = await query.ToListAsync();
            var timeEntriesByEmployee = timeEntries
                .GroupBy(t => t.EmployeeID);

            return timeEntriesByEmployee;
        }

        public async Task<IEnumerable<TimeEntry>> GetByDatePeriodAsync(int organizationId,
                                                                        TimePeriod timePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, timePeriod).
                        ToListAsync();
        }

        public async Task<IEnumerable<TimeEntry>> GetByEmployeeAndDatePeriodAsync(int organizationId,
                                                                                int employeeId,
                                                                                TimePeriod timePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, timePeriod)
                .Where(x => x.EmployeeID == employeeId)
                .OrderBy(x => x.Date)
                .ToListAsync();
        }

        private IQueryable<TimeEntry> CreateBaseQueryByDatePeriod(int organizationId,
                                                                    TimePeriod timePeriod)
        {
            return _context.TimeEntries.
                    Where(x => x.OrganizationID == organizationId).
                    Where(x => timePeriod.Start <= x.Date).
                    Where(x => x.Date <= timePeriod.End);
        }
    }
}