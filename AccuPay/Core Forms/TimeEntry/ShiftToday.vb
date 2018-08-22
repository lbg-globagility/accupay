Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Tools

Public Class CurrentShift

    Public Const StandardWorkingHours As Decimal = 8

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
            Return If(ShiftSchedule?.Shift?.WorkHours, StandardWorkingHours)
        End Get
    End Property

    Public ReadOnly Property HasShift As Boolean
        Get
            Return Shift IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property HasBreaktime As Boolean
        Get
            Return BreakPeriod IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property IsRestDay As Boolean
        Get
            Return If(ShiftSchedule?.IsRestDay, False)
        End Get
    End Property

    Public ReadOnly Property IsWorkingDay As Boolean
        Get
            Return Not If(ShiftSchedule?.IsRestDay, True)
        End Get
    End Property

    Public ReadOnly Property IsNightShift As Boolean
        Get
            Return If(ShiftSchedule?.IsNightShift, False)
        End Get
    End Property

    Public Sub New(shift As Shift, [date] As DateTime)
        Me.Shift = shift
        Me.Date = [date]

        If shift Is Nothing Then
            Return
        End If

        Me.ShiftPeriod = TimePeriod.FromTime(shift.TimeFrom, shift.TimeTo, [date])

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

End Class
