Option Strict On

Public Class WorkSheetIsEmptyException
    Inherits Exception

    Public Sub New(Optional message As String = "WorkSheet is empty.")
        MyBase.New(message)
    End Sub

End Class