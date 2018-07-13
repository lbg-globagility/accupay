Option Strict On

Namespace Global.AccuPay

    Public Class TimePeriod

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

        Public Function Intersects(period As TimePeriod) As Boolean
            Return Me.Start <= period.End And
                Me.End >= period.Start
        End Function

        Public Function Overlap(period As TimePeriod) As TimePeriod
            Return New TimePeriod(
                {Me.Start, period.Start}.Max(),
                {Me.End, period.End}.Min())
        End Function

        Public Function Difference(period As TimePeriod) As IList(Of TimePeriod)
            If Not Intersects(period) Then
                Return New List(Of TimePeriod) From {Me}
            End If

            Dim periods = New List(Of TimePeriod)
            If Start < period.Start Then
                periods.Add(New TimePeriod(Start, period.Start))
            End If

            If [End] > period.End Then
                periods.Add(New TimePeriod(period.End, [End]))
            End If

            Return periods
        End Function

        Public Overrides Function ToString() As String
            Return $"{Start.ToString()} to {[End].ToString()}"
        End Function

    End Class

End Namespace