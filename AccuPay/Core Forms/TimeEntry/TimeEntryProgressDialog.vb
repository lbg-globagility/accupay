Option Strict On

Public Class TimeEntryProgressDialog

    Public Sub Progress()
        CompletionProgressBar.Value = 100
    End Sub

    Public Sub Reset()
        CompletionProgressBar.Value = 0
    End Sub

End Class
