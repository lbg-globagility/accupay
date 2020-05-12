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
        private readonly PayrollContext _context;

        public PaystubEmailHistoryRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaystubEmailHistory>> GetByPaystubIdsAsync(int[] paystubIds)
        {
            return await _context.PaystubEmailHistories.
                Where(x => paystubIds.Contains(x.PaystubID)).
                ToListAsync();
        }
    }
}