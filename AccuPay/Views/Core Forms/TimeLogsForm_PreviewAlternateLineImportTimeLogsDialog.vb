Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Extensions
Imports AccuPay.Helper.TimeLogsReader

Public Class TimeLogsForm_PreviewAlternateLineImportTimeLogsDialog

    Private _logs As IList(Of ImportTimeAttendanceLog)

    Private _errors As IList(Of ImportTimeAttendanceLog)

    Private _originalLogs As IList(Of ImportTimeAttendanceLog)

    Private _originalErrors As IList(Of ImportTimeAttendanceLog)

    Public Property Cancelled As Boolean

    Private _dtp As New DateTimePicker()
    Private _Rectangle As Rectangle

    Sub New(logs As IList(Of ImportTimeAttendanceLog), errors As IList(Of ImportTimeAttendanceLog))

        Me._originalLogs = logs.
                    OrderBy(Function(l) l.Employee?.LastName).
                    ThenBy(Function(l) l.Employee?.FirstName).
                    ThenBy(Function(l) l.Employee?.MiddleName).
                    ThenBy(Function(l) l.LogDate).
                    ThenBy(Function(l) l.DateTime).
                    ToList

        Me._logs = Me._originalLogs

        Me._originalErrors = errors.
                    OrderBy(Function(l) l.LineNumber).
                    ToList()

        Me._errors = Me._originalErrors

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Cancelled = True

    End Sub

    Private Sub TimeAttendanceLogDataGrid_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) _
        Handles TimeAttendanceLogDataGrid.CellClick
        If TimeAttendanceLogDataGrid.Columns(e.ColumnIndex) Is TimeAttendanceLogDataGridLogDate Then
            _Rectangle = TimeAttendanceLogDataGrid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, True)
            _dtp.Size = New Size(_Rectangle.Width, _Rectangle.Height)
            _dtp.Location = New Point(_Rectangle.X, _Rectangle.Y)

            Dim currentLog As ImportTimeAttendanceLog = GetCurrentTimeLogByGridRowIndex(e.RowIndex)

            If currentLog Is Nothing Then Return

            _dtp.Value = If(currentLog.LogDate, currentLog.DateTime.ToMinimumHourValue())

            _dtp.Visible = True
        Else
            _dtp.Visible = False
        End If
    End Sub

    Private Sub dtp_TextChange(ByVal sender As Object, ByVal e As EventArgs)
        TimeAttendanceLogDataGrid.CurrentCell.Value = _dtp.Text.ToString()
    End Sub

    Private Sub TimeAttendanceLogDataGrid_Scroll(ByVal sender As Object, ByVal e As ScrollEventArgs) _
        Handles TimeAttendanceLogDataGrid.Scroll
        _dtp.Visible = False
    End Sub

    Private Sub TimeLogsForm_PreviewAlternateLineImportTimeLogsDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        TimeAttendanceLogDataGrid.Controls.Add(_dtp)
        _dtp.Visible = False
        _dtp.Format = DateTimePickerFormat.Custom
        AddHandler _dtp.ValueChanged, AddressOf dtp_TextChange

        Dim errorCount = Me._errors.Count

        If errorCount > 0 Then

            If errorCount = 1 Then
                lblStatus.Text = "There is 1 error."
            Else
                lblStatus.Text = $"There are {errorCount} errors."

            End If

            lblStatus.Text += " Failed logs will not be saved."
            lblStatus.BackColor = Color.Red
        Else
            lblStatus.Text = "No errors found."
            lblStatus.BackColor = Color.Green
        End If

        TimeAttendanceLogDataGrid.AutoGenerateColumns = False
        TimeAttendanceLogErrorsDataGrid.AutoGenerateColumns = False

        ParsedTabControl.Text = $"Ok ({Me._logs.Count})"
        ErrorsTabControl.Text = $"Errors ({errorCount})"

        TimeAttendanceLogDataGrid.DataSource = Me._logs

        TimeAttendanceLogErrorsDataGrid.DataSource = Me._errors

    End Sub

    Private Sub FooterButton_Click(sender As Object, e As EventArgs) _
        Handles btnOK.Click, btnClose.Click

        If sender Is btnOK Then

            Me.Cancelled = False

        End If

        Me.Close()

    End Sub

    Private Sub dgvErrors_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles TimeAttendanceLogErrorsDataGrid.CellFormatting
        Dim value As String = e.Value.ToString().Replace(vbTab, "     ")
        e.Value = value
    End Sub

    Private Sub TimeAttendanceLogDataGrid_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) _
        Handles TimeAttendanceLogDataGrid.CellFormatting

        Dim currentLog As ImportTimeAttendanceLog = GetCurrentTimeLogByGridRowIndex(e.RowIndex)

        If currentLog Is Nothing Then Return

        Dim currentColumn As DataGridViewColumn = TimeAttendanceLogDataGrid.Columns(e.ColumnIndex)

        If currentColumn Is TimeAttendanceLogDataGridTimeInButton Then
            If currentLog.IsTimeIn = True Then
                e.CellStyle.BackColor = Color.Green
                e.CellStyle.SelectionBackColor = Color.Green
                e.CellStyle.SelectionForeColor = Color.Black
                e.CellStyle.Font = New Font(e.CellStyle.Font, FontStyle.Bold)
            Else
                e.CellStyle.BackColor = Color.White
                e.CellStyle.SelectionBackColor = Color.White
                e.CellStyle.SelectionForeColor = Color.Black
            End If

        ElseIf currentColumn Is TimeAttendanceLogDataGridTimeOutButton Then
            If currentLog.IsTimeIn = False Then
                e.CellStyle.BackColor = Color.Green
                e.CellStyle.SelectionBackColor = Color.Green
                e.CellStyle.SelectionForeColor = Color.Black
                e.CellStyle.Font = New Font(e.CellStyle.Font, FontStyle.Bold)
            Else
                e.CellStyle.BackColor = Color.White
                e.CellStyle.SelectionBackColor = Color.White
                e.CellStyle.SelectionForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub TimeAttendanceLogDataGrid_CellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) _
        Handles TimeAttendanceLogDataGrid.CellMouseUp

        TimeAttendanceLogDataGrid.EndEdit()

        TimeAttendanceLogDataGrid.Refresh()

    End Sub

    Private Sub TimeAttendanceLogDataGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) _
                                           Handles TimeAttendanceLogDataGrid.CellContentClick

        Dim currentLog As ImportTimeAttendanceLog = GetCurrentTimeLogByGridRowIndex(e.RowIndex)

        If currentLog Is Nothing Then Return

        If e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then Return

        Dim currentColumn = TimeAttendanceLogDataGrid.Columns(e.ColumnIndex)

        If TypeOf currentColumn IsNot DataGridViewButtonColumn Then Return

        If currentColumn Is TimeAttendanceLogDataGridTimeInButton Then

            If currentLog.IsTimeIn Is Nothing OrElse currentLog.IsTimeIn = False Then
                currentLog.IsTimeIn = True
            Else
                'Reset IsTimeIn if it is currently TRUE and he clicked IN button
                'currentLog.IsTimeIn = Nothing
            End If

        ElseIf currentColumn Is TimeAttendanceLogDataGridTimeOutButton Then

            If currentLog.IsTimeIn Is Nothing OrElse currentLog.IsTimeIn = True Then
                currentLog.IsTimeIn = False
            Else
                'Reset IsTimeIn if it is currently FALSE and he clicked OUT button
                'currentLog.IsTimeIn = Nothing
            End If

        ElseIf currentColumn Is TimeAttendanceLogDataGridDecrementLogDayButton Then

            If currentLog.LogDate IsNot Nothing Then
                currentLog.LogDate = currentLog.LogDate.Value.AddDays(-1)
            End If

        ElseIf currentColumn Is TimeAttendanceLogDataGridIncrementLogDayButton Then

            If currentLog.LogDate IsNot Nothing Then
                currentLog.LogDate = currentLog.LogDate.Value.AddDays(1)
            End If
        End If

    End Sub

    Private Function GetCurrentTimeLogByGridRowIndex(rowIndex As Integer) As ImportTimeAttendanceLog

        If rowIndex < 0 Then Return Nothing

        If rowIndex > Me._logs.Count - 1 Then Return Nothing

        Return Me._logs(rowIndex)

    End Function

    Private Async Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged

        Dim searchValue = txtSearch.Text.ToLower()

        Me._logs = Await Task.Run(Function() As IList(Of ImportTimeAttendanceLog)
                                      Return Me._originalLogs.
                                          Where(Function(o) o.EmployeeFullName.ToLower.Contains(searchValue) _
                                            OrElse o.EmployeeNumber.ToLower.Contains(searchValue)).
                                          ToList
                                  End Function)

        TimeAttendanceLogDataGrid.DataSource = Me._logs

    End Sub

    Private Async Sub txtErrorSearch_TextChanged(sender As Object, e As EventArgs) Handles txtErrorSearch.TextChanged

        Dim searchValue = txtErrorSearch.Text.ToLower()

        Me._errors = Await Task.Run(Function() As IList(Of ImportTimeAttendanceLog)
                                        Return Me._originalErrors.
                                          Where(Function(o) o.LineContent.ToLower.Contains(searchValue)).
                                          ToList
                                    End Function)

        TimeAttendanceLogErrorsDataGrid.DataSource = Me._errors

    End Sub

End Class