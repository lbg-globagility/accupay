Public Class ShiftToday

    Public ReadOnly Property RangeStart As Date

    Public ReadOnly Property RangeEnd As Date

    Public ReadOnly Property BreaktimeStart As Date

    Public ReadOnly Property BreaktimeEnd As Date

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

        RangeStart = TimeUtility.RangeStart(shiftDate, shift.ShiftFrom)
        RangeEnd = TimeUtility.RangeEnd(shiftDate, shift.ShiftFrom, shift.ShiftTo)

        BreaktimeStart = TimeUtility.RangeStart(shiftDate, shift.BreaktimeFrom)
        BreaktimeEnd = TimeUtility.RangeEnd(shiftDate, shift.BreaktimeFrom, shift.BreaktimeTo)
    End Sub

End Class
