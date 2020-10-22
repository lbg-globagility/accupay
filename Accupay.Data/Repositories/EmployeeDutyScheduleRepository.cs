using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class EmployeeDutyScheduleRepository
    {
        private readonly PayrollContext _context;

        public EmployeeDutyScheduleRepository(PayrollContext context)
        {
            _context = context;
        }

        #region Save

        public async Task ChangeManyAsync(
            List<EmployeeDutySchedule> added = null,
            List<EmployeeDutySchedule> updated = null,
            List<EmployeeDutySchedule> deleted = null)
        {
            if (added != null)
            {
                added.ForEach(shift =>
                {
                    _context.Entry(shift).State = EntityState.Added;
                });

                var addedOvertimes = updated
                    .Where(s => s.IsShiftBasedAutoOvertimeEnabled)
                    .Where(s => s.OvertimeId == null)
                    .Where(s => s.Overtime != null)
                    .Select(s => s.Overtime)
                    .ToList();
                if (addedOvertimes.Any())
                {
                    var validOvertimes = addedOvertimes
                        .Where(ot => ot.OTStartDate == ot.OTEndDate)
                        .Where(ot => ot.OTEndTime != null)
                        .Where(ot => ot.OrganizationID != null)
                        .Where(ot => ot.EmployeeID != null)
                        .Where(ot => ot.Status == Overtime.StatusApproved)
                        .ToList();
                    validOvertimes.ForEach(ot => { _context.Entry(ot).State = EntityState.Added; });
                }
            }

            if (updated != null)
            {
                updated.ForEach(shift =>
                {
                    _context.Entry(shift).State = EntityState.Modified;
                });

                var addedOvertimes = updated
                    .Where(s => s.IsShiftBasedAutoOvertimeEnabled)
                    .Where(s => s.OvertimeId == null)
                    .Where(s => s.Overtime != null)
                    .Select(s => s.Overtime)
                    .ToList();
                if (addedOvertimes.Any()) addedOvertimes.ForEach(ot => { _context.Entry(ot).State = EntityState.Added; });

                var updatedOvertimes = updated
                    .Where(s => s.IsShiftBasedAutoOvertimeEnabled)
                    .Where(s => s.OvertimeId != null)
                    .Where(s => s.Overtime != null)
                    .Select(s => s.Overtime)
                    .ToList();
                if (updatedOvertimes.Any()) updatedOvertimes.ForEach(ot => { _context.Entry(ot).State = EntityState.Modified; });
            }

            if (deleted != null)
            {
                deleted = deleted
                    .GroupBy(x => x.RowID)
                    .Select(x => x.FirstOrDefault())
                    .ToList();

                DeleteAssocOvertime(deleted);

                _context.EmployeeDutySchedules.RemoveRange(deleted);
            }

            await _context.SaveChangesAsync();
        }

        private void DeleteAssocOvertime(List<EmployeeDutySchedule> deleted)
        {
            var overtimes = deleted
                .Where(s => s.IsShiftBasedAutoOvertimeEnabled)
                .Where(s => s.Overtime != null)
                .Where(s => s.OvertimeId != null)
                .Select(s => s.Overtime)
                .ToList();
            if (overtimes.Any()) overtimes.ForEach(ot => { _context.Entry(ot).State = EntityState.Deleted; });
        }

        #endregion Save

        #region Queries

        #region List of entities

        public ICollection<EmployeeDutySchedule> GetByDatePeriod(
            int organizationId,
            TimePeriod datePeriod)
        {
            return CreateBaseQueryByDatePeriod(organizationId, datePeriod).ToList();
        }

        public async Task<ICollection<EmployeeDutySchedule>> GetByDatePeriodAsync(
            int organizationId,
            TimePeriod datePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, datePeriod).ToListAsync();
        }

        public async Task<ICollection<EmployeeDutySchedule>> GetByEmployeeAndDatePeriodAsync(
            int organizationId,
            int employeeId,
            TimePeriod datePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, datePeriod)
                .Where(x => x.EmployeeID == employeeId)
                .ToListAsync();
        }

        public async Task<List<EmployeeDutySchedule>> GetByMultipleEmployeeAndBetweenDatePeriodAsync(
            int organizationId,
            IEnumerable<int> employeeIds,
            TimePeriod timePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, timePeriod)
                .Include(x => x.Employee)
                .Where(x => employeeIds.Contains(x.EmployeeID.Value))
                .ToListAsync();
        }

        public async Task<(ICollection<Employee> employees, int total, ICollection<EmployeeDutySchedule>)> ListByEmployeeAsync(
            int organizationId,
            ShiftsByEmployeePageOptions options)
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

            var dutySchedules = await _context.EmployeeDutySchedules
                .Where(x => employeeIds.Contains(x.EmployeeID))
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => options.DateFrom.Date <= x.DateSched && x.DateSched <= options.DateTo.Date)
                .ToListAsync();

            return (employees, total, dutySchedules);
        }

        public async Task<ICollection<EmployeeDutySchedule>> GetByEmployeeAndDatePeriodAsync(
            int organizationId,
            int[] employeeIds,
            TimePeriod datePeriod)
        {
            return await CreateBaseQueryByMultipleEmployeeDatePeriod(
                    organizationId,
                    employeeIds,
                    datePeriod)
                .ToListAsync();
        }

        public async Task<ICollection<EmployeeDutySchedule>> GetByEmployeeAndDatePeriodWithEmployeeAsync(
            int organizationId,
            int[] employeeIds,
            TimePeriod datePeriod)
        {
            return await CreateBaseQueryByMultipleEmployeeDatePeriod(
                    organizationId,
                    employeeIds,
                    datePeriod)
                .Include(x => x.Employee)
                .ToListAsync();
        }

        public async Task<ICollection<EmployeeDutySchedule>> GetByEmployeeAndDatePeriodWithEmployeeAsync(
            int organizationId,
            int[] employeeIds,
            TimePeriod datePeriod,
            bool isShiftBasedAutoOvertimeEnabled)
        {
            if (!isShiftBasedAutoOvertimeEnabled)
            {
                return await GetByEmployeeAndDatePeriodWithEmployeeAsync(organizationId, employeeIds, datePeriod);
            }

            return await CreateBaseQueryByMultipleEmployeeDatePeriod(
                    organizationId,
                    employeeIds,
                    datePeriod)
                .Include(x => x.Employee)
                .Include(l => l.Overtime)
                .ToListAsync();
        }

        public async Task<PaginatedList<EmployeeDutySchedule>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
        {
            var query = _context.EmployeeDutySchedules
                .Include(x => x.Employee)
                .Where(x => x.OrganizationID == organizationId)
                .OrderBy(x => x.Employee.LastName)
                .ThenBy(x => x.Employee.FirstName)
                .ThenByDescending(x => x.DateSched)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.DateSched.ToString(), searchTerm) ||
                    EF.Functions.Like(x.Employee.EmployeeNo, searchTerm) ||
                    EF.Functions.Like(x.Employee.FirstName, searchTerm) ||
                    EF.Functions.Like(x.Employee.LastName, searchTerm));
            }

            var shifts = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedList<EmployeeDutySchedule>(shifts, count);
        }

        #endregion List of entities

        #endregion Queries

        private IQueryable<EmployeeDutySchedule> CreateBaseQueryByMultipleEmployeeDatePeriod(
            int organizationId,
            int[] employeeIds,
            TimePeriod datePeriod)
        {
            return CreateBaseQueryByDatePeriod(organizationId, datePeriod)
                .Where(x => employeeIds.Contains(x.EmployeeID.Value));
        }

        private IQueryable<EmployeeDutySchedule> CreateBaseQueryByDatePeriod(int organizationId, TimePeriod datePeriod)
        {
            return _context.EmployeeDutySchedules
                .Where(l => l.OrganizationID == organizationId)
                .Where(l => datePeriod.Start <= l.DateSched)
                .Where(l => l.DateSched <= datePeriod.End);
        }
    }
}