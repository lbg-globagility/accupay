Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports Microsoft.Extensions.DependencyInjection

Public Class OrganizationUserRolesControl

    Private _users As ICollection(Of AspNetUser)
    Private _roles As List(Of AspNetRole)

    Private ReadOnly _policy As IPolicyHelper

    Sub New()

        InitializeComponent()

        If MainServiceProvider IsNot Nothing Then

            _policy = MainServiceProvider.GetRequiredService(Of IPolicyHelper)
        End If

    End Sub

    Private Async Sub OrganizationUserRolesControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        UserRoleGrid.AutoGenerateColumns = False

        Await GetUsers()

        Await PopulateRoleComboBox()
    End Sub

    Public Async Function SetOrganization(organizationId As Integer?, isReadOnly As Boolean) As Task

        UserRoleGrid.ReadOnly = isReadOnly

        Await PopulateUserRoleGrid(organizationId)

    End Function

    Private Async Function GetUsers() As Task
        If _users IsNot Nothing AndAlso _users.Count > 0 Then Return

        Dim repository = MainServiceProvider.GetRequiredService(Of IAspNetUserRepository)
        _users = (Await repository.List(PageOptions.AllData, Z_Client)).users.
            OrderBy(Function(o) o.FullNameLastNameFirst).
            ToList()

    End Function

    Private Sub UserRoleGrid_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles UserRoleGrid.DataError
        e.ThrowException = False
    End Sub

    Private Async Function PopulateRoleComboBox() As Task

        Dim roleRepository = MainServiceProvider.GetRequiredService(Of IRoleRepository)
        _roles = (Await roleRepository.List(PageOptions.AllData, Z_Client)).
            roles.
            OrderBy(Function(r) r.Name).
            ToList()

        _roles.Insert(0, New AspNetRole() With
        {
            .Name = "<UNAUTHORIZED>"
        })

        RoleColumn.DisplayMember = "Name"
        RoleColumn.ValueMember = "Id"

        RoleColumn.DataSource = _roles
    End Function

    Private Async Function PopulateUserRoleGrid(organizationId As Integer?) As Task

        Dim reepository = MainServiceProvider.GetRequiredService(Of IOrganizationRepository)

        Dim userRoles As New List(Of UserRoleData)

        If organizationId IsNot Nothing Then
            userRoles = (Await reepository.GetUserRolesAsync(organizationId.Value)).ToList()

        End If

        Dim models As New List(Of UserRoleViewModel)

        If _users Is Nothing OrElse _users.Count = 0 Then
            Await GetUsers()
        End If

        Dim defaultRoleId = _roles.FirstOrDefault(Function(r) r.IsAdmin)?.Id

        For Each user In _users

            Dim userRole = userRoles.FirstOrDefault(Function(o) o.User?.Id IsNot Nothing AndAlso o.User.Id = user.Id)

            'since VB cannot handle ternary operators well
            'we have to do this sh*t
            Dim roleId As Integer?
            If _policy.UseUserLevel Then
                roleId = defaultRoleId
            Else
                roleId = userRole?.Role?.Id
            End If

            models.Add(New UserRoleViewModel(
                organizationId:=organizationId,
                roleId:=roleId,
                user:=user))

        Next

        If RoleColumn.Items.Count = 0 Then
            Await PopulateRoleComboBox()
        End If

        UserRoleGrid.DataSource = models
    End Function

    Public Function GetUserRoles(Optional allowNullUserId As Boolean = False) As List(Of UserRoleIdData)

        UserRoleGrid.EndEdit()

        Dim userRoles = CType(UserRoleGrid.DataSource, List(Of UserRoleViewModel))

        Return userRoles.
            Select(Function(r) r.ToUserRole(allowNullUserId)).
            Where(Function(r) r IsNot Nothing).
            ToList()

    End Function

    Public Class UserRoleViewModel

        Public Property RoleId As Integer

        Private ReadOnly _originalRoleId As Integer

        Private ReadOnly Property _organizationId As Integer?

        Private ReadOnly _user As AspNetUser

        Public ReadOnly Property FullName As String
            Get
                Return _user?.FullNameLastNameFirst
            End Get
        End Property

        Sub New(organizationId As Integer?, roleId As Integer?, user As AspNetUser)

            _originalRoleId = If(roleId, 0)
            _user = user

            _organizationId = organizationId
            Me.RoleId = _originalRoleId
        End Sub

        Public Function ToUserRole(allowNullOrganizationId As Boolean) As UserRoleIdData

            If (Not allowNullOrganizationId AndAlso _organizationId Is Nothing) OrElse _user.Id <= 0 Then
                Return Nothing
            End If

            Return New UserRoleIdData(If(_organizationId Is Nothing, 0, Me._organizationId.Value), _user.Id, Me.RoleId)
        End Function

    End Class

End Class
