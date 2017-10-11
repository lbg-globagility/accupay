Public Class Shift

    Public Property RowID As Integer?

    Public Property ShiftFrom As TimeSpan?

    Public Property ShiftTo As TimeSpan?

    Public Property BreaktimeFrom As TimeSpan?

    Public Property BreaktimeTo As TimeSpan?

    Public Property DivisorToDailyRate As Decimal

    Public ReadOnly Property HasBreaktime As Boolean
        Get
            Return BreaktimeFrom.HasValue And BreaktimeTo.HasValue
        End Get
    End Property

End Class
