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

        RangeStart = PayrollTime.RangeStart(shift.ShiftFrom, shiftDate)
        RangeEnd = PayrollTime.RangeEnd(shift.ShiftFrom, shift.ShiftTo, shiftDate)

        BreaktimeStart = PayrollTime.RangeStart(shift.BreaktimeFrom, shiftDate)
        BreaktimeEnd = PayrollTime.RangeEnd(shift.BreaktimeFrom, shift.BreaktimeTo, shiftDate)
    End Sub

End Class
