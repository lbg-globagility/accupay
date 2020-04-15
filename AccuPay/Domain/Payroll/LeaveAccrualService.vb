Option Strict On

Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Public Class LeaveAccrualService

    Private _calculator As LeaveAccrualCalculator

    Public Sub New()
        _calculator = New LeaveAccrualCalculator()
    End Sub

    Public Async Sub ComputeAccrual(employee As Employee, payperiod As PayPeriod)
        Using context = New PayrollContext()
            Dim firstPayperiodOfYear = Await context.PayPeriods.
                Where(Function(p) p.Year = payperiod.Year).
                OrderBy(Function(p) p.PayFromDate).
                FirstOrDefaultAsync()

            Dim lastPayperiodOfYear = Await context.PayPeriods.
                Where(Function(p) p.Year = payperiod.Year).
                OrderByDescending(Function(p) p.PayFromDate).
                FirstOrDefaultAsync()

            _calculator.Calculate(payperiod, employee.SickLeaveAllowance, firstPayperiodOfYear, lastPayperiodOfYear)
        End Using
    End Sub

End Class
