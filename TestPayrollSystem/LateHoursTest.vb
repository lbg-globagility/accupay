Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Services
Imports AccuPay.Data.ValueObjects

<TestFixture>
Public Class LateHoursTest

    Private _calculator As TimeEntryCalculator

    <OneTimeSetUp>
    Public Sub Setup()
        _calculator = New TimeEntryCalculator()
    End Sub

    <TestCase("10:00", "19:00", 2)>
    <TestCase("12:00", "19:00", 4)>
    <TestCase("12:30", "19:00", 4)>
    <TestCase("13:00", "19:00", 4)>
    <TestCase("15:00", "19:00", 6)>
    <TestCase("17:00", "19:00", 8)>
    Public Sub Should_Compute_Correct_Late_Hours(timeIn As String, timeOut As String, answer As Decimal)
        Dim shift = New Shift(
            TimeSpan.Parse("8:00"), TimeSpan.Parse("19:00"),
            TimeSpan.Parse("12:00"), TimeSpan.Parse("13:00"))
        Dim currentShift = New CurrentShift(shift, Date.Parse("2018-01-01"))

        Dim workStart = Date.Parse($"2018-01-01 {timeIn}")
        Dim workEnd = Date.Parse($"2018-01-01 {timeOut}")
        Dim workPeriod = New TimePeriod(workStart, workEnd)

        Dim result = _calculator.ComputeLateHours(workPeriod, currentShift, False)

        Assert.That(result, [Is].EqualTo(answer))
    End Sub

End Class