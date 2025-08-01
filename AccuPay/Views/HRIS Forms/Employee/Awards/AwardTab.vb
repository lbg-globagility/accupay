Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports AccuPay.Desktop.Enums
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class AwardTab

    Private _employee As Employee

    Private _awards As IEnumerable(Of Award)

    Private _currentAward As Award

    Private _mode As FormMode = FormMode.Empty

    Public Sub New()

        InitializeComponent()

        dgvAwards.AutoGenerateColumns = False

    End Sub

    Public Async Function SetEmployee(employee As Employee) As Task
        pbEmployee.Focus()
        _employee = employee

        txtFullname.Text = employee.FullNameWithMiddleInitial
        txtEmployeeID.Text = employee.EmployeeIdWithPositionAndEmployeeType
        pbEmployee.Image = ConvByteToImage(employee.Image)

        Await LoadAwards()
    End Function

    Private Async Function LoadAwards() As Task
        If _employee?.RowID Is Nothing Then Return

        Dim awardRepo = MainServiceProvider.GetRequiredService(Of IAwardRepository)
        _awards = Await awardRepo.GetByEmployeeAsync(_employee.RowID.Value)

        RemoveHandler dgvAwards.SelectionChanged, AddressOf dgvAwards_SelectionChanged
        dgvAwards.DataSource = _awards

        If _awards.Count > 0 Then
            SelectAward(DirectCast(dgvAwards.CurrentRow?.DataBoundItem, Award))
            ChangeMode(FormMode.Editing)
            FormToolsControl(True)
        Else
            SelectAward(Nothing)
            _currentAward = New Award
            ChangeMode(FormMode.Empty)
            FormToolsControl(False)
        End If

        AddHandler dgvAwards.SelectionChanged, AddressOf dgvAwards_SelectionChanged
    End Function

    Private Sub FormToolsControl(control As Boolean)
        txtAwardType.Enabled = control
        txtDescription.Enabled = control
        dtpAwardDate.Enabled = control
    End Sub

    Private Sub SelectAward(award As Award)
        If award IsNot Nothing Then
            _currentAward = award

            txtAwardType.Text = _currentAward.AwardType
            txtDescription.Text = _currentAward.AwardDescription
            dtpAwardDate.Value = _currentAward.AwardDate
        Else
            ClearForm()
        End If
    End Sub

    Private Sub ClearForm()
        txtAwardType.Text = ""
        txtDescription.Text = ""
        dtpAwardDate.Value = Today
    End Sub

    Private Sub dgvAwards_SelectionChanged(sender As Object, e As EventArgs) Handles dgvAwards.SelectionChanged
        If _awards.Count > 0 Then
            Dim award = DirectCast(dgvAwards.CurrentRow?.DataBoundItem, Award)
            SelectAward(award)

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
            SelectAward(Nothing)
            EnableBonusGrid()
        ElseIf _mode = FormMode.Editing Then
            Await LoadAwards()
        End If

        If _currentAward Is Nothing Then
            ChangeMode(FormMode.Empty)
        Else
            ChangeMode(FormMode.Editing)
        End If
    End Sub

    Private Sub EnableBonusGrid()
        AddHandler dgvAwards.SelectionChanged, AddressOf dgvAwards_SelectionChanged

        If dgvAwards.Rows.Count > 0 Then
            dgvAwards.Item(1, 0).Selected = True
            SelectAward(DirectCast(dgvAwards.CurrentRow.DataBoundItem, Award))
        End If
    End Sub

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        If _currentAward?.RowID Is Nothing Then

            MessageBoxHelper.Warning("No selected award!")
            Return
        End If

        Dim result = MsgBox("Are you sure you want to delete this Award?", MsgBoxStyle.YesNo, "Delete Award")

        If result = MsgBoxResult.Yes Then
            Await FunctionUtils.TryCatchFunctionAsync("Delete Award",
                Async Function()

                    Dim dataService = MainServiceProvider.GetRequiredService(Of IAwardDataService)
                    Await dataService.DeleteAsync(_currentAward.RowID.Value, z_User)

                    Await LoadAwards()
                End Function)

        End If
    End Sub

    Private Async Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        If _employee Is Nothing Then
            MessageBoxHelper.ErrorMessage("Please select an employee first.")
            Return
        End If

        Dim form As New AddAwardForm(_employee)
        form.ShowDialog()

        If form.IsSaved Then
            Await LoadAwards()

            If form.ShowBalloon Then
                ShowBalloonInfo("Award successfuly added.", "Saved")
            End If

        End If
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, pbEmployee, 47, -103)
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        pbEmployee.Focus()
        If Await SaveAward() Then
            Await LoadAwards()
        End If
    End Sub

    Private Async Function SaveAward() As Task(Of Boolean)
        Dim messageTitle = ""
        Dim succeed As Boolean = False

        If String.IsNullOrWhiteSpace(txtAwardType.Text) Then
            messageTitle = "Invalid Input"
            ShowBalloonInfo("Award Type empty.", messageTitle)
            Return False
        End If

        Await FunctionUtils.TryCatchFunctionAsync("Save Award",
            Async Function()
                If IsChanged() Then

                    With _currentAward
                        .AwardType = txtAwardType.Text
                        .AwardDescription = txtDescription.Text
                        .AwardDate = dtpAwardDate.Value
                    End With

                    Dim dataService = MainServiceProvider.GetRequiredService(Of IAwardDataService)
                    Await dataService.SaveAsync(_currentAward, z_User)

                    messageTitle = "Update Award"
                    succeed = True
                Else
                    MessageBoxHelper.Warning("No value changed")
                End If
            End Function)

        If succeed Then
            ShowBalloonInfo("Award successfuly saved.", messageTitle)
            Return True
        End If
        Return False
    End Function

    Private Function IsChanged() As Boolean
        If _currentAward.AwardType <> txtAwardType.Text OrElse
                _currentAward.AwardDescription <> txtDescription.Text OrElse
                _currentAward.AwardDate <> dtpAwardDate.Value Then
            Return True
        End If
        Return False
    End Function

    Private Sub btnUserActivity_Click(sender As Object, e As EventArgs) Handles btnUserActivity.Click

        Dim formEntityName As String = "Award"

        Dim userActivity As New UserActivityForm(formEntityName)
        userActivity.ShowDialog()
    End Sub

End Class
