Option Strict On

Namespace Global.AccuPay

    Public Class TimeInterval

        Public ReadOnly Start As Date

        Public ReadOnly [End] As Date

        Public ReadOnly Property Length As TimeSpan
            Get
                Return [End] - Start
            End Get
        End Property

        Public Sub New(start As Date, [end] As Date)
            Me.Start = start
            Me.End = [end]
        End Sub

        Public Function Contains(moment As Date) As Boolean
            Return (Start <= moment) And (moment <= [End])
        End Function

        Public Function Overlaps(timeInterval As TimeInterval) As Boolean
            Return Me.Start <= timeInterval.End And
                Me.End >= timeInterval.Start
        End Function

        Public Function Intersect(timeInterval As TimeInterval) As TimeInterval
            Return New TimeInterval(
                {Me.Start, timeInterval.Start}.Max(),
                {Me.End, timeInterval.End}.Min())
        End Function

    End Class

End Namespace