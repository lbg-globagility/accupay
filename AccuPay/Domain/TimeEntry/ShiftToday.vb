Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports AccuPay.Tools

Public Class CurrentShift

    Public Const StandardWorkingHours As Decimal = 8

    Private _defaultRestDay As Integer?

    Private _shiftSchedule2 As EmployeeDutySchedule

    Public ReadOnly Property Start As Date
        Get
            Return ShiftPeriod.Start
        End Get
    End Property

    Public ReadOnly Property [End] As Date
        Get
            Return ShiftPeriod.End
        End Get
    End Property

    Public ReadOnly Property BreaktimeStart As Date?
        Get
            Return BreakPeriod.Start
        End Get
    End Property

    Public ReadOnly Property BreaktimeEnd As Date?
        Get
            Return BreakPeriod.End
        End Get
    End Property

    Public ReadOnly Property [Date] As Date

    Public ReadOnly Property Shift As Shift

    Public ReadOnly Property ShiftPeriod As TimePeriod

    Public ReadOnly Property BreakPeriod As TimePeriod

    Public ReadOnly Property ShiftSchedule As ShiftSchedule

    Public ReadOnly Property WorkingHours As Decimal
        Get
            Return If(_shiftSchedule2?.WorkHours, If(ShiftSchedule?.Shift?.WorkHours, StandardWorkingHours))
        End Get
    End Property

    Public ReadOnly Property HasShift As Boolean
        Get
            Return Shift IsNot Nothing Or _shiftSchedule2 IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property HasBreaktime As Boolean
        Get
            Return BreakPeriod IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property IsRestDay As Boolean
        Get
            Dim isRestDayOffset = False

            If _shiftSchedule2 IsNot Nothing Then

                isRestDayOffset = _shiftSchedule2.IsRestDay

            ElseIf ShiftSchedule IsNot Nothing Then

                isRestDayOffset = ShiftSchedule.IsRestDay

            End If

            Dim isDefaultRestDay = False
            If _defaultRestDay.HasValue Then
                isDefaultRestDay = (_defaultRestDay.Value - 1) = Me.Date.DayOfWeek
            End If

            Return (isRestDayOffset And (Not isDefaultRestDay)) Or ((Not isRestDayOffset) And isDefaultRestDay)
        End Get
    End Property

    Public ReadOnly Property IsWorkingDay As Boolean
        Get
            Return Not IsRestDay
        End Get
    End Property

    Public ReadOnly Property IsNightShift As Boolean
        Get
            Return True
        End Get
    End Property

    Public Sub New(shift As Shift, [date] As DateTime)
        Me.Shift = shift
        Me.Date = [date]

        If shift Is Nothing Then
            Return
        End If

        Me.ShiftPeriod =
            TimePeriod.FromTime(New TimeSpan(shift.TimeFrom.Hours, shift.TimeFrom.Minutes, 0),
                                New TimeSpan(shift.TimeTo.Hours, shift.TimeTo.Minutes, 0),
                                [date])

        If shift.HasBreaktime Then
            Dim nextDay = [date].AddDays(1)
            Dim breakDate = If(shift.BreaktimeFrom > shift.TimeFrom, [date], nextDay)

            Me.BreakPeriod = TimePeriod.FromTime(shift.BreaktimeFrom.Value, shift.BreaktimeTo.Value, breakDate)
        End If
    End Sub

    Public Sub New(shiftSchedule As ShiftSchedule, [date] As DateTime)
        Me.New(shiftSchedule?.Shift, [date])
        Me.ShiftSchedule = shiftSchedule
    End Sub

    Public Sub SetDefaultRestDay(dayOfWeek As Integer?)
        _defaultRestDay = dayOfWeek
    End Sub

    Public Sub New(shiftSchedule As EmployeeDutySchedule, [date] As DateTime)

        Me.Date = [date]

        If shiftSchedule Is Nothing OrElse
            shiftSchedule.StartTime Is Nothing OrElse
            shiftSchedule.EndTime Is Nothing Then
            Return
        End If

        _shiftSchedule2 = shiftSchedule

        Me.ShiftPeriod =
            TimePeriod.FromTime(New TimeSpan(shiftSchedule.StartTime.Value.Hours, shiftSchedule.StartTime.Value.Minutes, 0),
                                New TimeSpan(shiftSchedule.EndTime.Value.Hours, shiftSchedule.EndTime.Value.Minutes, 0),
                               Me.Date)

        If shiftSchedule.BreakStartTime.HasValue Then
            Dim nextDay = Me.Date.AddDays(1)
            Dim breakDate = If(shiftSchedule.BreakStartTime > shiftSchedule.StartTime, Me.Date, nextDay)

            Dim breakTimeEnd = shiftSchedule.BreakStartTime.Value.AddHours(shiftSchedule.BreakLength)
            Me.BreakPeriod = TimePeriod.FromTime(shiftSchedule.BreakStartTime.Value, breakTimeEnd, breakDate)
        End If
    End Sub

End Class
