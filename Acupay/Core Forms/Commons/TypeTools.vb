Option Strict On

Public Class TypeTools

    Public Shared Function ParseDecimal(text As String) As Decimal
        If String.IsNullOrWhiteSpace(text) Then
            Return 0
        End If

        Return Decimal.Parse(text)
    End Function

    Public Shared Function ParseDecimalOrNull(text As String) As Decimal?
        If String.IsNullOrWhiteSpace(text) Then
            Return Nothing
        End If

        Return Decimal.Parse(text)
    End Function

End Class
