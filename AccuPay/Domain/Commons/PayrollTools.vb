Option Strict On
Imports AccuPay.Entity

Public Class PayrollTools

    Public Const MonthsPerYear As Integer = 12

    Public Const WorkHoursPerDay As Integer = 8

    Public Const DivisorToDailyRate As Integer = 8

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

    Public Shared Function HasWorkedLastWorkingDay(
                            currentDate As Date,
                            currentTimeEntries As IList(Of TimeEntry),
                            payratesCalendar As PayratesCalendar) As Boolean

        Dim lastPotentialEntry = currentDate.Date.AddDays(-3)

        Dim lastTimeEntries = currentTimeEntries.
            Where(Function(t) lastPotentialEntry <= t.Date And t.Date <= currentDate.Date).
            OrderByDescending(Function(t) t.Date).
            ToList()

        For Each lastTimeEntry In lastTimeEntries
            ' If employee has no shift set for the day, it's not a working day.
            If lastTimeEntry.HasShift = False Then
                Continue For
            End If

            If lastTimeEntry.IsRestDay Then

                If lastTimeEntry.TotalDayPay > 0 Then
                    Return True
                End If

                Continue For
            End If

            Dim payRate = payratesCalendar.Find(lastTimeEntry.Date)
            If payRate.IsHoliday Then
                If lastTimeEntry.TotalDayPay > 0 Then
                    Return True
                End If

                Continue For
            End If

            Return lastTimeEntry.RegularHours > 0 Or lastTimeEntry.TotalLeaveHours > 0
        Next

        Return False
    End Function

End Class
