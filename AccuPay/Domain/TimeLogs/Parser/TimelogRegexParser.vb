Option Strict On

Imports System.Text.RegularExpressions
Imports AccuPay.Utils

Namespace Global.AccuPay.Domain.TimeLogs

    Public Class RegexTimelogParser

        Private ReadOnly _expression As String

        Private ReadOnly _inValue As String

        Private ReadOnly _outValue As String

        Public Sub New(expression As String, Optional inValue As String = Nothing, Optional outValue As String = Nothing)
            _expression = expression
            _inValue = inValue
            _outValue = outValue
        End Sub

        Public Function Parse(line As String) As Result
            Dim regex = New Regex(_expression)

            Dim result = New Result()

            Dim match = regex.Match(line)

            result.EmployeeNo = ReadCaptureValue(match, "employeeno")

            Dim year = ObjectUtils.ToInteger(ReadCaptureValue(match, "year"))
            Dim month = ObjectUtils.ToInteger(ReadCaptureValue(match, "month"))
            Dim day = ObjectUtils.ToInteger(ReadCaptureValue(match, "day"))

            Dim [date] = New Date(year, month, day)
            result.Date = [date]

            Dim hour = ObjectUtils.ToInteger(ReadCaptureValue(match, "hour"))
            Dim minute = ObjectUtils.ToInteger(ReadCaptureValue(match, "minute"))

            Dim time = New TimeSpan(hour, minute, 0)
            result.Time = time

            result.Status = ReadEntryStatus(ReadCaptureValue(match, "entry"))

            Return result
        End Function

        Private Function ReadCaptureValue(match As Match, name As String) As String
            Dim value = match.Groups(name).Value

            Return If(String.IsNullOrEmpty(value), Nothing, value)
        End Function

        Public Function ReadEntryStatus(value As String) As EntryStatus
            If value = _inValue Then
                Return EntryStatus.In
            ElseIf value = _outValue Then
                Return EntryStatus.Out
            Else
                Return EntryStatus.Undefined
            End If
        End Function

    End Class

    Public Class Result

        Public Property EmployeeNo As String

        Public Property [Date] As Date

        Public Property Time As TimeSpan

        Public Property Status As EntryStatus

    End Class

    Public Enum EntryStatus
        Undefined
        [In]
        Out
    End Enum

End Namespace
