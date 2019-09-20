Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.SimplifiedEntities
Imports AccuPay.Utils
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

    Public Shared Function CheckIfUsingUserLevel() As Boolean

        Using context As New PayrollContext

            Dim settings = New ListOfValueCollection(context.ListOfValues.ToList())

            If settings.GetBoolean("User Policy.UseUserLevel", False) Then
                Return True
            End If

        End Using

        Return False

    End Function

    Public Shared Async Function ValidatePayPeriodAction(payPeriodId As Integer?) As Task(Of Boolean)

        Using context As New PayrollContext

            If payPeriodId Is Nothing Then
                MessageBoxHelper.Warning("Pay period does not exists. Please refresh the form.")
                Return False
            End If

            Dim payPeriod = Await context.PayPeriods.
                                FirstOrDefaultAsync(Function(p) p.RowID.Value = payPeriodId.Value)

            If payPeriod Is Nothing Then
                MessageBoxHelper.Warning("Pay period does not exists. Please refresh the form.")
                Return False
            End If

            Dim otherProcessingPayPeriod = Await context.Paystubs.
                        Include(Function(p) p.PayPeriod).
                        Where(Function(p) p.PayPeriod.RowID.Value <> payPeriodId.Value).
                        Where(Function(p) p.PayPeriod.IsClosed = False).
                        Where(Function(p) p.PayPeriod.OrganizationID.Value = z_OrganizationID).
                        FirstOrDefaultAsync()

            If payPeriod.IsClosed Then

                MessageBoxHelper.Warning("The pay period you selected is already closed. Please reopen so you can alter the data for that pay period. If there are ""Processing"" pay periods, make sure to close them first.")
                Return False

            ElseIf Not payPeriod.IsClosed AndAlso otherProcessingPayPeriod IsNot Nothing Then

                MessageBoxHelper.Warning("There is currently a pay period with ""PROCESSING"" status. Please finish that pay period first then close it to process other open pay periods.")
                Return False

            End If
        End Using

        Return True

    End Function

    Public Shared Function GetNextPayPeriod(payPeriodId As Integer?) As PayPeriod

        If payPeriodId Is Nothing Then Return Nothing

        Using context As New PayrollContext

            Dim currentPayPeriod = context.PayPeriods.
                        FirstOrDefault(Function(p) p.RowID.Value = payPeriodId.Value)

            If currentPayPeriod Is Nothing Then Return Nothing

            Return context.PayPeriods.
                                Where(Function(p) p.OrganizationID.Value = currentPayPeriod.OrganizationID.Value).
                                Where(Function(p) p.PayFrequencyID.Value = currentPayPeriod.PayFrequencyID.Value).
                                Where(Function(p) p.PayFromDate > currentPayPeriod.PayFromDate).
                                OrderBy(Function(p) p.PayFromDate).
                                FirstOrDefault

        End Using

    End Function

End Class