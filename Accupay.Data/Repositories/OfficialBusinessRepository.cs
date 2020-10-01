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
    public class OfficialBusinessRepository : SavableRepository<OfficialBusiness>
    {
        public OfficialBusinessRepository(PayrollContext context) : base(context)
        {
        }

        protected override void DetachNavigationProperties(OfficialBusiness officialBusiness)
        {
            if (officialBusiness.Employee != null)
            {
                _context.Entry(officialBusiness.Employee).State = EntityState.Detached;
            }
        }

        #region Queries

        #region Single entity

        public async Task<OfficialBusiness> GetByIdWithEmployeeAsync(int id)
        {
            return await _context.OfficialBusinesses
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(l => l.RowID == id);
        }

        #endregion Single entity

        #region List of entities

        public async Task<ICollection<OfficialBusiness>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.OfficialBusinesses
                .Where(l => l.EmployeeID == employeeId)
                .ToListAsync();
        }

        public async Task<PaginatedList<OfficialBusiness>> GetPaginatedListAsync(OfficialBusinessPageOptions options, int organizationId)
        {
            var query = _context.OfficialBusinesses
                .Include(x => x.Employee)
                .Where(x => x.OrganizationID == organizationId)
                .OrderByDescending(x => x.StartDate)
                .ThenBy(x => x.StartTime)
                .ThenBy(x => x.Employee.LastName)
                .ThenBy(x => x.Employee.FirstName)
                .AsQueryable();

            if (options.HasEmployeeId)
            {
                query = query.Where(t => t.EmployeeID == options.EmployeeId);
            }

            if (options.HasSearchTerm)
            {
                var searchTerm = $"%{options.Term}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.Employee.EmployeeNo, searchTerm) ||
                    EF.Functions.Like(x.Employee.FirstName, searchTerm) ||
                    EF.Functions.Like(x.Employee.LastName, searchTerm));
            }

            if (options.HasDateFrom)
            {
                query = query.Where(x => options.DateFrom.Value.Date <= x.StartDate);
            }
            if (options.HasDateTo)
            {
                query = query.Where(x => x.StartDate <= options.DateTo.Value.Date);
            }

            var officialBusinesses = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedList<OfficialBusiness>(officialBusinesses, count);
        }

        internal async Task<List<OfficialBusiness>> GetByEmployeeIdsBetweenDatesAsync(int organizationId, List<int> employeeIds, TimePeriod timePeriod)
        {
            var result = await _context.OfficialBusinesses
                .Include(ot => ot.Employee)
                .Where(ot => ot.OrganizationID == organizationId)
                .Where(ot => employeeIds.Contains(ot.EmployeeID.Value))
                .Where(ot => ot.StartDate >= timePeriod.Start)
                .Where(ot => ot.StartDate <= timePeriod.End)
                .ToListAsync();

            return result;
        }

        public ICollection<OfficialBusiness> GetAllApprovedByDatePeriod(int organizationId, TimePeriod datePeriod)
        {
            return CreateByQueryAllApprovedByDatePeriod(organizationId, datePeriod)
                .ToList();
        }

        public async Task<ICollection<OfficialBusiness>> GetAllApprovedByEmployeeAndDatePeriodAsync(int organizationId, int employeeId, TimePeriod datePeriod)
        {
            return await CreateByQueryAllApprovedByDatePeriod(organizationId, datePeriod)
                .Where(l => l.EmployeeID == employeeId)
                .ToListAsync();
        }

        private IQueryable<OfficialBusiness> CreateByQueryAllApprovedByDatePeriod(int organizationId, TimePeriod datePeriod)
        {
            return _context.OfficialBusinesses
                .Where(l => l.OrganizationID == organizationId)
                .Where(l => datePeriod.Start <= l.StartDate)
                .Where(l => l.EndDate <= datePeriod.End)
                .Where(l => l.Status == OfficialBusiness.StatusApproved);
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