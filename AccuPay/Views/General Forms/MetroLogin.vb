Imports AccuPay.Data.Enums
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Utils

Public Class MetroLogin
    Private _userRepository As UserRepository

    Private _organizationRepository As OrganizationRepository
    Private err_count As Integer
    Private freq As String

    Protected Overrides Sub OnLoad(e As EventArgs)
        _userRepository = New UserRepository()
        _organizationRepository = New OrganizationRepository

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
        txtbxUserID.Text = "admin"
        txtbxPword.Text = "admin"
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

    Private Sub Login_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxPword.KeyPress,
                                                                                    txtbxUserID.KeyPress,
                                                                                    cbxorganiz.KeyPress

        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 13 Then

            btnlogin_Click(btnlogin, New EventArgs)

        End If

    End Sub

    Async Sub btnlogin_Click(sender As Object, e As EventArgs) Handles btnlogin.Click

        btnlogin.Enabled = False

        Dim n_edUserID As New EncryptString(txtbxUserID.Text)

        Dim n_edPWord As New EncryptString(txtbxPword.Text)

        Dim n_ReadSQLFunction As New ReadSQLFunction("user_improper_out",
                                                     "return_val",
                                                     orgztnID,
                                                     n_edUserID.ResultValue,
                                                     n_edPWord.ResultValue) 'New EncryptData(txtbxPword.Text).ResultValue

        z_User = UserAuthentication(n_edPWord.ResultValue)

        Console.WriteLine(DecryptData("¯õæøøüô÷"))

        If z_User > 0 Then

            Try

                If cbxorganiz.SelectedIndex = -1 Then

                    WarnBalloon("Please select a company.", "Invalid company", btnlogin, btnlogin.Width - 18, -69)

                    cbxorganiz.Focus()
                    btnlogin.Enabled = True
                    Exit Sub

                End If

                err_count = 0 'resets the failed log in attempt count

                Dim userFNameLName = EXECQUER("SELECT CONCAT(COALESCE(FirstName,'.'),',',COALESCE(LastName,'.')) FROM user WHERE RowID='" & z_User & "';")

                Dim splitFNameLName = Split(userFNameLName, ",")

                userFirstName = splitFNameLName(0).ToString.Replace(".", "")

                If userFirstName = "" Then
                Else
                    userFirstName = StrConv(userFirstName, VbStrConv.ProperCase)

                End If

                userLastName = splitFNameLName(1).ToString.Replace(".", "")

                If userLastName = "" Then
                Else
                    userLastName = StrConv(userLastName, VbStrConv.ProperCase)

                End If

                If dbnow = Nothing Then

                    dbnow = EXECQUER(CURDATE_MDY)

                End If

                If cbxorganiz.SelectedIndex <> -1 Then

                    numofdaysthisyear = EXECQUER("SELECT DAYOFYEAR(LAST_DAY(CONCAT(YEAR(CURRENT_DATE()),'-12-01')));")

                    If freq <> orgztnID Then
                        freq = orgztnID

                    End If

                    position_view_table =
                        New SQLQueryToDatatable(String.Concat("SELECT pv.*",
                                                              " FROM position_view pv",
                                                              " INNER JOIN `user` u ON u.RowID=", z_User,
                                                              " INNER JOIN `position` pos ON pos.RowID=u.PositionID",
                                                              " INNER JOIN `position` p ON p.PositionName=pos.PositionName AND p.OrganizationID=pv.OrganizationID",
                                                              " WHERE pv.PositionID=p.RowID",
                                                              " AND pv.OrganizationID='", orgztnID, "';")).ResultTable

                    Dim i = position_view_table.Rows.Count

                End If

                z_postName = EXECQUER("SELECT p.PositionName FROM user u INNER JOIN position p ON p.RowID=u.PositionID WHERE u.RowID='" & z_User & "';")

                If orgztnID <> Nothing Then

                    If Await CheckIfAuthorizedByUserLevel() Then
                        MDIPrimaryForm.Show()
                    End If

                    'Dim n_MDIPrimaryForm As New MDIPrimaryForm
                    'n_MDIPrimaryForm.Show()
                End If
            Catch ex As Exception

                MsgBox(getErrExcptn(ex, Me.Name))

            End Try
        Else
            WarnBalloon("Please input your correct credentials.", "Invalid credentials", btnlogin, btnlogin.Width - 18, -69)

            txtbxUserID.Focus()

            err_count += 1 'increments the failed log in attempt

            'If (err_log_limit > err_count) = False Then 'failed log in attempt reaches its limit
            '    'prompts the log in failure
            '    MessageBox.Show("You have reached the failed login attempts." & Environment.NewLine & "Sorry, please retry later.",
            '                    "Failed login",
            '                    MessageBoxButtons.OK,
            '                    MessageBoxIcon.Stop)
            '    'then exits the application
            '    Me.Close()

            'End If

        End If
        btnlogin.Enabled = True
    End Sub

    Private Async Function CheckIfAuthorizedByUserLevel() As Threading.Tasks.Task(Of Boolean)
        Dim user = Await _userRepository.GetByIdAsync(z_User)

        If user Is Nothing Then

            MessageBoxHelper.ErrorMessage("User does not exists.")
            Return False
        End If

        Dim settings = ListOfValueCollection.Create()

        If settings.GetBoolean("User Policy.UseUserLevel", False) = False Then
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

                MessageBoxHelper.ErrorMessage("You are not authorized to access this organization.")
                Return False

            End If

        End If

        Return False

    End Function

    Function UserAuthentication(Optional pass_word As Object = Nothing)
        Dim n_ReadSQLFunction As New ReadSQLFunction("UserAuthentication",
                                                     "returnvaue",
                                                     New EncryptString(txtbxUserID.Text).ResultValue,
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

    Private Sub lnklblleave_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)

        Dim n_LeaveForm As New LeaveForm

        With n_LeaveForm

            .CboListOfValue1.Visible = False

            .Label3.Visible = False

            .Label4.Visible = False

            .Show()

        End With

    End Sub

    Private Sub lnklblovertime_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)

        Dim n_OverTimeForm As New OverTimeForm

        With n_OverTimeForm

            .cboOTStatus.Visible = False

            .Label186.Visible = False

            .Label4.Visible = False

            .Show()

        End With

    End Sub

    Private Sub lnklblobf_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)

        Dim n_OBFForm As New OBFForm

        With n_OBFForm

            .cboOBFStatus.Visible = False

            .Label186.Visible = False

            .Label4.Visible = False

            .Show()

        End With

    End Sub

End Class

Friend Class EncryptString

    Dim n_ResultValue = Nothing

    Property ResultValue As Object

        Get
            Return n_ResultValue

        End Get

        Set(value As Object)
            n_ResultValue = value

        End Set

    End Property

    Sub New(StringToEncrypt As String)

        n_ResultValue = EncrypedData(StringToEncrypt)

    End Sub

    Private Function EncrypedData(ByVal a As String)

        Dim Encryped = Nothing

        If Not a Is Nothing Then

            For Each x As Char In a

                Dim ToCOn As Integer = Convert.ToInt64(x) + 133

                Encryped &= Convert.ToChar(Convert.ToInt64(ToCOn))

            Next

        End If

        Return Encryped

    End Function

End Class