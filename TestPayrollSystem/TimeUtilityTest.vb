Imports System.Text
Imports Acupay

<TestFixture>
Public Class TimeUtilityTest

    <Test>
    Public Sub Should_Combine_Date_And_TimeSpan()
        Dim day = Date.Parse("2017-01-01")
        Dim time = TimeSpan.Parse("10:30:12")

        Dim expected = Date.Parse("2017-01-01 10:30:12")
        Dim answer = TimeUtility.Combine(day, time)

        Assert.AreEqual(expected, answer)
    End Sub

    <Test>
    Public Sub Range_Start_Works_For_OneDay()
        Dim day = Date.Parse("2017-01-01")
        Dim time = TimeSpan.Parse("10:30:12")

        Dim expected = Date.Parse("2017-01-01 10:30:12")
        Dim answer = TimeUtility.RangeStart(day, time)

        Assert.AreEqual(expected, answer)
    End Sub

    <Test>
    Public Sub Range_End_Works_For_OneDay()
        Dim day = Date.Parse("2017-01-01")
        Dim timeIn = TimeSpan.Parse("10:30:12")
        Dim timeOut = TimeSpan.Parse("16:15:00")

        Dim expected = Date.Parse("2017-01-01 16:15:00")
        Dim answer = TimeUtility.RangeEnd(day, timeIn, timeOut)

        Assert.AreEqual(expected, answer)
    End Sub

    <Test>
    Public Sub Range_End_Works_Across_TwoDays()
        Dim day = Date.Parse("2017-01-01")
        Dim timeIn = TimeSpan.Parse("16:00:00")
        Dim timeOut = TimeSpan.Parse("02:00:00")

        Dim expected = Date.Parse("2017-01-02 02:00:00")
        Dim answer = TimeUtility.RangeEnd(day, timeIn, timeOut)

        Assert.AreEqual(expected, answer)
    End Sub

End Class
