Option Strict On

Imports AccuPay.Entity

Public Class CurrentShift

    Public ReadOnly Property Start As Date

    Public ReadOnly Property [End] As Date

    Public ReadOnly Property BreaktimeStart As Date?

    Public ReadOnly Property BreaktimeEnd As Date?

    Public ReadOnly Property ShiftDate As Date

    Public ReadOnly Property Shift As Shift

    Public ReadOnly Property HasBreaktime As Boolean
        Get
            Return Shift.HasBreaktime
        End Get
    End Property

    Public Sub New(shift As Shift, shiftDate As Date)
        Me.Shift = shift
        Me.ShiftDate = shiftDate

        Start = TimeUtility.RangeStart(shiftDate, shift.TimeFrom)
        [End] = TimeUtility.RangeEnd(shiftDate, shift.TimeFrom, shift.TimeTo)

        If shift.HasBreaktime Then
            BreaktimeStart = TimeUtility.RangeStart(shiftDate, shift.BreaktimeFrom.Value)
            BreaktimeEnd = TimeUtility.RangeEnd(shiftDate, shift.BreaktimeFrom.Value, shift.BreaktimeTo.Value)
        End If
    End Sub

End Class
