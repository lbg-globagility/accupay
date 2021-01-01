using AccuPay.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Repositories
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
            return await _context.PaystubEmailHistories
                .Where(x => paystubIds.Contains(x.PaystubID))
                .ToListAsync();
        }

        public async Task DeleteByEmployeeAndPayPeriodAsync(int employeeId, int payPeriodId)
        {
            var emails = await _context.PaystubEmailHistories
                .Where(x => x.Paystub.EmployeeID == employeeId)
                .Where(x => x.Paystub.PayPeriodID == payPeriodId)
                .ToListAsync();

            if (!emails.Any()) return;

            _context.PaystubEmailHistories.RemoveRange(emails);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByPayPeriodAsync(int payPeriodId)
        {
            var emails = await _context.PaystubEmailHistories
                .Where(x => x.Paystub.PayPeriodID == payPeriodId)
                .ToListAsync();

            if (!emails.Any()) return;

            _context.PaystubEmailHistories.RemoveRange(emails);
            await _context.SaveChangesAsync();
        }
    }
}