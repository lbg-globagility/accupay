Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Helper.TimeLogsReader
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Utils

Public Class TimeLogsForm_PreviewAlternateLineImportTimeLogsDialog

    Private _logs As IList(Of ImportTimeAttendanceLog)

    Private _errors As IList(Of ImportTimeAttendanceLog)

    Private _originalLogs As IList(Of ImportTimeAttendanceLog)

    Private _originalErrors As IList(Of ImportTimeAttendanceLog)

    Public Property Cancelled As Boolean

    Private _dtp As New DateTimePicker()
    Private _Rectangle As Rectangle

    Private ReadOnly _timeAttendanceHelper As ITimeAttendanceHelper

    Private _employeeDetails As New List(Of EmployeeDetail)

    Sub New(timeAttendanceHelper As ITimeAttendanceHelper, otherErrorLogs As IList(Of ImportTimeAttendanceLog))

        _timeAttendanceHelper = timeAttendanceHelper

        'determines the IstimeIn, LogDate, and Employee values
        Dim allLogs = _timeAttendanceHelper.Analyze()

        Dim validLogs = allLogs.Where(Function(l) l.HasError = False).ToList()
        Dim invalidLogs = allLogs.Where(Function(l) l.HasError = True).ToList()

        invalidLogs.AddRange(otherErrorLogs)

        Me._originalLogs = validLogs.
                    OrderBy(Function(l) l.Employee?.LastName).
                    ThenBy(Function(l) l.Employee?.FirstName).
                    ThenBy(Function(l) l.Employee?.MiddleName).
                    ThenBy(Function(l) l.LogDate).
                    ThenBy(Function(l) l.DateTime).
                    ToList

        Me._logs = Me._originalLogs

        Me._originalErrors = invalidLogs.
                    OrderBy(Function(l) l.LineNumber).
                    ToList()

        Me._errors = Me._originalErrors

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Cancelled = True
        Me._timeAttendanceHelper = _timeAttendanceHelper
    End Sub

    Private Sub TimeAttendanceLogDataGrid_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) _
        Handles TimeAttendanceLogDataGrid.CellClick
        If TimeAttendanceLogDataGrid.Columns(e.ColumnIndex) Is TimeAttendanceLogDataGridLogDate Then
            _Rectangle = TimeAttendanceLogDataGrid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, True)
            _dtp.Size = New Size(_Rectangle.Width, _Rectangle.Height)
            _dtp.Location = New Point(_Rectangle.X, _Rectangle.Y)

            Dim currentLog As ImportTimeAttendanceLog = GetCurrentTimeLogByGridRowIndex(e.RowIndex)

            If currentLog Is Nothing Then Return

            If currentLog.LogDate Is Nothing Then

                currentLog.LogDate = currentLog.DateTime.ToMinimumHourValue()

            End If

            _dtp.Value = currentLog.LogDate.Value

            '_dtp.Bac

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

        TabControl1.Visible = False

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

        EmployeeFilterComboBox.ValueMember = "RowID"
        EmployeeFilterComboBox.DisplayMember = "Description"

    End Sub

    Private Sub TimeLogsForm_PreviewAlternateLineImportTimeLogsDialog_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        ValidateLogs(True)
        TabControl1.Visible = True

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
        Else
            e.CellStyle.BackColor = If(currentLog.HasWarning, Color.Yellow, Color.White)
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
                currentLog.IsTimeIn = Nothing
            End If

        ElseIf currentColumn Is TimeAttendanceLogDataGridTimeOutButton Then

            If currentLog.IsTimeIn Is Nothing OrElse currentLog.IsTimeIn = True Then
                currentLog.IsTimeIn = False
            Else
                'Reset IsTimeIn if it is currently FALSE and he clicked OUT button
                currentLog.IsTimeIn = Nothing
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

    'Private Async Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged

    '    Dim searchValue = txtSearch.Text.ToLower()

    '    Me._logs = Await Task.Run(Function() As IList(Of ImportTimeAttendanceLog)
    '                                  Return Me._originalLogs.
    '                                      Where(Function(o) o.EmployeeFullName.ToLower.Contains(searchValue) _
    '                                        OrElse o.EmployeeNumber.ToLower.Contains(searchValue)).
    '                                      ToList
    '                              End Function)

    '    TimeAttendanceLogDataGrid.DataSource = Me._logs

    'End Sub

    Private Async Sub txtErrorSearch_TextChanged(sender As Object, e As EventArgs) Handles txtErrorSearch.TextChanged

        Dim searchValue = txtErrorSearch.Text.ToLower()

        Me._errors = Await Task.Run(Function() As IList(Of ImportTimeAttendanceLog)
                                        Return Me._originalErrors.
                                          Where(Function(o) o.LineContent.ToLower.Contains(searchValue)).
                                          ToList
                                    End Function)

        TimeAttendanceLogErrorsDataGrid.DataSource = Me._errors

    End Sub

    Private Sub BtnRevalidate_Click(sender As Object, e As EventArgs) Handles btnRevalidate.Click
        ValidateLogs()

    End Sub

    Private Sub ValidateLogs(Optional isFirstLoad As Boolean = False)
        Me.Cursor = Cursors.WaitCursor

        _timeAttendanceHelper.Validate()

        Me.Cursor = Cursors.Default

        Dim warningLogsCount = Me._originalLogs.Where(Function(l) l.HasWarning).Count

        btnRevalidate.Text = $"&Revalidate Logs ({warningLogsCount})"

        Dim messageTitle = "Logs Revalidation"

        If warningLogsCount > 0 Then

            If isFirstLoad Then

                MessageBoxHelper.Warning($"There are {warningLogsCount} warning(s). It is advisable to import the overtimes and shifts first before importing the time logs.", messageTitle, MessageBoxButtons.OK)
            Else

                MessageBoxHelper.Warning($"{warningLogsCount} warnings remains.", messageTitle, MessageBoxButtons.OK)

            End If
        Else

            If isFirstLoad = False Then

                MessageBoxHelper.Information("No more warnings!", messageTitle)
            Else

            End If

        End If

        RefreshEmployeeComboBox()

    End Sub

    Private Sub EmployeeFilterComboBox_DrawItem(sender As Object, e As DrawItemEventArgs) Handles EmployeeFilterComboBox.DrawItem

        Dim backgroundColor As Brush
        Dim g = e.Graphics
        Dim rect = e.Bounds
        Dim displayString = ""
        Dim font = e.Font

        Dim selectedEmployee As EmployeeDetail

        If e.Index < 0 Then

            Return
        Else

            selectedEmployee = _employeeDetails(e.Index)
        End If

        If Nullable.Equals(selectedEmployee?.HasWarning, True) Then

            backgroundColor = Brushes.Yellow
        Else

            backgroundColor = Brushes.White

        End If

        If e.Index >= 0 AndAlso selectedEmployee IsNot Nothing Then

            displayString = selectedEmployee.Description

        End If

        g.FillRectangle(backgroundColor, rect.X, rect.Y, rect.Width, rect.Height)
        g.DrawString(displayString, font, Brushes.Black, rect.X, rect.Y)

    End Sub

    Private Async Sub EmployeeFilterComboBox_SelectedValueChanged(sender As Object, e As EventArgs) Handles EmployeeFilterComboBox.SelectedValueChanged
        Await RefreshFilteredLogs()

    End Sub

    Private Async Function RefreshFilteredLogs() As Task
        If EmployeeFilterComboBox.SelectedIndex < 0 OrElse
                        EmployeeFilterComboBox.SelectedIndex >= _employeeDetails.Count Then

            Return
        End If

        Dim selectedEmployee = _employeeDetails(EmployeeFilterComboBox.SelectedIndex)

        If selectedEmployee Is Nothing OrElse selectedEmployee.RowID Is Nothing Then

            Me._logs = Me._originalLogs
        Else

            Me._logs = Await Task.Run(Function() As IList(Of ImportTimeAttendanceLog)
                                          Return Me._originalLogs.
                                              Where(Function(o) Nullable.Equals(selectedEmployee.RowID, o.Employee?.RowID)).
                                              ToList
                                      End Function)

        End If

        TimeAttendanceLogDataGrid.DataSource = Me._logs
    End Function

    Private Sub RefreshEmployeeComboBox()
        Dim employeeDetails As New List(Of EmployeeDetail)

        employeeDetails.Add(New EmployeeDetail With {
                     .RowID = Nothing,
                     .Name = "<ALL>",
                     .HasWarning = False
        })

        For Each log In Me._originalLogs

            If log.HasError Then Continue For
            If log.Employee?.RowID Is Nothing Then Continue For

            Dim employee = employeeDetails.
                            Where(Function(e) Nullable.Equals(e.RowID, log.Employee?.RowID)).
                            FirstOrDefault

            If employee Is Nothing Then

                employee = New EmployeeDetail() With {
                    .RowID = log.Employee.RowID,
                    .Name = log.EmployeeFullName,
                    .EmployeeNumber = log.EmployeeNumber,
                    .HasWarning = False
                }

                employeeDetails.Add(employee)

            End If

            If log.HasWarning = False Then Continue For

            employee.HasWarning = True

        Next

        Dim oldSelectedIndex = EmployeeFilterComboBox.SelectedIndex

        _employeeDetails = employeeDetails

        EmployeeFilterComboBox.DataSource = _employeeDetails

        If oldSelectedIndex > 0 AndAlso
            oldSelectedIndex < _employeeDetails.Count Then

            EmployeeFilterComboBox.SelectedIndex = oldSelectedIndex

        End If

        EmployeeFilterComboBox.Refresh()
    End Sub

    Public Class EmployeeDetail

        Public Property RowID As Integer?
        Public Property Name As String
        Public Property EmployeeNumber As String
        Public Property HasWarning As Boolean

        Public ReadOnly Property Description() As String
            Get
                If String.IsNullOrWhiteSpace(EmployeeNumber) Then

                    Return Name
                Else

                    Return $"{EmployeeNumber} - {Name}"

                End If

            End Get
        End Property

    End Class

End Class