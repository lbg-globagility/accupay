Imports System.Runtime.CompilerServices

Namespace Global.AccuPay.Extensions

    Module TimeSpanExtensions

        <Extension()>
        Public Function ToStringFormat(timeSpanInput As TimeSpan, format As String) As String

            Dim currentDate = Date.Now.ToMinimumHourValue
            Return currentDate.Add(timeSpanInput).ToString(format)

        End Function

    End Module

End Namespace