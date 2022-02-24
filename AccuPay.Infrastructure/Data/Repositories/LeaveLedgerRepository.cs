using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class LeaveLedgerRepository : ILeaveLedgerRepository
    {
        private readonly PayrollContext _context;

        public LeaveLedgerRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<ICollection<LeaveLedger>> GetAll(int organizationId)
        {
            return await _context.LeaveLedgers
                //.AsNoTracking()
                .Include(t => t.Product)
                .Include(t => t.LeaveTransactions)
                .Where(t => t.OrganizationID == organizationId)
                .ToListAsync();
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

        public async Task CreateBeginningBalanceAsync(int employeeId,
            int leaveTypeId,
            int userId,
            int organizationId,
            decimal balance,
            string description = "",
            DateTime? transactionDate = null)
        {
            // this should be get or create leave ledger
            var ledger = await _context.LeaveLedgers
                .Include(x => x.LastTransaction)
                .Where(x => x.EmployeeID == employeeId)
                .Where(x => x.ProductID == leaveTypeId)
                .FirstOrDefaultAsync();

            if (ledger == null) return;

            var newTransaction = LeaveTransaction.NewLeaveTransaction(userId: userId,
                organizationId: organizationId,
                employeeId: employeeId,
                leaveLedgerId: ledger.RowID,
                payPeriodId: null,
                paystubId: null,
                referenceId: null,
                transactionDate: transactionDate ?? DateTime.Now,
                description: description,
                type: LeaveTransactionType.Credit,
                amount: balance,
                balance: balance);

            _context.LeaveTransactions.Add(newTransaction);
            ledger.LastTransaction = newTransaction;

            await _context.SaveChangesAsync();
        }

        public async Task CreateManyLeaveTransactionsAsync(IList<LeaveTransaction> leaveTransactions)
        {
            if (leaveTransactions == null) return;

            //if (leaveLedger == null && leaveLedger?.LeaveTransactions == null) throw new LeaveResetException(message: "leaveLedger doesn't exists");

            await _context.LeaveTransactions.AddRangeAsync(leaveTransactions);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateManyAsync(IList<LeaveLedger> leaveLedgers)
        {
            foreach (var leaveLedger in leaveLedgers)
            {
                _context.Entry(leaveLedger).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteManyLeaveTransactionsAsync(IList<LeaveTransaction> leaveTransactions)
        {
            foreach (var leaveTransaction in leaveTransactions)
            {
                _context.Entry(leaveTransaction).State = EntityState.Deleted;
            }

            await _context.SaveChangesAsync();
        }
    }
}
