Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Enums
Imports AccuPay.Data.Interfaces
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class MetroLogin

    Private Const UnauthorizedOrganizationMessage As String = "You are not authorized to access this organization."

    Private ReadOnly _policyHelper As PolicyHelper

    Private ReadOnly _organizationRepository As OrganizationRepository

    Private ReadOnly _roleRepository As RoleRepository

    Private ReadOnly _userRepository As AspNetUserRepository

    Private ReadOnly _encryptor As IEncryption

    Sub New()

        InitializeComponent()

        DependencyInjectionHelper.ConfigureDependencyInjection()

        _policyHelper = MainServiceProvider.GetRequiredService(Of PolicyHelper)

        _organizationRepository = MainServiceProvider.GetRequiredService(Of OrganizationRepository)

        _roleRepository = MainServiceProvider.GetRequiredService(Of RoleRepository)

        _userRepository = MainServiceProvider.GetRequiredService(Of AspNetUserRepository)

        _encryptor = MainServiceProvider.GetRequiredService(Of IEncryption)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        ReloadOrganization()

        MyBase.OnLoad(e)

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            Me.Close()

            Return True
        Else
            'ShiftTemplater
            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub MetroLogin_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        'n_FileObserver.Undetect()

        Application.Exit()

    End Sub

    Private Sub MetroLogin_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Debugger.IsAttached Then
            AssignDefaultCredentials()

        End If
    End Sub

    Public Sub AssignDefaultCredentials()
        UserNameTextBox.Text = "admin"
        PasswordTextBox.Text = "admin"
    End Sub

    Private Sub cbxorganiz_DropDown(sender As Object, e As EventArgs) Handles cbxorganiz.DropDown

        'TODO: this code has an error sometimes when there is no organization

        Static cb_font As Font = cbxorganiz.Font

        'Dim cb_width As Integer = cbxorganiz.DropDownWidth

        Dim grp As Graphics = cbxorganiz.CreateGraphics()

        Dim vertScrollBarWidth As Integer = If(cbxorganiz.Items.Count > cbxorganiz.MaxDropDownItems, SystemInformation.VerticalScrollBarWidth, 0)

        Dim wiidth As Integer = 0

        Dim data_source As New DataTable

        data_source = cbxorganiz.DataSource

        Dim i = 0

        Dim drp_downwidhths As Integer()

        ReDim drp_downwidhths(data_source.Rows.Count - 1)

        For Each strRow As DataRow In data_source.Rows

            wiidth = CInt(grp.MeasureString(CStr(strRow(1)), cb_font).Width) + vertScrollBarWidth

            drp_downwidhths(i) = wiidth

            'If cb_width < wiidth Then
            '    wiidth = wiidth
            'End If

            i += 1

        Next

        Dim max_drp_downwidhth As Integer = drp_downwidhths.Max

        cbxorganiz.DropDownWidth = max_drp_downwidhth 'wiidth, cb_width

    End Sub

    Private Sub Login_KeyPress(sender As Object, e As KeyPressEventArgs) Handles PasswordTextBox.KeyPress,
                                                                                    UserNameTextBox.KeyPress,
                                                                                    cbxorganiz.KeyPress

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
        Dim loadUserPrivileges = Sub(userId As Integer, organizationId As Integer)
                                     position_view_table =
                        New SQLQueryToDatatable(String.Concat("SELECT pv.*",
                                                              " FROM position_view pv",
                                                              " INNER JOIN `user` u ON u.RowID=", userId,
                                                              " INNER JOIN `position` pos ON pos.RowID=u.PositionID",
                                                              " INNER JOIN `position` p ON p.PositionName=pos.PositionName AND p.OrganizationID=pv.OrganizationID",
                                                              " WHERE pv.PositionID=p.RowID",
                                                              " AND pv.OrganizationID='", organizationId, "';")).ResultTable

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

        If cbxorganiz.SelectedIndex = -1 Then
            WarnBalloon("Please select a company.", "Invalid company", btnlogin, btnlogin.Width - 18, -69)
            cbxorganiz.Focus()
            enableButton()
            Return
        End If

        USER_ROLE = Await _roleRepository.GetByUserAndOrganization(userId:=user.Id, organizationId:=z_OrganizationID)

        If USER_ROLE Is Nothing Then

            MessageBoxHelper.ErrorMessage(UnauthorizedOrganizationMessage)
            enableButton()
            Return

        End If

        z_User = user.Id

        loadUserPrivileges(z_User, z_OrganizationID)

        userFirstName = user.FirstName
        z_postName = USER_ROLE.Name

        If dbnow Is Nothing Then dbnow = EXECQUER(CURDATE_MDY)
        If numofdaysthisyear = 0 Then numofdaysthisyear = EXECQUER("SELECT DAYOFYEAR(LAST_DAY(CONCAT(YEAR(CURRENT_DATE()),'-12-01')));")

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

        If _policyHelper.UseUserLevel = False Then
            Return True
        End If

        If user.UserLevel = UserLevel.One OrElse
                user.UserLevel = UserLevel.Two OrElse
                user.UserLevel = UserLevel.Four OrElse
                user.UserLevel = UserLevel.Five Then

            Return True

        End If

        Dim organization = _organizationRepository.GetById(z_OrganizationID)

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

    Function UserAuthentication(Optional pass_word As Object = Nothing)
        Dim n_ReadSQLFunction As New ReadSQLFunction("UserAuthentication",
                                                     "returnvaue",
                                                     _encryptor.Encrypt(UserNameTextBox.Text),
                                                     pass_word,
                                                     orgztnID)

        Dim returnobj = n_ReadSQLFunction.ReturnValue
        Dim returnvalue = ValNoComma(returnobj)

        Return returnvalue
    End Function

    Private Sub cbxorganiz_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxorganiz.SelectedIndexChanged

        PhotoImages.Image = Nothing

        orgNam = cbxorganiz.Text

        z_CompanyName = orgNam

        ''orgztnID = EXECQUER("SELECT RowID FROM organization WHERE Name='" & orgNam & "' LIMIT 1;")

        orgztnID = cbxorganiz.SelectedValue

        z_OrganizationID = ValNoComma(orgztnID)

        Dim org_emblem As New DataTable

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT Image FROM organization WHERE RowID='" & orgztnID & "' AND Image IS NOT NULL;")

        org_emblem = n_SQLQueryToDatatable.ResultTable

        If org_emblem.Rows.Count > 0 Then

            PhotoImages.Image = ConvertByteToImage(org_emblem.Rows(0)("Image"))

        End If

    End Sub

    Private Sub MetroLink1_Click(sender As Object, e As EventArgs) Handles MetroLink1.Click

        Dim n_ForgotPasswordForm As New ForgotPasswordForm

        n_ForgotPasswordForm.ShowDialog()
        '# ####################################### #
        'ForgotPasswordForm.Show()

        'MsgBox(Convert.ToBoolean("1"))

        'cbxorganiz.Enabled = Convert.ToBoolean("1")

        ''MsgBox(Convert.ToBoolean("0").ToString)

        'cbxorganiz.Enabled = Convert.ToBoolean("0")

        'Dim dialog_box = MessageBox.Show("Come on", "", MessageBoxButtons.YesNoCancel)

        'If dialog_box = Windows.Forms.DialogResult.Yes Then
        '    cbxorganiz.Enabled = Convert.ToBoolean(1)
        'Else
        '    cbxorganiz.Enabled = Convert.ToBoolean(0)
        'End If
        '# ####################################### #

    End Sub

    Sub ReloadOrganization()

        Dim strQuery As String = "SELECT RowID,Name FROM organization WHERE NoPurpose='0' ORDER BY Name;"

        Static once As SByte = 0

        If once = 0 Then

            once = 1

            Dim n_SQLQueryToDatatable As New SQLQueryToDatatable(strQuery)

            cbxorganiz.ValueMember = n_SQLQueryToDatatable.ResultTable.Columns(0).ColumnName

            cbxorganiz.DisplayMember = n_SQLQueryToDatatable.ResultTable.Columns(1).ColumnName

            cbxorganiz.DataSource = n_SQLQueryToDatatable.ResultTable
        Else

            Dim isThereSomeNewToOrganization =
                EXECQUER("SELECT EXISTS(SELECT RowID FROM organization WHERE DATE_FORMAT(Created,'%Y-%m-%d')=CURDATE() OR DATE_FORMAT(LastUpd,'%Y-%m-%d')=CURDATE() LIMIT 1);")

            If isThereSomeNewToOrganization = "1" Then

                Dim n_SQLQueryToDatatable As New SQLQueryToDatatable(strQuery)

                cbxorganiz.ValueMember = n_SQLQueryToDatatable.ResultTable.Columns(0).ColumnName

                cbxorganiz.DisplayMember = n_SQLQueryToDatatable.ResultTable.Columns(1).ColumnName

                cbxorganiz.DataSource = n_SQLQueryToDatatable.ResultTable
            Else

            End If

        End If

    End Sub

End Class