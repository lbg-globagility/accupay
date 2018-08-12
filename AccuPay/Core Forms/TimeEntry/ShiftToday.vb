Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Tools

Public Class CurrentShift

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

    Public ReadOnly Property HasBreaktime As Boolean
        Get
            Return BreakPeriod IsNot Nothing
        End Get
    End Property

    Public Sub New(shift As Shift, [date] As DateTime)
        Me.Shift = shift
        Me.Date = [date]

        Dim nextDay = [date].AddDays(1)

        Dim shiftStart = Calendar.Create([date], shift.TimeFrom)
        Dim shiftEnd = Calendar.Create(If(shift.TimeTo > shift.TimeFrom, [date], nextDay), shift.TimeTo)

        Me.ShiftPeriod = New TimePeriod(shiftStart, shiftEnd)

        If shift.HasBreaktime Then
            Dim breaktimeStart = Calendar.Create(
                If(shift.BreaktimeFrom > shift.TimeFrom, [date], nextDay),
                shift.BreaktimeFrom.Value)

            Dim breaktimeEnd = Calendar.Create(
                If(shift.BreaktimeTo > shift.TimeFrom, [date], nextDay),
                shift.BreaktimeTo.Value)

            Me.BreakPeriod = New TimePeriod(breaktimeStart, breaktimeEnd)
        End If
    End Sub

End Class
