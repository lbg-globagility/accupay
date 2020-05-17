using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class AdjustmentRepository
    {
        private readonly PayrollContext _context;

        public AdjustmentRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Adjustment>> GetByPaystubWithProductAsync(int paystubId)
        {
            return await _context.Adjustments.
                            Include(x => x.Product).
                            Where(x => x.PaystubID == paystubId).
                            ToListAsync();
        }
    }
}