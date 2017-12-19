Imports System.Collections.ObjectModel
Imports System.ComponentModel
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

    Private Class PayStubObject

        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property CreatedBy As Integer?

        Public Property LastUpdBy As Integer?

        Public Property PayPeriodID As Integer?

        Public Property EmployeeID As String

        Public Property PayFromDate As Date

        Public Property PayToDate As Date

        Public Property RegularHours As Decimal

        Public Property RegularPay As Decimal

        Public Property OvertimeHours As Decimal

        Public Property OvertimePay As Decimal

        Public Property NightDiffHours As Decimal

        Public Property NightDiffPay As Decimal

        Public Property NightDiffOvertimeHours As Decimal

        Public Property NightDiffOvertimePay As Decimal

        Public Property RestDayHours As Decimal

        Public Property RestDayPay As Decimal

        Public Property LeavePay As Decimal

        Public Property HolidayPay As Decimal

        Public Property LateHours As Decimal

        Public Property LateDeduction As Decimal

        Public Property UndertimeHours As Decimal

        Public Property UndertimeDeduction As Decimal

        Public Property AbsenceDeduction As Decimal

        Public Property BasicPay As Decimal

        Public Property WorkPay As Decimal

        Public Property TotalAllowance As Decimal

        Public Property TotalBonus As Decimal

        Public Property TotalGrossSalary As Decimal

        Public Property TotalNetSalary As Decimal

        Public Property TotalTaxableSalary As Decimal

        Public Property WithholdingTaxAmount As Decimal

        Public Property TotalEmpWithholdingTax As Decimal

        Public Property TotalEmpSSS As Decimal

        Public Property TotalCompSSS As Decimal

        Public Property TotalEmpPhilHealth As Decimal

        Public Property TotalCompPhilHealth As Decimal

        Public Property TotalEmpHDMF As Decimal

        Public Property TotalCompHDMF As Decimal

        Public Property TotalLoanDeduction As Decimal

    End Class

    Public Property PayrollDateFrom As String

    Public Property PayrollDateTo As String

    Public Property PayPeriodID As String

    Private _employees As DataRow
    Private isEndOfMonth2 As String
    Private _isFirstHalf As Boolean
    Private _isEndOfMonth As Boolean
    Private _allSalaries As DataTable

    Private _allLoanSchedules As ICollection(Of PayrollSys.LoanSchedule)

    Private _allLoanTransactions As ICollection(Of PayrollSys.LoanTransaction)

    Private allOneTimeAllowances As DataTable
    Private allDailyAllowances As DataTable
    Private allSemiMonthlyAllowances As DataTable
    Private allMonthlyAllowances As DataTable
    Private allWeeklyAllowances As DataTable

    Private allNoTaxOneTimeAllowances As DataTable
    Private allNoTaxDailyAllowances As DataTable
    Private allNoTaxWeeklyAllowances As DataTable
    Private allNoTaxSemiMonthlyAllowances As DataTable
    Private allNoTaxMonthlyAllowances As DataTable

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
    Private _previousTimeEntries As DataTable
    Private _VeryFirstPayPeriodIDOfThisYear As Object
    Private _withThirteenthMonthPay As SByte

    Private _withholdingTaxTable As DataTable

    Private _filingStatuses As DataTable

    Private Delegate Sub NotifyMainWindow(ByVal progress_index As Integer)

    Private _notifyMainWindow As NotifyMainWindow

    Private form_caller As Form

    Private payPeriod As DataRow
    Private annualUnusedLeaves As DataTable
    Private unusedLeaveProductID As String
    Private existingUnusedLeaveAdjustments As DataTable

    Private agencyFeeSummary As DataTable

    Private _employee As DataRow

    Private _payStub As PayStubObject

    Private _products As IEnumerable(Of Product)

    Private _philHealthDeductionSchedule As String
    Private _sssDeductionSchedule As String
    Private _hdmfDeductionSchedule As String
    Private _withholdingTaxSchedule As String

    Private _connection As MySqlConnection

    Private numberofweeksthismonth As Integer

    Private _vacationLeaveBalanceAmount As Integer

    Private _sickLeaveBalanceAmount As Integer

    Private _sickLeaveHours As Integer

    Private _paystubs As IEnumerable(Of AccuPay.Entity.Paystub)

    Private _socialSecurityBrackets As IEnumerable(Of SocialSecurityBracket)

    Private _philHealthBrackets As IEnumerable(Of PhilHealthBracket)

    Sub New(employees As DataRow,
            payPeriodHalfNo As String,
            allSalaries As DataTable,
            allLoanSchedules As ICollection(Of PayrollSys.LoanSchedule),
            allLoanTransactions As ICollection(Of PayrollSys.LoanTransaction),
            allDailyAllowances As DataTable,
            allMonthlyAllowances As DataTable,
            allOneTimeAllowances As DataTable,
            allSemiMonthlyAllowances As DataTable,
            allWeeklyAllowances As DataTable,
            allNoTaxDailyAllowances As DataTable,
            allNoTaxMonthlyAllowances As DataTable,
            allNoTaxOneTimeAllowances As DataTable,
            allNoTaxSemiMonthlyAllowances As DataTable,
            allNoTaxWeeklyAllowances As DataTable,
            allFixedTaxableMonthlyAllowances As DataTable,
            allFixedNonTaxableMonthlyAllowances As DataTable,
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
            previousTimeEntries As DataTable,
            VeryFirstPayPeriodIDOfThisYear As Object,
            withThirteenthMonthPay As SByte,
            filingStatuses As DataTable,
            withholdingTaxTable As DataTable,
            products As IEnumerable(Of Product),
            paystubs As IEnumerable(Of AccuPay.Entity.Paystub),
            resources As PayrollResources,
            Optional pay_stub_frm As PayStubForm = Nothing)

        form_caller = pay_stub_frm

        _employees = employees
        isEndOfMonth2 = payPeriodHalfNo
        Me._allSalaries = allSalaries
        _allLoanSchedules = allLoanSchedules
        _allLoanTransactions = allLoanTransactions

        Me.allOneTimeAllowances = allOneTimeAllowances
        Me.allDailyAllowances = allDailyAllowances
        Me.allWeeklyAllowances = allWeeklyAllowances
        Me.allSemiMonthlyAllowances = allSemiMonthlyAllowances
        Me.allMonthlyAllowances = allMonthlyAllowances

        Me.allNoTaxOneTimeAllowances = allNoTaxOneTimeAllowances
        Me.allNoTaxDailyAllowances = allNoTaxDailyAllowances
        Me.allNoTaxWeeklyAllowances = allNoTaxWeeklyAllowances
        Me.allNoTaxSemiMonthlyAllowances = allNoTaxSemiMonthlyAllowances
        Me.allNoTaxMonthlyAllowances = allNoTaxMonthlyAllowances
        Me.allFixedTaxableMonthlyAllowances = allFixedTaxableMonthlyAllowances
        Me.allFixedNonTaxableMonthlyAllowances = allFixedNonTaxableMonthlyAllowances

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
        _previousTimeEntries = previousTimeEntries
        _VeryFirstPayPeriodIDOfThisYear = VeryFirstPayPeriodIDOfThisYear
        _withThirteenthMonthPay = withThirteenthMonthPay

        _filingStatuses = filingStatuses
        _withholdingTaxTable = withholdingTaxTable

        _notifyMainWindow = AddressOf pay_stub_frm.ProgressCounter

        _products = products
        _paystubs = paystubs

        _isFirstHalf = (payPeriodHalfNo = "1")
        _isEndOfMonth = (payPeriodHalfNo = "0")
        _socialSecurityBrackets = resources.SocialSecurityBrackets
        _philHealthBrackets = resources.PhilHealthBrackets
    End Sub

    Sub PayrollGeneration_BackgroundWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        DoProcess()
    End Sub

    Public Sub DoProcess()
        Console.WriteLine("PayrolLGeneration - DoProcess")
        Try
            Dim pause_process_message As String = String.Empty

            'LoadCurrentPayperiod()
            'LoadAnnualUnusedLeaves()
            'LoadProductIDForUnusedLeave()
            'LoadExistingUnusedLeaveAdjustments()
            'ResetUnusedLeaveAdjustments()

            'Dim sel_employee_dattab = _employees.Select("PositionID IS NULL")

            'If sel_employee_dattab.Count > 0 Then
            '    For Each drow In sel_employee_dattab
            '        pause_process_message = "Employee '" & CStr(drow("EmployeeID")) & "' has no position." &
            '            vbNewLine & "Please supply his/her position before proceeding to payroll."
            '        'e.Cancel = True
            '        'If bgworkgenpayroll.CancellationPending Then
            '        '    bgworkgenpayroll.CancelAsync()
            '        'End If
            '    Next
            'End If

            Dim date_to_use = If(CDate(PayrollDateFrom) > CDate(PayrollDateTo), CDate(PayrollDateFrom), CDate(PayrollDateTo))
            Dim dateStr_to_use = Format(date_to_use, "yyyy-MM-dd")
            numberofweeksthismonth = CInt(New MySQLExecuteQuery("SELECT `COUNTTHEWEEKS`('" & dateStr_to_use & "');").Result)

            GeneratePayStub(_employees)
        Catch ex As Exception
            Console.WriteLine(getErrExcptn(ex, "PayrollGeneration - Error"))
            logger.Error("DoProcess", ex)
        Finally
            'employee_dattab.Dispose()
            '_allSalaries.Dispose()
            'emp_loans.Dispose()
            'emp_bonus.Dispose()
            'allDailyAllowances.Dispose()
            'allMonthlyAllowances.Dispose()
            'allOneTimeAllowances.Dispose()
            'allSemiMonthlyAllowances.Dispose()
            'allWeeklyAllowances.Dispose()
            'allNoTaxDailyAllowances.Dispose()
            'allNoTaxMonthlyAllowances.Dispose()
            'allNoTaxOneTimeAllowances.Dispose()
            'allNoTaxSemiMonthlyAllowances.Dispose()
            'allNoTaxWeeklyAllowances.Dispose()
            'allDailyBonuses.Dispose()
            'allMonthlyBonuses.Dispose()
            'allOneTimeBonuses.Dispose()
            'allSemiMonthlyBonuses.Dispose()
            'allWeeklyBonuses.Dispose()
            'allNoTaxDailyBonuses.Dispose()
            'allNoTaxMonthlyBonuses.Dispose()
            'allNoTaxOneTimeBonuses.Dispose()
            'allNoTaxSemiMonthlyBonuses.Dispose()
            'allNoTaxWeeklyBonuses.Dispose()
            'numofdaypresent.Dispose()
            'allTimeEntries.Dispose()
            'dtemployeefirsttimesalary.Dispose()
            'previousTimeEntries.Dispose()
            'withholdingTaxTable.Dispose()
            'filingStatus.Dispose()
            'fixedNonTaxableMonthlyAllowances.Dispose()
            'fixedTaxableMonthlyAllowances.Dispose()
        End Try
    End Sub

    Private Sub GeneratePayStub(employee As DataRow)
        Dim transaction As MySqlTransaction = Nothing
        Dim newLoanTransactions = New Collection(Of PayrollSys.LoanTransaction)

        Try
            _payStub = New PayStubObject()

            Dim totalVacationDaysLeft = 0D
            Dim grossIncomeLastPayPeriod = 0D

            _employee = employee

            _sssDeductionSchedule = employee("SSSDeductSched").ToString
            _philHealthDeductionSchedule = employee("PhHealthDeductSched").ToString
            _hdmfDeductionSchedule = employee("HDMFDeductSched").ToString
            _withholdingTaxSchedule = employee("WTaxDeductSched").ToString

            _payStub.EmployeeID = Trim(CStr(employee("RowID")))

            Dim salary = _allSalaries.Select($"EmployeeID = '{_payStub.EmployeeID}'").FirstOrDefault()

            Dim loanTransactions = _allLoanTransactions.Where(Function(t) t.EmployeeID = _payStub.EmployeeID)

            If loanTransactions.Count > 0 Then
                _payStub.TotalLoanDeduction = loanTransactions.Aggregate(0D, Function(total, t) total + t.DeductionAmount)
            Else
                Dim acceptedLoans As String() = {}
                If _isFirstHalf Then
                    acceptedLoans = {"Per pay period", "First half"}
                ElseIf _isEndOfMonth Then
                    acceptedLoans = {"Per pay period", "End of the month"}
                End If

                Dim loanSchedules = _allLoanSchedules.
                    Where(Function(l) l.EmployeeID = _payStub.EmployeeID).
                    Where(Function(l) acceptedLoans.Contains(l.DeductionSchedule)).
                    ToList()

                For Each loanSchedule In loanSchedules
                    Dim loanTransaction = New PayrollSys.LoanTransaction() With {
                        .Created = Date.Now(),
                        .LastUpd = Date.Now(),
                        .OrganizationID = z_OrganizationID,
                        .EmployeeID = _payStub.EmployeeID,
                        .PayPeriodID = PayPeriodID,
                        .LoanScheduleID = loanSchedule.RowID,
                        .LoanPayPeriodLeft = loanSchedule.LoanPayPeriodLeft - 1
                    }

                    If loanSchedule.DeductionAmount > loanSchedule.TotalBalanceLeft Then
                        loanTransaction.DeductionAmount = loanSchedule.TotalBalanceLeft
                    Else
                        loanTransaction.DeductionAmount = loanSchedule.DeductionAmount
                    End If

                    loanTransaction.TotalBalanceLeft = loanSchedule.TotalBalanceLeft - loanTransaction.DeductionAmount

                    newLoanTransactions.Add(loanTransaction)
                Next

                _payStub.TotalLoanDeduction = newLoanTransactions.Aggregate(0D, Function(total, x) x.DeductionAmount + total)
            End If

            totalVacationDaysLeft = 0D

            CalculateAllowances()
            CalculateBonuses()

            Dim governmentContributions = 0D
            Dim timeEntrySummary = _allTimeEntries.Select($"EmployeeID = '{_payStub.EmployeeID}'").FirstOrDefault()

            If timeEntrySummary IsNot Nothing Then

                grossIncomeLastPayPeriod = 0D
                totalVacationDaysLeft = 0D

                If salary IsNot Nothing Then

                    Dim skipGovernmentDeductions = CStr(employee("IsFirstTimeSalary")) = "1"
                    Dim basicPay = ValNoComma(salary("BasicPay"))

                    _payStub.RegularHours = ValNoComma(timeEntrySummary("RegularHoursWorked"))
                    _payStub.RegularPay = ValNoComma(timeEntrySummary("RegularHoursAmount"))

                    _payStub.OvertimeHours = ValNoComma(timeEntrySummary("OvertimeHoursWorked"))
                    _payStub.OvertimePay = ValNoComma(timeEntrySummary("OvertimeHoursAmount"))

                    _payStub.NightDiffHours = ValNoComma(timeEntrySummary("NightDifferentialHours"))
                    _payStub.NightDiffPay = ValNoComma(timeEntrySummary("NightDiffHoursAmount"))

                    _payStub.NightDiffOvertimeHours = ValNoComma(timeEntrySummary("NightDifferentialOTHours"))
                    _payStub.NightDiffOvertimePay = ValNoComma(timeEntrySummary("NightDiffOTHoursAmount"))

                    _payStub.RestDayHours = ValNoComma(timeEntrySummary("RestDayHours"))
                    _payStub.RestDayPay = ValNoComma(timeEntrySummary("RestDayAmount"))

                    _payStub.LeavePay = ValNoComma(timeEntrySummary("Leavepayment"))
                    _payStub.HolidayPay = ValNoComma(timeEntrySummary("HolidayPayAmount"))

                    _payStub.LateHours = ValNoComma(timeEntrySummary("HoursLate"))
                    _payStub.LateDeduction = ValNoComma(timeEntrySummary("HoursLateAmount"))

                    _payStub.UndertimeHours = ValNoComma(timeEntrySummary("UndertimeHours"))
                    _payStub.UndertimeDeduction = ValNoComma(timeEntrySummary("UndertimeHoursAmount"))

                    _vacationLeaveBalanceAmount = _employee("LeaveBalance")
                    _sickLeaveBalanceAmount = _employee("SickLeaveBalance")

                    _payStub.AbsenceDeduction = ValNoComma(timeEntrySummary("Absent"))

                    Dim sel_dtemployeefirsttimesalary = _employeeFirstTimeSalary.Select($"EmployeeID = '{_payStub.EmployeeID}'")
                    Dim employmentType = StrConv(employee("EmployeeType").ToString, VbStrConv.ProperCase)

                    If employmentType = SalaryType.Fixed Then

                        _payStub.WorkPay = basicPay + (_payStub.HolidayPay + _payStub.OvertimePay + _payStub.NightDiffPay + _payStub.NightDiffOvertimePay)

                        Dim previousOvertimePay = ValNoComma(_previousTimeEntries.Compute("SUM(OvertimeHoursAmount)", $"EmployeeID = '{_payStub.EmployeeID}'"))
                        Dim previousNightDiffPay = ValNoComma(_previousTimeEntries.Compute("SUM(NightDiffHoursAmount)", $"EmployeeID = '{_payStub.EmployeeID}'"))
                        Dim previousNightDiffOTPay = ValNoComma(_previousTimeEntries.Compute("SUM(NightDiffOTHoursAmount)", $"EmployeeID = '{_payStub.EmployeeID}'"))

                        grossIncomeLastPayPeriod = basicPay + previousOvertimePay + previousNightDiffPay + previousNightDiffOTPay

                        _payStub.TotalTaxableSalary = basicPay

                    ElseIf employmentType = SalaryType.Monthly Then

                        If skipGovernmentDeductions Then
                            _payStub.WorkPay = ValNoComma(timeEntrySummary("TotalDayPay"))

                            grossIncomeLastPayPeriod = ValNoComma(_previousTimeEntries.Compute("SUM(TotalDayPay)", $"EmployeeID = '{_payStub.EmployeeID}'"))
                        Else
                            Dim totalDeduction = _payStub.LateDeduction + _payStub.UndertimeDeduction + _payStub.AbsenceDeduction
                            Dim extraPay = _payStub.OvertimePay + _payStub.NightDiffPay + _payStub.NightDiffOvertimePay + _payStub.RestDayPay + _payStub.HolidayPay

                            _payStub.WorkPay = (basicPay + extraPay) - totalDeduction
                            _payStub.TotalTaxableSalary = basicPay

                            If _previousTimeEntries.Select($"EmployeeID = '{_payStub.EmployeeID}'").Count > 0 Then
                                grossIncomeLastPayPeriod = basicPay
                            End If

                            Dim previousLateDeduction = ValNoComma(_previousTimeEntries.Compute("SUM(HoursLateAmount)", $"EmployeeID = '{_payStub.EmployeeID}'"))
                            Dim previousUndertimeDeduction = ValNoComma(_previousTimeEntries.Compute("SUM(UndertimeHoursAmount)", $"EmployeeID = '{_payStub.EmployeeID}'"))
                            Dim previousAbsenceDeduction = ValNoComma(_previousTimeEntries.Compute("SUM(Absent)", $"EmployeeID = '{_payStub.EmployeeID}'"))
                            Dim totalPreviousDeduction = previousLateDeduction + previousUndertimeDeduction + previousAbsenceDeduction

                            grossIncomeLastPayPeriod -= totalPreviousDeduction
                            grossIncomeLastPayPeriod += ValNoComma(_previousTimeEntries.Compute("MIN(emtAmount)", $"EmployeeID = '{_payStub.EmployeeID}'"))
                        End If

                    ElseIf employmentType = SalaryType.Daily Then
                        _payStub.WorkPay = ValNoComma(timeEntrySummary("TotalDayPay"))
                        grossIncomeLastPayPeriod = ValNoComma(_previousTimeEntries.Compute("SUM(TotalDayPay)", $"EmployeeID = '{_payStub.EmployeeID}'"))
                    End If

                    CalculateSss(salary)
                    CalculatePhilHealth(salary)
                    CalculateHdmf(salary)

                    governmentContributions = _payStub.TotalEmpSSS + _payStub.TotalEmpPhilHealth + _payStub.TotalEmpHDMF

                    If IsWithholdingTaxPaidOnFirstHalf() Or IsWithholdingTaxPaidOnEndOfTheMonth() Then
                        _payStub.TotalTaxableSalary += _payStub.TotalTaxableSalary - governmentContributions
                    ElseIf IsWithholdingTaxPaidPerPayPeriod() Then
                        _payStub.TotalTaxableSalary += -governmentContributions
                    End If

                    CalculateWithholdingTax()
                End If
            End If

            _payStub.TotalGrossSalary = _payStub.WorkPay + _payStub.TotalBonus + _payStub.TotalAllowance
            _payStub.TotalNetSalary = _payStub.TotalGrossSalary - (governmentContributions + _payStub.TotalLoanDeduction + _payStub.WithholdingTaxAmount)

            Dim vacationLeaveProduct = _products.Where(Function(p) p.PartNo = "Vacation leave").FirstOrDefault()
            Dim sickLeaveProduct = _products.Where(Function(p) p.PartNo = "Sick leave").FirstOrDefault()

            _connection = New MySqlConnection(mysql_conn_text)
            Dim command = New MySqlCommand("SavePayStub", _connection, transaction)

            If _connection.State = ConnectionState.Closed Then
                _connection.Open()
            End If

            transaction = _connection.BeginTransaction()
            command.CommandTimeout = 5000
            command.CommandType = CommandType.StoredProcedure

            With command.Parameters
                .Clear()
                .AddWithValue("pstub_RowID", DBNull.Value)
                .AddWithValue("pstub_OrganizationID", orgztnID)
                .AddWithValue("pstub_CreatedBy", z_User)
                .AddWithValue("pstub_LastUpdBy", z_User)
                .AddWithValue("pstub_PayPeriodID", PayPeriodID)
                .AddWithValue("pstub_EmployeeID", _payStub.EmployeeID)
                .AddWithValue("pstub_TimeEntryID", DBNull.Value)
                .AddWithValue("pstub_PayFromDate", PayrollDateFrom)
                .AddWithValue("pstub_PayToDate", PayrollDateTo)
                .AddWithValue("$RegularHours", _payStub.RegularHours)
                .AddWithValue("$RegularPay", _payStub.RegularPay)
                .AddWithValue("$OvertimeHours", _payStub.OvertimeHours)
                .AddWithValue("$OvertimePay", _payStub.OvertimePay)
                .AddWithValue("$NightDiffHours", _payStub.NightDiffHours)
                .AddWithValue("$NightDiffPay", _payStub.NightDiffPay)
                .AddWithValue("$NightDiffOvertimeHours", _payStub.NightDiffOvertimeHours)
                .AddWithValue("$NightDiffOvertimePay", _payStub.NightDiffOvertimePay)
                .AddWithValue("$RestDayHours", _payStub.RestDayHours)
                .AddWithValue("$RestDayPay", _payStub.RestDayPay)
                .AddWithValue("$LeavePay", _payStub.LeavePay)
                .AddWithValue("$HolidayPay", _payStub.HolidayPay)
                .AddWithValue("$LateHours", _payStub.LateHours)
                .AddWithValue("$LateDeduction", _payStub.LateDeduction)
                .AddWithValue("$UndertimeHours", _payStub.UndertimeHours)
                .AddWithValue("$UndertimeDeduction", _payStub.UndertimeDeduction)
                .AddWithValue("$AbsenceDeduction", _payStub.AbsenceDeduction)
                .AddWithValue("$WorkPay", _payStub.WorkPay)
                .AddWithValue("pstub_TotalAllowance", _payStub.TotalAllowance)
                .AddWithValue("pstub_TotalBonus", _payStub.TotalBonus)
                .AddWithValue("pstub_TotalGrossSalary", _payStub.TotalGrossSalary)
                .AddWithValue("pstub_TotalNetSalary", _payStub.TotalNetSalary)
                .AddWithValue("pstub_TotalTaxableSalary", _payStub.TotalTaxableSalary)
                .AddWithValue("pstub_TotalEmpWithholdingTax", _payStub.WithholdingTaxAmount)
                .AddWithValue("pstub_TotalEmpSSS", _payStub.TotalEmpSSS)
                .AddWithValue("pstub_TotalCompSSS", _payStub.TotalCompSSS)
                .AddWithValue("pstub_TotalEmpPhilhealth", _payStub.TotalEmpPhilHealth)
                .AddWithValue("pstub_TotalCompPhilhealth", _payStub.TotalCompPhilHealth)
                .AddWithValue("pstub_TotalEmpHDMF", _payStub.TotalEmpHDMF)
                .AddWithValue("pstub_TotalCompHDMF", _payStub.TotalCompHDMF)
                .AddWithValue("pstub_TotalVacationDaysLeft", totalVacationDaysLeft)
                .AddWithValue("pstub_TotalLoans", _payStub.TotalLoanDeduction)
                .Add("NewID", MySqlDbType.Int32)
                .Item("NewID").Direction = ParameterDirection.ReturnValue
            End With

            command.ExecuteNonQuery()
            _payStub.RowID = command.Parameters("NewID").Value

            transaction.Commit()
            command.Dispose()
            _connection.Close()

            Using context = New PayrollContext()
                Dim vacationLeaveBalance =
                    (From p In context.PaystubItems
                     Where p.Product.PartNo = "Vacation leave" And
                        p.PayStubID = _payStub.RowID).
                    FirstOrDefault()

                If vacationLeaveBalance Is Nothing Then
                    vacationLeaveBalance = New PaystubItem() With {
                        .OrganizationID = z_OrganizationID,
                        .Created = Date.Now,
                        .ProductID = vacationLeaveProduct?.RowID,
                        .PayAmount = _vacationLeaveBalanceAmount,
                        .PayStubID = _payStub.RowID
                    }

                    context.PaystubItems.Add(vacationLeaveBalance)
                End If

                Dim sickLeaveBalance =
                    (From p In context.PaystubItems
                     Where p.Product.PartNo = "Sick leave" And
                        p.PayStubID = _payStub.RowID).
                    FirstOrDefault()

                If sickLeaveBalance Is Nothing Then
                    sickLeaveBalance = New PaystubItem() With {
                        .OrganizationID = z_OrganizationID,
                        .Created = Date.Now,
                        .ProductID = sickLeaveProduct?.RowID,
                        .PayAmount = _sickLeaveBalanceAmount,
                        .PayStubID = _payStub.RowID
                    }

                    context.PaystubItems.Add(sickLeaveBalance)
                End If

                context.LoanTransactions.AddRange(newLoanTransactions)

                context.SaveChanges()
            End Using

            form_caller.BeginInvoke(_notifyMainWindow, 1)
        Catch ex As Exception
            If transaction IsNot Nothing Then
                transaction.Rollback()
            End If

            Throw New Exception($"Failure to generate paystub for employee {_payStub.EmployeeID}", ex)
        Finally
            If _connection IsNot Nothing Then
                If _connection.State = ConnectionState.Open Then
                    _connection.Close()
                    _connection.Dispose()
                End If
            End If
        End Try
    End Sub

    Private Sub CalculateAllowances()
        Dim oneTimeAllowances = allOneTimeAllowances.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim dailyAllowances = allDailyAllowances.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim weeklyAllowances = allWeeklyAllowances.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim semiMonthlyAllowances = allSemiMonthlyAllowances.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim monthlyAllowances = allMonthlyAllowances.Select($"EmployeeID = '{_payStub.EmployeeID}'")

        Dim totalOneTimeAllowances = 0D
        For Each drowonceallow In oneTimeAllowances
            totalOneTimeAllowances += ValNoComma(drowonceallow("TotalAllowanceAmount"))
        Next

        Dim totalDailyAllowances = ValNoComma(allDailyAllowances.Compute("SUM(TotalAllowanceAmount)", $"EmployeeID = '{_payStub.EmployeeID}'"))

        Dim totalWeeklyAllowances = 0D
        For Each weeklyAllowance In weeklyAllowances
            totalWeeklyAllowances = ValNoComma(weeklyAllowance("TotalAllowanceAmount"))
        Next
        Dim totalSemiMonthlyAllowances = ValNoComma(allSemiMonthlyAllowances.Compute("SUM(TotalAllowanceAmount)", $"EmployeeID = '{_payStub.EmployeeID}'"))
        Dim totalMonthlyAllowances = ValNoComma(allMonthlyAllowances.Compute("SUM(TotalAllowanceAmount)", $"EmployeeID = '{_payStub.EmployeeID}'"))
        Dim totalFixedTaxableMonthlyAllowance = ValNoComma(allFixedTaxableMonthlyAllowances.Compute("SUM(AllowanceAmount)", $"EmployeeID = '{_payStub.EmployeeID}'"))

        Dim totalTaxableAllowance = (
            totalOneTimeAllowances +
            totalDailyAllowances +
            totalWeeklyAllowances +
            totalSemiMonthlyAllowances +
            totalMonthlyAllowances +
            totalFixedTaxableMonthlyAllowance
        )

        Dim totalOneTimeNoTaxAllowances = ValNoComma(allNoTaxOneTimeAllowances.Compute("SUM(TotalAllowanceAmount)", $"EmployeeID = '{_payStub.EmployeeID}'"))
        Dim totalDailyNoTaxAllowances = ValNoComma(allNoTaxDailyAllowances.Compute("SUM(TotalAllowanceAmount)", $"EmployeeID = '{_payStub.EmployeeID}'"))
        Dim totalWeeklyNoTaxAllowances = ValNoComma(allNoTaxWeeklyAllowances.Compute("SUM(TotalAllowanceAmount)", $"EmployeeID = '{_payStub.EmployeeID}'"))
        Dim totalSemiMonthlyNoTaxAllowances = ValNoComma(allNoTaxSemiMonthlyAllowances.Compute("SUM(TotalAllowanceAmount)", $"EmployeeID = '{_payStub.EmployeeID}'"))
        Dim totalMonthlyNoTaxAllowances = ValNoComma(allNoTaxMonthlyAllowances.Compute("SUM(TotalAllowanceAmount)", $"EmployeeID = '{_payStub.EmployeeID}'"))
        Dim totalFixedMonthlyNoTaxAllowances = ValNoComma(allFixedNonTaxableMonthlyAllowances.Compute("SUM(AllowanceAmount)", $"EmployeeID = '{_payStub.EmployeeID}'"))

        Dim totalNoTaxAllowance = (
            totalOneTimeNoTaxAllowances +
            totalDailyNoTaxAllowances +
            totalWeeklyNoTaxAllowances +
            totalSemiMonthlyNoTaxAllowances +
            totalMonthlyNoTaxAllowances +
            totalFixedMonthlyNoTaxAllowances
        )

        _payStub.TotalAllowance = totalTaxableAllowance + totalNoTaxAllowance
    End Sub

    Private Sub CalculateBonuses()
        Dim oneTimeBonuses = allOneTimeBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim dailyBonuses = allDailyBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim weeklyBonuses = allWeeklyBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim semiMonthlyBonuses = allSemiMonthlyBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim monthlyBonuses = allMonthlyBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")

        Dim noTaxOneBonuses = allNoTaxOneTimeBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim noTaxDailyBonuses = allNoTaxDailyBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim noTaxWeeklyBonuses = allNoTaxWeeklyBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim noTaxSemiMonthlyBonuses = allNoTaxSemiMonthlyBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim noTaxMonthlyBonuses = allNoTaxMonthlyBonuses.Select($"EmployeeID = '{_payStub.EmployeeID}'")
        Dim workDaysPerYear = CInt(_employee("WorkDaysPerYear"))
        Dim divisorMonthlys = If(
                CInt(_employee("PayFrequencyID")) = 1,
                2,
                If(
                    CInt(_employee("PayFrequencyID")) = 2,
                    1,
                    If(
                        CInt(_employee("PayFrequencyID")) = 3,
                        CInt(New MySQLExecuteQuery("SELECT COUNT(RowID) FROM employeetimeentry WHERE EmployeeID='" & _payStub.EmployeeID & "' AND Date BETWEEN '" & PayrollDateFrom & "' AND '" & PayrollDateTo & "' AND IFNULL(TotalDayPay,0)!=0 AND OrganizationID='" & orgztnID & "';").Result),
                        numberofweeksthismonth
                    )
                )
            )

        Dim totalTaxableBonus = 0D

        Dim valdaynotax_bon = 0D
        For Each drowdaybon In noTaxDailyBonuses
            valdaynotax_bon = ValNoComma(drowdaybon("BonusAmount"))

            If _employee("EmployeeType").ToString = "Fixed" Then
                valdaynotax_bon = valdaynotax_bon * workDaysPerYear 'numofweekends
            Else
                Dim daymultiplier = _numOfDayPresent.Select($"EmployeeID = '{_payStub.EmployeeID}'")
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
            valsemimnotax_bon +
            (valmonthnotax_bon / divisorMonthlys)
        )

        _payStub.TotalBonus = totalTaxableBonus + totalNoTaxBonus
    End Sub

    Private Sub CalculateSss(salary As DataRow)
        Dim employeeSssPerMonth = ValNoComma(salary("EmployeeContributionAmount"))
        Dim employerSssPerMonth = ValNoComma(salary("EmployerContributionAmount"))
        Dim payPeriodsPerMonth = ValNoComma(_employee("PAYFREQUENCY_DIVISOR"))

        If False Then
            Dim socialSecurityBracket = _socialSecurityBrackets.FirstOrDefault(
                Function(s)
                    Return s.RangeFromAmount <= _payStub.TotalGrossSalary And
                        s.RangeToAmount >= _payStub.TotalGrossSalary
                End Function
            )

            employeeSssPerMonth = socialSecurityBracket.EmployeeContributionAmount
            employerSssPerMonth = socialSecurityBracket.EmployerContributionAmount
        End If

        If IsSssPaidOnFirstHalf() Or IsSssPaidOnEndOfTheMonth() Then
            _payStub.TotalEmpSSS = employeeSssPerMonth
            _payStub.TotalCompSSS = employerSssPerMonth
        ElseIf IsSssPaidPerPayPeriod() Then
            _payStub.TotalEmpSSS = employeeSssPerMonth / payPeriodsPerMonth
            _payStub.TotalCompSSS = employerSssPerMonth / payPeriodsPerMonth
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

        If False Then
            Dim philHealthBracket = _philHealthBrackets.FirstOrDefault(
                Function(p)
                    Return p.SalaryRangeFrom <= _payStub.TotalGrossSalary And
                        p.SalaryRangeTo >= _payStub.TotalGrossSalary
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

        If IsPhilHealthPaidOnFirstHalf() Or IsPhilHealthPaidOnEndOfTheMonth() Then
            _payStub.TotalEmpPhilHealth = employeeShare
            _payStub.TotalCompPhilHealth = employerShare
        ElseIf IsPhilHealthPaidPerPayPeriod() Then
            _payStub.TotalEmpPhilHealth = employeeShare / payPeriodsPerMonth
            _payStub.TotalCompPhilHealth = employerShare / payPeriodsPerMonth
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

        If IsHdmfPaidOnFirstHalf() Or IsHdmfPaidOnEndOfTheMonth() Then
            _payStub.TotalEmpHDMF = employeeHdmfPerMonth
            _payStub.TotalCompHDMF = employerHdmfPerMonth
        ElseIf IsHdmfPaidPerPayPeriod() Then
            _payStub.TotalEmpHDMF = employeeHdmfPerMonth / payPeriodsPerMonth
            _payStub.TotalCompHDMF = employerHdmfPerMonth / payPeriodsPerMonth
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
            _payStub.TotalTaxableSalary = 0D
        End If

        If _payStub.TotalTaxableSalary > 0D Then
            Dim maritalStatus = _employee("MaritalStatus").ToString
            Dim noOfDependents = _employee("NoOfDependents").ToString

            Dim filingStatus = _filingStatuses _
                .Select($"
                    MaritalStatus = '{maritalStatus}' AND
                    Dependent <= '{noOfDependents}'
                ") _
                .OrderByDescending(Function(f) CInt(f("Dependent"))) _
                .FirstOrDefault()

            Dim filingStatusID = 1
            If filingStatus IsNot Nothing Then
                filingStatusID = CInt(filingStatus("RowID"))
            End If

            Dim withholdingTaxBracket = _withholdingTaxTable.Select($"
                    FilingStatusID = '{filingStatusID}' AND
                    PayFrequencyID = '{payFrequencyID}' AND
                    TaxableIncomeFromAmount <= {_payStub.TotalTaxableSalary} AND {_payStub.TotalTaxableSalary} <= TaxableIncomeToAmount
                ") _
                .FirstOrDefault()

            If withholdingTaxBracket IsNot Nothing Then
                Dim exemptionAmount = ValNoComma(withholdingTaxBracket("ExemptionAmount"))
                Dim taxableIncomeFromAmount = ValNoComma(withholdingTaxBracket("TaxableIncomeFromAmount"))
                Dim exemptionInExcessAmount = ValNoComma(withholdingTaxBracket("ExemptionInExcessAmount"))

                _payStub.WithholdingTaxAmount = exemptionAmount + ((_payStub.TotalTaxableSalary - taxableIncomeFromAmount) * exemptionInExcessAmount)
            End If
        End If
    End Sub

    Private Function IsWithholdingTaxPaidOnFirstHalf() As Boolean
        Return _isFirstHalf And (_withholdingTaxSchedule = ContributionSchedule.FirstHalf)
    End Function

    Private Function IsWithholdingTaxPaidOnEndOfTheMonth() As Boolean
        Return _isEndOfMonth And (_withholdingTaxSchedule = ContributionSchedule.EndOfTheMonth)
    End Function

    Private Function IsWithholdingTaxPaidPerPayPeriod() As Boolean
        Return _withholdingTaxSchedule = ContributionSchedule.PerPayPeriod
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

        Dim basicPay = ValNoComma(salaryAgreement("BasicPay"))
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
