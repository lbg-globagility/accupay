using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class LeaveLedgerRepository
    {
        private readonly PayrollContext context;

        public LeaveLedgerRepository(PayrollContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<LeaveLedger>> GetAllByEmployee(int? employeeId)
        {
            return await context.LeaveLedgers
                .Where(t => t.EmployeeID == employeeId)
                .ToListAsync();
        }

        public async Task<ICollection<LeaveTransaction>> GetTransactionsByLedger(int? leaveLedgerId)
        {
            return await context.LeaveTransactions
                .Where(t => t.LeaveLedgerID == leaveLedgerId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }
    }
}