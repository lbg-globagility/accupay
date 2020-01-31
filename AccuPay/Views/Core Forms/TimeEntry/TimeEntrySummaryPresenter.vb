Option Strict On

Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports MySql.Data.MySqlClient

Namespace TimeEntrySummary

    Class PayPeriod

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

    Class Employee

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

    Class TimeEntry

        Public Shared HoursPerDay As TimeSpan = New TimeSpan(24, 0, 0)

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

    End Class

    Public Class TimeEntrySummaryPresenter

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

        Private Shared Function ConvertToDate(time As TimeSpan?) As Date?
            If Not time.HasValue Then
                Return Nothing
            End If

            If time > TimeEntry.HoursPerDay Then
                Return Date.Parse((time - TimeEntry.HoursPerDay).ToString())
            Else
                Return Date.Parse(time.ToString())
            End If
        End Function

        Private Async Function GetPayPeriods(organizationID As Integer,
                                             year As Integer,
                                             salaryType As Integer) As Task(Of Collection(Of PayPeriod))
            Dim sql = <![CDATA[
                SELECT PayFromDate, PayToDate, Year, Month, OrdinalValue
                FROM payperiod
                WHERE payperiod.OrganizationID = @OrganizationID AND
                    payperiod.Year = @Year AND
                    payperiod.TotalGrossSalary = @SalaryType;
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
                    employeetimeentry.TotalDayPay
                FROM employeetimeentry
                LEFT JOIN employeetimeentrydetails
                ON employeetimeentrydetails.Date = employeetimeentry.Date AND
                    employeetimeentrydetails.OrganizationID = employeetimeentry.OrganizationID AND
                    employeetimeentrydetails.EmployeeID = employeetimeentry.EmployeeID
                LEFT JOIN employeeshift
                ON employeeshift.RowID = employeetimeentry.EmployeeShiftID
                LEFT JOIN shift
                ON shift.RowID = employeeshift.ShiftID
                WHERE employeetimeentry.EmployeeID = @EmployeeID AND
                    employeetimeentry.`Date` BETWEEN @DateFrom AND @DateTo;
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

                    timeEntries.Add(timeEntry)
                End While
            End Using

            Return timeEntries
        End Function

    End Class

End Namespace