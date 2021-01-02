Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class UserForm

    Private ReadOnly _userRepository As IAspNetUserRepository

    Sub New()

        InitializeComponent()

        _userRepository = MainServiceProvider.GetRequiredService(Of IAspNetUserRepository)
    End Sub

    Private Async Sub UsersForm2_Load(sender As Object, e As EventArgs) Handles Me.Load

        UserGrid.AutoGenerateColumns = False

        Await CheckRolePermissions()

        Await PopulateUserGrid()

        AddHandler UserGrid.SelectionChanged, AddressOf UserGridSelectionChanged
    End Sub

    Private Async Function CheckRolePermissions() As Task
        Dim role = Await PermissionHelper.GetRoleAsync(PermissionConstant.USER)

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

    Private Async Function PopulateUserGrid(Optional selectedUser As AspNetUser = Nothing) As Task

        UserGrid.DataSource = (Await _userRepository.List(PageOptions.AllData, Z_Client)).
            users.
            OrderBy(Function(u) u.LastName).
            ToList()

        Dim users = CType(UserGrid.DataSource, List(Of AspNetUser))

        If users.Any Then

            If selectedUser Is Nothing Then
                selectedUser = CType(UserGrid.CurrentRow.DataBoundItem, AspNetUser)
            End If

            If selectedUser IsNot Nothing Then

                Dim currentUser = users.FirstOrDefault(Function(u) u.Id = selectedUser.Id)

                selectedUser = currentUser

                Dim currentUserIndex = users.IndexOf(currentUser)

                UserGrid.CurrentCell = UserGrid.Rows(currentUserIndex).Cells(0)

            End If

        End If

        Await UserUserControl.SetUser(selectedUser)
    End Function

    Private Function GetSelectedUser() As AspNetUser
        If UserGrid.RowCount = 0 Then Return Nothing

        Return CType(UserGrid.CurrentRow.DataBoundItem, AspNetUser)

    End Function

    Private Async Sub NewToolStripButton_Click(sender As Object, e As EventArgs) Handles NewToolStripButton.Click

        Dim dialog As New NewUserForm()
        dialog.ShowDialog()

        If dialog.IsSaved AndAlso dialog.NewUser IsNot Nothing Then

            RemoveHandler UserGrid.SelectionChanged, AddressOf UserGridSelectionChanged

            Await PopulateUserGrid(dialog.NewUser)

            AddHandler UserGrid.SelectionChanged, AddressOf UserGridSelectionChanged

            InfoBalloon("New user has been successfully created.", "User Created", LabelForBalloon, 0, -69)

        End If

    End Sub

    Private Async Sub SaveToolStripButton_Click(sender As Object, e As EventArgs) Handles SaveToolStripButton.Click

        If UserUserControl.HasChanged = False Then

            MessageBoxHelper.Warning("No unsaved changes!")
            Return

        End If

        If UserUserControl.ValidateForm() = False Then Return

        Dim user = UserUserControl.GetUserData()

        Dim userRoles = UserUserControl.GetUserRoles()

        Await FunctionUtils.TryCatchFunctionAsync("Save User",
            Async Function()
                Dim userService = MainServiceProvider.GetRequiredService(Of UserDataService)
                Dim roleService = MainServiceProvider.GetRequiredService(Of RoleDataService)

                Await userService.UpdateAsync(user, isEncrypted:=True)

                Await roleService.UpdateUserRolesAsync(userRoles, Z_Client)

                RemoveHandler UserGrid.SelectionChanged, AddressOf UserGridSelectionChanged

                Await PopulateUserGrid(GetSelectedUser())

                AddHandler UserGrid.SelectionChanged, AddressOf UserGridSelectionChanged

                InfoBalloon("User has been successfully saved.", "User Saved", LabelForBalloon, 0, -69)
            End Function)
    End Sub

    Private Async Sub DeleteToolStripButton_Click(sender As Object, e As EventArgs) Handles DeleteToolStripButton.Click

        Dim currentUser = GetSelectedUser()

        If currentUser Is Nothing Then

            MessageBoxHelper.Warning("No selected user!")
            Return

        End If

        If MessageBoxHelper.Confirm(Of Boolean) _
        ($"Are you sure you want to delete the user `{currentUser.UserName}`?", "Confirm Deletion") = False Then

            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync("Delete User",
            Async Function()
                Dim userService = MainServiceProvider.GetRequiredService(Of UserDataService)

                Await userService.SoftDeleteAsync(
                    id:=currentUser.Id,
                    deletedByUserId:=z_User,
                    clientId:=Z_Client
                )

                RemoveHandler UserGrid.SelectionChanged, AddressOf UserGridSelectionChanged

                Await PopulateUserGrid()

                AddHandler UserGrid.SelectionChanged, AddressOf UserGridSelectionChanged

                InfoBalloon("User has been successfully deleted.", "User Deleted", LabelForBalloon, 0, -69)
            End Function)

    End Sub

    Private Async Sub CancelToolStripButton_Click(sender As Object, e As EventArgs) Handles CancelToolStripButton.Click

        RemoveHandler UserGrid.SelectionChanged, AddressOf UserGridSelectionChanged

        Await PopulateUserGrid(GetSelectedUser())

        AddHandler UserGrid.SelectionChanged, AddressOf UserGridSelectionChanged
    End Sub

    Private Sub CloseToolStripButton_Click(sender As Object, e As EventArgs) Handles CloseToolStripButton.Click
        Me.Close()
    End Sub

    Private Sub UsersForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        InfoBalloon(, , LabelForBalloon, , , 1)

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        GeneralForm.listGeneralForm.Remove(Me.Name)

    End Sub

    Private Async Sub UserGridSelectionChanged(sender As Object, e As EventArgs)

        Await UserUserControl.SetUser(GetSelectedUser())

    End Sub

End Class
