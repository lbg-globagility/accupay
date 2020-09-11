Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class UsersForm2

    Private ReadOnly _userRepository As AspNetUserRepository

    Sub New()

        InitializeComponent()

        _userRepository = MainServiceProvider.GetRequiredService(Of AspNetUserRepository)
    End Sub

    Private Async Sub UsersForm2_Load(sender As Object, e As EventArgs) Handles Me.Load

        UserGrid.AutoGenerateColumns = False

        Await PopulateUserGrid()

        AddHandler UserGrid.SelectionChanged, AddressOf UserGridSelectionChanged
    End Sub

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

    Private Sub NewButton_Click(sender As Object, e As EventArgs) Handles NewButton.Click

    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

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

                Await userService.UpdateAsync(user)

                Await roleService.UpdateUserRolesAsync(userRoles, Z_Client)

                RemoveHandler UserGrid.SelectionChanged, AddressOf UserGridSelectionChanged

                Await PopulateUserGrid(GetSelectedUser())

                AddHandler UserGrid.SelectionChanged, AddressOf UserGridSelectionChanged

                InfoBalloon("User has been successfully saved.", "User Saved", LabelForBalloon, 0, -69)
            End Function)
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click

    End Sub

    Private Async Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click

        RemoveHandler UserGrid.SelectionChanged, AddressOf UserGridSelectionChanged

        Await PopulateUserGrid(GetSelectedUser())

        AddHandler UserGrid.SelectionChanged, AddressOf UserGridSelectionChanged
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
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