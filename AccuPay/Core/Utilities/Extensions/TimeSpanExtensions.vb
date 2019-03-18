Imports System.Runtime.CompilerServices

Namespace Global.AccuPay.Extensions

    Module TimeSpanExtensions
        Private Const MINUTES_PER_HOUR As Decimal = 60

        <Extension()>
        Public Function ToStringFormat(timeSpanInput As TimeSpan, format As String) As String

            Dim currentDate = Date.Now.ToMinimumHourValue
            Return currentDate.Add(timeSpanInput).ToString(format)

        End Function

        <Extension()>
        Public Function AddHours(timeSpanInput As TimeSpan, value As Decimal) As TimeSpan
            Return timeSpanInput.Add(New TimeSpan(0, value * MINUTES_PER_HOUR, 0))
        End Function

    End Module

End Namespace