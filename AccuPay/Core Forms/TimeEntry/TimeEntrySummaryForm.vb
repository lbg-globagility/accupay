Option Strict On

Imports MySql.Data.MySqlClient
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports log4net

Public Class TimeEntrySummaryForm

    Private Shared _logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Shared HoursPerDay As TimeSpan = New TimeSpan(24, 0, 0)

    Private _selectedYear As Integer

    Private _weeklyPayPeriods As ICollection(Of PayPeriod)

    Private _semiMonthlyPayPeriods As ICollection(Of PayPeriod)

    Private _payPeriods As ICollection(Of PayPeriod)

    Private _employees As ICollection(Of Employee)

    Private _selectedEmployee As Employee

    Private _selectedPayPeriod As PayPeriod

    Private _declared As Boolean = True

    Private WithEvents timeEntDurationModal As TimEntduration

    Private Sub TimeEntrySummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Default selected year is the current year
        _selectedYear = Date.Today.Year

        Dim loadEmployeesTask = LoadEmployees()
        Dim loadPayPeriodsTask = LoadPayPeriods()
        LoadYears()
    End Sub

    Private Async Function LoadEmployees() As Task
        _employees = Await GetEmployees()
        employeesDataGridView.DataSource = _employees

        'If _selectedEmployee Is Nothing Then
        '    _selectedEmployee = _employees.FirstOrDefault()
        'End If
    End Function

    Private Async Function GetEmployees() As Task(Of ICollection(Of Employee))
        Dim sql = <![CDATA[
            SELECT
                employee.RowID,
                employee.EmployeeID,
                employee.FirstName,
                employee.LastName
            FROM employee
            WHERE employee.OrganizationID = @OrganizationID
            ORDER BY
                employee.LastName,
                employee.FirstName
        ]]>.Value

        Dim employees = New Collection(Of Employee)

        Using connection As New MySqlConnection(connectionString),
            command As New MySqlCommand(sql, connection)

            command.Parameters.AddWithValue("@OrganizationID", CStr(z_OrganizationID))

            Await connection.OpenAsync()
            Dim reader = Await command.ExecuteReaderAsync()
            While Await reader.ReadAsync()
                Dim rowID = reader.GetValue(Of String)("RowID")
                Dim employeeID = reader.GetValue(Of String)("EmployeeID")
                Dim firstName = reader.GetValue(Of String)("FirstName")
                Dim lastName = reader.GetValue(Of String)("LastName")

                Dim employee = New Employee() With {
                    .RowID = rowID,
                    .EmployeeID = employeeID,
                    .FirstName = firstName,
                    .LastName = lastName
                }

                employees.Add(employee)
            End While
        End Using

        Return employees
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

        If _declared Then
            timeEntriesDataGridView.DataSource = Await GetTimeEntries(_selectedEmployee, _selectedPayPeriod)
        Else
            timeEntriesDataGridView.DataSource = Await GetActualTimeEntries(_selectedEmployee, _selectedPayPeriod)
        End If
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
                employeetimeentry.RowID,
                employeetimeentry.Date,
                employeetimeentrydetails.TimeIn,
                employeetimeentrydetails.TimeOut,
                shift.TimeFrom AS ShiftFrom,
                shift.TimeTo AS ShiftTo,
                employeetimeentry.RegularHoursWorked,
                employeetimeentry.RegularHoursAmount,
                employeetimeentry.NightDifferentialHours,
                employeetimeentry.NightDiffHoursAmount,
                employeetimeentry.OvertimeHoursWorked,
                employeetimeentry.OvertimeHoursAmount,
                employeetimeentry.NightDifferentialOTHours,
                employeetimeentry.NightDiffOTHoursAmount,
                employeetimeentry.RestDayHours,
                employeetimeentry.RestDayAmount,
                employeetimeentry.RestDayOTHours,
                employeetimeentry.RestDayOTPay,
                employeetimeentry.LeavePayment,
                employeetimeentry.HoursLate,
                employeetimeentry.HoursLateAmount,
                employeetimeentry.UndertimeHours,
                employeetimeentry.UndertimeHoursAmount,
                employeetimeentry.VacationLeaveHours,
                employeetimeentry.SickLeaveHours,
                employeetimeentry.OtherLeaveHours,
                employeetimeentry.Leavepayment,
                employeetimeentry.SpecialHolidayHours,
                employeetimeentry.SpecialHolidayPay,
                employeetimeentry.SpecialHolidayOTHours,
                employeetimeentry.SpecialHolidayOTPay,
                employeetimeentry.RegularHolidayHours,
                employeetimeentry.RegularHolidayPay,
                employeetimeentry.RegularHolidayOTHours,
                employeetimeentry.RegularHolidayOTPay,
                employeetimeentry.HolidayPayAmount,
                employeetimeentry.AbsentHours,
                employeetimeentry.Absent,
                employeetimeentry.TotalHoursWorked,
                employeetimeentry.TotalDayPay,
                ofb.OffBusStartTime,
                ofb.OffBusEndTime,
                ot.OTStartTime,
                ot.OTEndTIme,
                payrate.PayType
            FROM employeetimeentry
            LEFT JOIN (
                SELECT EmployeeID, Date, MAX(Created) Created
                FROM employeetimeentrydetails
                WHERE Date BETWEEN @DateFrom AND @DateTo
                GROUP BY EmployeeID, Date
            ) latest
            ON latest.EmployeeID = employeetimeentry.EmployeeID AND
                latest.Date = employeetimeentry.Date
            LEFT JOIN employeetimeentrydetails
            ON employeetimeentrydetails.Date = employeetimeentry.Date AND
                employeetimeentrydetails.OrganizationID = employeetimeentry.OrganizationID AND
                employeetimeentrydetails.EmployeeID = employeetimeentry.EmployeeID AND
                employeetimeentrydetails.Created = latest.Created
            LEFT JOIN employeeshift
            ON employeeshift.RowID = employeetimeentry.EmployeeShiftID
            LEFT JOIN employeeofficialbusiness ofb
            ON ofb.OffBusStartDate = employeetimeentry.Date AND
                ofb.EmployeeID = employeetimeentry.EmployeeID
            LEFT JOIN employeeovertime ot
            ON ot.OTStartDate = employeetimeentry.Date AND
                ot.EmployeeID = employeetimeentry.EmployeeID AND
                ot.OTStatus = 'Approved'
            LEFT JOIN shift
            ON shift.RowID = employeeshift.ShiftID
            LEFT JOIN payrate
            ON payrate.Date = employeetimeentry.Date AND
                payrate.OrganizationID = employeetimeentry.OrganizationID
            WHERE employeetimeentry.EmployeeID = @EmployeeID AND
                employeetimeentry.`Date` BETWEEN @DateFrom AND @DateTo
            ORDER BY employeetimeentry.`Date`;
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

    Private Async Function GetActualTimeEntries(employee As Employee, payPeriod As PayPeriod) As Task(Of ICollection(Of TimeEntry))
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
                ofb.OffBusEndTime
            FROM employeetimeentryactual eta
            LEFT JOIN employeetimeentry ete
            ON ete.EmployeeID = eta.EmployeeID AND
                ete.Date = eta.Date
            LEFT JOIN (
                SELECT EmployeeID, Date, MAX(Created) Created
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
                employeetimeentrydetails.Created = latest.Created
            LEFT JOIN employeeshift
            ON employeeshift.RowID = eta.EmployeeShiftID
            LEFT JOIN employeeofficialbusiness ofb
            ON ofb.OffBusStartDate = eta.Date AND
                ofb.EmployeeID = eta.EmployeeID
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
            generator.Start()
            MsgBox("Finished generation.")
        End If

        Dim progressDialog = New TimeEntryProgressDialog()
        progressDialog.Show()
    End Sub

    Private Sub DoneGenerating() Handles timeEntDurationModal.DoneGenerating
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

        Dim matchCriteria =
            Function(employee As Employee) As Boolean
                Dim containsEmployeeId = employee.EmployeeID.ToLower().Contains(searchValue)
                Dim containsFullName = employee.FullName.ToLower().Contains(searchValue)

                Dim reverseFullName = employee.LastName.ToLower() + " " + employee.FirstName.ToLower()
                Dim containsFullNameInReverse = reverseFullName.Contains(searchValue)

                Return containsEmployeeId Or containsFullName Or containsFullNameInReverse
            End Function

        Dim filteredTask = Task.Factory.StartNew(
            Function() New BindingList(Of Employee)(
                _employees.Where(matchCriteria).ToList()
            )
        )

        employeesDataGridView.DataSource = Await filteredTask
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

    Private Sub declaredButton_Click(sender As Object, e As EventArgs) Handles declaredButton.Click
        _declared = Not _declared

        If _declared Then
            declaredButton.Text = "Actual"
        Else
            declaredButton.Text = "Declared"
        End If

        LoadTimeEntries()
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

    Private Class Employee

        Public Property RowID As String
        Public Property EmployeeID As String
        Public Property FirstName As String
        Public Property LastName As String

        Public Function FullName() As String
            Return FirstName + " " + LastName
        End Function

        Public Overrides Function ToString() As String
            Return EmployeeID
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

End Class
