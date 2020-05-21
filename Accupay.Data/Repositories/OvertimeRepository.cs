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
    public class OvertimeRepository
    {
        private readonly PayrollContext _context;

        public OvertimeRepository(PayrollContext context)
        {
            _context = context;
        }

        #region CRUD

        public async Task DeleteAsync(int id)
        {
            var overtime = await GetByIdAsync(id);

            _context.Remove(overtime);

            await _context.SaveChangesAsync();
        }

        public async Task SaveManyAsync(List<Overtime> overtimes)
        {
            foreach (var overtime in overtimes)
            {
                await SaveWithContextAsync(overtime);
            }

            await _context.SaveChangesAsync();
        }

        public async Task SaveAsync(Overtime overtime)
        {
            await SaveWithContextAsync(overtime, deferSave: false);
        }

        private async Task SaveWithContextAsync(Overtime overtime,
                                                bool deferSave = true)
        {
            if (overtime.OTStartTime.HasValue)
            {
                overtime.OTStartTime = overtime.OTStartTime.Value.StripSeconds();
            }
            if (overtime.OTEndTime.HasValue)
            {
                overtime.OTEndTime = overtime.OTEndTime.Value.StripSeconds();
            }

            overtime.UpdateEndDate();

            SaveFunction(overtime);

            if (deferSave == false)
            {
                await _context.SaveChangesAsync();
            }
        }

        private void SaveFunction(Overtime overtime)
        {
            if (overtime.RowID == null)
                _context.Overtimes.Add(overtime);
            else
                _context.Entry(overtime).State = EntityState.Modified;
        }

        public async Task DeleteManyAsync(IList<int?> ids)
        {
            var overtimes = await _context.Overtimes.
                Where(o => ids.Contains(o.RowID)).
                ToListAsync();

            _context.RemoveRange(overtimes);

            await _context.SaveChangesAsync();
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        public async Task<Overtime> GetByIdAsync(int id)
        {
            return await _context.Overtimes.FirstOrDefaultAsync(l => l.RowID == id);
        }

        public async Task<Overtime> GetByIdWithEmployeeAsync(int id)
        {
            return await _context.Overtimes
                                .Include(x => x.Employee)
                                .FirstOrDefaultAsync(l => l.RowID == id);
        }

        #endregion Single entity

        #region List of entities

        public async Task<IEnumerable<Overtime>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.Overtimes.
                                Where(l => l.EmployeeID == employeeId).
                                ToListAsync();
        }

        public async Task<PaginatedListResult<Overtime>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
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

        public async Task<IEnumerable<Overtime>> GetByDatePeriodAsync(
                                                        int organizationId,
                                                        TimePeriod timePeriod,
                                                        OvertimeStatus overtimeStatus = OvertimeStatus.All)
        {
            return await CreateBaseQueryByTimePeriod(organizationId,
                                                    timePeriod,
                                                    overtimeStatus).
                            ToListAsync();
        }

        public IEnumerable<Overtime> GetByDatePeriod(int organizationId,
                                                    TimePeriod timePeriod,
                                                    OvertimeStatus overtimeStatus = OvertimeStatus.All)
        {
            return CreateBaseQueryByTimePeriod(organizationId,
                                                timePeriod,
                                                overtimeStatus).
                        ToList();
        }

        public IEnumerable<Overtime> GetByEmployeeIDsAndDatePeriod(int organizationId,
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

        #endregion List of entities

        #region Others

        public List<string> GetStatusList()
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