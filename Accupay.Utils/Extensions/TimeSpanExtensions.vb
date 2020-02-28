Imports System.Runtime.CompilerServices

Module TimeSpanExtensions
    Private Const MINUTES_PER_HOUR As Decimal = 60

    <Extension()>
    Public Function ToStringFormat(
                    timeSpanInput As TimeSpan,
                    format As String,
                    Optional currentDate As Date? = Nothing) As String

        currentDate = If(currentDate Is Nothing, Date.Now.ToMinimumHourValue, currentDate.ToMinimumHourValue)

        Return currentDate.Value.Add(timeSpanInput).ToString(format)

    End Function

    <Extension()>
    Public Function ToStringFormat(
                    timeSpanInput As TimeSpan?,
                    format As String,
                    Optional currentDate As Date? = Nothing) As String

        currentDate = If(currentDate Is Nothing, Date.Now.ToMinimumHourValue, currentDate.ToMinimumHourValue)

        If Not timeSpanInput.HasValue Then Return ""
        Return currentDate.Value.Add(timeSpanInput).ToString(format)

    End Function

    <Extension()>
    Public Function AddHours(timeSpanInput As TimeSpan, value As Decimal) As TimeSpan
        Return timeSpanInput.Add(New TimeSpan(0, value * MINUTES_PER_HOUR, 0))
    End Function

    <Extension()>
    Public Function StripSeconds(timeSpanInput As TimeSpan) As TimeSpan

        Return New TimeSpan(timeSpanInput.Hours, timeSpanInput.Minutes, 0)

    End Function

End Module