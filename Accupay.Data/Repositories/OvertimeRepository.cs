using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Helpers;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class OvertimeRepository : BaseRepository
    {
        private readonly PayrollContext _context;

        public OvertimeRepository(PayrollContext context)
        {
            _context = context;
        }

        #region CRUD

        internal async Task DeleteAsync(Overtime overtime)
        {
            _context.Remove(overtime);

            await _context.SaveChangesAsync();
        }

        internal async Task DeleteManyAsync(IEnumerable<int> ids)
        {
            var overtimes = await _context.Overtimes.
                Where(o => ids.Contains(o.RowID.Value)).
                ToListAsync();

            _context.RemoveRange(overtimes);

            await _context.SaveChangesAsync();
        }

        internal async Task SaveAsync(Overtime overtime)
        {
            SaveFunction(overtime);
            await _context.SaveChangesAsync();
        }

        internal async Task SaveManyAsync(List<Overtime> overtimes)
        {
            overtimes.ForEach(x => SaveFunction(x));
            await _context.SaveChangesAsync();
        }

        private void SaveFunction(Overtime overtime)
        {
            if (IsNewEntity(overtime.RowID))
            {
                _context.Overtimes.Add(overtime);
            }
            else
            {
                _context.Entry(overtime).State = EntityState.Modified;
            }
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        internal async Task<Overtime> GetByIdAsync(int id)
        {
            return await _context.Overtimes.FirstOrDefaultAsync(l => l.RowID == id);
        }

        internal async Task<Overtime> GetByIdWithEmployeeAsync(int id)
        {
            return await _context.Overtimes
                                .Include(x => x.Employee)
                                .FirstOrDefaultAsync(l => l.RowID == id);
        }

        #endregion Single entity

        #region List of entities

        internal async Task<IEnumerable<Overtime>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.Overtimes.
                                Where(l => l.EmployeeID == employeeId).
                                ToListAsync();
        }

        internal async Task<PaginatedListResult<Overtime>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
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

            var overtimes = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedListResult<Overtime>(overtimes, count);
        }

        internal IEnumerable<Overtime> GetByEmployeeIDsAndDatePeriod(int organizationId,
                                                                TimePeriod timePeriod,
                                                                List<int> employeeIdList,
                                                                OvertimeStatus overtimeStatus = OvertimeStatus.All)
        {
            return CreateBaseQueryByTimePeriod(organizationId,
                                                timePeriod,
                                                overtimeStatus).
                            Where(ot => employeeIdList.Contains(ot.EmployeeID.Value)).
                            ToList();
        }

        internal async Task<IEnumerable<Overtime>> GetByDatePeriodAsync(
                                                        int organizationId,
                                                        TimePeriod timePeriod,
                                                        OvertimeStatus overtimeStatus = OvertimeStatus.All)
        {
            return await CreateBaseQueryByTimePeriod(organizationId,
                                                    timePeriod,
                                                    overtimeStatus).
                            ToListAsync();
        }

        internal IEnumerable<Overtime> GetByDatePeriod(int organizationId,
                                                    TimePeriod timePeriod,
                                                    OvertimeStatus overtimeStatus = OvertimeStatus.All)
        {
            return CreateBaseQueryByTimePeriod(organizationId,
                                                timePeriod,
                                                overtimeStatus).
                        ToList();
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

        private IQueryable<Overtime> CreateBaseQueryByTimePeriod(int organizationId,
                                                                        TimePeriod timePeriod,
                                                                        OvertimeStatus overtimeStatus)
        {
            var baseQuery = _context.Overtimes.
                                Where(ot => ot.OrganizationID == organizationId).
                                Where(o => timePeriod.Start <= o.OTStartDate).
                                Where(o => o.OTStartDate <= timePeriod.End);

            switch (overtimeStatus)
            {
                case OvertimeStatus.All: break;

                case OvertimeStatus.Approved:
                    baseQuery = baseQuery.Where(o => o.Status.Trim().ToLower() ==
                                                Overtime.StatusApproved.ToTrimmedLowerCase());
                    break;

                case OvertimeStatus.Pending:
                    baseQuery = baseQuery.Where(o => o.Status.Trim().ToLower() ==
                                                Overtime.StatusPending.ToTrimmedLowerCase());
                    break;

                default: break;
            }

            return baseQuery;
        }

        #endregion Private helper methods
    }
}