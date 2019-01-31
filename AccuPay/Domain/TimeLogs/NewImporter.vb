Option Strict On

Imports System.Globalization
Imports System.IO
Imports System.Text.RegularExpressions
Imports AccuPay.Utils

Public Class TimeLogsReader

    'currently not used after using ObjectUtils conversion
    Private Const DateTimePattern As String = "^(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2}) (\d{1,2}):(\d{1,2}):?(\d{1,2})?$"

    Public Function Import(filename As String) As (IList(Of TimeAttendanceLog), IList(Of ErrorLog))
        Dim logs = New List(Of TimeAttendanceLog)
        Dim errors = New List(Of ErrorLog)

        Dim lineNumber As Integer = 0
        Dim lineContent As String
        Using fileStream = New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite),
            stream = New StreamReader(fileStream)
            Do
                lineNumber += 1
                lineContent = stream.ReadLine()

                Dim parseOutput = ParseLine(lineContent)

                Dim log = parseOutput.Item1
                Dim errorMessage = parseOutput.Item2

                If log IsNot Nothing Then
                    logs.Add(log)
                Else
                    If String.IsNullOrWhiteSpace(errorMessage) = False Then
                        errors.Add(New ErrorLog With {
                            .LineNumber = lineNumber,
                            .Content = lineContent,
                            .Reason = errorMessage
                        })
                    End If
                End If

            Loop Until lineContent Is Nothing
        End Using

        Return (logs, errors)
    End Function

    Private Function ParseLine(line As String) As (TimeAttendanceLog, String)
        Try
            If String.IsNullOrEmpty(line) Then
                'not considered error if it's just an empty line
                'but it won't still be added to TimeAttendanceLog
                Return (Nothing, "")
            End If

            Dim parts = Regex.Split(line, "\t")

            If parts.Length < 2 Then
                Return (Nothing, "Needs at least 2 items in one line separated by a tab.")
            End If

            Dim employeeNo = Trim(parts(0))

            Dim logDate = ObjectUtils.ToNullableDateTime(parts(1))

            If logDate Is Nothing Then
                Return (Nothing, "Second column must be a valid Date Time.")
            End If

            Return (
            New TimeAttendanceLog() With {
                .EmployeeNo = employeeNo,
                .DateTime = CType(logDate, Date)
            },
            "")

        Catch ex As Exception
            Return (Nothing, "Error reading the line. Please check the template.")
            'Throw New ParseTimeLogException(line, ex)
        End Try
    End Function

    'currently not used after using ObjectUtils conversion
    Private Function ParseDateTime(value As String) As DateTime
        Dim pattern = New Regex(DateTimePattern)
        Dim match = pattern.Match(value)

        If Not match.Success Then
            Return Nothing
        End If

        Dim year = Integer.Parse(match.Groups(1).Value)
        Dim month = Integer.Parse(match.Groups(2).Value)
        Dim day = Integer.Parse(match.Groups(3).Value)
        Dim hours = Integer.Parse(match.Groups(4).Value)
        Dim minutes = Integer.Parse(match.Groups(5).Value)
        Dim seconds = Integer.Parse(match.Groups(6).Value)

        Return New DateTime(year, month, day, hours, minutes, seconds)
    End Function

    Public Class ErrorLog
        Public Property LineNumber As Integer
        Public Property Content As String
        Public Property Reason As String
    End Class

End Class

    Public Class TimeAttendanceLog

    Public Property EmployeeNo As String

    Public Property [DateTime] As DateTime

    Public Property FirstColumn As Integer

    Public Property SecondColumn As Integer

    Public Property ThirdColumn As Integer

    Public Property FourthColumn As Integer

End Class