using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data.Repositories
{
    public class AdjustmentRepository : SavableRepository<Adjustment>, IAdjustmentRepository
    {
        public AdjustmentRepository(PayrollContext context) : base(context) { }

        public async Task AppendManyAsync(int organizationId, int userId, int payPeriodId, List<Adjustment> adjustments)
        {
            throw new System.NotImplementedException();
        }

        public async Task AppendManyAsync(int organizationId, int userId, int payPeriodId, List<ActualAdjustment> actualAdjustments)
        {
            var paystubIds = actualAdjustments.Select(a => a.PaystubID).ToArray();
            var productIds = actualAdjustments.Select(a => a.ProductID).ToArray();
            var origAdjustments = await _context.ActualAdjustments.
                Where(a => paystubIds.Contains(a.PaystubID)).
                Where(a => productIds.Contains(a.ProductID)).
                ToListAsync();

            var added = new List<ActualAdjustment>();
            foreach (var adjustment in actualAdjustments)
            {
                var origAdjustment = origAdjustments.
                    Where(a => a.PaystubID == adjustment.PaystubID).
                    Where(a => a.ProductID == adjustment.ProductID).
                    FirstOrDefault();

                if(origAdjustment != null)
                {
                    origAdjustment.Amount = adjustment.Amount;
                    origAdjustment.LastUpdBy = userId;
                    _context.Entry(origAdjustment).State = EntityState.Modified;
                } else
                {
                    added.Add(adjustment);
                }
            }

            _context.ActualAdjustments.AddRange(added);

            await _context.SaveChangesAsync();
        }
    }
}
