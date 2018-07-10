Imports System.Collections.ObjectModel
Imports System.Data.Entity
Imports AccuPay.Entity
Imports AccuPay.Loans
Imports log4net
Imports NHibernate
Imports PayrollSys

Public Class PayrollGeneration

    Private Const PagibigEmployerAmount As Decimal = 100

    Private Shared logger As ILog = LogManager.GetLogger("PayrollLogger")

    Private _employee As DataRow

    Private _employee2 As Employee

    Private _isFirstHalf As Boolean
    Private _isEndOfMonth As Boolean
    Private _allSalaries As DataTable

    Private _allLoanSchedules As ICollection(Of LoanSchedule)

    Private _allLoanTransactions As ICollection(Of LoanTransaction)

    Private allWeeklyAllowances As DataTable
    Private allNoTaxWeeklyAllowances As DataTable

    Private _previousTimeEntries2 As ICollection(Of TimeEntry)

    Private _filingStatuses As DataTable

    Private Delegate Sub NotifyMainWindow(success As Boolean)

    Private _notifyMainWindow As NotifyMainWindow

    Private form_caller As Form

    Private _payPeriod As PayPeriod

    Private annualUnusedLeaves As DataTable
    Private unusedLeaveProductID As String
    Private existingUnusedLeaveAdjustments As DataTable

    Private _paystub As Paystub

    Private _previousPaystub As Paystub

    Private _products As IEnumerable(Of Product)

    Private _philHealthDeductionSchedule As String
    Private _sssDeductionSchedule As String
    Private _hdmfDeductionSchedule As String
    Private _withholdingTaxSchedule As String

    Private _socialSecurityBrackets As IEnumerable(Of SocialSecurityBracket)

    Private _philHealthBrackets As IEnumerable(Of PhilHealthBracket)

    Private _withholdingTaxBrackets As IEnumerable(Of WithholdingTaxBracket)

    Private _settings As ListOfValueCollection

    Private _timeEntries As ICollection(Of TimeEntry)

    Private _payRates As IReadOnlyDictionary(Of Date, PayRate)

    Private _allowances As ICollection(Of Allowance)

    Private _allowanceItems As ICollection(Of AllowanceItem) = New List(Of AllowanceItem)

    Private _actualtimeentries As ICollection(Of ActualTimeEntry)

    Sub New(employee As DataRow,
            payPeriodHalfNo As String,
            allWeeklyAllowances As DataTable,
            allNoTaxWeeklyAllowances As DataTable,
            filingStatuses As DataTable,
            resources As PayrollResources,
            Optional pay_stub_frm As PayStubForm = Nothing)
        form_caller = pay_stub_frm

        _employee = employee
        _employee2 = resources.Employees.
            FirstOrDefault(Function(e) e.RowID.ToString() = _employee("RowID"))

        Me._allSalaries = resources.Salaries
        _allLoanSchedules = resources.LoanSchedules
        _allLoanTransactions = resources.LoanTransactions

        Me.allWeeklyAllowances = allWeeklyAllowances
        Me.allNoTaxWeeklyAllowances = allNoTaxWeeklyAllowances

        _filingStatuses = filingStatuses

        _notifyMainWindow = AddressOf pay_stub_frm.ProgressCounter

        _products = resources.Products

        _paystub = resources.Paystubs.FirstOrDefault(
            Function(p) Nullable.Equals(p.EmployeeID, _employee2.RowID))

        _isFirstHalf = (payPeriodHalfNo = "1")
        _isEndOfMonth = (payPeriodHalfNo = "0")
        _socialSecurityBrackets = resources.SocialSecurityBrackets
        _philHealthBrackets = resources.PhilHealthBrackets
        _withholdingTaxBrackets = resources.WithholdingTaxBrackets

        _previousPaystub = resources.PreviousPaystubs.FirstOrDefault(
            Function(p) p.EmployeeID = _employee2.RowID)

        _settings = New ListOfValueCollection(resources.ListOfValues)
        _payPeriod = resources.PayPeriod

        _previousTimeEntries2 = resources.TimeEntries.
            Where(Function(t) t.EmployeeID = _employee2.RowID).
            ToList()

        _timeEntries = resources.TimeEntries.
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

            GeneratePayStub()
            form_caller.BeginInvoke(_notifyMainWindow, True)
        Catch ex As Exception
            logger.Error("DoProcess", ex)
            form_caller.BeginInvoke(_notifyMainWindow, False)
        End Try
    End Sub

    Private Sub GeneratePayStub()
        Try
            If _paystub Is Nothing Then
                _paystub = New Paystub() With {
                    .OrganizationID = z_OrganizationID,
                    .CreatedBy = z_User,
                    .LastUpdBy = z_User,
                    .EmployeeID = _employee2.RowID,
                    .PayPeriodID = _payPeriod.RowID,
                    .PayFromdate = _payPeriod.PayFromDate,
                    .PayToDate = _payPeriod.PayToDate
                }
            End If

            _sssDeductionSchedule = _employee("SSSDeductSched").ToString
            _philHealthDeductionSchedule = _employee("PhHealthDeductSched").ToString
            _hdmfDeductionSchedule = _employee("HDMFDeductSched").ToString
            _withholdingTaxSchedule = _employee("WTaxDeductSched").ToString

            _paystub.EmployeeID = _employee2.RowID

            Dim salary = _allSalaries.Select($"EmployeeID = '{_paystub.EmployeeID}'").FirstOrDefault()

            CalculateAllowances()

            _paystub.TotalAdjustments = If(_paystub?.Adjustments.Sum(Function(a) a.Amount), 0)

            Dim governmentContributions = 0D

            If (_timeEntries.Count > 0) And (salary IsNot Nothing) Then

                Dim basicPay = CDec(ValNoComma(salary("BasicPay")))

                ComputeHours()

                Dim currentTaxableIncome = 0D

                If _employee2.IsMonthly Or _employee2.IsFixed Then
                    If _employee2.WorkDaysPerYear > 0 Then
                        Dim workDaysPerPayPeriod = _employee2.WorkDaysPerYear / CalendarConstants.MonthsInAYear / CalendarConstants.SemiMonthlyPayPeriodsPerMonth

                        _paystub.BasicHours = workDaysPerPayPeriod * 8
                    End If

                    _paystub.BasicPay = basicPay
                End If

                If _employee2.EmployeeType = SalaryType.Fixed Then

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

                    _paystub.TotalEarnings = basicPay + extraPay

                    currentTaxableIncome = basicPay

                ElseIf _employee2.EmployeeType = SalaryType.Monthly Then

                    Dim isFirstPayAsDailyRule = _settings.GetBoolean("Payroll Policy", "isfirstsalarydaily")

                    Dim isFirstPay =
                        _payPeriod.PayFromDate <= _employee2.StartDate And
                        _employee2.StartDate <= _payPeriod.PayToDate

                    If isFirstPay And isFirstPayAsDailyRule Then
                        _paystub.TotalEarnings = _timeEntries.Sum(Function(t) t.TotalDayPay)
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

                        Dim taxablePolicy = If(_settings.GetString("Payroll Policy.paystub.taxableincome"), "Basic Pay")

                        If taxablePolicy = "Gross Income" Then
                            currentTaxableIncome = _paystub.TotalEarnings
                        Else
                            currentTaxableIncome = basicPay
                        End If
                    End If

                ElseIf _employee2.EmployeeType = SalaryType.Daily Then
                    _paystub.BasicHours = _timeEntries.
                        Where(Function(t) Not If(t.ShiftSchedule?.IsRestDay, True)).
                        Sum(Function(t) t.ShiftSchedule?.Shift?.WorkHours)

                    _paystub.BasicPay = _timeEntries.Sum(Function(t) t.BasicDayPay)

                    _paystub.TotalEarnings =
                        _paystub.RegularPay +
                        _paystub.OvertimePay +
                        _paystub.NightDiffPay +
                        _paystub.NightDiffOvertimePay +
                        _paystub.RestDayPay +
                        _paystub.RestDayOTPay +
                        _paystub.SpecialHolidayPay +
                        _paystub.SpecialHolidayOTPay +
                        _paystub.RegularHolidayPay +
                        _paystub.RegularHolidayOTPay +
                        _paystub.LeavePay
                End If

                CalculateSss(salary)
                CalculatePhilHealth(salary)
                CalculateHdmf(salary)

                governmentContributions = _paystub.SssEmployeeShare + _paystub.PhilHealthEmployeeShare + _paystub.HdmfEmployeeShare
                currentTaxableIncome = currentTaxableIncome - governmentContributions

                If IsWithholdingTaxPaidOnFirstHalf() Or IsWithholdingTaxPaidOnEndOfTheMonth() Then
                    _paystub.TaxableIncome = currentTaxableIncome + If(_previousPaystub?.TaxableIncome, 0D)
                ElseIf IsWithholdingTaxPaidPerPayPeriod() Then
                    _paystub.TaxableIncome = currentTaxableIncome
                Else
                    _paystub.TaxableIncome = currentTaxableIncome
                End If

                CalculateWithholdingTax()
            End If

            Dim newLoanTransactions = ComputeLoans()

            _paystub.GrossPay = _paystub.TotalEarnings + _paystub.TotalBonus + _paystub.TotalAllowance
            _paystub.NetPay = AccuMath.CommercialRound(_paystub.GrossPay - (governmentContributions + _paystub.TotalLoans + _paystub.WithholdingTax) + _paystub.TotalAdjustments)

            Dim vacationLeaveProduct = _products.Where(Function(p) p.PartNo = "Vacation leave").FirstOrDefault()
            Dim sickLeaveProduct = _products.Where(Function(p) p.PartNo = "Sick leave").FirstOrDefault()

            Using session = SessionFactory.Instance.OpenSession(),
                trans = session.BeginTransaction()

                session.FlushMode = FlushMode.Commit

                UpdateLeaveLedger(session)

                If _paystub.RowID IsNot Nothing Then
                    _paystub = session.Merge(_paystub)
                End If

                session.Delete("from AllowanceItem a where a.Paystub.RowID = ?", _paystub.RowID, NHibernate.NHibernateUtil.Int32)

                _paystub.AllowanceItems.Clear()
                For Each allowanceItem In _allowanceItems
                    _paystub.AllowanceItems.Add(allowanceItem)
                Next

                ComputeThirteenthMonthPay(salary)

                Dim vacationLeaveBalance = session.Query(Of PaystubItem).
                    Where(Function(p) p.Product.PartNo = "Vacation leave").
                    Where(Function(p) CBool(p.Paystub.RowID = _paystub.RowID)).
                    FirstOrDefault()

                If vacationLeaveBalance Is Nothing Then
                    Dim vacationLeaveUsed = _timeEntries.Sum(Function(t) t.VacationLeaveHours)
                    Dim newBalance = _employee2.LeaveBalance - vacationLeaveUsed

                    vacationLeaveBalance = New PaystubItem() With {
                        .OrganizationID = z_OrganizationID,
                        .ProductID = vacationLeaveProduct?.RowID,
                        .PayAmount = newBalance,
                        .Paystub = _paystub
                    }

                    _paystub.PaystubItems.Add(vacationLeaveBalance)
                End If

                Dim sickLeaveBalance = session.Query(Of PaystubItem).
                    Where(Function(p) p.Product.PartNo = "Sick leave").
                    Where(Function(p) CBool(p.Paystub.RowID = _paystub.RowID)).
                    FirstOrDefault()

                If sickLeaveBalance Is Nothing Then
                    Dim sickLeaveUsed = _timeEntries.Sum(Function(t) t.SickLeaveHours)
                    Dim newBalance = _employee2.SickLeaveBalance - sickLeaveUsed

                    sickLeaveBalance = New PaystubItem() With {
                        .OrganizationID = z_OrganizationID,
                        .ProductID = sickLeaveProduct?.RowID,
                        .PayAmount = newBalance,
                        .Paystub = _paystub
                    }

                    _paystub.PaystubItems.Add(sickLeaveBalance)
                End If

                session.SaveOrUpdate(_paystub)

                For Each newLoanTransaction In newLoanTransactions
                    session.Save(newLoanTransaction)
                Next

                trans.Commit()
            End Using
        Catch ex As Exception
            Throw New Exception($"Failure to generate paystub for employee {_paystub.EmployeeID}", ex)
        End Try
    End Sub

    Private Sub ComputeHours()
        _paystub.RegularHours = _timeEntries.Sum(Function(t) t.RegularHours)
        _paystub.RegularPay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.RegularPay))

        _paystub.OvertimeHours = _timeEntries.Sum(Function(t) t.OvertimeHours)
        _paystub.OvertimePay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.OvertimePay))

        _paystub.NightDiffHours = _timeEntries.Sum(Function(t) t.NightDiffHours)
        _paystub.NightDiffPay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.NightDiffPay))

        _paystub.NightDiffOvertimeHours = _timeEntries.Sum(Function(t) t.NightDiffOTHours)
        _paystub.NightDiffOvertimePay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.NightDiffOTPay))

        _paystub.RestDayHours = _timeEntries.Sum(Function(t) t.RestDayHours)
        _paystub.RestDayPay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.RestDayPay))

        _paystub.RestDayOTHours = _timeEntries.Sum(Function(t) t.RestDayOTHours)
        _paystub.RestDayOTPay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.RestDayOTPay))

        _paystub.SpecialHolidayHours = _timeEntries.Sum(Function(t) t.SpecialHolidayHours)
        _paystub.SpecialHolidayPay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.SpecialHolidayPay))

        _paystub.SpecialHolidayOTHours = _timeEntries.Sum(Function(t) t.SpecialHolidayOTHours)
        _paystub.SpecialHolidayOTPay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.SpecialHolidayOTPay))

        _paystub.RegularHolidayHours = _timeEntries.Sum(Function(t) t.RegularHolidayHours)
        _paystub.RegularHolidayPay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.RegularHolidayPay))

        _paystub.RegularHolidayOTHours = _timeEntries.Sum(Function(t) t.RegularHolidayOTHours)
        _paystub.RegularHolidayOTPay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.RegularHolidayOTPay))

        _paystub.HolidayPay = _timeEntries.Sum(Function(t) t.HolidayPay)

        _paystub.LeaveHours = _timeEntries.Sum(Function(t) t.TotalLeaveHours)
        _paystub.LeavePay = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.LeavePay))

        _paystub.LateHours = _timeEntries.Sum(Function(t) t.LateHours)
        _paystub.LateDeduction = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.LateDeduction))

        _paystub.UndertimeHours = _timeEntries.Sum(Function(t) t.UndertimeHours)
        _paystub.UndertimeDeduction = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.UndertimeDeduction))

        _paystub.AbsentHours = _timeEntries.Sum(Function(t) t.AbsentHours)
        _paystub.AbsenceDeduction = AccuMath.CommercialRound(_timeEntries.Sum(Function(t) t.AbsentDeduction))
    End Sub

    Private Sub CalculateAllowances()
        For Each allowance In _allowances
            Dim item = New AllowanceItem() With {
                .OrganizationID = z_OrganizationID,
                .Created = Date.Now,
                .LastUpd = Date.Now,
                .CreatedBy = z_User,
                .LastUpdBy = z_User,
                .PayPeriodID = _payPeriod.RowID,
                .AllowanceID = allowance.RowID,
                .Paystub = _paystub
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

        _paystub.TotalAllowance = AccuMath.CommercialRound(_allowanceItems.Sum(Function(a) a.Amount))
    End Sub

    Private Function CalculateSemiMonthlyProratedAllowance(allowance As Allowance) As AllowanceItem
        Dim workDaysPerYear = _employee2.WorkDaysPerYear
        Dim workingDays = CDec(workDaysPerYear / CalendarConstants.MonthsInAYear / CalendarConstants.SemiMonthlyPayPeriodsPerMonth)
        Dim dailyRate = allowance.Amount / workingDays

        Dim allowanceItem = New AllowanceItem() With {
            .OrganizationID = z_OrganizationID,
            .CreatedBy = z_User,
            .LastUpdBy = z_User,
            .Paystub = _paystub,
            .PayPeriodID = _payPeriod.RowID,
            .AllowanceID = allowance.RowID,
            .Amount = allowance.Amount
        }

        For Each TimeEntry In _timeEntries
            Dim divisor = If(TimeEntry.ShiftSchedule?.Shift?.DivisorToDailyRate, 8D)
            Dim hourlyRate = dailyRate / divisor

            Dim deductionHours =
                TimeEntry.LateHours +
                TimeEntry.UndertimeHours +
                TimeEntry.AbsentHours
            Dim deductionAmount = -(hourlyRate * deductionHours)

            Dim additionalAmount = 0D
            Dim giveAllowanceDuringHolidays = _settings.GetBoolean("Payroll Policy", "allowances.holiday")
            If giveAllowanceDuringHolidays Then
                Dim payRate = _payRates(TimeEntry.Date)

                If (payRate.IsSpecialNonWorkingHoliday And _employee2.CalcSpecialHoliday) Or
                   (payRate.IsRegularHoliday And _employee2.CalcHoliday) Then
                    additionalAmount = TimeEntry.RegularHours * hourlyRate * (payRate.CommonRate - 1D)
                End If
            End If

            allowanceItem.AddPerDay(TimeEntry.Date, deductionAmount + additionalAmount)
        Next

        Return allowanceItem
    End Function

    Private Function CalculateOneTimeAllowances(allowance As Allowance) As Decimal
        Return allowance.Amount
    End Function

    Private Function CalculateDailyProratedAllowance(allowance As Allowance) As AllowanceItem
        Dim dailyRate = allowance.Amount

        Dim allowanceItem = New AllowanceItem() With {
            .OrganizationID = z_OrganizationID,
            .CreatedBy = z_User,
            .LastUpdBy = z_User,
            .Paystub = _paystub,
            .PayPeriodID = _payPeriod.RowID,
            .AllowanceID = allowance.RowID
        }

        For Each TimeEntry In _timeEntries
            Dim divisor = If(TimeEntry.ShiftSchedule?.Shift?.DivisorToDailyRate, 8D)
            Dim hourlyRate = dailyRate / divisor

            Dim amount = 0D
            Dim payRate = _payRates(TimeEntry.Date)
            If payRate.IsRegularDay Then
                Dim isRestDay = TimeEntry.RestDayHours > 0

                If isRestDay Then
                    amount = dailyRate
                Else
                    amount = (TimeEntry.RegularHours + TimeEntry.TotalLeaveHours) * hourlyRate
                End If
            ElseIf payRate.IsSpecialNonWorkingHoliday Then
                Dim countableHours = TimeEntry.RegularHours + TimeEntry.SpecialHolidayHours + TimeEntry.TotalLeaveHours

                amount = If(countableHours > 0, dailyRate, 0D)
            ElseIf payRate.IsRegularHoliday Then
                amount = (TimeEntry.RegularHours + TimeEntry.RegularHolidayHours) * hourlyRate

                If HasWorkedLastWorkingDay(TimeEntry) Then
                    If _settings.GetString("AllowancePolicy.CalculationType") = "Hourly" Then
                        Dim workHours = If(TimeEntry.ShiftSchedule?.Shift?.WorkHours, 8D)

                        amount += {workHours * hourlyRate, dailyRate}.Max()
                    Else
                        amount += dailyRate
                    End If
                End If
            End If

            allowanceItem.AddPerDay(TimeEntry.Date, amount)
        Next

        Return allowanceItem
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

            If lastTimeEntry?.ShiftSchedule.IsRestDay Then
                If lastTimeEntry.TotalDayPay > 0 Then
                    Return True
                End If

                Continue For
            End If

            Dim payRate = _payRates(lastTimeEntry.Date)
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

    Private Sub UpdateLeaveLedger(session As ISession)
        Dim leaves = session.Query(Of Leave).
            Where(Function(l) _payPeriod.PayFromDate <= l.StartDate).
            Where(Function(l) l.StartDate <= _payPeriod.PayToDate).
            Where(Function(l) CBool(l.EmployeeID = _employee2.RowID)).
            ToList()

        Dim leaveIds = leaves.Select(Function(l) l.RowID)

        Dim transactions = (From t In session.Query(Of LeaveTransaction)
                            Where leaveIds.Contains(t.ReferenceID)).ToList()

        Dim employeeId = _employee2.RowID
        Dim ledgers = session.Query(Of LeaveLedger).
            Include(Function(x) x.Product).
            Include(Function(x) x.LastTransaction).
            Where(Function(x) CBool(x.EmployeeID = employeeId)).
            ToList()

        Dim newLeaveTransactions = New List(Of LeaveTransaction)
        For Each leave In leaves
            ' If a transaction has already been made for the current leave, skip the current leave.
            If transactions.Any(Function(t) Nullable.Equals(t.ReferenceID, leave.RowID)) Then
                Continue For
            Else
                Dim ledger = ledgers.
                    FirstOrDefault(Function(l) l.Product.PartNo = leave.LeaveType)

                Dim timeEntry = _timeEntries.
                    FirstOrDefault(Function(t) t.Date = leave.StartDate)

                If timeEntry Is Nothing Then
                    Continue For
                End If

                Dim newTransaction = New LeaveTransaction() With {
                    .OrganizationID = z_OrganizationID,
                    .Created = Date.Now,
                    .EmployeeID = leave.EmployeeID,
                    .PayPeriodID = _payPeriod.RowID,
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

    Private Sub CalculateSss(salary As DataRow)
        Dim is_weekly As Boolean = Convert.ToBoolean(Convert.ToInt16(_employee("IsWeeklyPaid")))

        Dim findSocialSecurityBracket =
            Function(amount As Decimal) _socialSecurityBrackets.FirstOrDefault(
                Function(s) s.RangeFromAmount <= amount And s.RangeToAmount >= amount)

        Dim sssCalculation = _settings.GetEnum("SocialSecuritySystem.CalculationBasis", SssCalculationBasis.BasicSalary)

        Dim isSssProrated =
            (sssCalculation = SssCalculationBasis.Earnings) Or
            (sssCalculation = SssCalculationBasis.GrossPay)

        Dim employeeSssPerMonth = 0D
        Dim employerSssPerMonth = 0D

        If isSssProrated Then
            Dim socialSecurityBracket As SocialSecurityBracket = Nothing

            If sssCalculation = SssCalculationBasis.Earnings Then
                Dim totalEarnings = If(_previousPaystub?.TotalEarnings, 0) + _paystub.TotalEarnings

                socialSecurityBracket = findSocialSecurityBracket(totalEarnings)
            ElseIf sssCalculation = SssCalculationBasis.GrossPay Then
                Dim totalGrossPay = If(_previousPaystub?.GrossPay, 0) + _paystub.GrossPay

                socialSecurityBracket = findSocialSecurityBracket(totalGrossPay)
            End If

            If socialSecurityBracket IsNot Nothing Then
                employeeSssPerMonth = socialSecurityBracket?.EmployeeContributionAmount
                employerSssPerMonth = socialSecurityBracket?.EmployerContributionAmount
            End If
        ElseIf sssCalculation = SssCalculationBasis.BasicSalary Then
            employeeSssPerMonth = ValNoComma(salary("EmployeeContributionAmount"))
            employerSssPerMonth = If(employeeSssPerMonth = 0, 0, ValNoComma(salary("EmployerContributionAmount")))
        End If

        If is_weekly Then
            Dim is_deduct_sched_to_thisperiod = False

            If _employee2.IsUnderAgency Then
                is_deduct_sched_to_thisperiod = _payPeriod.SSSWeeklyAgentContribSched
            Else
                is_deduct_sched_to_thisperiod = _payPeriod.SSSWeeklyContribSched
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
                Dim payPeriodsPerMonth = ValNoComma(_employee("PAYFREQUENCY_DIVISOR"))

                _paystub.SssEmployeeShare = employeeSssPerMonth / payPeriodsPerMonth
                _paystub.SssEmployerShare = employerSssPerMonth / payPeriodsPerMonth
            Else
                _paystub.SssEmployeeShare = 0
                _paystub.SssEmployerShare = 0
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
        Dim philHealthCalculation = _settings.GetEnum(
            "PhilHealth.CalculationBasis",
            PhilHealthCalculationBasis.BasicSalary)

        Dim isPhilHealthProrated =
            (philHealthCalculation = PhilHealthCalculationBasis.Earnings) Or
            (philHealthCalculation = PhilHealthCalculationBasis.GrossPay)

        Dim totalContribution = 0D
        If philHealthCalculation = PhilHealthCalculationBasis.BasicSalary Then
            ' If philHealth calculation is based on the basic salary, get it from the salary record
            totalContribution = ConvertToType(Of Decimal)(salary("PhilHealthDeduction"))
        ElseIf isPhilHealthProrated Then
            Dim basisPay = 0D

            If philHealthCalculation = PhilHealthCalculationBasis.Earnings Then
                basisPay = If(_previousPaystub?.TotalEarnings, 0) + _paystub.TotalEarnings
            ElseIf philHealthCalculation = PhilHealthCalculationBasis.GrossPay Then
                basisPay = If(_previousPaystub?.GrossPay, 0) + _paystub.GrossPay
            End If

            totalContribution = ComputePhilHealth(basisPay)
        End If

        Dim halfContribution = AccuMath.Truncate(totalContribution / 2, 2)

        Dim philHealthNoRemainder = _settings.GetBoolean("PhilHealth.Remainder", True)

        Dim remainder = 0D
        ' Account for any division loss by putting the missing value to the employer share
        If philHealthNoRemainder Then
            Dim expectedTotal = halfContribution * 2

            If expectedTotal < totalContribution Then
                remainder = totalContribution - expectedTotal
            End If
        End If

        Dim employeeShare = halfContribution
        Dim employerShare = halfContribution + remainder

        Dim payPeriodsPerMonth = CDec(ValNoComma(_employee("PAYFREQUENCY_DIVISOR")))

        Dim is_weekly As Boolean = Convert.ToBoolean(Convert.ToInt16(_employee("IsWeeklyPaid")))

        If is_weekly Then
            Dim is_deduct_sched_to_thisperiod = If(
                _employee2.IsUnderAgency,
                _payPeriod.PhHWeeklyAgentContribSched,
                _payPeriod.PhHWeeklyContribSched)

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
            Else
                _paystub.PhilHealthEmployeeShare = 0
                _paystub.PhilHealthEmployerShare = 0
            End If
        End If
    End Sub

    Private Function ComputePhilHealth(basis As Decimal) As Decimal
        Dim findPhilHealthBracket =
            Function(amount As Decimal) _philHealthBrackets.FirstOrDefault(
                Function(p) p.SalaryRangeFrom <= amount And
                    p.SalaryRangeTo >= amount)

        Dim philHealthSettings = _settings.GetSublist("PhilHealth")

        Dim minimum = philHealthSettings.GetDecimal("MinimumContribution")
        Dim maximum = philHealthSettings.GetDecimal("MaximumContribution")
        Dim rate = philHealthSettings.GetDecimal("Rate") / 100

        ' Contribution should be bounded by the minimum and maximum
        Dim contribution = {{basis * rate, minimum}.Max(), maximum}.Min()
        ' Truncate to the nearest cent
        contribution = AccuMath.Truncate(contribution, 2)

        Return contribution
    End Function

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
        Dim employerHdmfPerMonth = If(employeeHdmfPerMonth = 0, 0, PagibigEmployerAmount)
        Dim payPeriodsPerMonth = ValNoComma(_employee("PAYFREQUENCY_DIVISOR"))
        Dim is_weekly As Boolean = Convert.ToBoolean(Convert.ToInt16(_employee("IsWeeklyPaid")))

        If is_weekly Then
            Dim is_deduct_sched_to_thisperiod = If(
                _employee2.IsUnderAgency,
                _payPeriod.HDMFWeeklyAgentContribSched,
                _payPeriod.HDMFWeeklyContribSched)

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
            Else
                _paystub.HdmfEmployeeShare = 0
                _paystub.HdmfEmployerShare = 0
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
            _paystub.WithholdingTax = 0
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
            Dim is_deduct_sched_to_thisperiod = If(
                _employee2.IsUnderAgency,
                _payPeriod.WTaxWeeklyAgentContribSched,
                _payPeriod.WTaxWeeklyContribSched)

            If is_deduct_sched_to_thisperiod Then
                _paystub.WithholdingTax = AccuMath.CommercialRound(exemptionAmount + (excessAmount * exemptionInExcessAmount))
            Else
                _paystub.WithholdingTax = 0
            End If
        Else
            _paystub.WithholdingTax = AccuMath.CommercialRound(exemptionAmount + (excessAmount * exemptionInExcessAmount))
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

    Private Function ComputeLoans() As IList(Of LoanTransaction)
        Dim existingLoanTransactions = _allLoanTransactions.
               Where(Function(t) Nullable.Equals(t.EmployeeID, _paystub.EmployeeID))
        Dim newLoanTransactions = New List(Of LoanTransaction)

        If existingLoanTransactions.Count > 0 Then
            _paystub.TotalLoans = existingLoanTransactions.Sum(Function(t) t.Amount)
        Else
            Dim acceptedLoans As String() = {}
            If _isFirstHalf Then
                acceptedLoans = {"Per pay period", "First half"}
            ElseIf _isEndOfMonth Then
                acceptedLoans = {"Per pay period", "End of the month"}
            End If

            Dim loanSchedules = _allLoanSchedules.
                Where(Function(l) Nullable.Equals(l.EmployeeID, _paystub.EmployeeID)).
                Where(Function(l) acceptedLoans.Contains(l.DeductionSchedule)).
                ToList()

            For Each loanSchedule In loanSchedules
                Dim loanTransaction = New LoanTransaction() With {
                    .Created = Date.Now(),
                    .LastUpd = Date.Now(),
                    .OrganizationID = z_OrganizationID,
                    .EmployeeID = _paystub.EmployeeID,
                    .PayPeriodID = _payPeriod.RowID,
                    .LoanSchedule = loanSchedule,
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

        Return newLoanTransactions
    End Function

    Private Sub ComputeThirteenthMonthPay(salaryrow As DataRow)
        If _paystub.ThirteenthMonthPay Is Nothing Then
            _paystub.ThirteenthMonthPay = New ThirteenthMonthPay() With {
                .OrganizationID = z_OrganizationID,
                .CreatedBy = z_User
            }
        Else
            _paystub.ThirteenthMonthPay.LastUpdBy = z_User
        End If

        Dim contractual_employment_statuses = New String() {"Contractual", "SERVICE CONTRACT"}

        Dim basicpay_13month = 0D

        If _employee2.IsDaily Then
            basicpay_13month = If(
                contractual_employment_statuses.Contains(_employee2.EmploymentStatus),
                _timeEntries.
                    Where(Function(t) Not If(t.ShiftSchedule?.IsRestDay, False)).
                    Sum(Function(t) t.BasicDayPay + t.LeavePay),
                _actualtimeentries.
                    Where(Function(t) Not If(t.ShiftSchedule?.IsRestDay, False)).
                    Sum(Function(t) t.BasicDayPay + t.LeavePay))

        ElseIf _employee2.IsMonthly Or _employee2.IsFixed Then
            Dim trueSalary = Convert.ToDecimal(salaryrow("TrueSalary"))
            Dim basicPay = trueSalary / CalendarConstants.SemiMonthlyPayPeriodsPerMonth

            Dim totalDeductions = _actualtimeentries.Sum(Function(t) t.LateDeduction + t.UndertimeDeduction + t.AbsentDeduction)

            basicpay_13month = (basicPay - totalDeductions)
        End If

        _paystub.ThirteenthMonthPay.BasicPay = basicpay_13month
        _paystub.ThirteenthMonthPay.Amount = (basicpay_13month / CalendarConstants.MonthsInAYear)
        _paystub.ThirteenthMonthPay.Paystub = _paystub
    End Sub

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

End Class
