Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Enums
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class NewListOfValDisciplinaryPenaltyForm

    Private _actions As IEnumerable(Of ListOfValue)

    Private _currentAction As ListOfValue

    Private _mode As FormMode = FormMode.Empty

    Private ReadOnly _listOfValRepo As ListOfValueRepository

    Public Sub New()

        InitializeComponent()

        dgvActions.AutoGenerateColumns = False

        _listOfValRepo = MainServiceProvider.GetRequiredService(Of ListOfValueRepository)

    End Sub

    Private Async Sub NewListOfValDisciplinaryPenalty_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await LoadActions()
    End Sub

    Private Async Function LoadActions() As Task

        RemoveHandler dgvActions.SelectionChanged, AddressOf DgvActions_SelectionChanged

        _actions = Await _listOfValRepo.GetEmployeeDisciplinaryPenalties()
        dgvActions.DataSource = _actions

        If _actions.Count > 0 Then
            SelectAction(DirectCast(dgvActions.CurrentRow?.DataBoundItem, ListOfValue))
            ChangeMode(FormMode.Editing)
        Else
            SelectAction(Nothing)
            _currentAction = New ListOfValue
            ChangeMode(FormMode.Empty)
        End If

        AddHandler dgvActions.SelectionChanged, AddressOf DgvActions_SelectionChanged
    End Function

    Private Sub DgvActions_SelectionChanged(sender As Object, e As EventArgs) Handles dgvActions.SelectionChanged
        If _actions.Count > 0 Then
            SelectAction(DirectCast(dgvActions.CurrentRow?.DataBoundItem, ListOfValue))
        End If
    End Sub

    Private Sub SelectAction(action As ListOfValue)
        If action IsNot Nothing Then
            _currentAction = action

            With _currentAction
                txtName.Text = .DisplayValue
                txtDescription.Text = .Description
            End With
        Else
            ClearForm()
        End If
    End Sub

    Private Sub ClearForm()
        txtName.Text = ""
        txtDescription.Text = ""
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

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        If _currentAction?.RowID Is Nothing Then

            MessageBoxHelper.Warning("Please select an action.")
            Return
        End If

        Dim result = MsgBox("Are you sure you want to delete this Action?", MsgBoxStyle.YesNo, "Delete Action")

        If result = MsgBoxResult.Yes Then
            Await FunctionUtils.TryCatchFunctionAsync("Delete Disciplinary Action Penalty",
                Async Function()

                    Dim dataService = MainServiceProvider.GetRequiredService(Of ListOfValueDataService)
                    Await dataService.DeleteAsync(_currentAction.RowID.Value, z_User)

                    Await LoadActions()
                End Function)

        End If
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Await SaveActionPenalty() Then
            Await LoadActions()
        End If
    End Sub

    Private Async Function SaveActionPenalty() As Task(Of Boolean)
        Dim messageTitle = ""
        Dim succeed As Boolean = False

        If String.IsNullOrWhiteSpace(txtName.Text) Then
            ShowBalloonInfo("Action Name is empty.", "Invalid input.")
            Return False
        End If

        Await FunctionUtils.TryCatchFunctionAsync("Save Disciplinary Action Penaty",
            Async Function()
                If isChanged() Then

                    With _currentAction
                        .DisplayValue = txtName.Text
                        .LIC = txtName.Text
                        .Description = txtDescription.Text

                        If Not .RowID.HasValue Then

                            .Active = "Yes"
                            .Type = "Employee Disciplinary Penalty"
                            .OrderBy = GetMaxOrderBy()

                            messageTitle = "Save Disciplinary Action Penalty"
                        Else

                            messageTitle = "Update Disciplinary Action Penalty"
                        End If
                    End With

                    Dim dataService = MainServiceProvider.GetRequiredService(Of ListOfValueDataService)
                    Await dataService.SaveAsync(_currentAction, z_User)

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

    Private Function GetMaxOrderBy() As Integer
        Dim maxOrderBy As Integer = 0
        If _actions.Count > 0 Then
            For Each item In _actions
                If item.OrderBy.Value > maxOrderBy Then maxOrderBy = item.OrderBy.Value
            Next
        End If
        Return maxOrderBy + 1
    End Function

    Private Function isChanged() As Boolean
        With _currentAction
            If .DisplayValue <> txtName.Text OrElse
                    .Description <> txtDescription.Text Then
                Return True
            End If
        End With
        Return False
    End Function

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, lblActionName, 150, -115)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        RemoveHandler dgvActions.SelectionChanged, AddressOf DgvActions_SelectionChanged    'prevents from adding too many handlers
        AddHandler dgvActions.SelectionChanged, AddressOf DgvActions_SelectionChanged

        If _actions.Count > 0 Then
            ChangeMode(FormMode.Editing)
            SelectAction(DirectCast(dgvActions.CurrentRow?.DataBoundItem, ListOfValue))
        Else
            ChangeMode(FormMode.Empty)
            SelectAction(Nothing)
            Return
        End If
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        ChangeMode(FormMode.Creating)

        dgvActions.ClearSelection()
        RemoveHandler dgvActions.SelectionChanged, AddressOf DgvActions_SelectionChanged

        SelectAction(Nothing)

        _currentAction = New ListOfValue
    End Sub

End Class
