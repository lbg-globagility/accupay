using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PayPeriodRepository
    {
        public PayPeriod GetById(int id)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.PayPeriods.FirstOrDefault(x => x.RowID == id);
            }
        }

        public async Task<PayPeriod> GetByIdAsync(int id)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.PayPeriods.FirstOrDefaultAsync(x => x.RowID == id);
            }
        }

        public async Task<IEnumerable<PayPeriod>> GetAllSemiMonthly(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.PayPeriods.
                                Where(x => x.OrganizationID == organizationId).
                                Where(x => x.IsSemiMonthly).
                                ToListAsync();
            }
        }
    }
}