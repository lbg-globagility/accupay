Option Strict On

Imports System.IO
Imports System.Globalization
Imports System.Collections.ObjectModel
Imports System.Text.RegularExpressions

Module TimeInTimeOutParserModule

    Private dateDashFormat As String = "MM-dd-yyyy"
    Private dateSlashFormat As String = "MM/dd/yyyy"

    Class ParseTimeLogException
        Inherits Exception

        Private Shared Function FormatMessage(line As String) As String
            Return $"Cannot parse the following line: {line}"
        End Function

        Public Sub New(line As String)
            MyBase.New(FormatMessage(line))
        End Sub

        Public Sub New(line As String, ex As Exception)
            MyBase.New(FormatMessage(line), ex)
        End Sub
    End Class

    Class FixedFormatTimeEntry
        Public Property EmployeeID As Integer
        Public Property DateOccurred As Date
        Public Property TimeIn As String
        Public Property TimeOut As String

        Public Sub New(employeeID As Integer, dateOccurred As Date, timeIn As String, timeOut As String)
            Me.EmployeeID = employeeID
            Me.DateOccurred = dateOccurred
            Me.TimeIn = timeIn
            Me.TimeOut = timeOut
        End Sub
    End Class

    Class ConventionalTimeLogs

        Private e_uniq_key As String = String.Empty

        Private date_and_time As Object = Nothing

        Sub New(employee_uniq_key As String,
                date_andtime As Object)
            Me.e_uniq_key = employee_uniq_key
            Me.date_and_time = date_andtime

        End Sub

        Property EmployeUniqueKey As String
            Get
                Return e_uniq_key

            End Get

            Set(value As String)
                e_uniq_key = value

            End Set

        End Property

        Property DateAndTime As Object
            Get
                Return date_and_time

            End Get

            Set(value As Object)
                date_and_time = value

            End Set

        End Property

    End Class

    Class TimeInTimeOutParser
        Public Function Parse(filename As String) As Collection(Of FixedFormatTimeEntry)
            Dim timeEntries = New Collection(Of FixedFormatTimeEntry)

            Using reader As New StreamReader(filename)
                Dim currentLine As String

                Do
                    currentLine = reader.ReadLine()
                    ParseLine(currentLine, timeEntries)
                Loop Until currentLine Is Nothing
            End Using

            Return timeEntries
        End Function

        Public Function ParseConventionalTimeLogs(filename As String) As Collection(Of ConventionalTimeLogs)
            Dim timeEntries = New Collection(Of ConventionalTimeLogs)

            Using reader As New StreamReader(filename)
                Dim currentLine As String = String.Empty

                While reader.Peek() >= 0

                    currentLine = reader.ReadLine()

                    If currentLine.Length > 0 Then

                        Dim values =
                            Split(currentLine,
                                  vbTab)

                        timeEntries.
                            Add(New ConventionalTimeLogs(values(0),
                                                         values(1)))

                    End If

                End While

            End Using

            Return timeEntries

        End Function

        Private Sub ParseLine(line As String, timeEntries As Collection(Of FixedFormatTimeEntry))
            Try
                If String.IsNullOrEmpty(line) Then
                    Return
                End If

                Dim parts = Regex.Split(Trim(line), "\s+")

                If parts.Length < 3 Then
                    Return
                End If

                Dim employeeID = CInt(Trim(parts(0)))

                Dim logDate = parts(1)

                Dim dateFormat = String.Empty
                If logDate.Contains("-") Then
                    dateFormat = dateDashFormat
                ElseIf logDate.Contains("/") Then
                    dateFormat = dateSlashFormat
                End If

                Dim dateOccurred = Date.ParseExact(logDate, dateFormat, CultureInfo.InvariantCulture)

                Dim timeIn = Trim(parts(2))
                Dim timeOut = If(parts.Length > 3, Trim(parts(3)), Nothing)

                If Not String.IsNullOrEmpty(timeIn) Or Not String.IsNullOrEmpty(timeOut) Then
                    timeEntries.Add(
                            New FixedFormatTimeEntry(employeeID, dateOccurred, timeIn, timeOut)
                        )
                End If
            Catch ex As Exception
                Throw New ParseTimeLogException(line, ex)
            End Try
        End Sub
    End Class

End Module
