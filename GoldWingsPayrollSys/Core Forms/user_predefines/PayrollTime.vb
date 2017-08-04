Public Class PayrollTime

    Public Shared Function RangeStart(startTime As TimeSpan, dateToday As Date) As Date
        Return Create(dateToday, startTime)
    End Function

    Public Shared Function RangeEnd(startTime As TimeSpan, endTime As TimeSpan, dateToday As Date) As Date
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
