Imports System.Globalization
Imports System.Runtime.CompilerServices

Module StringExtensions

    <Extension()>
    Public Function ToNullableTimeSpan(
                        timespanString As String,
                        Optional timeFormat As String = "H:mm") As TimeSpan?

        If timespanString Is Nothing Then Return Nothing

        Dim dt As Date

        If timespanString.Trim = "24:00" Then
            '"24:00" is considered invalid in parsing
            timespanString = "00:00"

        End If

        If Date.TryParseExact(timespanString, timeFormat,
                                  CultureInfo.InvariantCulture,
                                  DateTimeStyles.None, dt) Then

            Return dt.TimeOfDay
        Else
            Return Nothing
        End If

    End Function

End Module