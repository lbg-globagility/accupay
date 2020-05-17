using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class SalaryRepository
    {
        private readonly PayrollContext _context;

        public SalaryRepository(PayrollContext context)
        {
            _context = context;
        }

        #region CRUD

        public async Task DeleteAsync(int id)
        {
            var salary = await GetByIdAsync(id);

            _context.Salaries.Remove(salary);

            await _context.SaveChangesAsync();
        }

        public async Task SaveManyAsync(List<Salary> salaries)
        {
            foreach (var salary in salaries)
            {
                await SaveWithContextAsync(salary);

                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveAsync(Salary salary)
        {
            await SaveWithContextAsync(salary, deferSave: false);
        }

        private async Task SaveWithContextAsync(Salary salary,
                                                bool deferSave = true)
        {
            if (deferSave == false)
            {
                SaveFunction(salary);
                await _context.SaveChangesAsync();
            }
            else
            {
                SaveFunction(salary);
            }
        }

        private void SaveFunction(Salary salary)
        {
            if (salary.RowID == null)
                _context.Salaries.Add(salary);
            else
                _context.Entry(salary).State = EntityState.Modified;
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        public async Task<Salary> GetByIdAsync(int id)
        {
            return await _context.Salaries.Where(x => x.RowID == id).FirstOrDefaultAsync();
        }

        #endregion Single entity

        #region List of entities

        public IEnumerable<Salary> GetByEmployee(int employeeId)
        {
            return _context.Salaries.
                    Where(x => x.EmployeeID == employeeId).
                    ToList();
        }

        public async Task<IEnumerable<Salary>> GetAll(int organizationId)
        {
            return await _context.Salaries.
                Where(x => x.OrganizationID == organizationId).
                ToListAsync();
        }

        public IEnumerable<Salary> GetByCutOff(int organizationId, DateTime cutoffStart)
        {
            return CreateBaseQueryByCutOff(organizationId, cutoffStart).
                            ToList();
        }

        public async Task<IEnumerable<Salary>> GetByCutOffAsync(int organizationId, DateTime cutoffStart)
        {
            return await CreateBaseQueryByCutOff(organizationId, cutoffStart).
                            ToListAsync();
        }

        #endregion List of entities

        #endregion Queries

        private IQueryable<Salary> CreateBaseQueryByCutOff(int organizationId,
                                                        DateTime cutoffStart)
        {
            return _context.Salaries.
                            Where(x => x.OrganizationID == organizationId).
                            Where(x => x.EffectiveFrom <= cutoffStart).
                            OrderByDescending(x => x.EffectiveFrom).
                            GroupBy(x => x.EmployeeID).
                            Select(g => g.FirstOrDefault());
        }
    }
}