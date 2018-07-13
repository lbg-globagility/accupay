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
        Dim currentShift = GetShift("8:30", "17:30", "2017-01-10")

        Dim overtime = New Overtime With {
            .Start = Date.Parse("2018-01-01 17:30"),
            .End = Date.Parse("2018-01-01 19:30")
        }

        Dim workStart = Date.Parse($"2018-01-01 {timeIn}")
        Dim workEnd = Date.Parse($"2018-01-01 {timeOut}")

        Dim result = _calculator.ComputeOvertimeHours(workStart, workEnd, overtime, currentShift)

        Assert.That(result, [Is].EqualTo(answer))
    End Sub

    Private Function GetShift(timeIn As String, timeOut As String, [date] As String) As CurrentShift
        Dim shift = New Shift(TimeSpan.Parse(timeIn), TimeSpan.Parse(timeOut))
        Return New CurrentShift(shift, Date.Parse([date]))
    End Function

End Class
