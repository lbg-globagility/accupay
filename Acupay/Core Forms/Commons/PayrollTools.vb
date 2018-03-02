Option Strict On

Public Class PayrollTools

    Public Const MonthsPerYear As Integer = 12

    Public Shared Function GetWorkDaysPerMonth(workDaysPerYear As Decimal) As Decimal
        Return workDaysPerYear / MonthsPerYear
    End Function

End Class
