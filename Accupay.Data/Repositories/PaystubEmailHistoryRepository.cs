using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PaystubEmailHistoryRepository
    {
        public async Task<IEnumerable<PaystubEmailHistory>> GetByPaystubIdsAsync(int[] paystubIds)
        {
            using (var context = new PayrollContext())
            {
                return await context.PaystubEmailHistories.
                    Where(x => paystubIds.Contains(x.PaystubID)).
                    ToListAsync();
            }
        }
    }
}