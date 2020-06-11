using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class LeaveAccrualService
    {
        private readonly LeaveAccrualCalculator _calculator;

        private readonly PayrollContext context;

        public LeaveAccrualService(PayrollContext context)
        {
            _calculator = new LeaveAccrualCalculator();
            this.context = context;
        }

        public async Task CheckAccruals(int organizationId, int userId)
        {
            ICollection<Employee> employees;

            employees = await context.Employees
                .Where(e => e.OrganizationID == organizationId)
                .ToListAsync();

            foreach (var employee in employees)
            {
                await ComputeAccrual2(employee, organizationId, userId);
            }
        }

        public async Task ComputeAccrual(Employee employee, PayPeriod payperiod, int z_OrganizationID, int z_User)
        {
            var startOfFirstYear = employee.StartDate;

            var firstPayperiodOfYear = await context.PayPeriods
                .Where(p => p.PayFromDate <= startOfFirstYear && startOfFirstYear <= p.PayToDate)
                .Where(p => p.OrganizationID == z_OrganizationID)
                .FirstOrDefaultAsync();

            var endOfFirstYear = employee.StartDate.AddYears(1);

            var lastPayperiodOfYear = await context.PayPeriods
                .Where(p => p.PayFromDate <= endOfFirstYear && endOfFirstYear <= p.PayToDate)
                .Where(p => p.OrganizationID == z_OrganizationID)
                .FirstOrDefaultAsync();

            await UpdateVacationLeaveLedger(context, employee, payperiod, firstPayperiodOfYear, lastPayperiodOfYear, z_OrganizationID, z_User);
            await UpdateSickLeaveLedger(context, employee, payperiod, firstPayperiodOfYear, lastPayperiodOfYear, z_OrganizationID, z_User);

            await context.SaveChangesAsync();
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
                .Where(p => p.OrganizationID == z_OrganizationID)
                .FirstOrDefaultAsync();

            var ledger = await context.LeaveLedgers
                .Where(l => l.EmployeeID == employee.RowID)
                .Where(l => l.ProductID == leaveType.RowID)
                .FirstOrDefaultAsync();

            var lastTransaction = await context.LeaveTransactions
                .Where(t => t.RowID == ledger.LastTransactionID)
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
                .Where(p => p.OrganizationID == z_OrganizationID)
                .FirstOrDefaultAsync();

            var ledger = await context.LeaveLedgers
                .Where(l => l.EmployeeID == employee.RowID)
                .Where(l => l.ProductID == leaveType.RowID)
                .FirstOrDefaultAsync();

            var lastTransaction = await context.LeaveTransactions
                .Where(t => t.RowID == ledger.LastTransactionID)
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
            try
            {
                await UpdateLeaveLedger(context, employee, employee.VacationLeaveAllowance, ProductConstant.VACATION_LEAVE, z_OrganizationID, z_User);
                await UpdateLeaveLedger(context, employee, employee.SickLeaveAllowance, ProductConstant.SICK_LEAVE, z_OrganizationID, z_User);

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
        }

        private async Task UpdateLeaveLedger(
            PayrollContext context,
            Employee employee,
            decimal leaveAllowance,
            string leaveTypeName,
            int organizationId,
            int userId)
        {
            var leaveType = await context.Products
                .Where(p => p.PartNo == leaveTypeName)
                .Where(p => p.OrganizationID == organizationId)
                .FirstOrDefaultAsync();

            var ledger = await context.LeaveLedgers.
                Where(l => l.EmployeeID == employee.RowID).
                Where(l => l.ProductID == leaveType.RowID).
                FirstOrDefaultAsync();

            var lastAccrual = await context.LeaveTransactions
                .Where(t => t.LeaveLedgerID == ledger.RowID)
                .Where(t => t.Description == "Accrual")
                .OrderByDescending(t => t.TransactionDate)
                .FirstOrDefaultAsync();

            var lastTransaction = await context.LeaveTransactions
                .Where(t => t.RowID == ledger.LastTransactionID)
                .FirstOrDefaultAsync();

            var currentDate = DateTime.Today;

            while (true)
            {
                var nextAccrualDate = lastAccrual == null
                    ? employee.StartDate.AddMonths(1).AddDays(1)
                    : lastAccrual.TransactionDate.AddMonths(1);
                
                var finalAccrualDate = employee.StartDate.AddMonths(12).AddDays(1);

                // Check if the current date is still too early to update the ledger
                if (currentDate < nextAccrualDate)
                    return;

                // If the next accrual date is past the final accrual, then we stop
                // accruing the employee
                if (finalAccrualDate < nextAccrualDate)
                    return;

                var leaveHours = _calculator.Calculate2(employee, nextAccrualDate, leaveAllowance, lastAccrual);

                if (leaveHours == 0)
                    return;

                var newTransaction = new LeaveTransaction()
                {
                    LeaveLedgerID = ledger.RowID,
                    EmployeeID = employee.RowID,
                    CreatedBy = userId,
                    OrganizationID = organizationId,
                    Type = LeaveTransactionType.Credit,
                    TransactionDate = nextAccrualDate,
                    Amount = leaveHours,
                    Description = "Accrual",
                    Balance = (lastTransaction?.Balance ?? 0) + leaveHours
                };

                employee.LeaveBalance += leaveHours;
                context.LeaveTransactions.Add(newTransaction);
                ledger.LastTransaction = newTransaction;

                lastTransaction = newTransaction;
                lastAccrual = newTransaction;
            }
        }
    }
}