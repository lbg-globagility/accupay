using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class LeaveRepository
    {
        private readonly PayrollContext _context;

        public LeaveRepository(PayrollContext context)
        {
            _context = context;
        }

        #region CRUD

        public async Task DeleteAsync(int id)
        {
            var leave = await GetByIdAsync(id);

            _context.Remove(leave);

            await _context.SaveChangesAsync();
        }

        public async Task SaveWithContextAsync(Leave leave, bool deferSave = true)
        {
            if (leave.StartTime.HasValue)
            {
                leave.StartTime = leave.StartTime.Value.StripSeconds();
            }

            if (leave.EndTime.HasValue)
            {
                leave.EndTime = leave.EndTime.Value.StripSeconds();
            }

            leave.UpdateEndDate();

            await SaveAsyncFunction(leave);

            if (deferSave == false)
            {
                await _context.SaveChangesAsync();
            }
        }

        private async Task SaveAsyncFunction(Leave leave)
        {
            if (await _context.Leaves.Where(l => leave.RowID == null ? true : leave.RowID != l.RowID).
                                Where(l => l.EmployeeID == leave.EmployeeID).
                                Where(l => (leave.StartDate.Date >= l.StartDate.Date && leave.StartDate.Date <= l.EndDate.Value.Date) ||
                                        (leave.EndDate.Value.Date >= l.StartDate.Date && leave.EndDate.Value.Date <= l.EndDate.Value.Date)).
                                AnyAsync())
                throw new BusinessLogicException($"Employee already has a leave for {leave.StartDate.ToShortDateString()}");

            if (leave.RowID == null)
            {
                _context.Leaves.Add(leave);
            }
            else
            {
                // since we used LoadAsync() above, we can't just simply attach the leave
                // TODO: refactor the validate leave above to not load the whole database
                // better if we use transactions then check afterwards if there are invariants
                // just like in metrotiles
                await UpdateAsync(leave, _context);
            }
        }

        private async Task UpdateAsync(Leave leave, PayrollContext context)
        {
            var currentLeave = await context.Leaves.
                                FirstOrDefaultAsync(l => l.RowID == leave.RowID);

            if (currentLeave == null) return;

            currentLeave.StartTime = leave.StartTime;
            currentLeave.EndTime = leave.EndTime;
            currentLeave.LeaveType = leave.LeaveType;
            currentLeave.StartDate = leave.StartDate;
            currentLeave.EndDate = leave.EndDate;
            currentLeave.Reason = leave.Reason;
            currentLeave.Comments = leave.Comments;
            currentLeave.Status = leave.Status;
            currentLeave.LastUpdBy = leave.LastUpdBy;
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

        public async Task<IEnumerable<Leave>> GetByEmployeeAsync(int employeeId)
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

        public IEnumerable<Leave> GetAllApprovedByDatePeriod(int organizationId, TimePeriod datePeriod)
        {
            return CreateBaseQueryByDatePeriod(organizationId, datePeriod)
                    .Where(l => l.Status.Trim().ToLower() == Leave.StatusApproved.ToTrimmedLowerCase())
                    .ToList();
        }

        public async Task<IEnumerable<Leave>> GetAllApprovedByEmployeeAndDatePeriod(int organizationId, int employeeId, TimePeriod datePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, datePeriod)
                    .Where(l => l.Status.Trim().ToLower() == Leave.StatusApproved.ToTrimmedLowerCase())
                    .Where(x => x.EmployeeID == employeeId)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Leave>> GetByDatePeriodAsync(int organizationId, TimePeriod datePeriod)
        {
            return await CreateBaseQueryByDatePeriod(organizationId, datePeriod).ToListAsync();
        }

        public async Task<IEnumerable<Leave>> GetFilteredAllAsync(Expression<Func<Leave, bool>> filter)
        {
            return await _context.Leaves.Where(filter).ToListAsync();
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