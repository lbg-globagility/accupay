Option Strict On
Imports AccuPay
Imports AccuPay.Entity

<TestFixture>
Public Class UndertimeHoursTest

    Public Property _calculator As TimeEntryCalculator

    <OneTimeSetUp>
    Public Sub Setup()
        _calculator = New TimeEntryCalculator()
    End Sub

    <TestCase("8:00", "10:00", 8)>
    <TestCase("8:00", "12:00", 6)>
    <TestCase("8:00", "12:30", 6)>
    <TestCase("8:00", "13:00", 6)>
    <TestCase("8:00", "15:00", 4)>
    <TestCase("8:00", "18:00", 1)>
    Public Sub Should_Compute_Correct_Undertime_Hours(timeIn As String, timeOut As String, answer As Decimal)
        Dim shift = New Shift(
            TimeSpan.Parse("8:00"), TimeSpan.Parse("19:00"),
            TimeSpan.Parse("12:00"), TimeSpan.Parse("13:00"))
        Dim currentShift = New CurrentShift(shift, Date.Parse("2018-01-01"))

        Dim workStart = Date.Parse($"2018-01-01 {timeIn}")
        Dim workEnd = Date.Parse($"2018-01-01 {timeOut}")
        Dim workPeriod = New TimePeriod(workStart, workEnd)

        Dim result = _calculator.ComputeUndertimeHours(workPeriod, currentShift, False)

        Assert.That(result, [Is].EqualTo(answer))
    End Sub

End Class
