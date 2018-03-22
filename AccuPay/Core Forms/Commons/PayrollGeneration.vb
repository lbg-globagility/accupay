Imports System.Collections.ObjectModel
Imports System.Data.Entity
Imports AccuPay.Entity
Imports log4net
Imports MySql.Data.MySqlClient
Imports PayrollSys

Public Class PayrollGeneration

    Private Shared logger As ILog = LogManager.GetLogger("PayrollLogger")

    Private Class PayFrequency
        Public Const SemiMonthly As Integer = 1
        Public Const Monthly As Integer = 2
    End Class

    Private Class SalaryType
        Public Const Fixed As String = "Fixed"
        Public Const Monthly As String = "Monthly"
        Public Const Daily As String = "Daily"
    End Class

    Private Class ContributionSchedule
        Public Const FirstHalf As String = "First half"
        Public Const EndOfTheMonth As String = "End of the month"
        Public Const PerPayPeriod As String = "Per pay period"
    End Class

    Public Property PayrollDateFrom As String

    Public Property PayrollDateTo As String

    Public Property PayPeriodID As String

    Private _employee As DataRow

    Private _employee2 As Employee

    Private isEndOfMonth2 As String
    Private _isFirstHalf As Boolean
    Private _isEndOfMonth As Boolean
    Private _allSalaries As DataTable

    Private _allLoanSchedules As ICollection(Of PayrollSys.LoanSchedule)

    Private _allLoanTransactions As ICollection(Of PayrollSys.LoanTransaction)

    Private allWeeklyAllowances As DataTable
    Private allNoTaxWeeklyAllowances As DataTable

    Private allOneTimeBonuses As DataTable
    Private allDailyBonuses As DataTable
    Private allWeeklyBonuses As DataTable
    Private allSemiMonthlyBonuses As DataTable
    Private allMonthlyBonuses As DataTable

    Private allNoTaxOneTimeBonuses As DataTable
    Private allNoTaxDailyBonuses As DataTable
    Private allNoTaxWeeklyBonuses As DataTable
    Private allNoTaxSemiMonthlyBonuses As DataTable
    Private allNoTaxMonthlyBonuses As DataTable

    Private allFixedTaxableMonthlyAllowances As DataTable
    Private allFixedNonTaxableMonthlyAllowances As DataTable

    Private _numOfDayPresent As DataTable
    Private _allTimeEntries As DataTable
    Private _employeeFirstTimeSalary As DataTable
    Private _VeryFirstPayPeriodIDOfThisYear As Object
    Private _withThirteenthMonthPay As SByte

    Private _previousTimeEntries2 As ICollection(Of TimeEntry)

    Private _withholdingTaxTable As DataTable

    Private _filingStatuses As DataTable

    Private Delegate Sub NotifyMainWindow(success As Boolean)

    Private _notifyMainWindow As NotifyMainWindow

    Private form_caller As Form

    Private payPeriod As DataRow

    Private _payPeriod As PayPeriod

    Private annualUnusedLeaves As DataTable
    Private unusedLeaveProductID As String
    Private existingUnusedLeaveAdjustments As DataTable

    Private agencyFeeSummary As DataTable

    Private _paystub As Paystub

    Private _products As IEnumerable(Of Product)

    Private _philHealthDeductionSchedule As String
    Private _sssDeductionSchedule As String
    Private _hdmfDeductionSchedule As String
    Private _withholdingTaxSchedule As String

    Private _connection As MySqlConnection

    Private numberofweeksthismonth As Integer

    Private _vacationLeaveBalance As Decimal

    Private _sickLeaveBalance As Decimal

    Private _otherLeaveBalance As Decimal

    Private _sickLeaveUsed As Decimal

    Private _vacationLeaveUsed As Decimal

    Private _otherLeaveUsed As Decimal

    Private _paystubs As IEnumerable(Of Paystub)

    Private _previousPaystubs As IEnumerable(Of Paystub)

    Private _socialSecurityBrackets As IEnumerable(Of SocialSecurityBracket)

    Private _philHealthBrackets As IEnumerable(Of PhilHealthBracket)

    Private _withholdingTaxBrackets As IEnumerable(Of WithholdingTaxBracket)

    Private _listOfValues As IList(Of ListOfValue)

    Private _settings As ListOfValueCollection

    Private _timeEntries2 As ICollection(Of TimeEntry)

    Private _payRates As IReadOnlyDictionary(Of Date, PayRate)

    Private _allowances As ICollection(Of Allowance)

    Private _allowanceItems As ICollection(Of AllowanceItem) = New List(Of AllowanceItem)

    Private _actualtimeentries As ICollection(Of ActualTimeEntry)

    Sub New(employee As DataRow,
            payPeriodHalfNo As String,
            allSalaries As DataTable,
            allLoanSchedules As ICollection(Of PayrollSys.LoanSchedule),
            allLoanTransactions As ICollection(Of PayrollSys.LoanTransaction),
            allWeeklyAllowances As DataTable,
            allNoTaxWeeklyAllowances As DataTable,
            allDailyBonuses As DataTable,
            allMonthlyBonuses As DataTable,
            allOneTimeBonuses As DataTable,
            allSemiMonthlyBonuses As DataTable,
            allWeeklyBonuses As DataTable,
            allNoTaxDailyBonuses As DataTable,
            allNoTaxMonthlyBonuses As DataTable,
            allNoTaxOneTimeBonuses As DataTable,
            allNoTaxSemiMonthlyBonuses As DataTable,
            allNoTaxWeeklyBonuses As DataTable,
            numOfDayPresent As DataTable,
            allTimeEntries As DataTable,
            dtemployeefirsttimesalary As DataTable,
            VeryFirstPayPeriodIDOfThisYear As Object,
            withThirteenthMonthPay As SByte,
            filingStatuses As DataTable,
            withholdingTaxTable As DataTable,
            products As IEnumerable(Of Product),
            paystubs As IEnumerable(Of Paystub),
            resources As PayrollResources,
            Optional pay_stub_frm As PayStubForm = Nothing)
        form_caller = pay_stub_frm

        _employee = employee
        _employee2 = resources.Employees.
            FirstOrDefault(Function(e) e.RowID.ToString() = _employee("RowID"))

        isEndOfMonth2 = payPeriodHalfNo
        Me._allSalaries = allSalaries
        _allLoanSchedules = allLoanSchedules
        _allLoanTransactions = allLoanTransactions

        Me.allWeeklyAllowances = allWeeklyAllowances
        Me.allNoTaxWeeklyAllowances = allNoTaxWeeklyAllowances

        Me.allOneTimeBonuses = allOneTimeBonuses
        Me.allDailyBonuses = allDailyBonuses
        Me.allWeeklyBonuses = allWeeklyBonuses
        Me.allSemiMonthlyBonuses = allSemiMonthlyBonuses
        Me.allMonthlyBonuses = allMonthlyBonuses

        Me.allNoTaxOneTimeBonuses = allNoTaxOneTimeBonuses
        Me.allNoTaxDailyBonuses = allNoTaxDailyBonuses
        Me.allNoTaxWeeklyBonuses = allNoTaxWeeklyBonuses
        Me.allNoTaxSemiMonthlyBonuses = allNoTaxSemiMonthlyBonuses
        Me.allNoTaxMonthlyBonuses = allNoTaxMonthlyBonuses

        _numOfDayPresent = numOfDayPresent
        _allTimeEntries = allTimeEntries
        _employeeFirstTimeSalary = dtemployeefirsttimesalary
        _VeryFirstPayPeriodIDOfThisYear = VeryFirstPayPeriodIDOfThisYear
        _withThirteenthMonthPay = withThirteenthMonthPay

        _filingStatuses = filingStatuses
        _withholdingTaxTable = withholdingTaxTable

        _notifyMainWindow = AddressOf pay_stub_frm.ProgressCounter

        _products = products
        _paystubs = resources.Paystubs

        _isFirstHalf = (payPeriodHalfNo = "1")
        _isEndOfMonth = (payPeriodHalfNo = "0")
        _socialSecurityBrackets = resources.SocialSecurityBrackets
        _philHealthBrackets = resources.PhilHealthBrackets
        _withholdingTaxBrackets = resources.WithholdingTaxBrackets

        _listOfValues = resources.ListOfValues
        _previousPaystubs = resources.PreviousPaystubs
        _settings = New ListOfValueCollection(_listOfValues)
        _payPeriod = resources.PayPeriod

        _previousTimeEntries2 = resources.TimeEntries2.
            Where(Function(t) t.EmployeeID = _employee2.RowID).
            ToList()

        _timeEntries2 = resources.TimeEntries2.
            Where(Function(t) t.EmployeeID = _employee2.RowID).
            Where(Function(t) _payPeriod.PayFromDate <= t.Date And t.Date <= _payPeriod.PayToDate).
            ToList()

        _actualtimeentries = resources.ActualTimeEntries.
            Where(Function(t) t.EmployeeID = _employee2.RowID).
            ToList()

        _payRates = resources.PayRates.ToDictionary(Function(p) p.RateDate)

        _allowances = resources.Allowances.
            Where(Function(a) a.EmployeeID = _employee2.RowID).
            ToList()
    End Sub

    Sub PayrollGeneration_BackgroundWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        DoProcess()
    End Sub

    Public Sub DoProcess()
        Try
            Console.WriteLine($"Generate Paystub for #{_employee2.RowID} - {_employee2.LastName}, {_employee2.FirstName}")
            Dim date_to_use = If(CDate(PayrollDateFrom) > CDate(PayrollDateTo), CDate(PayrollDateFrom), CDate(PayrollDateTo))
            Dim dateStr_to_use = Format(date_to_use, "yyyy-MM-dd")
            numberofweeksthismonth = CInt(New MySQLExecuteQuery("SELECT `COUNTTHEWEEKS`('" & dateStr_to_use & "');").Result)

            GeneratePayStub()
            form_caller.BeginInvoke(_notifyMainWindow, True)
        Catch ex As Exception
            Console.WriteLine(getErrExcptn(ex, "PayrollGeneration - Error"))
            logger.Error("DoProcess", ex)
            form_caller.BeginInvoke(_notifyMainWindow, False)
        End Try
    End Sub

    Private Sub GeneratePayStub()
        Dim transaction As MySqlTransaction = Nothing
        Dim newLoanTransactions = New Collection(Of PayrollSys.LoanTransaction)

        Try
            _paystub = _paystubs.FirstOrDefault(
                Function(p) Nullable.Equals(p.EmployeeID, _employee2.RowID))

            If _paystub Is Nothing Then
                _paystub = New Paystub() With {
                    .OrganizationID = z_OrganizationID,
                    .CreatedBy = z_User,
                    .LastUpdBy = z_User,
                    .EmployeeID = _employee2.RowID,
                    .PayPeriodID = PayPeriodID,
                    .PayFromdate = PayrollDateFrom,
                    .PayToDate = PayrollDateTo
                }
            End If

            Dim totalVacationDaysLeft = 0D

            _sssDeductionSchedule = _employee("SSSDeductSched").ToString
            _philHealthDeductionSchedule = _employee("PhHealthDeductSched").ToString
            _hdmfDeductionSchedule = _employee("HDMFDeductSched").ToString
            _withholdingTaxSchedule = _employee("WTaxDeductSched").ToString

            _paystub.EmployeeID = _employee2.RowID

            Dim salary = _allSalaries.Select($"EmployeeID = '{_paystub.EmployeeID}'").FirstOrDefault()

            Dim loanTransactions = _allLoanTransactions.Where(Function(t) t.EmployeeID = _paystub.EmployeeID)

            If loanTransactions.Count > 0 Then
                _paystub.TotalLoans = loanTransactions.Aggregate(0D, Function(total, t) total + t.Amount)
            Else
                Dim acceptedLoans As String() = {}
                If _isFirstHalf Then
                    acceptedLoans = {"Per pay period", "First half"}
                ElseIf _isEndOfMonth Then
                    acceptedLoans = {"Per pay period", "End of the month"}
                End If

                Dim loanSchedules = _allLoanSchedules.
                    Where(Function(l) l.EmployeeID = _paystub.EmployeeID).
                    Where(Function(l) acceptedLoans.Contains(l.DeductionSchedule)).
                    ToList()

                For Each loanSchedule In loanSchedules
                    Dim loanTransaction = New PayrollSys.LoanTransaction() With {
                        .Created = Date.Now(),
                        .LastUpd = Date.Now(),
                        .OrganizationID = z_OrganizationID,
                        .EmployeeID = _paystub.EmployeeID,
                        .PayPeriodID = PayPeriodID,
                        .LoanScheduleID = loanSchedule.RowID,
                        .LoanPayPeriodLeft = loanSchedule.LoanPayPeriodLeft - 1
                    }

                    If loanSchedule.DeductionAmount > loanSchedule.TotalBalanceLeft Then
                        loanTransaction.Amount = loanSchedule.TotalBalanceLeft
                    Else
                        loanTransaction.Amount = loanSchedule.DeductionAmount
                    End If

                    loanTransaction.TotalBalance = loanSchedule.TotalBalanceLeft - loanTransaction.Amount

                    newLoanTransactions.Add(loanTransaction)
                Next

                _paystub.TotalLoans = newLoanTransactions.Aggregate(0D, Function(total, x) x.Amount + total)
            End If

            totalVacationDaysLeft = 0D

            CalculateAllowances()
            CalculateBonuses()

            _paystub.TotalAdjustments = If(_paystub?.Adjustments.Sum(Function(a) a.Amount), 0)

            Dim governmentContributions = 0D
            Dim timeEntrySummary = _allTimeEntries.Select($"EmployeeID = '{_paystub.EmployeeID}'").FirstOrDefault()

            If timeEntrySummary IsNot Nothing Then

                totalVacationDaysLeft = 0D

                If salary IsNot Nothing Then

                    Dim isFirstPay =
                        _payPeriod.PayFromDate <= _employee2.StartDate And
                        _employee2.StartDate <= _payPeriod.PayToDate

                    Dim basicPay = ValNoComma(salary("BasicPay"))

                    ComputeHours()

                    _vacationLeaveBalance = _employee2.LeaveBalance
                    _sickLeaveBalance = _employee2.SickLeaveBalance
                    _otherLeaveBalance = _employee2.OtherLeaveBalance

                    _vacationLeaveUsed = CDec(timeEntrySummary("VacationLeaveHours"))
                    _sickLeaveUsed = CDec(timeEntrySummary("SickLeaveHours"))
                    _otherLeaveUsed = CDec(timeEntrySummary("OtherLeaveHours"))

                    Dim sel_dtemployeefirsttimesalary = _employeeFirstTimeSalary.Select($"EmployeeID = '{_paystub.EmployeeID}'")

                    Dim previousPaystub = _previousPaystubs?.
                        Where(Function(p) Nullable.Equals(p.EmployeeID, _paystub.EmployeeID)).
                        FirstOrDefault()

                    Dim currentTaxableIncome = 0D
                    If _employee2.EmployeeType = SalaryType.Fixed Then

                        _paystub.TotalEarnings = basicPay + (_paystub.HolidayPay + _paystub.OvertimePay + _paystub.NightDiffPay + _paystub.NightDiffOvertimePay)

                        currentTaxableIncome = basicPay

                    ElseIf _employee2.EmployeeType = SalaryType.Monthly Then

                        Dim isFirstPayAsDailyRule = _settings.GetBoolean("Payroll Policy", "isfirstsalarydaily")

                        If isFirstPay And isFirstPayAsDailyRule Then
                            _paystub.TotalEarnings = ValNoComma(timeEntrySummary("TotalDayPay"))
                        Else
                            Dim totalDeduction = _paystub.LateDeduction + _paystub.UndertimeDeduction + _paystub.AbsenceDeduction
                            Dim extraPay =
                                _paystub.OvertimePay +
                                _paystub.NightDiffPay +
                                _paystub.NightDiffOvertimePay +
                                _paystub.RestDayPay +
                                _paystub.RestDayOTPay +
                                _paystub.SpecialHolidayPay +
                                _paystub.SpecialHolidayOTPay +
                                _paystub.RegularHolidayPay +
                                _paystub.RegularHolidayOTPay

                            _paystub.TotalEarnings = (basicPay + extraPay) - totalDeduction

                            Dim taxablePolicy = If(_settings.GetString("Payroll Policy", "paystub.taxableincome"), "Basic Pay")

                            If taxablePolicy = "Gross Income" Then
                                currentTaxableIncome = _paystub.TotalEarnings
                            Else
                                currentTaxableIncome = basicPay
                            End If
                        End If

                    ElseIf _employee2.EmployeeType = SalaryType.Daily Then
                        _paystub.TotalEarnings = ValNoComma(timeEntrySummary("TotalDayPay"))
                    End If

                    CalculateSss(salary)
                    CalculatePhilHealth(salary)
                    CalculateHdmf(salary)

                    governmentContributions = _paystub.SssEmployeeShare + _paystub.PhilHealthEmployeeShare + _paystub.HdmfEmployeeShare
                    currentTaxableIncome = currentTaxableIncome - governmentContributions

                    _paystub.TaxableIncome = currentTaxableIncome

                    If IsWithholdingTaxPaidOnFirstHalf() Or IsWithholdingTaxPaidOnEndOfTheMonth() Then
                        _paystub.TaxableIncome = currentTaxableIncome + If(previousPaystub?.TaxableIncome, 0D)
                    ElseIf IsWithholdingTaxPaidPerPayPeriod() Then
                        _paystub.TaxableIncome = currentTaxableIncome
                    End If

                    CalculateWithholdingTax()
                End If
            End If

            _paystub.GrossPay = _paystub.TotalEarnings + _paystub.TotalBonus + _paystub.TotalAllowance
            _paystub.NetPay = _paystub.GrossPay - (governmentContributions + _paystub.TotalLoans + _paystub.WithholdingTax)

            Dim vacationLeaveProduct = _products.Where(Function(p) p.PartNo = "Vacation leave").FirstOrDefault()
            Dim sickLeaveProduct = _products.Where(Function(p) p.PartNo = "Sick leave").FirstOrDefault()

            Using context = New PayrollContext()
                If _paystub.RowID.HasValue Then
                    context.Entry(_paystub).State = EntityState.Modified
                Else
                    context.Paystubs.Add(_paystub)
                End If

                ' Delete and replace the old allowanceItems with the newly recalculated ones
                context.Set(Of AllowanceItem).RemoveRange(_paystub.AllowanceItems)
                _paystub.AllowanceItems = _allowanceItems

                UpdateLeaveLedger(context)

                Dim vacationLeaveBalance =
                    (From p In context.PaystubItems
                     Where p.Product.PartNo = "Vacation leave" And
                        p.PayStubID = _paystub.RowID).
                    FirstOrDefault()

                If vacationLeaveBalance Is Nothing Then
                    Dim newBalance = _vacationLeaveBalance - _vacationLeaveUsed

                    vacationLeaveBalance = New PaystubItem() With {
                        .OrganizationID = z_OrganizationID,
                        .Created = Date.Now,
                        .ProductID = vacationLeaveProduct?.RowID,
                        .PayAmount = newBalance,
                        .PayStubID = _paystub.RowID
                    }

                    _paystub.PaystubItems.Add(vacationLeaveBalance)
                End If

                Dim sickLeaveBalance =
                    (From p In context.PaystubItems
                     Where p.Product.PartNo = "Sick leave" And
                        p.PayStubID = _paystub.RowID).
                    FirstOrDefault()

                If sickLeaveBalance Is Nothing Then
                    Dim newBalance = _sickLeaveBalance - _sickLeaveUsed

                    sickLeaveBalance = New PaystubItem() With {
                        .OrganizationID = z_OrganizationID,
                        .Created = Date.Now,
                        .ProductID = sickLeaveProduct?.RowID,
                        .PayAmount = newBalance,
                        .PayStubID = _paystub.RowID
                    }

                    _paystub.PaystubItems.Add(sickLeaveBalance)
                End If

                context.LoanTransactions.AddRange(newLoanTransactions)

                ComputeThirteenthMonthPay(salary)

                context.SaveChanges()
            End Using


        Catch ex As Exception
            If transaction IsNot Nothing Then
                transaction.Rollback()
            End If

            Throw New Exception($"Failure to generate paystub for employee {_paystub.EmployeeID}", ex)
        Finally
            If _connection IsNot Nothing Then
                If _connection.State = ConnectionState.Open Then
                    _connection.Close()
                    _connection.Dispose()
                End If
            End If
        End Try
    End Sub

    Private Sub ComputeThirteenthMonthPay(salaryrow As DataRow)

        If _paystub.ThirteenthMonthPay Is Nothing Then
            _paystub.ThirteenthMonthPay = New ThirteenthMonthPay() With {.OrganizationID = z_OrganizationID,
                .CreatedBy = z_User}
        Else
            _paystub.ThirteenthMonthPay = New ThirteenthMonthPay() With {.LastUpdBy = z_User}
        End If

        Dim contractual_employment_statuses =
            New String() {"Contractual", "SERVICE CONTRACT"}

        Dim month_count = 12

        Dim basicpay_13month As Decimal = 0

        If _employee2.IsDaily Then

            basicpay_13month =
                If(contractual_employment_statuses.Contains(_employee2.EmploymentStatus),
                _timeEntries2.Sum(Function(t) t.BasicDayPay + t.LeavePay),
                _actualtimeentries.Sum(Function(t) t.BasicDayPay + t.LeavePay))

        ElseIf _employee2.IsMonthly Then
            Dim basic_salary = Convert.ToDecimal(salaryrow("BasicPay"))

            basicpay_13month =
                (basic_salary -
                _actualtimeentries.Sum(Function(t) t.LateDeduction + t.UndertimeDeduction + t.AbsentDeduction))

        End If

        _paystub.ThirteenthMonthPay.BasicPay = basicpay_13month

        _paystub.ThirteenthMonthPay.Amount = (basicpay_13month / month_count)

    End Sub

    Private Sub ComputeHours()
        With _paystub
            .RegularHours = _timeEntries2.Sum(Function(t) t.RegularHours)
            .RegularPay = _timeEntries2.Sum(Function(t) t.RegularPay)

            .OvertimeHours = _timeEntries2.Sum(Function(t) t.OvertimeHours)
            .OvertimePay = _timeEntries2.Sum(Function(t) t.OvertimePay)

            .NightDiffHours = _timeEntries2.Sum(Function(t) t.NightDiffHours)
            .NightDiffPay = _timeEntries2.Sum(Function(t) t.NightDiffPay)

            .NightDiffOvertimeHours = _timeEntries2.Sum(Function(t) t.NightDiffOTHours)
            .NightDiffOvertimePay = _timeEntries2.Sum(Function(t) t.NightDiffOTPay)

            .RestDayHours = _timeEntries2.Sum(Function(t) t.RestDayHours)
            .RestDayPay = _timeEntries2.Sum(Function(t) t.RestDayPay)

            .RestDayOTHours = _timeEntries2.Sum(Function(t) t.RestDayOTHours)
            .RestDayOTPay = _timeEntries2.Sum(Function(t) t.RestDayOTPay)

            .SpecialHolidayHours = _timeEntries2.Sum(Function(t) t.SpecialHolidayHours)
            .SpecialHolidayPay = _timeEntries2.Sum(Function(t) t.SpecialHolidayPay)

            .SpecialHolidayOTHours = _timeEntries2.Sum(Function(t) t.SpecialHolidayOTHours)
            .SpecialHolidayOTPay = _timeEntries2.Sum(Function(t) t.SpecialHolidayOTPay)

            .RegularHolidayHours = _timeEntries2.Sum(Function(t) t.RegularHolidayHours)
            .RegularHolidayPay = _timeEntries2.Sum(Function(t) t.RegularHolidayPay)

            .RegularHolidayOTHours = _timeEntries2.Sum(Function(t) t.RegularHolidayOTHours)
            .RegularHolidayOTPay = _timeEntries2.Sum(Function(t) t.RegularHolidayOTPay)

            .HolidayPay = _timeEntries2.Sum(Function(t) t.HolidayPay)

            .LeaveHours = _timeEntries2.Sum(Function(t) t.TotalLeaveHours)
            .LeavePay = _timeEntries2.Sum(Function(t) t.LeavePay)

            .LateHours = _timeEntries2.Sum(Function(t) t.LateHours)
            .LateDeduction = _timeEntries2.Sum(Function(t) t.LateDeduction)

            .UndertimeHours = _timeEntries2.Sum(Function(t) t.UndertimeHours)
            .UndertimeDeduction = _timeEntries2.Sum(Function(t) t.UndertimeDeduction)

            .AbsentHours = _timeEntries2.Sum(Function(t) t.AbsentHours)
            .AbsenceDeduction = _timeEntries2.Sum(Function(t) t.AbsentDeduction)
        End With
    End Sub

    Private Function CalculateSemiMonthlyProratedAllowance(allowance As Allowance) As AllowanceItem
        Dim workDaysPerYear = _employee2.WorkDaysPerYear
        Dim workingDays = CDec(workDaysPerYear / CalendarConstants.MonthsInAYear / CalendarConstants.SemiMonthlyPayPeriodsPerMonth)
        Dim dailyRate = allowance.Amount / workingDays

        Dim allowancesPerDay = New Collection(Of AllowancePerDay)
        For Each timeEntry In _timeEntries2
            Dim divisor = If(timeEntry.ShiftSchedule?.Shift?.DivisorToDailyRate, 8D)
            Dim hourlyRate = dailyRate / divisor

            Dim deductionHours =
                timeEntry.LateHours +
                timeEntry.UndertimeHours +
                timeEntry.AbsentHours
            Dim deductionAmount = -(hourlyRate * deductionHours)

            Dim additionalAmount = 0D
            Dim giveAllowanceDuringHolidays = _settings.GetBoolean("Payroll Policy", "allowances.holiday")
            If giveAllowanceDuringHolidays Then
                Dim payRate = _payRates(timeEntry.Date)

                If (payRate.IsSpecialNonWorkingHoliday And _employee2.CalcSpecialHoliday) Or
                   (payRate.IsRegularHoliday And _employee2.CalcHoliday) Then
                    additionalAmount = timeEntry.RegularHours * hourlyRate * (payRate.CommonRate - 1D)
                End If
            End If

            allowancesPerDay.Add(New AllowancePerDay() With {
                .Date = timeEntry.Date,
                .Amount = deductionAmount + additionalAmount
            })
        Next

        Dim sumTotalOfDays = allowancesPerDay.Sum(Function(a) a.Amount)
        Dim baseAllowance = allowance.Amount

        Dim allowanceItem = New AllowanceItem() With {
            .OrganizationID = z_OrganizationID,
            .CreatedBy = z_User,
            .LastUpdBy = z_User,
            .PayPeriodID = PayPeriodID,
            .AllowanceID = allowance.RowID,
            .Amount = baseAllowance + sumTotalOfDays,
            .AllowancesPerDay = allowancesPerDay
        }

        Return allowanceItem
    End Function

    Private Function CalculateDailyProratedAllowance(allowance As Allowance) As AllowanceItem
        Dim dailyRate = allowance.Amount

        Dim allowancesPerDay = New Collection(Of AllowancePerDay)
        For Each timeEntry In _timeEntries2
            Dim divisor = If(timeEntry.ShiftSchedule?.Shift?.DivisorToDailyRate, 8D)
            Dim hourlyRate = dailyRate / divisor

            Dim amount = 0D
            Dim payRate = _payRates(timeEntry.Date)
            If payRate.IsRegularDay Then
                Dim isRestDay = If(timeEntry.ShiftSchedule?.IsRestDay, False)

                If isRestDay Then
                    amount = If(timeEntry.RestDayHours > 0, dailyRate, 0)
                Else
                    amount = (timeEntry.RegularHours + timeEntry.TotalLeaveHours) * hourlyRate
                End If
            ElseIf payRate.IsSpecialNonWorkingHoliday Then
                Dim countableHours = timeEntry.RegularHours + timeEntry.SpecialHolidayHours + timeEntry.TotalLeaveHours

                amount = If(countableHours > 0, dailyRate, 0D)
            ElseIf payRate.IsRegularHoliday Then
                amount = (timeEntry.RegularHours + timeEntry.RegularHolidayHours) * hourlyRate

                If HasWorkedLastWorkingDay(timeEntry) Then
                    amount += dailyRate
                End If
            End If

            allowancesPerDay.Add(New AllowancePerDay() With {
                .Date = timeEntry.Date,
                .Amount = amount
            })
        Next

        Dim totalAmount = allowancesPerDay.Sum(Function(a) a.Amount)

        Dim allowanceItem = New AllowanceItem() With {
            .OrganizationID = z_OrganizationID,
            .CreatedBy = z_User,
            .LastUpdBy = z_User,
            .PayPeriodID = PayPeriodID,
            .AllowanceID = allowance.RowID,
            .Amount = totalAmount,
            .AllowancesPerDay = allowancesPerDay
        }

        Return allowanceItem
    End Function

    Private Function CalculateOneTimeAllowances(allowance As Allowance)
        Return allowance.Amount
    End Function

    Private Function HasWorkedLastWorkingDay(current As TimeEntry) As Boolean
        Dim lastPotentialEntry = current.Date.AddDays(-3)

        Dim lastTimeEntries = _previousTimeEntries2.
            Where(Function(t) lastPotentialEntry <= t.Date And t.Date <= current.Date).
            Reverse().
            ToList()

        For Each lastTimeEntry In lastTimeEntries
            ' If employee has no shift set for the day, it's not a working day.
            If lastTimeEntry?.ShiftSchedule?.Shift Is Nothing Then
                Continue For
            End If

            If lastTimeEntry?.ShiftSchedule.IsRestDay And (lastTimeEntry.TotalDayPay > 0D) Then
                Return True
            End If

            Dim payRate = _payRates(lastTimeEntry.Date)
            If payRate.IsHoliday And (lastTimeEntry.TotalDayPay > 0D) Then
                Return True
            End If

            Return lastTimeEntry.RegularHours > 0 Or lastTimeEntry.TotalLeaveHours > 0
        Next

        Return False
    End Function

    Private Sub CalculateAllowances()
        For Each allowance In _allowances
            Dim item = New AllowanceItem() With {
                .OrganizationID = z_OrganizationID,
                .Created = Date.Now,
                .LastUpd = Date.Now,
                .CreatedBy = z_User,
                .LastUpdBy = z_User,
                .PayPeriodID = PayPeriodID,
                .AllowanceID = allowance.RowID
            }

            If allowance.AllowanceFrequency = "One time" Then
                item.Amount = allowance.Amount
            ElseIf allowance.AllowanceFrequency = "Daily" Then
                item = CalculateDailyProratedAllowance(allowance)
            ElseIf allowance.AllowanceFrequency = "Semi-monthly" Then
                If allowance.Product.Fixed Then
                    item.Amount = allowance.Amount
                Else
                    item = CalculateSemiMonthlyProratedAllowance(allowance)
                End If
            ElseIf allowance.AllowanceFrequency = "Monthly" Then
                If allowance.Product.Fixed And _isEndOfMonth Then
                    item.Amount = allowance.Amount
                End If
            Else
                item = Nothing
            End If

            _allowanceItems.Add(item)
        Next

        _paystub.TotalAllowance = _allowanceItems.Sum(Function(a) a.Amount)
    End Sub

    Private Sub UpdateLeaveLedger(db As PayrollContext)
        Dim leaves = (From l In db.Leaves
                      Where PayrollDateFrom <= l.StartDate And
                            l.StartDate <= PayrollDateTo And
                            l.EmployeeID = _employee2.RowID
                      Select l).ToList()

        Dim leaveIds = leaves.Select(Function(l) l.RowID)

        Dim transactions = (From t In db.LeaveTransactions
                            Where leaveIds.Contains(t.ReferenceID)).ToList()

        Dim ledgers = (From l In db.LeaveLedgers.
                           Include(Function(l) l.Product).
                           Include(Function(l) l.LastTransaction)
                       Where l.EmployeeID = _employee2.RowID).
                       ToList()

        Dim newLeaveTransactions = New List(Of LeaveTransaction)
        For Each leave In leaves
            ' If a transaction has already been made for the current leave, skip the current leave.
            If transactions.Any(Function(t) t.ReferenceID = leave.RowID) Then
                Continue For
            Else
                Dim ledger = ledgers.
                    FirstOrDefault(Function(l) l.Product.PartNo = leave.LeaveType)

                Dim timeEntry = _timeEntries2.
                    FirstOrDefault(Function(t) t.Date = leave.StartDate)

                If timeEntry Is Nothing Then
                    Continue For
                End If

                Dim newTransaction = New LeaveTransaction() With {
                    .OrganizationID = z_OrganizationID,
                    .Created = Date.Now,
                    .EmployeeID = leave.EmployeeID,
                    .PayPeriodID = PayPeriodID,
                    .ReferenceID = leave.RowID,
                    .TransactionDate = Date.Today,
                    .Type = LeaveTransactionType.Debit,
                    .Amount = timeEntry.TotalLeaveHours,
                    .Balance = If(ledger?.LastTransaction?.Balance, 0) - timeEntry.TotalLeaveHours
                }

                ledger.LeaveTransactions.Add(newTransaction)
                ledger.LastTransaction = newTransaction
            End If
        Next
    End Sub

    Private Sub CalculateBonuses()
        Dim oneTimeBonuses = allOneTimeBonuses.Select($"EmployeeID = '{_paystub.EmployeeID}'")
        Dim dailyBonuses = allDailyBonuses.Select($"EmployeeID = '{_paystub.EmployeeID}'")
        Dim weeklyBonuses = allWeeklyBonuses.Select($"EmployeeID = '{_paystub.EmployeeID}'")
        Dim semiMonthlyBonuses = allSemiMonthlyBonuses.Select($"EmployeeID = '{_paystub.EmployeeID}'")
        Dim monthlyBonuses = allMonthlyBonuses.Select($"EmployeeID = '{_paystub.EmployeeID}'")

        Dim noTaxOneBonuses = allNoTaxOneTimeBonuses.Select($"EmployeeID = '{_paystub.EmployeeID}'")
        Dim noTaxDailyBonuses = allNoTaxDailyBonuses.Select($"EmployeeID = '{_paystub.EmployeeID}'")
        Dim noTaxWeeklyBonuses = allNoTaxWeeklyBonuses.Select($"EmployeeID = '{_paystub.EmployeeID}'")
        Dim noTaxSemiMonthlyBonuses = allNoTaxSemiMonthlyBonuses.Select($"EmployeeID = '{_paystub.EmployeeID}'")
        Dim noTaxMonthlyBonuses = allNoTaxMonthlyBonuses.Select($"EmployeeID = '{_paystub.EmployeeID}'")
        Dim workDaysPerYear = _employee2.WorkDaysPerYear
        'Dim divisorMonthlys = If(
        '        CInt(_employee("PayFrequencyID")) = 1,
        '        2,
        '        If(
        '            CInt(_employee("PayFrequencyID")) = 2,
        '            1,
        '            If(
        '                CInt(_employee("PayFrequencyID")) = 3,
        '                CInt(New MySQLExecuteQuery("SELECT COUNT(RowID) FROM employeetimeentry WHERE EmployeeID='" & _payStub.EmployeeID & "' AND Date BETWEEN '" & PayrollDateFrom & "' AND '" & PayrollDateTo & "' AND IFNULL(TotalDayPay,0)!=0 AND OrganizationID='" & orgztnID & "';").Result),
        '                numberofweeksthismonth
        '            )
        '        )
        '    )

        Dim totalTaxableBonus = 0D

        Dim valdaynotax_bon = 0D
        For Each drowdaybon In noTaxDailyBonuses
            valdaynotax_bon = ValNoComma(drowdaybon("BonusAmount"))

            If _employee2.EmployeeType = "Fixed" Then
                valdaynotax_bon = valdaynotax_bon * workDaysPerYear 'numofweekends
            Else
                Dim daymultiplier = _numOfDayPresent.Select($"EmployeeID = '{_paystub.EmployeeID}'")
                For Each drowdaymultip In daymultiplier
                    Dim i_val = CInt(drowdaymultip("DaysAttended"))
                    valdaynotax_bon = valdaynotax_bon * i_val
                    Exit For
                Next

            End If

            Exit For
        Next

        Dim valmonthnotax_bon = 0D

        If isEndOfMonth2 = "1" Then

            For Each drowmonbon In noTaxMonthlyBonuses
                'valmonthnotax_bon = drowmonbon("BonusAmount")
            Next

        End If

        Dim valoncenotax_bon = 0D
        For Each drowoncebon In noTaxOneBonuses
            'valoncenotax_bon = drowoncebon("BonusAmount")
        Next

        Dim valsemimnotax_bon = 0D
        For Each drowsemimbon In noTaxSemiMonthlyBonuses
            'valoncenotax_bon = drowsemimbon("BonusAmount")
        Next

        Dim valweeknotax_bon = 0D
        For Each drowweekbon In noTaxWeeklyBonuses
            'valoncenotax_bon = drowweekbon("BonusAmount")
        Next

        'this is non-taxable
        Dim totalNoTaxBonus = (
            valoncenotax_bon +
            valdaynotax_bon +
            valweeknotax_bon +
            valsemimnotax_bon
        )

        _paystub.TotalBonus = totalTaxableBonus + totalNoTaxBonus
    End Sub

    Private Sub CalculateSss(salary As DataRow)
        Dim employeeSssPerMonth = ValNoComma(salary("EmployeeContributionAmount"))
        Dim employerSssPerMonth = ValNoComma(salary("EmployerContributionAmount"))
        Dim payPeriodsPerMonth = ValNoComma(_employee("PAYFREQUENCY_DIVISOR"))
        Dim is_weekly As Boolean = Convert.ToBoolean(Convert.ToInt16(_employee("IsWeeklyPaid")))
        Dim is_under_agency As Boolean = Convert.ToBoolean(Convert.ToInt16(_employee("IsUnderAgency")))

        If False Then
            Dim socialSecurityBracket = _socialSecurityBrackets.FirstOrDefault(
                Function(s)
                    Return s.RangeFromAmount <= _paystub.GrossPay And
                        s.RangeToAmount >= _paystub.GrossPay
                End Function
            )

            employeeSssPerMonth = socialSecurityBracket.EmployeeContributionAmount
            employerSssPerMonth = socialSecurityBracket.EmployerContributionAmount
        End If

        If is_weekly Then
            Dim is_deduct_sched_to_thisperiod As Boolean = False

            Dim pp = New Collection(Of PayPeriod)

            If is_under_agency Then
                Using context = New PayrollContext()
                    Dim query = From p In context.PayPeriods
                                Where p.RowID = _PayPeriodID

                    is_deduct_sched_to_thisperiod =
                        Convert.ToBoolean(query.FirstOrDefault.SSSWeeklyAgentContribSched)
                End Using
            Else
                Using context = New PayrollContext()
                    Dim query = From p In context.PayPeriods
                                Where p.RowID = _PayPeriodID

                    is_deduct_sched_to_thisperiod =
                        Convert.ToBoolean(query.FirstOrDefault.SSSWeeklyContribSched)
                End Using
            End If

            If is_deduct_sched_to_thisperiod Then
                _paystub.SssEmployeeShare = employeeSssPerMonth
                _paystub.SssEmployerShare = employerSssPerMonth
            Else
                _paystub.SssEmployeeShare = 0
                _paystub.SssEmployerShare = 0
            End If
        Else
            If IsSssPaidOnFirstHalf() Or IsSssPaidOnEndOfTheMonth() Then
                _paystub.SssEmployeeShare = employeeSssPerMonth
                _paystub.SssEmployerShare = employerSssPerMonth
            ElseIf IsSssPaidPerPayPeriod() Then
                _paystub.SssEmployeeShare = employeeSssPerMonth / payPeriodsPerMonth
                _paystub.SssEmployerShare = employerSssPerMonth / payPeriodsPerMonth
            End If
        End If

    End Sub

    Private Function IsSssPaidOnFirstHalf() As Boolean
        Return _isFirstHalf And (_sssDeductionSchedule = ContributionSchedule.FirstHalf)
    End Function

    Private Function IsSssPaidOnEndOfTheMonth() As Boolean
        Return _isEndOfMonth And (_sssDeductionSchedule = ContributionSchedule.EndOfTheMonth)
    End Function

    Private Function IsSssPaidPerPayPeriod() As Boolean
        Return _sssDeductionSchedule = ContributionSchedule.PerPayPeriod
    End Function

    Private Sub CalculatePhilHealth(salary As DataRow)
        Dim totalContribution = ConvertToType(Of Decimal)(salary("PhilHealthDeduction"))
        Dim is_weekly As Boolean = Convert.ToBoolean(Convert.ToInt16(_employee("IsWeeklyPaid")))
        Dim is_under_agency As Boolean = Convert.ToBoolean(Convert.ToInt16(_employee("IsUnderAgency")))

        If False Then
            Dim philHealthBracket = _philHealthBrackets.FirstOrDefault(
                Function(p)
                    Return p.SalaryRangeFrom <= _paystub.GrossPay And
                        p.SalaryRangeTo >= _paystub.GrossPay
                End Function
            )

            totalContribution = If(philHealthBracket?.TotalMonthlyPremium, 0D)
        End If

        Dim halfContribution = AccuMath.Truncate(totalContribution / 2, 2)

        ' Account for any division loss by putting the missing value to the employer share
        Dim expectedTotal = halfContribution * 2
        Dim remaining = 0D
        If expectedTotal < totalContribution Then
            remaining = totalContribution - expectedTotal
        End If

        Dim employeeShare = halfContribution
        Dim employerShare = halfContribution + remaining

        Dim payPeriodsPerMonth = ValNoComma(_employee("PAYFREQUENCY_DIVISOR"))


        If is_weekly Then
            Dim is_deduct_sched_to_thisperiod As Boolean = False

            Dim pp = New Collection(Of PayPeriod)

            If is_under_agency Then
                Using context = New PayrollContext()
                    Dim query = From p In context.PayPeriods
                                Where p.RowID = _PayPeriodID

                    is_deduct_sched_to_thisperiod =
                        Convert.ToBoolean(query.FirstOrDefault.PhHWeeklyAgentContribSched)
                End Using
            Else
                Using context = New PayrollContext()
                    Dim query = From p In context.PayPeriods
                                Where p.RowID = _PayPeriodID

                    is_deduct_sched_to_thisperiod =
                        Convert.ToBoolean(query.FirstOrDefault.PhHWeeklyContribSched)
                End Using
            End If

            If is_deduct_sched_to_thisperiod Then
                _paystub.PhilHealthEmployeeShare = employeeShare
                _paystub.PhilHealthEmployerShare = employerShare
            Else
                _paystub.PhilHealthEmployeeShare = 0
                _paystub.PhilHealthEmployerShare = 0
            End If
        Else
            If IsPhilHealthPaidOnFirstHalf() Or IsPhilHealthPaidOnEndOfTheMonth() Then
                _paystub.PhilHealthEmployeeShare = employeeShare
                _paystub.PhilHealthEmployerShare = employerShare
            ElseIf IsPhilHealthPaidPerPayPeriod() Then
                _paystub.PhilHealthEmployeeShare = employeeShare / payPeriodsPerMonth
                _paystub.PhilHealthEmployerShare = employerShare / payPeriodsPerMonth
            End If
        End If
    End Sub

    Private Function IsPhilHealthPaidOnFirstHalf() As Boolean
        Return _isFirstHalf And (_philHealthDeductionSchedule = ContributionSchedule.FirstHalf)
    End Function

    Private Function IsPhilHealthPaidOnEndOfTheMonth() As Boolean
        Return _isEndOfMonth And (_philHealthDeductionSchedule = ContributionSchedule.EndOfTheMonth)
    End Function

    Private Function IsPhilHealthPaidPerPayPeriod() As Boolean
        Return _philHealthDeductionSchedule = ContributionSchedule.PerPayPeriod
    End Function

    Private Sub CalculateHdmf(salary As DataRow)
        Dim employeeHdmfPerMonth = ValNoComma(salary("HDMFAmount"))
        Dim employerHdmfPerMonth = 100D
        Dim payPeriodsPerMonth = ValNoComma(_employee("PAYFREQUENCY_DIVISOR"))
        Dim is_weekly As Boolean = Convert.ToBoolean(Convert.ToInt16(_employee("IsWeeklyPaid")))
        Dim is_under_agency As Boolean = Convert.ToBoolean(Convert.ToInt16(_employee("IsUnderAgency")))


        If is_weekly Then
            Dim is_deduct_sched_to_thisperiod As Boolean = False

            Dim pp = New Collection(Of PayPeriod)

            If is_under_agency Then
                Using context = New PayrollContext()
                    Dim query = From p In context.PayPeriods
                                Where p.RowID = _PayPeriodID

                    is_deduct_sched_to_thisperiod =
                        Convert.ToBoolean(query.FirstOrDefault.HDMFWeeklyAgentContribSched)
                End Using
            Else
                Using context = New PayrollContext()
                    Dim query = From p In context.PayPeriods
                                Where p.RowID = _PayPeriodID

                    is_deduct_sched_to_thisperiod =
                        Convert.ToBoolean(query.FirstOrDefault.HDMFWeeklyContribSched)
                End Using
            End If

            If is_deduct_sched_to_thisperiod Then
                _paystub.HdmfEmployeeShare = employeeHdmfPerMonth
                _paystub.HdmfEmployerShare = employerHdmfPerMonth
            Else
                _paystub.HdmfEmployeeShare = 0
                _paystub.HdmfEmployerShare = 0
            End If
        Else
            If IsHdmfPaidOnFirstHalf() Or IsHdmfPaidOnEndOfTheMonth() Then
                _paystub.HdmfEmployeeShare = employeeHdmfPerMonth
                _paystub.HdmfEmployerShare = employerHdmfPerMonth
            ElseIf IsHdmfPaidPerPayPeriod() Then
                _paystub.HdmfEmployeeShare = employeeHdmfPerMonth / payPeriodsPerMonth
                _paystub.HdmfEmployerShare = employerHdmfPerMonth / payPeriodsPerMonth
            End If
        End If
    End Sub

    Private Function IsHdmfPaidOnFirstHalf() As Boolean
        Return _isFirstHalf And (_hdmfDeductionSchedule = ContributionSchedule.FirstHalf)
    End Function

    Private Function IsHdmfPaidOnEndOfTheMonth() As Boolean
        Return _isEndOfMonth And (_hdmfDeductionSchedule = ContributionSchedule.EndOfTheMonth)
    End Function

    Private Function IsHdmfPaidPerPayPeriod() As Boolean
        Return _hdmfDeductionSchedule = ContributionSchedule.PerPayPeriod
    End Function

    Private Sub CalculateWithholdingTax()
        Dim payFrequencyID As Integer
        Dim is_weekly As Boolean = Convert.ToBoolean(Convert.ToInt16(_employee("IsWeeklyPaid")))
        Dim is_under_agency As Boolean = Convert.ToBoolean(Convert.ToInt16(_employee("IsUnderAgency")))

        If IsWithholdingTaxPaidOnFirstHalf() Or IsWithholdingTaxPaidOnEndOfTheMonth() Then
            payFrequencyID = PayFrequency.Monthly
        ElseIf IsWithholdingTaxPaidPerPayPeriod() Then
            payFrequencyID = PayFrequency.SemiMonthly
        End If

        ' Round the daily rate to two decimal places since amounts in the 3rd decimal place
        ' isn't significant enough to warrant the employee to be taxable.
        Dim dailyRate = Math.Round(ValNoComma(_employee("EmpRatePerDay")), 2)

        Dim minimumWage = ValNoComma(_employee("MinimumWageAmount"))
        Dim isMinimumWageEarner = dailyRate <= minimumWage

        If isMinimumWageEarner Then
            _paystub.TaxableIncome = 0D
        End If

        If Not (_paystub.TaxableIncome > 0D And IsScheduledForTaxation()) Then
            Return
        End If

        Dim maritalStatus = _employee2.MaritalStatus
        Dim noOfDependents = _employee2.NoOfDependents

        Dim filingStatus = _filingStatuses.
            Select($"
                MaritalStatus = '{maritalStatus}' AND
                Dependent <= '{noOfDependents}'
            ").
            OrderByDescending(Function(f) CInt(f("Dependent"))).
            FirstOrDefault()

        Dim filingStatusID = 1
        If filingStatus IsNot Nothing Then
            filingStatusID = CInt(filingStatus("RowID"))
        End If

        Dim bracket = GetMatchingTaxBracket(payFrequencyID, filingStatusID)

        If bracket Is Nothing Then
            Return
        End If

        Dim exemptionAmount = bracket.ExemptionAmount
        Dim taxableIncomeFromAmount = bracket.TaxableIncomeFromAmount
        Dim exemptionInExcessAmount = bracket.ExemptionInExcessAmount

        Dim excessAmount = _paystub.TaxableIncome - taxableIncomeFromAmount


        If is_weekly Then
            Dim is_deduct_sched_to_thisperiod As Boolean = False

            Dim pp = New Collection(Of PayPeriod)

            If is_under_agency Then
                Using context = New PayrollContext()
                    Dim query = From p In context.PayPeriods
                                Where p.RowID = _PayPeriodID

                    is_deduct_sched_to_thisperiod =
                        Convert.ToBoolean(query.FirstOrDefault.WTaxWeeklyAgentContribSched)
                End Using
            Else
                Using context = New PayrollContext()
                    Dim query = From p In context.PayPeriods
                                Where p.RowID = _PayPeriodID

                    is_deduct_sched_to_thisperiod =
                        Convert.ToBoolean(query.FirstOrDefault.WTaxWeeklyContribSched)
                End Using
            End If

            If is_deduct_sched_to_thisperiod Then
                _paystub.WithholdingTax = exemptionAmount + (excessAmount * exemptionInExcessAmount)
            Else
                _paystub.WithholdingTax = 0
            End If
        Else
            _paystub.WithholdingTax = exemptionAmount + (excessAmount * exemptionInExcessAmount)
        End If
    End Sub

    Private Function GetMatchingTaxBracket(payFrequencyID As Integer?, filingStatusID As Integer?) As WithholdingTaxBracket
        Dim taxEffectivityDate = New Date(_payPeriod.Year, _payPeriod.Month, 1)

        Dim possibleBrackets =
            (From w In _withholdingTaxBrackets
             Where w.PayFrequencyID = payFrequencyID And
                 w.TaxableIncomeFromAmount <= _paystub.TaxableIncome And
                 _paystub.TaxableIncome <= w.TaxableIncomeToAmount And
                 w.EffectiveDateFrom <= taxEffectivityDate And
                 taxEffectivityDate <= w.EffectiveDateTo
             Select w).
             ToList()

        ' If there are more than one tax brackets that matches the previous list, filter by
        ' the tax filing status.
        If possibleBrackets.Count > 1 Then
            Return possibleBrackets.
                Where(Function(b) Nullable.Equals(b.FilingStatusID, filingStatusID)).
                FirstOrDefault()
        ElseIf possibleBrackets.Count = 1 Then
            Return possibleBrackets.First()
        End If

        Return Nothing
    End Function

    Private Function IsWithholdingTaxPaidOnFirstHalf() As Boolean
        Return _isFirstHalf And (_withholdingTaxSchedule = ContributionSchedule.FirstHalf)
    End Function

    Private Function IsWithholdingTaxPaidOnEndOfTheMonth() As Boolean
        Return _isEndOfMonth And (_withholdingTaxSchedule = ContributionSchedule.EndOfTheMonth)
    End Function

    Private Function IsWithholdingTaxPaidPerPayPeriod() As Boolean
        Return _withholdingTaxSchedule = ContributionSchedule.PerPayPeriod
    End Function

    Private Function IsScheduledForTaxation() As Boolean
        Return (_isFirstHalf And IsWithholdingTaxPaidOnFirstHalf()) Or
            (_isEndOfMonth And IsWithholdingTaxPaidOnEndOfTheMonth()) Or
            IsWithholdingTaxPaidPerPayPeriod()
    End Function

    Private Function IsFirstPayperiodOfTheYear() As Boolean
        If Me.payPeriod Is Nothing Then
            Return False
        End If

        Return CInt(Me.payPeriod("OrdinalValue")) = 1
    End Function

    Private Sub LoadAnnualUnusedLeaves()
        Dim sql = <![CDATA[
            SELECT RowID, OrganizationID, EmployeeID, TotalLeave, Year
            FROM annualunusedleave
            WHERE OrganizationID = @OrganizationID
                AND Year = @Year;
        ]]>.Value

        Dim currentYear = CInt(Me.payPeriod("Year"))
        Dim previousYear = currentYear - 1

        sql = sql.Replace("@OrganizationID", CStr(z_OrganizationID)) _
            .Replace("@Year", CStr(previousYear))

        Me.annualUnusedLeaves = GetDatatable(sql)
    End Sub

    Private Sub LoadExistingUnusedLeaveAdjustments()
        Dim sql = <![CDATA[
            SELECT
                paystubadjustment.*,
                paystub.EmployeeID
            FROM paystubadjustment
            LEFT JOIN paystub
                ON paystub.RowID = paystubadjustment.PayStubID
            WHERE
                paystubadjustment.ProductID = @ProductID AND
                paystub.PayPeriodID = @PayPeriodID AND
                paystubadjustment.OrganizationID = @OrganizationID;
        ]]>.Value

        sql = sql.Replace("@ProductID", unusedLeaveProductID) _
            .Replace("@OrganizationID", CStr(z_OrganizationID)) _
            .Replace("@PayPeriodID", PayPeriodID)

        Me.existingUnusedLeaveAdjustments = GetDatatable(sql)
    End Sub

    Private Sub LoadCurrentPayperiod()
        Dim sql = <![CDATA[
            SELECT Year, OrdinalValue
            FROM payperiod
            WHERE RowID = @PayPeriodID;
        ]]>.Value

        sql = sql.Replace("@PayPeriodID", Me.PayPeriodID)

        Dim results = GetDatatable(sql)
        Me.payPeriod = results(0)
    End Sub

    Private Sub LoadProductIDForUnusedLeave()
        Dim sql = <![CDATA[
            SELECT RowID
            FROM product
            WHERE PartNo = 'Unused leaves'
                AND OrganizationID = @OrganizationID
            LIMIT 1;
        ]]>.Value

        sql = sql.Replace("@OrganizationID", CStr(z_OrganizationID))

        Dim results = GetDatatable(sql)
        Dim row = results(0)
        Me.unusedLeaveProductID = CStr(row("RowID"))
    End Sub

    Private Sub ResetUnusedLeaveAdjustments()
        Dim sql = <![CDATA[
            DELETE paystubadjustment
            FROM paystubadjustment
            LEFT JOIN paystub
                ON paystub.RowID = paystubadjustment.PayStubID
            WHERE paystubadjustment.ProductID = @ProductID AND
                paystub.PayPeriodID = @PayPeriodID AND
                paystubadjustment.OrganizationID = @OrganizationID;
        ]]>.Value

        sql = sql.Replace("@ProductID", unusedLeaveProductID) _
            .Replace("@OrganizationID", CStr(z_OrganizationID)) _
            .Replace("@PayPeriodID", PayPeriodID)

        EXECQUER(sql)
    End Sub

    Private Sub ConvertLeaveToCash(employee As DataRow)
        Dim employeeID = employee("RowID")
        Dim employeeType = employee("EmployeeType")
        Dim employeeOID = employee("EmployeeID")
        Dim unusedLeave = annualUnusedLeaves.Select($"EmployeeID = '{employeeID}'") _
            .FirstOrDefault()

        ' Perform basic checks to see if employee has any unused leaves
        If unusedLeave Is Nothing Then
            Return
        End If

        Dim totalLeaves = ValNoComma(unusedLeave("TotalLeave"))

        If totalLeaves <= 0D Then
            Return
        End If

        ' Let's find this employee's salary to derive the hourly pay
        Dim salaryAgreement = _allSalaries.Select($"EmployeeID = '{employeeID}'") _
            .FirstOrDefault()

        Dim basicPay = ConvertToType(Of Decimal)(salaryAgreement("BasicPay"))
        Dim hoursInAWorkDay = 8
        Dim workDaysPerYear = ValNoComma(employee("WorkDaysPerYear"))
        Dim workDaysPerMonth = workDaysPerYear / 12

        Dim hourlySalary = 0D
        If CStr(employee("EmployeeType")) = SalaryType.Daily Then
            hourlySalary = basicPay / hoursInAWorkDay
        ElseIf CStr(employee("EmployeeType")) = SalaryType.Monthly Then
            hourlySalary = basicPay / workDaysPerMonth / hoursInAWorkDay
        End If

        Dim adjustmentAmount = hourlySalary * totalLeaves

        ' Make the custom remark for the adjustment
        Dim totalLeavesInDays = totalLeaves / hoursInAWorkDay
        Dim remarks = totalLeavesInDays & " unused leaves"

        Dim existingAdjustment = existingUnusedLeaveAdjustments.Select($"EmployeeID = '{employeeID}'") _
            .FirstOrDefault()

        Dim payStubAdjustmentID = If(existingAdjustment IsNot Nothing, existingAdjustment("RowID"), Nothing)

        Dim a = New ReadSQLFunction(
            "I_paystubadjustment",
            "returnvalue",
            orgztnID,
            z_User,
            unusedLeaveProductID,
            adjustmentAmount,
            remarks,
            employeeOID,
            Me.PayPeriodID,
            payStubAdjustmentID
        )
    End Sub

End Class

Friend Class MySQLExecuteQuery

    Private priv_conn As New MySqlConnection

    Private priv_da As New MySqlDataAdapter

    Private priv_cmd As New MySqlCommand

    Private getResult As Object

    Dim dr As MySqlDataReader

    Sub New(ByVal cmdsql As String,
            Optional cmd_time_out As Integer = 0)

        Static except_this_string() As String = {"CALL", "UPDATE", "DELETE"}

        'Dim n_DataBaseConnection As New DataBaseConnection

        If cmd_time_out > 0 Then
            'n_DataBaseConnection.GetStringMySQLConnectionString
            priv_conn.ConnectionString = mysql_conn_text &
                "default command timeout=" & cmd_time_out & ";"
        Else
            'n_DataBaseConnection.GetStringMySQLConnectionString
            priv_conn.ConnectionString = mysql_conn_text

        End If

        Try

            If priv_conn.State = ConnectionState.Open Then : priv_conn.Close() : End If

            priv_conn.Open()

            priv_cmd = New MySqlCommand

            With priv_cmd

                .CommandType = CommandType.Text

                .Connection = priv_conn

                .CommandText = cmdsql

                If cmd_time_out > 0 Then
                    .CommandTimeout = cmd_time_out
                End If

                If cmdsql.Contains("CALL") Then

                    .ExecuteNonQuery()

                ElseIf FindingWordsInString(cmdsql,
                                            except_this_string) Then

                    .ExecuteNonQuery()
                Else

                    dr = .ExecuteReader()

                End If

            End With

            If cmdsql.Contains("CALL") Then
                getResult = Nothing
            ElseIf FindingWordsInString(cmdsql,
                                        except_this_string) Then
                getResult = Nothing
            Else

                If dr.Read = True Then

                    If IsDBNull(dr(0)) Then
                        getResult = String.Empty
                    Else
                        getResult = dr(0)

                    End If
                Else
                    getResult = Nothing

                End If

            End If
        Catch ex As Exception

            _hasError = True
            _error_message = getErrExcptn(ex, MyBase.ToString)
            'MsgBox(_error_message, , cmdsql)
            Console.WriteLine(_error_message)
        Finally

            If dr IsNot Nothing Then
                dr.Close()
                dr.Dispose()
            End If

            priv_conn.Close()

            priv_cmd.Dispose()

        End Try

    End Sub

    Property Result As Object

        Get
            Return getResult

        End Get

        Set(value As Object)
            getResult = value

        End Set

    End Property

    Sub ExecuteQuery()

    End Sub

    Dim _hasError As Boolean = False

    Property HasError As Boolean

        Get
            Return _hasError

        End Get

        Set(value As Boolean)
            _hasError = value

        End Set

    End Property

    Dim _error_message As String = String.Empty

    Property ErrorMessage As String

        Get
            Return _error_message

        End Get

        Set(value As String)
            _error_message = value

        End Set

    End Property

End Class
