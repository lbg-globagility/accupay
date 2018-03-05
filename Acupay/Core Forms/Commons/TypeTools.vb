Option Strict On

Public Class TypeTools

    Public Shared Function ParseDecimal(text As String) As Decimal
        If String.IsNullOrWhiteSpace(text) Then
            Return 0D
        End If

        Return Decimal.Parse(text)
    End Function

End Class
