Option Strict On
Imports AccuPay
Imports AccuPay.Entity

<TestFixture>
Public Class CurrentShiftTest

    <Test>
    Public Sub ShouldShiftTest1()
        Dim shift = New Shift() With {
            .TimeFrom = TimeSpan.Parse("8:00"),
            .TimeTo = TimeSpan.Parse("18:00"),
            .BreaktimeFrom = TimeSpan.Parse("12:00"),
            .BreaktimeTo = TimeSpan.Parse("13:00")
        }

        Dim currentDay = New DateTime(2017, 1, 1)
        Dim currentShift = New CurrentShift(shift, currentDay)

        Dim correctStart = Date.Parse("2017-01-01 12:00")
        Assert.AreEqual(correctStart, currentShift.BreaktimeStart)

        Dim correctEnd = Date.Parse("2017-01-01 13:00")
        Assert.AreEqual(correctEnd, currentShift.BreaktimeEnd)
    End Sub

    <Test>
    Public Sub ShouldShiftTest2()
        Dim shift = New Shift() With {
            .TimeFrom = TimeSpan.Parse("18:00"),
            .TimeTo = TimeSpan.Parse("3:00"),
            .BreaktimeFrom = TimeSpan.Parse("23:00"),
            .BreaktimeTo = TimeSpan.Parse("0:00")
        }

        Dim currentDay = New DateTime(2017, 1, 1)
        Dim currentShift = New CurrentShift(shift, currentDay)

        Dim correctStart = DateTime.Parse("2017-01-01 23:00")
        Assert.AreEqual(correctStart, currentShift.BreaktimeStart)

        Dim correctEnd = DateTime.Parse("2017-01-02 00:00")
        Assert.AreEqual(correctEnd, currentShift.BreaktimeEnd)
    End Sub

    <Test>
    Public Sub ShouldShiftTest3()
        Dim shift = New Shift() With {
            .TimeFrom = TimeSpan.Parse("18:00"),
            .TimeTo = TimeSpan.Parse("3:00"),
            .BreaktimeFrom = TimeSpan.Parse("0:00"),
            .BreaktimeTo = TimeSpan.Parse("1:00")
        }

        Dim currentDay = New DateTime(2017, 1, 1)
        Dim currentShift = New CurrentShift(shift, currentDay)

        Dim correctStart = DateTime.Parse("2017-01-02 00:00")
        Assert.AreEqual(correctStart, CurrentShift.BreaktimeStart)

        Dim correctEnd = DateTime.Parse("2017-01-02 01:00")
        Assert.AreEqual(correctEnd, CurrentShift.BreaktimeEnd)
    End Sub

End Class
