using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class OfficialBusinessRepository : BaseRepository
    {
        private readonly PayrollContext _context;

        public OfficialBusinessRepository(PayrollContext context)
        {
            _context = context;
        }

        #region CRUD

        internal async Task DeleteAsync(OfficialBusiness officialBusiness)
        {
            _context.Remove(officialBusiness);
            await _context.SaveChangesAsync();
        }

        internal async Task SaveAsync(OfficialBusiness officialBusiness)
        {
            SaveFunction(officialBusiness);
            await _context.SaveChangesAsync();
        }

        internal async Task SaveManyAsync(List<OfficialBusiness> officialBusinesses)
        {
            officialBusinesses.ForEach(x => SaveFunction(x));
            await _context.SaveChangesAsync();
        }

        private void SaveFunction(OfficialBusiness officialBusiness)
        {
            if (officialBusiness.Employee != null)
            {
                _context.Entry(officialBusiness.Employee).State = EntityState.Unchanged;
            }

            if (IsNewEntity(officialBusiness.RowID))
            {
                _context.OfficialBusinesses.Add(officialBusiness);
            }
            else
            {
                _context.Entry(officialBusiness).State = EntityState.Modified;
            }
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        internal async Task<OfficialBusiness> GetByIdAsync(int id)
        {
            return await _context.OfficialBusinesses
                                .FirstOrDefaultAsync(l => l.RowID == id);
        }

        internal async Task<OfficialBusiness> GetByIdWithEmployeeAsync(int id)
        {
            return await _context.OfficialBusinesses
                                .Include(x => x.Employee)
                                .FirstOrDefaultAsync(l => l.RowID == id);
        }

        #endregion Single entity

        #region List of entities

        internal async Task<IEnumerable<OfficialBusiness>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.OfficialBusinesses.
                                Where(l => l.EmployeeID == employeeId).
                                ToListAsync();
        }

        internal async Task<PaginatedListResult<OfficialBusiness>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
        {
            var query = _context.OfficialBusinesses
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
                    EF.Functions.Like(x.Employee.EmployeeNo, searchTerm) ||
                    EF.Functions.Like(x.Employee.FirstName, searchTerm) ||
                    EF.Functions.Like(x.Employee.LastName, searchTerm));
            }

            var officialBusinesses = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedListResult<OfficialBusiness>(officialBusinesses, count);
        }

        internal IEnumerable<OfficialBusiness> GetAllApprovedByDatePeriod(int organizationId, TimePeriod timePeriod)
        {
            return _context.OfficialBusinesses.
                            Where(l => l.OrganizationID == organizationId).
                            Where(l => timePeriod.Start <= l.StartDate).
                            Where(l => l.EndDate <= timePeriod.End).
                            Where(l => l.Status == OfficialBusiness.StatusApproved).
                            ToList();
        }

        #endregion List of entities

        #region Others

        internal List<string> GetStatusList()
        {
            return new List<string>()
            {
                OfficialBusiness.StatusPending,
                OfficialBusiness.StatusApproved
            };
        }

        #endregion Others

        #endregion Queries
    }
}