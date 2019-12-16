Option Strict On

Imports AccuPay.Tools

<TestFixture>
Public Class CalendarTest

    <TestCase("6:30 AM")>
    <TestCase("6:30 a")>
    <TestCase("630 a")>
    <TestCase("6:30")>
    <TestCase("630")>
    <TestCase("6 30")>
    <TestCase("6.30")>
    <TestCase("6:3 AM")>
    <TestCase("6:3 a")>
    <TestCase("6:3")>
    <TestCase("6 3")>
    <TestCase("6.3")>
    Public Sub Should_Parse_TimeSpan_Of_Format(text As String)
        Dim expected = New TimeSpan(6, 30, 0)
        Dim result = Calendar.ToTimespan(text)

        Assert.AreEqual(expected, result)
    End Sub

    <TestCase("6", "06:00")>
    <TestCase("12", "12:00")>
    <TestCase("15", "15:00")>
    <TestCase("6 AM", "06:00")>
    <TestCase("6 PM", "18:00")>
    Public Sub Should_Parse_TimeSpan_Of_Only_Hour(text As String, correct As String)
        Dim expected = TimeSpan.Parse(correct)
        Dim result = Calendar.ToTimespan(text)

        Assert.AreEqual(expected, result)
    End Sub

    <TestCase("6:30 AM", "06:30")>
    <TestCase("6:30 PM", "18:30")>
    <TestCase("12:00 AM", "00:00")>
    <TestCase("12:00 PM", "12:00")>
    Public Sub Should_Parse_TimeSpan_Of_Correct_Clock(text As String, correct As String)
        Dim expected = TimeSpan.Parse(correct)
        Dim result = Calendar.ToTimespan(text)

        Assert.AreEqual(expected, result)
    End Sub

    <TestCase("")>
    <TestCase("  ")>
    <TestCase("2500")>
    <TestCase("22:30 AM")>
    Public Sub Should_Fail_And_Return_Null(text As String)
        Dim expected = Calendar.ToTimespan(text)

        Assert.IsNull(expected)
    End Sub

End Class
