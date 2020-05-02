using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class AdjustmentRepository
    {
        public async Task<IEnumerable<Adjustment>> GetByPaystubAsync(int paystubId)
        {
            using (var context = new PayrollContext())
            {
                return await context.Adjustments.
                                Where(x => x.PaystubID == paystubId).
                                ToListAsync();
            }
        }
    }
}