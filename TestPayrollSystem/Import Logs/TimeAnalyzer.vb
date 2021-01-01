Option Strict On

Imports System.IO
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Services
Imports AccuPay.Data.Services.Imports

Public Class TimeAnalyzer

    Protected _projectPath As String

    Sub New()

        _projectPath = Directory.GetParent(
            Directory.GetParent(
                AppDomain.CurrentDomain.BaseDirectory
            ).Parent.FullName
        ).FullName

    End Sub

    Protected Function GetParsedTimeLogs() As IList(Of TimeLogImportModel)
        Dim importer = New TimeLogsReader()

        Dim filename = _projectPath & "\_timelogs_test.dat"

        Dim importOutput = importer.Read(filename)
        Dim logs = importOutput.Logs

        Return logs
    End Function

    Protected Function GetSampleEmployees() As List(Of Employee)
        Dim employees = New List(Of Employee)

        Dim employeeId As Integer? = 1

        Dim employee As New Employee With {
            .RowID = employeeId,
            .EmployeeNo = "10123"
        }

        employees.Add(employee)

        Return employees
    End Function

    Protected Function GetSampleShifts() As List(Of Shift)
        Dim shifts = New List(Of Shift)

        Dim shift As Shift

        Dim employeeId As Integer? = 1
        Dim employeeId_2 As Integer? = 2

        shift = New Shift() With {
            .EmployeeID = employeeId,
            .DateSched = Date.Parse("2018-06-01"),
            .StartTime = TimeSpan.Parse("08:00:00"),
            .EndTime = TimeSpan.Parse("17:00:00")
        }

        shifts.Add(shift)

        shift = New Shift() With {
            .EmployeeID = employeeId,
            .DateSched = Date.Parse("2018-06-02"),
            .StartTime = TimeSpan.Parse("08:00:00"),
            .EndTime = TimeSpan.Parse("17:00:00")
        }

        shifts.Add(shift)

        shift = New Shift() With {
            .EmployeeID = employeeId_2,
            .DateSched = Date.Parse("2018-06-01"),
            .StartTime = TimeSpan.Parse("08:00:00"),
            .EndTime = TimeSpan.Parse("17:00:00")
        }

        shifts.Add(shift)

        shift = New Shift() With {
            .EmployeeID = employeeId_2,
            .DateSched = Date.Parse("2018-06-02"),
            .StartTime = TimeSpan.Parse("08:00:00"),
            .EndTime = TimeSpan.Parse("17:00:00")
        }

        shifts.Add(shift)

        shift = New Shift() With {
            .EmployeeID = employeeId,
            .DateSched = Date.Parse("2018-06-03"),
            .StartTime = TimeSpan.Parse("08:00:00"),
            .EndTime = TimeSpan.Parse("17:00:00")
        }

        shifts.Add(shift)

        shift = New Shift() With {
            .EmployeeID = employeeId,
            .DateSched = Date.Parse("2018-06-04"),
            .StartTime = TimeSpan.Parse("08:00:00"),
            .EndTime = TimeSpan.Parse("17:00:00")
        }
        shifts.Add(shift)

        shift = New Shift() With {
            .EmployeeID = employeeId,
            .DateSched = Date.Parse("2018-06-05"),
            .StartTime = TimeSpan.Parse("08:00:00"),
            .EndTime = TimeSpan.Parse("17:00:00")
        }
        shifts.Add(shift)
        shift = New Shift() With {
            .EmployeeID = employeeId,
            .DateSched = Date.Parse("2018-06-06"),
            .StartTime = TimeSpan.Parse("08:00:00"),
            .EndTime = TimeSpan.Parse("17:00:00")
        }
        shifts.Add(shift)
        shift = New Shift() With {
            .EmployeeID = employeeId,
            .DateSched = Date.Parse("2018-06-07"),
            .StartTime = TimeSpan.Parse("08:00:00"),
            .EndTime = TimeSpan.Parse("17:00:00")
        }
        shifts.Add(shift)

        Return shifts
    End Function

    Protected Function GetSampleShifts_WithNextShiftWithoutShift() As List(Of Shift)
        Dim employeeShifts = New List(Of Shift)

        Dim employeeShift As Shift

        Dim employeeId As Integer? = 1
        Dim employeeId_2 As Integer? = 2

        employeeShift = New Shift() With {
            .EmployeeID = employeeId,
            .DateSched = Date.Parse("2018-06-01"),
            .StartTime = TimeSpan.Parse("08:00:00"),
            .EndTime = TimeSpan.Parse("17:00:00")
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New Shift() With {
            .EmployeeID = employeeId,
            .DateSched = Date.Parse("2018-06-02"),
            .StartTime = TimeSpan.Parse("08:00:00"),
            .EndTime = TimeSpan.Parse("17:00:00")
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New Shift() With {
            .EmployeeID = employeeId_2,
            .DateSched = Date.Parse("2018-06-01"),
            .StartTime = TimeSpan.Parse("08:00:00"),
            .EndTime = TimeSpan.Parse("17:00:00")
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New Shift() With {
            .EmployeeID = employeeId_2,
            .DateSched = Date.Parse("2018-06-02"),
            .StartTime = TimeSpan.Parse("08:00:00"),
            .EndTime = TimeSpan.Parse("17:00:00")
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New Shift() With {
            .EmployeeID = employeeId,
            .DateSched = Date.Parse("2018-06-03"),
            .StartTime = TimeSpan.Parse("08:00:00"),
            .EndTime = TimeSpan.Parse("17:00:00")
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New Shift() With {
            .EmployeeID = employeeId,
            .DateSched = Date.Parse("2018-06-04"),
            .StartTime = TimeSpan.Parse("08:00:00"),
            .EndTime = TimeSpan.Parse("17:00:00")
        }
        employeeShifts.Add(employeeShift)

        employeeShift = New Shift() With {
            .EmployeeID = employeeId,
            .DateSched = Date.Parse("2018-06-05"),
            .StartTime = TimeSpan.Parse("08:00:00"),
            .EndTime = TimeSpan.Parse("17:00:00")
        }
        employeeShifts.Add(employeeShift)
        employeeShift = New Shift() With {
            .EmployeeID = employeeId,
            .DateSched = Date.Parse("2018-06-06"),
            .StartTime = TimeSpan.Parse("08:00:00"),
            .EndTime = TimeSpan.Parse("17:00:00")
        }
        employeeShifts.Add(employeeShift)
        employeeShift = New Shift() With {
            .EmployeeID = employeeId,
            .DateSched = Date.Parse("2018-06-07"),
            .StartTime = Nothing,
            .EndTime = Nothing
        }
        employeeShifts.Add(employeeShift)

        Return employeeShifts
    End Function

    Protected Function GetSampleEmployeeOvertimes() As List(Of Overtime)

        Return New List(Of Overtime)

    End Function

    Protected Sub AssertTimeLog(time As TimeLog, correctTimeIn As String, correctTimeOut As String, correctDate As String)
        Dim timeIn As TimeSpan?
        If correctTimeIn Is Nothing Then
            timeIn = Nothing
        Else
            Try
                timeIn = TimeSpan.Parse(correctTimeIn)
            Catch ex As Exception
                timeIn = Nothing
            End Try
        End If

        Dim timeOut As TimeSpan?
        If correctTimeOut Is Nothing Then
            timeOut = Nothing
        Else
            Try
                timeOut = TimeSpan.Parse(correctTimeOut)
            Catch ex As Exception
                timeOut = Nothing
            End Try
        End If

        Assert.Multiple(
            Sub()
                If String.IsNullOrWhiteSpace(correctTimeIn) Then
                    Assert.That(time.TimeIn, [Is].Null, "Date", correctDate)
                Else
                    Assert.That(time.TimeIn, [Is].EqualTo(timeIn), correctDate)
                End If

                Assert.That(time.TimeOut, [Is].EqualTo(timeOut), correctDate)
                Assert.That(time.LogDate.Date, [Is].EqualTo(DateTime.Parse(correctDate)))
            End Sub)
    End Sub

End Class
