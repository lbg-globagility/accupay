Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class NewRoleForm

    Private ReadOnly _roleRepository As RoleRepository

    Public Property IsSaved As Boolean
    Public Property NewRole As AspNetRole

    Sub New()

        InitializeComponent()

        _roleRepository = MainServiceProvider.GetRequiredService(Of RoleRepository)
    End Sub

    Private Async Sub NewRoleForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim currentRole As New AspNetRole() With
        {
            .ClientId = Z_Client
        }

        Await RoleUserControl.SetRole(currentRole)

    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Dim currentRole = RoleUserControl.GetUpdatedRole()

        Await FunctionUtils.TryCatchFunctionAsync("Create Role",
            Async Function()
                Dim roleRepository = MainServiceProvider.GetRequiredService(Of RoleDataService)
                Await roleRepository.CreateAsync(currentRole)

                USER_ROLES = Await _roleRepository.GetByUserAndOrganization(userId:=z_User, organizationId:=z_OrganizationID)

                IsSaved = True

                Me.Close()
                Me.NewRole = currentRole

            End Function)

    End Sub

End Class