Option Strict On

Imports System.Collections.ObjectModel
Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Core
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Enums
Imports AccuPay.Core.Exceptions
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports AccuPay.Core.ValueObjects
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports AccuPay.Utilities.Extensions
Imports log4net
Imports Microsoft.Extensions.DependencyInjection
Imports MySql.Data.MySqlClient

Public Class TimeEntrySummaryForm

    Private Const Clock24HourFormat As String = "HH:mm"
    Private Const Clock12HourFormat As String = "hh:mm tt"

    Private Shared _logger As ILog = LogManager.GetLogger(Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Shared HoursPerDay As TimeSpan = New TimeSpan(24, 0, 0)

    Private Shared OptionalColumns As List(Of String) = New List(Of String) From {
        "SpecialHolidayHours",
        "SpecialHolidayPay"
    }

    Private _selectedYear As Integer

    Private _weeklyPayPeriods As ICollection(Of PayPeriod)

    Private _semiMonthlyPayPeriods As ICollection(Of PayPeriod)

    Private _employees As ICollection(Of Employee)

    Private _selectedEmployee As Employee

    Private _breakTimeBrackets As List(Of BreakTimeBracket)

    Private _selectedPayPeriod As PayPeriod

    Private _isActual As Boolean = False

    Private _isAmPm As Boolean = False

    Private _currentTimeEntryDate As Date

    Private _hideMoneyColumns As Boolean

    Private _formHasLoaded As Boolean = False

    Private _generateDefaultShiftAndTimeLogsButtonHidden As Boolean

    Private _regenerateTimeEntryButtonHidden As Boolean

    Private _tsBtnDeleteTimeEntryHidden As Boolean
    Private _currentSystemOwner As SystemOwner
    Private _organization As Organization
    Private ReadOnly _policy As IPolicyHelper

    Private ReadOnly _payPeriodService As IPayPeriodDataService

    Private ReadOnly _timeEntryDataService As ITimeEntryDataService

    Private ReadOnly _breakTimeBracketRepository As IBreakTimeBracketRepository

    Private ReadOnly _employeeRepository As IEmployeeRepository

    Private ReadOnly _payPeriodRepository As IPayPeriodRepository

    Private ReadOnly _roleRepository As IRoleRepository

    Private ReadOnly _timeAttendanceLogRepository As ITimeAttendanceLogRepository

    Private ReadOnly _userRepository As IAspNetUserRepository
    Private ReadOnly _systemOwnerService As ISystemOwnerService
    Private ReadOnly _organizationRepository As IOrganizationRepository

    Sub New()

        InitializeComponent()

        _policy = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

        _payPeriodService = MainServiceProvider.GetRequiredService(Of IPayPeriodDataService)

        _timeEntryDataService = MainServiceProvider.GetRequiredService(Of ITimeEntryDataService)

        _breakTimeBracketRepository = MainServiceProvider.GetRequiredService(Of IBreakTimeBracketRepository)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)

        _roleRepository = MainServiceProvider.GetRequiredService(Of IRoleRepository)

        _timeAttendanceLogRepository = MainServiceProvider.GetRequiredService(Of ITimeAttendanceLogRepository)

        _userRepository = MainServiceProvider.GetRequiredService(Of IAspNetUserRepository)

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of ISystemOwnerService)

        _organizationRepository = MainServiceProvider.GetRequiredService(Of IOrganizationRepository)

    End Sub

    Private Async Sub TimeEntrySummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _currentSystemOwner = Await _systemOwnerService.GetCurrentSystemOwnerEntityAsync()
        _organization = Await _organizationRepository.GetByIdWithAddressAsync(z_OrganizationID)

        _formHasLoaded = True

        employeesDataGridView.AutoGenerateColumns = False
        timeEntriesDataGridView.AutoGenerateColumns = False

        ' Default selected year is the current year
        _selectedYear = Date.Today.Year

        tsBtnDeleteTimeEntry.Visible = False
        RegenerateTimeEntryButton.Visible = False
        GenerateDefaultShiftAndTimeLogsButton.Visible = False

        Await LoadEmployees()
        Await LoadPayPeriods()

        _breakTimeBrackets = New List(Of BreakTimeBracket)
        If _policy.ComputeBreakTimeLate Then
            _breakTimeBrackets = Await GetBreakTimeBrackets()
        End If

        UpdateFormBaseOnPolicy()

        If Not _policy.UseUserLevel Then

            Await CheckRolePermissions()
        End If

        LoadYears()

        AddHandler employeesDataGridView.SelectionChanged, AddressOf employeesDataGridView_SelectionChanged
    End Sub

    Private Sub UpdateFormBaseOnPolicy()

        actualButton.Visible = _policy.ShowActual

        ' maybe show this always because employee can have different calendars if they are in different branches
        ColumnBranch.Visible = _policy.UseCostCenter

        GenerateDefaultShiftAndTimeLogsButton.Visible = _policy.UseDefaultShiftAndTimeLogs

        _generateDefaultShiftAndTimeLogsButtonHidden = Not _policy.UseDefaultShiftAndTimeLogs

        CheckIfMoneyColumnsAreGoingToBeHidden()
    End Sub

    Private Async Sub CheckIfMoneyColumnsAreGoingToBeHidden()
        Dim user = Await _userRepository.GetByIdAsync(z_User)

        If user Is Nothing Then

            MessageBoxHelper.ErrorMessage("Cannot read user data. Please log out and try to log in again.")
        End If

        If Not _policy.UseUserLevel Then Return

        _hideMoneyColumns = user.UserLevel <> UserLevel.One AndAlso
            user.UserLevel <> UserLevel.Two AndAlso
            user.UserLevel <> UserLevel.Three
    End Sub

    Private Async Function CheckRolePermissions() As Task

        USER_ROLE = Await _roleRepository.GetByUserAndOrganizationAsync(userId:=z_User, organizationId:=z_OrganizationID)

        Dim timeLogPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.TIMELOG).FirstOrDefault()
        Dim shiftPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.SHIFT).FirstOrDefault()
        Dim timeEntryPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.TIMEENTRY).FirstOrDefault()
        Dim payPeriodPermission = USER_ROLE?.RolePermissions?.Where(Function(r) r.Permission.Name = PermissionConstant.PAYPERIOD).FirstOrDefault()

        If timeLogPermission Is Nothing OrElse
            timeLogPermission.Create = False OrElse
            timeLogPermission.Update = False OrElse
            shiftPermission Is Nothing OrElse
            shiftPermission.Create = False OrElse
            shiftPermission.Update = False Then

            GenerateDefaultShiftAndTimeLogsButton.Visible = False

            _generateDefaultShiftAndTimeLogsButtonHidden = True

        End If

        Dim updateTimeEntryPermission = timeEntryPermission IsNot Nothing AndAlso timeEntryPermission.Update
        Dim deleteTimeEntryPermission = timeEntryPermission IsNot Nothing AndAlso timeEntryPermission.Delete

        If Not updateTimeEntryPermission Then
            RegenerateTimeEntryButton.Visible = False
            _regenerateTimeEntryButtonHidden = True
        End If

        If Not deleteTimeEntryPermission Then
            tsBtnDeleteTimeEntry.Visible = False
            _tsBtnDeleteTimeEntryHidden = True
        End If

        StartNewPayrollButton.Visible =
            If(timeEntryPermission?.Create, False) AndAlso
            If(payPeriodPermission?.Create, False)
    End Function

    Private Async Function GetBreakTimeBrackets() As Task(Of List(Of BreakTimeBracket))

        Return (Await _breakTimeBracketRepository.
            GetAllAsync(z_OrganizationID)).
            ToList()

    End Function

    Private Async Function LoadEmployees() As Task
        _employees = Await GetEmployeesWithPosition()
        employeesDataGridView.DataSource = _employees
    End Function

    Private Async Function GetEmployeesWithPosition() As Task(Of ICollection(Of Employee))
        Dim periodEndDate = If(_selectedPayPeriod Is Nothing, Date.Now(), _selectedPayPeriod.PayToDate)

        Dim unsortedList = Await _employeeRepository.GetAllWithinServicePeriodWithPositionAsync(
            organizationId:=z_OrganizationID,
            currentDate:=periodEndDate)
        Dim list = unsortedList.
            OrderBy(Function(e) e.LastName).
            ToList()
        Return CType(list, ICollection(Of Employee))

    End Function

    Public Async Function LoadPayPeriods() As Task

        If _formHasLoaded = False Then Return

        RemoveHandler payPeriodsDataGridView.SelectionChanged, AddressOf payPeriodDataGridView_SelectionChanged

        Dim currentSelectedPayPeriodInGridView = DirectCast(payPeriodsDataGridView.CurrentCell?.Value, PayPeriod)

        Dim payPeriods = Await GetPayPeriods(_selectedYear)

        Dim numOfRows = 2

        If _currentSystemOwner.IsMorningSun AndAlso
            payPeriods.Any(Function(p) p.IsWeekly) Then

            numOfRows = payPeriods.
                GroupBy(Function(p) p.Month).
                Max(Function(p) p.Count())
        End If

        payPeriodsDataGridView.Rows.Clear()
        payPeriodsDataGridView.Rows.Add(numOfRows)

        payPeriodsDataGridView.ClearSelection()

        Dim monthRowCounters(11) As Integer

        For Each payperiod In payPeriods
            Dim monthNo = payperiod.Month

            Dim columnIndex = monthNo - 1
            Dim rowIndex = monthRowCounters(columnIndex)

            Dim payFromDate = payperiod.PayFromDate.ToString("dd MMM")
            Dim payToDate = payperiod.PayToDate.ToString("dd MMM")
            Dim label = payFromDate + " - " + payToDate

            Dim currentCell = payPeriodsDataGridView.Rows(rowIndex).Cells(columnIndex)

            currentCell.Value = payperiod

            If payperiod.PayFromDate = currentSelectedPayPeriodInGridView?.PayFromDate Then
                currentCell.Selected = True
                ' To update the data of the pay period if it has changed.
                ' For example its status may have been updated.
                currentSelectedPayPeriodInGridView = DirectCast(payPeriodsDataGridView.CurrentCell?.Value, PayPeriod)
            End If

            If payperiod.IsClosed Then

                currentCell.Style.SelectionBackColor = SystemColors.Highlight
                currentCell.Style.BackColor = Color.White
                currentCell.Style.ForeColor = Color.Black

            ElseIf payperiod.IsOpen Then

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

        Await SetSelectedPeriodAndEmployee(numOfRows, currentSelectedPayPeriodInGridView, payPeriods)

        If Not _tsBtnDeleteTimeEntryHidden Then
            tsBtnDeleteTimeEntry.Visible = _selectedPayPeriod IsNot Nothing AndAlso _selectedPayPeriod.IsOpen

        End If

        If Not _regenerateTimeEntryButtonHidden Then
            RegenerateTimeEntryButton.Visible = _selectedPayPeriod IsNot Nothing AndAlso _selectedPayPeriod.IsOpen
        End If

        If Not _generateDefaultShiftAndTimeLogsButtonHidden Then
            GenerateDefaultShiftAndTimeLogsButton.Visible = _selectedPayPeriod IsNot Nothing AndAlso Not _selectedPayPeriod.IsClosed
        End If

        Await LoadTimeEntries()

        AddHandler payPeriodsDataGridView.SelectionChanged, AddressOf payPeriodDataGridView_SelectionChanged

    End Function

    Private Async Function SetSelectedPeriodAndEmployee(numOfRows As Integer, currentSelectedPayPeriodInGridView As PayPeriod, payPeriods As ICollection(Of PayPeriod)) As Task

        If _selectedEmployee Is Nothing AndAlso employeesDataGridView.CurrentRow IsNot Nothing Then

            _selectedEmployee = CType(employeesDataGridView.CurrentRow.DataBoundItem, Employee)

        End If

        If currentSelectedPayPeriodInGridView IsNot Nothing Then

            ' To update the data of the pay period if it has changed.
            ' For example its status may have been updated.
            _selectedPayPeriod = currentSelectedPayPeriodInGridView
        Else

            Dim currentlyWorkedOnPayPeriod = Await _payPeriodRepository.GetOpenOrCurrentPayPeriodAsync(
                organizationId:=z_OrganizationID,
                currentUserId:=z_User)

            _selectedPayPeriod = payPeriods.FirstOrDefault(Function(p) p.PayFromDate = currentlyWorkedOnPayPeriod.PayFromDate)

            If _selectedPayPeriod IsNot Nothing Then

                Dim rowIdx = (_selectedPayPeriod.OrdinalValue - 1) Mod numOfRows
                Dim payPeriodCell = payPeriodsDataGridView.Rows(rowIdx).Cells(_selectedPayPeriod.Month - 1)
                payPeriodsDataGridView.CurrentCell = payPeriodCell

            End If

        End If

    End Function

    Private Async Function GetPayPeriods(year As Integer) As Task(Of ICollection(Of PayPeriod))

        If _currentSystemOwner.IsMorningSun AndAlso
            _organization.IsWeekly Then

            Return (Await _payPeriodRepository.GetYearlyPayPeriodsOfWeeklyAsync(
                    organization:=_organization,
                    year:=year,
                    currentUserId:=z_User)).
                Select(Function(p) New PayPeriod(p)).
                ToList()
        End If

        Return (Await _payPeriodRepository.
            GetYearlyPayPeriodsAsync(
                organizationId:=z_OrganizationID,
                year:=year,
                currentUserId:=z_User)).
            Select(Function(p) New PayPeriod(p)).
            ToList()
    End Function

    Private Async Function LoadTimeEntries() As Task
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
    End Function

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

    Private Sub LoadYears()

        Dim years = Enumerable.Range(2010, 26).ToList()

        cboYears.ComboBox.DataSource = years
        cboYears.ComboBox.SelectedItem = _selectedYear

        AddHandler cboYears.SelectedIndexChanged, AddressOf cboYears_SelectedIndexChanged
    End Sub

    Private Async Function GetTimeEntries(employee As Employee, payPeriod As PayPeriod) As Task(Of ICollection(Of TimeEntry))

        Dim sql = <![CDATA[
            DROP TEMPORARY TABLE IF EXISTS `latesttimelogs`;
            CREATE TEMPORARY TABLE IF NOT EXISTS `latesttimelogs`
            SELECT RowID, `Date`, EmployeeID, MAX(LastUpd) `UpdatedTimeLog`
            FROM employeetimeentrydetails
            WHERE EmployeeID=@EmployeeID
            AND Date BETWEEN @DateFrom AND @DateTo
            GROUP BY `Date`
            ORDER BY `Date`, LastUpd DESC;

            SELECT
                ete.RowID,
                ete.Date,
                etd.TimeIn,
                etd.TimeOut,
                etd.RowID,
                IFNULL(shiftschedules.StartTime, NULL) AS ShiftFrom,
                IFNULL(shiftschedules.EndTime, NULL) AS ShiftTo,
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
                ete.SingleParentLeaveHours,
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
                etd.TimeStampIn,
                etd.TimeStampOut,
                l.LeaveStartTime,
                l.LeaveEndTime,
                IFNULL(shiftschedules.IsRestDay, FALSE) `IsRestDay`,
                ete.BranchID,
                branch.BranchName
            FROM employeetimeentry ete
            LEFT JOIN `latesttimelogs`
                ON `latesttimelogs`.EmployeeID = ete.EmployeeID AND
                    `latesttimelogs`.Date = ete.Date
            LEFT JOIN employeetimeentrydetails etd
                ON etd.Date = ete.Date AND
                    etd.OrganizationID = ete.OrganizationID AND
                    etd.EmployeeID = ete.EmployeeID AND
                    etd.RowID = `latesttimelogs`.RowID
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
            LEFT JOIN shiftschedules
                ON shiftschedules.EmployeeID = ete.EmployeeID AND
                    shiftschedules.`Date` = ete.`Date`

            LEFT JOIN branch
                ON ete.BranchID = branch.RowID

            WHERE ete.EmployeeID = @EmployeeID AND
                ete.`Date` BETWEEN @DateFrom AND @DateTo
            ORDER BY ete.`Date`;
        ]]>.Value

        Dim timeEntries = New Collection(Of TimeEntry)
        Dim calendarCollection As CalendarCollection = Await GetCalendarCollection(payPeriod)

        Using connection As New MySqlConnection(connectionString),
            command As New MySqlCommand(sql, connection)

            With command.Parameters
                .AddWithValue("@EmployeeID", employee.RowID)
                .AddWithValue("@DateFrom", payPeriod.PayFromDate)
                .AddWithValue("@DateTo", payPeriod.PayToDate)
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
                    .SingleParentLeaveHours = reader.GetValue(Of Decimal)("SingleParentLeaveHours"),
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
                    .IsRestDay = reader.GetValue(Of Boolean)("IsRestDay"),
                    .BranchID = reader.GetValue(Of Integer?)("BranchID"),
                    .BranchName = reader.GetValue(Of String)("BranchName")
                }

                timeEntry.GetHolidayType(calendarCollection)

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
                        .SingleParentLeaveHours += timeEntry.SingleParentLeaveHours
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
            DROP TEMPORARY TABLE IF EXISTS `latesttimelogs`;
            CREATE TEMPORARY TABLE IF NOT EXISTS `latesttimelogs`
            SELECT RowID, `Date`, EmployeeID, MAX(LastUpd) `UpdatedTimeLog`
            FROM employeetimeentrydetails
            WHERE EmployeeID=@EmployeeID
            AND Date BETWEEN @DateFrom AND @DateTo
            GROUP BY `Date`
            ORDER BY `Date`, LastUpd DESC;

            SELECT
                eta.RowID,
                eta.Date,
                employeetimeentrydetails.TimeIn,
                employeetimeentrydetails.TimeOut,
                IFNULL(shiftschedules.StartTime, NULL) AS ShiftFrom,
                IFNULL(shiftschedules.EndTime, NULL) AS ShiftTo,
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
                IFNULL(shiftschedules.IsRestDay, FALSE) `IsRestDay`,
                ete.BranchID,
                branch.BranchName
            FROM employeetimeentryactual eta
            LEFT JOIN employeetimeentry ete
                ON ete.EmployeeID = eta.EmployeeID AND
                    ete.Date = eta.Date
            LEFT JOIN `latesttimelogs`
                ON `latesttimelogs`.EmployeeID = ete.EmployeeID AND
                    `latesttimelogs`.Date = ete.Date
            LEFT JOIN employeetimeentrydetails
                ON employeetimeentrydetails.Date = eta.Date AND
                employeetimeentrydetails.OrganizationID = eta.OrganizationID AND
                employeetimeentrydetails.EmployeeID = eta.EmployeeID AND
                employeetimeentrydetails.RowID = `latesttimelogs`.RowID
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

            LEFT JOIN branch
                ON ete.BranchID = branch.RowID

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
            End With

            Await connection.OpenAsync()
            Dim reader = Await command.ExecuteReaderAsync()

            Dim totalTimeEntry = New TimeEntry()
            Dim calendarCollection As CalendarCollection = Await GetCalendarCollection(payPeriod)

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
                    .IsRestDay = reader.GetValue(Of Boolean)("IsRestDay"),
                    .BranchID = reader.GetValue(Of Integer?)("BranchID"),
                    .BranchName = reader.GetValue(Of String)("BranchName")
                }

                timeEntry.GetHolidayType(calendarCollection)

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

    Private Async Function GetCalendarCollection(payPeriod As PayPeriod) As Task(Of CalendarCollection)

        If payPeriod Is Nothing Then Return Nothing

        Dim calendarService = MainServiceProvider.GetRequiredService(Of ICalendarService)

        Return Await calendarService.GetCalendarCollectionAsync(New TimePeriod(payPeriod.PayFromDate, payPeriod.PayToDate))
    End Function

    Private Async Sub StartNewPayrollButton_Click(sender As Object, e As EventArgs) Handles StartNewPayrollButton.Click

        Dim currentOpenPayPeriod = Await _payPeriodRepository.GetCurrentOpenAsync(z_OrganizationID)
        If currentOpenPayPeriod IsNot Nothing Then
            MessageBoxHelper.Warning(PayPeriodDataService.HasCurrentlyOpenErrorMessage(currentOpenPayPeriod))
            Return
        End If

        Dim startDate As Date
        Dim endDate As Date
        Dim result As DialogResult
        Dim currentPayperiod As Entities.PayPeriod

        Using dialog = New StartNewPayPeriodDialog(_selectedPayPeriod)
            result = dialog.ShowDialog()

            If result = DialogResult.OK Then
                startDate = dialog.Start
                endDate = dialog.End
                currentPayperiod = dialog.CurrentPayperiod
            End If
        End Using

        If result = DialogResult.OK Then

            Dim payPeriod = Await _payPeriodRepository.GetAsync(organization:=_organization, payPeriod:=currentPayperiod)

            If payPeriod Is Nothing OrElse payPeriod.RowID Is Nothing OrElse Not payPeriod.IsOpen Then
                MessageBoxHelper.Warning("Pay period does not exists or is not ""Open"" yet.")
                Return

            End If

            Await GenerateTimeEntries(
                startDate:=startDate,
                endDate:=endDate,
                payPeriodId:=payPeriod.RowID.Value)
        End If

    End Sub

    Private Async Function GenerateTimeEntries(startDate As Date, endDate As Date, payPeriodId As Integer) As Task

        'We are using a fresh instance of EmployeeRepository
        Dim repository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)
        'later, we can let the user choose the employees that they want to generate.
        Dim employees = Await repository.GetAllActiveAsync(z_OrganizationID)

        Dim generator As New TimeEntryGeneration(employees, additionalProgressCount:=1, isMorningSun:=_currentSystemOwner.IsMorningSun)
        Dim progressDialog = New ProgressDialog(generator, "Generating time entries...")

        Dim payPeriod As New TimePeriod(startDate, endDate)

        MDIPrimaryForm.Enabled = False
        progressDialog.Show()

        generator.SetCurrentMessage("Loading resources...")
        GetResources(
            progressDialog,
            payPeriod,
            Sub(resourcesTask)

                If resourcesTask Is Nothing Then

                    HandleErrorLoadingResources(progressDialog)
                    Return
                End If

                generator.IncreaseProgress("Finished loading resources.")

                Dim generationTask = Task.Run(
                    Async Function()
                        Await generator.Start(resourcesTask.Result, payPeriod, payPeriodId)
                    End Function
                )

                generationTask.ContinueWith(
                    Sub() DoneGenerating(
                        progressDialog:=progressDialog,
                        results:=generator.Results),
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.FromCurrentSynchronizationContext
                )

                generationTask.ContinueWith(
                    Sub(t) TimeEntryGeneratorError(t, progressDialog),
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnFaulted,
                    TaskScheduler.FromCurrentSynchronizationContext
                )
            End Sub)

    End Function

    Private Async Sub DoneGenerating(progressDialog As ProgressDialog, results As IReadOnlyCollection(Of ProgressGenerator.IResult))

        CloseProgressDialog(progressDialog)

        Dim saveResults = results.Select(Function(r) CType(r, EmployeeResult)).ToList()

        Dim dialog = New EmployeeResultsDialog(
            saveResults,
            title:="Time Entry Results",
            generationDescription:="Time entry generation",
            entityDescription:="time entries") With {
            .Owner = Me
        }

        dialog.ShowDialog()
        Await LoadTimeEntries()

    End Sub

    Private Sub TimeEntryGeneratorError(t As Task, progressDialog As ProgressDialog)

        CloseProgressDialog(progressDialog)

        Const MessageTitle As String = "Generate Time Entries"

        If t.Exception?.InnerException.GetType() Is GetType(BusinessLogicException) Then

            MessageBoxHelper.ErrorMessage(t.Exception?.InnerException.Message, MessageTitle)
        Else
            Debugger.Break()
            MessageBoxHelper.DefaultErrorMessage(MessageTitle, t.Exception)
        End If

    End Sub

    Private Sub GetResources(progressDialog As ProgressDialog, payPeriod As TimePeriod, callBackAfterLoadResources As Action(Of Task(Of ITimeEntryResources)))
        Dim resources = MainServiceProvider.GetRequiredService(Of ITimeEntryResources)

        Dim loadTask = Task.Run(
            Function()
                Dim resourcesTask = resources.Load(
                    organizationId:=z_OrganizationID,
                    cutoffStart:=payPeriod.Start,
                    cutoffEnd:=payPeriod.End)

                resourcesTask.Wait()

                Return resources
            End Function)

        loadTask.ContinueWith(
            callBackAfterLoadResources,
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnRanToCompletion,
            TaskScheduler.FromCurrentSynchronizationContext
        )

        loadTask.ContinueWith(
            Sub(t) LoadingResourcesOnError(t, progressDialog),
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext
        )
    End Sub

    Private Sub LoadingResourcesOnError(t As Task, progressDialog As ProgressDialog)

        _logger.Error("Error loading one of the time entry data.", t.Exception)

        HandleErrorLoadingResources(progressDialog)

    End Sub

    Private Shared Sub HandleErrorLoadingResources(progressDialog As ProgressDialog)
        CloseProgressDialog(progressDialog)

        MsgBox("Something went wrong while loading the time entry data needed for calculation. Please contact Globagility Inc. for assistance.", MsgBoxStyle.OkOnly, "Time Entry Resources")
    End Sub

    Private Shared Sub CloseProgressDialog(progressDialog As ProgressDialog)

        MDIPrimaryForm.Enabled = True

        If progressDialog Is Nothing Then Return

        progressDialog.Close()
        progressDialog.Dispose()
    End Sub

    Private Async Sub RegenerateTimeEntryButton_Click(sender As Object, e As EventArgs) Handles RegenerateTimeEntryButton.Click

        Dim validate = Await _payPeriodService.ValidatePayPeriodActionAsync(_selectedPayPeriod.RowID)

        If validate = FunctionResult.Failed Then

            MessageBoxHelper.Warning(validate.Message)
            Return
        End If

        Await GenerateTimeEntries(
            startDate:=_selectedPayPeriod.PayFromDate,
            endDate:=_selectedPayPeriod.PayToDate,
            payPeriodId:=_selectedPayPeriod.RowID.Value)

    End Sub

    Private Async Sub employeesDataGridView_SelectionChanged(sender As Object, e As EventArgs)
        If employeesDataGridView.CurrentRow Is Nothing Then
            Return
        End If

        Dim employee = DirectCast(employeesDataGridView.CurrentRow.DataBoundItem, Employee)
        If employee Is _selectedEmployee Then
            Return
        End If

        _selectedEmployee = employee
        Await LoadTimeEntries()
    End Sub

    Private Async Sub payPeriodDataGridView_SelectionChanged(sender As Object, e As EventArgs)
        If payPeriodsDataGridView.CurrentRow Is Nothing Then
            Return
        End If

        Dim payPeriod = DirectCast(payPeriodsDataGridView.CurrentCell.Value, PayPeriod)
        If payPeriod Is _selectedPayPeriod Then
            Return
        End If

        _selectedPayPeriod = payPeriod

        Dim isOpen = _selectedPayPeriod IsNot Nothing AndAlso _selectedPayPeriod.IsOpen

        If Not _tsBtnDeleteTimeEntryHidden Then
            tsBtnDeleteTimeEntry.Visible = isOpen

        End If

        If Not _regenerateTimeEntryButtonHidden Then
            RegenerateTimeEntryButton.Visible = isOpen
        End If

        If Not _generateDefaultShiftAndTimeLogsButtonHidden Then
            GenerateDefaultShiftAndTimeLogsButton.Visible = _selectedPayPeriod IsNot Nothing AndAlso Not _selectedPayPeriod.IsClosed
        End If

        Await LoadTimeEntries()
    End Sub

    Private Sub tsbtnClose_Click(sender As Object, e As EventArgs) Handles tsbtnClose.Click
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

    Private Async Sub actualButtonn_Click(sender As Object, e As EventArgs) Handles actualButton.Click
        _isActual = Not _isActual

        actualButton.Checked = _isActual

        Await LoadTimeEntries()
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
        Public Property Year As Integer Implements IPayPeriod.Year
        Public Property Month As Integer Implements IPayPeriod.Month
        Public Property OrdinalValue As Integer
        Public Property Status As PayPeriodStatus
        Public Property IsFirstHalf As Boolean Implements IPayPeriod.IsFirstHalf
        Public ReadOnly Property IsWeekly As Boolean

        Sub New(payPeriodData As Entities.PayPeriod)

            RowID = payPeriodData.RowID
            PayFromDate = payPeriodData.PayFromDate
            PayToDate = payPeriodData.PayToDate
            Year = payPeriodData.Year
            Month = payPeriodData.Month
            OrdinalValue = payPeriodData.OrdinalValue
            Status = payPeriodData.Status
            IsFirstHalf = payPeriodData.IsFirstHalf
            IsWeekly = payPeriodData.IsWeekly
        End Sub

        Public Overrides Function ToString() As String
            Dim dateFrom = PayFromDate.ToString("MMM dd")
            Dim dateTo = PayToDate.ToString("MMM dd")

            Return dateFrom + " - " + dateTo
        End Function

        Public ReadOnly Property IsClosed As Boolean
            Get
                Return Status = PayPeriodStatus.Closed
            End Get
        End Property

        Public ReadOnly Property IsOpen As Boolean
            Get
                Return Status = PayPeriodStatus.Open
            End Get
        End Property

        Public ReadOnly Property IsPending As Boolean
            Get
                Return Status = PayPeriodStatus.Pending
            End Get
        End Property

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
        Public Property SingleParentLeaveHours As Decimal
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
        Public Property BranchID As Integer?
        Public Property BranchName As String

        Private _holidayType As String

        Public Property HolidayType As String
            Get
                Return _holidayType
            End Get
            Set(value As String)
                _holidayType = If(value = CalendarConstant.RegularDay, "", value)
            End Set
        End Property

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

            Dim abbreviatn As String = String.Empty
            If holidayName = CalendarConstant.RegularHoliday Then abbreviatn = "RHol"
            If holidayName = CalendarConstant.SpecialNonWorkingHoliday Then abbreviatn = "SHol"
            If holidayName = CalendarConstant.DoubleHoliday Then abbreviatn = "DHol"
            If holidayName = CalendarConstant.RegularDayAndSpecialHoliday Then abbreviatn = "RHol+SHol"
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

        Public Sub GetHolidayType(calendarCollection As CalendarCollection)

            If calendarCollection Is Nothing Then
                Me.HolidayType = CalendarConstant.RegularDay
                Return
            End If

            Dim currentPayRate = calendarCollection.
                GetCalendar(BranchID).
                Find(EntryDate.Value)

            If currentPayRate IsNot Nothing Then

                If TypeOf currentPayRate Is CalendarDay Then

                    Dim calendarDayName = DirectCast(currentPayRate, CalendarDay)?.DayType?.Name

                    If {CalendarConstant.DoubleHoliday,
                        CalendarConstant.RegularHoliday,
                        CalendarConstant.SpecialNonWorkingHoliday,
                        CalendarConstant.RegularDay,
                        CalendarConstant.RegularDayAndSpecialHoliday}.
                        Contains(calendarDayName) Then

                        Me.HolidayType = calendarDayName
                        Return
                    End If

                End If

                If currentPayRate.IsDoubleHoliday Then
                    Me.HolidayType = CalendarConstant.DoubleHoliday

                ElseIf currentPayRate.IsRegularHoliday Then
                    Me.HolidayType = CalendarConstant.RegularHoliday

                ElseIf currentPayRate.IsSpecialNonWorkingHoliday Then
                    Me.HolidayType = CalendarConstant.SpecialNonWorkingHoliday
                Else
                    Me.HolidayType = CalendarConstant.RegularDay
                End If
            Else
                Me.HolidayType = CalendarConstant.RegularDay
            End If

        End Sub

    End Class

    Private Sub cboYears_SelectedIndexChanged(sender As Object, e As EventArgs)
        _selectedYear = DirectCast(cboYears.SelectedItem, Integer)
        Dim task = LoadPayPeriods()
    End Sub

    Private Sub timeEntriesDataGridView_SelectionChangedAsync(sender As Object, e As EventArgs) Handles timeEntriesDataGridView.SelectionChanged

    End Sub

    Private Async Sub timeEntriesDataGridView_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles timeEntriesDataGridView.CellContentClick

        If _selectedEmployee?.RowID Is Nothing Then Return

        If e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then Return

        If _policy.ComputeBreakTimeLate = False Then Return

        Dim currentRow = timeEntriesDataGridView.Rows(e.RowIndex)
        Dim currentColumn = timeEntriesDataGridView.Columns(e.ColumnIndex)

        If currentRow Is Nothing OrElse currentColumn Is Nothing Then Return

        If currentColumn Is ColumnRegHrs Then

            Dim nullableCurrentDate =
                ObjectUtils.ToNullableDateTime(currentRow.Cells(ColumnDate.Index).Value)

            If nullableCurrentDate Is Nothing Then Return

            Dim currentDate = Convert.ToDateTime(nullableCurrentDate)

            Dim timeAttendanceLogs = Await _
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

        Dim timeAttendanceLogs = (Await _timeAttendanceLogRepository.
                                GetByDateAndEmployeeAsync(currentDate, _selectedEmployee.RowID.Value)).
                                ToList()

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

                    actualTimeIn = dateTimeIn.Value.
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

                    actualTimeOut = dateTimeOut.Value.
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

        If _policy.ComputeBreakTimeLate = False Then Return

        'format except the last row
        If e.RowIndex >= timeEntriesDataGridView.Rows.Count - 1 Then Return

        Dim currentColumn = timeEntriesDataGridView.Columns(e.ColumnIndex)

        If currentColumn Is ColumnRegHrs Then

            e.CellStyle.ForeColor = Color.Blue
            e.CellStyle.Font = New Font(e.CellStyle.Font, FontStyle.Underline)
        End If
    End Sub

    Private Async Sub tsBtnDeleteTimeEntry_ClickAsync(sender As Object, e As EventArgs) Handles tsBtnDeleteTimeEntry.Click

        Dim validate = Await _payPeriodService.ValidatePayPeriodActionAsync(_selectedPayPeriod.RowID)

        If validate = FunctionResult.Failed Then

            MessageBoxHelper.Warning(validate.Message)
            Return
        End If

        If _selectedEmployee?.RowID Is Nothing Then

            MessageBoxHelper.Warning("No selected employee!")
            Return

        End If

        Dim ask = String.Concat(
            $"Proceed deleting employee's [{_selectedEmployee.EmployeeNo}] time entries between ",
            _selectedPayPeriod.PayFromDate.ToShortDateString(),
            " and ",
            _selectedPayPeriod.PayToDate.ToShortDateString(),
            " ?")

        Dim askConfirmation = MessageBox.Show(ask, "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If askConfirmation = DialogResult.No Then Return

        Await FunctionUtils.TryCatchFunctionAsync("Delete Time Entries",
            Async Function()
                Await _timeEntryDataService.DeleteByEmployeeAsync(
                    employeeId:=_selectedEmployee.RowID.Value,
                    payPeriodId:=_selectedPayPeriod.RowID.Value,
                    currentlyLoggedInUserId:=z_User
                )

                MessageBoxHelper.Information("Time entries successfully deleted.")

                Await LoadTimeEntries()
            End Function)
    End Sub

    Private Function GetPayPeriodString() As String
        Return $"'{_selectedPayPeriod.PayFromDate.ToShortDateString()}' to '{_selectedPayPeriod.PayToDate.ToShortDateString()}'"
    End Function

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

    Private Async Sub GenerateDefaultShiftAndTimeLogsButton_Click(sender As Object, e As EventArgs) Handles GenerateDefaultShiftAndTimeLogsButton.Click

        If _selectedPayPeriod Is Nothing Then

            MessageBoxHelper.Warning("Select a pay period first.")
            Return

        End If

        Dim form = New DefaultShiftAndTimeLogsForm(_selectedPayPeriod)
        form.ShowDialog()

        If form.NewPayPeriod IsNot Nothing Then
            Await LoadPayPeriods()
        End If

    End Sub

    Private Sub UserActivityToolStripButton_Click(sender As Object, e As EventArgs) Handles UserActivityToolStripButton.Click

        Dim formEntityName As String = "Time Entry"

        Dim userActivity As New UserActivityForm(formEntityName)
        userActivity.ShowDialog()
    End Sub

End Class
