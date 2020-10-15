Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers

<TestFixture>
Public Class CurrentShiftTest

    <Test>
    Public Sub ShouldShiftTest1()

        Dim currentDay = New DateTime(2017, 1, 1)

        Dim shift = New EmployeeDutySchedule() With {
            .DateSched = currentDay,
            .StartTime = TimeSpan.Parse("8:00"),
            .EndTime = TimeSpan.Parse("18:00"),
            .BreakStartTime = TimeSpan.Parse("12:00"),
            .BreakLength = 1
        }

        Dim currentShift = New CurrentShift(shift, currentDay)

        Dim correctStart = Date.Parse("2017-01-01 12:00")
        Assert.AreEqual(correctStart, currentShift.BreaktimeStart)

        Dim correctEnd = Date.Parse("2017-01-01 13:00")
        Assert.AreEqual(correctEnd, currentShift.BreaktimeEnd)
    End Sub

    <Test>
    Public Sub ShouldShiftTest2()

        Dim currentDay = New DateTime(2017, 1, 1)

        Dim shift = New EmployeeDutySchedule() With {
            .DateSched = currentDay,
            .StartTime = TimeSpan.Parse("18:00"),
            .EndTime = TimeSpan.Parse("3:00"),
            .BreakStartTime = TimeSpan.Parse("23:00"),
            .BreakLength = 1
        }

        Dim currentShift = New CurrentShift(shift, currentDay)

        Dim correctStart = Date.Parse("2017-01-01 23:00")
        Assert.AreEqual(correctStart, currentShift.BreaktimeStart)

        Dim correctEnd = Date.Parse("2017-01-02 00:00")
        Assert.AreEqual(correctEnd, currentShift.BreaktimeEnd)
    End Sub

    <Test>
    Public Sub ShouldShiftTest3()
        Dim currentDay = New DateTime(2017, 1, 1)

        Dim shift = New EmployeeDutySchedule() With {
            .DateSched = currentDay,
            .StartTime = TimeSpan.Parse("18:00"),
            .EndTime = TimeSpan.Parse("3:00"),
            .BreakStartTime = TimeSpan.Parse("0:00"),
            .BreakLength = 1
        }

        Dim currentShift = New CurrentShift(shift, currentDay)

        Dim correctStart = Date.Parse("2017-01-02 00:00")
        Assert.AreEqual(correctStart, currentShift.BreaktimeStart)

        Dim correctEnd = Date.Parse("2017-01-02 01:00")
        Assert.AreEqual(correctEnd, currentShift.BreaktimeEnd)
    End Sub

End Class