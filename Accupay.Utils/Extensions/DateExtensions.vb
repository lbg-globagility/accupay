Imports System.Runtime.CompilerServices

Module DateExtensions

    <Extension()>
    Public Function ToStringFormatOrNull(input As Date?, format As String) As String

        If input Is Nothing Then Return Nothing
        Return input.Value.ToString(format)

    End Function

    <Extension()>
    Public Function ToMinimumHourValue(input As Date) As Date

        Return New DateTime(input.Year, input.Month, input.Day, 0, 0, 0)

    End Function

    <Extension()>
    Public Function ToMaximumHourValue(input As Date) As Date

        Return New DateTime(input.Year, input.Month, input.Day, 23, 59, 59)

    End Function

    <Extension()>
    Public Function ToMinimumHourValue(input As Date?) As Date

        If input Is Nothing Then Return Nothing

        Return New DateTime(
            input.Value.Year, input.Value.Month, input.Value.Day, 0, 0, 0)

    End Function

    <Extension()>
    Public Function ToMaximumHourValue(input As Date?) As Date

        If input Is Nothing Then Return Nothing

        Return New DateTime(
            input.Value.Year, input.Value.Month, input.Value.Day, 23, 59, 59)

    End Function

End Module