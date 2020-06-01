using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class AllowanceRepository : BaseRepository
    {
        private readonly PayrollContext _context;

        public AllowanceRepository(PayrollContext context)
        {
            _context = context;
        }

        #region CRUD

        internal async Task DeleteAsync(Allowance allowance)
        {
            _context.Remove(allowance);
            await _context.SaveChangesAsync();
        }

        internal async Task SaveAsync(Allowance allowance)
        {
            SaveFunction(allowance);
            await _context.SaveChangesAsync();
        }

        internal async Task SaveManyAsync(List<Allowance> allowances)
        {
            allowances.ForEach(x => SaveFunction(x));
            await _context.SaveChangesAsync();
        }

        private void SaveFunction(Allowance allowance)
        {
            if (allowance.Employee != null)
            {
                _context.Entry(allowance.Employee).State = EntityState.Unchanged;
            }

            if (allowance.Product != null)
            {
                _context.Entry(allowance.Product).State = EntityState.Unchanged;

                if (allowance.Product.CategoryEntity != null)
                {
                    _context.Entry(allowance.Product.CategoryEntity).State = EntityState.Unchanged;
                }
            }

            if (IsNewEntity(allowance.RowID))
            {
                _context.Allowances.Add(allowance);
            }
            else
            {
                _context.Entry(allowance).State = EntityState.Modified;
            }
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        internal async Task<Allowance> GetByIdAsync(int id)
        {
            return await _context.Allowances
                                    .FirstOrDefaultAsync(l => l.RowID == id);
        }

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

        internal async Task<PaginatedListResult<Allowance>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
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

            return new PaginatedListResult<Allowance>(allowances, count);
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