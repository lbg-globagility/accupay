Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Enums
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Interfaces
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class UserUserControl

    Private ReadOnly _organizationRepository As OrganizationRepository

    Private ReadOnly _encryptor As IEncryption

    Private _organizations As ICollection(Of Organization)

    Private _currentUser As AspNetUser

    Sub New()

        InitializeComponent()

        If MainServiceProvider IsNot Nothing Then

            _organizationRepository = MainServiceProvider.GetRequiredService(Of OrganizationRepository)

            _encryptor = MainServiceProvider.GetRequiredService(Of IEncryption)
        End If

    End Sub

    Private Async Sub UserUserControl_Load(sender As Object, e As EventArgs) Handles Me.Load

        UserRoleGrid.AutoGenerateColumns = False

        Await GetOrganizations()

        PopulateUserLeveComboBox()

        Await PopulateRoleComboBox()
    End Sub

    Private Async Function GetOrganizations() As Task
        If _organizations IsNot Nothing AndAlso _organizations.Count > 0 Then Return

        _organizations = (Await _organizationRepository.List(OrganizationPageOptions.AllData, Z_Client)).organizations.
            OrderBy(Function(o) o.Name).
            ToList()
    End Function

    Private Sub UserRoleGrid_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles UserRoleGrid.DataError
        e.ThrowException = False
    End Sub

    Private Sub PopulateUserLeveComboBox()
        UserLevelComboBox.DataSource = [Enum].GetValues(GetType(UserLevel)).Cast(Of UserLevel)().ToList
    End Sub

    Private Async Function PopulateRoleComboBox() As Task

        Dim roleRepository = MainServiceProvider.GetRequiredService(Of RoleRepository)
        Dim roles = (Await roleRepository.List(PageOptions.AllData, Z_Client)).
            roles.
            OrderBy(Function(r) r.Name).
            ToList()

        roles.Insert(0, New AspNetRole() With
        {
            .Name = "<UNAUTHORIZED>"
        })

        RoleColumn.DisplayMember = "Name"
        RoleColumn.ValueMember = "Id"

        RoleColumn.DataSource = roles
    End Function

    Public Async Function SetUser(user As AspNetUser) As Task

        _currentUser = user

        PopulateUserFields(user)

        Await PopulateUserRoleGrid(user)

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

        Dim userRepository = MainServiceProvider.GetRequiredService(Of AspNetUserRepository)
        Dim userRoles = Await userRepository.GetUserRolesAsync(user.Id)

        Dim models As New List(Of UserRoleViewModel)

        If _organizations IsNot Nothing AndAlso _organizations.Count > 0 Then
            Await GetOrganizations()
        End If

        For Each organization In _organizations

            Dim userRole = userRoles.FirstOrDefault(Function(o) o.OrganizationId = organization.RowID.Value)

            models.Add(New UserRoleViewModel(
                userId:=user.Id,
                roleId:=userRole?.Role?.Id,
                organization:=organization))

        Next

        If RoleColumn.Items.Count = 0 Then
            Await PopulateRoleComboBox()
        End If

        UserRoleGrid.DataSource = models
    End Function

    Public Function GetUserRoles() As List(Of UserRoleIdData)

        UserRoleGrid.EndEdit()

        Dim userRoles = CType(UserRoleGrid.DataSource, List(Of UserRoleViewModel))

        Return userRoles.
            Where(Function(r) r.HasChanged).
            Select(Function(r) r.ToUserRole()).
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
                _currentUser.DesktopPassword.ToTrimmedLowerCase() <> changedUser.DesktopPassword.ToTrimmedLowerCase()

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

        Public Function ToUserRole() As UserRoleIdData

            If _userId Is Nothing OrElse _organization.RowID Is Nothing Then
                Return Nothing
            End If

            Return New UserRoleIdData(_organization.RowID.Value, Me._userId.Value, Me.RoleId)
        End Function

        Public Function HasChanged() As Boolean

            Return Me.RoleId <> _originalRoleId

        End Function

    End Class

End Class