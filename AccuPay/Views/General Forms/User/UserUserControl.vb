Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Enums
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports AccuPay.Desktop.Enums
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class UserUserControl

    Private ReadOnly _encryptor As IEncryption

    Private ReadOnly _policy As IPolicyHelper

    Private _organizations As ICollection(Of Organization)

    Private _currentUser As AspNetUser

    Private _formMode As FormMode
    Private _roles As List(Of AspNetRole)

    Sub New()

        InitializeComponent()

        If MainServiceProvider IsNot Nothing Then

            _encryptor = MainServiceProvider.GetRequiredService(Of IEncryption)

            _policy = MainServiceProvider.GetRequiredService(Of IPolicyHelper)
        End If

    End Sub

    Private Async Sub UserUserControl_Load(sender As Object, e As EventArgs) Handles Me.Load

        UserRoleGrid.AutoGenerateColumns = False

        UserLevelComboBox.Visible = _policy.UseUserLevel
        UserLevelLabel.Visible = _policy.UseUserLevel
        UserRoleGrid.Visible = Not _policy.UseUserLevel
        UserRoleLabel.Visible = Not _policy.UseUserLevel

        Await GetOrganizations()

        PopulateUserLeveComboBox()

        Await PopulateRoleComboBox()
    End Sub

    Public Async Function SetUser(user As AspNetUser, Optional formMode As FormMode = FormMode.Editing) As Task

        _currentUser = user

        _formMode = formMode

        If (_formMode = FormMode.Creating AndAlso (Await PermissionHelper.DoesAllowCreateAsync(PermissionConstant.USER, policyHelper:=_policy)) = False) OrElse
           (_formMode = FormMode.Editing AndAlso (Await PermissionHelper.DoesAllowUpdateAsync(PermissionConstant.USER, policyHelper:=_policy)) = False) Then

            SetFormToReadOnly()
        End If

        PopulateUserFields(user)

        Await PopulateUserRoleGrid(user)

    End Function

    Private Sub SetFormToReadOnly()

        UserRoleGrid.ReadOnly = True
        DetailsGroup.Enabled = False

    End Sub

    Private Async Function GetOrganizations() As Task
        If _organizations IsNot Nothing AndAlso _organizations.Count > 0 Then Return

        Dim userRepository = MainServiceProvider.GetRequiredService(Of IAspNetUserRepository)
        Dim userRoles = Await userRepository.GetUserRolesAsync(z_User)

        Dim allowedOrganizations = userRoles.
            GroupBy(Function(o) o.OrganizationId).
            Select(Function(o) o.Key).
            ToArray()

        ' TODO: check also if in that organization, the user has a role with permission to create and update Role

        Dim organizationRepository = MainServiceProvider.GetRequiredService(Of IOrganizationRepository)
        Dim organizations = (Await organizationRepository.List(OrganizationPageOptions.AllData, Z_Client)).organizations.
            OrderBy(Function(o) o.Name).
            ToList()

        _organizations = organizations.
            Where(Function(o) allowedOrganizations.Contains(o.RowID.Value)).
            ToList()

    End Function

    Private Sub UserRoleGrid_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles UserRoleGrid.DataError
        e.ThrowException = False
    End Sub

    Private Sub PopulateUserLeveComboBox()
        UserLevelComboBox.DataSource = [Enum].GetValues(GetType(UserLevel)).Cast(Of UserLevel)().ToList
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

    Private Sub PopulateUserFields(user As AspNetUser)

        LastNameTextBox.Text = user.LastName
        FirstNameTextBox.Text = user.FirstName

        If UserLevelComboBox.Items.Count = 0 Then
            PopulateUserLeveComboBox()
        End If

        UserLevelComboBox.SelectedIndex = user.UserLevel

        EmailTextBox.Text = user.Email
        UserNameTextBox.Text = user.UserName
        PasswordTextBox.Text = _encryptor.Decrypt(user.DesktopPassword)
        ConfirmPasswordTextBox.Text = PasswordTextBox.Text
    End Sub

    Private Async Function PopulateUserRoleGrid(user As AspNetUser) As Task

        If RoleColumn.Items.Count = 0 Then
            Await PopulateRoleComboBox()
        End If

        Dim userRepository = MainServiceProvider.GetRequiredService(Of IAspNetUserRepository)
        Dim userRoles = Await userRepository.GetUserRolesAsync(user.Id)

        Dim models As New List(Of UserRoleViewModel)

        If _organizations Is Nothing OrElse _organizations.Count = 0 Then
            Await GetOrganizations()
        End If

        Dim defaultRoleId = _roles.FirstOrDefault(Function(r) r.IsAdmin)?.Id

        For Each organization In _organizations

            Dim userRole = userRoles.FirstOrDefault(Function(o) o.OrganizationId = organization.RowID.Value)

            'since VB cannot handle ternary operators well
            'we have to do this sh*t
            Dim roleId As Integer?
            If _policy.UseUserLevel Then
                roleId = defaultRoleId
            Else
                roleId = userRole?.Role?.Id
            End If

            models.Add(New UserRoleViewModel(
                userId:=user.Id,
                roleId:=roleId,
                organization:=organization))

        Next

        UserRoleGrid.DataSource = models
    End Function

    Public Function GetUserRoles(Optional allowNullUserId As Boolean = False) As List(Of UserRoleIdData)

        UserRoleGrid.EndEdit()

        Dim userRoles = CType(UserRoleGrid.DataSource, List(Of UserRoleViewModel))

        Return userRoles.
            Where(Function(r) r.HasChanged).
            Select(Function(r) r.ToUserRole(allowNullUserId)).
            Where(Function(r) r IsNot Nothing).
            ToList()

    End Function

    Public Function GetUserData() As AspNetUser

        If _currentUser Is Nothing Then Return Nothing

        Dim changedUser = _currentUser.CloneJson()

        changedUser.LastName = LastNameTextBox.Text.Trim()
        changedUser.FirstName = FirstNameTextBox.Text.Trim()

        changedUser.UserLevel = UserLevelComboBox.SelectedIndex

        changedUser.Email = EmailTextBox.Text.Trim()
        changedUser.UserName = UserNameTextBox.Text.Trim()
        changedUser.DesktopPassword = _encryptor.Encrypt(PasswordTextBox.Text.Trim())

        Return changedUser
    End Function

    Public Function HasChanged() As Boolean

        If _currentUser Is Nothing Then Return False

        UserRoleGrid.EndEdit()

        Dim userRoles = CType(UserRoleGrid.DataSource, List(Of UserRoleViewModel))

        If userRoles.Any(Function(r) r.HasChanged) Then Return True

        Dim changedUser = GetUserData()

        Return _currentUser.LastName.ToTrimmedLowerCase() <> changedUser.LastName.ToTrimmedLowerCase() OrElse
                _currentUser.FirstName.ToTrimmedLowerCase() <> changedUser.FirstName.ToTrimmedLowerCase() OrElse
                _currentUser.UserLevel <> changedUser.UserLevel OrElse
                _currentUser.Email.ToTrimmedLowerCase() <> changedUser.Email.ToTrimmedLowerCase() OrElse
                _currentUser.UserName.ToTrimmedLowerCase() <> changedUser.UserName.ToTrimmedLowerCase() OrElse
                _currentUser.DesktopPassword.ToTrimmedLowerCase() <> changedUser.DesktopPassword.ToTrimmedLowerCase() OrElse
                changedUser.DesktopPassword <> _encryptor.Encrypt(ConfirmPasswordTextBox.Text.Trim())

    End Function

    Public Function ValidateForm() As Boolean

        If String.IsNullOrWhiteSpace(LastNameTextBox.Text.Trim()) Then
            MessageBoxHelper.Warning("Last Name is required.")
            LastNameTextBox.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(FirstNameTextBox.Text.Trim()) Then
            MessageBoxHelper.Warning("First Name is required.")
            FirstNameTextBox.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(UserNameTextBox.Text.Trim()) Then
            MessageBoxHelper.Warning("UserName is required.")
            UserNameTextBox.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(PasswordTextBox.Text.Trim()) Then
            MessageBoxHelper.Warning("Password is required.")
            PasswordTextBox.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(ConfirmPasswordTextBox.Text.Trim()) Then
            MessageBoxHelper.Warning("Confirm Password is required.")
            ConfirmPasswordTextBox.Focus()
            Return False
        End If

        If PasswordTextBox.Text <> ConfirmPasswordTextBox.Text Then
            MessageBoxHelper.Warning("Password does not match.")
            ConfirmPasswordTextBox.Focus()
            Return False
        End If

        Return True
    End Function

    Public Class UserRoleViewModel

        Public Property RoleId As Integer

        Private ReadOnly _originalRoleId As Integer

        Private ReadOnly Property _userId As Integer?

        Private ReadOnly _organization As Organization

        Public ReadOnly Property OrganizationName As String
            Get
                Return _organization?.Name
            End Get
        End Property

        Sub New(userId As Integer?, roleId As Integer?, organization As Organization)

            _originalRoleId = If(roleId, 0)
            _organization = organization

            _userId = userId
            Me.RoleId = _originalRoleId
        End Sub

        Public Function ToUserRole(allowNullUserId As Boolean) As UserRoleIdData

            If (Not allowNullUserId AndAlso _userId Is Nothing) OrElse _organization.RowID Is Nothing Then
                Return Nothing
            End If

            Return New UserRoleIdData(_organization.RowID.Value, Me._userId.Value, Me.RoleId)
        End Function

        Public Function HasChanged() As Boolean

            Return Me.RoleId <> _originalRoleId

        End Function

    End Class

End Class
