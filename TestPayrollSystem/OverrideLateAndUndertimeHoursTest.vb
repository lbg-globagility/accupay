Option Strict On

Imports AccuPay

<TestFixture>
Public Class OverrideLateAndUndertimeHoursTest

#Region "Normal duty"

    <Test>
    Public Sub ShouldHaveNoLateOrUndertime_WithNormalDutyPeriodWholeDay()

        Dim lateHours = 0
        Dim undertimeHours = 0

        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        Dim leavePeriod As TimePeriod = Nothing

        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(output.Item1, lateHours)
        Assert.AreEqual(output.Item2, undertimeHours)

    End Sub

    <Test>
    Public Sub ShouldHaveLate_WithNormalDutyPeriod()

        Dim lateHours = 0.75
        Dim undertimeHours = 0

        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 45, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        Dim leavePeriod As TimePeriod = Nothing

        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(output.Item1, lateHours)
        Assert.AreEqual(output.Item2, undertimeHours)

    End Sub

    <Test>
    Public Sub ShouldHaveUndertime_WithNormalDutyPeriod()

        Dim lateHours = 0
        Dim undertimeHours = 0.75

        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 17, 15, 0))

        Dim leavePeriod As TimePeriod = Nothing

        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(output.Item1, lateHours)
        Assert.AreEqual(output.Item2, undertimeHours)

    End Sub

    <Test>
    Public Sub ShouldHaveLateAndUndertime_WithNormalDutyPeriod()

        Dim lateHours = 0.75
        Dim undertimeHours = 0.75

        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 45, 0),
                        New Date(2019, 6, 24, 17, 15, 0))

        Dim leavePeriod As TimePeriod = Nothing

        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(output.Item1, lateHours)
        Assert.AreEqual(output.Item2, undertimeHours)

    End Sub

#End Region

#Region "Duty Then leave"

    <Test>
    Public Sub ShouldHaveLateAndUndertime_WithDutyAndLeavePeriod()

        Dim lateHours = 2
        Dim undertimeHours = 4

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '11am-12pm
        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 11, 0, 0),
                        New Date(2019, 6, 24, 12, 0, 0))

        '1pm-2pm
        Dim leavePeriod = New TimePeriod(
                        New Date(2019, 6, 24, 13, 0, 0),
                        New Date(2019, 6, 24, 14, 0, 0))

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(output.Item1, lateHours)
        Assert.AreEqual(output.Item2, undertimeHours)

    End Sub

    <Test>
    Public Sub ShouldHaveInBetweenUndertime_WithDutyAndLeavePeriod()

        Dim lateHours = 0
        Dim undertimeHours = 1

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '9am-12pm
        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 12, 0, 0))

        '2pm-6pm
        Dim leavePeriod = New TimePeriod(
                        New Date(2019, 6, 24, 14, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(lateHours, output.Item1)
        Assert.AreEqual(undertimeHours, output.Item2)

    End Sub

    <Test>
    Public Sub ShouldHaveLateAndInBetweenUndertime_WithDutyAndLeavePeriod()

        Dim lateHours = 1.5
        Dim undertimeHours = 2

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '10:30am-1:30pm
        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 10, 30, 0),
                        New Date(2019, 6, 24, 13, 30, 0))

        '3:30pm-6pm
        Dim leavePeriod = New TimePeriod(
                        New Date(2019, 6, 24, 15, 30, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(lateHours, output.Item1)
        Assert.AreEqual(undertimeHours, output.Item2)

    End Sub

    <Test>
    Public Sub ShouldHaveLateAndBeforeAndAfterLeaveUndertime_WithDutyAndLeavePeriod()

        Dim lateHours = 1.5
        Dim undertimeHours = 2.25

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '10:30am-1:30pm
        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 10, 30, 0),
                        New Date(2019, 6, 24, 13, 30, 0))

        '3:30pm-5:45pm
        Dim leavePeriod = New TimePeriod(
                        New Date(2019, 6, 24, 15, 30, 0),
                        New Date(2019, 6, 24, 17, 45, 0))

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(lateHours, output.Item1)
        Assert.AreEqual(undertimeHours, output.Item2)

    End Sub

    <Test>
    Public Sub ShouldHaveLateAndAfterLeaveUndertime_WithoutDutyPeriod()

        Dim lateHours = 5.5
        Dim undertimeHours = 0.25

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        'was absent but with leave
        Dim dutyPeriod As TimePeriod = Nothing

        '3:30pm-5:45pm
        Dim leavePeriod = New TimePeriod(
                        New Date(2019, 6, 24, 15, 30, 0),
                        New Date(2019, 6, 24, 17, 45, 0))

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(lateHours, output.Item1)
        Assert.AreEqual(undertimeHours, output.Item2)

    End Sub

#End Region

#Region "Leave first then duty"

    <Test>
    Public Sub ShouldHaveLateAndUndertime_WithLeaveFirstThenDuty()

        Dim lateHours = 2
        Dim undertimeHours = 4

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '1pm-2pm
        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 13, 0, 0),
                        New Date(2019, 6, 24, 14, 0, 0))

        '11am-12pm
        Dim leavePeriod = New TimePeriod(
                        New Date(2019, 6, 24, 11, 0, 0),
                        New Date(2019, 6, 24, 12, 0, 0))

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(output.Item1, lateHours)
        Assert.AreEqual(output.Item2, undertimeHours)

    End Sub

    <Test>
    Public Sub ShouldHaveInBetweenUndertime_WithLeaveFirstThenDuty()

        Dim lateHours = 0
        Dim undertimeHours = 1

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '2pm-6pm
        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 14, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '9am-12pm
        Dim leavePeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 12, 0, 0))

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(lateHours, output.Item1)
        Assert.AreEqual(undertimeHours, output.Item2)

    End Sub

    <Test>
    Public Sub ShouldHaveLateAndInBetweenUndertime_WithLeaveFirstThenDuty()

        Dim lateHours = 1.5
        Dim undertimeHours = 2

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '3:30pm-6pm
        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 15, 30, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '10:30am-1:30pm
        Dim leavePeriod = New TimePeriod(
                        New Date(2019, 6, 24, 10, 30, 0),
                        New Date(2019, 6, 24, 13, 30, 0))

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(lateHours, output.Item1)
        Assert.AreEqual(undertimeHours, output.Item2)

    End Sub

    <Test>
    Public Sub ShouldHaveLateAndBeforeAndAfterLeaveUndertime_WithLeaveFirstThenDuty()

        Dim lateHours = 1.5
        Dim undertimeHours = 2.25

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '3:30pm-5:45pm
        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 15, 30, 0),
                        New Date(2019, 6, 24, 17, 45, 0))

        '10:30am-1:30pm
        Dim leavePeriod = New TimePeriod(
                        New Date(2019, 6, 24, 10, 30, 0),
                        New Date(2019, 6, 24, 13, 30, 0))

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(lateHours, output.Item1)
        Assert.AreEqual(undertimeHours, output.Item2)

    End Sub

    <Test>
    Public Sub ShouldHaveLateAndAfterDutyUndertime_WithOutLeave()

        Dim lateHours = 5.5
        Dim undertimeHours = 0.25

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '3:30pm-5:45pm
        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 15, 30, 0),
                        New Date(2019, 6, 24, 17, 45, 0))

        'was absent but with leave
        Dim leavePeriod As TimePeriod = Nothing

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(lateHours, output.Item1)
        Assert.AreEqual(undertimeHours, output.Item2)

    End Sub

#End Region

#Region "Duty then leave with overlap"

    <Test>
    Public Sub ShouldHaveLateAndUndertime_WithDutyAndLeaveOverlap()

        Dim lateHours = 2
        Dim undertimeHours = 4

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '11am-1:30pm
        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 11, 0, 0),
                        New Date(2019, 6, 24, 13, 30, 0))

        '1pm-2pm
        Dim leavePeriod = New TimePeriod(
                        New Date(2019, 6, 24, 13, 0, 0),
                        New Date(2019, 6, 24, 14, 0, 0))

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(output.Item1, lateHours)
        Assert.AreEqual(output.Item2, undertimeHours)

    End Sub

    <Test>
    Public Sub ShouldHaveInBetweenUndertime_WithDutyAndLeaveOverlap()

        Dim lateHours = 0
        Dim undertimeHours = 0

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '9am-2:30pm
        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 14, 30, 0))

        '2pm-6pm
        Dim leavePeriod = New TimePeriod(
                        New Date(2019, 6, 24, 14, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(lateHours, output.Item1)
        Assert.AreEqual(undertimeHours, output.Item2)

    End Sub

    <Test>
    Public Sub ShouldHaveLateAndInBetweenUndertime_WithDutyAndLeaveOverlap()

        Dim lateHours = 1.5
        Dim undertimeHours = 0

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '10:30am-4pm
        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 10, 30, 0),
                        New Date(2019, 6, 24, 16, 0, 0))

        '3:30pm-6pm
        Dim leavePeriod = New TimePeriod(
                        New Date(2019, 6, 24, 15, 30, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(lateHours, output.Item1)
        Assert.AreEqual(undertimeHours, output.Item2)

    End Sub

    <Test>
    Public Sub ShouldHaveLateAndBeforeAndAfterLeaveUndertime_WithDutyAndLeaveOverlap()

        Dim lateHours = 1.5
        Dim undertimeHours = 0.25

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '10:30am-4pm
        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 10, 30, 0),
                        New Date(2019, 6, 24, 16, 0, 0))

        '3:30pm-5:45pm
        Dim leavePeriod = New TimePeriod(
                        New Date(2019, 6, 24, 15, 30, 0),
                        New Date(2019, 6, 24, 17, 45, 0))

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(lateHours, output.Item1)
        Assert.AreEqual(undertimeHours, output.Item2)

    End Sub

#End Region

#Region "Leave first then duty with overlap"

    <Test>
    Public Sub ShouldHaveLateAndUndertime_WithLeaveFirstThenDutyOverlap()

        Dim lateHours = 2
        Dim undertimeHours = 4

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '1pm-2pm
        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 13, 0, 0),
                        New Date(2019, 6, 24, 14, 0, 0))

        '11am-1:30pm
        Dim leavePeriod = New TimePeriod(
                        New Date(2019, 6, 24, 11, 0, 0),
                        New Date(2019, 6, 24, 13, 30, 0))

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(output.Item1, lateHours)
        Assert.AreEqual(output.Item2, undertimeHours)

    End Sub

    <Test>
    Public Sub ShouldHaveInBetweenUndertime_WithLeaveFirstThenDutyOverlap()

        Dim lateHours = 0
        Dim undertimeHours = 0

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '2pm-6pm
        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 14, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '9am-2:30pm
        Dim leavePeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 14, 30, 0))

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(lateHours, output.Item1)
        Assert.AreEqual(undertimeHours, output.Item2)

    End Sub

    <Test>
    Public Sub ShouldHaveLateAndInBetweenUndertime_WithLeaveFirstThenDutyOverlap()

        Dim lateHours = 1.5
        Dim undertimeHours = 0

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '3:30pm-6pm
        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 15, 30, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '10:30am-4pm
        Dim leavePeriod = New TimePeriod(
                        New Date(2019, 6, 24, 10, 30, 0),
                        New Date(2019, 6, 24, 16, 0, 0))

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(lateHours, output.Item1)
        Assert.AreEqual(undertimeHours, output.Item2)

    End Sub

    <Test>
    Public Sub ShouldHaveLateAndBeforeAndAfterLeaveUndertime_WithLeaveFirstThenDutyOverlap()

        Dim lateHours = 1.5
        Dim undertimeHours = 0.25

        '9am-6pm
        Dim shiftPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 9, 0, 0),
                        New Date(2019, 6, 24, 18, 0, 0))

        '3:30pm-5:45pm
        Dim dutyPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 15, 30, 0),
                        New Date(2019, 6, 24, 17, 45, 0))

        '10:30am-4pm
        Dim leavePeriod = New TimePeriod(
                        New Date(2019, 6, 24, 10, 30, 0),
                        New Date(2019, 6, 24, 16, 0, 0))

        '12pm-1pm
        Dim breakPeriod = New TimePeriod(
                        New Date(2019, 6, 24, 12, 0, 0),
                        New Date(2019, 6, 24, 13, 0, 0))

        Dim computeBreakTimeLatePolicy As Boolean = False

        Dim output = DayCalculator.ComputeLateAndUndertimeHours(
                                        shiftPeriod,
                                        dutyPeriod,
                                        leavePeriod,
                                        breakPeriod,
                                        computeBreakTimeLatePolicy)

        Assert.AreEqual(lateHours, output.Item1)
        Assert.AreEqual(undertimeHours, output.Item2)

    End Sub

#End Region

End Class