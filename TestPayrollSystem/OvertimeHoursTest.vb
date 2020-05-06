Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Services
Imports AccuPay.Data.ValueObjects
Imports AccuPay.Utilities

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
    Public Sub Should_Calculate_Overtimes_For_Daytime_Hours(timeIn As String, timeOut As String, expected As Decimal)
        CorrectOvertimeHours(
            shiftStartTime:="8:30",
            shiftEndTime:="17:30",
            otStartTime:="17:30",
            otEndTime:="19:30",
            timeIn:=timeIn,
            timeOut:=timeOut,
            expected:=expected)
    End Sub

    <TestCase("13:00", "00:00", 2)>
    <TestCase("13:00", "23:30", 1.5)>
    <TestCase("13:00", "1:00", 2)>
    Public Sub Should_Calculate_Overtimes_For_Ending_On_Midnight(timeIn As String, timeOut As String, expected As Decimal)
        CorrectOvertimeHours(
            shiftStartTime:="13:00",
            shiftEndTime:="22:00",
            otStartTime:="22:00",
            otEndTime:="00:00",
            timeIn:=timeIn,
            timeOut:=timeOut,
            expected:=expected)
    End Sub

    <TestCase("14:30", "1:30", 2)>
    <TestCase("14:30", "1:00", 1.5)>
    <TestCase("14:30", "2:00", 2)>
    Public Sub Should_Calculate_Overtimes_For_Ending_The_Next_Day(timeIn As String, timeOut As String, expected As Decimal)
        CorrectOvertimeHours(
            shiftStartTime:="14:30",
            shiftEndTime:="23:30",
            otStartTime:="23:30",
            otEndTime:="1:30",
            timeIn:=timeIn,
            timeOut:=timeOut,
            expected:=expected)
    End Sub

    <TestCase("15:00", "2:00", 2)>
    <TestCase("15:00", "1:30", 1.5)>
    <TestCase("15:00", "2:30", 2)>
    Public Sub Should_Calculate_Overtimes_For_Starting_The_Next_Day(timeIn As String, timeOut As String, expected As Decimal)
        CorrectOvertimeHours(
            shiftStartTime:="15:00",
            shiftEndTime:="00:00",
            otStartTime:="00:00",
            otEndTime:="2:00",
            timeIn:=timeIn,
            timeOut:=timeOut,
            expected:=expected)
    End Sub

    <TestCase("17:00", "4:00", 2)>
    <TestCase("17:00", "3:30", 1.5)>
    <TestCase("17:00", "4:30", 2)>
    Public Sub Should_Calculate_Overtimes_Staring_On_Midnight(timeIn As String, timeOut As String, expected As Decimal)
        CorrectOvertimeHours(
            shiftStartTime:="17:00",
            shiftEndTime:="2:00",
            otStartTime:="2:00",
            otEndTime:="4:00",
            timeIn:=timeIn,
            timeOut:=timeOut,
            expected:=expected)
    End Sub

    <TestCase("6:30", "17:30", 2)>
    <TestCase("7:00", "17:30", 1.5)>
    <TestCase("6:00", "17:30", 2)>
    Public Sub Should_Calculate_Overtimes_For_PreShift(timeIn As String, timeOut As String, expected As Decimal)
        CorrectOvertimeHours(
            shiftStartTime:="8:30",
            shiftEndTime:="17:30",
            otStartTime:="6:30",
            otEndTime:="8:30",
            timeIn:=timeIn,
            timeOut:=timeOut,
            expected:=expected)
    End Sub

    <TestCase("22:00", "9:00", 2)>
    <TestCase("22:30", "9:00", 1.5)>
    <TestCase("21:30", "9:00", 2)>
    Public Sub Should_Calculate_Overtimes_For_PreShift_On_The_Previous_Day(timeIn As String, timeOut As String, expected As Decimal)
        CorrectOvertimeHours(
            shiftStartTime:="00:00",
            shiftEndTime:="9:00",
            otStartTime:="22:00",
            otEndTime:="0:00",
            timeIn:=timeIn,
            timeOut:=timeOut,
            expected:=expected)
    End Sub

    Private Sub CorrectOvertimeHours(
        shiftStartTime As String,
        shiftEndTime As String,
        otStartTime As String,
        otEndTime As String,
        timeIn As String,
        timeOut As String,
        expected As Decimal)

        Dim today = Date.Parse("2017-01-01")
        Dim currentShift = GetShift(shiftStartTime, shiftEndTime, today)

        Dim overtime = New Overtime With {
            .OTStartDate = today,
            .OTStartTime = TimeSpan.Parse(otStartTime),
            .OTEndTime = TimeSpan.Parse(otEndTime)
        }

        Dim workStart = TimeUtility.RangeStart(today, TimeSpan.Parse(timeIn))
        Dim workEnd = TimeUtility.RangeEnd(today, TimeSpan.Parse(timeIn), TimeSpan.Parse(timeOut))
        Dim workPeriod = New TimePeriod(workStart, workEnd)

        Dim result = _calculator.ComputeOvertimeHours(workPeriod, overtime, currentShift, Nothing)

        Assert.That(result, [Is].EqualTo(expected))
    End Sub

    Private Function GetShift(timeIn As String, timeOut As String, [date] As Date) As CurrentShift
        Dim shift = New Shift(TimeSpan.Parse(timeIn), TimeSpan.Parse(timeOut))
        Return New CurrentShift(shift, [date])
    End Function

End Class