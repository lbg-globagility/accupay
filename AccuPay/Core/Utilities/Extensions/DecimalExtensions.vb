Option Strict On

Imports System.Runtime.CompilerServices
Imports AccuPay.Utils

Namespace Global.AccuPay.Extensions
    Public Module DecimalExtensions

        <Extension()>
        Public Function RoundToString(input As Decimal, Optional decimalPlace As Integer = 2) As String

            Dim format = "#,##0."

            For i = 1 To decimalPlace
                format &= "0"
            Next

            Return AccuMath.CommercialRound(input, decimalPlace).ToString(format)

        End Function

        Public Function RoundToString(input As Decimal?, Optional decimalPlace As Integer = 2) As String

            If input Is Nothing Then input = 0

            Dim format = "#,##0."

            For i = 1 To decimalPlace
                format &= "0"
            Next

            Return AccuMath.CommercialRound(input, decimalPlace).ToString(format)

        End Function

    End Module

End Namespace