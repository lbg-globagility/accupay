Public Class TimeUtility

    Public Shared Function RangeStart(dateToday As Date, startTime As TimeSpan) As Date
        Return Create(dateToday, startTime)
    End Function

    Public Shared Function RangeEnd(dateToday As Date, startTime As TimeSpan, endTime As TimeSpan) As Date
        Dim dateTomorrow = dateToday.AddDays(1)
        Return Create(
            If(endTime > startTime, dateToday, dateTomorrow),
            endTime
        )
    End Function

    Public Shared Function Create(day As Date, time As TimeSpan) As Date
        Dim timestampString = $"{day.ToString("yyyy-MM-dd")} {time.ToString("hh\:mm\:ss")}"

        Return Date.Parse(timestampString)
    End Function

End Class
