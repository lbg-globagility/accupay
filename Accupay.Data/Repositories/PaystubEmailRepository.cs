using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PaystubEmailRepository
    {
        public async Task CreateManyAsync(IEnumerable<PaystubEmail> paystubEmails)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.PaystubEmails.AddRange(paystubEmails);
                await context.SaveChangesAsync();
            }
        }

        public PaystubEmail FirstOnQueueWithPaystubDetails()
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.PaystubEmails
                    .Where(x => x.Status == PaystubEmail.StatusWaiting)
                    .Include(x => x.Paystub.PayPeriod)
                    .Include(x => x.Paystub.Employee)
                    .FirstOrDefault();
            }
        }

        public async Task<IEnumerable<PaystubEmail>> GetByPaystubIdsAsync(int[] paystubIds)
        {
            using (var context = new PayrollContext())
            {
                return await context.PaystubEmails.
                    Where(x => paystubIds.Contains(x.PaystubID)).
                    ToListAsync();
            }
        }
    }
}