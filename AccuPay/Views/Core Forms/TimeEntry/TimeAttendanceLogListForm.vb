Imports AccuPay.Entity

Public Class TimeAttendanceLogListForm

    Private _timeAttendanceLogs As New List(Of TimeAttendanceLog)

    Sub New(timeAttendanceLogs As List(Of TimeAttendanceLog))

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        _timeAttendanceLogs = timeAttendanceLogs
    End Sub

    Private Sub TimeAttendanceLogListForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        TimeAttendanceLogDataGrid.AutoGenerateColumns = False

        TimeAttendanceLogDataGrid.DataSource = _timeAttendanceLogs

    End Sub

    Private Sub TimeAttendanceLogDataGrid_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles TimeAttendanceLogDataGrid.CellFormatting

        Dim currentLog As TimeAttendanceLog = GetCurrentTimeLogByGridRowIndex(e.RowIndex)

        If currentLog Is Nothing Then Return


        Dim currentColumn As DataGridViewColumn = TimeAttendanceLogDataGrid.Columns(e.ColumnIndex)

        If currentColumn Is TimeAttendanceLogDataGridLogType Then

            If currentLog.IsTimeIn Is Nothing Then
                e.CellStyle.BackColor = Color.White
                e.CellStyle.SelectionBackColor = Color.White
                e.CellStyle.SelectionForeColor = Color.Black

            ElseIf currentLog.IsTimeIn = True Then
                e.CellStyle.BackColor = Color.Green
                e.CellStyle.SelectionBackColor = Color.Green
                e.CellStyle.SelectionForeColor = Color.Black
                e.CellStyle.Font = New Font(e.CellStyle.Font, FontStyle.Bold)

            ElseIf currentLog.IsTimeIn = False Then
                e.CellStyle.BackColor = Color.Yellow
                e.CellStyle.SelectionBackColor = Color.Yellow
                e.CellStyle.SelectionForeColor = Color.Black
                e.CellStyle.Font = New Font(e.CellStyle.Font, FontStyle.Bold)
            End If
        End If

    End Sub

    Private Function GetCurrentTimeLogByGridRowIndex(rowIndex As Integer) As TimeAttendanceLog

        If rowIndex < 0 Then Return Nothing

        If rowIndex > Me._timeAttendanceLogs.Count - 1 Then Return Nothing

        Return Me._timeAttendanceLogs(rowIndex)

    End Function

End Class