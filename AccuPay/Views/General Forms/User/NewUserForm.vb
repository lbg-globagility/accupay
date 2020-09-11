Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class NewUserForm

    Public Property IsSaved As Boolean
    Public Property NewUser As AspNetUser

    Private Async Sub NewUserForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim currentUser As New AspNetUser() With
        {
            .ClientId = Z_Client
        }

        Await UserUserControl.SetUser(currentUser, Desktop.Enums.FormMode.Creating)

    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        If UserUserControl.ValidateForm() = False Then Return

        Dim currentUser = UserUserControl.GetUserData()

        Dim userRoles = UserUserControl.GetUserRoles(allowNullUserId:=True)

        Await FunctionUtils.TryCatchFunctionAsync("Create User",
            Async Function()
                Dim userService = MainServiceProvider.GetRequiredService(Of UserDataService)
                Dim roleService = MainServiceProvider.GetRequiredService(Of RoleDataService)

                Await userService.CreateAsync(currentUser, isEncrypted:=True)

                For Each userRole In userRoles

                    userRole.UserId = currentUser.Id

                Next

                Await roleService.UpdateUserRolesAsync(userRoles, Z_Client)

                Me.IsSaved = True
                Me.NewUser = currentUser

                Me.Close()

            End Function)

    End Sub

End Class