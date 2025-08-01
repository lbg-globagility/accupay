Option Strict On

Imports System.Configuration
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Enums
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Utilities
Imports log4net
Imports Microsoft.Extensions.DependencyInjection

Public Class MetroLogin

    Private Const UnauthorizedOrganizationMessage As String = "You are not authorized to access this organization."

    Private ReadOnly _policyHelper As IPolicyHelper

    Private ReadOnly _organizationRepository As IOrganizationRepository

    Private ReadOnly _roleRepository As IRoleRepository

    Private ReadOnly _systemInfoRepository As ISystemInfoRepository

    Private ReadOnly _userRepository As IAspNetUserRepository

    Private ReadOnly _encryptor As IEncryption

    Private Shared ReadOnly _logger As ILog = LogManager.GetLogger("LoginLogger")

    Sub New()

        InitializeComponent()

        DependencyInjectionHelper.ConfigureDependencyInjection()

        _policyHelper = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

        _organizationRepository = MainServiceProvider.GetRequiredService(Of IOrganizationRepository)

        _roleRepository = MainServiceProvider.GetRequiredService(Of IRoleRepository)

        _systemInfoRepository = MainServiceProvider.GetRequiredService(Of ISystemInfoRepository)

        _userRepository = MainServiceProvider.GetRequiredService(Of IAspNetUserRepository)

        _encryptor = MainServiceProvider.GetRequiredService(Of IEncryption)
    End Sub

    Protected Overrides Async Sub OnLoad(e As EventArgs)

        Await ReloadOrganizationAsync()

        MyBase.OnLoad(e)

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            Me.Close()

            Return True
        Else
            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub MetroLogin_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        Application.Exit()

    End Sub

    Private Async Sub MetroLogin_Load(sender As Object, e As EventArgs) Handles Me.Load

        Await CheckAppVersion()

        If Debugger.IsAttached Then
            AssignDefaultCredentials()

        End If

        OrganizationComboBox.DisplayMember = "Name"
    End Sub

    Public Async Function CheckAppVersion() As Task
        'Const warningMessage As String = "Please install the latest version of AccuPay. Contact your IT Department or Globagility Inc. for assistance."

        'Dim fsdfsd = Function(userVersion As String, systemVersion As String) As String
        '                 Return 
        '             End Function
        Dim warningMessage As String = String.Empty

        Try
            Dim appSettings = ConfigurationManager.AppSettings
            Dim currentVersion = appSettings.Get("payroll.version")

            Dim latestVersion = Await _systemInfoRepository.GetDesktopVersion()

            warningMessage = $"Please install the AccuPay version {latestVersion} (yours: {currentVersion}). Contact your IT Department or Globagility Inc. for assistance."

            If currentVersion <> latestVersion Then

                MessageBoxHelper.Warning(warningMessage)
                _logger.Error($"Incorrect version. Current version '{currentVersion}' | Latest version '{latestVersion}'")
                Me.Close()

            End If
        Catch ex As Exception
            MessageBoxHelper.Warning(warningMessage)
            _logger.Error("Exception in checking app version in desktop login.", ex)
            Me.Close()
        End Try
    End Function

    Public Sub AssignDefaultCredentials()
        UserNameTextBox.Text = "admin"
        PasswordTextBox.Text = "adminqwerty"
    End Sub

    Private Sub OrganizationComboBox_DropDown(sender As Object, e As EventArgs) Handles OrganizationComboBox.DropDown

        Dim organizations = CType(OrganizationComboBox.DataSource, List(Of Organization))

        If Not organizations.Any() Then Return

        Static font As Font = OrganizationComboBox.Font
        Dim grp As Graphics = OrganizationComboBox.CreateGraphics()

        Dim vertScrollBarWidth As Integer = If(OrganizationComboBox.Items.Count > OrganizationComboBox.MaxDropDownItems, SystemInformation.VerticalScrollBarWidth, 0)

        Dim width As Integer = 0

        Dim longestWord = organizations.
            OrderByDescending(Function(o) o.Name.Length).
            Select(Function(o) o.Name).
            FirstOrDefault()

        width = CInt(grp.MeasureString(longestWord, font).Width) + vertScrollBarWidth

        OrganizationComboBox.DropDownWidth = width

    End Sub

    Private Sub Login_KeyPress(sender As Object, e As KeyPressEventArgs) Handles _
        PasswordTextBox.KeyPress,
        UserNameTextBox.KeyPress,
        OrganizationComboBox.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 13 Then

            Login_Click(btnlogin, New EventArgs)

        End If

    End Sub

    Private Async Sub Login_Click(sender As Object, e As EventArgs) Handles btnlogin.Click
        Dim enableButton = Sub()
                               btnlogin.Enabled = True
                           End Sub
        Dim disableButton = Sub()
                                btnlogin.Enabled = False
                            End Sub
        Dim showError = Sub()
                            WarnBalloon("Please input your correct credentials.", "Invalid credentials", btnlogin, btnlogin.Width - 18, -69)
                            enableButton()
                        End Sub

        disableButton()

        Dim username As String = UserNameTextBox.Text
        Dim passkey As String = _encryptor.Encrypt(PasswordTextBox.Text)

        Dim user = Await _userRepository.GetByUserNameAsync(username)

        If user Is Nothing Then
            showError()
            Return
        End If

        Dim passwordMatch = user.DesktopPassword = passkey
        If Not passwordMatch Then
            showError()
            Return
        End If

        If OrganizationComboBox.SelectedIndex = -1 Then
            WarnBalloon("Please select a company.", "Invalid company", btnlogin, btnlogin.Width - 18, -69)
            OrganizationComboBox.Focus()
            enableButton()
            Return
        End If

        USER_ROLE = Await _roleRepository.GetByUserAndOrganizationAsync(userId:=user.Id, organizationId:=z_OrganizationID)

        If Not _policyHelper.UseUserLevel AndAlso USER_ROLE Is Nothing Then

            MessageBoxHelper.ErrorMessage(UnauthorizedOrganizationMessage)
            enableButton()
            Return

        End If

        z_User = user.Id
        Z_Client = user.ClientId

        userFirstName = user.FirstName
        z_postName = USER_ROLE?.Name

        If dbnow Is Nothing Then dbnow = EXECQUER(CURDATE_MDY)

        If Await CheckIfAuthorizedByUserLevel() Then
            MDIPrimaryForm.Show()
        End If

        enableButton()
    End Sub

    Private Async Function CheckIfAuthorizedByUserLevel() As Task(Of Boolean)
        Dim user = Await _userRepository.GetByIdAsync(z_User)

        If user Is Nothing Then

            MessageBoxHelper.ErrorMessage("User does not exists.")
            Return False
        End If

        If Not _policyHelper.UseUserLevel Then
            Return True
        End If

        If user.UserLevel = UserLevel.One OrElse
                user.UserLevel = UserLevel.Two OrElse
                user.UserLevel = UserLevel.Four OrElse
                user.UserLevel = UserLevel.Five Then

            Return True

        End If

        Dim organization = Await _organizationRepository.GetByIdAsync(z_OrganizationID)

        If organization Is Nothing Then

            MessageBoxHelper.ErrorMessage("Organization does not exists.")
            Return False
        End If

        If user.UserLevel = UserLevel.Three Then

            If organization.IsAgency = True Then

                Return True
            Else

                MessageBoxHelper.ErrorMessage(UnauthorizedOrganizationMessage)
                Return False

            End If

        End If

        Return False

    End Function

    Private Sub OrganizationComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles OrganizationComboBox.SelectedIndexChanged

        If OrganizationComboBox.SelectedValue Is Nothing Then Return

        Dim organization = CType(OrganizationComboBox.SelectedValue, Organization)

        If organization Is Nothing Then Return

        PhotoImages.Image = Nothing

        orgNam = organization.Name

        z_CompanyName = orgNam

        orgztnID = organization.RowID.ToString()

        z_OrganizationID = organization.RowID.Value

        If organization.Image IsNot Nothing AndAlso organization.Image.Length > 0 Then

            PhotoImages.Image = ConvertByteToImage(organization.Image)

        End If

    End Sub

    Private Sub MetroLink1_Click(sender As Object, e As EventArgs) Handles MetroLink1.Click

        'Hide forgot password until we require each client to provide an email account
        'or provide an email account by globagility that will be used in forgot password process
        Dim n_ForgotPasswordForm As New ForgotPasswordForm

        n_ForgotPasswordForm.ShowDialog()

    End Sub

    Async Function ReloadOrganizationAsync() As Task

        Dim list = Await _organizationRepository.List(OrganizationPageOptions.AllData, 1)

        If list.organizations IsNot Nothing Then

            OrganizationComboBox.DataSource = list.organizations.
                OrderBy(Function(o) o.Name).
                ToList()

        End If
    End Function

End Class
