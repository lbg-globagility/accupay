Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports AccuPay.Repository
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

                Dim today = Date.Today.ToMinimumHourValue

                Dim payPeriodThisDay = Await context.PayPeriods.
                                        Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                                        Where(Function(p) p.IsMonthly).
                                        Where(Function(p) p.PayToDate >= today).
                                        Where(Function(p) p.PayFromDate <= today).
                                        OrderByDescending(Function(p) p.PayToDate).
                                        FirstOrDefaultAsync

                'This is done to ensure that we get the correct year.
                'For scenarios like The first cutoff for 2019 is Dec. 16-31, 2018
                'workingYearThisDay for December 16 should be 2019, not 2018.
                Dim workingYearThisDay = payPeriodThisDay?.Year

                If workingYearThisDay Is Nothing Then

                    Return Nothing

                End If

                Return Await context.PayPeriods.
                        Where(Function(p) p.Year = workingYearThisDay.Value).
                        Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
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

    Friend Shared Async Function GetOrCreateEmployeeEcola(
                            employeeId As Integer,
                            payDateFrom As Date,
                            payDateTo As Date,
                            Optional allowanceFrequency As String = Allowance.FREQUENCY_SEMI_MONTHLY,
                            Optional amount As Decimal = 0,
                            Optional effectiveEndDateShouldBeNull As Boolean = False) _
        As Task(Of Allowance)

        Dim allowanceRepository As New AllowanceRepository
        Dim productRepository As New ProductRepository

        Using context = New PayrollContext()
            Dim ecolaAllowance As Allowance = Await GetEmployeeEcola(employeeId,
                                                  payDateFrom,
                                                  payDateTo,
                                                  allowanceRepository,
                                                  context)

            If ecolaAllowance Is Nothing Then

                Dim ecolaProductId = (Await productRepository.GetOrCreateAllowanceType(ProductConstant.ECOLA))?.RowID

                Dim effectiveEndDate As Date?

                If effectiveEndDateShouldBeNull Then

                    effectiveEndDate = Nothing
                Else
                    effectiveEndDate = Nothing

                End If

                ecolaAllowance = New Allowance
                ecolaAllowance.EmployeeID = employeeId
                ecolaAllowance.ProductID = ecolaProductId
                ecolaAllowance.AllowanceFrequency = allowanceFrequency
                ecolaAllowance.EffectiveStartDate = payDateFrom
                ecolaAllowance.EffectiveEndDate = effectiveEndDate
                ecolaAllowance.Amount = amount

                Await allowanceRepository.SaveAsync(ecolaAllowance, context)
                Await context.SaveChangesAsync()

                ecolaAllowance = Await GetEmployeeEcola(employeeId,
                                                  payDateFrom,
                                                  payDateTo,
                                                  allowanceRepository,
                                                  context)
            End If

            Return ecolaAllowance
        End Using

    End Function

    Friend Shared Async Function GetFirstPayPeriodOfTheYear(context As PayrollContext, currentPayPeriod As PayPeriod) As Task(Of PayPeriod)

        Dim currentPayPeriodYear = currentPayPeriod?.Year

        If currentPayPeriodYear Is Nothing Then Return Nothing

        If context Is Nothing Then
            context = New PayrollContext
        End If

        Return Await context.PayPeriods.
                        Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                        Where(Function(p) p.IsMonthly).
                        Where(Function(p) p.Year = currentPayPeriodYear.Value).
                        Where(Function(p) p.IsFirstPayPeriodOfTheYear).
                        FirstOrDefaultAsync
    End Function

    Friend Shared Async Function GetFirstDayOfTheYear(context As PayrollContext, currentPayPeriod As PayPeriod) As Task(Of Date?)

        Dim firstPayPeriodOfTheYear = Await GetFirstPayPeriodOfTheYear(context, currentPayPeriod)

        Return firstPayPeriodOfTheYear?.PayFromDate

    End Function

    Private Shared Async Function GetEmployeeEcola(employeeId As Integer, payDateFrom As Date, payDateTo As Date, allowanceRepository As AllowanceRepository, context As PayrollContext) As Task(Of Allowance)
        Return Await allowanceRepository.
                                    GetAllowancesWithPayPeriodBaseQuery(context, payDateFrom, payDateTo).
                                    Where(Function(a) a.EmployeeID.Value = employeeId).
                                    Where(Function(a) a.Product.PartNo.ToLower() = ProductConstant.ECOLA).
                                    FirstOrDefaultAsync
    End Function

    Public Shared Sub UpdateLoanSchedule(paypRowID As Integer)

        Dim param_array = New Object() {orgztnID, paypRowID, z_User}

        Static strquery_recompute_13monthpay As String =
                        "call recompute_thirteenthmonthpay(?organizid, ?payprowid, ?userrowid);"

        Dim n_ExecSQLProcedure = New SQL(strquery_recompute_13monthpay, param_array)
        n_ExecSQLProcedure.ExecuteQuery()
    End Sub

    Public Shared Function CheckIfUsingUserLevel() As Boolean

        Using context As New PayrollContext

            Dim settings = New ListOfValueCollection(context.ListOfValues.ToList())

            If settings.GetBoolean("User Policy.UseUserLevel", False) Then
                Return True
            End If

        End Using

        Return False

    End Function

End Class