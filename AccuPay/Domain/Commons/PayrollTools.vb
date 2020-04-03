Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data
Imports AccuPay.Entity
Imports AccuPay.Repository
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Utils
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Public Class PayrollTools

    Public Const MonthsPerYear As Integer = 12

    Public Const WorkHoursPerDay As Integer = 8

    Public Const DivisorToDailyRate As Integer = 8

    Public Const SemiMonthlyPayPeriodsPerMonth As Integer = 2

    Public Const PayFrequencySemiMonthlyId As Integer = 1

    Public Const PayFrequencyWeeklyId As Integer = 4
    Private Const fourDays As Integer = 4

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
                            currentTimeEntries As ICollection(Of TimeEntry),
                            calendarCollection As CalendarCollection) As Boolean

        Dim threeDaysPrior = fourDays * -1
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

            Dim totalDayPay = lastTimeEntry.GetTotalDayPay()

            If lastTimeEntry.IsRestDay Then

                If totalDayPay > 0 Then
                    Return True
                End If

                Continue For
            End If

            Dim payrateCalendar = calendarCollection.GetCalendar(lastTimeEntry.BranchID)
            Dim payrate = payrateCalendar.Find(lastTimeEntry.Date)
            If payrate.IsHoliday Then
                If totalDayPay > 0 Then
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
                            calendarCollection As CalendarCollection) As Boolean

        Dim thirdDateAfterCurrDate = legalHolidayDate.Date.AddDays(fourDays)

        Dim postTimeEntries = currentTimeEntries.
            Where(Function(t) legalHolidayDate.Date < t.Date And t.Date <= thirdDateAfterCurrDate).
            OrderBy(Function(t) t.Date).
            ToList()

        For Each timeEntry In postTimeEntries
            If timeEntry.HasShift = False Then
                Continue For
            End If

            Dim totalDayPay = timeEntry.GetTotalDayPay()

            If timeEntry.IsRestDay Then

                If totalDayPay > 0 Then
                    Return True
                End If

                Continue For
            End If

            Dim payrateCalendar = calendarCollection.GetCalendar(timeEntry.BranchID)
            Dim payrate = payrateCalendar.Find(timeEntry.Date)
            If payrate.IsHoliday Then
                If totalDayPay > 0 Then
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
                                    Optional payperiods As IEnumerable(Of IPayPeriod) = Nothing) As Task(Of IPayPeriod)

        Return Await Task.Run(Function()

                                  'replace this with a policy
                                  'fourlinq can use this feature also
                                  'for clients that has the same attendance and payroll period
                                  Dim isBenchmarkOwner = ((New SystemOwner).CurrentSystemOwner = SystemOwner.Benchmark)

                                  Dim currentDay = Date.Today.ToMinimumHourValue

                                  Using context = New PayrollContext()

                                      If payperiods Is Nothing OrElse payperiods.Count = 0 Then
                                          payperiods = context.PayPeriods.
                                     Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                                     Where(Function(p) p.IsSemiMonthly)
                                      End If

                                      If isBenchmarkOwner Then

                                          Return payperiods.
                                     Where(Function(p) currentDay >= p.PayFromDate AndAlso currentDay <= p.PayToDate).
                                     LastOrDefault
                                      Else

                                          Return payperiods.
                                     Where(Function(p) p.PayToDate < currentDay).
                                     LastOrDefault
                                      End If
                                  End Using

                              End Function)

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
                        Where(Function(p) p.IsSemiMonthly).
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

    Public Shared Sub DeletePaystub(employeeId As Integer, payPeriodId As Integer)

        Dim n_ExecuteQuery As New ExecuteQuery("SELECT RowID" &
                                                       " FROM paystub" &
                                                       " WHERE EmployeeID='" & employeeId & "'" &
                                                       " AND OrganizationID='" & orgztnID & "'" &
                                                       " AND PayPeriodID='" & payPeriodId & "'" &
                                                       " LIMIT 1;")

        Dim paystubRowID As Object = Nothing

        paystubRowID = n_ExecuteQuery.Result

        If paystubRowID IsNot Nothing Then
            n_ExecuteQuery = New ExecuteQuery("CALL DEL_specificpaystub('" & paystubRowID.ToString & "');")
        End If

    End Sub

    Public Shared Async Function ValidatePayPeriodAction(payPeriodId As Integer?) As Task(Of Boolean)

        Dim sys_ownr As New SystemOwner

        If sys_ownr.CurrentSystemOwner = SystemOwner.Benchmark Then

            'Add temporarily. Consult maam mely first as she is still testing the system with multiple pay periods
            Return True

        End If

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

    Public Shared Function GetOrganizationAddress() As String

        Dim str_quer_address As String =
            String.Concat("SELECT CONCAT_WS(', '",
                          ", IF(LENGTH(TRIM(ad.StreetAddress1)) = 0, NULL, ad.StreetAddress1)",
                          ", IF(LENGTH(TRIM(ad.StreetAddress2)) = 0, NULL, ad.StreetAddress2)",
                          ", IF(LENGTH(TRIM(ad.Barangay)) = 0, NULL, ad.Barangay)",
                          ", IF(LENGTH(TRIM(ad.CityTown)) = 0, NULL, ad.CityTown)",
                          ", IF(LENGTH(TRIM(ad.Country)) = 0, NULL, ad.Country)",
                          ", IF(LENGTH(TRIM(ad.State)) = 0, NULL, ad.State)",
                          ") `Result`",
                          " FROM organization og",
                          " LEFT JOIN address ad ON ad.RowID = og.PrimaryAddressID",
                          " WHERE og.RowID = ", orgztnID, ";")

        Return Convert.ToString(New SQL(str_quer_address).GetFoundRow)

    End Function

    Public Shared Function GetCalendarCollection(threeDaysBeforeCutoff As Date,
                                                    payDateTo As Date,
                                                    context As PayrollContext,
                                                    calculationBasis As PayRateCalculationBasis) _
                                                    As CalendarCollection
        Dim payrates = context.PayRates.
                                Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                                Where(Function(p) threeDaysBeforeCutoff <= p.Date AndAlso
                                                    p.Date <= payDateTo).
                                ToList()
        If calculationBasis = PayRateCalculationBasis.Branch Then
            Dim branches = context.Branches.ToList()

            Dim calendarDays = context.CalendarDays.
                         Include(Function(t) t.DayType).
                         Where(Function(t) threeDaysBeforeCutoff <= t.Date AndAlso t.Date <= payDateTo).
                         ToList()

            Return New CalendarCollection(payrates, branches, calendarDays)
        Else
            Return New CalendarCollection(payrates)
        End If
    End Function

End Class