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

    End Module

End Namespace