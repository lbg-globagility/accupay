Option Strict On

Imports MySql.Data.MySqlClient
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports log4net

Public Class TimeEntrySummary

    Private Shared _logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Class PayPeriod
        Public Property PayFromDate As Date
        Public Property PayToDate As Date
        Public Property Year As Integer
        Public Property Month As Integer

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
        Public Property EntryDate As Date
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
        Public Property LeavePay As Decimal
        Public Property HolidayPay As Decimal
        Public Property UndertimeHours As Decimal
        Public Property UndertimeAmount As Decimal
        Public Property LateHours As Decimal
        Public Property LateAmount As Decimal
        Public Property AbsentAmount As Decimal
        Public Property TotalHoursWorked As Decimal
        Public Property TotalDayPay As Decimal

        Public ReadOnly Property TimeInDisplay As DateTime?
            Get
                If Not TimeIn.HasValue Then
                    Return Nothing
                End If

                Return DateTime.Parse(TimeIn.ToString())
            End Get
        End Property

        Public ReadOnly Property TimeOutDisplay As DateTime?
            Get
                If Not TimeOut.HasValue Then
                    Return Nothing
                End If

                Return DateTime.Parse(TimeOut.ToString())
            End Get
        End Property

        Public ReadOnly Property ShiftFromDisplay As DateTime?
            Get
                If Not ShiftFrom.HasValue Then
                    Return Nothing
                End If

                Return DateTime.Parse(ShiftFrom.ToString())
            End Get
        End Property

        Public ReadOnly Property ShiftToDisplay As DateTime?
            Get
                If Not ShiftTo.HasValue Then
                    Return Nothing
                End If

                Return DateTime.Parse(ShiftTo.ToString())
            End Get
        End Property

    End Class

    Private _payPeriods As Collection(Of PayPeriod)

    Private _employees As Collection(Of Employee)

    Private _selectedEmployee As Employee

    Private _selectedPayPeriod As PayPeriod

    Private WithEvents _timeEntdurationModal As TimEntduration

    Private Async Sub LoadEmployees()
        _employees = Await GetEmployees()
        employeesDataGridView.DataSource = _employees

        If _selectedEmployee Is Nothing Then
            _selectedEmployee = _employees.FirstOrDefault()
        End If
    End Sub

    Private Async Function GetEmployees() As Task(Of Collection(Of Employee))
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

    Public Async Sub LoadPayPeriods()
        Me._payPeriods = Await GetPayPeriods(z_OrganizationID, 2017, 1)
        payPeriodsDataGridView.Rows.Add(2)

        Dim monthCounters(11) As Integer

        For Each payperiod In Me._payPeriods
            Dim monthNo = payperiod.Month
            Dim counter = monthCounters(monthNo - 1)

            Dim payFromDate = payperiod.PayFromDate.ToString("dd MMM")
            Dim payToDate = payperiod.PayToDate.ToString("dd MMM")
            Dim label = payFromDate + " - " + payToDate

            payPeriodsDataGridView.Rows(counter).Cells(monthNo - 1).Value = payperiod

            counter += 1
            monthCounters(monthNo - 1) = counter
        Next

        If _selectedPayPeriod Is Nothing Then
            _selectedPayPeriod = _payPeriods.FirstOrDefault()
        End If
    End Sub

    Private Async Function GetPayPeriods(organizationID As Integer,
                                         year As Integer,
                                         salaryType As Integer) As Task(Of Collection(Of PayPeriod))
        Dim sql = <![CDATA[
            SELECT PayFromDate, PayToDate, Year, Month
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
                    .Month = reader.GetValue(Of Integer)("Month")
                }

                payPeriods.Add(payPeriod)
            End While
        End Using

        Return payPeriods
    End Function

    Public Async Sub LoadTimeEntries()
        _logger.Info(New Object() {"LoadTimeEntries()", _selectedEmployee, _selectedPayPeriod})

        If _selectedEmployee Is Nothing Or _selectedPayPeriod Is Nothing Then
            Return
        End If

        timeEntriesDataGridView.AutoGenerateColumns = False
        timeEntriesDataGridView.DataSource = Await GetTimeEntries(_selectedEmployee, _selectedPayPeriod)
    End Sub

    Private Async Function GetYears() As Task(Of Collection(Of Integer))
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

    Private Async Function GetTimeEntries(employee As Employee, payPeriod As PayPeriod) As Task(Of Collection(Of TimeEntry))
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
                employeetimeentry.LeavePayment,
                employeetimeentry.HoursLate,
                employeetimeentry.HoursLateAmount,
                employeetimeentry.UndertimeHours,
                employeetimeentry.UndertimeHoursAmount,
                employeetimeentry.Leavepayment,
                employeetimeentry.HolidayPayAmount,
                employeetimeentry.Absent,
                employeetimeentry.TotalHoursWorked,
                employeetimeentry.TotalDayPay
            FROM employeetimeentry
            LEFT JOIN employeetimeentrydetails
                ON employeetimeentrydetails.Date = employeetimeentry.Date
                AND employeetimeentrydetails.OrganizationID = employeetimeentry.OrganizationID
                AND employeetimeentrydetails.EmployeeID = employeetimeentry.EmployeeID
            LEFT JOIN employeeshift
                ON employeeshift.RowID = employeetimeentry.EmployeeShiftID
            LEFT JOIN shift
                ON shift.RowID = employeeshift.ShiftID
            WHERE employeetimeentry.EmployeeID = @EmployeeID
                AND employeetimeentry.`Date` BETWEEN @DateFrom AND @DateTo;
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

                timeEntries.Add(timeEntry)
            End While
        End Using

        Return timeEntries
    End Function

    Private Sub generateTimeEntryButton_Click(sender As Object, e As EventArgs) Handles generateTimeEntryButton.Click
        Return
        _timeEntdurationModal = New TimEntduration(DateTime.Today)

        _timeEntdurationModal.ShowDialog()
    End Sub

    Private Sub a() Handles _timeEntdurationModal.DoneGenerating
        Trace.Write("Hello")
    End Sub

    Private Sub TimeEntrySummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadEmployees()
        LoadPayPeriods()
    End Sub

    Private Sub employeesDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles employeesDataGridView.SelectionChanged
        If employeesDataGridView.CurrentRow Is Nothing Then
            Return
        End If

        Dim employee = DirectCast(employeesDataGridView.CurrentRow.DataBoundItem, Employee)

        If employee IsNot _selectedEmployee Then
            _selectedEmployee = employee

            LoadTimeEntries()
        End If
    End Sub

    Private Sub payPeriodDataGridView_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles payPeriodsDataGridView.CellClick
        If e.RowIndex = -1 Then
            Return
        End If

        If payPeriodsDataGridView.CurrentRow Is Nothing Then
            Return
        End If

        Dim payPeriod = DirectCast(payPeriodsDataGridView.CurrentCell.Value, PayPeriod)

        If payPeriod IsNot _selectedPayPeriod Then
            _selectedPayPeriod = payPeriod

            LoadTimeEntries()
        End If
    End Sub

    Private Sub tsbtnCloseempawar_Click(sender As Object, e As EventArgs) Handles tsbtnCloseempawar.Click
        Me.Close()
        TimeAttendForm.listTimeAttendForm.Remove(Me.Name)
    End Sub

    Private Sub searchTextBox_TextChanged(sender As Object, e As EventArgs) Handles searchTextBox.TextChanged
        FilterEmployees()
    End Sub

    Private Sub FilterEmployees()
        Dim searchValue = searchTextBox.Text.ToLower()

        Dim matchCriteria = Function(employee As Employee) As Boolean
                                Dim containsEmployeeId = employee.EmployeeID.ToLower().Contains(searchValue)
                                Dim containsFullName = employee.FullName.ToLower().Contains(searchValue)

                                Dim reverseFullName = employee.LastName.ToLower() + " " + employee.FirstName.ToLower()
                                Dim containsFullNameInReverse = reverseFullName.Contains(searchValue)

                                Return containsEmployeeId Or containsFullName Or containsFullNameInReverse
                            End Function

        Dim filtered = New BindingList(Of Employee)(
            _employees.Where(matchCriteria).ToList()
        )

        employeesDataGridView.DataSource = filtered
        employeesDataGridView.Update()
    End Sub

End Class