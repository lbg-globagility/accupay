Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class UserPrivilegeForm

    Private ReadOnly _aspNetUserRepository As AspNetUserRepository

    Private ReadOnly _roleRepository As RoleRepository

    Private ReadOnly _permissionRepository As PermissionRepository

    Private _permissions As List(Of Permission)

    Sub New()

        InitializeComponent()

        _aspNetUserRepository = MainServiceProvider.GetRequiredService(Of AspNetUserRepository)

        _permissionRepository = MainServiceProvider.GetRequiredService(Of PermissionRepository)

        _roleRepository = MainServiceProvider.GetRequiredService(Of RoleRepository)

        _permissions = New List(Of Permission)

    End Sub

    Private Async Sub userprivil_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        RoleGrid.AutoGenerateColumns = False
        RolePermissionGrid.AutoGenerateColumns = False

        If Await PermissionHelper.AllowUpdate(PermissionConstant.ROLE) Then
            SaveButton.Visible = True
            CancelButton.Visible = True
        Else
            SaveButton.Visible = False
            CancelButton.Visible = False

            SetFormToReadOnly()

        End If

        Await GetPermissions()

        Await PopulateRoleGrid()

        Await UpdateRolePermissionGrid()

        AddHandler RoleGrid.SelectionChanged, AddressOf RoleGridSelectionChanged

    End Sub

    Private Sub SetFormToReadOnly()

        RolePermissionGrid.ReadOnly = True
        RoleNameTextBox.Enabled = False
        AllReadCheckBox.Enabled = False
        AllCreateCheckBox.Enabled = False
        AllUpdateCheckBox.Enabled = False
        AllDeleteCheckBox.Enabled = False

    End Sub

    Private Async Function UpdateRolePermissionGrid() As Task

        Dim rolePermissionModels = RolePermissionViewModel.TransformList(_permissions)

        Dim selectedRole = GetSelectedRole()

        If selectedRole IsNot Nothing Then

            RoleNameTextBox.Text = selectedRole.Name

            Dim role = Await _roleRepository.GetById(selectedRole.Id)

            rolePermissionModels.ForEach(
                Sub(model)

                    Dim rolePermission = role.RolePermissions.FirstOrDefault(Function(p) p.PermissionId = model.Permission.Id)
                    model.UpdateRolePermission(rolePermission, role)

                End Sub)

        End If

        UpdateBatchCheckBox(rolePermissionModels)

        RolePermissionGrid.DataSource = rolePermissionModels.OrderBy(Function(m) m.Permission.Name).ToList()

    End Function

    Private Sub UpdateBatchCheckBox(rolePermissionModels As List(Of RolePermissionViewModel))
        RemoveHandler AllReadCheckBox.CheckedChanged, AddressOf AllReadCheckBox_CheckedChanged
        RemoveHandler AllCreateCheckBox.CheckedChanged, AddressOf AllCreateCheckBox_CheckedChanged
        RemoveHandler AllUpdateCheckBox.CheckedChanged, AddressOf AllUpdateCheckBox_CheckedChanged
        RemoveHandler AllDeleteCheckBox.CheckedChanged, AddressOf AllDeleteCheckBox_CheckedChanged

        AllReadCheckBox.Checked = rolePermissionModels.Where(Function(r) r.Read).Count = _permissions.Count
        AllCreateCheckBox.Checked = rolePermissionModels.Where(Function(r) r.Create).Count = _permissions.Count
        AllUpdateCheckBox.Checked = rolePermissionModels.Where(Function(r) r.Update).Count = _permissions.Count
        AllDeleteCheckBox.Checked = rolePermissionModels.Where(Function(r) r.Delete).Count = _permissions.Count

        AddHandler AllReadCheckBox.CheckedChanged, AddressOf AllReadCheckBox_CheckedChanged
        AddHandler AllCreateCheckBox.CheckedChanged, AddressOf AllCreateCheckBox_CheckedChanged
        AddHandler AllUpdateCheckBox.CheckedChanged, AddressOf AllUpdateCheckBox_CheckedChanged
        AddHandler AllDeleteCheckBox.CheckedChanged, AddressOf AllDeleteCheckBox_CheckedChanged
    End Sub

    Private Function GetSelectedRole() As AspNetRole
        If RoleGrid.RowCount = 0 Then Return Nothing

        Return CType(RoleGrid.CurrentRow.DataBoundItem, RoleViewModel)?.Role

    End Function

    Private Async Function GetPermissions() As Task
        _permissions = (Await _permissionRepository.GetAll()).ToList()
    End Function

    Private Async Function PopulateRoleGrid() As Task

        Dim user = Await _aspNetUserRepository.GetById(z_User)

        Dim userRole = Await _roleRepository.GetByUserAndOrganization(user.Id, z_OrganizationID)

        Dim list = Await _roleRepository.List(PageOptions.AllData, Z_Client)

        Dim roles = RoleViewModel.TransformList(list.roles.ToList(), userRole.Id)

        RoleGrid.DataSource = roles.OrderBy(Function(r) r.DisplayName).ToList()

    End Function

    Private Async Sub RoleGridSelectionChanged(sender As Object, e As EventArgs)
        Await UpdateRolePermissionGrid()
    End Sub

    Private Async Sub SaveButtonClicked(sender As Object, e As EventArgs) Handles SaveButton.Click

        RolePermissionGrid.EndEdit()

        lblforballoon.Focus()

        Dim rolePermissionModels = CType(RolePermissionGrid.DataSource, List(Of RolePermissionViewModel))

        Dim changedPermissions = rolePermissionModels.Where(Function(r) r.HasChanged).ToList()

        Dim currentRole = GetSelectedRole()
        Dim originalName = currentRole.Name

        If RoleGrid.RowCount = 0 OrElse currentRole Is Nothing OrElse (changedPermissions.Any() = False AndAlso originalName = RoleNameTextBox.Text.Trim()) Then
            MessageBoxHelper.Warning("No unsaved changes!")
            Return
        End If

        currentRole.Name = RoleNameTextBox.Text.Trim()
        currentRole.RolePermissions = rolePermissionModels.Select(Function(r) r.GetUpdatedRolePermission()).ToList()

        Await FunctionUtils.TryCatchFunctionAsync("Save Permissions",
            Async Function()
                Dim roleRepository = MainServiceProvider.GetRequiredService(Of RoleDataService)
                Await roleRepository.UpdateAsync(currentRole)

                Dim selectedRoleModel = CType(RoleGrid.CurrentRow.DataBoundItem, RoleViewModel)
                selectedRoleModel.UpdateDisplayName(currentRole.Name)
                RoleGrid.Refresh()

                changedPermissions.ForEach(
                    Sub(permission)
                        permission.CommitPermissions()
                    End Sub)

                USER_ROLES = Await _roleRepository.GetByUserAndOrganization(userId:=z_User, organizationId:=z_OrganizationID)

                InfoBalloon("User privilege has been successfully saved.", "Successfully save", lblforballoon, 0, -69)
            End Function,
            errorCallBack:=
            Sub()
                currentRole.Name = originalName
            End Sub)
    End Sub

    Private Async Sub tsbtnCancelUserPrivil_Click(sender As Object, e As EventArgs) Handles CancelButton.Click

        Await UpdateRolePermissionGrid()

    End Sub

    Private Sub tsbtnCloseUserPrivil_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Me.Close()
    End Sub

    Private Sub AllReadCheckBox_CheckedChanged(sender As Object, e As EventArgs)

        BatchUpdateRolePermissionGrid(checkAllRead:=AllReadCheckBox.Checked)
    End Sub

    Private Sub AllCreateCheckBox_CheckedChanged(sender As Object, e As EventArgs)

        BatchUpdateRolePermissionGrid(checkAllCreate:=AllCreateCheckBox.Checked)
    End Sub

    Private Sub AllUpdateCheckBox_CheckedChanged(sender As Object, e As EventArgs)

        BatchUpdateRolePermissionGrid(checkAllUpdate:=AllUpdateCheckBox.Checked)
    End Sub

    Private Sub AllDeleteCheckBox_CheckedChanged(sender As Object, e As EventArgs)

        BatchUpdateRolePermissionGrid(checkAllDelete:=AllDeleteCheckBox.Checked)
    End Sub

    Private Sub userprivil_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        InfoBalloon(, , lblforballoon, , , 1)

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        GeneralForm.listGeneralForm.Remove(Me.Name)
    End Sub

    Private Sub dgvpositview_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles RolePermissionGrid.DataError
        e.ThrowException = False

    End Sub

    Private Sub BatchUpdateRolePermissionGrid(
        Optional checkAllRead As Boolean? = Nothing,
        Optional checkAllCreate As Boolean? = Nothing,
        Optional checkAllUpdate As Boolean? = Nothing,
        Optional checkAllDelete As Boolean? = Nothing
        )

        RolePermissionGrid.EndEdit()
        Dim rolePermissions = CType(RolePermissionGrid.DataSource, List(Of RolePermissionViewModel))

        rolePermissions.ForEach(
            Sub(permission)
                If checkAllRead IsNot Nothing Then
                    permission.Read = checkAllRead.Value
                ElseIf checkAllCreate IsNot Nothing Then
                    permission.Create = checkAllCreate.Value
                ElseIf checkAllUpdate IsNot Nothing Then
                    permission.Update = checkAllUpdate.Value
                ElseIf checkAllDelete IsNot Nothing Then
                    permission.Delete = checkAllDelete.Value
                End If
            End Sub)

        RolePermissionGrid.DataSource = rolePermissions
        RolePermissionGrid.Refresh()
    End Sub

    Private Sub RolePermissionGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles RolePermissionGrid.CellContentClick

        If e.ColumnIndex < 0 OrElse e.RowIndex < 0 Then Return

        Dim rolePermissions = CType(RolePermissionGrid.DataSource, List(Of RolePermissionViewModel))

        Dim currentColumn = RolePermissionGrid.Columns(e.ColumnIndex)

        RolePermissionGrid.EndEdit()

        If currentColumn.Index = ReadColumn.Index Then

            RemoveHandler AllReadCheckBox.CheckedChanged, AddressOf AllReadCheckBox_CheckedChanged
            AllReadCheckBox.Checked = Not rolePermissions.Any(Function(r) Not r.Read)
            AddHandler AllReadCheckBox.CheckedChanged, AddressOf AllReadCheckBox_CheckedChanged

        ElseIf currentColumn.Index = CreateColumn.Index Then

            RemoveHandler AllCreateCheckBox.CheckedChanged, AddressOf AllCreateCheckBox_CheckedChanged
            AllCreateCheckBox.Checked = Not rolePermissions.Any(Function(r) Not r.Create)
            AddHandler AllCreateCheckBox.CheckedChanged, AddressOf AllCreateCheckBox_CheckedChanged

        ElseIf currentColumn.Index = UpdateColumn.Index Then

            RemoveHandler AllUpdateCheckBox.CheckedChanged, AddressOf AllUpdateCheckBox_CheckedChanged
            AllUpdateCheckBox.Checked = Not rolePermissions.Any(Function(r) Not r.Update)
            AddHandler AllUpdateCheckBox.CheckedChanged, AddressOf AllUpdateCheckBox_CheckedChanged

        ElseIf currentColumn.Index = DeleteColumn.Index Then

            RemoveHandler AllDeleteCheckBox.CheckedChanged, AddressOf AllDeleteCheckBox_CheckedChanged
            AllDeleteCheckBox.Checked = Not rolePermissions.Any(Function(r) Not r.Delete)
            AddHandler AllDeleteCheckBox.CheckedChanged, AddressOf AllDeleteCheckBox_CheckedChanged
        End If

    End Sub

    Public Class RolePermissionViewModel

        Public Property Permission As Permission

        Public Property RolePermission As RolePermission

        Public Property Read As Boolean

        Public Property Create As Boolean

        Public Property Update As Boolean

        Public Property Delete As Boolean

        Sub New(permission As Permission)
            Me.Permission = permission
        End Sub

        Public ReadOnly Property DisplayName As String
            Get
                Return Permission.Name
            End Get
        End Property

        Public ReadOnly Property HasChanged As Boolean
            Get
                Return RolePermission.Read <> Read OrElse
                    RolePermission.Create <> Create OrElse
                    RolePermission.Update <> Update OrElse
                    RolePermission.Delete <> Delete
            End Get
        End Property

        Public Shared Function TransformList(list As List(Of Permission)) As List(Of RolePermissionViewModel)

            Dim models As New List(Of RolePermissionViewModel)

            list.ForEach(
                Sub(permission)

                    models.Add(New RolePermissionViewModel(permission))

                End Sub)

            Return models
        End Function

        Public Sub UpdateRolePermission(rolePermission As RolePermission, role As AspNetRole)

            If rolePermission Is Nothing Then
                role.SetPermission(Permission)

                rolePermission = role.GetPermission(Permission.Name)
            End If

            Me.RolePermission = rolePermission
            Me.RolePermission.Role = Nothing
            Me.RolePermission.Permission = Nothing

            Me.Read = Me.RolePermission.Read
            Me.Create = Me.RolePermission.Create
            Me.Update = Me.RolePermission.Update
            Me.Delete = Me.RolePermission.Delete
        End Sub

        Public Sub CommitPermissions()
            Me.RolePermission.Read = Me.Read
            Me.RolePermission.Create = Me.Create
            Me.RolePermission.Update = Me.Update
            Me.RolePermission.Delete = Me.Delete
        End Sub

        Public Function GetUpdatedRolePermission() As RolePermission
            Dim updateRolePermission = Me.RolePermission.CloneJson()

            updateRolePermission.Read = Me.Read
            updateRolePermission.Create = Me.Create
            updateRolePermission.Update = Me.Update
            updateRolePermission.Delete = Me.Delete

            Return updateRolePermission
        End Function

    End Class

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