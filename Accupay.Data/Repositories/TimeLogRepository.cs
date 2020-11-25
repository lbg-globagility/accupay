using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class TimeLogRepository : SavableRepository<TimeLog>
    {
        public TimeLogRepository(PayrollContext context) : base(context)
        {
        }

        #region Save

        public async Task SaveImportAsync(
            IReadOnlyCollection<TimeLog> timeLogs,
            IReadOnlyCollection<TimeAttendanceLog> timeAttendanceLogs = null)
        {
            string importId = GenerateImportId(_context);

            foreach (var timeLog in timeLogs)
            {
                timeLog.TimeentrylogsImportID = importId;

                _context.TimeLogs.Add(timeLog);

                var minimumDate = timeLog.LogDate.ToMinimumHourValue();
                var maximumDate = timeLog.LogDate.ToMaximumHourValue();

                _context.TimeAttendanceLogs
                    .RemoveRange(_context.TimeAttendanceLogs
                        .Where(t => t.TimeStamp >= minimumDate)
                        .Where(t => t.TimeStamp <= maximumDate));

                if (timeLog.Employee != null)
                {
                    _context.Entry(timeLog.Employee).State = EntityState.Detached;
                }
            }

            if (timeAttendanceLogs != null)
            {
                foreach (var timeAttendanceLog in timeAttendanceLogs)
                {
                    timeAttendanceLog.ImportNumber = importId;

                    _context.TimeAttendanceLogs.Add(timeAttendanceLog);

                    if (timeAttendanceLog.Employee != null)
                    {
                        _context.Entry(timeAttendanceLog.Employee).State = EntityState.Detached;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        #endregion Save

        #region Queries

        public async Task<ICollection<TimeLog>> GetByDatePeriodAsync(int organizationId, TimePeriod datePeriod)
        {
            return await CreateBaseQueryByDatePeriod(datePeriod)
                .Where(x => x.OrganizationID == organizationId)
                .ToListAsync();
        }

        public async Task<ICollection<TimeLog>> GetByEmployeeAndDateAsync(int employeeId, DateTime date)
        {
            return await _context.TimeLogs
                .Where(x => x.EmployeeID == employeeId)
                .Where(x => x.LogDate == date)
                .ToListAsync();
        }

        public async Task<ICollection<TimeLog>> GetLatestByEmployeeAndDatePeriodAsync(
            int employeeId,
            TimePeriod datePeriod)
        {
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

        private string GenerateImportId(PayrollContext context)
        {
            var importId = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var originalImportId = importId;

            int counter = 0;

            while (context.TimeLogs
                .FirstOrDefault(
                    t => t.TimeentrylogsImportID == importId) != null ||
                    context.TimeAttendanceLogs.FirstOrDefault(t => t.ImportNumber == importId) != null)
            {
                counter += 1;

                importId = originalImportId + "_" + counter;
            }

            return importId;
        }
    }
}
