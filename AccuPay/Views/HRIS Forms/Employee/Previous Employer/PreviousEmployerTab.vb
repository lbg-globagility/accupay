Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Desktop.Enums
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class PreviousEmployerTab

    Private Const FormEntityName As String = "Previous Employer"

    Private _employee As Employee

    Private _previousEmployers As IEnumerable(Of PreviousEmployer)

    Private _currentPrevEmployer As PreviousEmployer

    Private _mode As FormMode = FormMode.Empty

    Private ReadOnly _userActivityRepo As UserActivityRepository

    Public Sub New()

        InitializeComponent()

        dgvPrevEmployers.AutoGenerateColumns = False

        If MainServiceProvider IsNot Nothing Then

            _userActivityRepo = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
        End If

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

        Dim previousEmployerRepo = MainServiceProvider.GetRequiredService(Of PreviousEmployerRepository)
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
        Dim result = MsgBox("Are you sure you want to delete this Company?", MsgBoxStyle.YesNo, "Delete Previous Employer")

        If result = MsgBoxResult.Yes Then
            Await FunctionUtils.TryCatchFunctionAsync("Delete Previous Employer",
                Async Function()
                    Dim previousEmployerRepo = MainServiceProvider.GetRequiredService(Of PreviousEmployerRepository)
                    Await previousEmployerRepo.DeleteAsync(_currentPrevEmployer)

                    Await _userActivityRepo.RecordDeleteAsync(
                        z_User,
                        FormEntityName,
                        entityId:=_currentPrevEmployer.RowID.Value,
                        organizationId:=z_OrganizationID,
                        changedEmployeeId:=_currentPrevEmployer.EmployeeID,
                        suffixIdentifier:=$" with name '{_currentPrevEmployer.Name}'")

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
                    Dim oldPrevEmployer = _currentPrevEmployer.CloneJson()

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

                    Dim previousEmployerRepo = MainServiceProvider.GetRequiredService(Of PreviousEmployerRepository)
                    Await previousEmployerRepo.UpdateAsync(_currentPrevEmployer)

                    RecordUpdatePrevEmployer(oldPrevEmployer)

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

    Private Sub RecordUpdatePrevEmployer(oldPrevEmployer As PreviousEmployer)
        Dim changes = New List(Of UserActivityItem)

        If oldPrevEmployer Is Nothing Then Return

        Dim suffixIdentifier = $"of previous employer with name '{oldPrevEmployer.Name}'."

        If _currentPrevEmployer.Name <> oldPrevEmployer.Name Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldPrevEmployer.RowID.Value,
                .Description = $"Updated name from '{oldPrevEmployer.Name}' to '{_currentPrevEmployer.Name}' {suffixIdentifier}",
                .ChangedEmployeeId = oldPrevEmployer.EmployeeID
            })
        End If
        If _currentPrevEmployer.TradeName <> oldPrevEmployer.TradeName Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldPrevEmployer.RowID.Value,
                .Description = $"Updated trade name from '{oldPrevEmployer.TradeName}' to '{_currentPrevEmployer.TradeName}' {suffixIdentifier}",
                .ChangedEmployeeId = oldPrevEmployer.EmployeeID
            })
        End If
        If _currentPrevEmployer.ContactName <> oldPrevEmployer.ContactName Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldPrevEmployer.RowID.Value,
                .Description = $"Updated contact name from '{oldPrevEmployer.ContactName}' to '{_currentPrevEmployer.ContactName}' {suffixIdentifier}",
                .ChangedEmployeeId = oldPrevEmployer.EmployeeID
            })
        End If
        If _currentPrevEmployer.MainPhone <> oldPrevEmployer.MainPhone Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldPrevEmployer.RowID.Value,
                .Description = $"Updated main phone from '{oldPrevEmployer.MainPhone}' to '{_currentPrevEmployer.MainPhone}' {suffixIdentifier}",
                .ChangedEmployeeId = oldPrevEmployer.EmployeeID
            })
        End If
        If _currentPrevEmployer.AltPhone <> oldPrevEmployer.AltPhone Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldPrevEmployer.RowID.Value,
                .Description = $"Updated alt phone from '{oldPrevEmployer.AltPhone}' to '{_currentPrevEmployer.AltPhone}' {suffixIdentifier}",
                .ChangedEmployeeId = oldPrevEmployer.EmployeeID
            })
        End If
        If _currentPrevEmployer.FaxNumber <> oldPrevEmployer.FaxNumber Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldPrevEmployer.RowID.Value,
                .Description = $"Updated fax number from '{oldPrevEmployer.FaxNumber}' to '{_currentPrevEmployer.FaxNumber}' {suffixIdentifier}",
                .ChangedEmployeeId = oldPrevEmployer.EmployeeID
            })
        End If
        If _currentPrevEmployer.EmailAddress <> oldPrevEmployer.EmailAddress Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldPrevEmployer.RowID.Value,
                .Description = $"Updated email address from '{oldPrevEmployer.EmailAddress}' to '{_currentPrevEmployer.EmailAddress}' {suffixIdentifier}",
                .ChangedEmployeeId = oldPrevEmployer.EmployeeID
            })
        End If
        If _currentPrevEmployer.AltEmailAddress <> oldPrevEmployer.AltEmailAddress Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldPrevEmployer.RowID.Value,
                .Description = $"Updated alt email address from '{oldPrevEmployer.AltEmailAddress}' to '{_currentPrevEmployer.AltEmailAddress}' {suffixIdentifier}",
                .ChangedEmployeeId = oldPrevEmployer.EmployeeID
            })
        End If
        If _currentPrevEmployer.URL <> oldPrevEmployer.URL Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldPrevEmployer.RowID.Value,
                .Description = $"Updated URL from '{oldPrevEmployer.URL}' to '{_currentPrevEmployer.URL}' {suffixIdentifier}",
                .ChangedEmployeeId = oldPrevEmployer.EmployeeID
            })
        End If
        If _currentPrevEmployer.TINNo <> oldPrevEmployer.TINNo Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldPrevEmployer.RowID.Value,
                .Description = $"Updated TIN number from '{oldPrevEmployer.TINNo}' to '{_currentPrevEmployer.TINNo}' {suffixIdentifier}",
                .ChangedEmployeeId = oldPrevEmployer.EmployeeID
            })
        End If
        If _currentPrevEmployer.JobTitle <> oldPrevEmployer.JobTitle Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldPrevEmployer.RowID.Value,
                .Description = $"Updated job title from '{oldPrevEmployer.JobTitle}' to '{_currentPrevEmployer.JobTitle}' {suffixIdentifier}",
                .ChangedEmployeeId = oldPrevEmployer.EmployeeID
            })
        End If
        If _currentPrevEmployer.JobFunction <> oldPrevEmployer.JobFunction Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldPrevEmployer.RowID.Value,
                .Description = $"Updated job function from '{oldPrevEmployer.JobFunction}' to '{_currentPrevEmployer.JobFunction}' {suffixIdentifier}",
                .ChangedEmployeeId = oldPrevEmployer.EmployeeID
            })
        End If
        If _currentPrevEmployer.OrganizationType <> oldPrevEmployer.OrganizationType Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldPrevEmployer.RowID.Value,
                .Description = $"Updated organization type from '{oldPrevEmployer.OrganizationType}' to '{_currentPrevEmployer.OrganizationType}' {suffixIdentifier}",
                .ChangedEmployeeId = oldPrevEmployer.EmployeeID
            })
        End If
        If _currentPrevEmployer.ExperienceFrom.ToShortDateString <> oldPrevEmployer.ExperienceFrom.ToShortDateString Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldPrevEmployer.RowID.Value,
                .Description = $"Updated experience start date from '{oldPrevEmployer.ExperienceFrom.ToShortDateString}' to '{_currentPrevEmployer.ExperienceFrom.ToShortDateString}' {suffixIdentifier}",
                .ChangedEmployeeId = oldPrevEmployer.EmployeeID
            })
        End If
        If _currentPrevEmployer.ExperienceTo.ToShortDateString <> oldPrevEmployer.ExperienceTo.ToShortDateString Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldPrevEmployer.RowID.Value,
                .Description = $"Updated experience end date from '{oldPrevEmployer.ExperienceTo.ToShortDateString}' to '{_currentPrevEmployer.ExperienceTo.ToShortDateString}' {suffixIdentifier}",
                .ChangedEmployeeId = oldPrevEmployer.EmployeeID
            })
        End If
        If _currentPrevEmployer.BusinessAddress <> oldPrevEmployer.BusinessAddress Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldPrevEmployer.RowID.Value,
                .Description = $"Updated company address from '{oldPrevEmployer.BusinessAddress}' to '{_currentPrevEmployer.BusinessAddress}' {suffixIdentifier}",
                .ChangedEmployeeId = oldPrevEmployer.EmployeeID
            })
        End If

        If changes.Any() Then

            _userActivityRepo.CreateRecord(z_User, FormEntityName, z_OrganizationID, UserActivityRepository.RecordTypeEdit, changes)
        End If

    End Sub

    Private Sub btnUserActivity_Click(sender As Object, e As EventArgs) Handles btnUserActivity.Click
        Dim userActivity As New UserActivityForm(FormEntityName)
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
