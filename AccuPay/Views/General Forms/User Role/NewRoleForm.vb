Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class NewRoleForm

    Private ReadOnly _roleRepository As IRoleRepository

    Public Property IsSaved As Boolean
    Public Property NewRole As AspNetRole

    Sub New()

        InitializeComponent()

        _roleRepository = MainServiceProvider.GetRequiredService(Of IRoleRepository)
    End Sub

    Private Async Sub NewRoleForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim currentRole As New AspNetRole() With
        {
            .ClientId = Z_Client
        }

        Await RoleUserControl.SetRole(currentRole, Desktop.Enums.FormMode.Creating)

    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Dim currentRole = RoleUserControl.GetUpdatedRole()

        Await FunctionUtils.TryCatchFunctionAsync("Create Role",
            Async Function()
                Dim roleRepository = MainServiceProvider.GetRequiredService(Of RoleDataService)
                Await roleRepository.CreateAsync(currentRole)

                USER_ROLE = Await _roleRepository.GetByUserAndOrganizationAsync(userId:=z_User, organizationId:=z_OrganizationID)

                Me.IsSaved = True
                Me.NewRole = currentRole

                Me.Close()

            End Function)

    End Sub

End Class
