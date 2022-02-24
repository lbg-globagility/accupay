using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data.Repositories
{
    public class CashoutUnusedLeaveRepository : ICashoutUnusedLeaveRepository
    {
        private readonly PayrollContext _context;
        private readonly IPayPeriodRepository _payPeriodRepository;

        public CashoutUnusedLeaveRepository(PayrollContext context,
            IPayPeriodRepository payPeriodRepository)
        {
            _context = context;
            _payPeriodRepository = payPeriodRepository;
        }

        public async Task ConsumeLeaveBalanceAsync(int organizationId, int userId, int payPeriodId, List<CashoutUnusedLeave> data)
        {
            var employeeIds = data.Select(d => d.EmployeeID).ToArray();
            var payPeriodIds = data.Select(d => d.PayPeriodID).ToArray();
            var orig = await _context.CashoutUnusedLeaves.
                Where(c => employeeIds.Contains(c.EmployeeID)).
                Where(c => payPeriodIds.Contains(c.PayPeriodID)).
                ToListAsync();

            var added = new List<CashoutUnusedLeave>();
            foreach (var d in data)
            {
                var match = orig.
                    Where(o => o.EmployeeID == d.EmployeeID).
                    Where(o => o.PayPeriodID == d.PayPeriodID).
                    FirstOrDefault();

                if (match != null)
                {
                    match.LeaveHours = d.LeaveHours;
                    match.Amount = d.Amount;
                    _context.Entry(match).State = EntityState.Modified;
                }
                else
                {
                    added.Add(d);
                    _context.CashoutUnusedLeaves.Add(d);
                }
            }

            var payPeriod = await _context.PayPeriods.FindAsync(payPeriodId);

            var employees = await _context.Employees.
                Where(l => employeeIds.Contains(l.RowID.Value)).
                ToListAsync();

            var leaveLedgerIds = added.Select(c => c.LeaveLedgerID).ToArray();
            var leaveLedgers = await _context.LeaveLedgers.
                Where(l => leaveLedgerIds.Contains(l.RowID.Value)).
                ToListAsync();
            foreach (var leaveLedger in leaveLedgers)
            {
                var cashoutLeave = added.
                    Where(c => c.EmployeeID == leaveLedger.EmployeeID).
                    Where(c => c.LeaveLedgerID == leaveLedger.RowID).
                    Where(c => c.PayPeriodID == payPeriodId).
                    FirstOrDefault();
                var lastTransaction = LeaveTransaction.NewLeaveTransaction(userId: userId,
                    organizationId: organizationId,
                    employeeId: leaveLedger.EmployeeID,
                    leaveLedgerId: leaveLedger.RowID,
                    payPeriodId: payPeriodId,
                    paystubId: cashoutLeave.PaystubID,
                    referenceId: null,
                    transactionDate: payPeriod.PayToDate,
                    description: "cashout unused leave",
                    type: "Debit",
                    amount: cashoutLeave.LeaveHours,
                    balance: 0);

                var matchLeaveTransaction = await _context.LeaveTransactions.
                    Where(l => l.OrganizationID == lastTransaction.OrganizationID).
                    Where(l => l.EmployeeID == lastTransaction.EmployeeID).
                    Where(l => l.LeaveLedgerID == lastTransaction.LeaveLedgerID).
                    Where(l => l.PayPeriodID == lastTransaction.PayPeriodID).
                    Where(l => l.PaystubID == lastTransaction.PaystubID).
                    Where(l => l.TransactionDate == lastTransaction.TransactionDate).
                    Where(l => l.Description == lastTransaction.Description).
                    Where(l => l.Type == lastTransaction.Type).
                    FirstOrDefaultAsync();
                if(matchLeaveTransaction == null)
                {
                    _context.LeaveTransactions.Add(lastTransaction);
                    leaveLedger.LastTransaction = lastTransaction;
                    leaveLedger.LastUpdBy = userId;
                    _context.Entry(leaveLedger).State = EntityState.Modified;
                }

                var employee = employees.
                    FirstOrDefault(e => e.RowID==leaveLedger.EmployeeID);
                if (cashoutLeave.IsVacation)
                {
                    employee.LastUpdBy = userId;
                    employee.LeaveBalance = 0;
                }
                if (cashoutLeave.IsSick)
                {
                    employee.LastUpdBy = userId;
                    employee.SickLeaveBalance = 0;
                }
                if (cashoutLeave.IsOthers)
                {
                    employee.LastUpdBy = userId;
                    employee.SetOtherLeaveBalance(0);
                }
                if (cashoutLeave.IsParental)
                {
                    employee.LastUpdBy = userId;
                    employee.SetParentalLeaveBalance(0);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<CashoutUnusedLeave>> GetByPeriodAsync(int payPeriodId)
        {
            var payPeriod = await _context.PayPeriods.FindAsync(payPeriodId);
            var payPeriodIds = (await _context.PayPeriods.
                    Where(p => p.Year == payPeriod.Year).
                    Where(p => p.PayFrequencyID == payPeriod.PayFrequencyID).
                    Where(p => p.OrganizationID == payPeriod.OrganizationID).
                    ToListAsync()).
                Select(p => p.RowID.Value).
                ToArray();

            return await _context.CashoutUnusedLeaves.
                Include(c => c.LeaveLedger).
                    ThenInclude(l => l.Product).
                Include(c => c.PayPeriod).
                Where(c => payPeriodIds.Contains(c.PayPeriodID)).
                ToListAsync();
        }

        public async Task<List<CashoutUnusedLeave>> GetFromLatestPeriodAsync(int organizationId)
        {
            var payPeriod = await _payPeriodRepository.GetLatestAsync(organizationId);

            return await GetByPeriodAsync(payPeriod.RowID.Value);
        }

        public async Task SaveManyAsync(List<CashoutUnusedLeave> data)
        {
            var employeeIds = data.Select(d => d.EmployeeID).ToArray();
            var payPeriodIds = data.Select(d => d.PayPeriodID).ToArray();
            var orig = await _context.CashoutUnusedLeaves.
                Where(c => employeeIds.Contains(c.EmployeeID)).
                Where(c => payPeriodIds.Contains(c.PayPeriodID)).
                ToListAsync();
            foreach (var d in data)
            {
                var match = orig.
                    Where(o => o.EmployeeID == d.EmployeeID).
                    Where(o => o.PayPeriodID == d.PayPeriodID).
                    FirstOrDefault();

                if (match != null)
                {
                    match.LeaveHours = d.LeaveHours;
                    match.Amount = d.Amount;
                    _context.Entry(match).State = EntityState.Modified;
                }else
                {
                    _context.CashoutUnusedLeaves.Add(d);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
