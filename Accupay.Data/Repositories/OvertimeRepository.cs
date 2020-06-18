using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
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
    public class OvertimeRepository : SavableRepository<Overtime>
    {
        public OvertimeRepository(PayrollContext context) : base(context)
        {
        }

        internal async Task DeleteManyAsync(IEnumerable<int> ids)
        {
            var overtimes = await _context.Overtimes
                .Where(o => ids.Contains(o.RowID.Value))
                .ToListAsync();

            _context.RemoveRange(overtimes);

            await _context.SaveChangesAsync();
        }

        #region CRUD

        protected override void DetachNavigationProperties(Overtime overtime)
        {
            if (overtime.Employee != null)
            {
                _context.Entry(overtime.Employee).State = EntityState.Detached;
            }
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        internal async Task<Overtime> GetByIdWithEmployeeAsync(int id)
        {
            return await _context.Overtimes
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(l => l.RowID == id);
        }

        #endregion Single entity

        #region List of entities

        internal async Task<ICollection<Overtime>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.Overtimes
                .Where(l => l.EmployeeID == employeeId)
                .ToListAsync();
        }

        internal async Task<PaginatedListResult<Overtime>> GetPaginatedListAsync(
            PageOptions options,
            int organizationId,
            string searchTerm = "",
            DateTime? dateFrom = null,
            DateTime? dateTo = null)
        {
            var query = _context.Overtimes
                .Include(x => x.Employee)
                .Where(x => x.OrganizationID == organizationId)
                .OrderByDescending(x => x.OTStartDate)
                    .ThenBy(x => x.OTStartTime)
                    .ThenBy(x => x.Employee.LastName)
                    .ThenBy(x => x.Employee.FirstName)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.Employee.EmployeeNo, searchTerm) ||
                    EF.Functions.Like(x.Employee.FirstName, searchTerm) ||
                    EF.Functions.Like(x.Employee.LastName, searchTerm));
            }

            if (dateFrom.HasValue)
            {
                query = query.Where(t => dateFrom.Value.Date <= t.OTStartDate);
            }
            if (dateTo.HasValue)
            {
                query = query.Where(t => t.OTStartDate <= dateTo.Value.Date);
            }

            var overtimes = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedListResult<Overtime>(overtimes, count);
        }

        internal async Task<ICollection<Overtime>> GetByEmployeeAndDatePeriod(
            int organizationId,
            int employeeId,
            TimePeriod datePeriod,
            OvertimeStatus overtimeStatus = OvertimeStatus.All)
        {
            return await CreateBaseQueryByTimePeriod(
                    organizationId,
                    datePeriod,
                    overtimeStatus)
                .Where(ot => ot.EmployeeID == employeeId)
                .ToListAsync();
        }

        internal ICollection<Overtime> GetByEmployeeIDsAndDatePeriod(
            int organizationId,
            List<int> employeeIdList,
            TimePeriod datePeriod,
            OvertimeStatus overtimeStatus = OvertimeStatus.All)
        {
            return CreateBaseQueryByTimePeriod(
                    organizationId,
                    datePeriod,
                    overtimeStatus)
                .Where(x => employeeIdList.Contains(x.EmployeeID.Value))
                .ToList();
        }

        internal async Task<ICollection<Overtime>> GetByDatePeriodAsync(
            int organizationId,
            TimePeriod datePeriod,
            OvertimeStatus overtimeStatus = OvertimeStatus.All)
        {
            return await CreateBaseQueryByTimePeriod(
                    organizationId,
                    datePeriod,
                    overtimeStatus)
                .ToListAsync();
        }

        public ICollection<Overtime> GetByDatePeriod(
            int organizationId,
            TimePeriod datePeriod,
            OvertimeStatus overtimeStatus = OvertimeStatus.All)
        {
            return CreateBaseQueryByTimePeriod(
                    organizationId,
                    datePeriod,
                    overtimeStatus)
                .ToList();
        }

        #endregion List of entities

        #region Others

        internal List<string> GetStatusList()
        {
            return new List<string>()
            {
                Overtime.StatusPending,
                Overtime.StatusApproved
            };
        }

        #endregion Others

        #endregion Queries

        #region Private helper methods

        private IQueryable<Overtime> CreateBaseQueryByTimePeriod(
            int organizationId,
            TimePeriod datePeriod,
            OvertimeStatus overtimeStatus)
        {
            var baseQuery = _context.Overtimes
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => datePeriod.Start <= x.OTStartDate)
                .Where(x => x.OTStartDate <= datePeriod.End);

            switch (overtimeStatus)
            {
                case OvertimeStatus.All: break;

                case OvertimeStatus.Approved:
                    baseQuery = baseQuery.Where(x => x.Status.Trim().ToLower() ==
                                                Overtime.StatusApproved.ToTrimmedLowerCase());
                    break;

                case OvertimeStatus.Pending:
                    baseQuery = baseQuery.Where(x => x.Status.Trim().ToLower() ==
                                                Overtime.StatusPending.ToTrimmedLowerCase());
                    break;

                default: break;
            }

            return baseQuery;
        }

        #endregion Private helper methods
    }
}