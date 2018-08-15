Option Strict On

Imports AccuPay
Imports AccuPay.Entity

<TestFixture>
Public Class TimeImporter

    <Test>
    Public Sub ShouldImport()
        Dim importer = New TimeLogsReader()
        Dim filename = "C:\Users\GLOBAL-H\Desktop\Ondevilla\1_attlog.dat"

        Dim logs = importer.Import(filename)
        logs = logs.OrderByDescending(Function(x) x.EmployeeNo).ThenBy(Function(y) y.DateTime).ToList
        Dim record = logs.Count

        Dim employeeShifts = New List(Of ShiftSchedule)
        Dim employeeShift = New ShiftSchedule()

        employeeShift = New ShiftSchedule() With {
            .EffectiveFrom = Date.Parse("2018-06-01"),
            .Shift = New Shift() With {
            .TimeFrom = TimeSpan.Parse("08:00:00"),
            .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EffectiveFrom = Date.Parse("2018-06-02"),
            .Shift = New Shift() With {
            .TimeFrom = TimeSpan.Parse("08:00:00"),
            .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EffectiveFrom = Date.Parse("2018-06-03"),
            .Shift = New Shift() With {
            .TimeFrom = TimeSpan.Parse("08:00:00"),
            .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EffectiveFrom = Date.Parse("2018-06-04"),
            .Shift = New Shift() With {
            .TimeFrom = TimeSpan.Parse("08:00:00"),
            .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }
        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EffectiveFrom = Date.Parse("2018-06-05"),
            .Shift = New Shift() With {
            .TimeFrom = TimeSpan.Parse("08:00:00"),
            .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }
        employeeShifts.Add(employeeShift)
        employeeShift = New ShiftSchedule() With {
            .EffectiveFrom = Date.Parse("2018-06-06"),
            .Shift = New Shift() With {
            .TimeFrom = TimeSpan.Parse("08:00:00"),
            .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }
        employeeShifts.Add(employeeShift)
        employeeShift = New ShiftSchedule() With {
            .EffectiveFrom = Date.Parse("2018-06-07"),
            .Shift = New Shift() With {
            .TimeFrom = TimeSpan.Parse("08:00:00"),
            .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }
        employeeShifts.Add(employeeShift)

        Dim analyzer = New TimeAttendanceAnalyzer()
        'Dim results = analyzer.FourHoursRule(logs, employeeShifts)
        Dim results = analyzer.FourHoursRuleBasedOnEmployeeShift(logs, employeeShifts)

        AssertTimeLog(results.Item(0), "08:30:00", "18:00:00", "2018-06-01")
        AssertTimeLog(results.Item(1), "07:00:00", "01:00:00", "2018-06-02")
        AssertTimeLog(results.Item(2), "06:00:00", "20:00:00", "2018-06-03")
        AssertTimeLog(results.Item(3), "05:00:00", "03:00:00", "2018-06-04")
        AssertTimeLog(results.Item(4), "04:00:00", "01:00:00", "2018-06-05")
        AssertTimeLog(results.Item(5), "08:00:00", "03:00:00", "2018-06-06")
        AssertTimeLog(results.Item(6), "", "23:30:00", "2018-06-07")

        'AssertTimeLog(results.Item(0), "04:00:00", "01:00:00", "2018-06-05")
        'AssertTimeLog(results.Item(1), "08:00:00", "03:00:00", "2018-06-06")
        'AssertTimeLog(results.Item(2), "", "23:30:00", "2018-06-07")

    End Sub

    Private Sub AssertTimeLog(time As TimeLogInOut, correctTimeIn As String, correctTimeOut As String, correctDate As String)
        If String.IsNullOrWhiteSpace(correctTimeIn) Then
            Assert.IsNull(time.TimeIn)
        Else
            Assert.AreEqual(TimeSpan.Parse(correctTimeIn), time.TimeIn)
        End If

        Assert.AreEqual(TimeSpan.Parse(correctTimeOut), time.TimeOut)
        Assert.AreEqual(DateTime.Parse(correctDate), time.LogDate.Date)

    End Sub

End Class