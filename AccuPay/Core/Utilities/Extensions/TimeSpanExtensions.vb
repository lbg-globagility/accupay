Option Strict On

Imports System.Runtime.CompilerServices

Namespace Global.AccuPay.Extensions

    Module TimeSpanExtensions
        Private Const MINUTES_PER_HOUR As Decimal = 60

        <Extension()>
        Public Function AddHours(input As TimeSpan, hours As Decimal) As TimeSpan

            Return input.Add(New TimeSpan(0, CInt(hours * MINUTES_PER_HOUR), 0))
        End Function

    End Module

End Namespace