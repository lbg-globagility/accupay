Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports AccuPay.Desktop.Enums
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class CertificationTab

    Private _employee As Employee

    Private _certifications As IEnumerable(Of Certification)

    Private _currentCertification As Certification

    Private _mode As FormMode = FormMode.Empty

    Public Sub New()

        InitializeComponent()

        dgvCertifications.AutoGenerateColumns = False

    End Sub

    Public Async Function SetEmployee(employee As Employee) As Task
        pbEmployee.Focus()
        _employee = employee

        txtFullname.Text = employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = employee.EmployeeIdWithPositionAndEmployeeType
        pbEmployee.Image = ConvByteToImage(employee.Image)

        Await LoadCertifications()
    End Function

    Private Async Function LoadCertifications() As Task
        If _employee?.RowID Is Nothing Then Return

        Dim certificationRepo = MainServiceProvider.GetRequiredService(Of ICertificationRepository)
        _certifications = Await certificationRepo.GetByEmployeeAsync(_employee.RowID.Value)

        RemoveHandler dgvCertifications.SelectionChanged, AddressOf dgvCertifications_SelectionChanged
        dgvCertifications.DataSource = _certifications

        If _certifications.Count > 0 Then
            SelectCertification(DirectCast(dgvCertifications.CurrentRow?.DataBoundItem, Certification))
            ChangeMode(FormMode.Editing)
            FormToolsControl(True)
        Else
            SelectCertification(Nothing)
            _currentCertification = New Certification
            ChangeMode(FormMode.Empty)
            FormToolsControl(False)
        End If

        AddHandler dgvCertifications.SelectionChanged, AddressOf dgvCertifications_SelectionChanged
    End Function

    Private Sub FormToolsControl(control As Boolean)
        txtCertificationType.Enabled = control
        txtIssuingAuthority.Enabled = control
        txtCertificationNo.Enabled = control
        dtpIssueDate.Enabled = control
        dtpExpirationDate.Enabled = control
        txtComments.Enabled = control
    End Sub

    Private Sub SelectCertification(certification As Certification)
        If certification IsNot Nothing Then
            _currentCertification = certification

            txtCertificationType.Text = _currentCertification.CertificationType
            txtIssuingAuthority.Text = _currentCertification.IssuingAuthority
            txtCertificationNo.Text = _currentCertification.CertificationNo
            dtpIssueDate.Value = _currentCertification.IssueDate
            If _currentCertification.ExpirationDate Is Nothing Then
                dtpExpirationDate.Value = _currentCertification.IssueDate
                dtpExpirationDate.Checked = False
            Else
                dtpExpirationDate.Checked = True
                dtpExpirationDate.Value = _currentCertification.ExpirationDate.Value
            End If
            txtComments.Text = _currentCertification.Comments
        Else
            ClearForm()
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

    Private Sub ChangeMode(mode As FormMode)
        _mode = mode

        Select Case _mode
            Case FormMode.Disabled
                btnNew.Enabled = False
                btnSave.Enabled = False
                btnDelete.Enabled = False
                btnCancel.Enabled = False
            Case FormMode.Empty
                btnNew.Enabled = True
                btnSave.Enabled = False
                btnDelete.Enabled = False
                btnCancel.Enabled = False
            Case FormMode.Creating
                btnNew.Enabled = False
                btnSave.Enabled = True
                btnDelete.Enabled = False
                btnCancel.Enabled = True
            Case FormMode.Editing
                btnNew.Enabled = True
                btnSave.Enabled = True
                btnDelete.Enabled = True
                btnCancel.Enabled = True
        End Select
    End Sub

    Private Sub dgvCertifications_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCertifications.SelectionChanged
        If _certifications.Count > 0 Then
            Dim certification = DirectCast(dgvCertifications.CurrentRow?.DataBoundItem, Certification)
            SelectCertification(certification)

        End If
    End Sub

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        If _currentCertification?.RowID Is Nothing Then

            MessageBoxHelper.Warning("No selected certification!")
            Return
        End If

        Dim result = MsgBox("Are you sure you want to delete this Certification?", MsgBoxStyle.YesNo, "Delete Certification")

        If result = MsgBoxResult.Yes Then
            Await FunctionUtils.TryCatchFunctionAsync("Delete Certification",
                Async Function()

                    Dim dataService = MainServiceProvider.GetRequiredService(Of ICertificationDataService)
                    Await dataService.DeleteAsync(_currentCertification.RowID.Value, z_User)

                    Await LoadCertifications()
                End Function)

        End If
    End Sub

    Private Async Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        If _employee Is Nothing Then
            MessageBoxHelper.ErrorMessage("Please select an employee first.")
            Return
        End If

        Dim form As New AddCertificationForm(_employee)
        form.ShowDialog()

        If form.IsSaved Then
            Await LoadCertifications()

            If form.ShowBalloon Then
                ShowBalloonInfo("Certification successfuly added.", "Saved")
            End If

        End If
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, pbEmployee, 47, -103)
    End Sub

    Private Sub Dates_ValueCHanged(sender As Object, e As EventArgs) Handles dtpIssueDate.ValueChanged, dtpExpirationDate.ValueChanged
        ValidateDate()
    End Sub

    Private Sub ValidateDate()
        If dtpExpirationDate.Value < dtpIssueDate.Value And dtpExpirationDate.Checked Then
            dtpExpirationDate.Value = dtpIssueDate.Value
        End If
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        pbEmployee.Focus()
        If Await SaveCertification() Then
            Await LoadCertifications()
        End If
    End Sub

    Private Async Function SaveCertification() As Task(Of Boolean)
        Dim messageTitle = ""
        Dim succeed As Boolean = False

        If String.IsNullOrWhiteSpace(txtCertificationType.Text) Then
            messageTitle = "Invalid Input"
            ShowBalloonInfo("Certification Type empty.", messageTitle)
            Return False
        End If

        Await FunctionUtils.TryCatchFunctionAsync("Save Certification",
            Async Function()
                If IsChanged() Then

                    With _currentCertification
                        .CertificationType = txtCertificationType.Text
                        .CertificationNo = txtCertificationNo.Text
                        .IssuingAuthority = txtIssuingAuthority.Text
                        .IssueDate = dtpIssueDate.Value
                        If dtpExpirationDate.Checked Then
                            .ExpirationDate = dtpExpirationDate.Value
                        Else
                            .ExpirationDate = Nothing
                        End If
                        .Comments = txtComments.Text
                    End With

                    Dim dataService = MainServiceProvider.GetRequiredService(Of ICertificationDataService)
                    Await dataService.SaveAsync(_currentCertification, z_User)

                    messageTitle = "Update Certification"
                    succeed = True
                Else
                    MessageBoxHelper.Warning("No value changed")
                End If
            End Function)

        If succeed Then
            ShowBalloonInfo("Certification successfuly saved.", messageTitle)
            Return True
        End If
        Return False
    End Function

    Private Function IsChanged() As Boolean
        If _currentCertification.CertificationType <> txtCertificationType.Text OrElse
                _currentCertification.IssuingAuthority <> txtIssuingAuthority.Text OrElse
                _currentCertification.CertificationNo <> txtCertificationNo.Text OrElse
                _currentCertification.IssueDate <> dtpIssueDate.Value OrElse
                (_currentCertification.ExpirationDate Is Nothing And dtpExpirationDate.Checked) OrElse      'from nothing to hasvalue.
                (_currentCertification.ExpirationDate.HasValue And Not dtpExpirationDate.Checked) OrElse    'from hasvalue to nothing.
                (_currentCertification.ExpirationDate.HasValue AndAlso dtpExpirationDate.Checked AndAlso    'check if both have value
                (_currentCertification.ExpirationDate.Value <> dtpExpirationDate.Value)) OrElse             'then check if changed.
            _currentCertification.Comments <> txtComments.Text Then
            Return True
        End If
        Return False
    End Function

    Private Sub ToolStripButton11_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        EmployeeForm.Close()
    End Sub

    Private Async Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If _mode = FormMode.Creating Then
            SelectCertification(Nothing)
        ElseIf _mode = FormMode.Editing Then
            Await LoadCertifications()
        End If

        If _currentCertification Is Nothing Then
            ChangeMode(FormMode.Empty)
        Else
            ChangeMode(FormMode.Editing)
        End If
    End Sub

    Private Sub UserActivity_Click(sender As Object, e As EventArgs) Handles btnUserActivity.Click

        Dim formEntityName As String = "Certification"

        Dim userActivity As New UserActivityForm(formEntityName)
        userActivity.ShowDialog()
    End Sub

End Class
