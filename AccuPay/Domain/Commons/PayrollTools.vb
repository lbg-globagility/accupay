Option Strict On
Imports AccuPay.Entity

Public Class PayrollTools

    Public Const MonthsPerYear As Integer = 12

    Public Const WorkHoursPerDay As Integer = 8

    Public Shared Function GetEmployeeMonthlyRate(
                            employee As Employee,
                            basicSalary As Decimal) As Decimal

        If employee.IsMonthly OrElse employee.IsFixed Then

            Return basicSalary

        ElseIf employee.IsDaily Then

            Return basicSalary * GetWorkDaysPerMonth(employee.WorkDaysPerYear)

        End If

        Return 0

    End Function

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
