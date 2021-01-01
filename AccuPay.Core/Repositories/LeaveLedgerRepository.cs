using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Repositories
{
    public class LeaveLedgerRepository
    {
        private readonly PayrollContext _context;

        public LeaveLedgerRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<ICollection<LeaveLedger>> GetAllByEmployee(int? employeeId)
        {
            return await _context.LeaveLedgers
                .Where(t => t.EmployeeID == employeeId)
                .ToListAsync();
        }

        public async Task<ICollection<LeaveTransaction>> GetTransactionsByLedger(int? leaveLedgerId)
        {
            return await _context.LeaveTransactions
                .Where(t => t.LeaveLedgerID == leaveLedgerId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<PaginatedList<LeaveTransaction>> ListTransactionsAsync(PageOptions options, int organizationId, int id, string type = null)
        {
            var query = _context.LeaveTransactions
                .Include(x => x.Employee)
                .Include(x => x.LeaveLedger)
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => x.EmployeeID == id)
                .Where(x => x.LeaveLedger.Product.PartNo == type)
                .OrderByDescending(x => x.TransactionDate)
                .AsQueryable();

            var transaction = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedList<LeaveTransaction>(transaction, count);
        }

        public async Task<IEnumerable<LeaveLedger>> GetLeaveBalancesAsync(int organizationId, string searchTerm = null)
        {
            var balanceList = await _context.LeaveLedgers
                .Include(x => x.LastTransaction.Employee)
                .Include(x => x.Product)
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => x.Product.PartNo == ProductConstant.VACATION_LEAVE || x.Product.PartNo == ProductConstant.SICK_LEAVE)
                .Where(x => x.LastTransactionID != null)
                .OrderBy(x => x.LastTransaction.Employee.LastName)
                .ThenBy(x => x.LastTransaction.Employee.FirstName)
                .ToListAsync();

            var query = balanceList.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.LastTransaction.Employee.FullNameWithMiddleInitialLastNameFirst, searchTerm) ||
                    EF.Functions.Like(x.EmployeeID.ToString(), searchTerm));
            }

            return query;
        }

        public async Task CreateBeginningBalanceAsync(int employeeId, int leaveTypeId, int userId, int organizationId, decimal balance)
        {
            // this should be get or create leave ledger
            var ledger = await _context.LeaveLedgers
                .Include(x => x.LastTransaction)
                .Where(x => x.EmployeeID == employeeId)
                .Where(x => x.ProductID == leaveTypeId)
                .FirstOrDefaultAsync();

            if (ledger == null) return;

            var newTransaction = new LeaveTransaction()
            {
                LeaveLedgerID = ledger.RowID,
                EmployeeID = employeeId,
                CreatedBy = userId,
                OrganizationID = organizationId,
                Type = LeaveTransactionType.Credit,
                TransactionDate = DateTime.Now,
                Amount = balance,
                Description = "Beginning Balance",
                Balance = balance
            };

            _context.LeaveTransactions.Add(newTransaction);
            ledger.LastTransaction = newTransaction;

            await _context.SaveChangesAsync();
        }
    }
}