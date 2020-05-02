using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class AdjustmentRepository
    {
        public async Task<IEnumerable<Adjustment>> GetByPaystubWithProductAsync(int paystubId)
        {
            using (var context = new PayrollContext())
            {
                return await context.Adjustments.
                                Include(x => x.Product).
                                Where(x => x.PaystubID == paystubId).
                                ToListAsync();
            }
        }
    }
}