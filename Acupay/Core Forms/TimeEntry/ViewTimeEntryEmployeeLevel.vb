Option Strict On

Imports MySql.Data.MySqlClient
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports log4net

Public Class ViewTimeEntryEmployeeLevel

    Private Shared _logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Shared HoursPerDay As TimeSpan = New TimeSpan(24, 0, 0)

    Private _weeklyPayPeriods As ICollection(Of PayPeriod)

    Private _semiMonthlyPayPeriods As ICollection(Of PayPeriod)

    Private _payPeriods As ICollection(Of PayPeriod)

    Private _employees As ICollection(Of Employee)

    Private _organizations As ICollection(Of Organization)

    Private _selectedEmployee As Employee

    Private _selectedPayPeriod As PayPeriod

    Private _declared As Boolean = True

    Private selected_og_rowid As String = String.Empty

    Private WithEvents timeEntDurationModal As TimEntduration

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ViewTimeEntryEmployeeLevel()
    End Sub

    Sub ViewTimeEntryEmployeeLevel()

        cboOrganizations.ContextMenu = New ContextMenu

        Size = New Size(305, 186)

    End Sub

    Private Sub TimeEntrySummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim loadOrganizationtask = LoadOrganizations()

        Dim loadEmployeesTask = LoadEmployees()

        Dim loadPayPeriodsTask = LoadPayPeriods()

    End Sub

    Private Async Function LoadEmployees() As Task
        _employees = Await GetEmployees()
        employeesDataGridView.DataSource = _employees

        'If _selectedEmployee Is Nothing Then
        '    _selectedEmployee = _employees.FirstOrDefault()
        'End If
    End Function

    Private Async Function LoadOrganizations() As Task
        _organizations = Await GetOrganizations()
        cboOrganizations.DataSource = _organizations

    End Function

    Private Async Function GetEmployees() As Task(Of ICollection(Of Employee))
        Dim sql = <![CDATA[
            SELECT
                employee.RowID,
                employee.EmployeeID,
                employee.FirstName,
                employee.LastName,
                employee.OrganizationID
            FROM employee
            ORDER BY
                employee.LastName,
                employee.FirstName
        ]]>.Value

        Dim employees = New Collection(Of Employee)

        Using connection As New MySqlConnection(connectionString),
            command As New MySqlCommand(sql, connection)

            Await connection.OpenAsync()
            Dim reader = Await command.ExecuteReaderAsync()
            While Await reader.ReadAsync()
                Dim rowID = reader.GetValue(Of String)("RowID")
                Dim employeeID = reader.GetValue(Of String)("EmployeeID")
                Dim firstName = reader.GetValue(Of String)("FirstName")
                Dim lastName = reader.GetValue(Of String)("LastName")
                Dim og_rowid = reader.GetValue(Of String)("OrganizationID")

                Dim employee = New Employee() With {
                    .RowID = rowID,
                    .EmployeeID = employeeID,
                    .FirstName = firstName,
                    .LastName = lastName,
                    .OrganizationID = og_rowid
                }

                employees.Add(employee)
            End While
        End Using

        Return employees
    End Function

    Private Async Function GetOrganizations() As Task(Of ICollection(Of Organization))
        Dim sql = <![CDATA[
            SELECT og.RowID
            ,og.Name
            FROM organization og
            WHERE og.NoPurpose = @is_nopurpose
        ]]>.Value

        Dim organizations = New Collection(Of Organization)

        Using connection As New MySqlConnection(connectionString),
            command As New MySqlCommand(sql, connection)

            command.Parameters.AddWithValue("@is_nopurpose", "0")

            Await connection.OpenAsync()
            Dim reader = Await command.ExecuteReaderAsync()
            While Await reader.ReadAsync()

                Dim og_rowid = reader.GetValue(Of String)("RowID")
                Dim og_name = reader.GetValue(Of String)("Name")

                Dim og =
                    New Organization() _
                    With {.RowID = og_rowid,
                          .Name = og_name}

                organizations.Add(og)

            End While
        End Using

        Return organizations

    End Function

    Public Async Function LoadPayPeriods() As Task
        Dim numOfRows = 2

        _payPeriods = Await GetPayPeriods(z_OrganizationID, 2017, 1)
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
                employeetimeentry.LeavePayment,
                employeetimeentry.HoursLate,
                employeetimeentry.HoursLateAmount,
                employeetimeentry.UndertimeHours,
                employeetimeentry.UndertimeHoursAmount,
                employeetimeentry.Leavepayment,
                employeetimeentry.HolidayPayAmount,
                employeetimeentry.Absent,
                employeetimeentry.TotalHoursWorked,
                employeetimeentry.TotalDayPay,
                payrate.PayType
            FROM employeetimeentry
            LEFT JOIN employeetimeentrydetails
            ON employeetimeentrydetails.Date = employeetimeentry.Date AND
                employeetimeentrydetails.OrganizationID = employeetimeentry.OrganizationID AND
                employeetimeentrydetails.EmployeeID = employeetimeentry.EmployeeID
            LEFT JOIN employeeshift
            ON employeeshift.RowID = employeetimeentry.EmployeeShiftID
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

    Private Async Function GetActualTimeEntries(employee As Employee, payPeriod As PayPeriod) As Task(Of ICollection(Of TimeEntry))
        Dim sql = <![CDATA[
            SELECT
                employeetimeentryactual.RowID,
                employeetimeentryactual.Date,
                employeetimeentrydetails.TimeIn,
                employeetimeentrydetails.TimeOut,
                shift.TimeFrom AS ShiftFrom,
                shift.TimeTo AS ShiftTo,
                employeetimeentryactual.RegularHoursWorked,
                employeetimeentryactual.RegularHoursAmount,
                employeetimeentryactual.NightDifferentialHours,
                employeetimeentryactual.NightDiffHoursAmount,
                employeetimeentryactual.OvertimeHoursWorked,
                employeetimeentryactual.OvertimeHoursAmount,
                employeetimeentryactual.NightDifferentialOTHours,
                employeetimeentryactual.NightDiffOTHoursAmount,
                employeetimeentryactual.RestDayHours,
                employeetimeentryactual.RestDayAmount,
                employeetimeentryactual.LeavePayment,
                employeetimeentryactual.HoursLate,
                employeetimeentryactual.HoursLateAmount,
                employeetimeentryactual.UndertimeHours,
                employeetimeentryactual.UndertimeHoursAmount,
                employeetimeentryactual.Leavepayment,
                employeetimeentryactual.HolidayPayAmount,
                employeetimeentryactual.Absent,
                employeetimeentryactual.TotalHoursWorked,
                employeetimeentryactual.TotalDayPay
            FROM employeetimeentryactual
            LEFT JOIN employeetimeentrydetails
            ON employeetimeentrydetails.Date = employeetimeentryactual.Date AND
                employeetimeentrydetails.OrganizationID = employeetimeentryactual.OrganizationID AND
                employeetimeentrydetails.EmployeeID = employeetimeentryactual.EmployeeID
            LEFT JOIN employeeshift
            ON employeeshift.RowID = employeetimeentryactual.EmployeeShiftID
            LEFT JOIN shift
            ON shift.RowID = employeeshift.ShiftID
            WHERE employeetimeentryactual.EmployeeID = @EmployeeID AND
                employeetimeentryactual.`Date` BETWEEN @DateFrom AND @DateTo
            ORDER BY employeetimeentryactual.`Date`;
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

    Private Sub generateTimeEntryButton_Click(sender As Object, e As EventArgs)
        Return
        timeEntDurationModal = New TimEntduration(DateTime.Today)

        timeEntDurationModal.ShowDialog()
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

    Private Sub tsbtnCloseempawar_Click(sender As Object, e As EventArgs)
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
                'Dim containsEmployeeId = employee.EmployeeID.ToLower().Contains(searchValue)
                Dim containsEmployeeId = (employee.EmployeeID = searchValue)
                Dim containsFullName = employee.FullName.ToLower().Contains(searchValue)

                Dim reverseFullName = employee.LastName.ToLower() + " " + employee.FirstName.ToLower()
                Dim containsFullNameInReverse = reverseFullName.Contains(searchValue)

                Dim containsOrganizationID = (employee.OrganizationID = selected_og_rowid)

                Return (containsEmployeeId And containsOrganizationID)
                'Or containsFullName Or containsFullNameInReverse
            End Function

        Dim filteredTask = Task.Factory.StartNew(
            Function() New BindingList(Of Employee)(
                _employees.Where(matchCriteria).ToList()
            )
        )

        employeesDataGridView.DataSource = Await filteredTask
        employeesDataGridView.Update()

        btnRerfresh.Enabled = (employeesDataGridView.RowCount > 0)

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

    Private Sub declaredButton_Click(sender As Object, e As EventArgs) Handles declaredButton1.Click
        _declared = Not _declared

        If _declared Then
            declaredButton1.Text = "Actual"
        Else
            declaredButton1.Text = "Declared"
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

        Public Property OrganizationID As String

        Public Function FullName() As String
            Return FirstName + " " + LastName
        End Function

        Public Overrides Function ToString() As String
            Return EmployeeID
        End Function

    End Class

    Private Class Organization

        Public Property RowID As String

        Public Property Name As String

    End Class

    Private Class TimeEntry

        Public Property RowID As Integer?
        Public Property EntryDate As Date?
        Public Property TimeIn As TimeSpan?
        Public Property TimeOut As TimeSpan?
        Public Property ShiftFrom As TimeSpan?
        Public Property ShiftTo As TimeSpan?
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
        Public Property LeavePay As Decimal
        Public Property HolidayPay As Decimal
        Public Property UndertimeHours As Decimal
        Public Property UndertimeAmount As Decimal
        Public Property LateHours As Decimal
        Public Property LateAmount As Decimal
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

    Private Sub btnRerfresh_Click(sender As Object, e As EventArgs) Handles btnRerfresh.Click

        Dim _bool As Boolean = btnRerfresh.Enabled

        ShowTimeEntryPanel(_bool)

        TabPage_Enter(TabPage1, New EventArgs)

    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub ShowTimeEntryPanel(show_ui As Boolean)

        Panel2.Visible = show_ui

        Panel1.Visible = (Not show_ui)

        If show_ui Then

            Panel2.Location = New Point(0, Panel2.Location.Y)

            Size = New Size(1107, 617)
        Else

        End If

    End Sub

    Private Sub ViewTimeEntryEmployeeLevel_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        If Panel2.Visible Then

            Dim prompt =
                MessageBox.Show(Me,
                                "Do you want to quit viewing ?",
                                "Quit view time entry",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question)

            e.Cancel =
                (prompt = DialogResult.No)

        End If

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            Me.Close()

            Return False
        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub cboOrganizations_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOrganizations.SelectedIndexChanged

    End Sub

    Private Sub cboOrganizations_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboOrganizations.SelectedValueChanged
        selected_og_rowid = Convert.ToString(cboOrganizations.SelectedValue)

        Dim has_searched As Boolean = (searchTextBox.Text.Trim.Length > 0)

        If has_searched Then
            FilterEmployees()
        End If

    End Sub

    Private Sub cboOrganizations_DropDown(sender As Object, e As EventArgs) Handles cboOrganizations.DropDown

        Static cb_font As Font = cboOrganizations.Font

        'Dim cb_width As Integer = cboOrganizations.DropDownWidth

        Dim grp As Graphics = cboOrganizations.CreateGraphics()

        Dim vertScrollBarWidth As Integer = If(cboOrganizations.Items.Count > cboOrganizations.MaxDropDownItems, SystemInformation.VerticalScrollBarWidth, 0)

        Dim wiidth As Integer = 0

        Dim data_source As Object = Nothing

        data_source = cboOrganizations.DataSource

        Dim i = 0

        Dim drp_downwidhths As Integer()

        'ReDim drp_downwidhths(data_source.Rows.Count - 1)
        ReDim drp_downwidhths(_organizations.Count - 1)

        'For Each strRow As DataRow In data_source.Rows

        '    wiidth = CInt(grp.MeasureString(CStr(strRow(1)), cb_font).Width) + vertScrollBarWidth

        '    drp_downwidhths(i) = wiidth

        '    'If cb_width < wiidth Then
        '    '    wiidth = wiidth
        '    'End If

        '    i += 1

        'Next

        For Each og As Organization In _organizations

            wiidth = CInt(grp.MeasureString(CStr(og.Name), cb_font).Width) + vertScrollBarWidth

            drp_downwidhths(i) = wiidth

        Next

        Dim max_drp_downwidhth As Integer = drp_downwidhths.Max

        cboOrganizations.DropDownWidth = max_drp_downwidhth 'wiidth, cb_width

    End Sub

    Private Sub TabPage_Enter(sender As Object, e As EventArgs) _
        Handles TabPage1.Enter,
                TabPage2.Enter

        Dim sender_obj = DirectCast(sender, TabPage)

        _declared = (sender_obj.Name = "TabPage2")

        LoadTimeEntries()

    End Sub

    Private Sub cboOrganizations_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboOrganizations.KeyPress
        e.Handled = True
    End Sub

End Class
