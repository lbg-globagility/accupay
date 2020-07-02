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
    public class TimeLogRepository
    {
        private readonly PayrollContext _context;

        public TimeLogRepository(PayrollContext context)
        {
            _context = context;
        }

        public class CompositeKey
        {
            public int EmployeeId { get; set; }
            public DateTime Date { get; set; }

            public CompositeKey(int employeeId, DateTime date)
            {
                EmployeeId = employeeId;

                Date = date;
            }
        }

        #region CRUD

        public async Task ChangeManyAsync(
            List<TimeLog> addedTimeLogs,
            List<TimeLog> updatedTimeLogs,
            List<TimeLog> deletedTimeLogs)
        {
            if (addedTimeLogs != null)
            {
                addedTimeLogs.ForEach(timeLog =>
                {
                    _context.Entry(timeLog).State = EntityState.Added;
                });
            }

            if (updatedTimeLogs != null)
            {
                updatedTimeLogs.ForEach(timeLog =>
                {
                    _context.Entry(timeLog).State = EntityState.Modified;
                });
            }

            if (deletedTimeLogs != null)
            {
                deletedTimeLogs = deletedTimeLogs
                    .GroupBy(x => x.RowID)
                    .Select(x => x.FirstOrDefault())
                    .ToList();
                _context.TimeLogs.RemoveRange(deletedTimeLogs);
            }

            await _context.SaveChangesAsync();
        }

        internal async Task UpdateAsync(TimeLog timeLog)
        {
            _context.Entry(timeLog).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        internal async Task DeleteAsync(TimeLog timeLog)
        {
            _context.Remove(timeLog);
            await _context.SaveChangesAsync();
        }

        internal async Task SaveImportAsync(IReadOnlyCollection<TimeLog> timeLogs,
                                        IReadOnlyCollection<TimeAttendanceLog> timeAttendanceLogs = null)
        {
            string importId = GenerateImportId(_context);

            foreach (var timeLog in timeLogs)
            {
                timeLog.TimeentrylogsImportID = importId;

                _context.TimeLogs.Add(timeLog);

                var minimumDate = timeLog.LogDate.ToMinimumHourValue();
                var maximumDate = timeLog.LogDate.ToMaximumHourValue();

                _context.TimeAttendanceLogs.
                    RemoveRange(_context.TimeAttendanceLogs
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

        internal async Task CreateAsync(TimeLog timeLog)
        {
            _context.TimeLogs.Add(timeLog);
            await _context.SaveChangesAsync();
        }

        #endregion CRUD

        #region Queries

        public async Task<ICollection<TimeLog>> GetByEmployeeAndDateAsync(int employeeId, DateTime date)
        {
            return await _context.TimeLogs
                .Where(x => x.EmployeeID == employeeId)
                .Where(x => x.LogDate == date)
                .ToListAsync();
        }

        public ICollection<TimeLog> GetByDatePeriod(int organizationId, TimePeriod datePeriod)
        {
            return CreateBaseQueryByDatePeriod(datePeriod)
                .Where(x => x.OrganizationID == organizationId)
                .ToList();
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

        public async Task<PaginatedListResult<TimeLog>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
        {
            var query = _context.TimeLogs
                .Include(x => x.Employee)
                .Include(x => x.Branch)
                .Where(x => x.OrganizationID == organizationId)
                .OrderBy(x => x.Employee.LastName)
                .ThenBy(x => x.Employee.FirstName)
                .ThenByDescending(x => x.LogDate)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.LogDate.ToString(), searchTerm) ||
                    EF.Functions.Like(x.Employee.EmployeeNo, searchTerm) ||
                    EF.Functions.Like(x.Employee.FirstName, searchTerm) ||
                    EF.Functions.Like(x.Employee.LastName, searchTerm) ||
                    EF.Functions.Like(x.Branch.Name, searchTerm));
            }

            var timeLogs = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedListResult<TimeLog>(timeLogs, count);
        }

        public async Task<(ICollection<Employee> employees, int total, ICollection<TimeLog> timeLogs)> ListByEmployeeAsync(
            int organizationId,
            PageOptions options,
            DateTime dateFrom,
            DateTime dateTo,
            string searchTerm)
        {
            // TODO: might want to use employeeRepository for this query
            var query = _context.Employees
                .Where(x => x.OrganizationID == organizationId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.EmployeeNo, searchTerm) ||
                    EF.Functions.Like(x.FirstName, searchTerm) ||
                    EF.Functions.Like(x.LastName, searchTerm));
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
                .Where(x => dateFrom <= x.LogDate && x.LogDate <= dateTo)
                .ToListAsync();

            return (employees, total, timeLogs);
        }

        public async Task<TimeLog> GetByIdWithEmployeeAsync(int id)
        {
            return await _context.TimeLogs
                .Include(b => b.Branch)
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(l => l.RowID == id);
        }

        public async Task<TimeLog> GetByIdAsync(int id)
        {
            return await _context.TimeLogs.FirstOrDefaultAsync(l => l.RowID == id);
        }

        public async Task<TimeLog> GetByIdAsync(CompositeKey key)
        {
            return await _context.TimeLogs
                .Where(x => x.EmployeeID == key.EmployeeId)
                .Where(x => x.LogDate == key.Date)
                .FirstOrDefaultAsync();
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
                .FirstOrDefault(t => t.TimeentrylogsImportID == importId) != null ||
                                context.TimeAttendanceLogs
                                    .FirstOrDefault(t => t.ImportNumber == importId)
                                != null)
            {
                counter += 1;

                importId = originalImportId + "_" + counter;
            }

            return importId;
        }
    }
}