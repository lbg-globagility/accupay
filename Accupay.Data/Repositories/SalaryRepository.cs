using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class SalaryRepository : SavableRepository<Salary>
    {
        public SalaryRepository(PayrollContext context) : base(context)
        {
        }

        #region Save

        protected override void DetachNavigationProperties(Salary salary)
        {
            if (salary.Employee != null)
            {
                _context.Entry(salary.Employee).State = EntityState.Detached;
            }
        }

        #endregion Save

        #region Queries

        #region Single entity

        public async Task<Salary> GetByIdWithEmployeeAsync(int id)
        {
            return await _context.Salaries
                .Include(x => x.Employee)
                .Where(x => x.RowID == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Salary> GetLatest(int employeeId)
        {
            return await _context.Salaries
                .Where(t => t.EmployeeID == employeeId)
                .OrderByDescending(t => t.EffectiveFrom)
                .FirstOrDefaultAsync();
        }

        #endregion Single entity

        #region List of entities

        public ICollection<Salary> GetByEmployee(int employeeId)
        {
            return _context.Salaries
                .Where(x => x.EmployeeID == employeeId)
                .ToList();
        }

        public async Task<PaginatedList<Salary>> List(
            PageOptions options,
            int organizationId,
            string searchTerm = "",
            int? employeeId = null)
        {
            var query = _context.Salaries
                .Include(x => x.Employee)
                .Where(x => x.OrganizationID == organizationId)
                .OrderByDescending(x => x.EffectiveFrom)
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

            if (employeeId.HasValue)
            {
                query = query.Where(t => t.EmployeeID == employeeId);
            }

            var salaries = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedList<Salary>(salaries, count);
        }

        public async Task<ICollection<Salary>> GetAllAsync(int organizationId)
        {
            return await _context.Salaries
                .Where(x => x.OrganizationID == organizationId)
                .ToListAsync();
        }

        public ICollection<Salary> GetByCutOff(int organizationId, DateTime cutoffStart, DateTime? cutoffEnd = null)
        {
            return CreateBaseQueryByCutOff(cutoffStart, cutoffEnd)
                .Where(x => x.OrganizationID == organizationId)
                .ToList();
        }

        public async Task<ICollection<Salary>> GetByCutOffAsync(int organizationId, DateTime cutoffStart)
        {
            return await CreateBaseQueryByCutOff(cutoffStart)
                .Where(x => x.OrganizationID == organizationId)
                .ToListAsync();
        }

        public async Task<ICollection<Salary>> GetByMultipleEmployeeAsync(int[] employeeIds, DateTime cutoffStart)
        {
            return await CreateBaseQueryByCutOff(cutoffStart)
                .Where(x => employeeIds.Contains(x.EmployeeID.Value))
                .ToListAsync();
        }

        #endregion List of entities

        #endregion Queries

        private IQueryable<Salary> CreateBaseQueryByCutOff(DateTime cutoffStart, DateTime? cutoffEnd = null)
        {
            if (cutoffEnd != null)
            {
                return _context.Salaries
                    .Where(sal => SatisfiedDate(sal.EffectiveFrom, cutoffStart, cutoffEnd))
                    .OrderByDescending(x => x.EffectiveFrom)
                    .GroupBy(x => x.EmployeeID)
                    .Select(g => g.FirstOrDefault());
            }

            return _context.Salaries
                .Where(x => x.EffectiveFrom <= cutoffStart)
                .OrderByDescending(x => x.EffectiveFrom)
                .GroupBy(x => x.EmployeeID)
                .Select(g => g.FirstOrDefault());
        }

        private bool SatisfiedDate(DateTime salaryEffectiveFrom, DateTime cutoffStart, DateTime? cutoffEnd)
        {
            return !(salaryEffectiveFrom <= cutoffStart) ? salaryEffectiveFrom <= cutoffEnd : salaryEffectiveFrom <= cutoffStart;
        }
    }
}