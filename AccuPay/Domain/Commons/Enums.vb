Option Strict On

Public Class Enums(Of TEnum As Structure)

    Private Sub New()
    End Sub

    Public Shared Function Parse(name As String) As TEnum
        Dim value = [Enum].Parse(GetType(TEnum), name)
        Return CType(value, TEnum)
    End Function

End Class
