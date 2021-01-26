Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports Microsoft.Extensions.DependencyInjection

Namespace Desktop.Helpers

    Public Class PermissionHelper

        Public Shared Async Function DoesAllowReadAsync(
            permissionName As String,
            Optional userRole As AspNetRole = Nothing,
            Optional policyHelper As IPolicyHelper = Nothing) _
            As Task(Of Boolean)

            Dim roleData = Await GetRoleAsync(permissionName, userRole, policyHelper)

            If roleData.Success Then
                Return roleData.RolePermission.Read
            Else
                Return False
            End If

        End Function

        Public Shared Async Function DoesAllowCreateAsync(
            permissionName As String,
            Optional userRole As AspNetRole = Nothing,
            Optional policyHelper As IPolicyHelper = Nothing) _
            As Task(Of Boolean)

            Dim roleData = Await GetRoleAsync(permissionName, userRole, policyHelper)

            If roleData.Success Then
                Return roleData.RolePermission.Create
            Else
                Return False
            End If

        End Function

        Public Shared Async Function DoesAllowUpdateAsync(
            permissionName As String,
            Optional userRole As AspNetRole = Nothing,
            Optional policyHelper As IPolicyHelper = Nothing) _
            As Task(Of Boolean)

            Dim roleData = Await GetRoleAsync(permissionName, userRole, policyHelper)

            If roleData.Success Then
                Return roleData.RolePermission.Update
            Else
                Return False
            End If

        End Function

        Public Shared Function DoesAllowRead(
            permissionName As String,
            Optional userRole As AspNetRole = Nothing,
            Optional policyHelper As IPolicyHelper = Nothing) _
            As Boolean

            Dim roleData = GetRole(permissionName, userRole, policyHelper)

            If roleData.Success Then
                Return roleData.RolePermission.Read
            Else
                Return False
            End If

        End Function

        Public Shared Function DoesAllowCreate(
            permissionName As String,
            Optional userRole As AspNetRole = Nothing,
            Optional policyHelper As IPolicyHelper = Nothing) _
            As Boolean

            Dim roleData = GetRole(permissionName, userRole, policyHelper)

            If roleData.Success Then
                Return roleData.RolePermission.Create
            Else
                Return False
            End If

        End Function

        Public Shared Function DoesAllowUpdate(
            permissionName As String,
            Optional userRole As AspNetRole = Nothing,
            Optional policyHelper As IPolicyHelper = Nothing) _
            As Boolean

            Dim roleData = GetRole(permissionName, userRole, policyHelper)

            If roleData.Success AndAlso roleData.RolePermission IsNot Nothing Then
                Return roleData.RolePermission.Update
            Else
                Return False
            End If

        End Function

        Public Shared Async Function GetRoleAsync(
            permissionName As String,
            Optional userRole As AspNetRole = Nothing,
            Optional policyHelper As IPolicyHelper = Nothing) _
            As Task(Of (Success As Boolean, RolePermission As RolePermission))

            If String.IsNullOrWhiteSpace(permissionName) Then Return (False, Nothing)

            Dim role = Await GetRolePermissionAsync(permissionName, userRole)

            Return GetRoleBase(permissionName, policyHelper, role)
        End Function

        Public Shared Function GetRole(
            permissionName As String,
            Optional userRole As AspNetRole = Nothing,
            Optional policyHelper As IPolicyHelper = Nothing) _
            As (Success As Boolean, RolePermission As RolePermission)

            If String.IsNullOrWhiteSpace(permissionName) Then Return (False, Nothing)

            Dim role = GetRolePermission(permissionName, userRole)

            Return GetRoleBase(permissionName, policyHelper, role)
        End Function

        Private Shared Function GetRoleBase(permissionName As String, ByRef policyHelper As IPolicyHelper, role As RolePermission) As (Success As Boolean, RolePermission As RolePermission)
            If policyHelper Is Nothing Then
                policyHelper = MainServiceProvider.GetRequiredService(Of IPolicyHelper)
            End If

            If policyHelper.UseUserLevel Then

                Dim userLevelNotAllowedList = {PermissionConstant.ROLE.ToUpper()}
                If userLevelNotAllowedList.Contains(permissionName.ToUpper()) Then
                    Return (False, Nothing)
                End If
            Else
            End If

            Return (If(role Is Nothing, False, True), role)
        End Function

        Private Shared Async Function GetRolePermissionAsync(permissionName As String, userRole As AspNetRole) As Task(Of RolePermission)
            If userRole Is Nothing Then
                Dim roleRepository = MainServiceProvider.GetRequiredService(Of IRoleRepository)
                USER_ROLE = Await roleRepository.GetByUserAndOrganizationAsync(userId:=z_User, organizationId:=z_OrganizationID)
                userRole = USER_ROLE
            End If

            Return GetRolePermissionByName(permissionName, userRole)
        End Function

        Private Shared Function GetRolePermission(permissionName As String, userRole As AspNetRole) As RolePermission

            If userRole Is Nothing Then
                Dim roleRepository = MainServiceProvider.GetRequiredService(Of IRoleRepository)
                USER_ROLE = roleRepository.GetByUserAndOrganization(userId:=z_User, organizationId:=z_OrganizationID)
                userRole = USER_ROLE
            End If

            Return GetRolePermissionByName(permissionName, userRole)
        End Function

        Private Shared Function GetRolePermissionByName(permissionName As String, userRole As AspNetRole) As RolePermission

            Return userRole?.RolePermissions?.Where(Function(r) r.Permission.Name = permissionName).FirstOrDefault()
        End Function

    End Class

End Namespace
