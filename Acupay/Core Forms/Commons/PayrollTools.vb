Option Strict On

Public Class PayrollTools

    Public Const MonthsPerYear As Integer = 12

    Public Const WorkHoursPerDay As Integer = 8

    Public Shared Function GetWorkDaysPerMonth(workDaysPerYear As Decimal) As Decimal
        Return workDaysPerYear / MonthsPerYear
    End Function

    Public Shared Function GetDailyRate(monthlySalary As Decimal, workDaysPerYear As Decimal) As Decimal
        Return monthlySalary / GetWorkDaysPerMonth(workDaysPerYear)
    End Function

    Public Shared Function GetHourlyRate(monthlySalary As Decimal, workDaysPerYear As Decimal) As Decimal
        Return monthlySalary / GetWorkDaysPerMonth(workDaysPerYear) / WorkHoursPerDay
    End Function

End Class
