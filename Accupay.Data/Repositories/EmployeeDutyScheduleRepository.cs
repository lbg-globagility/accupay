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

        internal async Task DeleteAsync(EmployeeDutySchedule shift)
        {
            _context.Remove(shift);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeManyAsync(
            List<EmployeeDutySchedule> addedShifts = null,
            List<EmployeeDutySchedule> updatedShifts = null,
            List<EmployeeDutySchedule> deletedShifts = null)
        {
            if (addedShifts != null)
            {
                addedShifts.ForEach(shift =>
                {
                    _context.Entry(shift).State = EntityState.Added;
                });
            }

            if (updatedShifts != null)
            {
                updatedShifts.ForEach(shift =>
                {
                    _context.Entry(shift).State = EntityState.Modified;
                });
            }

            if (deletedShifts != null)
            {
                deletedShifts = deletedShifts
                    .GroupBy(x => x.RowID)
                    .Select(x => x.FirstOrDefault())
                    .ToList();
                _context.EmployeeDutySchedules.RemoveRange(deletedShifts);
            }

            await _context.SaveChangesAsync();
        }

        internal async Task CreateAsync(EmployeeDutySchedule shift)
        {
            _context.EmployeeDutySchedules.Add(shift);
            await _context.SaveChangesAsync();
        }

        internal async Task UpdateAsync(EmployeeDutySchedule shift)
        {
            _context.Entry(shift).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        public async Task<EmployeeDutySchedule> GetByIdAsync(int id)
        {
            return await _context.EmployeeDutySchedules.FirstOrDefaultAsync(l => l.RowID == id);
        }

        public async Task<EmployeeDutySchedule> GetByIdAsync(CompositeKey key)
        {
            return await _context.EmployeeDutySchedules
                .Where(x => x.EmployeeID == key.EmployeeId)
                .Where(x => x.DateSched == key.Date)
                .FirstOrDefaultAsync();
        }

        public async Task<List<EmployeeDutySchedule>> GetByMultipleEmployeeAndBetweenDatePeriodAsync(int organizationId, List<int> employeeIds, TimePeriod timePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, timePeriod)
                .Include(x => x.Employee)
                .Where(x => employeeIds.Contains(x.EmployeeID.Value))
                .ToListAsync();
        }

        public async Task<EmployeeDutySchedule> GetByIdWithEmployeeAsync(int id)
        {
            return await _context.EmployeeDutySchedules
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(l => l.RowID == id);
        }

        #endregion Single entity

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

        internal async Task<(ICollection<Employee> employees, int total, ICollection<EmployeeDutySchedule>)> ListByEmployeeAsync(int organizationId, ShiftsByEmployeePageOptions options)
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