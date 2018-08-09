Option Strict On

Imports System.Globalization
Imports System.IO
Imports System.Text.RegularExpressions

Public Class TimeLogsReader

    Private Const DateTimePattern As String = "^(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2}) (\d{1,2}):(\d{1,2}):?(\d{1,2})?$"

    Public Function Import(filename As String) As IList(Of TimeAttendanceLog)
        Dim logs = New List(Of TimeAttendanceLog)

        Using fileStream = New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite),
            stream = New StreamReader(fileStream)

            Dim line = stream.ReadLine()

            Dim log = ParseLine(line)

            If log IsNot Nothing Then
                logs.Add(log)
            End If
        End Using

        Return logs
    End Function

    Private Function ParseLine(line As String) As TimeAttendanceLog
        Try
            If String.IsNullOrEmpty(line) Then
                Return Nothing
            End If

            Dim parts = Regex.Split(line, "\t")

            If parts.Length < 3 Then
                Return Nothing
            End If

            Dim employeeNo = Trim(parts(0))

            Dim logDate = ParseDateTime(parts(1))

            Return New TimeAttendanceLog() With {
                .EmployeeNo = employeeNo,
                .DateTime = logDate
            }
        Catch ex As Exception
            Throw New ParseTimeLogException(line, ex)
        End Try
    End Function

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




End Class

Public Class TimeAttendanceLog

    Public EmployeeNo As String

    Public [DateTime] As DateTime

    Public FirstColumn As Integer

    Public SecondColumn As Integer

    Public ThirdColumn As Integer

    Public FourthColumn As Integer

End Class