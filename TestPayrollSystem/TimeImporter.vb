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

        Dim employeeShifts = New List(Of ShiftSchedule)


        Dim employeeshift = New ShiftSchedule() With {
            .EffectiveFrom = Date.Parse("2018-06-01"),
            .Shift = New Shift() With {
            .TimeFrom = TimeSpan.Parse("08:30:00"),
            .TimeTo = TimeSpan.Parse("08:30:00")
            }
        }
        employeeShifts.Add(employeeshift)

        Dim analyzer = New TimeAttendanceAnalyzer()
        Dim results = analyzer.Analyze(logs, employeeShifts)

        Dim firstresult = results.Item(0)


    End Sub

End Class
