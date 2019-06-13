Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.SimplifiedEntities
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Public Class PayrollTools

    Public Const MonthsPerYear As Integer = 12

    Public Const WorkHoursPerDay As Integer = 8

    Public Const DivisorToDailyRate As Integer = 8

    Public Const PayFrequencyMonthlyId As Integer = 1

    Public Const PayFrequencyWeeklyId As Integer = 4

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

    Public Shared Function GetDailyRate(monthlyRate As Decimal, workDaysPerYear As Decimal) As Decimal
        Return monthlyRate / GetWorkDaysPerMonth(workDaysPerYear)
    End Function

    Public Shared Function GetDailyRate(salary As Salary, employee As Employee) As Decimal
        Dim dailyRate = 0D

        If salary Is Nothing Then
            Return 0
        End If

        If employee.IsDaily Then
            dailyRate = salary.BasicSalary
        ElseIf employee.IsMonthly Or employee.IsFixed Then
            If employee.WorkDaysPerYear = 0 Then Return 0
            dailyRate = salary.BasicSalary / (employee.WorkDaysPerYear / 12)
        End If

        Return dailyRate
    End Function

    Public Shared Function GetHourlyRateByMonthlyRate(monthlyRate As Decimal, workDaysPerYear As Decimal) As Decimal
        Return monthlyRate / GetWorkDaysPerMonth(workDaysPerYear) / WorkHoursPerDay
    End Function

    Public Shared Function GetHourlyRateByDailyRate(dailyRate As Decimal) As Decimal
        Return dailyRate / WorkHoursPerDay
    End Function

    Public Shared Function GetHourlyRateByDailyRate(salary As Salary, employee As Employee) As Decimal

        Return GetDailyRate(salary, employee) / employee.WorkDaysPerYear
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

    Friend Shared Async Function GetCurrentlyWorkedOnPayPeriodByCurrentYear(
                                    Optional payperiods As IList(Of IPayPeriod) = Nothing) As Task(Of IPayPeriod)

        If payperiods Is Nothing OrElse payperiods.Count = 0 Then

            Using context = New PayrollContext()
                Dim pastPayPeriods = Await context.PayPeriods.
                        Where(Function(p) p.Year = Now.Year).
                        Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                        Where(Function(p) p.IsMonthly).
                        ToListAsync()

                payperiods = New List(Of IPayPeriod)(pastPayPeriods)
            End Using

        End If

        Return payperiods.
                Where(Function(p) p.PayToDate < Date.Now).
                LastOrDefault

    End Function

End Class