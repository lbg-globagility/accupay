using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class TimeLogRepository : SavableRepository<TimeLog>, ITimeLogRepository
    {
        public TimeLogRepository(PayrollContext context) : base(context)
        {
        }

        #region Save

        protected override void DetachNavigationProperties(TimeLog timeLog)
        {
            if (timeLog.Employee != null)
            {
                _context.Entry(timeLog.Employee).State = EntityState.Detached;
            }
        }

        #endregion Save

        #region Queries

        public async Task<ICollection<TimeLog>> GetByDatePeriodAsync(int organizationId, TimePeriod datePeriod)
        {
            return await CreateBaseQueryByDatePeriod(datePeriod)
                .Where(x => x.OrganizationID == organizationId)
                .ToListAsync();
        }

        public async Task<ICollection<TimeLog>> GetLatestByEmployeeAndDatePeriodAsync(
            int employeeId,
            TimePeriod datePeriod)
        {
            // TODO: employeetimeentrydetails date should be unique so no query like this should be needed.
            return await CreateBaseQueryByDatePeriod(datePeriod)
                .Where(x => x.EmployeeID == employeeId)
                .OrderByDescending(x => x.LastUpd)
                .GroupBy(x => x.LogDate)
                .Select(x => x.FirstOrDefault())
                .OrderBy(x => x.LogDate)
                .ToListAsync();
        }

        public async Task<ICollection<TimeLog>> GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(
            IEnumerable<int> employeeIds,
            TimePeriod datePeriod)
        {
            return await CreateBaseQueryByDatePeriod(datePeriod)
                .Include(x => x.Employee)
                .Where(x => employeeIds.Contains(x.EmployeeID.Value))
                .ToListAsync();
        }

        public async Task<(ICollection<Employee> employees, int total, ICollection<TimeLog> timeLogs)> ListByEmployeeAsync(
            int organizationId,
            TimeLogsByEmployeePageOptions options)
        {
            // TODO: might want to use employeeRepository for this query
            var query = _context.Employees
                .Where(x => x.OrganizationID == organizationId);

            if (options.HasSearchTerm)
            {
                var searchTerm = $"%{options.SearchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.EmployeeNo, searchTerm) ||
                    EF.Functions.Like(x.FirstName, searchTerm) ||
                    EF.Functions.Like(x.LastName, searchTerm));
            }

            if (options.HasStatus)
            {
                if (options.Status == "Active only")
                {
                    query = query.Where(e => e.EmploymentStatus != "Resigned" && e.EmploymentStatus != "Terminated");
                }
            }

            query = query
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName);

            var employees = await query.Page(options).ToListAsync();
            var total = await query.CountAsync();

            var employeeIds = employees.Select(x => x.RowID);

            var timeLogs = await _context.TimeLogs
                .Include(x => x.Branch)
                .Where(x => employeeIds.Contains(x.EmployeeID))
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => options.DateFrom.Date <= x.LogDate && x.LogDate <= options.DateTo.Date)
                .ToListAsync();

            return (employees, total, timeLogs);
        }

        private IQueryable<TimeLog> CreateBaseQueryByDatePeriod(TimePeriod datePeriod)
        {
            return _context.TimeLogs
                .Where(x => datePeriod.Start <= x.LogDate)
                .Where(x => x.LogDate <= datePeriod.End);
        }

        #endregion Queries
    }
}
