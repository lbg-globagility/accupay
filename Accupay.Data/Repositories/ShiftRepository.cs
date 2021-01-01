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
    public class ShiftRepository : SavableRepository<Shift>
    {
        public ShiftRepository(PayrollContext context) : base(context)
        {
        }

        #region Queries

        #region List of entities

        public async Task<ICollection<Shift>> GetByDatePeriodAsync(
            int organizationId,
            TimePeriod datePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, datePeriod).ToListAsync();
        }

        public async Task<ICollection<Shift>> GetByEmployeeAndDatePeriodAsync(
            int organizationId,
            int employeeId,
            TimePeriod datePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, datePeriod)
                .Where(x => x.EmployeeID == employeeId)
                .ToListAsync();
        }

        public async Task<ICollection<Shift>> GetByMultipleEmployeeAndBetweenDatePeriodAsync(
            int organizationId,
            IEnumerable<int> employeeIds,
            TimePeriod timePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, timePeriod)
                .Include(x => x.Employee)
                .Where(x => employeeIds.Contains(x.EmployeeID.Value))
                .ToListAsync();
        }

        public async Task<(ICollection<Employee> employees, int total, ICollection<Shift>)> ListByEmployeeAsync(
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

            var dutySchedules = await _context.Shifts
                .Where(x => employeeIds.Contains(x.EmployeeID))
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => options.DateFrom.Date <= x.DateSched && x.DateSched <= options.DateTo.Date)
                .ToListAsync();

            return (employees, total, dutySchedules);
        }

        public async Task<ICollection<Shift>> GetByEmployeeAndDatePeriodAsync(
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

        public async Task<ICollection<Shift>> GetByEmployeeAndDatePeriodWithEmployeeAsync(
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

        #endregion List of entities

        #endregion Queries

        private IQueryable<Shift> CreateBaseQueryByMultipleEmployeeDatePeriod(
            int organizationId,
            int[] employeeIds,
            TimePeriod datePeriod)
        {
            return CreateBaseQueryByDatePeriod(organizationId, datePeriod)
                .Where(x => employeeIds.Contains(x.EmployeeID.Value));
        }

        private IQueryable<Shift> CreateBaseQueryByDatePeriod(int organizationId, TimePeriod datePeriod)
        {
            return _context.Shifts
                .Where(l => l.OrganizationID == organizationId)
                .Where(l => datePeriod.Start <= l.DateSched)
                .Where(l => l.DateSched <= datePeriod.End);
        }
    }
}
