Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Tools
Imports AccuPay.Utilities

Public Class LeaveAccrualCalculator

    Public Function Calculate(currentPayperiod As PayPeriod,
                              leaveAllowance As Decimal,
                              firstPayperiodOfYear As PayPeriod,
                              lastPayperiodOfYear As PayPeriod) As Decimal
        Dim daysInYear = Calendar.DaysBetween(firstPayperiodOfYear.PayFromDate, lastPayperiodOfYear.PayToDate)

        Dim startDateOfYear = firstPayperiodOfYear.PayFromDate

        Dim daysSinceLastCutoff = Calendar.DaysBetween(startDateOfYear, currentPayperiod.PayFromDate.AddDays(-1))
        Dim totalSinceLastCutoff = ComputeTotalLeave(daysSinceLastCutoff, daysInYear, leaveAllowance)

        Dim daysSinceCurrentCutoff = Calendar.DaysBetween(startDateOfYear, currentPayperiod.PayToDate)
        Dim totalSinceCurrentCutoff = ComputeTotalLeave(daysSinceCurrentCutoff, daysInYear, leaveAllowance)

        ' The difference between the total of the last cutoff and the total for the current cutoff will be the
        ' actual leave hours to be given.
        Dim leaveForCutoff = totalSinceCurrentCutoff - totalSinceLastCutoff

        Return leaveForCutoff
    End Function

    Private Function ComputeTotalLeave(daysPassed As Integer, daysInYear As Integer, leaveAllowance As Decimal) As Decimal
        Dim leaves = (daysPassed / daysInYear) * leaveAllowance

        Return AccuMath.CommercialRound(leaves)
    End Function

End Class
