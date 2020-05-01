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
        #region CRUD

        public async Task DeleteAsync(int id)
        {
            using (PayrollContext context = new PayrollContext())
            {
                var salary = await GetByIdAsync(id);

                context.Salaries.Remove(salary);

                await context.SaveChangesAsync();
            }
        }

        public async Task SaveAsync(Salary salary)
        {
            await SaveWithContextAsync(salary);
        }

        private async Task SaveWithContextAsync(Salary salary,
                                                PayrollContext passedContext = null)
        {
            if (passedContext == null)
            {
                using (var newContext = new PayrollContext())
                {
                    SaveFunction(salary, newContext);
                    await newContext.SaveChangesAsync();
                }
            }
            else
            {
                SaveFunction(salary, passedContext);
            }
        }

        private void SaveFunction(Salary salary, PayrollContext context)
        {
            if (salary.RowID == null)
                context.Salaries.Add(salary);
            else
                context.Entry(salary).State = EntityState.Modified;
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        public async Task<Salary> GetByIdAsync(int id)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Salaries.Where(x => x.RowID == id).FirstOrDefaultAsync();
            }
        }

        #endregion Single entity

        #region List of entities

        public IEnumerable<Salary> GetByEmployee(int employeeId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.Salaries.
                        Where(x => x.EmployeeID == employeeId).
                        ToList();
            }
        }

        public async Task<IEnumerable<Salary>> GetAll(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Salaries.
                    Where(x => x.OrganizationID == organizationId).
                    ToListAsync();
            }
        }

        public IEnumerable<Salary> GetByCutOff(int organizationId, DateTime cutoffStart)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return CreateBaseQueryByCutOff(organizationId, cutoffStart, context).
                                ToList();
            }
        }

        public async Task<IEnumerable<Salary>> GetByCutOffAsync(int organizationId, DateTime cutoffStart)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await CreateBaseQueryByCutOff(organizationId, cutoffStart, context).
                                ToListAsync();
            }
        }

        #endregion List of entities

        #endregion Queries

        private IQueryable<Salary> CreateBaseQueryByCutOff(int organizationId,
                                                        DateTime cutoffStart,
                                                        PayrollContext context)
        {
            return context.Salaries.
                            Where(x => x.OrganizationID == organizationId).
                            Where(x => x.EffectiveFrom <= cutoffStart).
                            OrderByDescending(x => x.EffectiveFrom).
                            GroupBy(x => x.EmployeeID).
                            Select(g => g.FirstOrDefault());
        }
    }
}