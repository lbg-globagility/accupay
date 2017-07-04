Option Strict On

Imports MySql.Data.MySqlClient
Imports System.Collections.ObjectModel
Imports System.Threading.Tasks

Public Class TimeEntrySummary

    Private Class PayPeriod
        Public Property PayFromDate As Date
        Public Property PayToDate As Date
        Public Property Year As Integer
        Public Property Month As Integer
    End Class

    Private Class Employee
        Public Property RowID As String
        Public Property EmployeeID As String
        Public Property FirstName As String
        Public Property LastName As String
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
        Public Property OvertimeHours As Decimal
        Public Property OvertimeAmount As Decimal
        Public Property HolidayPayAmount As Decimal
        Public Property TotalPay As Decimal

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

    Private payPeriods As Collection(Of PayPeriod)
    Private employees As Collection(Of Employee)

    Private Async Sub LoadEmployees()
        Me.employees = Await GetEmployees()
        employeesDataGridView.DataSource = Me.employees
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
        Me.payPeriods = Await GetPayPeriods()
        payPeriodDataGridView.Rows.Add(2)

        Dim monthCounters(11) As Integer

        For Each payperiod In Me.payPeriods
            Dim monthNo = payperiod.Month
            Dim counter = monthCounters(monthNo - 1)

            Dim payFromDate = payperiod.PayFromDate.ToString("dd MMM")
            Dim payToDate = payperiod.PayToDate.ToString("dd MMM")
            Dim label = payFromDate + " - " + payToDate

            payPeriodDataGridView.Rows(counter).Cells(monthNo - 1).Value = label

            counter += 1
            monthCounters(monthNo - 1) = counter
        Next
    End Sub

    Private Async Function GetPayPeriods() As Task(Of Collection(Of PayPeriod))
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
                .AddWithValue("@Year", CStr(2016))
                .AddWithValue("@SalaryType", CStr(1))
            End With

            Await connection.OpenAsync()
            Dim reader = Await command.ExecuteReaderAsync()
            While Await reader.ReadAsync()
                Dim payFromDate = reader.GetValue(Of Date)("PayFromDate")
                Dim payToDate = reader.GetValue(Of Date)("PayToDate")
                Dim year = reader.GetValue(Of Integer)("Year")
                Dim month = reader.GetValue(Of Integer)("Month")

                Dim payPeriod = New PayPeriod() With {
                    .PayFromDate = payFromDate,
                    .PayToDate = payToDate,
                    .Year = year,
                    .Month = month
                }

                payPeriods.Add(payPeriod)
            End While
        End Using

        Return payPeriods
    End Function

    Public Async Sub LoadTimeEntries()
        timeEntriesDataGridView.AutoGenerateColumns = False
        timeEntriesDataGridView.DataSource = Await GetTimeEntries()
    End Sub

    Private Async Function GetTimeEntries() As Task(Of Collection(Of TimeEntry))
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
                employeetimeentry.OvertimeHoursWorked,
                employeetimeentry.OvertimeHoursAmount,
                employeetimeentry.HolidayPayAmount,
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
                .AddWithValue("@EmployeeID", CStr(36))
                .AddWithValue("@DateFrom", "2017-06-06")
                .AddWithValue("@DateTo", "2017-06-20")
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
                    .OvertimeHours = reader.GetValue(Of Decimal)("OvertimeHoursWorked"),
                    .OvertimeAmount = reader.GetValue(Of Decimal)("OvertimeHoursAmount"),
                    .HolidayPayAmount = reader.GetValue(Of Decimal)("HolidayPayAmount"),
                    .TotalPay = reader.GetValue(Of Decimal)("TotalDayPay")
                }

                timeEntries.Add(timeEntry)
            End While
        End Using

        Return timeEntries
    End Function

    Private Sub TimeEntrySummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadEmployees()
        LoadPayPeriods()
        LoadTimeEntries()
    End Sub

End Class