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
        public async Task<IEnumerable<Salary>> GetAll(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Salaries.
                    Where(s => s.OrganizationID == organizationId).
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

        private IQueryable<Salary> CreateBaseQueryByCutOff(int organizationId,
                                                        DateTime cutoffStart,
                                                        PayrollContext context)
        {
            return context.Salaries.
                            Where(s => s.OrganizationID == organizationId).
                            Where(s => s.EffectiveFrom <= cutoffStart).
                            OrderByDescending(s => s.EffectiveFrom).
                            GroupBy(s => s.EmployeeID).
                            Select(g => g.FirstOrDefault());
        }
    }
}