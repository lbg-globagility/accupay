using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class LeaveAccrualService
    {
        private LeaveAccrualCalculator _calculator;

        public LeaveAccrualService()
        {
            _calculator = new LeaveAccrualCalculator();
        }

        public async Task ComputeAccrual(Employee employee, PayPeriod payperiod, int z_OrganizationID, int z_User)
        {
            using (var context = new PayrollContext())
            {
                var startOfFirstYear = employee.StartDate;

                var firstPayperiodOfYear = await context.PayPeriods
                    .Where(p => p.PayFromDate <= startOfFirstYear && startOfFirstYear <= p.PayToDate)
                    .Where(p => p.OrganizationID.Value == z_OrganizationID)
                    .FirstOrDefaultAsync();

                var endOfFirstYear = employee.StartDate.AddYears(1);

                var lastPayperiodOfYear = await context.PayPeriods
                    .Where(p => p.PayFromDate <= endOfFirstYear && endOfFirstYear <= p.PayToDate)
                    .Where(p => p.OrganizationID.Value == z_OrganizationID)
                    .FirstOrDefaultAsync();

                await UpdateVacationLeaveLedger(context, employee, payperiod, firstPayperiodOfYear, lastPayperiodOfYear, z_OrganizationID, z_User);
                await UpdateSickLeaveLedger(context, employee, payperiod, firstPayperiodOfYear, lastPayperiodOfYear, z_OrganizationID, z_User);

                await context.SaveChangesAsync();
            }
        }

        private async Task UpdateVacationLeaveLedger(PayrollContext context,
                                                     Employee employee,
                                                     PayPeriod payperiod,
                                                     PayPeriod firstPayperiodOfYear,
                                                     PayPeriod lastPayperiodOfYear,
                                                     int z_OrganizationID,
                                                     int z_User)
        {
            var leaveType = await context.Products
                .Where(p => p.PartNo == ProductConstant.SICK_LEAVE)
                .Where(p => p.OrganizationID.Value == z_OrganizationID)
                .FirstOrDefaultAsync();

            var ledger = await context.LeaveLedgers
                .Where(l => l.EmployeeID.Value == employee.RowID.Value)
                .Where(l => l.ProductID.Value == leaveType.RowID)
                .FirstOrDefaultAsync();

            var lastTransaction = await context.LeaveTransactions
                .Where(t => t.RowID.Value == ledger.LastTransactionID)
                .FirstOrDefaultAsync();

            var existingTransaction = await context.LeaveTransactions
                .Where(t => t.TransactionDate == payperiod.PayToDate)
                .Where(t => t.Description == "Accrual")
                .FirstOrDefaultAsync();

            if (existingTransaction != null)
                return;

            var leaveHours = _calculator.Calculate(employee, payperiod, employee.VacationLeaveAllowance, firstPayperiodOfYear, lastPayperiodOfYear);

            var newTransaction = new LeaveTransaction()
            {
                LeaveLedgerID = ledger.RowID,
                EmployeeID = employee.RowID,
                CreatedBy = z_User,
                OrganizationID = z_OrganizationID,
                Type = LeaveTransactionType.Credit,
                TransactionDate = payperiod.PayToDate,
                Amount = leaveHours,
                Description = "Accrual",
                Balance = lastTransaction.Balance + leaveHours
            };

            context.LeaveTransactions.Add(newTransaction);
            ledger.LastTransaction = newTransaction;
        }

        private async Task UpdateSickLeaveLedger(PayrollContext context,
                                                 Employee employee,
                                                 PayPeriod payperiod,
                                                 PayPeriod firstPayperiodOfYear,
                                                 PayPeriod lastPayperiodOfYear,
                                                 int z_OrganizationID,
                                                 int z_User)
        {
            var leaveType = await context.Products
                .Where(p => p.PartNo == ProductConstant.VACATION_LEAVE)
                .Where(p => p.OrganizationID.Value == z_OrganizationID)
                .FirstOrDefaultAsync();

            var ledger = await context.LeaveLedgers
                .Where(l => l.EmployeeID.Value == employee.RowID.Value)
                .Where(l => l.ProductID.Value == leaveType.RowID)
                .FirstOrDefaultAsync();

            var lastTransaction = await context.LeaveTransactions
                .Where(t => t.RowID.Value == ledger.LastTransactionID)
                .FirstOrDefaultAsync();

            var existingTransaction = await context.LeaveTransactions
                .Where(t => t.TransactionDate == payperiod.PayToDate)
                .Where(t => t.Description == "Accrual")
                .FirstOrDefaultAsync();

            if (existingTransaction != null)
                return;

            var leaveHours = _calculator.Calculate(employee, payperiod, employee.SickLeaveAllowance, firstPayperiodOfYear, lastPayperiodOfYear);

            var newTransaction = new LeaveTransaction()
            {
                LeaveLedgerID = ledger.RowID,
                EmployeeID = employee.RowID,
                CreatedBy = z_User,
                OrganizationID = z_OrganizationID,
                Type = LeaveTransactionType.Credit,
                TransactionDate = payperiod.PayToDate,
                Amount = leaveHours,
                Description = "Accrual",
                Balance = lastTransaction.Balance + leaveHours
            };

            context.LeaveTransactions.Add(newTransaction);
            ledger.LastTransaction = newTransaction;
        }

        public async Task ComputeAccrual2(Employee employee, int z_OrganizationID, int z_User)
        {
            using (var context = new PayrollContext())
            {
                await UpdateVacationLeaveLedger2(context, employee, z_OrganizationID, z_User);
                await UpdateSickLeaveLedger2(context, employee, z_OrganizationID, z_User);

                await context.SaveChangesAsync();
            }
        }

        private async Task UpdateVacationLeaveLedger2(PayrollContext context, Employee employee, int z_OrganizationID, int z_User)
        {
            var leaveType = await context.Products
                .Where(p => p.PartNo == ProductConstant.VACATION_LEAVE)
                .Where(p => p.OrganizationID.Value == z_OrganizationID)
                .FirstOrDefaultAsync();

            var ledger = await context.LeaveLedgers.
                Where(l => l.EmployeeID.Value == employee.RowID.Value).
                Where(l => l.ProductID.Value == leaveType.RowID).FirstOrDefaultAsync();

            var lastTransaction = await context.LeaveTransactions
                .Where(t => t.RowID.Value == ledger.LastTransactionID.GetValueOrDefault())
                .Where(t => t.Description == "Accrual")
                .OrderByDescending(t => t.TransactionDate).FirstOrDefaultAsync();

            var leaveHours = _calculator.Calculate2(employee, DateTime.Now, employee.VacationLeaveAllowance, lastTransaction);

            var newTransaction = new LeaveTransaction()
            {
                LeaveLedgerID = ledger.RowID,
                EmployeeID = employee.RowID,
                CreatedBy = z_User,
                OrganizationID = z_OrganizationID,
                Type = LeaveTransactionType.Credit,
                TransactionDate = DateTime.Now,
                Amount = leaveHours,
                Description = "Accrual",
                Balance = lastTransaction.Balance + leaveHours
            };

            context.LeaveTransactions.Add(newTransaction);
            ledger.LastTransaction = newTransaction;
        }

        private async Task UpdateSickLeaveLedger2(PayrollContext context, Employee employee, int z_OrganizationID, int z_User)
        {
            var leaveType = await context.Products
                .Where(p => p.PartNo == ProductConstant.SICK_LEAVE)
                .Where(p => p.OrganizationID.Value == z_OrganizationID)
                .FirstOrDefaultAsync();

            var ledger = await context.LeaveLedgers
                .Where(l => l.EmployeeID.Value == employee.RowID.Value)
                .Where(l => System.Convert.ToBoolean(l.ProductID.Value == leaveType.RowID))
                .FirstOrDefaultAsync();

            var lastTransaction = await context.LeaveTransactions
                .Where(t => t.RowID.Value == ledger.LastTransactionID.GetValueOrDefault())
                .Where(t => t.Description == "Accrual")
                .OrderByDescending(t => t.TransactionDate)
                .FirstOrDefaultAsync();

            var leaveHours = _calculator.Calculate2(employee, DateTime.Now, employee.VacationLeaveAllowance, lastTransaction);

            var newTransaction = new LeaveTransaction()
            {
                LeaveLedgerID = ledger.RowID,
                EmployeeID = employee.RowID,
                CreatedBy = z_User,
                OrganizationID = z_OrganizationID,
                Type = LeaveTransactionType.Credit,
                TransactionDate = DateTime.Now,
                Amount = leaveHours,
                Description = "Accrual",
                Balance = lastTransaction.Balance + leaveHours
            };

            context.LeaveTransactions.Add(newTransaction);
            ledger.LastTransaction = newTransaction;
        }
    }
}
