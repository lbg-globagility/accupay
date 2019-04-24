Public Class ContributionSchedule

    Public Const FIRST_HALF As String = "First half"
    Public Const END_OF_THE_MONTH As String = "End of the month"
    Public Const PER_PAY_PERIOD As String = "Per pay period"


    Public Const LAST_WEEK_OF_THE_MONTH As String = "Last week of the month"

    Public Shared Function GetList() As List(Of String)
        Return New List(Of String) From {
                        FIRST_HALF,
                        END_OF_THE_MONTH,
                        PER_PAY_PERIOD
                }
    End Function

End Class
