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

        Dim record = logs.Count

        Dim employeeShifts = New List(Of ShiftSchedule)


        Dim employeeshift = New ShiftSchedule() With {
            .EffectiveFrom = Date.Parse("2018-06-01"),
            .Shift = New Shift() With {
            .TimeFrom = TimeSpan.Parse("08:00:00"),
            .TimeTo = TimeSpan.Parse("05:00:00")
            }
        }
        employeeShifts.Add(employeeshift)

        Dim analyzer = New TimeAttendanceAnalyzer()
        Dim results = analyzer.Analyze(logs, employeeShifts)

        Dim firstresult = results.Item(0)

        'Assert.AreEqual("08:30:00", firstresult.TimeIn)
        'Assert.AreEqual("08:30:00", firstresult.TimeOut)
        'Assert.AreEqual("2018-06-01", firstresult.LogDate.Date)

    End Sub

End Class
