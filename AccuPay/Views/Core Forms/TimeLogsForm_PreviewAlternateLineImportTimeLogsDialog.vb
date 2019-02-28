Imports AccuPay

Public Class TimeLogsForm_PreviewAlternateLineImportTimeLogsDialog

    Public Property Logs As IList(Of TimeAttendanceLog)

    Public Property Errors As IList(Of TimeLogsReader.ErrorLog)

    Public Property Cancelled As Boolean

    Public Property IsChangeableType As Boolean

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Cancelled = True

        'This will be changed immediately to false on Load
        Me.IsChangeableType = True

    End Sub

    Private Sub TimeLogsForm_PreviewAlternateLineImportTimeLogsDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ToggleChangeableType()

        Dim errorCount = Me.Errors.Count

        If errorCount > 0 Then

            If errorCount = 1 Then
                lblStatus.Text = $"There is 1 error."
            Else
                lblStatus.Text = $"There are {errorCount} errors."

            End If

            lblStatus.Text += "Failed logs will not be saved."
            lblStatus.BackColor = Color.Red


        Else
            lblStatus.Text = $"There is no error."
            lblStatus.BackColor = Color.Green
        End If


        dgvLogs.AutoGenerateColumns = False

        dgvErrors.AutoGenerateColumns = False

        ParsedTabControl.Text = $"Ok ({Me.Logs.Count})"
        ErrorsTabControl.Text = $"Errors ({errorCount})"

        dgvLogs.DataSource = Me.Logs

        dgvErrors.DataSource = Me.Errors

    End Sub


    Private Sub FooterButton_Click(sender As Object, e As EventArgs) _
        Handles btnOK.Click, btnClose.Click

        If sender Is btnOK Then

            Me.Cancelled = False

        End If

        Me.Close()

    End Sub

    Private Sub dgvErrors_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvErrors.CellFormatting
        Dim value As String = e.Value.ToString().Replace(vbTab, "     ")
        e.Value = value
    End Sub

    Private Sub dgvLogs_CellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvLogs.CellMouseUp

        dgvLogs.EndEdit()

        dgvLogs.Refresh()

    End Sub

    Private Sub btnChangeTimeType_Click(sender As Object, e As EventArgs) Handles btnChangeTimeType.Click

        ToggleChangeableType()

    End Sub

    Private Sub ToggleChangeableType()
        IsChangeableType = Not Me.IsChangeableType

        dgvLogsColumnType.Visible = IsChangeableType
        dgvLogsColumnIsTimeIn.Visible = IsChangeableType

        If IsChangeableType Then
            btnChangeTimeType.Text = "Ca&ncel Edit Time Type"

        Else
            btnChangeTimeType.Text = "&Edit Time Type"
        End If
    End Sub
End Class