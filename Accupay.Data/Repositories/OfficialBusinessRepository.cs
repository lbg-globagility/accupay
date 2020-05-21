using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class OfficialBusinessRepository
    {
        private readonly PayrollContext _context;

        public OfficialBusinessRepository(PayrollContext context)
        {
            _context = context;
        }

        #region CRUD

        public async Task DeleteAsync(int id)
        {
            var officialBusiness = await GetByIdAsync(id);

            _context.Remove(officialBusiness);

            await _context.SaveChangesAsync();
        }

        public async Task SaveManyAsync(List<OfficialBusiness> officialBusinesses)
        {
            foreach (var officialBusiness in officialBusinesses)
            {
                await this.SaveWithContextAsync(officialBusiness);

                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveAsync(OfficialBusiness officialBusiness)
        {
            await SaveWithContextAsync(officialBusiness, deferSave: false);
        }

        private async Task SaveWithContextAsync(OfficialBusiness officialBusiness,
                                                bool deferSave = true)
        {
            if (officialBusiness.EmployeeID == null)
                throw new BusinessLogicException("Employee does not exists.");

            if (officialBusiness.StartTime.HasValue)
            {
                officialBusiness.StartTime = officialBusiness.StartTime.Value.StripSeconds();
            }
            if (officialBusiness.EndTime.HasValue)
            {
                officialBusiness.EndTime = officialBusiness.EndTime.Value.StripSeconds();
            }

            officialBusiness.UpdateEndDate();

            await SaveAsyncFunction(officialBusiness);

            if (deferSave == false)
            {
                await _context.SaveChangesAsync();
            }
        }

        private async Task SaveAsyncFunction(OfficialBusiness officialBusiness)
        {
            if (await _context.OfficialBusinesses.
                    Where(l => officialBusiness.RowID == null ? true : officialBusiness.RowID != l.RowID).
                    Where(l => l.EmployeeID == officialBusiness.EmployeeID).
                    Where(l => (l.StartDate.HasValue && officialBusiness.StartDate.HasValue && l.StartDate.Value.Date == officialBusiness.StartDate.Value.Date)).
                    AnyAsync())
                throw new BusinessLogicException($"Employee already has an OB for {officialBusiness.StartDate.Value.ToShortDateString()}");

            if (officialBusiness.RowID == null)
            {
                _context.OfficialBusinesses.Add(officialBusiness);
            }
            else
            {
                _context.OfficialBusinesses.Attach(officialBusiness);
                _context.Entry(officialBusiness).State = EntityState.Modified;
            }
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        public async Task<OfficialBusiness> GetByIdAsync(int id)
        {
            return await _context.OfficialBusinesses.
                            FirstOrDefaultAsync(l => l.RowID == id);
        }

        public async Task<OfficialBusiness> GetByIdWithEmployeeAsync(int id)
        {
            return await _context.OfficialBusinesses
                                .Include(x => x.Employee)
                                .FirstOrDefaultAsync(l => l.RowID == id);
        }

        #endregion Single entity

        #region List of entities

        public async Task<IEnumerable<OfficialBusiness>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.OfficialBusinesses.
                                Where(l => l.EmployeeID == employeeId).
                                ToListAsync();
        }

        public async Task<PaginatedListResult<OfficialBusiness>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
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

        public IEnumerable<OfficialBusiness> GetAllApprovedByDatePeriod(int organizationId, TimePeriod timePeriod)
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

        public List<string> GetStatusList()
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