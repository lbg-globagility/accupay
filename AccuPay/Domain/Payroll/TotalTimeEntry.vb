Imports AccuPay.Data
Imports AccuPay.Entity
Imports AccuPay.Utilities
Imports PayrollSys

Public Class TotalTimeEntry

    Public Shared Function Calculate(timeEntries As ICollection(Of TimeEntry),
                                     salary As Salary,
                                     employee As IEmployee,
                                     actualtimeentries As ICollection(Of ActualTimeEntry)) _
                                     As TotalTimeEntry

        Return New TotalTimeEntry(timeEntries, salary, employee, actualtimeentries)
    End Function

    Private Sub New(timeEntries As ICollection(Of TimeEntry),
                    salary As Salary,
                    employee As IEmployee,
                    actualtimeentries As ICollection(Of ActualTimeEntry))

        _hourlyRate = PayrollTools.GetHourlyRateByDailyRate(salary, employee)
        _actualHourlyRate = PayrollTools.GetHourlyRateByDailyRate(salary, employee, isActual:=True)

        _regularHours = timeEntries.Sum(Function(t) t.RegularHours)
        _regularPay = _hourlyRate * _regularHours
        _actualRegularPay = _actualHourlyRate * _regularHours

        _overtimeHours = timeEntries.Sum(Function(t) t.OvertimeHours)
        _overtimePay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.OvertimePay))
        _actualOvertimePay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.OvertimePay))

        _nightDiffHours = timeEntries.Sum(Function(t) t.NightDiffHours)
        _nightDiffPay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.NightDiffPay))
        _actualNightDiffPay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.NightDiffPay))

        _nightDiffOvertimeHours = timeEntries.Sum(Function(t) t.NightDiffOTHours)
        _nightDiffOvertimePay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.NightDiffOTPay))
        _actualNightDiffOvertimePay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.NightDiffOTPay))

        _restDayHours = timeEntries.Sum(Function(t) t.RestDayHours)
        _restDayPay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.RestDayPay))
        _actualRestDayPay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.RestDayPay))

        _restDayOTHours = timeEntries.Sum(Function(t) t.RestDayOTHours)
        _restDayOTPay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.RestDayOTPay))
        _actualRestDayOTPay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.RestDayOTPay))

        _specialHolidayHours = timeEntries.Sum(Function(t) t.SpecialHolidayHours)
        _specialHolidayPay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.SpecialHolidayPay))
        _actualSpecialHolidayPay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.SpecialHolidayPay))

        _specialHolidayOTHours = timeEntries.Sum(Function(t) t.SpecialHolidayOTHours)
        _specialHolidayOTPay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.SpecialHolidayOTPay))
        _actualSpecialHolidayOTPay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.SpecialHolidayOTPay))

        _regularHolidayHours = timeEntries.Sum(Function(t) t.RegularHolidayHours)
        _regularHolidayPay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.RegularHolidayPay))
        _actualRegularHolidayPay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.RegularHolidayPay))

        _regularHolidayOTHours = timeEntries.Sum(Function(t) t.RegularHolidayOTHours)
        _regularHolidayOTPay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.RegularHolidayOTPay))
        _actualRegularHolidayOTPay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.RegularHolidayOTPay))

        _holidayPay = timeEntries.Sum(Function(t) t.HolidayPay)

        _leaveHours = timeEntries.Sum(Function(t) t.TotalLeaveHours)
        _leavePay = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.LeavePay))
        _actualLeavePay = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.LeavePay))

        _lateHours = timeEntries.Sum(Function(t) t.LateHours)
        _lateDeduction = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.LateDeduction))
        _actualLateDeduction = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.LateDeduction))

        _undertimeHours = timeEntries.Sum(Function(t) t.UndertimeHours)
        _undertimeDeduction = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.UndertimeDeduction))
        _actualUndertimeDeduction = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.UndertimeDeduction))

        _absentHours = timeEntries.Sum(Function(t) t.AbsentHours)
        _absenceDeduction = AccuMath.CommercialRound(timeEntries.Sum(Function(t) t.AbsentDeduction))
        _actualAbsenceDeduction = AccuMath.CommercialRound(actualtimeentries.Sum(Function(t) t.AbsentDeduction))

    End Sub

    Private _hourlyRate As Decimal

    Public ReadOnly Property HourlyRate As Decimal
        Get
            Return _hourlyRate
        End Get
    End Property

    Private _actualHourlyRate As Decimal

    Public ReadOnly Property ActualHourlyRate As Decimal
        Get
            Return _actualHourlyRate
        End Get
    End Property

    Private _regularHours As Decimal

    Public ReadOnly Property RegularHours As Decimal
        Get
            Return _regularHours
        End Get
    End Property

    Private _regularPay As Decimal

    Public ReadOnly Property RegularPay As Decimal
        Get
            Return _regularPay
        End Get
    End Property

    Private _actualRegularPay As Decimal

    Public ReadOnly Property ActualRegularPay As Decimal
        Get
            Return _actualRegularPay
        End Get
    End Property

    Private _overtimeHours As Decimal

    Public ReadOnly Property OvertimeHours As Decimal
        Get
            Return _overtimeHours
        End Get
    End Property

    Private _overtimePay As Decimal

    Public ReadOnly Property OvertimePay As Decimal
        Get
            Return _overtimePay
        End Get
    End Property

    Private _actualOvertimePay As Decimal

    Public ReadOnly Property ActualOvertimePay As Decimal
        Get
            Return _actualOvertimePay
        End Get
    End Property

    Private _nightDiffHours As Decimal

    Public ReadOnly Property NightDiffHours As Decimal
        Get
            Return _nightDiffHours
        End Get
    End Property

    Private _nightDiffPay As Decimal

    Public ReadOnly Property NightDiffPay As Decimal
        Get
            Return _nightDiffPay
        End Get
    End Property

    Private _actualNightDiffPay As Decimal

    Public ReadOnly Property ActualNightDiffPay As Decimal
        Get
            Return _actualNightDiffPay
        End Get
    End Property

    Private _nightDiffOvertimeHours As Decimal

    Public ReadOnly Property NightDiffOvertimeHours As Decimal
        Get
            Return _nightDiffOvertimeHours
        End Get
    End Property

    Private _nightDiffOvertimePay As Decimal

    Public ReadOnly Property NightDiffOvertimePay As Decimal
        Get
            Return _nightDiffOvertimePay
        End Get
    End Property

    Private _actualNightDiffOvertimePay As Decimal

    Public ReadOnly Property ActualNightDiffOvertimePay As Decimal
        Get
            Return _actualNightDiffOvertimePay
        End Get
    End Property

    Private _restDayHours As Decimal

    Public ReadOnly Property RestDayHours As Decimal
        Get
            Return _restDayHours
        End Get
    End Property

    Private _restDayPay As Decimal

    Public ReadOnly Property RestDayPay As Decimal
        Get
            Return _restDayPay
        End Get
    End Property

    Private _actualRestDayPay As Decimal

    Public ReadOnly Property ActualRestDayPay As Decimal
        Get
            Return _actualRestDayPay
        End Get
    End Property

    Private _restDayOTHours As Decimal

    Public ReadOnly Property RestDayOTHours As Decimal
        Get
            Return _restDayOTHours
        End Get
    End Property

    Private _restDayOTPay As Decimal

    Public ReadOnly Property RestDayOTPay As Decimal
        Get
            Return _restDayOTPay
        End Get
    End Property

    Private _actualRestDayOTPay As Decimal

    Public ReadOnly Property ActualRestDayOTPay As Decimal
        Get
            Return _actualRestDayOTPay
        End Get
    End Property

    Private _specialHolidayHours As Decimal

    Public ReadOnly Property SpecialHolidayHours As Decimal
        Get
            Return _specialHolidayHours
        End Get
    End Property

    Private _specialHolidayPay As Decimal

    Public ReadOnly Property SpecialHolidayPay As Decimal
        Get
            Return _specialHolidayPay
        End Get
    End Property

    Private _actualSpecialHolidayPay As Decimal

    Public ReadOnly Property ActualSpecialHolidayPay As Decimal
        Get
            Return _actualSpecialHolidayPay
        End Get
    End Property

    Private _specialHolidayOTHours As Decimal

    Public ReadOnly Property SpecialHolidayOTHours As Decimal
        Get
            Return _specialHolidayOTHours
        End Get
    End Property

    Private _specialHolidayOTPay As Decimal

    Public ReadOnly Property SpecialHolidayOTPay As Decimal
        Get
            Return _specialHolidayOTPay
        End Get
    End Property

    Private _actualSpecialHolidayOTPay As Decimal

    Public ReadOnly Property ActualSpecialHolidayOTPay As Decimal
        Get
            Return _actualSpecialHolidayOTPay
        End Get
    End Property

    Private _regularHolidayHours As Decimal

    Public ReadOnly Property RegularHolidayHours As Decimal
        Get
            Return _regularHolidayHours
        End Get
    End Property

    Private _regularHolidayPay As Decimal

    Public ReadOnly Property RegularHolidayPay As Decimal
        Get
            Return _regularHolidayPay
        End Get
    End Property

    Private _actualRegularHolidayPay As Decimal

    Public ReadOnly Property ActualRegularHolidayPay As Decimal
        Get
            Return _actualRegularHolidayPay
        End Get
    End Property

    Private _regularHolidayOTHours As Decimal

    Public ReadOnly Property RegularHolidayOTHours As Decimal
        Get
            Return _regularHolidayOTHours
        End Get
    End Property

    Private _regularHolidayOTPay As Decimal

    Public ReadOnly Property RegularHolidayOTPay As Decimal
        Get
            Return _regularHolidayOTPay
        End Get
    End Property

    Private _actualRegularHolidayOTPay As Decimal

    Public ReadOnly Property ActualRegularHolidayOTPay As Decimal
        Get
            Return _actualRegularHolidayOTPay
        End Get
    End Property

    Private _holidayPay As Decimal

    Public ReadOnly Property HolidayPay As Decimal
        Get
            Return _holidayPay
        End Get
    End Property

    Private _leaveHours As Decimal

    Public ReadOnly Property LeaveHours As Decimal
        Get
            Return _leaveHours
        End Get
    End Property

    Private _leavePay As Decimal

    Public ReadOnly Property LeavePay As Decimal
        Get
            Return _leavePay
        End Get
    End Property

    Private _actualLeavePay As Decimal

    Public ReadOnly Property ActualLeavePay As Decimal
        Get
            Return _actualLeavePay
        End Get
    End Property

    Private _lateHours As Decimal

    Public ReadOnly Property LateHours As Decimal
        Get
            Return _lateHours
        End Get
    End Property

    Private _lateDeduction As Decimal

    Public ReadOnly Property LateDeduction As Decimal
        Get
            Return _lateDeduction
        End Get
    End Property

    Private _actualLateDeduction As Decimal

    Public ReadOnly Property ActualLateDeduction As Decimal
        Get
            Return _actualLateDeduction
        End Get
    End Property

    Private _undertimeHours As Decimal

    Public ReadOnly Property UndertimeHours As Decimal
        Get
            Return _undertimeHours
        End Get
    End Property

    Private _undertimeDeduction As Decimal

    Public ReadOnly Property UndertimeDeduction As Decimal
        Get
            Return _undertimeDeduction
        End Get
    End Property

    Private _actualUndertimeDeduction As Decimal

    Public ReadOnly Property ActualUndertimeDeduction As Decimal
        Get
            Return _actualUndertimeDeduction
        End Get
    End Property

    Private _absentHours As Decimal

    Public ReadOnly Property AbsentHours As Decimal
        Get
            Return _absentHours
        End Get
    End Property

    Private _absenceDeduction As Decimal

    Public ReadOnly Property AbsenceDeduction As Decimal
        Get
            Return _absenceDeduction
        End Get
    End Property

    Private _actualAbsenceDeduction As Decimal

    Public ReadOnly Property ActualAbsenceDeduction As Decimal
        Get
            Return _actualAbsenceDeduction
        End Get
    End Property

End Class