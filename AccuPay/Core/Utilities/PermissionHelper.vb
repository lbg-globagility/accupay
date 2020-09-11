Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports Microsoft.Extensions.DependencyInjection

Namespace Desktop.Helpers

    Public Class PermissionHelper

        Public Shared Async Function AllowRead(
            permissionName As String,
            Optional userRole As AspNetRole = Nothing,
            Optional policyHelper As PolicyHelper = Nothing) _
            As Task(Of Boolean)

            Dim roleData = Await GetRoleAsync(permissionName, userRole, policyHelper)

            If roleData.Success Then
                Return roleData.RolePermission.Read
            Else
                Return False
            End If

        End Function

        Public Shared Async Function AllowCreate(
            permissionName As String,
            Optional userRole As AspNetRole = Nothing,
            Optional policyHelper As PolicyHelper = Nothing) _
            As Task(Of Boolean)

            Dim roleData = Await GetRoleAsync(permissionName, userRole, policyHelper)

            If roleData.Success Then
                Return roleData.RolePermission.Create
            Else
                Return False
            End If

        End Function

        Public Shared Async Function AllowUpdate(
            permissionName As String,
            Optional userRole As AspNetRole = Nothing,
            Optional policyHelper As PolicyHelper = Nothing) _
            As Task(Of Boolean)

            Dim roleData = Await GetRoleAsync(permissionName, userRole, policyHelper)

            If roleData.Success Then
                Return roleData.RolePermission.Update
            Else
                Return False
            End If

        End Function

        Public Shared Async Function GetRoleAsync(
            permissionName As String,
            Optional userRole As AspNetRole = Nothing,
            Optional policyHelper As PolicyHelper = Nothing) _
            As Task(Of (Success As Boolean, RolePermission As RolePermission))

            If String.IsNullOrWhiteSpace(permissionName) Then Return (False, Nothing)

            Dim role = Await rolePermission(permissionName, userRole)

            If policyHelper Is Nothing Then
                policyHelper = MainServiceProvider.GetRequiredService(Of PolicyHelper)
            End If

            If policyHelper.UseUserLevel Then

                Dim userLevelNotAllowedList = {PermissionConstant.ROLE.ToUpper()}
                If userLevelNotAllowedList.Contains(permissionName.ToUpper()) Then
                    Return (False, Nothing)
                End If
            Else
            End If

            Return (True, role)
        End Function

        Private Shared Async Function rolePermission(permissionName As String, userRole As AspNetRole) As Task(Of RolePermission)
            If userRole Is Nothing Then
                Dim roleRepository = MainServiceProvider.GetRequiredService(Of RoleRepository)
                USER_ROLES = Await roleRepository.GetByUserAndOrganization(userId:=z_User, organizationId:=z_OrganizationID)
                userRole = USER_ROLES
            End If

            Return userRole?.RolePermissions?.Where(Function(r) r.Permission.Name = permissionName).FirstOrDefault()
        End Function

    End Class

End Namespace