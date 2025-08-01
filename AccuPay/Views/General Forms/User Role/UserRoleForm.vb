Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class UserRoleForm

    Private ReadOnly _userRepository As IAspNetUserRepository

    Private ReadOnly _roleRepository As IRoleRepository

    Sub New()

        InitializeComponent()

        _userRepository = MainServiceProvider.GetRequiredService(Of IAspNetUserRepository)

        _roleRepository = MainServiceProvider.GetRequiredService(Of IRoleRepository)

    End Sub

    Private Async Sub UserRoleForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        RoleGrid.AutoGenerateColumns = False

        Await CheckRolePermissions()

        Await PopulateRoleGrid()

        Await RoleUserControl.SetRole(GetSelectedRole())

        AddHandler RoleGrid.SelectionChanged, AddressOf RoleGridSelectionChanged

    End Sub

    Private Async Function CheckRolePermissions() As Task
        Dim role = Await PermissionHelper.GetRoleAsync(PermissionConstant.ROLE)

        NewToolStripButton.Visible = False
        SaveToolStripButton.Visible = False
        CancelToolStripButton.Visible = False
        DeleteToolStripButton.Visible = False

        If role.Success Then

            If role.RolePermission.Create Then
                NewToolStripButton.Visible = True

            End If

            If role.RolePermission.Update Then
                SaveToolStripButton.Visible = True
                CancelToolStripButton.Visible = True
            End If

            If role.RolePermission.Delete Then
                DeleteToolStripButton.Visible = True

            End If

        End If
    End Function

    Private Function GetSelectedRole() As AspNetRole
        If RoleGrid.RowCount = 0 Then Return Nothing

        Return CType(RoleGrid.CurrentRow.DataBoundItem, RoleViewModel)?.Role

    End Function

    Private Async Function PopulateRoleGrid() As Task

        Dim user = Await _userRepository.GetByIdAsync(z_User)

        Dim userRole = Await _roleRepository.GetByUserAndOrganizationAsync(user.Id, z_OrganizationID)

        Dim list = Await _roleRepository.List(PageOptions.AllData, Z_Client)

        Dim roles = RoleViewModel.TransformList(list.roles.ToList(), userRole.Id)

        RoleGrid.DataSource = roles.OrderBy(Function(r) r.DisplayName).ToList()

    End Function

    Private Async Sub RoleGridSelectionChanged(sender As Object, e As EventArgs)
        Await RoleUserControl.SetRole(GetSelectedRole())
    End Sub

    Private Async Sub NewToolStripButton_Click(sender As Object, e As EventArgs) Handles NewToolStripButton.Click

        Dim dialog As New NewRoleForm()
        dialog.ShowDialog()

        If dialog.IsSaved AndAlso dialog.NewRole IsNot Nothing Then

            Await RefreshRoleGrid(dialog.NewRole)

            InfoBalloon("New role has been successfully created.", "Role Created", LabelForBalloon, 0, -69)

        End If

    End Sub

    Private Async Sub SaveToolStripButtonClicked(sender As Object, e As EventArgs) Handles SaveToolStripButton.Click

        LabelForBalloon.Focus()

        If RoleUserControl.HasChanged = False Then

            MessageBoxHelper.Warning("No unsaved changes!")
            Return

        End If

        Dim originalName = GetSelectedRole().Name.Trim()
        Dim currentRole = RoleUserControl.GetUpdatedRole()

        Await FunctionUtils.TryCatchFunctionAsync("Save Role",
            Async Function()
                Dim roleRepository = MainServiceProvider.GetRequiredService(Of IRoleDataService)
                Await roleRepository.UpdateAsync(currentRole)

                Await RefreshRoleGrid(currentRole)

                USER_ROLE = Await _roleRepository.GetByUserAndOrganizationAsync(userId:=z_User, organizationId:=z_OrganizationID)

                InfoBalloon("Role has been successfully saved.", "Role Saved", LabelForBalloon, 0, -69)
            End Function,
            errorCallBack:=
            Sub()
                currentRole.Name = originalName
            End Sub)
    End Sub

    Private Async Sub DeleteToolStripButton_Click(sender As Object, e As EventArgs) Handles DeleteToolStripButton.Click

        Dim currentRole = GetSelectedRole()

        If currentRole Is Nothing Then

            MessageBoxHelper.Warning("No selected role!")
            Return

        End If

        If MessageBoxHelper.Confirm(Of Boolean) _
        ($"Are you sure you want to delete the role `{currentRole.Name}`?", "Confirm Deletion") = False Then

            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync("Delete Role",
            Async Function()
                Dim roleRepository = MainServiceProvider.GetRequiredService(Of IRoleDataService)
                Await roleRepository.DeleteAsync(currentRole.Id)

                Await RefreshRoleGrid(currentRole)

                USER_ROLE = Await _roleRepository.GetByUserAndOrganizationAsync(userId:=z_User, organizationId:=z_OrganizationID)

                InfoBalloon("Role has been successfully deleted.", "Role Deleted", LabelForBalloon, 0, -69)
            End Function)

    End Sub

    Private Async Sub CancelToolStripButton_Click(sender As Object, e As EventArgs) Handles CancelToolStripButton.Click
        Await RoleUserControl.SetRole(GetSelectedRole())
    End Sub

    Private Sub CloseToolStripButton_Click(sender As Object, e As EventArgs) Handles CloseToolStripButton.Click
        Me.Close()
    End Sub

    Private Async Function RefreshRoleGrid(currentRole As AspNetRole) As Task

        RemoveHandler RoleGrid.SelectionChanged, AddressOf RoleGridSelectionChanged

        Await PopulateRoleGrid()
        Dim roles = CType(RoleGrid.DataSource, List(Of RoleViewModel))

        If roles.Any Then

            Dim currentRoleModel = roles.FirstOrDefault(Function(r) r.Role.Id = currentRole.Id)

            If currentRoleModel Is Nothing Then
                currentRole = CType(RoleGrid.CurrentRow.DataBoundItem, RoleViewModel)?.Role
            End If

            If currentRoleModel IsNot Nothing Then

                Dim currentRoleIndex = roles.IndexOf(currentRoleModel)

                RoleGrid.CurrentCell = RoleGrid.Rows(currentRoleIndex).Cells(0)

            End If

        End If

        Await RoleUserControl.SetRole(currentRole)

        AddHandler RoleGrid.SelectionChanged, AddressOf RoleGridSelectionChanged

    End Function

    Private Sub UserRoleForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        InfoBalloon(, , LabelForBalloon, , , 1)

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        GeneralForm.listGeneralForm.Remove(Me.Name)
    End Sub

    Private Sub dgvpositview_DataError(sender As Object, e As DataGridViewDataErrorEventArgs)
        e.ThrowException = False

    End Sub

    Public Class RoleViewModel

        Private ReadOnly _currentRoleId As Integer

        Private _displayName As String

        Public ReadOnly Property DisplayName As String
            Get
                Return _displayName
            End Get
        End Property

        Public Property Role As AspNetRole

        Sub New(role As AspNetRole, currentRoleId As Integer)

            Me.Role = role
            Me._currentRoleId = currentRoleId
            UpdateDisplayName(Me.Role.Name)

        End Sub

        Public Sub UpdateDisplayName(name As String)

            _displayName = name

            If Me.Role.Id = _currentRoleId Then
                _displayName &= " (your position)"
            End If
        End Sub

        Public Shared Function TransformList(list As List(Of AspNetRole), currentRoleId As Integer) As List(Of RoleViewModel)

            Dim models As New List(Of RoleViewModel)

            list.ForEach(
                Sub(role)

                    models.Add(New RoleViewModel(role, currentRoleId))

                End Sub)

            Return models
        End Function

    End Class

End Class
