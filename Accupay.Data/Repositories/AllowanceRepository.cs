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
    public class AllowanceRepository
    {
        private readonly PayrollContext _context;

        public AllowanceRepository(PayrollContext context)
        {
            _context = context;
        }

        #region CRUD

        public async Task DeleteAsync(int id)
        {
            var allowance = await GetByIdAsync(id);

            _context.Remove(allowance);

            await _context.SaveChangesAsync();
        }

        public async Task SaveManyAsync(List<Allowance> allowances)
        {
            foreach (var allowance in allowances)
            {
                await SaveWithContextAsync(allowance);

                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveAsync(Allowance allowance)
        {
            await SaveWithContextAsync(allowance, deferSave: false);
        }

        private async Task SaveWithContextAsync(Allowance allowance, bool deferSave = true)
        {
            if (allowance.Product != null)
            {
                _context.Entry(allowance.Product).State = EntityState.Detached;
            }

            await SaveAsyncFunction(allowance);

            if (deferSave == false)
            {
                await _context.SaveChangesAsync();
            }
        }

        private async Task SaveAsyncFunction(Allowance allowance)
        {
            if (allowance.ProductID == null)
                throw new ArgumentException("Allowance type cannot be empty.");

            var product = await _context.Products.
                                    Where(p => p.RowID == allowance.ProductID).
                                    FirstOrDefaultAsync();

            if (product == null)
                throw new ArgumentException("The selected allowance type no longer exists. Please close then reopen the form to view the latest data.");

            if (allowance.IsMonthly && !product.Fixed)
                throw new ArgumentException("Only fixed allowance type are allowed for Monthly allowances.");

            // add or update the allowance
            if (allowance.RowID == null)
                _context.Allowances.Add(allowance);
            else
                _context.Entry(allowance).State = EntityState.Modified;
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        public async Task<Allowance> GetByIdAsync(int id)
        {
            return await _context.Allowances.FirstOrDefaultAsync(l => l.RowID == id);
        }

        public async Task<Allowance> GetEmployeeEcolaAsync(int employeeId,
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

        public async Task<IEnumerable<Allowance>> GetByEmployeeWithProductAsync(int employeeId)
        {
            return await _context.Allowances.
                            Include(p => p.Product).
                            Where(l => l.EmployeeID == employeeId).
                            ToListAsync();
        }

        public async Task<PaginatedListResult<Allowance>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
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

        public ICollection<Allowance> GetByPayPeriodWithProduct(int organizationId,
                                                                TimePeriod timePeriod)
        {
            return CreateBaseQueryByTimePeriod(organizationId,
                                            timePeriod).
                                            ToList();
        }

        public async Task<ICollection<Allowance>> GetByPayPeriodWithProductAsync(int organizationId,
                                                                                TimePeriod timePeriod)
        {
            return await CreateBaseQueryByTimePeriod(organizationId,
                                                    timePeriod).
                                                    ToListAsync();
        }

        #endregion List of entities

        #region Others

        public List<string> GetFrequencyList()
        {
            return new List<string>()
            {
                Allowance.FREQUENCY_ONE_TIME,
                Allowance.FREQUENCY_DAILY,
                Allowance.FREQUENCY_SEMI_MONTHLY,
                Allowance.FREQUENCY_MONTHLY
            };
        }

        public async Task<bool> CheckIfAlreadyUsed(int id)
        {
            return await _context.AllowanceItems.AnyAsync(a => a.AllowanceID == id);
        }

        public async Task<bool> CheckIfAlreadyUsed(string allowanceName)
        {
            return await _context.AllowanceItems.
                                Include(x => x.Allowance).
                                Include(x => x.Allowance.Product).
                                Where(x => x.Allowance.Product.PartNo == allowanceName).
                                AnyAsync();
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