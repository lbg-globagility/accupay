Option Strict On

Imports System.IO
Imports System.Text.RegularExpressions
Imports AccuPay.Entity
Imports AccuPay.Utilities
Imports AccuPay.Utilities.Extensions

Namespace Global.AccuPay.Helper.TimeLogsReader

    Public Class TimeLogsReader

        Public Const FILE_NOT_FOUND_ERROR As String =
            "Import file not found. It may have been deleted or moved."

        'currently not used after using ObjectUtils conversion
        Private Const DateTimePattern As String = "^(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2}) (\d{1,2}):(\d{1,2}):?(\d{1,2})?$"

        Public Function Import(filename As String) As ImportOutput
            Dim output As New ImportOutput

            Dim logs = New List(Of ImportTimeAttendanceLog)

            Dim lineNumber As Integer = 0
            Dim lineContent As String

            Try
                Using fileStream = New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), stream = New StreamReader(fileStream)
                    Do
                        lineNumber += 1
                        lineContent = stream.ReadLine()

                        Dim log = ParseLine(lineContent, lineNumber)

                        If log Is Nothing Then
                            Continue Do
                        Else
                            logs.Add(log)
                        End If

                    Loop Until lineContent Is Nothing
                End Using
            Catch ex As FileNotFoundException
                output.IsImportSuccess = False
                output.ErrorMessage = FILE_NOT_FOUND_ERROR
            Catch ex As Exception
                output.IsImportSuccess = False
                output.ErrorMessage = "An error occured. Please try again or contact Globagility."
            End Try

            output.Logs = logs.Where(Function(l) l.HasError = False).ToList
            output.Errors = logs.Where(Function(l) l.HasError = True).ToList

            Return output
        End Function

        Private Function ParseLine(lineContent As String, lineNumber As Integer) As ImportTimeAttendanceLog
            Try
                If String.IsNullOrEmpty(lineContent) Then
                    'not considered error if it's just an empty line
                    'but it won't still be added to TimeAttendanceLog
                    Return Nothing
                End If

                Dim parts = Regex.Split(lineContent, "\t")

                If parts.Length < 2 Then
                    Return (New ImportTimeAttendanceLog() With {
                        .LineContent = lineContent,
                        .LineNumber = lineNumber,
                        .ErrorMessage = "Needs at least 2 items in one line separated by a tab."
                    })
                End If

                If String.IsNullOrEmpty(parts(0)) AndAlso String.IsNullOrEmpty(parts(1)) Then
                    'happens when blank lines was still read
                    'usually because it received an input like tab
                    Return Nothing
                End If

                Dim employeeNo = Trim(parts(0))

                Dim logDate = ObjectUtils.ToNullableDateTime(parts(1))

                If logDate Is Nothing Then
                    Return (New ImportTimeAttendanceLog() With {
                        .LineContent = lineContent,
                        .LineNumber = lineNumber,
                        .ErrorMessage = "Second column must be a valid Date Time."
                    })
                End If

                Return (
            New ImportTimeAttendanceLog() With {
                .EmployeeNumber = employeeNo,
                .DateTime = CType(logDate, Date),
                .LineContent = lineContent,
                .LineNumber = lineNumber
            })
            Catch ex As Exception
                Return (New ImportTimeAttendanceLog() With {
                        .LineContent = lineContent,
                        .LineNumber = lineNumber,
                        .ErrorMessage = "Error reading the line. Please check the template."
                })
            End Try
        End Function

        Public Class ImportOutput
            Public Property Logs As IList(Of ImportTimeAttendanceLog)
            Public Property Errors As IList(Of ImportTimeAttendanceLog)

            ''' <summary>
            ''' True if the file was read successfully. Even if there are errors parsing
            ''' some lines, as long as the file was read, this is still True.
            ''' Posible reason for this to become False is when it did not find the chosen
            ''' file.
            ''' </summary>
            Public Property IsImportSuccess As Boolean

            Public Property ErrorMessage As String

            Sub New()
                Me.Logs = New List(Of ImportTimeAttendanceLog)
                Me.Errors = New List(Of ImportTimeAttendanceLog)
                Me.IsImportSuccess = True

                Me.ErrorMessage = Nothing
            End Sub

        End Class

    End Class

    Public Class ImportTimeAttendanceLog

        ' - Contents and error message
        Public Property LineContent As String

        Public Property LineNumber As Integer
        Public Property ErrorMessage As String

        ' - Extracted contents
        Public Property EmployeeNumber As String

        Public Property [DateTime] As Date

        ' - analyzed data
        Public Property IsTimeIn As Boolean?

        Public Property LogDate As Date?
        Public Property Employee As Employee
        Public Property ShiftSchedule As ShiftSchedule
        Public Property EmployeeDutySchedule As EmployeeDutySchedule
        Public Property ShiftTimeInBounds As Date
        Public Property ShiftTimeOutBounds As Date
        Public Property WarningMessage As String

        Public Overrides Function ToString() As String
            Return DateTime.ToString("MM/dd/yyyy hh:mm tt")
        End Function

        Sub New()
            Me.IsTimeIn = Nothing
            Me.LogDate = Nothing

            Me.EmployeeNumber = Nothing

        End Sub

        Public ReadOnly Property Type As String
            Get
                Return If(IsTimeIn, "Time-in", "Time-out")
            End Get
        End Property

        Public ReadOnly Property ShiftDescription As String
            Get
                If ShiftSchedule IsNot Nothing AndAlso
                ShiftSchedule.Shift IsNot Nothing Then

                    Return $"{ShiftSchedule.Shift.TimeFrom.ToStringFormat("hh:mm tt")} - { _
                    ShiftSchedule.Shift.TimeTo.ToStringFormat("hh:mm tt")}"

                ElseIf EmployeeDutySchedule IsNot Nothing Then

                    Return $"{EmployeeDutySchedule.StartTime.ToStringFormat("hh:mm tt")} - { _
                    EmployeeDutySchedule.EndTime.ToStringFormat("hh:mm tt")}"

                End If

                Return "-"

            End Get
        End Property

        Public ReadOnly Property EmployeeFullName As String
            Get
                If Employee Is Nothing Then Return ""

                Return Employee.FirstName & " " &
                    If(String.IsNullOrWhiteSpace(Employee.MiddleName), "", Employee.MiddleName(0) + ". ") &
                    Employee.LastName
            End Get
        End Property

        Public ReadOnly Property HasError As Boolean
            Get
                Return ErrorMessage IsNot Nothing
            End Get
        End Property

        Public ReadOnly Property HasWarning As Boolean
            Get
                Return WarningMessage IsNot Nothing
            End Get
        End Property

        Public Shared Function GroupByEmployee(logs As IList(Of ImportTimeAttendanceLog)) _
            As List(Of IGrouping(Of String, ImportTimeAttendanceLog))

            Return logs.GroupBy(Function(l) l.EmployeeNumber).ToList()
        End Function

    End Class

End Namespace