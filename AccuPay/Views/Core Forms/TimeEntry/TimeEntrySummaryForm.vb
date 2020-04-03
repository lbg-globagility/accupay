Option Strict On

Imports System.Collections.ObjectModel
Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Data
Imports AccuPay.Entity
Imports AccuPay.Helpers
Imports AccuPay.Repository
Imports AccuPay.Utilities
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Utils
Imports log4net
Imports Microsoft.EntityFrameworkCore
Imports MySql.Data.MySqlClient

Public Class TimeEntrySummaryForm

    Private Const Clock24HourFormat As String = "HH:mm"
    Private Const Clock12HourFormat As String = "hh:mm tt"

    Private Shared _logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Shared HoursPerDay As TimeSpan = New TimeSpan(24, 0, 0)

    Private Shared OptionalColumns As List(Of String) = New List(Of String) From {
        "SpecialHolidayHours",
        "SpecialHolidayPay"
    }

    Private _selectedYear As Integer

    Private _weeklyPayPeriods As ICollection(Of PayPeriod)

    Private _semiMonthlyPayPeriods As ICollection(Of PayPeriod)

    Private _payPeriods As ICollection(Of PayPeriod)

    Private _employees As ICollection(Of Employee)

    Private _selectedEmployee As Employee

    Private _breakTimeBrackets As List(Of BreakTimeBracket)

    Private _selectedPayPeriod As PayPeriod

    Private _isActual As Boolean = False

    Private _isAmPm As Boolean = False

    Private _currentTimeEntryDate As Date

    Private _employeeRepository As EmployeeRepository

    Private _calculateBreakTimeLateHours As Boolean

    Private _useNewShift As Boolean

    Private _hideMoneyColumns As Boolean

    Private _formHasLoaded As Boolean = False

    Private Async Sub TimeEntrySummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        _formHasLoaded = True

        employeesDataGridView.AutoGenerateColumns = False
        timeEntriesDataGridView.AutoGenerateColumns = False

        ' Default selected year is the current year
        _selectedYear = Date.Today.Year

        _employeeRepository = New EmployeeRepository

        ' Hide `delete` and `regenerate` menu buttons by default
        tsBtnDeleteTimeEntry.Visible = False
        regenerateTimeEntryButton.Visible = False

        Dim policy As New PolicyHelper
        _calculateBreakTimeLateHours = policy.ComputeBreakTimeLate
        _useNewShift = policy.UseShiftSchedule

        Await LoadEmployees()
        Await LoadPayPeriods()

        _breakTimeBrackets = New List(Of BreakTimeBracket)
        If _calculateBreakTimeLateHours Then
            _breakTimeBrackets = GetBreakTimeBrackets()
        End If

        UpdateFormBaseOnPolicy()

        LoadYears()
    End Sub

    Private Sub UpdateFormBaseOnPolicy()
        Using context As New PayrollContext

            Dim settings = New ListOfValueCollection(context.ListOfValues.ToList())

            Dim showActual = (settings.GetBoolean("Policy.HideActual", True) = True)

            actualButton.Visible = showActual

            CheckIfMoneyColumnsAreGoingToBeHidden(settings)

        End Using
    End Sub

    Private Sub CheckIfMoneyColumnsAreGoingToBeHidden(settings As ListOfValueCollection)
        Using context As New PayrollContext

            Dim user = context.Users.FirstOrDefault(Function(u) u.RowID.Value = z_User)

            If user Is Nothing Then

                MessageBoxHelper.ErrorMessage("Cannot read user data. Please log out and try to log in again.")
            End If

            If settings.GetBoolean("User Policy.UseUserLevel", False) = False Then

                Return

            End If

            _hideMoneyColumns = user.UserLevel <> UserLevel.One AndAlso
                                user.UserLevel <> UserLevel.Two AndAlso
                                user.UserLevel <> UserLevel.Three

        End Using
    End Sub

    Private Function GetBreakTimeBrackets() As List(Of BreakTimeBracket)

        Using context As New PayrollContext
            Return context.BreakTimeBrackets.
                            Include(Function(b) b.Division).
                            Where(Function(b) Nullable.Equals(b.Division.OrganizationID, z_OrganizationID)).
                            ToList()
        End Using

    End Function

    Private Async Function LoadEmployees() As Task
        _employees = Await GetEmployeesWithPosition()
        employeesDataGridView.DataSource = _employees
    End Function

    Private Async Function GetEmployeesWithPosition() As Task(Of ICollection(Of Employee))

        Dim unsortedList = Await _employeeRepository.GetAllWithPositionAsync()
        Dim list = unsortedList.
            OrderBy(Function(e) e.LastName).
            ToList()
        Return CType(list, ICollection(Of Employee))

    End Function

    Public Async Function LoadPayPeriods() As Task

        If _formHasLoaded = False Then Return

        'tsBtnDeleteTimeEntry.Visible = False
        'regenerateTimeEntryButton.Visible = False

        Dim numOfRows = 2

        _payPeriods = Await GetPayPeriods(z_OrganizationID, _selectedYear, 1)
        payPeriodsDataGridView.Rows.Add(numOfRows)

        Dim monthRowCounters(11) As Integer

        For Each payperiod In Me._payPeriods
            Dim monthNo = payperiod.Month

            Dim columnIndex = monthNo - 1
            Dim rowIndex = monthRowCounters(columnIndex)

            Dim payFromDate = payperiod.PayFromDate.ToString("dd MMM")
            Dim payToDate = payperiod.PayToDate.ToString("dd MMM")
            Dim label = payFromDate + " - " + payToDate

            Dim currentCell = payPeriodsDataGridView.Rows(rowIndex).Cells(columnIndex)

            currentCell.Value = payperiod

            If payperiod.IsClosed Then

                currentCell.Style.SelectionBackColor = SystemColors.Highlight
                currentCell.Style.BackColor = Color.White
                currentCell.Style.ForeColor = Color.Black

            ElseIf payperiod.Status = PayPeriodStatusData.PayPeriodStatus.Processing Then

                currentCell.Style.SelectionBackColor = Color.Green
                currentCell.Style.BackColor = Color.Yellow
                currentCell.Style.ForeColor = Color.Black
            Else

                currentCell.Style.SelectionBackColor = SystemColors.Highlight
                currentCell.Style.BackColor = Color.White
                currentCell.Style.ForeColor = Color.Gray

            End If

            rowIndex += 1
            monthRowCounters(monthNo - 1) = rowIndex

        Next

        If _selectedPayPeriod Is Nothing Then
            Dim dateToday = DateTime.Today

            Dim currentlyWorkedOnPayPeriod = Await PayrollTools.GetCurrentlyWorkedOnPayPeriodByCurrentYear(New List(Of IPayPeriod)(_payPeriods))

            _selectedPayPeriod = _payPeriods.FirstOrDefault(Function(p) Nullable.Equals(p.RowID, currentlyWorkedOnPayPeriod.RowID))

            Dim rowIdx = (_selectedPayPeriod.OrdinalValue - 1) Mod numOfRows
            Dim payPeriodCell = payPeriodsDataGridView.Rows(rowIdx).Cells(_selectedPayPeriod.Month - 1)
            payPeriodsDataGridView.CurrentCell = payPeriodCell
        End If

        If _selectedPayPeriod IsNot payPeriodsDataGridView.CurrentCell Then
            _selectedPayPeriod = DirectCast(payPeriodsDataGridView.CurrentCell.Value, PayPeriod)
            LoadTimeEntries()
        End If

        Dim isPayperiodProcessing = _selectedPayPeriod.Status = PayPeriodStatusData.PayPeriodStatus.Processing

        tsBtnDeleteTimeEntry.Visible = isPayperiodProcessing
        regenerateTimeEntryButton.Visible = isPayperiodProcessing
    End Function

    Private Async Function GetPayPeriods(organizationID As Integer,
                                         year As Integer,
                                         salaryType As Integer) As Task(Of ICollection(Of PayPeriod))
        Dim sql = <![CDATA[
            SELECT RowID, PayFromDate, PayToDate, Year, Month, OrdinalValue, IsClosed
            FROM payperiod
            WHERE payperiod.OrganizationID = @OrganizationID
                AND payperiod.Year = @Year
                AND payperiod.TotalGrossSalary = @SalaryType;
        ]]>.Value

        Dim payPeriods = New Collection(Of PayPeriod)

        Dim payPeriodsWithPaystubCount = PayPeriodStatusData.GetPeriodsWithPaystubCount()

        Using connection As New MySqlConnection(connectionString),
            command As New MySqlCommand(sql, connection)

            With command.Parameters
                .AddWithValue("@OrganizationID", CStr(z_OrganizationID))
                .AddWithValue("@Year", CStr(year))
                .AddWithValue("@SalaryType", CStr(salaryType))
            End With

            Await connection.OpenAsync()

            Dim reader = Await command.ExecuteReaderAsync()
            While Await reader.ReadAsync()
                Dim payPeriod = New PayPeriod() With {
                    .RowID = reader.GetValue(Of Integer?)("RowID"),
                    .PayFromDate = reader.GetValue(Of Date)("PayFromDate"),
                    .PayToDate = reader.GetValue(Of Date)("PayToDate"),
                    .Year = reader.GetValue(Of Integer)("Year"),
                    .Month = reader.GetValue(Of Integer)("Month"),
                    .OrdinalValue = reader.GetValue(Of Integer)("OrdinalValue"),
                    .IsClosed = reader.GetValue(Of Boolean)("IsClosed")
                }

                If payPeriod.IsClosed Then
                    payPeriod.Status = PayPeriodStatusData.PayPeriodStatus.Closed
                Else
                    'if the pay period is open and has existing paystubs
                    'its status should be PROCESSING
                    If payPeriodsWithPaystubCount.Any(Function(p) p.RowID.Value = payPeriod.RowID.Value) Then
                        payPeriod.Status = PayPeriodStatusData.PayPeriodStatus.Processing
                    Else
                        payPeriod.Status = PayPeriodStatusData.PayPeriodStatus.Open

                    End If
                End If

                payPeriods.Add(payPeriod)

            End While
        End Using

        Return payPeriods
    End Function

    Private Async Sub LoadTimeEntries()
        If _selectedEmployee Is Nothing Or _selectedPayPeriod Is Nothing Then
            Return
        End If

        Dim loadingEmployee = _selectedEmployee

        Dim timeEntries As ICollection(Of TimeEntry)
        If _isActual Then
            timeEntries = Await GetActualTimeEntries(_selectedEmployee, _selectedPayPeriod)
        Else
            timeEntries = Await GetTimeEntries(_selectedEmployee, _selectedPayPeriod)
        End If

        If loadingEmployee.EmployeeNo <> _selectedEmployee.EmployeeNo Then
            Return
        End If

        SetVisibleColumns(timeEntries)
        timeEntriesDataGridView.DataSource = timeEntries
    End Sub

    Private Sub SetVisibleColumns(timeEntries As ICollection(Of TimeEntry))
        timeEntriesDataGridView.SuspendLayout()

        ColumnOBStart.Visible = timeEntries.Any(Function(t) t.OBStartTime.HasValue)
        ColumnOBEnd.Visible = timeEntries.Any(Function(t) t.OBEndTime.HasValue)

        ColumnOTStart.Visible = timeEntries.Any(Function(t) t.OTStartTime.HasValue)
        ColumnOTEnd.Visible = timeEntries.Any(Function(t) t.OTEndTime.HasValue)

        ColumnLeaveStart.Visible = timeEntries.Any(Function(t) t.LeaveStart.HasValue)
        ColumnLeaveEnd.Visible = timeEntries.Any(Function(t) t.LeaveEnd.HasValue)

        ColumnNDiffHrs.Visible = timeEntries.Any(Function(t) t.NightDiffHours > 0)
        ColumnNDiffPay.Visible = _hideMoneyColumns = False AndAlso timeEntries.Any(Function(t) t.NightDiffAmount > 0)

        ColumnNDiffOTHrs.Visible = timeEntries.Any(Function(t) t.NightDiffOTHours > 0)
        ColumnNDiffOTPay.Visible = _hideMoneyColumns = False AndAlso timeEntries.Any(Function(t) t.NightDiffOTAmount > 0)

        ColumnRDayHrs.Visible = timeEntries.Any(Function(t) t.RestDayHours > 0)
        ColumnRDayPay.Visible = _hideMoneyColumns = False AndAlso timeEntries.Any(Function(t) t.RestDayAmount > 0)

        ColumnRDayOTHrs.Visible = timeEntries.Any(Function(t) t.RestDayOTHours > 0)
        ColumnRDayOTPay.Visible = _hideMoneyColumns = False AndAlso timeEntries.Any(Function(t) t.RestDayOTPay > 0)

        ColumnSHolHrs.Visible = timeEntries.Any(Function(t) t.SpecialHolidayHours > 0)
        ColumnSHolPay.Visible = _hideMoneyColumns = False AndAlso timeEntries.Any(Function(t) t.SpecialHolidayPay > 0)

        ColumnSHolOTHrs.Visible = timeEntries.Any(Function(t) t.SpecialHolidayOTHours > 0)
        ColumnSHolOTPay.Visible = _hideMoneyColumns = False AndAlso timeEntries.Any(Function(t) t.SpecialHolidayOTPay > 0)

        ColumnRHolHrs.Visible = timeEntries.Any(Function(t) t.RegularHolidayHours > 0)
        ColumnRHolPay.Visible = _hideMoneyColumns = False AndAlso timeEntries.Any(Function(t) t.RegularHolidayPay > 0)

        ColumnRHolOTHrs.Visible = timeEntries.Any(Function(t) t.RegularHolidayOTHours > 0)
        ColumnRHolOTPay.Visible = _hideMoneyColumns = False AndAlso timeEntries.Any(Function(t) t.RegularHolidayOTPay > 0)

        ColumnOTPay.Visible = _hideMoneyColumns = False
        ColumnLeavePay.Visible = _hideMoneyColumns = False
        ColumnAbsent.Visible = _hideMoneyColumns = False
        ColumnLateDeduc.Visible = _hideMoneyColumns = False
        ColumnUndertimeDeduc.Visible = _hideMoneyColumns = False
        ColumnTotalPay.Visible = _hideMoneyColumns = False
        ColumnTotalAdditionalPay.Visible = _hideMoneyColumns = False

        timeEntriesDataGridView.ResumeLayout()
    End Sub

    Private Async Sub LoadYears()
        Dim years = Await GetYears()
        cboYears.ComboBox.DataSource = years
        cboYears.ComboBox.SelectedItem = _selectedYear

        AddHandler cboYears.SelectedIndexChanged, AddressOf cboYears_SelectedIndexChanged
    End Sub

    Private Async Function GetYears() As Task(Of ICollection(Of Integer))
        Dim sql = <![CDATA[
            SELECT payperiod.Year
            FROM payperiod
            WHERE payperiod.OrganizationID
            GROUP BY payperiod.Year
        ]]>.Value

        Dim years = New Collection(Of Integer)

        Using connection As New MySqlConnection(connectionString),
            command As New MySqlCommand(sql, connection)

            Await connection.OpenAsync()

            Dim reader = Await command.ExecuteReaderAsync()
            While Await reader.ReadAsync()
                years.Add(reader.GetValue(Of Integer)("Year"))
            End While
        End Using

        Return years
    End Function

    Private Async Function GetTimeEntries(employee As Employee, payPeriod As PayPeriod) As Task(Of ICollection(Of TimeEntry))

        Dim sql = <![CDATA[
            SELECT
                ete.RowID,
                ete.Date,
                etd.TimeIn,
                etd.TimeOut,
                etd.RowID,
                IF(@useNewSchedule, IFNULL(shiftschedules.StartTime, NULL), IFNULL(shift.TimeFrom, NULL)) AS ShiftFrom,
                IF(@useNewSchedule, IFNULL(shiftschedules.EndTime, NULL), IFNULL(shift.TimeTo, NULL)) AS ShiftTo,
                ete.RegularHoursWorked,
                ete.RegularHoursAmount,
                ete.NightDifferentialHours,
                ete.NightDiffHoursAmount,
                ete.OvertimeHoursWorked,
                ete.OvertimeHoursAmount,
                ete.NightDifferentialOTHours,
                ete.NightDiffOTHoursAmount,
                ete.RestDayHours,
                ete.RestDayAmount,
                ete.RestDayOTHours,
                ete.RestDayOTPay,
                ete.LeavePayment,
                ete.HoursLate,
                ete.HoursLateAmount,
                ete.UndertimeHours,
                ete.UndertimeHoursAmount,
                ete.VacationLeaveHours,
                ete.SickLeaveHours,
                ete.OtherLeaveHours,
                ete.Leavepayment,
                ete.SpecialHolidayHours,
                ete.SpecialHolidayPay,
                ete.SpecialHolidayOTHours,
                ete.SpecialHolidayOTPay,
                ete.RegularHolidayHours,
                ete.RegularHolidayPay,
                ete.RegularHolidayOTHours,
                ete.RegularHolidayOTPay,
                ete.HolidayPayAmount,
                ete.AbsentHours,
                ete.Absent,
                ete.TotalHoursWorked,
                ete.TotalDayPay,
                ofb.OffBusStartTime,
                ofb.OffBusEndTime,
                ot.OTStartTime,
                ot.OTEndTIme,
                IF(payrate.PayType='Regular Day', '', payrate.PayType) `PayType`,
                etd.TimeStampIn,
                etd.TimeStampOut,
                l.LeaveStartTime,
                l.LeaveEndTime,
                IF(@useNewSchedule, IFNULL(shiftschedules.IsRestDay, FALSE), IFNULL(employeeshift.RestDay, FALSE)) `IsRestDay`
            FROM employeetimeentry ete
            LEFT JOIN (
                SELECT EmployeeID, DATE,
				    (SELECT RowID
				    FROM employeetimeentrydetails
				    WHERE EmployeeID = groupedEtd.EmployeeID
				    AND DATE = groupedEtd.Date
				    ORDER BY LastUpd DESC
				    LIMIT 1) RowID
				FROM employeetimeentrydetails groupedEtd
				WHERE Date BETWEEN @DateFrom AND @DateTo
				GROUP BY EmployeeID, Date
            ) latest
                ON latest.EmployeeID = ete.EmployeeID AND
                    latest.Date = ete.Date
            LEFT JOIN employeetimeentrydetails etd
                ON etd.Date = ete.Date AND
                    etd.OrganizationID = ete.OrganizationID AND
                    etd.EmployeeID = ete.EmployeeID AND
                    etd.RowID = latest.RowID
            LEFT JOIN employeeshift
                ON employeeshift.RowID = ete.EmployeeShiftID
            LEFT JOIN (
                SELECT EmployeeID, OffBusStartDate Date, MAX(Created) Created
                FROM employeeofficialbusiness
                WHERE OffBusStartDate BETWEEN @DateFrom AND @DateTo
                GROUP BY EmployeeID, Date
            ) latestOb
                ON latestOb.EmployeeID = ete.EmployeeID AND
                    latestOb.Date = ete.Date
            LEFT JOIN employeeofficialbusiness ofb
                ON ofb.OffBusStartDate = ete.Date AND
                    ofb.EmployeeID = ete.EmployeeID AND
                    ofb.Created = latestOb.Created
            LEFT JOIN employeeovertime ot
                ON ot.OTStartDate = ete.Date AND
                    ot.EmployeeID = ete.EmployeeID AND
                    ot.OTStatus = 'Approved'
            LEFT JOIN employeeleave l
                ON l.LeaveStartDate = ete.Date AND
                    l.EmployeeID = ete.EmployeeID AND
                    l.Status = 'Approved'
            LEFT JOIN payrate
                ON payrate.Date = ete.Date AND
                    payrate.OrganizationID = ete.OrganizationID
            LEFT JOIN shiftschedules
                ON shiftschedules.EmployeeID = ete.EmployeeID AND
                    shiftschedules.`Date` = ete.`Date`

            LEFT JOIN shift
                ON employeeshift.ShiftID = shift.RowID

            WHERE ete.EmployeeID = @EmployeeID AND
                ete.`Date` BETWEEN @DateFrom AND @DateTo
            ORDER BY ete.`Date`;
        ]]>.Value

        Dim timeEntries = New Collection(Of TimeEntry)

        Using connection As New MySqlConnection(connectionString),
            command As New MySqlCommand(sql, connection)

            With command.Parameters
                .AddWithValue("@EmployeeID", employee.RowID)
                .AddWithValue("@DateFrom", payPeriod.PayFromDate)
                .AddWithValue("@DateTo", payPeriod.PayToDate)
                .AddWithValue("@UseNewSchedule", _useNewShift)
            End With

            Await connection.OpenAsync()
            Dim reader = Await command.ExecuteReaderAsync()

            Dim totalTimeEntry = New TimeEntry()

            While Await reader.ReadAsync()
                Dim timeEntry = New TimeEntry() With {
                    .RowID = reader.GetValue(Of Integer?)("RowID"),
                    .EntryDate = reader.GetValue(Of Date?)("Date"),
                    .TimeIn = reader.GetValue(Of TimeSpan?)("TimeIn"),
                    .TimeOut = reader.GetValue(Of TimeSpan?)("TimeOut"),
                    .ShiftFrom = reader.GetValue(Of TimeSpan?)("ShiftFrom"),
                    .ShiftTo = reader.GetValue(Of TimeSpan?)("ShiftTo"),
                    .OBStartTime = reader.GetValue(Of TimeSpan?)("OffBusStartTime"),
                    .OBEndTime = reader.GetValue(Of TimeSpan?)("OffBusEndTime"),
                    .OTStartTime = reader.GetValue(Of TimeSpan?)("OTStartTime"),
                    .OTEndTime = reader.GetValue(Of TimeSpan?)("OTEndTime"),
                    .LeaveStart = reader.GetValue(Of TimeSpan?)("LeaveStartTime"),
                    .LeaveEnd = reader.GetValue(Of TimeSpan?)("LeaveEndTime"),
                    .RegularHours = reader.GetValue(Of Decimal)("RegularHoursWorked"),
                    .RegularAmount = reader.GetValue(Of Decimal)("RegularHoursAmount"),
                    .NightDiffHours = reader.GetValue(Of Decimal)("NightDifferentialHours"),
                    .NightDiffAmount = reader.GetValue(Of Decimal)("NightDiffHoursAmount"),
                    .OvertimeHours = reader.GetValue(Of Decimal)("OvertimeHoursWorked"),
                    .OvertimeAmount = reader.GetValue(Of Decimal)("OvertimeHoursAmount"),
                    .NightDiffOTHours = reader.GetValue(Of Decimal)("NightDifferentialOTHours"),
                    .NightDiffOTAmount = reader.GetValue(Of Decimal)("NightDiffOTHoursAmount"),
                    .RestDayHours = reader.GetValue(Of Decimal)("RestDayHours"),
                    .RestDayAmount = reader.GetValue(Of Decimal)("RestDayAmount"),
                    .RestDayOTHours = reader.GetValue(Of Decimal)("RestDayOTHours"),
                    .RestDayOTPay = reader.GetValue(Of Decimal)("RestDayOTPay"),
                    .LateHours = reader.GetValue(Of Decimal)("HoursLate"),
                    .LateAmount = reader.GetValue(Of Decimal)("HoursLateAmount"),
                    .UndertimeHours = reader.GetValue(Of Decimal)("UndertimeHours"),
                    .UndertimeAmount = reader.GetValue(Of Decimal)("UndertimeHoursAmount"),
                    .AbsentHours = reader.GetValue(Of Decimal)("AbsentHours"),
                    .AbsentAmount = reader.GetValue(Of Decimal)("Absent"),
                    .VacationLeaveHours = reader.GetValue(Of Decimal)("VacationLeaveHours"),
                    .SickLeaveHours = reader.GetValue(Of Decimal)("SickLeaveHours"),
                    .OtherLeaveHours = reader.GetValue(Of Decimal)("OtherLeaveHours"),
                    .LeavePay = reader.GetValue(Of Decimal)("Leavepayment"),
                    .SpecialHolidayHours = reader.GetValue(Of Decimal)("SpecialHolidayHours"),
                    .SpecialHolidayPay = reader.GetValue(Of Decimal)("SpecialHolidayPay"),
                    .SpecialHolidayOTHours = reader.GetValue(Of Decimal)("SpecialHolidayOTHours"),
                    .SpecialHolidayOTPay = reader.GetValue(Of Decimal)("SpecialHolidayOTPay"),
                    .RegularHolidayPay = reader.GetValue(Of Decimal)("RegularHolidayPay"),
                    .RegularHolidayHours = reader.GetValue(Of Decimal)("RegularHolidayHours"),
                    .RegularHolidayOTHours = reader.GetValue(Of Decimal)("RegularHolidayOTHours"),
                    .RegularHolidayOTPay = reader.GetValue(Of Decimal)("RegularHolidayOTPay"),
                    .HolidayPay = reader.GetValue(Of Decimal)("HolidayPayAmount"),
                    .TotalHoursWorked = reader.GetValue(Of Decimal)("TotalHoursWorked"),
                    .TotalDayPay = reader.GetValue(Of Decimal)("TotalDayPay"),
                    .HolidayType = reader.GetValue(Of String)("PayType"),
                    .IsRestDay = reader.GetValue(Of Boolean)("IsRestDay")
                }

                'check first if there is duplicate
                If timeEntries.
                    FirstOrDefault(Function(t) Nullable.Equals(t.RowID, timeEntry.RowID)) _
                    Is Nothing Then

                    With totalTimeEntry
                        .RegularHours += timeEntry.RegularHours
                        .RegularAmount += timeEntry.RegularAmount
                        .OvertimeHours += timeEntry.OvertimeHours
                        .OvertimeAmount += timeEntry.OvertimeAmount
                        .NightDiffHours += timeEntry.NightDiffHours
                        .NightDiffAmount += timeEntry.NightDiffAmount
                        .NightDiffOTHours += timeEntry.NightDiffOTHours
                        .NightDiffOTAmount += timeEntry.NightDiffOTAmount
                        .RestDayHours += timeEntry.RestDayHours
                        .RestDayAmount += timeEntry.RestDayAmount
                        .RestDayOTHours += timeEntry.RestDayOTHours
                        .RestDayOTPay += timeEntry.RestDayOTPay
                        .SpecialHolidayHours += timeEntry.SpecialHolidayHours
                        .SpecialHolidayPay += timeEntry.SpecialHolidayPay
                        .SpecialHolidayOTHours += timeEntry.SpecialHolidayOTHours
                        .SpecialHolidayOTPay += timeEntry.SpecialHolidayOTPay
                        .RegularHolidayHours += timeEntry.RegularHolidayHours
                        .RegularHolidayPay += timeEntry.RegularHolidayPay
                        .RegularHolidayOTHours += timeEntry.RegularHolidayOTHours
                        .RegularHolidayOTPay += timeEntry.RegularHolidayOTPay
                        .HolidayPay += timeEntry.HolidayPay
                        .VacationLeaveHours += timeEntry.VacationLeaveHours
                        .SickLeaveHours += timeEntry.SickLeaveHours
                        .OtherLeaveHours += timeEntry.OtherLeaveHours
                        .LeavePay += timeEntry.LeavePay
                        .LateHours += timeEntry.LateHours
                        .LateAmount += timeEntry.LateAmount
                        .UndertimeHours += timeEntry.UndertimeHours
                        .UndertimeAmount += timeEntry.UndertimeAmount
                        .AbsentHours += timeEntry.AbsentHours
                        .AbsentAmount += timeEntry.AbsentAmount
                        .TotalHoursWorked += timeEntry.TotalHoursWorked
                        .TotalDayPay += timeEntry.TotalDayPay
                    End With
                End If

                timeEntries.Add(timeEntry)
            End While

            timeEntries.Add(totalTimeEntry)
        End Using

        Return timeEntries
    End Function

    Private Async Function GetActualTimeEntries(employee As Employee, payPeriod As PayPeriod) As Task(Of ICollection(Of TimeEntry))
        Dim sql = <![CDATA[
            SELECT
                eta.RowID,
                eta.Date,
                employeetimeentrydetails.TimeIn,
                employeetimeentrydetails.TimeOut,
                IF(@useNewSchedule, IFNULL(shiftschedules.StartTime, NULL), IFNULL(shift.TimeFrom, NULL)) AS ShiftFrom,
                IF(@useNewSchedule, IFNULL(shiftschedules.EndTime, NULL), IFNULL(shift.TimeTo, NULL)) AS ShiftTo,
                eta.RegularHoursWorked,
                eta.RegularHoursAmount,
                eta.NightDifferentialHours,
                eta.NightDiffHoursAmount,
                eta.OvertimeHoursWorked,
                eta.OvertimeHoursAmount,
                eta.NightDifferentialOTHours,
                eta.NightDiffOTHoursAmount,
                eta.RestDayHours,
                eta.RestDayAmount,
                eta.RestDayOTHours,
                eta.RestDayOTPay,
                ete.SpecialHolidayHours,
                eta.SpecialHolidayPay,
                ete.SpecialHolidayOTHours,
                eta.SpecialHolidayOTPay,
                ete.RegularHolidayHours,
                eta.RegularHolidayPay,
                ete.RegularHolidayOTHours,
                eta.RegularHolidayOTPay,
                eta.LeavePayment,
                eta.HoursLate,
                eta.HoursLateAmount,
                eta.UndertimeHours,
                eta.UndertimeHoursAmount,
                eta.Leavepayment,
                eta.HolidayPayAmount,
                eta.Absent,
                eta.TotalHoursWorked,
                eta.TotalDayPay,
                ofb.OffBusStartTime,
                ofb.OffBusEndTime,
                employeetimeentrydetails.TimeStampIn,
                employeetimeentrydetails.TimeStampOut,
                l.LeaveStartTime,
                l.LeaveEndTime,
                IF(payrate.PayType='Regular Day', '', payrate.PayType) `PayType`,
                IF(@useNewSchedule, IFNULL(shiftschedules.IsRestDay, FALSE), IFNULL(employeeshift.RestDay, FALSE)) `IsRestDay`
            FROM employeetimeentryactual eta
            LEFT JOIN employeetimeentry ete
                ON ete.EmployeeID = eta.EmployeeID AND
                    ete.Date = eta.Date
            LEFT JOIN payrate
                ON payrate.Date = ete.Date AND
                    payrate.OrganizationID = ete.OrganizationID
            LEFT JOIN (
                SELECT EmployeeID, DATE,
				    (SELECT RowID
				    FROM employeetimeentrydetails
				    WHERE EmployeeID = groupedEtd.EmployeeID
				    AND DATE = groupedEtd.Date
				    ORDER BY LastUpd DESC
				    LIMIT 1) RowID
				FROM employeetimeentrydetails groupedEtd
				WHERE Date BETWEEN @DateFrom AND @DateTo
				GROUP BY EmployeeID, Date
            ) latest
                ON latest.EmployeeID = eta.EmployeeID AND
                    latest.Date = eta.Date
            LEFT JOIN employeetimeentrydetails
                ON employeetimeentrydetails.Date = eta.Date AND
                employeetimeentrydetails.OrganizationID = eta.OrganizationID AND
                employeetimeentrydetails.EmployeeID = eta.EmployeeID AND
                employeetimeentrydetails.RowID = latest.RowID
            LEFT JOIN employeeshift
                ON employeeshift.RowID = eta.EmployeeShiftID
            LEFT JOIN (
                SELECT EmployeeID, OffBusStartDate Date, MAX(Created) Created
                FROM employeeofficialbusiness
                WHERE OffBusStartDate BETWEEN @DateFrom AND @DateTo
                GROUP BY EmployeeID, Date
            ) latestOb
                ON latestOb.EmployeeID = ete.EmployeeID AND
                    latestOb.Date = ete.Date
            LEFT JOIN employeeleave l
                ON l.LeaveStartDate = ete.Date AND
                    l.EmployeeID = ete.EmployeeID AND
                    l.Status = 'Approved'
            LEFT JOIN employeeofficialbusiness ofb
                ON ofb.OffBusStartDate = eta.Date AND
                    ofb.EmployeeID = eta.EmployeeID AND
                    latestOb.Created = ofb.Created
            LEFT JOIN shift
                ON shift.RowID = employeeshift.ShiftID

            LEFT JOIN shiftschedules
                ON shiftschedules.EmployeeID = ete.EmployeeID AND
                shiftschedules.`Date` = ete.`Date`

            WHERE eta.EmployeeID = @EmployeeID AND
                eta.`Date` BETWEEN @DateFrom AND @DateTo
            ORDER BY eta.`Date`;
        ]]>.Value

        Dim timeEntries = New Collection(Of TimeEntry)

        Using connection As New MySqlConnection(connectionString),
            command As New MySqlCommand(sql, connection)

            With command.Parameters
                .AddWithValue("@EmployeeID", employee.RowID)
                .AddWithValue("@DateFrom", payPeriod.PayFromDate)
                .AddWithValue("@DateTo", payPeriod.PayToDate)
                .AddWithValue("@UseNewSchedule", _useNewShift)
            End With

            Await connection.OpenAsync()
            Dim reader = Await command.ExecuteReaderAsync()

            Dim totalTimeEntry = New TimeEntry()

            While Await reader.ReadAsync()
                Dim timeEntry = New TimeEntry() With {
                    .RowID = reader.GetValue(Of Integer?)("RowID"),
                    .EntryDate = reader.GetValue(Of Date)("Date"),
                    .TimeIn = reader.GetValue(Of TimeSpan?)("TimeIn"),
                    .TimeOut = reader.GetValue(Of TimeSpan?)("TimeOut"),
                    .ShiftFrom = reader.GetValue(Of TimeSpan?)("ShiftFrom"),
                    .ShiftTo = reader.GetValue(Of TimeSpan?)("ShiftTo"),
                    .OBStartTime = reader.GetValue(Of TimeSpan?)("OffBusStartTime"),
                    .OBEndTime = reader.GetValue(Of TimeSpan?)("OffBusEndTime"),
                    .LeaveStart = reader.GetValue(Of TimeSpan?)("LeaveStartTime"),
                    .LeaveEnd = reader.GetValue(Of TimeSpan?)("LeaveEndTime"),
                    .RegularHours = reader.GetValue(Of Decimal)("RegularHoursWorked"),
                    .RegularAmount = reader.GetValue(Of Decimal)("RegularHoursAmount"),
                    .NightDiffHours = reader.GetValue(Of Decimal)("NightDifferentialHours"),
                    .NightDiffAmount = reader.GetValue(Of Decimal)("NightDiffHoursAmount"),
                    .OvertimeHours = reader.GetValue(Of Decimal)("OvertimeHoursWorked"),
                    .OvertimeAmount = reader.GetValue(Of Decimal)("OvertimeHoursAmount"),
                    .NightDiffOTHours = reader.GetValue(Of Decimal)("NightDifferentialOTHours"),
                    .NightDiffOTAmount = reader.GetValue(Of Decimal)("NightDiffOTHoursAmount"),
                    .RestDayHours = reader.GetValue(Of Decimal)("RestDayHours"),
                    .RestDayAmount = reader.GetValue(Of Decimal)("RestDayAmount"),
                    .RestDayOTHours = reader.GetValue(Of Decimal)("RestDayOTHours"),
                    .RestDayOTPay = reader.GetValue(Of Decimal)("RestDayOTPay"),
                    .SpecialHolidayHours = reader.GetValue(Of Decimal)("SpecialHolidayHours"),
                    .SpecialHolidayPay = reader.GetValue(Of Decimal)("SpecialHolidayPay"),
                    .SpecialHolidayOTHours = reader.GetValue(Of Decimal)("SpecialHolidayOTHours"),
                    .SpecialHolidayOTPay = reader.GetValue(Of Decimal)("SpecialHolidayOTPay"),
                    .RegularHolidayHours = reader.GetValue(Of Decimal)("RegularHolidayHours"),
                    .RegularHolidayPay = reader.GetValue(Of Decimal)("RegularHolidayPay"),
                    .RegularHolidayOTHours = reader.GetValue(Of Decimal)("RegularHolidayOTHours"),
                    .RegularHolidayOTPay = reader.GetValue(Of Decimal)("RegularHolidayOTPay"),
                    .LateHours = reader.GetValue(Of Decimal)("HoursLate"),
                    .LateAmount = reader.GetValue(Of Decimal)("HoursLateAmount"),
                    .UndertimeHours = reader.GetValue(Of Decimal)("UndertimeHours"),
                    .UndertimeAmount = reader.GetValue(Of Decimal)("UndertimeHoursAmount"),
                    .AbsentAmount = reader.GetValue(Of Decimal)("Absent"),
                    .LeavePay = reader.GetValue(Of Decimal)("Leavepayment"),
                    .HolidayPay = reader.GetValue(Of Decimal)("HolidayPayAmount"),
                    .TotalHoursWorked = reader.GetValue(Of Decimal)("TotalHoursWorked"),
                    .TotalDayPay = reader.GetValue(Of Decimal)("TotalDayPay"),
                    .HolidayType = reader.GetValue(Of String)("PayType"),
                    .IsRestDay = reader.GetValue(Of Boolean)("IsRestDay")
                }

                'check first if there is duplicate
                If timeEntries.
                    FirstOrDefault(Function(t) Nullable.Equals(t.RowID, timeEntry.RowID)) _
                    Is Nothing Then

                    totalTimeEntry.RegularHours += timeEntry.RegularHours
                    totalTimeEntry.RegularAmount += timeEntry.RegularAmount
                    totalTimeEntry.OvertimeHours += timeEntry.OvertimeHours
                    totalTimeEntry.OvertimeAmount += timeEntry.OvertimeAmount
                    totalTimeEntry.NightDiffHours += timeEntry.NightDiffHours
                    totalTimeEntry.NightDiffAmount += timeEntry.NightDiffAmount
                    totalTimeEntry.NightDiffOTHours += timeEntry.NightDiffOTHours
                    totalTimeEntry.NightDiffOTAmount += timeEntry.NightDiffOTAmount
                    totalTimeEntry.RestDayHours += timeEntry.RestDayHours
                    totalTimeEntry.RestDayAmount += timeEntry.RestDayAmount
                    totalTimeEntry.RestDayOTHours += timeEntry.RestDayOTHours
                    totalTimeEntry.RestDayOTPay += timeEntry.RestDayOTPay
                    totalTimeEntry.SpecialHolidayHours += timeEntry.SpecialHolidayHours
                    totalTimeEntry.SpecialHolidayPay += timeEntry.SpecialHolidayPay
                    totalTimeEntry.SpecialHolidayOTHours += timeEntry.SpecialHolidayOTHours
                    totalTimeEntry.SpecialHolidayOTPay += timeEntry.SpecialHolidayOTPay
                    totalTimeEntry.RegularHolidayHours += timeEntry.RegularHolidayHours
                    totalTimeEntry.RegularHolidayPay += timeEntry.RegularHolidayPay
                    totalTimeEntry.RegularHolidayOTHours += timeEntry.RegularHolidayOTHours
                    totalTimeEntry.RegularHolidayOTPay += timeEntry.RegularHolidayOTPay
                    totalTimeEntry.HolidayPay += timeEntry.HolidayPay
                    totalTimeEntry.LeavePay += timeEntry.LeavePay
                    totalTimeEntry.LateHours += timeEntry.LateHours
                    totalTimeEntry.LateAmount += timeEntry.LateAmount
                    totalTimeEntry.UndertimeHours += timeEntry.UndertimeHours
                    totalTimeEntry.UndertimeAmount += timeEntry.UndertimeAmount
                    totalTimeEntry.AbsentAmount += timeEntry.AbsentAmount
                    totalTimeEntry.TotalHoursWorked += timeEntry.TotalHoursWorked
                    totalTimeEntry.TotalDayPay += timeEntry.TotalDayPay

                End If

                timeEntries.Add(timeEntry)
            End While

            timeEntries.Add(totalTimeEntry)
        End Using

        Return timeEntries
    End Function

    Private Async Sub generateTimeEntryButton_Click(sender As Object, e As EventArgs) Handles generateTimeEntryButton.Click
        Dim startDate As Date
        Dim endDate As Date
        Dim result As DialogResult

        Using dialog = New DateRangePickerDialog(_selectedPayPeriod)
            result = dialog.ShowDialog()

            If result = DialogResult.OK Then
                startDate = dialog.Start
                endDate = dialog.End
            End If
        End Using

        If result = DialogResult.OK Then
            Dim generator = New TimeEntryGenerator(startDate, endDate)
            Dim progressDialog = New TimeEntryProgressDialog(generator)
            progressDialog.Show()

            Await Task.Run(Sub() generator.Start()).
                ContinueWith(
                    Sub() DoneGenerating(progressDialog, generator),
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.FromCurrentSynchronizationContext)
        End If

        LoadTimeEntries()
    End Sub

    Private Async Sub regenerateTimeEntryButton_Click(sender As Object, e As EventArgs) Handles regenerateTimeEntryButton.Click

        If Await PayrollTools.
                    ValidatePayPeriodAction(_selectedPayPeriod.RowID) = False Then Return

        Dim generator = New TimeEntryGenerator(_selectedPayPeriod.PayFromDate, _selectedPayPeriod.PayToDate)

        Dim progressDialog = New TimeEntryProgressDialog(generator)
        progressDialog.Show()
        Await Task.Run(Sub() generator.Start()).
            ContinueWith(
                Sub() DoneGenerating(progressDialog, generator),
                CancellationToken.None,
                TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.FromCurrentSynchronizationContext)

        LoadTimeEntries()
    End Sub

    Private Sub DoneGenerating(dialog As TimeEntryProgressDialog, generator As TimeEntryGenerator)
        dialog.Close()
        dialog.Dispose()

        Dim msgBoxText As String = "Done"

        If generator.ErrorCount > 0 Then
            Dim errorCount = generator.ErrorCount
            msgBoxText = String.Concat("Done, with ", errorCount, If(errorCount = 1, " error", " errors."))
        End If

        MsgBox(msgBoxText)

    End Sub

    Private Sub employeesDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles employeesDataGridView.SelectionChanged
        If employeesDataGridView.CurrentRow Is Nothing Then
            Return
        End If

        Dim employee = DirectCast(employeesDataGridView.CurrentRow.DataBoundItem, Employee)
        If employee Is _selectedEmployee Then
            Return
        End If

        _selectedEmployee = employee
        LoadTimeEntries()
    End Sub

    Private Sub payPeriodDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles payPeriodsDataGridView.SelectionChanged
        If payPeriodsDataGridView.CurrentRow Is Nothing Then
            Return
        End If

        Dim payPeriod = DirectCast(payPeriodsDataGridView.CurrentCell.Value, PayPeriod)
        If payPeriod Is _selectedPayPeriod Then
            Return
        End If

        _selectedPayPeriod = payPeriod

        Dim isPayPeriodProcessing = _selectedPayPeriod IsNot Nothing AndAlso
                _selectedPayPeriod.Status = PayPeriodStatusData.PayPeriodStatus.Processing

        tsBtnDeleteTimeEntry.Visible = isPayPeriodProcessing
        regenerateTimeEntryButton.Visible = isPayPeriodProcessing

        LoadTimeEntries()
    End Sub

    Private Sub tsbtnCloseempawar_Click(sender As Object, e As EventArgs) Handles tsbtnCloseempawar.Click
        Close()
        TimeAttendForm.listTimeAttendForm.Remove(Name)
    End Sub

    Private Sub searchTextBox_TextChanged(sender As Object, e As EventArgs) Handles searchTextBox.TextChanged
        FilterEmployees()
    End Sub

    Private Async Sub FilterEmployees()
        Dim searchValue = searchTextBox.Text.ToLower()

        employeesDataGridView.DataSource = Await _employeeRepository.SearchSimpleLocal(Me._employees, searchValue)
        employeesDataGridView.Update()
    End Sub

    Private Shared Function ConvertToDate(time As TimeSpan?) As Date?
        If Not time.HasValue Then
            Return Nothing
        End If

        If time >= HoursPerDay Then
            Return Date.Parse((time - HoursPerDay).ToString())
        Else
            Return Date.Parse(time.ToString())
        End If
    End Function

    Private Sub actualButtonn_Click(sender As Object, e As EventArgs) Handles actualButton.Click
        _isActual = Not _isActual

        actualButton.Checked = _isActual

        LoadTimeEntries()
    End Sub

    Private Sub btnAmPm_Click(sender As Object, e As EventArgs) Handles btnAmPm.Click
        _isAmPm = Not _isAmPm

        btnAmPm.Checked = _isAmPm

        If _isAmPm Then
            ColumnShiftFrom.DefaultCellStyle.Format = Clock12HourFormat
            ColumnShiftTo.DefaultCellStyle.Format = Clock12HourFormat
            ColumnTimeIn.DefaultCellStyle.Format = Clock12HourFormat
            ColumnTimeOut.DefaultCellStyle.Format = Clock12HourFormat
            ColumnOTStart.DefaultCellStyle.Format = Clock12HourFormat
            ColumnOTEnd.DefaultCellStyle.Format = Clock12HourFormat
            ColumnOBStart.DefaultCellStyle.Format = Clock12HourFormat
            ColumnOBEnd.DefaultCellStyle.Format = Clock12HourFormat
        Else
            ColumnShiftFrom.DefaultCellStyle.Format = Clock24HourFormat
            ColumnShiftTo.DefaultCellStyle.Format = Clock24HourFormat
            ColumnTimeIn.DefaultCellStyle.Format = Clock24HourFormat
            ColumnTimeOut.DefaultCellStyle.Format = Clock24HourFormat
            ColumnOTStart.DefaultCellStyle.Format = Clock24HourFormat
            ColumnOTEnd.DefaultCellStyle.Format = Clock24HourFormat
            ColumnOBStart.DefaultCellStyle.Format = Clock24HourFormat
            ColumnOBEnd.DefaultCellStyle.Format = Clock24HourFormat
        End If
    End Sub

    Private Class PayPeriod
        Implements IPayPeriod

        Public Property RowID As Integer? Implements IPayPeriod.RowID
        Public Property PayFromDate As Date Implements IPayPeriod.PayFromDate
        Public Property PayToDate As Date Implements IPayPeriod.PayToDate
        Public Property Year As Integer
        Public Property Month As Integer
        Public Property OrdinalValue As Integer
        Public Property IsClosed As Boolean
        Public Property Status As PayPeriodStatusData.PayPeriodStatus

        Public Overrides Function ToString() As String
            Dim dateFrom = PayFromDate.ToString("MMM dd")
            Dim dateTo = PayToDate.ToString("MMM dd")

            Return dateFrom + " - " + dateTo
        End Function

    End Class

    Private Class TimeEntry
        Public Property RowID As Integer?
        Public Property EntryDate As Date?
        Public Property TimeIn As TimeSpan?
        Public Property TimeOut As TimeSpan?
        Public Property ShiftFrom As TimeSpan?
        Public Property ShiftTo As TimeSpan?
        Public Property OBStartTime As TimeSpan?
        Public Property OBEndTime As TimeSpan?
        Public Property OTStartTime As TimeSpan?
        Public Property OTEndTime As TimeSpan?
        Public Property LeaveStart As TimeSpan?
        Public Property LeaveEnd As TimeSpan?
        Public Property RegularHours As Decimal
        Public Property RegularAmount As Decimal
        Public Property NightDiffHours As Decimal
        Public Property NightDiffAmount As Decimal
        Public Property OvertimeHours As Decimal
        Public Property OvertimeAmount As Decimal
        Public Property NightDiffOTHours As Decimal
        Public Property NightDiffOTAmount As Decimal
        Public Property RestDayHours As Decimal
        Public Property RestDayAmount As Decimal
        Public Property RestDayOTHours As Decimal
        Public Property RestDayOTPay As Decimal
        Public Property SpecialHolidayHours As Decimal
        Public Property SpecialHolidayPay As Decimal
        Public Property SpecialHolidayOTHours As Decimal
        Public Property SpecialHolidayOTPay As Decimal
        Public Property RegularHolidayHours As Decimal
        Public Property RegularHolidayPay As Decimal
        Public Property RegularHolidayOTHours As Decimal
        Public Property RegularHolidayOTPay As Decimal
        Public Property HolidayPay As Decimal
        Public Property VacationLeaveHours As Decimal
        Public Property SickLeaveHours As Decimal
        Public Property OtherLeaveHours As Decimal
        Public Property LeavePay As Decimal
        Public Property UndertimeHours As Decimal
        Public Property UndertimeAmount As Decimal
        Public Property LateHours As Decimal
        Public Property LateAmount As Decimal
        Public Property AbsentHours As Decimal
        Public Property AbsentAmount As Decimal
        Public Property TotalHoursWorked As Decimal
        Public Property TotalDayPay As Decimal

        Public Property IsRestDay As Boolean
        Public Property HolidayType As String

        Public ReadOnly Property TotalAdditionalPay As Decimal
            Get
                Return TotalDayPay - RegularAmount
            End Get
        End Property

        Public ReadOnly Property TotalAdditionalHours As Decimal
            Get
                Return (TotalHoursWorked + LeaveHours + NightDiffHours + NightDiffOTHours) - RegularHours
            End Get
        End Property

        Public ReadOnly Property DayType As String
            Get
                Dim restDayText = If(IsRestDay, "RDay", String.Empty)
                Dim texts = {HolidayAcronym(HolidayType), restDayText}
                Dim strings = texts.Where(Function(s) Not String.IsNullOrWhiteSpace(s)).ToArray()
                If Not strings.Any() Then Return String.Empty
                Return String.Join("+", strings)
            End Get
        End Property

        Private Function HolidayAcronym(holidayName As String) As String
            If String.IsNullOrWhiteSpace(holidayName) Then Return String.Empty
            Dim nameOfHoliday = holidayName.ToLower()

            Dim abbreviatn As String = String.Empty
            If nameOfHoliday = "regular holiday" Then abbreviatn = "RHol"
            If nameOfHoliday = "special non-working holiday" Then abbreviatn = "SHol"
            Return abbreviatn
        End Function

        Public ReadOnly Property TimeInDisplay As Date?
            Get
                Return ConvertToDate(TimeIn)
            End Get
        End Property

        Public ReadOnly Property TimeOutDisplay As Date?
            Get
                Return ConvertToDate(TimeOut)
            End Get
        End Property

        Public ReadOnly Property ShiftFromDisplay As Date?
            Get
                Return ConvertToDate(ShiftFrom)
            End Get
        End Property

        Public ReadOnly Property ShiftToDisplay As Date?
            Get
                Return ConvertToDate(ShiftTo)
            End Get
        End Property

        Public ReadOnly Property OTStartTimeDisplay As Date?
            Get
                Return ConvertToDate(OTStartTime)
            End Get
        End Property

        Public ReadOnly Property OTEndTimeDisplay As Date?
            Get
                Return ConvertToDate(OTEndTime)
            End Get
        End Property

        Public ReadOnly Property OBStartTimeDisplay As Date?
            Get
                Return ConvertToDate(OBStartTime)
            End Get
        End Property

        Public ReadOnly Property OBEndTimeDisplay As Date?
            Get
                Return ConvertToDate(OBEndTime)
            End Get
        End Property

        Public ReadOnly Property LeaveStartDisplay As Date?
            Get
                Return ConvertToDate(LeaveStart)
            End Get
        End Property

        Public ReadOnly Property LeaveEndDisplay As Date?
            Get
                Return ConvertToDate(LeaveEnd)
            End Get
        End Property

        Public ReadOnly Property LeaveHours As Decimal
            Get
                Return VacationLeaveHours + SickLeaveHours + OtherLeaveHours
            End Get
        End Property

        Public ReadOnly Property Remarks As String
            Get
                If LeavePay > 0 Then
                    Return "w/ Leave"
                End If

                If AbsentAmount > 0 Then
                    Return "Absent"
                End If

                If RestDayAmount > 0 Then
                    Return "Work on rest day"
                End If

                Return Nothing
            End Get
        End Property

    End Class

    Private Sub cboYears_SelectedIndexChanged(sender As Object, e As EventArgs)
        _selectedYear = DirectCast(cboYears.SelectedItem, Integer)
        Dim task = LoadPayPeriods()
    End Sub

    Private Sub timeEntriesDataGridView_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles timeEntriesDataGridView.CellMouseDown
        Dim hasRows = timeEntriesDataGridView.Rows.Count > 0
        If e.Button = Windows.Forms.MouseButtons.Right _
            And hasRows Then

            Dim negaOne = -1
            If e.ColumnIndex > negaOne And e.RowIndex > negaOne Then
                timeEntriesDataGridView.Focus()
                timeEntriesDataGridView.Item(e.ColumnIndex, e.RowIndex).Selected = True

                ctxtmenstrpTimeEntry.Show(MousePosition, ToolStripDropDownDirection.Default)
            End If
        End If
    End Sub

    Private Async Sub DeleteShiftToolStripMenuItem_ClickAsync(sender As Object, e As EventArgs) Handles DeleteShiftToolStripMenuItem.Click
        If timeEntriesDataGridView.Rows.Count > 0 Then

            If Await PayrollTools.
                    ValidatePayPeriodAction(_selectedPayPeriod.RowID) = False Then Return

            Dim currentRow = timeEntriesDataGridView.CurrentRow

            Dim dateTimeValue = currentRow?.Cells(ColumnDate.Name).Value

            Dim hasDateSelected = dateTimeValue IsNot Nothing

            If hasDateSelected Then
                Dim dateValue = DirectCast(dateTimeValue, Date)
                Dim employeeRowId = Convert.ToInt32(_selectedEmployee.RowID)

                Dim balloon As New ToolTip() With {
                    .ToolTipTitle = "Delete shift schedule",
                    .UseFading = True,
                    .UseAnimation = True,
                    .ShowAlways = True,
                    .IsBalloon = True,
                    .ToolTipIcon = ToolTipIcon.Info
                }

                If _useNewShift Then

                    Await DeleteNewShift(dateValue, employeeRowId, balloon)
                Else
                    Await DeleteOldShift(dateValue, employeeRowId, balloon)
                End If

            End If
        Else
            _currentTimeEntryDate = Date.Now
        End If
    End Sub

    Private Async Function DeleteNewShift(dateValue As Date, employeeRowId As Integer, balloon As ToolTip) As Task

        Using context = New PayrollContext()

            Dim currentShift = Await context.EmployeeDutySchedules.
                                        Where(Function(s) Nullable.Equals(s.EmployeeID, employeeRowId)).
                                        Where(Function(s) s.DateSched = dateValue).
                                        FirstOrDefaultAsync

            If currentShift IsNot Nothing Then

                context.EmployeeDutySchedules.Remove(currentShift)

                Try
                    Await context.SaveChangesAsync()

                    balloon.ToolTipTitle = "Shift schedule deleted successfully"
                    balloon.Show("Done!", Button1)

                    _selectedPayPeriod = Nothing
                    payPeriodDataGridView_SelectionChanged(timeEntriesDataGridView, New EventArgs)
                Catch ex As Exception
                    _logger.Error("Error deleting shift schedule.", ex)

                    MessageBoxHelper.DefaultErrorMessage()
                End Try

            End If

        End Using

    End Function

    Private Async Function DeleteOldShift(dateValue As Date, employeeRowId As Integer, balloon As ToolTip) As Task
        Using context = New PayrollContext()

            Dim eTimeEntry = Await context.TimeEntries.
                    Where(Function(et) Nullable.Equals(et.EmployeeID, employeeRowId)).
                    Where(Function(et) et.Date = dateValue).FirstOrDefaultAsync()

            Dim shiftRecord = Await context.ShiftSchedules.FindAsync(eTimeEntry.EmployeeShiftID)

            If shiftRecord IsNot Nothing Then
                balloon.Show("Please wait a few moment while deleting", Button1, 3000)
                InfoBalloon()
                Dim orgId = Convert.ToInt32(orgztnID)

                Dim hasTimeEntry = eTimeEntry IsNot Nothing

                Dim isShiftServeOneDay = (DateDiff("d", shiftRecord.EffectiveFrom, shiftRecord.EffectiveTo) = 0)
                If isShiftServeOneDay Then
                    context.ShiftSchedules.Remove(shiftRecord)
                Else

                    Dim primaryDiff = DateDiff("d", shiftRecord.EffectiveFrom, dateValue)
                    Dim ultiDiff = DateDiff("d", dateValue, shiftRecord.EffectiveTo)

                    Dim isLead = primaryDiff = 0
                    Dim isTrail = ultiDiff = 0

                    Dim effectDateFrom = shiftRecord.EffectiveFrom
                    Dim effectDateTo = shiftRecord.EffectiveTo

                    If isLead Then
                        shiftRecord.EffectiveFrom = effectDateFrom.AddDays(1)
                        shiftRecord.LastUpdBy = z_User
                    End If

                    If isTrail Then
                        shiftRecord.EffectiveTo = effectDateTo.AddDays(-1)
                        shiftRecord.LastUpdBy = z_User
                    End If

                    Dim isDoesntSatisfy = isLead = False And isTrail = False

                    If isDoesntSatisfy Then
                        shiftRecord.EffectiveTo = dateValue.AddDays(-1)
                        shiftRecord.LastUpdBy = z_User

                        Dim newShiftSched = New ShiftSchedule With {
                            .EffectiveFrom = dateValue.AddDays(1),
                            .EffectiveTo = effectDateTo,
                            .EmployeeID = shiftRecord.EmployeeID,
                            .ShiftID = shiftRecord.ShiftID,
                            .IsNightShift = shiftRecord.IsNightShift,
                            .OrganizationID = orgId,
                            .CreatedBy = z_User,
                            .LastUpdBy = z_User
                        }

                        context.ShiftSchedules.Add(newShiftSched)
                    End If

                End If

                If hasTimeEntry Then
                    eTimeEntry.EmployeeShiftID = Nothing
                End If

                Try
                    Await context.SaveChangesAsync()

                    'MsgBox("Shift schedule deleted successfully.", MsgBoxStyle.Information)
                    balloon.ToolTipTitle = "Shift schedule deleted successfully"
                    balloon.Show("Done!", Button1)

                    _selectedPayPeriod = Nothing
                    payPeriodDataGridView_SelectionChanged(timeEntriesDataGridView, New EventArgs)
                Catch ex As Exception
                    _logger.Error("Error deleting shift schedule.", ex)
                    MsgBox(String.Concat("Something went wrong when deleting shift schedule.", vbNewLine, "Please contact Globagility Inc. for assistance."),
                           MsgBoxStyle.OkOnly,
                           "Time entry summary form")
                    'Finally
                    '    If hasDateSelected _
                    '        And currentRow.Index < timeEntriesDataGridView.Rows.Count Then
                    '        timeEntriesDataGridView.Item(ColumnDate.Name, currentRow.Index).Selected = True
                    '    End If
                End Try
            End If
        End Using
    End Function

    Private Sub timeEntriesDataGridView_SelectionChangedAsync(sender As Object, e As EventArgs) Handles timeEntriesDataGridView.SelectionChanged

    End Sub

    Private Async Sub timeEntriesDataGridView_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles timeEntriesDataGridView.CellContentClick

        If _selectedEmployee Is Nothing Then Return

        If e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then Return

        If _calculateBreakTimeLateHours = False Then Return

        Dim currentRow = timeEntriesDataGridView.Rows(e.RowIndex)
        Dim currentColumn = timeEntriesDataGridView.Columns(e.ColumnIndex)

        If currentRow Is Nothing OrElse currentColumn Is Nothing Then Return

        If currentColumn Is ColumnRegHrs Then

            Dim nullableCurrentDate =
                ObjectUtils.ToNullableDateTime(currentRow.Cells(ColumnDate.Index).Value)

            If nullableCurrentDate Is Nothing Then Return

            Dim currentDate = Convert.ToDateTime(nullableCurrentDate)

            Dim timeAttendanceLogs As New List(Of TimeAttendanceLog)

            timeAttendanceLogs = Await _
               GetTimeAttendanceLogsOfSelectedTimeEntry(currentRow, currentDate)

            If timeAttendanceLogs Is Nothing OrElse
                timeAttendanceLogs.Count = 0 Then Return

            Dim currentShift = GetShiftTimePeriod(currentRow, currentDate)

            Dim breakTimeDuration = GetCurrentBreakTimeDuration(currentShift.Length.TotalHours)

            Dim form As New TimeAttendanceLogListForm(
                                            timeAttendanceLogs,
                                            currentShift,
                                            breakTimeDuration,
                                            _isAmPm,
                                            currentDate)
            form.ShowDialog()

        End If

    End Sub

    Private Function GetCurrentBreakTimeDuration(shiftDuration As Double) As Decimal

        Dim breakTimeBrackets = _breakTimeBrackets.
                Where(Function(b) Nullable.Equals(b.DivisionID, _selectedEmployee.Position?.DivisionID)).
                ToList()

        Return BreakTimeBracketHelper.GetBreakTimeDuration(breakTimeBrackets, shiftDuration)

    End Function

    Private Function GetShiftTimePeriod(currentRow As DataGridViewRow, currentDate As Date) As TimePeriod

        Dim shiftFromTime = ObjectUtils.
            ToNullableDateTime(currentRow.Cells(ColumnShiftFrom.Index).Value)

        Dim shiftToTime = ObjectUtils.
            ToNullableDateTime(currentRow.Cells(ColumnShiftTo.Index).Value)

        If shiftFromTime Is Nothing OrElse shiftToTime Is Nothing Then

            Return Nothing

        End If

        Dim shiftFrom = currentDate.
                            ToMinimumHourValue.
                            Add(shiftFromTime.Value.TimeOfDay)

        Dim shiftTo = If(shiftToTime <= shiftFromTime, currentDate.AddDays(1), currentDate)

        shiftTo = shiftTo.ToMinimumHourValue.Add(shiftToTime.Value.TimeOfDay)

        Return New TimePeriod(shiftFrom, shiftTo)

    End Function

    Private Async Function GetTimeAttendanceLogsOfSelectedTimeEntry(currentRow As DataGridViewRow, currentDate As Date) As Task(Of List(Of TimeAttendanceLog))
        Dim timeAttendanceLogs As List(Of TimeAttendanceLog)

        Using context As New PayrollContext

            timeAttendanceLogs = Await context.TimeAttendanceLogs.
                                    Where(Function(t) Nullable.Equals(t.EmployeeID, _selectedEmployee.RowID)).
                                    Where(Function(t) t.WorkDay = currentDate).
                                    OrderBy(Function(t) t.TimeStamp).
                                    ToListAsync

        End Using

        If timeAttendanceLogs Is Nothing OrElse timeAttendanceLogs.Count = 0 Then
            Dim timeIn = ObjectUtils.
                ToNullableDateTime(currentRow.Cells(ColumnTimeIn.Index).Value)

            Dim timeOut = ObjectUtils.
                ToNullableDateTime(currentRow.Cells(ColumnTimeOut.Index).Value)

            Dim dateTimeIn = ObjectUtils.
                ToNullableDateTime(currentRow.Cells(ColumnTimeStampIn.Index).Value)

            Dim dateTimeOut = ObjectUtils.
                ToNullableDateTime(currentRow.Cells(ColumnTimeStampOut.Index).Value)

            If timeIn IsNot Nothing Then

                Dim actualTimeIn As Date

                If dateTimeIn IsNot Nothing Then

                    actualTimeIn = dateTimeIn.
                                    ToMinimumHourValue.
                                    Add(timeIn.Value.TimeOfDay)
                Else
                    actualTimeIn = currentDate.
                                    ToMinimumHourValue.
                                    Add(timeIn.Value.TimeOfDay)

                End If

                timeAttendanceLogs.Add(
                    New TimeAttendanceLog With {
                        .IsTimeIn = True,
                        .TimeStamp = actualTimeIn
                })

            End If

            If timeOut IsNot Nothing Then

                Dim actualTimeOut As Date

                If dateTimeOut IsNot Nothing Then

                    actualTimeOut = dateTimeOut.
                                    ToMinimumHourValue.
                                    Add(timeOut.Value.TimeOfDay)
                Else

                    If timeOut >= timeIn Then
                        actualTimeOut = currentDate.
                                        ToMinimumHourValue.
                                        AddDays(1).
                                        Add(timeOut.Value.TimeOfDay)
                    Else
                        actualTimeOut = currentDate.
                                        ToMinimumHourValue.
                                        Add(timeOut.Value.TimeOfDay)

                    End If

                End If

                timeAttendanceLogs.Add(
                    New TimeAttendanceLog With {
                        .IsTimeIn = False,
                        .TimeStamp = actualTimeOut
                })
            End If

        End If

        Return timeAttendanceLogs
    End Function

    Private Sub timeEntriesDataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles timeEntriesDataGridView.CellFormatting
        AddLinkButtonEffectOnRegularHours(e)
    End Sub

    Private Sub AddLinkButtonEffectOnRegularHours(ByRef e As DataGridViewCellFormattingEventArgs)

        If _selectedEmployee Is Nothing Then Return

        If e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then Return

        If _calculateBreakTimeLateHours = False Then Return

        'format except the last row
        If e.RowIndex >= timeEntriesDataGridView.Rows.Count - 1 Then Return

        Dim currentColumn = timeEntriesDataGridView.Columns(e.ColumnIndex)

        If currentColumn Is ColumnRegHrs Then

            e.CellStyle.ForeColor = Color.Blue
            e.CellStyle.Font = New Font(e.CellStyle.Font, FontStyle.Underline)
        End If
    End Sub

    Private Sub tstbnResetLeaveBalance_Click(sender As Object, e As EventArgs) Handles tstbnResetLeaveBalance.Click
        Dim form As New PreviewLeaveBalanceForm
        form.ShowDialog()
    End Sub

    Private Async Sub tsBtnDeleteTimeEntry_ClickAsync(sender As Object, e As EventArgs) Handles tsBtnDeleteTimeEntry.Click

        If Await PayrollTools.
                    ValidatePayPeriodAction(_selectedPayPeriod.RowID) = False Then Return

        Dim ask = String.Concat("Proceed deleting employee's time entry between ", _selectedPayPeriod.PayFromDate.ToShortDateString,
                                " and ", _selectedPayPeriod.PayToDate.ToShortDateString, " ?")
        Dim askConfirmation = MessageBox.Show(ask, "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If askConfirmation = DialogResult.No Then
            Return
        End If

        Dim query = String.Concat("DELETE FROM employeetimeentry WHERE EmployeeID=@employeePrimKey AND `Date` BETWEEN @dateFrom AND @dateTo;",
                                  "DELETE FROM employeetimeentryactual WHERE EmployeeID=@employeePrimKey AND `Date` BETWEEN @dateFrom AND @dateTo;")
        Using command = New MySqlCommand(query,
                                         New MySqlConnection(mysql_conn_text))
            With command
                .Parameters.AddWithValue("@dateFrom", _selectedPayPeriod?.PayFromDate)
                .Parameters.AddWithValue("@dateTo", _selectedPayPeriod?.PayToDate)
                .Parameters.AddWithValue("@employeePrimKey", _selectedEmployee?.RowID)

                Await .Connection.OpenAsync()
            End With

            'Dim transactn = command.Connection.BeginTransaction

            Try
                Await command.ExecuteNonQueryAsync()
                'transactn.Commit()
            Catch ex As Exception
                'transactn.Rollback()
                _logger.Error("Deleting time entry period", ex)
            Finally
                LoadTimeEntries()
            End Try
        End Using
    End Sub

    Private Sub TimeEntriesDataGridView_DataSourceChanged(sender As Object, e As EventArgs) Handles timeEntriesDataGridView.DataSourceChanged
        timeEntriesDataGridView.Rows.OfType(Of DataGridViewRow).
            ToList().
            ForEach(Sub(row) ColorizeSundays(row))
    End Sub

    Private Sub ColorizeSundays(gridRow As DataGridViewRow)
        Dim timeEntry = DirectCast(gridRow.DataBoundItem, TimeEntry)

        If timeEntry.EntryDate?.DayOfWeek = DayOfWeek.Sunday Then
            gridRow.DefaultCellStyle.ForeColor = Color.Red
        End If
    End Sub

End Class