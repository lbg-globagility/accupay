Public Class TimeUtility

    Public Shared Function RangeStart(dateToday As Date, startTime As TimeSpan) As Date
        Return Combine(dateToday, startTime)
    End Function

    Public Shared Function RangeEnd(dateToday As Date, startTime As TimeSpan, endTime As TimeSpan) As Date
        Dim dateTomorrow = dateToday.AddDays(1)
        Return Combine(
            If(endTime > startTime, dateToday, dateTomorrow),
            endTime
        )
    End Function

    Public Shared Function Combine(day As Date, time As TimeSpan) As Date
        Dim timestampString = $"{day.ToString("yyyy-MM-dd")} {time.ToString("hh\:mm\:ss")}"

        Return Date.Parse(timestampString)
    End Function

    Public Shared Function ToDateTime(timeSpan As TimeSpan?) As DateTime?
        If Not timeSpan.HasValue Then
            Return Nothing
        End If

        Dim _today = Date.Now
        Dim _value = timeSpan.GetValueOrDefault

        Dim _dateTime =
            New Date(_today.Year, _today.Month, _today.Day,
                     _value.Hours, _value.Minutes, 0)

        Return _dateTime
    End Function

End Class
