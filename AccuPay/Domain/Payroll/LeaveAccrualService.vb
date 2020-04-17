Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Public Class LeaveAccrualService

    Private _calculator As LeaveAccrualCalculator

    Public Sub New()
        _calculator = New LeaveAccrualCalculator()
    End Sub

    Public Async Function ComputeAccrual(employee As Employee, payperiod As PayPeriod) As Task
        Using context = New PayrollContext()
            Dim firstPayperiodOfYear = Await context.PayPeriods.
                Where(Function(p) p.Year = payperiod.Year).
                Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                OrderBy(Function(p) p.PayFromDate).
                FirstOrDefaultAsync()

            Dim lastPayperiodOfYear = Await context.PayPeriods.
                Where(Function(p) p.Year = payperiod.Year).
                Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                OrderByDescending(Function(p) p.PayFromDate).
                FirstOrDefaultAsync()

            Await UpdateVacationLeaveLedger(context, employee, payperiod, firstPayperiodOfYear, lastPayperiodOfYear)
            Await UpdateSickLeaveLedger(context, employee, payperiod, firstPayperiodOfYear, lastPayperiodOfYear)

            'Dim sickLeaveType = Await context.Products.
            '    Where(Function(p) p.PartNo = ProductConstant.SICK_LEAVE).
            '    Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
            '    FirstOrDefaultAsync()

            'Dim ledger = Await context.LeaveLedgers.
            '    Where(Function(l) l.EmployeeID.Value = employee.RowID.Value).
            '    Where(Function(l) CBool(l.ProductID.Value = sickLeaveType.RowID)).
            '    FirstOrDefaultAsync()

            'Dim lastTransaction = Await context.LeaveTransactions.
            '    Where(Function(t) t.RowID.Value = ledger.LastTransactionID.Value).
            '    FirstOrDefaultAsync()

            'Dim leaveHours = _calculator.Calculate(payperiod, employee.SickLeaveAllowance, firstPayperiodOfYear, lastPayperiodOfYear)

            'Dim newTransaction = New LeaveTransaction() With {
            '    .LeaveLedgerID = ledger.RowID,
            '    .EmployeeID = employee.RowID,
            '    .CreatedBy = z_User,
            '    .OrganizationID = z_OrganizationID,
            '    .Type = LeaveTransactionType.Credit,
            '    .TransactionDate = payperiod.PayToDate,
            '    .Amount = leaveHours,
            '    .Balance = lastTransaction.Balance + leaveHours
            '}

            'context.LeaveTransactions.Add(newTransaction)
            'ledger.LastTransaction = newTransaction

            Await context.SaveChangesAsync()
        End Using
    End Function

    Private Async Function UpdateVacationLeaveLedger(context As PayrollContext,
                                                     employee As Employee,
                                                     payperiod As PayPeriod,
                                                     firstPayperiodOfYear As PayPeriod,
                                                     lastPayperiodOfYear As PayPeriod) As Task
        Dim leaveType = Await context.Products.
            Where(Function(p) p.PartNo = ProductConstant.SICK_LEAVE).
            Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
            FirstOrDefaultAsync()

        Dim ledger = Await context.LeaveLedgers.
            Where(Function(l) l.EmployeeID.Value = employee.RowID.Value).
            Where(Function(l) CBool(l.ProductID.Value = leaveType.RowID)).
            FirstOrDefaultAsync()

        Dim lastTransaction = Await context.LeaveTransactions.
            Where(Function(t) t.RowID.Value = ledger.LastTransactionID.Value).
            FirstOrDefaultAsync()

        Dim leaveHours = _calculator.Calculate(employee, payperiod, employee.VacationLeaveAllowance, firstPayperiodOfYear, lastPayperiodOfYear)

        Dim newTransaction = New LeaveTransaction() With {
            .LeaveLedgerID = ledger.RowID,
            .EmployeeID = employee.RowID,
            .CreatedBy = z_User,
            .OrganizationID = z_OrganizationID,
            .Type = LeaveTransactionType.Credit,
            .TransactionDate = payperiod.PayToDate,
            .Amount = leaveHours,
            .Balance = lastTransaction.Balance + leaveHours
        }

        context.LeaveTransactions.Add(newTransaction)
        ledger.LastTransaction = newTransaction
    End Function

    Private Async Function UpdateSickLeaveLedger(context As PayrollContext,
                                                 employee As Employee,
                                                 payperiod As PayPeriod,
                                                 firstPayperiodOfYear As PayPeriod,
                                                 lastPayperiodOfYear As PayPeriod) As Task
        Dim leaveType = Await context.Products.
            Where(Function(p) p.PartNo = ProductConstant.VACATION_LEAVE).
            Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
            FirstOrDefaultAsync()

        Dim ledger = Await context.LeaveLedgers.
            Where(Function(l) l.EmployeeID.Value = employee.RowID.Value).
            Where(Function(l) CBool(l.ProductID.Value = leaveType.RowID)).
            FirstOrDefaultAsync()

        Dim lastTransaction = Await context.LeaveTransactions.
            Where(Function(t) t.RowID.Value = ledger.LastTransactionID.Value).
            FirstOrDefaultAsync()

        Dim leaveHours = _calculator.Calculate(employee, payperiod, employee.SickLeaveAllowance, firstPayperiodOfYear, lastPayperiodOfYear)

        Dim newTransaction = New LeaveTransaction() With {
            .LeaveLedgerID = ledger.RowID,
            .EmployeeID = employee.RowID,
            .CreatedBy = z_User,
            .OrganizationID = z_OrganizationID,
            .Type = LeaveTransactionType.Credit,
            .TransactionDate = payperiod.PayToDate,
            .Amount = leaveHours,
            .Balance = lastTransaction.Balance + leaveHours
        }

        context.LeaveTransactions.Add(newTransaction)
        ledger.LastTransaction = newTransaction
    End Function

End Class
