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
    public class LeaveRepository : SavableRepository<Leave>
    {
        public LeaveRepository(PayrollContext context) : base(context)
        {
        }

        #region CRUD

        protected override void DetachNavigationProperties(Leave leave)
        {
            if (leave.Employee != null)
            {
                _context.Entry(leave.Employee).State = EntityState.Detached;
            }
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        public async Task<Leave> GetByIdAsync(int id)
        {
            return await _context.Leaves.FirstOrDefaultAsync(l => l.RowID == id);
        }

        public async Task<Leave> GetByIdWithEmployeeAsync(int id)
        {
            return await _context.Leaves
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(l => l.RowID == id);
        }

        #endregion Single entity

        #region List of entities

        public async Task<ICollection<Leave>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.Leaves
                .Where(l => l.EmployeeID == employeeId)
                .ToListAsync();
        }

        public async Task<PaginatedListResult<Leave>> GetPaginatedListAsync(
            PageOptions options,
            int organizationId,
            string searchTerm = "",
            DateTime? dateFrom = null,
            DateTime? dateTo = null)
        {
            var query = _context.Leaves
                .Include(x => x.Employee)
                .Where(x => x.OrganizationID == organizationId)
                .OrderByDescending(x => x.StartDate)
                    .ThenBy(x => x.StartTime)
                    .ThenBy(x => x.Employee.LastName)
                    .ThenBy(x => x.Employee.FirstName)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.LeaveType, searchTerm) ||
                    EF.Functions.Like(x.Employee.EmployeeNo, searchTerm) ||
                    EF.Functions.Like(x.Employee.FirstName, searchTerm) ||
                    EF.Functions.Like(x.Employee.LastName, searchTerm));
            }

            if (dateFrom.HasValue)
            {
                query = query.Where(t => dateFrom.Value.Date <= t.StartDate);
            }
            if (dateTo.HasValue)
            {
                query = query.Where(t => t.StartDate <= dateTo.Value.Date);
            }

            var leaves = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedListResult<Leave>(leaves, count);
        }

        public ICollection<Leave> GetAllApprovedByDatePeriod(int organizationId, TimePeriod datePeriod)
        {
            return CreateBaseQueryByDatePeriod(organizationId, datePeriod)
                    .Where(l => l.Status.Trim().ToLower() == Leave.StatusApproved.ToTrimmedLowerCase())
                    .ToList();
        }

        public async Task<ICollection<Leave>> GetAllApprovedByEmployeeAndDatePeriod(int organizationId, int employeeId, TimePeriod datePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, datePeriod)
                    .Where(l => l.Status.Trim().ToLower() == Leave.StatusApproved.ToTrimmedLowerCase())
                    .Where(x => x.EmployeeID == employeeId)
                    .ToListAsync();
        }

        public async Task<ICollection<Leave>> GetByDatePeriodAsync(int organizationId, TimePeriod datePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, datePeriod).ToListAsync();
        }

        public async Task<ICollection<Leave>> GetByEmployeeAndDatePeriodAsync(int organizationId, int[] employeeIds, TimePeriod datePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, datePeriod)
                .Where(x => employeeIds.Contains(x.EmployeeID.Value))
                .ToListAsync();
        }

        public async Task<ICollection<Leave>> GetUnusedApprovedLeavesByTypeAsync(int employeeId, Leave leave, DateTime firstDayOfTheYear, DateTime lastDayOfTheYear)
        {
            return await _context.Leaves
                .AsNoTracking()
                .Where(l => l.EmployeeID == employeeId)
                .Where(l => l.Status.Trim().ToUpper() == Leave.StatusApproved.ToUpper())
                .Where(l => l.LeaveType == leave.LeaveType)
                .Where(l => l.StartDate >= firstDayOfTheYear)
                .Where(l => l.StartDate <= lastDayOfTheYear)
                .Where(l => _context.LeaveTransactions
                    .Where(t => t.ReferenceID == l.RowID)
                    .Count() == 0)
                .ToListAsync();
        }

        #endregion List of entities

        #region Others

        public List<string> GetStatusList()
        {
            return new List<string>()
            {
                Leave.StatusPending,
                Leave.StatusApproved
            };
        }

        #endregion Others

        #endregion Queries

        #region Private helper methods

        private IQueryable<Leave> CreateBaseQueryByDatePeriod(int organizationId, TimePeriod datePeriod)
        {
            return _context.Leaves
                .Where(l => l.OrganizationID == organizationId)
                .Where(l => datePeriod.Start <= l.StartDate)
                .Where(l => l.EndDate <= datePeriod.End);
        }

        #endregion Private helper methods
    }
}