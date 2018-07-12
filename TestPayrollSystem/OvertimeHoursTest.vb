Option Strict On
Imports AccuPay
Imports AccuPay.Entity

<TestFixture>
Public Class OvertimeHoursTest

    Private _calculator As TimeEntryCalculator

    <OneTimeSetUp>
    Public Sub Setup()
        _calculator = New TimeEntryCalculator()
    End Sub

    <TestCase("8:30", "19:30", 2)>
    <TestCase("8:30", "19:00", 1.5)>
    <TestCase("8:30", "20:30", 2)>
    Public Sub Should_Calculate_Overtimes_For_Daytime_Hours(timeIn As String, timeOut As String, answer As Decimal)
        Dim shift = New Shift(TimeSpan.Parse("8:30"), TimeSpan.Parse("17:30"))
        Dim currentShift = New CurrentShift(shift, Date.Parse("2017-01-10"))

        Dim overtime = New Overtime With {
            .Start = Date.Parse("2018-01-01 17:30"),
            .End = Date.Parse("2018-01-01 19:30")
        }

        Dim workStart = Date.Parse($"2018-01-01 {timeIn}")
        Dim workEnd = Date.Parse($"2018-01-01 {timeOut}")

        Dim result = _calculator.ComputeOvertimeHours(workStart, workEnd, overtime, currentShift)

        Assert.AreEqual(answer, result)
    End Sub

End Class
