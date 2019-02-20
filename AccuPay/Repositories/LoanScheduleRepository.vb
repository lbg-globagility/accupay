Option Strict On

Namespace Global.AccuPay.Repository

    Public Class LoanScheduleRepository

        Public Function GetStatusList() As List(Of String)
            Return New List(Of String) From {
                    "In Progress",
                    "On hold",
                    "Cancelled",
                    "Complete"
            }
        End Function

    End Class

End Namespace

