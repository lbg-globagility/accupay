Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Services
Imports AccuPay.Data.ValueObjects

<TestFixture>
Public Class TimeEntryCalculatorTest

    <Test>
    Public Sub ShouldNotBeLate()
        Dim calculator = New TimeEntryCalculator()

        Dim workStart = Date.Parse("2017-01-01 08:30:00")
        Dim workEnd = Date.Parse("2017-01-01 17:30:00")
        Dim workPeriod = New TimePeriod(workStart, workEnd)

        Dim shift = New EmployeeDutySchedule() With {
            .DateSched = Date.Parse("2017-01-01"),
            .StartTime = TimeSpan.Parse("08:30"),
            .EndTime = TimeSpan.Parse("17:30")
        }

        Dim currentShift = New CurrentShift(shift, Date.Parse("2017-01-01"))

        Dim result = calculator.ComputeLateHours(workPeriod, currentShift, False)
        Assert.AreEqual(0D, result)
    End Sub

    <Test>
    Public Sub ShouldBeLate()
        Dim calculator = New TimeEntryCalculator()

        Dim workStart = Date.Parse("2017-01-01 09:00:00")
        Dim workEnd = Date.Parse("2017-01-01 17:30:00")
        Dim workPeriod = New TimePeriod(workStart, workEnd)

        Dim shift = New EmployeeDutySchedule() With {
            .DateSched = Date.Parse("2017-01-01"),
            .StartTime = TimeSpan.Parse("08:30"),
            .EndTime = TimeSpan.Parse("17:30")
        }

        Dim currentShift = New CurrentShift(shift, Date.Parse("2017-01-01"))

        Dim result = calculator.ComputeLateHours(workPeriod, currentShift, False)
        Assert.AreEqual(0.5D, result)
    End Sub

    <Test>
    Public Sub ShouldBeLateBreaktime()
        Dim calculator = New TimeEntryCalculator()

        Dim workStart = Date.Parse("2017-01-01 12:30:00")
        Dim workEnd = Date.Parse("2017-01-01 16:30:00")
        Dim workPeriod = New TimePeriod(workStart, workEnd)

        Dim shift = New EmployeeDutySchedule() With {
            .DateSched = Date.Parse("2017-01-01"),
            .StartTime = TimeSpan.Parse("08:30"),
            .EndTime = TimeSpan.Parse("17:30"),
            .BreakStartTime = TimeSpan.Parse("12:00"),
            .BreakLength = 1
        }

        Dim currentShift = New CurrentShift(shift, Date.Parse("2017-01-01"))

        Dim result = calculator.ComputeLateHours(workPeriod, currentShift, False)
        Assert.AreEqual(3.5D, result)
    End Sub

    <Test>
    Public Sub ShouldBeLateAfterBreaktime()
        Dim calculator = New TimeEntryCalculator()

        Dim workStart = Date.Parse("2017-01-01 14:00:00")
        Dim workEnd = Date.Parse("2017-01-01 17:30:00")
        Dim workPeriod = New TimePeriod(workStart, workEnd)

        Dim shift = New EmployeeDutySchedule() With {
            .DateSched = Date.Parse("2017-01-01"),
            .StartTime = TimeSpan.Parse("08:30"),
            .EndTime = TimeSpan.Parse("17:30"),
            .BreakStartTime = TimeSpan.Parse("12:00"),
            .BreakLength = 1
        }

        Dim currentShift = New CurrentShift(shift, Date.Parse("2017-01-01"))

        Dim result = calculator.ComputeLateHours(workPeriod, currentShift, False)
        Assert.AreEqual(4.5D, result)
    End Sub

End Class