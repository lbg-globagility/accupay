Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class AddCertificationForm

    Private _employee As Employee

    Private _newCertification As Certification
    Public Property IsSaved As Boolean
    Public Property ShowBalloon As Boolean

    Public Sub New(employee As Employee)

        InitializeComponent()

        _employee = employee

    End Sub

    Private Sub AddCertificationForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtFullName.Text = _employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = _employee.EmployeeIdWithPositionAndEmployeeType
        pbEmployee.Image = ConvByteToImage(_employee.Image)
    End Sub

    Private Async Sub AddAndCloseButton_Click(sender As Object, e As EventArgs) Handles AddAndCloseButton.Click, AddAndNewButton.Click
        pbEmployee.Focus() 'To lose focus on current control
        Dim succeed As Boolean
        Dim messageTitle = ""
        If String.IsNullOrWhiteSpace(txtCertificationType.Text) Then
            messageTitle = "Invalid Input"
            ShowBalloonInfo("Certification Type empty.", messageTitle)
            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync("New Certification",
            Async Function()
                _newCertification = New Certification
                With _newCertification
                    .CertificationType = txtCertificationType.Text
                    .IssuingAuthority = txtIssuingAuthority.Text
                    .CertificationNo = txtCertificationNo.Text
                    .IssueDate = dtpIssueDate.Value
                    If dtpExpirationDate.Checked Then
                        .ExpirationDate = dtpExpirationDate.Value
                    Else
                        .ExpirationDate = Nothing
                    End If
                    .Comments = txtComments.Text
                    .OrganizationID = z_OrganizationID
                    .EmployeeID = _employee.RowID.Value
                End With

                Dim dataService = MainServiceProvider.GetRequiredService(Of ICertificationDataService)
                Await dataService.SaveAsync(_newCertification, z_User)

                succeed = True
            End Function)

        If succeed Then
            IsSaved = True

            If sender Is AddAndNewButton Then
                ShowBalloonInfo("Bonus successfully added.", "Saved")
                ClearForm()
            Else
                ShowBalloon = True
                Me.Close()
            End If
        End If
    End Sub

    Private Sub ClearForm()
        txtCertificationType.Text = ""
        txtIssuingAuthority.Text = ""
        txtCertificationNo.Text = ""
        dtpIssueDate.Value = Today
        dtpExpirationDate.Enabled = True
        dtpExpirationDate.Value = Today
        txtComments.Text = ""
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, pbEmployee, 70, -74)
    End Sub

    Private Sub Dates_ValueChanged(sender As Object, e As EventArgs) Handles dtpIssueDate.ValueChanged, dtpExpirationDate.ValueChanged
        ValidateDate()
    End Sub

    Private Sub ValidateDate()
        If dtpExpirationDate.Value < dtpIssueDate.Value And dtpExpirationDate.Checked Then
            dtpExpirationDate.Value = dtpIssueDate.Value
        End If
    End Sub

    Private Sub CancelDialogButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click
        Me.Close()
    End Sub

End Class
