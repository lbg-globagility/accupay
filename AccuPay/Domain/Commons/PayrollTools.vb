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
    Private Const threeDays As Integer = 3

    Public Shared Function GetEmployeeMonthlyRate(
                            employee As Employee,
                            salary As Salary,
                            Optional isActual As Boolean = False) As Decimal

        Dim basicSalary = If(isActual, salary.BasicSalary + salary.AllowanceSalary, salary.BasicSalary)

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

    Public Shared Function GetDailyRate(salary As Salary, employee As Employee, Optional isActual As Boolean = False) As Decimal
        Dim dailyRate = 0D

        If salary Is Nothing Then
            Return 0
        End If

        Dim basicSalary = If(isActual, salary.BasicSalary + salary.AllowanceSalary, salary.BasicSalary)

        If employee.IsDaily Then
            dailyRate = basicSalary
        ElseIf employee.IsMonthly OrElse employee.IsFixed Then
            If employee.WorkDaysPerYear = 0 Then Return 0
            dailyRate = basicSalary / (employee.WorkDaysPerYear / 12)
        End If

        Return dailyRate
    End Function

    Public Shared Function GetHourlyRateByMonthlyRate(monthlyRate As Decimal, workDaysPerYear As Decimal) As Decimal
        Return monthlyRate / GetWorkDaysPerMonth(workDaysPerYear) / WorkHoursPerDay
    End Function

    Public Shared Function GetHourlyRateByDailyRate(dailyRate As Decimal) As Decimal
        Return dailyRate / WorkHoursPerDay
    End Function

    Public Shared Function GetHourlyRateByDailyRate(salary As Salary, employee As Employee, Optional isActual As Boolean = False) As Decimal

        Return GetDailyRate(salary, employee, isActual) / WorkHoursPerDay
    End Function

    Public Shared Function HasWorkedLastWorkingDay(
                            currentDate As Date,
                            currentTimeEntries As IList(Of TimeEntry),
                            payratesCalendar As PayratesCalendar) As Boolean

        Dim threeDaysPrior = threeDays * -1
        Dim lastPotentialEntry = currentDate.Date.AddDays(threeDaysPrior)

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

    Public Shared Function HasWorkAfterLegalHoliday(
                            legalHolidayDate As Date,
                            endOfCutOff As Date,
                            currentTimeEntries As IList(Of TimeEntry),
                            payratesCalendar As PayratesCalendar) As Boolean

        Dim thirdDateAfterCurrDate = legalHolidayDate.Date.AddDays(threeDays)

        Dim postTimeEntries = currentTimeEntries.
            Where(Function(t) legalHolidayDate.Date < t.Date And t.Date <= thirdDateAfterCurrDate).
            OrderBy(Function(t) t.Date).
            ToList()

        For Each timeEntry In postTimeEntries
            If timeEntry.HasShift = False Then
                Continue For
            End If

            If timeEntry.IsRestDay Then

                If timeEntry.TotalDayPay > 0 Then
                    Return True
                End If

                Continue For
            End If

            Dim payRate = payratesCalendar.Find(timeEntry.Date)
            If payRate.IsRegularHoliday Then
                If timeEntry.TotalDayPay > 0 Then
                    Return True
                End If

                Continue For
            End If

            Return timeEntry.RegularHours > 0 Or timeEntry.TotalLeaveHours > 0
        Next

        'If holiday exactly falls in ending date of cut-off, and no attendance 3days after it
        'will treat it that employee was present
        If Not postTimeEntries.Any() And endOfCutOff = legalHolidayDate Then Return True

        Return False
    End Function

    Friend Shared Async Function GetCurrentlyWorkedOnPayPeriodByCurrentYear(
                                    Optional payperiods As IList(Of IPayPeriod) = Nothing) As Task(Of IPayPeriod)

        If payperiods Is Nothing OrElse payperiods.Count = 0 Then

            Using context = New PayrollContext()
                Return Await context.PayPeriods.
                        Where(Function(p) p.Year = Now.Year).
                        Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                        Where(Function(p) p.IsMonthly).
                        Where(Function(p) p.PayToDate < Date.Now).
                        OrderByDescending(Function(p) p.PayToDate).
                        FirstOrDefaultAsync

            End Using
        Else

            Return payperiods.
                Where(Function(p) p.PayToDate < Date.Now).
                LastOrDefault

        End If

    End Function

End Class