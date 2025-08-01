Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Enums
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class RoleUserControl

    Private _currentRole As AspNetRole

    Private _formMode As FormMode

    Private ReadOnly _policy As IPolicyHelper

    Private ReadOnly _permissionRepository As IPermissionRepository

    Private _permissions As List(Of Permission)

    Sub New()

        InitializeComponent()

        'For issues in designer And also defensive programming
        If MainServiceProvider IsNot Nothing Then

            _policy = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

            _permissionRepository = MainServiceProvider.GetRequiredService(Of IPermissionRepository)
        End If

        _permissions = New List(Of Permission)

    End Sub

    Private Async Sub RoleUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        RolePermissionGrid.AutoGenerateColumns = False

        Await GetPermissions()

    End Sub

    Public Async Function SetRole(role As AspNetRole, Optional formMode As FormMode = FormMode.Editing) As Task
        _currentRole = role

        _formMode = formMode

        If (_formMode = FormMode.Creating AndAlso (Await PermissionHelper.DoesAllowCreateAsync(PermissionConstant.ROLE)) = False) OrElse
           (_formMode = FormMode.Editing AndAlso (Await PermissionHelper.DoesAllowUpdateAsync(PermissionConstant.ROLE)) = False) Then

            SetFormToReadOnly()
        End If

        Await UpdateRolePermissionGrid()
    End Function

    Public Function HasChanged() As Boolean

        RolePermissionGrid.EndEdit()

        Dim rolePermissionModels = CType(RolePermissionGrid.DataSource, List(Of RolePermissionViewModel))

        Dim changedPermissions = rolePermissionModels.Where(Function(r) r.HasChanged).ToList()

        Return _currentRole IsNot Nothing AndAlso (changedPermissions.Any() OrElse _currentRole.Name.Trim() <> RoleNameTextBox.Text.Trim())
    End Function

    Public Function GetUpdatedRole() As AspNetRole

        RolePermissionGrid.EndEdit()
        Dim rolePermissionModels = CType(RolePermissionGrid.DataSource, List(Of RolePermissionViewModel))

        _currentRole.Name = RoleNameTextBox.Text.Trim()
        _currentRole.RolePermissions = rolePermissionModels.
            Select(Function(r) r.GetUpdatedRolePermission(_currentRole)).
            Where(Function(r) r IsNot Nothing).
            ToList()

        Return _currentRole
    End Function

    Public Sub CommitPermissionChanges()
        Dim rolePermissionModels = CType(RolePermissionGrid.DataSource, List(Of RolePermissionViewModel))

        Dim changedPermissions = rolePermissionModels.Where(Function(r) r.HasChanged).ToList()

        changedPermissions.ForEach(
            Sub(permission)
                permission.CommitPermissions()
            End Sub)
    End Sub

    Private Sub SetFormToReadOnly()

        RolePermissionGrid.ReadOnly = True
        RoleNameTextBox.Enabled = False
        AllReadCheckBox.Enabled = False
        AllCreateCheckBox.Enabled = False
        AllUpdateCheckBox.Enabled = False
        AllDeleteCheckBox.Enabled = False

    End Sub

    Private Async Function GetPermissions() As Task

        If _permissions.Any() Then Return

        Dim permissions = (Await _permissionRepository.GetAll(forDesktopOnly:=True)).ToList()

        If Not _policy.UseAgency Then
            Dim agencyPermission = permissions.
                Where(Function(p) p.Name = PermissionConstant.AGENCY).
                FirstOrDefault()

            If agencyPermission IsNot Nothing Then

                permissions.Remove(agencyPermission)

            End If
        End If

        _permissions = permissions

    End Function

    Private Async Function UpdateRolePermissionGrid() As Task

        If _permissions.Any() = False Then
            Await GetPermissions()
        End If

        Dim rolePermissionModels = RolePermissionViewModel.TransformList(_permissions)

        If _currentRole IsNot Nothing Then

            RoleNameTextBox.Text = _currentRole.Name

            'This does not update when roleRepository is used as instance field
            Dim roleRepository = MainServiceProvider.GetRequiredService(Of IRoleRepository)
            Dim role = Await roleRepository.GetByIdAsync(_currentRole.Id)

            If role IsNot Nothing Then

                rolePermissionModels.ForEach(
                    Sub(model)

                        Dim rolePermission = role.RolePermissions.FirstOrDefault(Function(p) p.PermissionId = model.Permission.Id)
                        model.UpdateRolePermission(rolePermission, role)

                    End Sub)

            End If

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
                rolePermission = CreateRolePermission(role)
            End If

            Me.RolePermission = rolePermission
            Me.RolePermission.Role = Nothing
            Me.RolePermission.Permission = Nothing

            Me.Read = Me.RolePermission.Read
            Me.Create = Me.RolePermission.Create
            Me.Update = Me.RolePermission.Update
            Me.Delete = Me.RolePermission.Delete
        End Sub

        Private Function CreateRolePermission(role As AspNetRole) As RolePermission
            role.SetPermission(Permission)

            Return role.GetPermission(Permission.Name)
        End Function

        Public Sub CommitPermissions()
            Me.RolePermission.Read = Me.Read
            Me.RolePermission.Create = Me.Create
            Me.RolePermission.Update = Me.Update
            Me.RolePermission.Delete = Me.Delete
        End Sub

        Public Function GetUpdatedRolePermission(role As AspNetRole) As RolePermission
            Dim updateRolePermission = Me.RolePermission.CloneJson()

            If updateRolePermission Is Nothing Then

                updateRolePermission = CreateRolePermission(role)
                updateRolePermission.Read = Me.Read
                updateRolePermission.Create = Me.Create
                updateRolePermission.Update = Me.Update
                updateRolePermission.Delete = Me.Delete
            Else

                updateRolePermission.Read = Me.Read
                updateRolePermission.Create = Me.Create
                updateRolePermission.Update = Me.Update
                updateRolePermission.Delete = Me.Delete

            End If

            Return updateRolePermission
        End Function

    End Class

End Class
