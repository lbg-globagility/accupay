Option Strict On

Imports System.IO
Imports AccuPay
Imports AccuPay.Entity

<TestFixture>
Public Class TimeImporter

    Private _projectPath As String

    <SetUp>
    Public Sub Init()
        _projectPath = Directory.GetParent(
                            Directory.GetParent(
                                AppDomain.CurrentDomain.BaseDirectory
                            ).Parent.FullName
                        ).FullName
    End Sub


    <Test>
    Public Sub ShouldImport()
        Dim importer = New TimeLogsReader()

        Dim filename = _projectPath & "\_timelogs_test.dat"
        'Dim filename = "E:\Stuff\accupay\_timelogs\cinema.txt"
        'Dim filename = "E:\Stuff\accupay\_timelogs\fourlinq.dat"

        Dim importOutput = importer.Import(filename)
        Dim logs = importOutput.Logs

        logs = logs.OrderByDescending(Function(x) x.EmployeeNo).ThenBy(Function(y) y.DateTime).ToList

        Dim employeeShifts As List(Of ShiftSchedule) = GetSampleShiftSchedules()
        Dim employees As List(Of Employee) = GetSampleEmployees()

        Dim analyzer = New TimeAttendanceAnalyzer()

        Dim logsGroupedByEmployee = analyzer.GetLogsGroupByEmployee(logs)
        Dim results = analyzer.Analyze(employees, logsGroupedByEmployee, employeeShifts)
        AssertTimeLog(results.Item(0), "08:30:00", "18:00:00", "2018-06-01")
        AssertTimeLog(results.Item(1), "07:00:00", "01:00:00", "2018-06-02")
        AssertTimeLog(results.Item(2), "06:00:00", "20:00:00", "2018-06-03")
        AssertTimeLog(results.Item(3), "05:00:00", "03:00:00", "2018-06-04")
        AssertTimeLog(results.Item(4), "04:00:00", "01:00:00", "2018-06-05")
        AssertTimeLog(results.Item(5), "08:00:00", "03:00:00", "2018-06-06")
        AssertTimeLog(results.Item(6), "", "23:30:00", "2018-06-07")
    End Sub

    <Test>
    Public Sub ShouldImportWithoutShifts()
        Dim importer = New TimeLogsReader()

        Dim filename = _projectPath & "\_timelogs_test.dat"
        'Dim filename = "E:\Stuff\accupay\_timelogs\cinema.txt"
        'Dim filename = "E:\Stuff\accupay\_timelogs\fourlinq.dat"

        Dim importOutput = importer.Import(filename)
        Dim logs = importOutput.Logs

        logs = logs.OrderByDescending(Function(x) x.EmployeeNo).ThenBy(Function(y) y.DateTime).ToList

        Dim employeeShifts As New List(Of ShiftSchedule)
        Dim employees As List(Of Employee) = GetSampleEmployees()

        Dim analyzer = New TimeAttendanceAnalyzer()

        Dim logsGroupedByEmployee = analyzer.GetLogsGroupByEmployee(logs)
        Dim results = analyzer.Analyze(employees, logsGroupedByEmployee, employeeShifts)
        AssertTimeLog(results.Item(0), "08:30:00", "18:00:00", "2018-06-01")
        AssertTimeLog(results.Item(1), "07:00:00", Nothing, "2018-06-02")
        AssertTimeLog(results.Item(2), "01:00:00", "20:00:00", "2018-06-03")
        AssertTimeLog(results.Item(3), "05:00:00", Nothing, "2018-06-04")
        AssertTimeLog(results.Item(4), "03:00:00", "23:59:00", "2018-06-05")
        AssertTimeLog(results.Item(5), "01:00:00", "23:00:00", "2018-06-06")
        AssertTimeLog(results.Item(6), "02:00:00", "23:30:00", "2018-06-07")
    End Sub

    <Test>
    Public Sub ShouldNotImport()
        Dim importer = New TimeLogsReader()

        Dim filename = _projectPath & "\_timelogs_test_errors_sdfsdf.dat"
        Dim importOutput = importer.Import(filename)

        Dim errors = importOutput.Errors

        Assert.That(errors.Count = 1)
        Assert.That(errors(0).LineNumber = 0)
        Assert.That(errors(0).Reason = TimeLogsReader.ErrorLog.FILE_NOT_FOUND_ERROR)

    End Sub

    <Test>
    Public Sub ShouldHaveErrors()
        Dim importer = New TimeLogsReader()

        Dim filename = _projectPath & "\_timelogs_test_errors.dat"

        Dim importOutput = importer.Import(filename)
        Dim errors = importOutput.Errors

        Dim contentFormat = "    {0}" & vbTab & "{1}" & vbTab & "{2}" & vbTab & "{3}" & vbTab & "{4}" & vbTab & "{5}"

        Assert.That(errors.Count = 6)

        Assert.That(errors.Item(0).Reason = "Second column must be a valid Date Time.")
        Assert.That(errors.Item(0).LineNumber = 3)
        Assert.That(errors.Item(0).Content = String.Format(contentFormat,
                    "10123", "201A-06-02 07:00:00", "1", "0", "1", "0"))

        Assert.That(errors.Item(1).Reason = "Second column must be a valid Date Time.")
        Assert.That(errors.Item(1).LineNumber = 7)
        Assert.That(errors.Item(1).Content = String.Format(contentFormat,
                    "10123", "2018-0B-04 20:00:00", "1", "0", "1", "0"))

        Assert.That(errors.Item(2).Reason = "Second column must be a valid Date Time.")
        Assert.That(errors.Item(2).LineNumber = 9)
        Assert.That(errors.Item(2).Content = String.Format(contentFormat,
                    "10123", "2018-06-0C 03:00:00", "1", "0", "1", "0"))

        Assert.That(errors.Item(3).Reason = "Second column must be a valid Date Time.")
        Assert.That(errors.Item(3).LineNumber = 10)
        Assert.That(errors.Item(3).Content = String.Format(contentFormat,
                    "10123", "2018-06-05 0D:00:00", "1", "0", "1", "0"))

        Assert.That(errors.Item(4).Reason = "Second column must be a valid Date Time.")
        Assert.That(errors.Item(4).LineNumber = 12)
        Assert.That(errors.Item(4).Content = String.Format(contentFormat,
                    "10123", "2018-06-05 19:E0:00", "1", "0", "1", "0"))

        Assert.That(errors.Item(5).Reason = "Second column must be a valid Date Time.")
        Assert.That(errors.Item(5).LineNumber = 15)
        Assert.That(errors.Item(5).Content = String.Format(contentFormat,
                    "10123", "2018-06-06 08:00:F0", "1", "0", "1", "0"))
    End Sub



#Region "Private Functions"
    Private Function GetSampleEmployees() As List(Of Employee)
        Dim employees = New List(Of Employee)

        Dim employeeId As Integer? = 1

        Dim employee As New Employee With {
            .RowID = employeeId,
            .EmployeeNo = "10123"
        }

        employees.Add(employee)

        Return employees
    End Function

    Private Function GetSampleShiftSchedules() As List(Of ShiftSchedule)
        Dim employeeShifts = New List(Of ShiftSchedule)

        Dim employeeShift As ShiftSchedule

        Dim employeeId As Integer? = 1
        Dim employeeId_2 As Integer? = 2

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-01"),
            .EffectiveTo = Date.Parse("2018-06-01"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-02"),
            .EffectiveTo = Date.Parse("2018-06-02"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId_2,
            .EffectiveFrom = Date.Parse("2018-06-01"),
            .EffectiveTo = Date.Parse("2018-06-01"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId_2,
            .EffectiveFrom = Date.Parse("2018-06-02"),
            .EffectiveTo = Date.Parse("2018-06-02"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-03"),
            .EffectiveTo = Date.Parse("2018-06-03"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-04"),
            .EffectiveTo = Date.Parse("2018-06-04"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }
        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-05"),
            .EffectiveTo = Date.Parse("2018-06-05"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }
        employeeShifts.Add(employeeShift)
        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-06"),
            .EffectiveTo = Date.Parse("2018-06-06"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }
        employeeShifts.Add(employeeShift)
        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-07"),
            .EffectiveTo = Date.Parse("2018-06-07"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }
        employeeShifts.Add(employeeShift)

        Return employeeShifts
    End Function

    Private Sub AssertTimeLog(time As TimeLog, correctTimeIn As String, correctTimeOut As String, correctDate As String)
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
#End Region

End Class

