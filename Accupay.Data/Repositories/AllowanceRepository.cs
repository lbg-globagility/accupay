using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class AllowanceRepository : SavableRepository<Allowance>
    {
        public AllowanceRepository(PayrollContext context) : base(context)
        {
        }

        protected override void DetachNavigationProperties(Allowance allowance)
        {
            if (allowance.Employee != null)
            {
                _context.Entry(allowance.Employee).State = EntityState.Detached;
            }

            if (allowance.Product != null)
            {
                _context.Entry(allowance.Product).State = EntityState.Detached;

                if (allowance.Product.CategoryEntity != null)
                {
                    _context.Entry(allowance.Product.CategoryEntity).State = EntityState.Detached;
                }
            }
        }

        #region Queries

        #region Single entity

        internal async Task<Allowance> GetByIdWithEmployeeAndProductAsync(int id)
        {
            return await _context.Allowances
                                .Include(x => x.Employee)
                                .Include(x => x.Product)
                                .FirstOrDefaultAsync(l => l.RowID == id);
        }

        internal async Task<Allowance> GetEmployeeEcolaAsync(int employeeId,
                                                        int organizationId,
                                                        TimePeriod timePeriod)
        {
            return await CreateBaseQueryByTimePeriod(organizationId,
                                                    timePeriod).

                                Where(a => a.EmployeeID == employeeId).
                                Where(a => a.Product.PartNo.ToLower() ==
                                            ProductConstant.ECOLA).
                                FirstOrDefaultAsync();
        }

        #endregion Single entity

        #region List of entities

        internal async Task<IEnumerable<Allowance>> GetByEmployeeWithProductAsync(int employeeId)
        {
            return await _context.Allowances.
                            Include(p => p.Product).
                            Where(l => l.EmployeeID == employeeId).
                            ToListAsync();
        }

        internal async Task<PaginatedList<Allowance>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
        {
            var query = _context.Allowances
                .Include(x => x.Employee)
                .Include(x => x.Product)
                .Where(x => x.OrganizationID == organizationId)
                .OrderByDescending(x => x.EffectiveStartDate)
                .ThenBy(x => x.Product.PartNo)
                .ThenBy(x => x.Employee.LastName)
                .ThenBy(x => x.Employee.FirstName)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.Product.PartNo, searchTerm) ||
                    EF.Functions.Like(x.Employee.EmployeeNo, searchTerm) ||
                    EF.Functions.Like(x.Employee.FirstName, searchTerm) ||
                    EF.Functions.Like(x.Employee.LastName, searchTerm));
            }

            var allowances = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedList<Allowance>(allowances, count);
        }

        internal ICollection<Allowance> GetByPayPeriodWithProduct(int organizationId,
                                                                TimePeriod timePeriod)
        {
            return CreateBaseQueryByTimePeriod(organizationId,
                                            timePeriod).
                                            ToList();
        }

        internal async Task<ICollection<Allowance>> GetByPayPeriodWithProductAsync(int organizationId,
                                                                                TimePeriod timePeriod)
        {
            return await CreateBaseQueryByTimePeriod(organizationId,
                                                    timePeriod).
                                                    ToListAsync();
        }

        #endregion List of entities

        #region Others

        internal List<string> GetFrequencyList()
        {
            return new List<string>()
            {
                Allowance.FREQUENCY_ONE_TIME,
                Allowance.FREQUENCY_DAILY,
                Allowance.FREQUENCY_SEMI_MONTHLY,
                Allowance.FREQUENCY_MONTHLY
            };
        }

        internal async Task<bool> CheckIfAlreadyUsedAsync(int id)
        {
            return await _context.AllowanceItems.AnyAsync(a => a.AllowanceID == id);
        }

        #endregion Others

        #endregion Queries

        #region Private helper methods

        private IQueryable<Allowance> CreateBaseQueryByTimePeriod(int organizationId,
                                                                TimePeriod timePeriod)
        {
            return _context.Allowances.
                    Include(a => a.Product).
                    Where(a => a.OrganizationID == organizationId).
                    Where(a => a.EffectiveStartDate <= timePeriod.End).
                    Where(a => a.EffectiveEndDate == null ? true : timePeriod.Start <= a.EffectiveEndDate);
        }

        #endregion Private helper methods
    }
}