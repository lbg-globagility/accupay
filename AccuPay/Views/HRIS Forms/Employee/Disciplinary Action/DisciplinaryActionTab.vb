Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Enums
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class DisciplinaryActionTab

    Private _employee As Employee

    Private _disciplinaryActions As IEnumerable(Of DisciplinaryAction)

    Private _currentDiscAction As DisciplinaryAction

    Private _findingNames As IEnumerable(Of Product)

    Private _actions As IEnumerable(Of ListOfValue)

    Private _currentFindingName As Product

    Private _mode As FormMode = FormMode.Empty

    Private ReadOnly _disciplinaryActionRepo As IDisciplinaryActionRepository

    Private ReadOnly _listOfValRepo As IListOfValueRepository

    Private ReadOnly _productRepo As IProductRepository

    Public Sub New()

        InitializeComponent()

        dgvDisciplinaryList.AutoGenerateColumns = False

        If MainServiceProvider IsNot Nothing Then

            _disciplinaryActionRepo = MainServiceProvider.GetRequiredService(Of IDisciplinaryActionRepository)
            _listOfValRepo = MainServiceProvider.GetRequiredService(Of IListOfValueRepository)
            _productRepo = MainServiceProvider.GetRequiredService(Of IProductRepository)
        End If

    End Sub

    Public Async Function SetEmployee(employee As Employee) As Task

        pbEmployee.Focus()

        _employee = employee

        txtFullname.Text = employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = employee.EmployeeIdWithPositionAndEmployeeType
        pbEmployee.Image = ConvByteToImage(employee.Image)

        Await LoadDisciplinaryActions()
    End Function

    Private Async Function LoadDisciplinaryActions() As Task
        If _employee?.RowID Is Nothing Then Return

        _disciplinaryActions = Await _disciplinaryActionRepo.GetByEmployeeAsync(_employee.RowID.Value)
        _disciplinaryActions = _disciplinaryActions.OrderByDescending(Function(x) x.DateFrom).ToList()

        _findingNames = Await _productRepo.GetDisciplinaryTypesAsync(z_OrganizationID)

        _actions = Await _listOfValRepo.GetEmployeeDisciplinaryPenalties()

        RemoveHandler dgvDisciplinaryList.SelectionChanged, AddressOf dgvDisciplinaryList_SelectionChanged

        dgvDisciplinaryList.DataSource = _disciplinaryActions
        cboFinding.DataSource = _findingNames
        cboAction.DataSource = _actions

        If _disciplinaryActions.Count > 0 Then
            SelectDisciplinaryAction(DirectCast(dgvDisciplinaryList.CurrentRow?.DataBoundItem, DisciplinaryAction))
            ChangeMode(FormMode.Editing)
            FormToolsControl(True)
        Else
            SelectDisciplinaryAction(Nothing)
            _currentDiscAction = New DisciplinaryAction
            ChangeMode(FormMode.Empty)
            FormToolsControl(False)
        End If

        AddHandler dgvDisciplinaryList.SelectionChanged, AddressOf dgvDisciplinaryList_SelectionChanged
    End Function

    Private Sub FormToolsControl(control As Boolean)
        cboFinding.Enabled = control
        cboAction.Enabled = control
        dtpEffectiveFrom.Enabled = control
        dtpEffectiveTo.Enabled = control
        txtDescription.Enabled = control
        txtComments.Enabled = control
    End Sub

    Private Sub SelectDisciplinaryAction(disciplinaryAction As DisciplinaryAction)
        If disciplinaryAction IsNot Nothing Then
            _currentDiscAction = disciplinaryAction
            With _currentDiscAction
                cboFinding.Text = .FindingName
                cboAction.Text = .Action
                dtpEffectiveFrom.Value = .DateFrom
                dtpEffectiveTo.Value = .DateTo
                txtDescription.Text = .FindingDescription
                txtComments.Text = .Comments

            End With
        Else
            ClearForm()
        End If
    End Sub

    Private Sub ClearForm()
        cboFinding.SelectedItem = Nothing
        cboAction.SelectedItem = Nothing
        dtpEffectiveFrom.Value = Today
        dtpEffectiveTo.Value = Today
        txtDescription.Text = ""
        txtComments.Text = ""
    End Sub

    Private Sub dgvDisciplinaryList_SelectionChanged(sender As Object, e As EventArgs) Handles dgvDisciplinaryList.SelectionChanged
        If _disciplinaryActions.Count > 0 Then
            Dim discAction = DirectCast(dgvDisciplinaryList.CurrentRow?.DataBoundItem, DisciplinaryAction)
            SelectDisciplinaryAction(discAction)

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
            SelectDisciplinaryAction(Nothing)
        ElseIf _mode = FormMode.Editing Then
            Await LoadDisciplinaryActions()
        End If

        If _currentDiscAction Is Nothing Then
            ChangeMode(FormMode.Empty)
        Else
            ChangeMode(FormMode.Editing)
        End If
    End Sub

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        If _currentDiscAction?.RowID Is Nothing Then

            MessageBoxHelper.Warning("No selected disciplinary action!")
            Return
        End If

        Dim result = MsgBox("Are you sure you want to delete this Action?", MsgBoxStyle.YesNo, "Delete Disciplinary Action")

        If result = MsgBoxResult.Yes Then
            Await FunctionUtils.TryCatchFunctionAsync("Delete Disciplinary Action",
                Async Function()

                    Dim dataService = MainServiceProvider.GetRequiredService(Of IDisciplinaryActionDataService)
                    Await dataService.DeleteAsync(_currentDiscAction.RowID.Value, z_User)

                    Await LoadDisciplinaryActions()
                End Function)

        End If
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        pbEmployee.Focus()
        If Await SaveDisciplinaryAction() Then
            Await LoadDisciplinaryActions()
        End If
    End Sub

    Private Async Function SaveDisciplinaryAction() As Task(Of Boolean)
        Dim messageTitle = ""
        Dim succeed As Boolean = False

        If cboFinding.SelectedItem Is Nothing Then
            ShowBalloonInfo("Finding Name is empty.", "Invalid input.")
            Return False
        ElseIf cboAction.SelectedItem Is Nothing Then
            ShowBalloonInfo("Action is empty.", "Invalid input.")
            Return False
        End If

        Await FunctionUtils.TryCatchFunctionAsync("Save Disciplinary Action",
            Async Function()
                If IsChanged() Then
                    Dim currentFinding As Product = CType(cboFinding.SelectedItem, Product)

                    With _currentDiscAction
                        .FindingID = currentFinding.RowID.Value
                        .Action = cboAction.Text
                        .DateFrom = dtpEffectiveFrom.Value
                        .DateTo = dtpEffectiveTo.Value
                        .FindingDescription = txtDescription.Text
                        .Comments = txtComments.Text
                    End With

                    Dim dataService = MainServiceProvider.GetRequiredService(Of IDisciplinaryActionDataService)
                    Await dataService.SaveAsync(_currentDiscAction, z_User)

                    messageTitle = "Update Disciplinary Action"
                    succeed = True
                Else
                    MessageBoxHelper.Warning("No value changed")
                End If
            End Function)

        If succeed Then
            ShowBalloonInfo("Disciplinary Action successfuly saved.", messageTitle)
            Return True
        End If
        Return False
    End Function

    Private Function IsChanged() As Boolean
        With _currentDiscAction
            If .FindingName <> cboFinding.Text OrElse
                    .Action <> cboAction.Text OrElse
                    .DateFrom <> dtpEffectiveFrom.Value OrElse
                    .DateTo <> dtpEffectiveTo.Value OrElse
                    .FindingDescription <> txtDescription.Text OrElse
                    .Comments <> txtComments.Text Then
                Return True
            End If
        End With
        Return False
    End Function

    Private Async Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        If _employee Is Nothing Then
            MessageBoxHelper.ErrorMessage("Please select an employee first.")
            Return
        End If

        Dim form As New AddDisciplinaryAction(_employee)
        form.ShowDialog()

        If form.IsSaved Then
            Await LoadDisciplinaryActions()

            If form.ShowBalloon Then
                ShowBalloonInfo("Disciplinary Action successfuly added.", "Saved")
            End If

        End If
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, pbEmployee, 47, -103)
    End Sub

    Private Sub btnUserActivity_Click(sender As Object, e As EventArgs) Handles btnUserActivity.Click

        Dim formEntityName As String = "Disciplinary Action"

        Dim userActivity As New UserActivityForm(formEntityName)
        userActivity.ShowDialog()
    End Sub

    Private Sub Dates_ValueChanged(sender As Object, e As EventArgs) Handles dtpEffectiveFrom.ValueChanged, dtpEffectiveTo.ValueChanged
        If dtpEffectiveTo.Value < dtpEffectiveFrom.Value Then
            dtpEffectiveTo.Value = dtpEffectiveFrom.Value
        End If
    End Sub

    Private Async Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked

        Dim form As New NewListOfValDisciplinaryPenaltyForm()

        With form
            .ShowDialog()
        End With
        Await LoadDisciplinaryActions()
    End Sub

    Private Async Sub lblAddFindingname_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblAddFindingname.LinkClicked

        Dim form As New NewProductDisciplinaryForm(_productRepo)

        With form
            .ShowDialog()
        End With
        Await LoadDisciplinaryActions()
    End Sub

End Class
