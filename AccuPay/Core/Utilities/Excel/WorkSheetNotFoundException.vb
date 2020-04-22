Option Strict On

Public Class WorkSheetNotFoundException
    Inherits Exception

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

End Class