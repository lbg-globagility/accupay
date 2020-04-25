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
        public async Task<List<Salary>> GetAll(int organizationID)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Salaries.
                    Where(s => s.OrganizationID == organizationID).
                    ToListAsync();
            }
        }

        public async Task<List<Salary>> GetAllByCutOff(int organizationID, DateTime cutoffStart)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Salaries.
                    Where(s => s.OrganizationID == organizationID).
                    Where(s => s.EffectiveFrom <= cutoffStart).
                    Where(s => cutoffStart <= (s.EffectiveTo ?? cutoffStart)).
                    ToListAsync();
            }
        }
    }
}