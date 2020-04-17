Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Tools
Imports AccuPay.Utilities

Public Class LeaveAccrualCalculator

    Public Function Calculate(employee As Employee,
                              currentPayperiod As PayPeriod,
                              leaveAllowance As Decimal,
                              firstPayperiodOfYear As PayPeriod,
                              lastPayperiodOfYear As PayPeriod) As Decimal
        Dim daysInYear = Calendar.DaysBetween(firstPayperiodOfYear.PayFromDate, lastPayperiodOfYear.PayToDate)

        If currentPayperiod.PayFromDate < employee.StartDate Then
            Dim daysInCutoff = Calendar.DaysBetween(employee.StartDate, currentPayperiod.PayToDate)
            Dim leaveHours = ComputeTotalLeave(daysInCutoff, daysInYear, leaveAllowance)

            Return leaveHours
        End If

        If currentPayperiod.RowID = firstPayperiodOfYear.RowID Then
            Dim daysInCutoff = Calendar.DaysBetween(currentPayperiod.PayFromDate, currentPayperiod.PayToDate)
            Dim leaveHours = ComputeTotalLeave(daysInCutoff, daysInYear, leaveAllowance)

            Return leaveHours
        Else
            Dim startDateOfYear = firstPayperiodOfYear.PayFromDate

            Dim daysSinceLastCutoff = Calendar.DaysBetween(startDateOfYear, currentPayperiod.PayFromDate.AddDays(-1))
            Dim totalSinceLastCutoff = ComputeTotalLeave(daysSinceLastCutoff, daysInYear, leaveAllowance)

            Dim daysSinceCurrentCutoff = Calendar.DaysBetween(startDateOfYear, currentPayperiod.PayToDate)
            Dim totalSinceCurrentCutoff = ComputeTotalLeave(daysSinceCurrentCutoff, daysInYear, leaveAllowance)

            ' The difference between the total of the last cutoff and the total for the current cutoff will be the
            ' actual leave hours to be given.
            Dim leaveHours = totalSinceCurrentCutoff - totalSinceLastCutoff

            Return leaveHours
        End If
    End Function

    Private Function ComputeTotalLeave(daysPassed As Integer, daysInYear As Integer, leaveAllowance As Decimal) As Decimal
        Dim leaves = (daysPassed / daysInYear) * leaveAllowance

        Return AccuMath.CommercialRound(leaves)
    End Function

End Class
