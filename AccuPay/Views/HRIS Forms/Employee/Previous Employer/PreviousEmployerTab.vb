Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Enums
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class PreviousEmployerTab

    Private _employee As Employee

    Private _previousEmployers As IEnumerable(Of PreviousEmployer)

    Private _currentPrevEmployer As PreviousEmployer

    Private _mode As FormMode = FormMode.Empty

    Public Sub New()

        InitializeComponent()

        dgvPrevEmployers.AutoGenerateColumns = False

    End Sub

    Public Async Function SetEmployee(employee As Employee) As Task
        pbEmployee.Focus()
        _employee = employee

        txtFullname.Text = employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = employee.EmployeeIdWithPositionAndEmployeeType
        pbEmployee.Image = ConvByteToImage(employee.Image)

        Await LoadPrevEmployers()
    End Function

    Private Async Function LoadPrevEmployers() As Task
        If _employee?.RowID Is Nothing Then Return

        Dim previousEmployerRepo = MainServiceProvider.GetRequiredService(Of IPreviousEmployerRepository)
        _previousEmployers = Await previousEmployerRepo.GetListByEmployeeAsync(_employee.RowID.Value)

        RemoveHandler dgvPrevEmployers.SelectionChanged, AddressOf dgvPrevEmployers_SelectionChanged
        dgvPrevEmployers.DataSource = _previousEmployers

        If _previousEmployers.Count > 0 Then
            SelectPrevEmployer(DirectCast(dgvPrevEmployers.CurrentRow?.DataBoundItem, PreviousEmployer))
            ChangeMode(FormMode.Editing)
            FormToolsControl(True)
        Else
            SelectPrevEmployer(Nothing)
            _currentPrevEmployer = New PreviousEmployer
            ChangeMode(FormMode.Empty)
            FormToolsControl(False)
        End If

        AddHandler dgvPrevEmployers.SelectionChanged, AddressOf dgvPrevEmployers_SelectionChanged
    End Function

    Private Sub FormToolsControl(control As Boolean)
        txtName.Enabled = control
        txtTradeName.Enabled = control
        txtContactName.Enabled = control
        txtMainPhone.Enabled = control
        txtAltPhone.Enabled = control
        txtFaxNo.Enabled = control
        txtEmailAdd.Enabled = control
        txtAltEmailAdd.Enabled = control
        txtUrl.Enabled = control
        txtTinNo.Enabled = control
        txtJobFunction.Enabled = control
        txtJobTitle.Enabled = control
        txtOrganizationType.Enabled = control
        dtpExFrom.Enabled = control
        dtpExTo.Enabled = control
        txtCompAddr.Enabled = control
    End Sub

    Private Sub SelectPrevEmployer(previousEmployer As PreviousEmployer)
        If previousEmployer IsNot Nothing Then
            _currentPrevEmployer = previousEmployer
            With _currentPrevEmployer
                txtName.Text = .Name
                txtTradeName.Text = .TradeName
                txtContactName.Text = .ContactName
                txtMainPhone.Text = .MainPhone
                txtAltPhone.Text = .AltPhone
                txtFaxNo.Text = .FaxNumber
                txtEmailAdd.Text = .EmailAddress
                txtAltEmailAdd.Text = .AltEmailAddress
                txtUrl.Text = .URL
                txtTinNo.Text = .TINNo
                txtJobTitle.Text = .JobTitle
                txtJobFunction.Text = .JobFunction
                txtOrganizationType.Text = .OrganizationType
                dtpExFrom.Value = .ExperienceFrom
                dtpExTo.Value = .ExperienceTo
                txtCompAddr.Text = .BusinessAddress

            End With
        Else
            ClearForm()
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
        dtpExFrom.Value = Today
        dtpExTo.Value = Today
        txtCompAddr.Text = ""
    End Sub

    Private Sub dgvPrevEmployers_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPrevEmployers.SelectionChanged
        If _previousEmployers.Count > 0 Then
            Dim prevEmployer = DirectCast(dgvPrevEmployers.CurrentRow?.DataBoundItem, PreviousEmployer)
            SelectPrevEmployer(prevEmployer)

        End If
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

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        EmployeeForm.Close()
    End Sub

    Private Async Sub btnCancel_ClickAsync(sender As Object, e As EventArgs) Handles btnCancel.Click
        If _mode = FormMode.Creating Then
            SelectPrevEmployer(Nothing)
        ElseIf _mode = FormMode.Editing Then
            Await LoadPrevEmployers()
        End If

        If _currentPrevEmployer Is Nothing Then
            ChangeMode(FormMode.Empty)
        Else
            ChangeMode(FormMode.Editing)
        End If
    End Sub

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        If _currentPrevEmployer?.RowID Is Nothing Then

            MessageBoxHelper.Warning("No selected previous employer!")
            Return
        End If

        Dim result = MsgBox("Are you sure you want to delete this Company?", MsgBoxStyle.YesNo, "Delete Previous Employer")

        If result = MsgBoxResult.Yes Then
            Await FunctionUtils.TryCatchFunctionAsync("Delete Previous Employer",
                Async Function()
                    Dim dataService = MainServiceProvider.GetRequiredService(Of IPreviousEmployerDataService)
                    Await dataService.DeleteAsync(_currentPrevEmployer.RowID.Value, z_User)

                    Await LoadPrevEmployers()
                End Function)

        End If
    End Sub

    Private Async Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        If _employee Is Nothing Then
            MessageBoxHelper.ErrorMessage("Please select an employee first.")
            Return
        End If

        Dim form As New AddPreviousEmployerForm(_employee)
        form.ShowDialog()

        If form.IsSaved Then
            Await LoadPrevEmployers()

            If form.ShowBalloon Then
                ShowBalloonInfo("Previous Employer successfuly added.", "Saved")
            End If

        End If
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, pbEmployee, 47, -103)
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        pbEmployee.Focus()
        If Await SavePrevEmployer() Then
            Await LoadPrevEmployers()
        End If
    End Sub

    Private Function IsFormCorrect() As Boolean
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
            Return False
        End If
        Return True
    End Function

    Private Async Function SavePrevEmployer() As Task(Of Boolean)
        Dim messageTitle = ""
        Dim succeed As Boolean = False

        If Not IsFormCorrect() Then Return False

        Await FunctionUtils.TryCatchFunctionAsync("Save Previous Employer",
            Async Function()
                If IsChanged() Then

                    With _currentPrevEmployer
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
                        .ExperienceFrom = dtpExFrom.Value
                        .ExperienceTo = dtpExTo.Value
                        .BusinessAddress = txtCompAddr.Text
                    End With

                    Dim dataService = MainServiceProvider.GetRequiredService(Of IPreviousEmployerDataService)
                    Await dataService.SaveAsync(_currentPrevEmployer, z_User)

                    messageTitle = "Update Previous Employer"
                    succeed = True
                Else
                    MessageBoxHelper.Warning("No value changed")
                End If
            End Function)

        If succeed Then
            ShowBalloonInfo("Previous Employer successfuly saved.", messageTitle)
            Return True
        End If
        Return False
    End Function

    Private Sub btnUserActivity_Click(sender As Object, e As EventArgs) Handles btnUserActivity.Click

        Dim formEntityName As String = "Previous Employer"

        Dim userActivity As New UserActivityForm(formEntityName)
        userActivity.ShowDialog()
    End Sub

    Private Function IsChanged() As Boolean
        With _currentPrevEmployer
            If txtName.Text <> .Name OrElse
                txtTradeName.Text <> .TradeName OrElse
                txtContactName.Text <> .ContactName OrElse
                txtMainPhone.Text <> .MainPhone OrElse
                txtAltPhone.Text <> .AltPhone OrElse
                txtFaxNo.Text <> .FaxNumber OrElse
                txtEmailAdd.Text <> .EmailAddress OrElse
                txtAltEmailAdd.Text <> .AltEmailAddress OrElse
                txtUrl.Text <> .URL OrElse
                txtTinNo.Text <> .TINNo OrElse
                txtJobTitle.Text <> .JobTitle OrElse
                txtJobFunction.Text <> .JobFunction OrElse
                txtOrganizationType.Text <> .OrganizationType OrElse
                dtpExFrom.Value.ToShortDateString <> .ExperienceFrom.ToShortDateString OrElse
                dtpExTo.Value.ToShortDateString <> .ExperienceTo.ToShortDateString OrElse
                txtCompAddr.Text <> .BusinessAddress Then
                Return True
            End If
            Return False
        End With
    End Function

    Private Sub Dates_ValueCHanged(sender As Object, e As EventArgs) Handles dtpExFrom.ValueChanged, dtpExTo.ValueChanged
        If dtpExTo.Value <= dtpExFrom.Value Then
            dtpExTo.Value = dtpExFrom.Value
        End If
    End Sub

End Class
