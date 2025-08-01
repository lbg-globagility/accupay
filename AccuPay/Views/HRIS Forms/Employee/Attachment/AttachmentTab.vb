Option Strict On

Imports System.IO
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports AccuPay.Desktop.Enums
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class AttachmentTab

    Private Const FormEntityName As String = "Attachment"

    Private _employee As Employee

    Private _attachments As IEnumerable(Of Attachment)

    Private _currentAttachment As Attachment

    Private _mode As FormMode = FormMode.Empty

    Private ReadOnly _attachmentRepo As IAttachmentRepository

    Private ReadOnly _listOfValRepo As IListOfValueRepository

    Public Sub New()

        InitializeComponent()

        dgvAttachments.AutoGenerateColumns = False

        If MainServiceProvider IsNot Nothing Then

            _attachmentRepo = MainServiceProvider.GetRequiredService(Of IAttachmentRepository)
            _listOfValRepo = MainServiceProvider.GetRequiredService(Of IListOfValueRepository)
        End If

    End Sub

    Public Async Function SetEmployee(employee As Employee) As Task

        _employee = employee

        txtFullname.Text = employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = employee.EmployeeIdWithPositionAndEmployeeType
        pbEmployee.Image = ConvByteToImage(employee.Image)

        Await LoadAttachments()
    End Function

    Private Async Function LoadAttachments() As Task
        If _employee?.RowID Is Nothing Then Return

        _attachments = Await _attachmentRepo.GetByEmployeeAsync(_employee.RowID.Value)

        RemoveHandler dgvAttachments.SelectionChanged, AddressOf dgvAttachments_SelectionChanged

        pbAttachment.Image = Nothing

        dgvAttachments.DataSource = _attachments

        If _attachments.Count > 0 Then
            SelectAttachment(DirectCast(dgvAttachments.CurrentRow?.DataBoundItem, Attachment))
            ChangeMode(FormMode.Editing)
        Else
            SelectAttachment(Nothing)
            _currentAttachment = New Attachment
            ChangeMode(FormMode.Empty)
        End If

        AddHandler dgvAttachments.SelectionChanged, AddressOf dgvAttachments_SelectionChanged
    End Function

    Private Sub ChangeMode(mode As FormMode)
        _mode = mode

        Select Case _mode
            Case FormMode.Disabled
                btnNew.Enabled = False
                btnDelete.Enabled = False
            Case FormMode.Empty
                btnNew.Enabled = True
                btnDelete.Enabled = False
            Case FormMode.Creating
                btnNew.Enabled = False
                btnDelete.Enabled = False
            Case FormMode.Editing
                btnNew.Enabled = True
                btnDelete.Enabled = True
        End Select
    End Sub

    Private Sub SelectAttachment(attachment As Attachment)
        If attachment?.FileType IsNot Nothing Then
            _currentAttachment = attachment

            With _currentAttachment
                If .FileType.ToLower = ".png" OrElse
                    .FileType.ToLower = ".jpg" OrElse
                    .FileType.ToLower = ".jpeg" Then
                    pbAttachment.Image = ConvertByteToImage(.AttachedFile)
                Else
                    pbAttachment.Image = Nothing
                End If
            End With

        End If

    End Sub

    Private Sub dgvAttachments_SelectionChanged(sender As Object, e As EventArgs) Handles dgvAttachments.SelectionChanged
        If _attachments.Count > 0 Then
            Dim attachment = DirectCast(dgvAttachments.CurrentRow?.DataBoundItem, Attachment)
            SelectAttachment(attachment)

        End If
    End Sub

    Private Sub ToolStripButton16_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        EmployeeForm.Close()
    End Sub

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        If _currentAttachment?.RowID Is Nothing Then

            MessageBoxHelper.Warning("No selected attachment!")
            Return
        End If

        Dim result = MsgBox("Are you sure you want to delete this Attachment?", MsgBoxStyle.YesNo, "Delete Attachment")

        If result = MsgBoxResult.Yes Then
            Await FunctionUtils.TryCatchFunctionAsync("Delete Attachment",
                Async Function()

                    Dim dataService = MainServiceProvider.GetRequiredService(Of IAttachmentDataService)
                    Await dataService.DeleteAsync(_currentAttachment.RowID.Value, z_User)

                    Await LoadAttachments()
                End Function)

        End If
    End Sub

    Private Async Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        If _employee Is Nothing Then
            MessageBoxHelper.ErrorMessage("Please select an employee first.")
            Return
        End If

        Dim form As New AddAttachmentForm(_employee)
        form.ShowDialog()

        If form.IsSaved Then
            Await LoadAttachments()

            If form.ShowBalloon Then
                ShowBalloonInfo("Attachment successfuly added.", "Saved")
            End If

        End If
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, pbEmployee, 100, -110)
    End Sub

    Private Sub btnattadl_Click(sender As Object, e As EventArgs) Handles btnattadl.Click
        dgvAttachments.Focus()

        Dim downloadFile As SaveFileDialog = New SaveFileDialog
        downloadFile.RestoreDirectory = True

        downloadFile.Filter = "All files (*.*)|*.*" &
                         "|JPEG (*.jpg)|*.jpg" &
                         "|PNG (*.PNG)|*.png" &
                         "|MS Word 97-2003 Document (*.doc)|*.doc" &
                         "|MS Word Document (*.docx)|*.docx" &
                         "|MS Excel 97-2003 Workbook (*.xls)|*.xls" &
                         "|MS Excel Workbook (*.xlsx)|*.xlsx"

        If downloadFile.ShowDialog = Windows.Forms.DialogResult.OK Then

            Dim savefilepath As String =
                    Path.GetFileNameWithoutExtension(downloadFile.FileName) &
                    _currentAttachment.FileType

            Dim fs As New FileStream(savefilepath, FileMode.Create)
            Dim blob As Byte() = _currentAttachment.AttachedFile
            fs.Write(blob, 0, blob.Length)
            fs.Close()
            fs = Nothing

            Dim prompt = MessageBox.Show("Do you want to open the saved file ?", "Show file", MessageBoxButtons.YesNoCancel)

            If prompt = Windows.Forms.DialogResult.Yes Then
                Process.Start(savefilepath)
            End If

        End If

    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles btnUserActivity.Click
        Dim userActivity As New UserActivityForm(FormEntityName)
        userActivity.ShowDialog()
    End Sub

    Private Sub dgvAttachments_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAttachments.CellContentClick
        Dim senderGrid = DirectCast(sender, DataGridView)

        If TypeOf senderGrid.Columns(e.ColumnIndex) Is DataGridViewButtonColumn AndAlso
            e.RowIndex >= 0 Then

            Dim savefilepath = Path.GetTempPath &
                _currentAttachment.FileName &
                _currentAttachment.FileType

            Dim fs As New FileStream(savefilepath, FileMode.Create)

            Dim blob As Byte() = DirectCast(_currentAttachment.AttachedFile, Byte())
            fs.Write(blob, 0, blob.Length)
            fs.Close()

            Process.Start(savefilepath)
        End If
    End Sub

End Class
