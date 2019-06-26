Option Strict On

Imports AccuPay
Imports AccuPay.Entity
Imports AccuPay.Helper.TimeLogsReader

<TestFixture>
Public Class TimeAttendanceHelperNewTest
    Inherits TimeAnalyzer
    Implements ITimeAnalyzer

    <Test>
    Public Sub ShouldImport() _
        Implements ITimeAnalyzer.ShouldImport

        Dim logs = New List(Of ImportTimeAttendanceLog)(GetParsedTimeLogs())

        Dim employeeShifts As List(Of EmployeeDutySchedule) = GetSampleEmployeeDutySchedules()
        Dim employees As List(Of Employee) = GetSampleEmployees()

        Dim employeeOvertimes As List(Of Overtime) = GetSampleEmployeeOvertimes()

        Dim timeAttendanceHelper = New TimeAttendanceHelperNew(logs, employees, employeeShifts, employeeOvertimes)

        logs = timeAttendanceHelper.Analyze()

        Dim results = timeAttendanceHelper.GenerateTimeLogs()

        AssertTimeLog(results.Item(0), "08:30:00", "18:00:00", "2018-06-01")
        AssertTimeLog(results.Item(1), "07:00:00", "01:00:00", "2018-06-02")
        AssertTimeLog(results.Item(2), "06:00:00", "20:00:00", "2018-06-03")
        AssertTimeLog(results.Item(3), "05:00:00", "03:00:00", "2018-06-04")
        AssertTimeLog(results.Item(4), "04:00:00", "01:00:00", "2018-06-05")
        AssertTimeLog(results.Item(5), "08:00:00", "03:00:00", "2018-06-06")

        ' Original AssertTimeLog(results.Item(6), "", "23:30:00", "2018-06-07")
        ' In old analyzer, 11:30 PM was analyzed as time out
        AssertTimeLog(results.Item(6), "23:30:00", "", "2018-06-07")
    End Sub

    <Test>
    Public Sub ShouldImport_WithoutShifts() _
        Implements ITimeAnalyzer.ShouldImport_WithoutShifts

        Dim logs = New List(Of ImportTimeAttendanceLog)(GetParsedTimeLogs())

        Dim employeeShifts As New List(Of EmployeeDutySchedule)
        Dim employees As List(Of Employee) = GetSampleEmployees()

        Dim employeeOvertimes As List(Of Overtime) = GetSampleEmployeeOvertimes()

        Dim timeAttendanceHelper = New TimeAttendanceHelperNew(logs, employees, employeeShifts, employeeOvertimes)

        logs = timeAttendanceHelper.Analyze()

        Dim results = timeAttendanceHelper.GenerateTimeLogs()

        AssertTimeLog(results.Item(0), "08:30:00", "18:00:00", "2018-06-01")
        AssertTimeLog(results.Item(1), "07:00:00", Nothing, "2018-06-02")
        AssertTimeLog(results.Item(2), "01:00:00", "20:00:00", "2018-06-03")
        AssertTimeLog(results.Item(3), "05:00:00", Nothing, "2018-06-04")
        AssertTimeLog(results.Item(4), "03:00:00", "23:59:00", "2018-06-05")
        AssertTimeLog(results.Item(5), "01:00:00", "23:00:00", "2018-06-06")
        AssertTimeLog(results.Item(6), "02:00:00", "23:30:00", "2018-06-07")
    End Sub

    <Test>
    Public Sub ShouldImport_WithNextShiftScheduleWithoutShift() _
        Implements ITimeAnalyzer.ShouldImport_WithNextShiftScheduleWithoutShift

        Dim logs = New List(Of ImportTimeAttendanceLog)(GetParsedTimeLogs())

        Dim employeeShifts As List(Of EmployeeDutySchedule) = GetSampleEmployeeDutySchedules_WithNextShiftScheduleWithoutShift()
        Dim employees As List(Of Employee) = GetSampleEmployees()

        Dim employeeOvertimes As List(Of Overtime) = GetSampleEmployeeOvertimes()

        Dim timeAttendanceHelper = New TimeAttendanceHelperNew(logs, employees, employeeShifts, employeeOvertimes)

        logs = timeAttendanceHelper.Analyze()

        Dim results = timeAttendanceHelper.GenerateTimeLogs()

        AssertTimeLog(results.Item(0), "08:30:00", "18:00:00", "2018-06-01")
        AssertTimeLog(results.Item(1), "07:00:00", "01:00:00", "2018-06-02")
        AssertTimeLog(results.Item(2), "06:00:00", "20:00:00", "2018-06-03")
        AssertTimeLog(results.Item(3), "05:00:00", "03:00:00", "2018-06-04")
        AssertTimeLog(results.Item(4), "04:00:00", "01:00:00", "2018-06-05")
        AssertTimeLog(results.Item(5), "08:00:00", "23:00:00", "2018-06-06")
    End Sub

End Class