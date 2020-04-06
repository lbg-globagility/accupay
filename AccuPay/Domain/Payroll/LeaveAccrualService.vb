Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Utilities
Imports Microsoft.EntityFrameworkCore

Public Class LeaveAccrualService

    Private _daysInPayrollYear As Integer

    Public Sub New()
        _daysInPayrollYear = 366
    End Sub

    Public Async Sub ComputeAccrual(employee As Employee, payperiod As PayPeriod)
        Using context = New PayrollContext()
            Dim firstPayperiod = Await context.PayPeriods.
                Where(Function(p) p.Year = payperiod.Year AndAlso p.Month = 1).
                FirstOrDefaultAsync()

            Dim startDateOfYear = firstPayperiod.PayFromDate

            Dim timeSinceLastCutoff = payperiod.PayFromDate.AddDays(-1) - startDateOfYear
            Dim leaveSinceLastCutoff = ComputeLeaveAmount(timeSinceLastCutoff, employee)

            Dim timeSinceCurrentCutoff = payperiod.PayToDate - startDateOfYear
            Dim leaveSinceCurrentCutoff = ComputeLeaveAmount(timeSinceCurrentCutoff, employee)

            ' Get the difference
            Dim leaveForCutoff = leaveSinceLastCutoff - leaveSinceCurrentCutoff
        End Using
    End Sub

    Private Function ComputeLeaveAmount(time As TimeSpan, employee As Employee) As Decimal
        Dim leaves = (time.Days / 366D) * employee.SickLeaveAllowance

        Return AccuMath.CommercialRound(leaves)
    End Function

    Private Function GetDaysInYear(year As Integer) As Integer
        Dim firstDay = New DateTime(year, 1, 1)
        Dim lastDay = New DateTime(year, 12, 31)

        Return (lastDay - firstDay).Days
    End Function

End Class
