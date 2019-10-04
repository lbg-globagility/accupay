Option Strict On

Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports AccuPay.Utils

Namespace Global.AccuPay.Extensions

    Module StringExtensions

        <Extension()>
        Public Function ToNullableTimeSpan(
                            timespanString As String) As TimeSpan?

            Return ObjectUtils.ToNullableTimeSpan(timespanString)

        End Function

        <Extension()>
        Public Function ToDecimal(num As String) As Decimal

            Return ObjectUtils.ToDecimal(num)
        End Function

        <Extension()>
        Public Function ToNullableDecimal(num As String) As Decimal?

            Return ObjectUtils.ToNullableDecimal(num)
        End Function

        <Extension()>
        Public Function Ellipsis(input As String, maxCharacters As Integer) As String

            If String.IsNullOrWhiteSpace(input) Then Return input

            If input.Length > maxCharacters Then

                Dim ellipsisString = "..."

                Return input.Substring(0, maxCharacters - ellipsisString.Length) & ellipsisString

            End If

            Return input

        End Function

    End Module

End Namespace