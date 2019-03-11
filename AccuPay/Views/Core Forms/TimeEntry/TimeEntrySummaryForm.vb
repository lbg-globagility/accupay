Option Strict On

Imports System.Collections.ObjectModel
Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports AccuPay.Repository
Imports AccuPay.Utils
Imports log4net
Imports Microsoft.EntityFrameworkCore
Imports MySql.Data.MySqlClient
Imports Simplified = AccuPay.SimplifiedEntities.GridView

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

    Private _employees As ICollection(Of Simplified.Employee)

    Private _selectedEmployee As Simplified.Employee

    Private _selectedPayPeriod As PayPeriod

    Private _isActual As Boolean = False

    Private _isAmPm As Boolean = False

    Private _currentTimeEntryDate As Date

    Private WithEvents timeEntDurationModal As TimEntduration

    Private _employeeRepository As EmployeeRepository

    Private _calculateBreakTimeLateHours As Boolean

    Private Sub TimeEntrySummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        employeesDataGridView.AutoGenerateColumns = False

        ' Default selected year is the current year
        _selectedYear = Date.Today.Year

        _employeeRepository = New EmployeeRepository

        _calculateBreakTimeLateHours = GetBreakTimeLateHoursPolicy()

        Dim loadEmployeesTask = LoadEmployees()
        Dim loadPayPeriodsTask = LoadPayPeriods()
        LoadYears()
    End Sub

    Private Function GetBreakTimeLateHoursPolicy() As Boolean
        Using context = New PayrollContext()

            Dim settings = New ListOfValueCollection(context.ListOfValues.ToList())

            Dim policy = New TimeEntryPolicy(settings)

            Return policy.BreakTimeLateHours
        End Using
    End Function

    Private Async Function LoadEmployees() As Task
        _employees = Await GetEmployees()
        employeesDataGridView.DataSource = _employees

        'If _selectedEmployee Is Nothing Then
        '    _selectedEmployee = _employees.FirstOrDefault()
        'End If
    End Function

    Private Async Function GetEmployees() As Task(Of ICollection(Of Simplified.Employee))

        Dim list = Await _employeeRepository.GetAll(Of Simplified.Employee)()
        Return CType(list, ICollection(Of Simplified.Employee))

    End Function

    Public Async Function LoadPayPeriods() As Task
        Dim numOfRows = 2

        _payPeriods = Await GetPayPeriods(z_OrganizationID, _selectedYear, 1)
        payPeriodsDataGridView.Rows.Add(numOfRows)

        Dim monthRowCounters(11) As Integer

        For Each payperiod In Me._payPeriods
            Dim monthNo = payperiod.Month
            Dim counter = monthRowCounters(monthNo - 1)

            Dim payFromDate = payperiod.PayFromDate.ToString("dd MMM")
            Dim payToDate = payperiod.PayToDate.ToString("dd MMM")
            Dim label = payFromDate + " - " + payToDate

            payPeriodsDataGridView.Rows(counter).Cells(monthNo - 1).Value = payperiod

            counter += 1
            monthRowCounters(monthNo - 1) = counter
        Next

        If _selectedPayPeriod Is Nothing Then
            Dim dateToday = DateTime.Today

            _selectedPayPeriod = _payPeriods.FirstOrDefault(
                Function(payPeriod)
                    Return (payPeriod.PayFromDate <= dateToday) And (payPeriod.PayToDate >= dateToday)
                End Function
            )

            Dim rowIdx = (_selectedPayPeriod.OrdinalValue - 1) Mod numOfRows
            Dim payPeriodCell = payPeriodsDataGridView.Rows(rowIdx).Cells(_selectedPayPeriod.Month - 1)
            payPeriodsDataGridView.CurrentCell = payPeriodCell
        End If

        If _selectedPayPeriod IsNot payPeriodsDataGridView.CurrentCell Then
            _selectedPayPeriod = DirectCast(payPeriodsDataGridView.CurrentCell.Value, PayPeriod)
            LoadTimeEntries()
        End If
    End Function

    Private Async Function GetPayPeriods(organizationID As Integer,
                                         year As Integer,
                                         salaryType As Integer) As Task(Of ICollection(Of PayPeriod))
        Dim sql = <![CDATA[
            SELECT PayFromDate, PayToDate, Year, Month, OrdinalValue
            FROM payperiod
            WHERE payperiod.OrganizationID = @OrganizationID
                AND payperiod.Year = @Year
                AND payperiod.TotalGrossSalary = @SalaryType;
        ]]>.Value

        Dim payPeriods = New Collection(Of PayPeriod)

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
                    .PayFromDate = reader.GetValue(Of Date)("PayFromDate"),
                    .PayToDate = reader.GetValue(Of Date)("PayToDate"),
                    .Year = reader.GetValue(Of Integer)("Year"),
                    .Month = reader.GetValue(Of Integer)("Month"),
                    .OrdinalValue = reader.GetValue(Of Integer)("OrdinalValue")
                }

                payPeriods.Add(payPeriod)
            End While
        End Using

        Return payPeriods
    End Function

    Public Async Sub LoadTimeEntries()
        If _selectedEmployee Is Nothing Or _selectedPayPeriod Is Nothing Then
            Return
        End If

        timeEntriesDataGridView.AutoGenerateColumns = False

        Dim timeEntries As ICollection(Of TimeEntry)
        If _isActual Then
            timeEntries = Await GetActualTimeEntries(_selectedEmployee, _selectedPayPeriod)
        Else
            timeEntries = Await GetTimeEntries(_selectedEmployee, _selectedPayPeriod)
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
        ColumnNDiffPay.Visible = timeEntries.Any(Function(t) t.NightDiffAmount > 0)

        ColumnNDiffOTHrs.Visible = timeEntries.Any(Function(t) t.NightDiffOTHours > 0)
        ColumnNDiffOTPay.Visible = timeEntries.Any(Function(t) t.NightDiffOTAmount > 0)

        ColumnRDayHrs.Visible = timeEntries.Any(Function(t) t.RestDayHours > 0)
        ColumnRDayPay.Visible = timeEntries.Any(Function(t) t.RestDayAmount > 0)

        ColumnRDayOTHrs.Visible = timeEntries.Any(Function(t) t.RestDayOTHours > 0)
        ColumnRDayOTPay.Visible = timeEntries.Any(Function(t) t.RestDayOTPay > 0)

        ColumnSHolHrs.Visible = timeEntries.Any(Function(t) t.SpecialHolidayHours > 0)
        ColumnSHolPay.Visible = timeEntries.Any(Function(t) t.SpecialHolidayPay > 0)

        ColumnSHolOTHrs.Visible = timeEntries.Any(Function(t) t.SpecialHolidayOTHours > 0)
        ColumnSHolOTPay.Visible = timeEntries.Any(Function(t) t.SpecialHolidayOTPay > 0)

        ColumnRHolHrs.Visible = timeEntries.Any(Function(t) t.RegularHolidayHours > 0)
        ColumnRHolPay.Visible = timeEntries.Any(Function(t) t.RegularHolidayPay > 0)

        ColumnRHolOTHrs.Visible = timeEntries.Any(Function(t) t.RegularHolidayOTHours > 0)
        ColumnRHolOTPay.Visible = timeEntries.Any(Function(t) t.RegularHolidayOTPay > 0)

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

    Private Async Function GetTimeEntries(employee As Simplified.Employee, payPeriod As PayPeriod) As Task(Of ICollection(Of TimeEntry))
        'WARN: this has a possibility to show wrong data since
        'we are joining employeetimeentrydetails by LastUpd
        'maybe this query should be replaced

        Dim sql = <![CDATA[
            SELECT
                ete.RowID,
                ete.Date,
                etd.TimeIn,
                etd.TimeOut,
                etd.RowID,
                shift.TimeFrom AS ShiftFrom,
                shift.TimeTo AS ShiftTo,
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
                payrate.PayType,
                etd.TimeStampIn,
                etd.TimeStampOut
            FROM employeetimeentry ete
            LEFT JOIN (
                SELECT EmployeeID, Date, MAX(LastUpd) LastUpd, RowID
                FROM employeetimeentrydetails
                WHERE Date BETWEEN @DateFrom AND @DateTo
                GROUP BY EmployeeID, Date
            ) latest
            ON latest.EmployeeID = ete.EmployeeID AND
                latest.Date = ete.Date
            LEFT JOIN employeetimeentrydetails etd
            ON etd.Date = ete.Date AND
                etd.OrganizationID = ete.OrganizationID AND
                etd.EmployeeID = ete.EmployeeID AND
                etd.LastUpd = latest.LastUpd
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
            LEFT JOIN shift
            ON shift.RowID = employeeshift.ShiftID
            LEFT JOIN payrate
            ON payrate.Date = ete.Date AND
                payrate.OrganizationID = ete.OrganizationID
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
                    .TotalDayPay = reader.GetValue(Of Decimal)("TotalDayPay")
                }

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

                timeEntries.Add(timeEntry)
            End While

            timeEntries.Add(totalTimeEntry)
        End Using

        Return timeEntries
    End Function

    Private Async Function GetActualTimeEntries(employee As Simplified.Employee, payPeriod As PayPeriod) As Task(Of ICollection(Of TimeEntry))
        Dim sql = <![CDATA[
            SELECT
                eta.RowID,
                eta.Date,
                employeetimeentrydetails.TimeIn,
                employeetimeentrydetails.TimeOut,
                shift.TimeFrom AS ShiftFrom,
                shift.TimeTo AS ShiftTo,
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
                employeetimeentrydetails.TimeStampOut
            FROM employeetimeentryactual eta
            LEFT JOIN employeetimeentry ete
            ON ete.EmployeeID = eta.EmployeeID AND
                ete.Date = eta.Date
            LEFT JOIN (
                SELECT EmployeeID, Date, MAX(LastUpd) LastUpd, Created
                FROM employeetimeentrydetails
                WHERE Date BETWEEN @DateFrom AND @DateTo
                GROUP BY EmployeeID, Date
            ) latest
            ON latest.EmployeeID = eta.EmployeeID AND
                latest.Date = eta.Date
            LEFT JOIN employeetimeentrydetails
            ON employeetimeentrydetails.Date = eta.Date AND
                employeetimeentrydetails.OrganizationID = eta.OrganizationID AND
                employeetimeentrydetails.EmployeeID = eta.EmployeeID AND
                employeetimeentrydetails.LastUpd = latest.LastUpd
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
            LEFT JOIN employeeofficialbusiness ofb
            ON ofb.OffBusStartDate = eta.Date AND
                ofb.EmployeeID = eta.EmployeeID AND
                latestOb.Created = ofb.Created
            LEFT JOIN shift
            ON shift.RowID = employeeshift.ShiftID
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
                    .TotalDayPay = reader.GetValue(Of Decimal)("TotalDayPay")
                }

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

                timeEntries.Add(timeEntry)
            End While

            timeEntries.Add(totalTimeEntry)
        End Using

        Return timeEntries
    End Function

    Private Sub generateTimeEntryButton_Click(sender As Object, e As EventArgs) Handles generateTimeEntryButton.Click
        Dim startDate As Date
        Dim endDate As Date
        Dim result As DialogResult

        Using dialog = New DateRangePickerDialog()
            result = dialog.ShowDialog()

            If result = DialogResult.OK Then
                startDate = dialog.Start
                endDate = dialog.End
            End If
        End Using

        If result = DialogResult.OK Then
            Dim generator = New TimeEntryGenerator(startDate, endDate)
            Dim progressDialog = New TimeEntryProgressDialog(generator)

            Task.Run(Sub() generator.Start()).
                ContinueWith(
                    Sub() DoneGenerating(progressDialog, generator),
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.FromCurrentSynchronizationContext)

            progressDialog.Show()
        End If
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

        Dim employee = DirectCast(employeesDataGridView.CurrentRow.DataBoundItem, Simplified.Employee)
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

        If time > HoursPerDay Then
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

        Public Property PayFromDate As Date
        Public Property PayToDate As Date
        Public Property Year As Integer
        Public Property Month As Integer
        Public Property OrdinalValue As Integer

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
            Dim currentRow = timeEntriesDataGridView.CurrentRow

            Dim dateTimeValue = currentRow?.Cells(ColumnDate.Name).Value

            Dim hasDateSelected = dateTimeValue IsNot Nothing

            If hasDateSelected Then
                Dim dateValue = DirectCast(dateTimeValue, Date)
                Dim employeeRowId = Convert.ToInt32(_selectedEmployee.RowID)

                Using context = New PayrollContext()

                    Dim eTimeEntry = Await context.TimeEntries.
                            Where(Function(et) Nullable.Equals(et.EmployeeID, employeeRowId)).
                            Where(Function(et) et.Date = dateValue).FirstOrDefaultAsync()

                    Dim shiftRecord = Await context.ShiftSchedules.FindAsync(eTimeEntry.EmployeeShiftID)

                    If shiftRecord IsNot Nothing Then
                        Dim balloon As New ToolTip() With {
                            .ToolTipTitle = "Delete shift schedule",
                            .UseFading = True,
                            .UseAnimation = True,
                            .ShowAlways = True,
                            .IsBalloon = True,
                            .ToolTipIcon = ToolTipIcon.Info
                        }
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
            End If
        Else
            _currentTimeEntryDate = Date.Now
        End If
    End Sub

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

            Dim form As New TimeAttendanceLogListForm(timeAttendanceLogs)
            form.ShowDialog()

        End If

    End Sub

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
End Class
