Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class AddPreviousEmployerForm

    Public Property IsSaved As Boolean
    Public Property ShowBalloon As Boolean

    Private _employee As Employee

    Private _newPreviousEmployer As PreviousEmployer

    Public Sub New(employee As Employee)

        InitializeComponent()

        _employee = employee

    End Sub

    Private Sub AddAwardForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtFullName.Text = _employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = _employee.EmployeeIdWithPositionAndEmployeeType
        pbEmployee.Image = ConvByteToImage(_employee.Image)

        ClearForm()
    End Sub

    Private Async Sub AddAndCloseButton_Click(sender As Object, e As EventArgs) Handles AddAndCloseButton.Click, AddAndNewButton.Click
        pbEmployee.Focus() 'To lose focus on current control
        Dim succeed As Boolean
        Dim messageBody = ""

        If String.IsNullOrWhiteSpace(txtName.Text) Then
            messageBody = "Company Name is empty."
        ElseIf String.IsNullOrWhiteSpace(txtContactName.Text) Then
            messageBody = "Contact Name is empty."
        ElseIf String.IsNullOrWhiteSpace(txtMainPhone.Text) Then
            messageBody = "Main Phone is empty."
        ElseIf String.IsNullOrWhiteSpace(txtEmailAdd.Text) Then
            messageBody = "Email Address is empty."
        ElseIf String.IsNullOrWhiteSpace(txtCompAddr.Text) Then
            messageBody = "Company Address is empty."
        End If

        If messageBody <> "" Then
            ShowBalloonInfo(messageBody, "Invalid Input")
            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync("New Previous Employer",
            Async Function()
                _newPreviousEmployer = New PreviousEmployer
                With _newPreviousEmployer
                    .Name = txtName.Text
                    .TradeName = txtTradeName.Text
                    .ContactName = txtContactName.Text
                    .MainPhone = txtMainPhone.Text
                    .AltPhone = txtAltPhone.Text
                    .FaxNumber = txtFaxNo.Text
                    .EmailAddress = txtEmailAdd.Text
                    .AltEmailAddress = txtAltEmailAdd.Text
                    .URL = txtUrl.Text
                    .TINNo = txtTinNo.Text
                    .JobTitle = txtJobTitle.Text
                    .JobFunction = txtJobFunction.Text
                    .OrganizationType = txtOrganizationType.Text
                    .ExperienceFrom = dtpExfrom.Value
                    .ExperienceTo = dtpExto.Value
                    .BusinessAddress = txtCompAddr.Text
                    .OrganizationID = z_OrganizationID
                    .EmployeeID = _employee.RowID.Value
                End With

                Dim dataService = MainServiceProvider.GetRequiredService(Of IPreviousEmployerDataService)
                Await dataService.SaveAsync(_newPreviousEmployer, z_User)

                succeed = True
            End Function)

        If succeed Then
            IsSaved = True

            If sender Is AddAndNewButton Then
                ShowBalloonInfo("Previous Employer successfully added.", "Saved")
                ClearForm()
            Else
                ShowBalloon = True
                Me.Close()
            End If
        End If

    End Sub

    Private Sub ClearForm()
        txtName.Text = ""
        txtTradeName.Text = ""
        txtContactName.Text = ""
        txtMainPhone.Text = ""
        txtAltPhone.Text = ""
        txtFaxNo.Text = ""
        txtEmailAdd.Text = ""
        txtAltEmailAdd.Text = ""
        txtUrl.Text = ""
        txtTinNo.Text = ""
        txtJobTitle.Text = ""
        txtJobFunction.Text = ""
        txtOrganizationType.Text = ""
        dtpExfrom.Value = Today
        dtpExto.Value = Today
        txtCompAddr.Text = ""
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, pbEmployee, 70, -74)
    End Sub

    Private Sub CancelDialogButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Me.Close()
    End Sub

    Private Sub Dates_ValueCHanged(sender As Object, e As EventArgs) Handles dtpExfrom.ValueChanged, dtpExto.ValueChanged
        If dtpExto.Value < dtpExfrom.Value Then
            dtpExto.Value = dtpExfrom.Value
        End If
    End Sub

End Class
