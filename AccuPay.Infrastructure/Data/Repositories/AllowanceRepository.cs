using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class AllowanceRepository : SavableRepository<Allowance>, IAllowanceRepository
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

                    if (allowance.Product.CategoryEntity.Products != null)
                    {
                        foreach (var categoryProduct in allowance.Product.CategoryEntity.Products)
                        {
                            _context.Entry(categoryProduct).State = EntityState.Detached;
                        }
                    }
                }
            }
        }

        #region Queries

        #region Single entity

        public async Task<Allowance> GetByIdWithEmployeeAndProductAsync(int id)
        {
            return await _context.Allowances
                .Include(x => x.Employee)
                .Include(x => x.Product)
                .Include(x => x.AllowanceType)
                .FirstOrDefaultAsync(l => l.RowID == id);
        }

        public async Task<Allowance> GetEmployeeEcolaAsync(
            int employeeId,
            int organizationId,
            TimePeriod timePeriod)
        {
            return await CreateBaseQueryByTimePeriod(
                    organizationId,
                    timePeriod)
                .Where(a => a.EmployeeID == employeeId)
                .Where(a => a.Product.PartNo.ToLower() == ProductConstant.ECOLA)
                .FirstOrDefaultAsync();
        }

        #endregion Single entity

        #region List of entities

        public async Task<ICollection<Allowance>> GetByEmployeeWithProductAsync(int employeeId)
        {
            return await _context.Allowances
                .Include(p => p.Product)
                .Where(l => l.EmployeeID == employeeId)
                .ToListAsync();
        }

        public async Task<PaginatedList<Allowance>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
        {
            var query = _context.Allowances
                .Include(x => x.Employee)
                .Include(x => x.Product)
                .Include(x => x.AllowanceType)
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

        public async Task<List<Allowance>> GetByEmployeeIdsBetweenDatesByAllowanceTypesAsync(List<int> employeeIds, List<string> allowanceTypeNames, TimePeriod timePeriod)
        {
            var sdfsd = await _context.Allowances
                .Include(a => a.Employee)
                .Include(a => a.AllowanceType)
                .Where(a => employeeIds.Contains(a.EmployeeID.Value))
                .Where(a => a.EffectiveStartDate >= timePeriod.Start)
                .Where(a => a.EffectiveEndDate <= timePeriod.End)
                .Where(a => allowanceTypeNames.Contains(a.AllowanceType.Name))
                .ToListAsync();

            return sdfsd;
        }

        public ICollection<Allowance> GetByPayPeriodWithProduct(int organizationId, TimePeriod timePeriod)
        {
            return CreateBaseQueryByTimePeriod(
                    organizationId,
                    timePeriod).
                ToList();
        }

        public async Task<ICollection<Allowance>> GetByPayPeriodWithProductAsync(int organizationId, TimePeriod timePeriod)
        {
            return await CreateBaseQueryByTimePeriod(
                    organizationId,
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

        public async Task<ICollection<PayPeriod>> GetPayPeriodsAsync(int id)
        {
            return await _context
                .AllowanceItems
                .AsNoTracking()
                .Include(x => x.PayPeriod)
                .Where(x => x.AllowanceID == id)
                .Select(x => x.PayPeriod)
                .ToListAsync();
        }

        public async Task<ICollection<PayPeriod>> GetPayPeriodsAsync(int[] ids)
        {
            return await _context
                .AllowanceItems
                .AsNoTracking()
                .Include(x => x.PayPeriod)
                .Where(x => ids.Contains(x.AllowanceID.Value))
                .Select(x => x.PayPeriod)
                .ToListAsync();
        }

        public async Task<ICollection<AllowanceItem>> GetAllowanceItemsWithPayPeriodAsync(int[] ids)
        {
            return await _context
                .AllowanceItems
                .AsNoTracking()
                .Include(x => x.PayPeriod)
                .Where(x => ids.Contains(x.AllowanceID.Value))
                .ToListAsync();
        }

        #endregion Others

        #endregion Queries

        #region Private helper methods

        private IQueryable<Allowance> CreateBaseQueryByTimePeriod(int organizationId, TimePeriod timePeriod)
        {
            return _context.Allowances
                .AsNoTracking()
                .Include(a => a.Product)
                .Where(a => a.OrganizationID == organizationId)
                .Where(a => a.EffectiveStartDate <= timePeriod.End)
                .Where(a => a.EffectiveEndDate == null ? true : timePeriod.Start <= a.EffectiveEndDate);
        }

        #endregion Private helper methods
    }
}
