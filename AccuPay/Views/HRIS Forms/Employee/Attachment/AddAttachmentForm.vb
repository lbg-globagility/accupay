Option Strict On

Imports System.IO
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class AddAttachmentForm

    Public Property IsSaved As Boolean

    Public Property ShowBalloon As Boolean

    Private _types As IEnumerable(Of ListOfValue)

    Private _newAttachment As Attachment

    Private _employee As Employee

    Private ReadOnly _listOfValRepo As IListOfValueRepository

    Public Sub New(employee As Employee)

        InitializeComponent()

        _employee = employee

        _listOfValRepo = MainServiceProvider.GetRequiredService(Of IListOfValueRepository)
    End Sub

    Private Async Sub AddAttachmentForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        EmployeeNameTextBox.Text = _employee.FullNameWithMiddleInitialLastNameFirst
        EmployeeNumberTextbox.Text = _employee.EmployeeIdWithPositionAndEmployeeType
        EmployeePictureBox.Image = ConvByteToImage(_employee.Image)

        _types = Await _listOfValRepo.GetEmployeeChecklistsAsync
        cboType.DataSource = _types

        ClearForm()
    End Sub

    Private Sub ClearForm()
        cboType.SelectedItem = Nothing
        ClearAttachment()
    End Sub

    Private Sub ClearAttachment()
        _newAttachment = New Attachment
        txtFileExtension.Clear()
        txtFileName.Clear()
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Dim browsefile As OpenFileDialog = New OpenFileDialog()

        browsefile.Filter = "JPEG(*.jpg)|*.jpg"

        browsefile.Filter =
            "All files (*.*)|*.*" &
            "|JPEG (*.jpg)|*.jpg" &
            "|PNG (*.PNG)|*.png" &
            "|MS Word 97-2003 Document (*.doc)|*.doc" &
            "|MS Word Document (*.docx)|*.docx" &
            "|MS Excel 97-2003 Workbook (*.xls)|*.xls" &
            "|MS Excel Workbook (*.xlsx)|*.xlsx"

        If browsefile.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim file = browsefile.FileName

            _newAttachment.AttachedFile = convertFileToByte(file)
            txtFileName.Text = Path.GetFileNameWithoutExtension(file)
            txtFileExtension.Text = Path.GetExtension(file)
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearAttachment()
    End Sub

    Private Sub CancelDialogButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Me.Close()
    End Sub

    Private Async Sub AddAndCloseButton_Click(sender As Object, e As EventArgs) Handles AddAndCloseButton.Click, AddAndNewButton.Click
        EmployeePictureBox.Focus() 'To lose focus on current control
        Dim succeed As Boolean
        Dim messageTitle = ""

        If cboType.SelectedItem Is Nothing Then
            messageTitle = "Invalid Type"
        ElseIf String.IsNullOrEmpty(txtFileName.Text) Then
            messageTitle = "Invalid File"
        End If

        If messageTitle <> "" Then
            ShowBalloonInfo("Error Input.", messageTitle)
            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync("New Attachment",
        Async Function()
            With _newAttachment
                .OrganizationID = z_OrganizationID
                .Type = cboType.Text
                .FileName = txtFileName.Text
                .FileType = txtFileExtension.Text
                .EmployeeID = _employee.RowID.Value
            End With

            Dim dataService = MainServiceProvider.GetRequiredService(Of IAttachmentDataService)
            Await dataService.SaveAsync(_newAttachment, z_User)

            succeed = True

        End Function)

        If succeed Then
            IsSaved = True

            If sender Is AddAndNewButton Then
                ShowBalloonInfo("Attachment successfully added.", "Saved")
                ClearForm()
            Else
                ShowBalloon = True
                Me.Close()
            End If
        End If
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, EmployeePictureBox, 67, -67)
    End Sub

End Class
