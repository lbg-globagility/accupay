Imports AccuPay.Data.Entities
Imports AccuPay.Data.Enums
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Interfaces
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class UsersForm

    Private isNew As Boolean = False

    Private dataSource As List(Of UserBoundItem)

    Private ReadOnly _policyHelper As PolicyHelper

    Private ReadOnly _userRepository As UserRepository

    Private ReadOnly _encryptor As IEncryption

    Sub New()

        InitializeComponent()

        _policyHelper = MainServiceProvider.GetRequiredService(Of PolicyHelper)

        _userRepository = MainServiceProvider.GetRequiredService(Of UserRepository)

        _encryptor = MainServiceProvider.GetRequiredService(Of IEncryption)

    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        OjbAssignNoContextMenu(cmbPosition)

        dgvUserList.AutoGenerateColumns = False

        MyBase.OnLoad(e)

    End Sub

    Private Async Sub FillUsers()
        Dim users = Await _userRepository.GetAllActiveWithPositionAsync()

        Dim source = users.Select(Function(u) New UserBoundItem(u))
        dataSource = source.ToList()
        'dataSource = New BindingList(Of UserBoundItem)
        'For Each s In source
        '    dataSource.Add(s)
        'Next

        dgvUserList.DataSource = dataSource

        DisplayValue()
    End Sub

    Private Sub DisplayValue()
        Dim user = GetUserBoundItem()
        If user Is Nothing Then Return

        With user
            txtUserName.Text = .UserID
            txtPassword.Text = _encryptor.Decrypt(.Password)
            txtConfirmPassword.Text = txtPassword.Text
            txtConfirmPassword.Tag = txtConfirmPassword.Text
            txtLastName.Text = .LastName
            txtFirstName.Text = .FirstName
            txtMiddleName.Text = .MiddleName
            txtEmailAdd.Text = .EmailAddress
            cmbPosition.Text = .PositionName
            cboxposition.SelectedValue = .PositionID

            UserLevelComboBox.SelectedIndex = .UserLevel
        End With

    End Sub

    Sub fillPosition()

        Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("SELECT p.RowID,p.PositionName" &
                                    " FROM position p INNER JOIN (SELECT GROUP_CONCAT(p.PositionName) `PositionName`" &
                                                                " FROM user u INNER JOIN position p ON p.RowID=u.PositionID) pp ON LOCATE(p.PositionName, pp.PositionName) = 0" &
                                    " WHERE p.OrganizationID=" & orgztnID &
                                    " UNION" &
                                    " SELECT p.RowID,p.PositionName FROM user u INNER JOIN position p ON p.RowID=u.PositionID WHERE u.RowID IS NOT NULL;") '='" & z_User & "'
        cboxposition.ValueMember = n_SQLQueryToDatatable.ResultTable.Columns(0).ColumnName
        cboxposition.DisplayMember = n_SQLQueryToDatatable.ResultTable.Columns(1).ColumnName
        cboxposition.DataSource = n_SQLQueryToDatatable.ResultTable

        Dim strQuery As String = "select PositionName from Position Where OrganizationID = '" & z_OrganizationID & "'"

        '"SELECT PositionName" & _
        '" FROM position" & _
        '" WHERE OrganizationID='" & orgztnID & "'" & _
        '" UNION" & _
        '" SELECT pos.PositionName" & _
        '" FROM USER u INNER JOIN POSITION pos ON pos.RowID=u.PositionID" & _
        '" WHERE u.OrganizationID = '" & orgztnID & "'" & _
        '" AND u.PositionID IS NOT NULL;"

        '_

        '"' ORDER BY PositionName AND RowID NOT IN (SELECT DISTINCT(PositionID) FROM employee WHERE PositionID IS NOT NULL AND OrganizationID=" & z_OrganizationID & ");"

        cmbPosition.Items.Clear()

        cmbPosition.Items.Add("")

        cmbPosition.Items.AddRange(CType(SQL_ArrayList(strQuery).ToArray(GetType(String)), String()))

        cmbPosition.SelectedIndex = 0

    End Sub

    Private Sub cleartextbox()
        txtConfirmPassword.Clear()
        txtPassword.Clear()
        txtEmailAdd.Clear()
        txtFirstName.Clear()
        txtLastName.Clear()
        txtMiddleName.Clear()
        cmbPosition.SelectedIndex = -1
        txtUserName.Clear()

        If UserLevelComboBox.Items.Count > 0 Then
            UserLevelComboBox.SelectedIndex = 0
        Else
            UserLevelComboBox.SelectedIndex = -1

        End If

    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        cleartextbox()
        btnNew.Enabled = False
        dgvUserList.Enabled = False

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        cleartextbox()
        btnNew.Enabled = True
        dgvUserList.Enabled = True

        Z_ErrorProvider.Dispose()

        btnCancel.Enabled = False
        FillUsers()
        btnCancel.Enabled = True
    End Sub

    Dim dontUpdate As SByte = 0

    Private Function ValidateRequiredFields() As Boolean
        Dim requiredControls = grpDetails.Controls.OfType(Of Control).
            Where(Function(e) TypeOf e Is TextBox Or TypeOf e Is ComboBox).
            Where(Function(e) Convert.ToString(e.Tag) = "Required").
            OrderBy(Function(e) e.TabIndex).
            ToList()

        Dim somethingToWorry = False

        Const warnMessage As String = "This field is required"

        For Each c In requiredControls
            Dim seemsEmpty = SetWarningIfEmpty(c, warnMessage) = False
            somethingToWorry = seemsEmpty

            If seemsEmpty Then
                SetWarning(c, warnMessage)
                Exit For
            Else
                SetWarningIfEmpty(c, String.Empty)
            End If
        Next

        Return somethingToWorry
    End Function

    Private Async Sub SaveUser_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Await FunctionUtils.TryCatchFunctionAsync("Save User",
            Async Function()
                Await SaveUser()
            End Function)

        btnSave.Enabled = True
    End Sub

    Private Async Function SaveUser() As Threading.Tasks.Task
        Dim enableSaveButton = Sub()
                                   btnSave.Enabled = True
                               End Sub
        Dim disableSaveButton = Sub()
                                    btnSave.Enabled = False
                                End Sub

        Dim usernameExistsAlready = Sub()
                                        Dim userIDErrorMessage = "User ID Already exist."
                                        myBalloon(userIDErrorMessage, "Save failed", lblSaveMsg, , -100)
                                        txtUserName.Focus()
                                        enableSaveButton()
                                    End Sub

        disableSaveButton()

        Dim hasWorry = ValidateRequiredFields()
        If hasWorry Then
            enableSaveButton()
            Return
        End If

        Dim passwordConfirmed = txtPassword.Text = txtConfirmPassword.Text

        If Not passwordConfirmed Then
            myBalloon("Password does not match.", "Save failed", lblSaveMsg, , -100)
            txtConfirmPassword.Focus()
            enableSaveButton()
            Return
        End If

        Dim dataService = MainServiceProvider.GetRequiredService(Of UserDataService)

        If isNew Then
            If Await _userRepository.CheckIfUsernameExistsAsync(txtUserName.Text) Then
                usernameExistsAlready()
                Return
            End If

            Dim newUser = User.NewUser(z_OrganizationID, z_User)

            ApplyChanges(newUser)
            Await dataService.CreateAsync(newUser)

            myBalloon("Successfully Save", "Saved", lblSaveMsg, , -100)

            FillUsers()

            btnNew.Enabled = True
            dgvUserList.Enabled = True
        Else
            Dim userBoundItem = GetUserBoundItem()
            If userBoundItem Is Nothing Then Return

            Dim user = Await _userRepository.GetByIdWithPositionAsync(userBoundItem.RowID)

            If _encryptor.Encrypt(txtUserName.Text) <> user.Username Then
                If Await _userRepository.CheckIfUsernameExistsAsync(txtUserName.Text) Then
                    usernameExistsAlready()
                    Return
                End If
            End If

            ApplyChanges(user)
            user.LastUpdBy = z_User
            Await dataService.UpdateAsync(user)

            myBalloon("Successfully Save", "Updated", lblSaveMsg, , -100)
        End If

        enableSaveButton()
    End Function

    Private Function GetUserBoundItem() As UserBoundItem
        Dim currentRow = dgvUserList.CurrentRow
        If currentRow Is Nothing Then Return Nothing
        Return DirectCast(currentRow.DataBoundItem, UserBoundItem)
    End Function

    Private Sub ApplyChanges(ByRef u As User)
        With u
            .Username = txtUserName.Text
            .Password = txtPassword.Text
            .LastName = txtLastName.Text
            .FirstName = txtFirstName.Text
            .MiddleName = txtMiddleName.Text
            .EmailAddress = txtEmailAdd.Text
            .PositionID = cboxposition.SelectedValue
            .UserLevel = UserLevelComboBox.SelectedIndex
        End With

        If isNew Then Return
        Dim userId = u.RowID

        Dim boundItem = dataSource.FirstOrDefault(Function(b) b.RowID = userId)
        With boundItem
            .UserID = txtUserName.Text
            .PositionName = u.Position.Name
            .LastName = u.LastName
            .FirstName = u.FirstName
            .MiddleName = u.MiddleName
            .EmailAddress = u.EmailAddress
            .UserLevel = u.UserLevel
            .UserLevelIndex = UserLevelHelper.GetUserLevelDescription(u.UserLevel)
            .Password = u.Password
            .PositionID = u.PositionID
            .RowID = u.RowID
        End With

        dgvUserList.DataSource = dataSource
        dgvUserList.Refresh()
    End Sub

    Private Sub SetWarning(textbox As Control, errorMessage As String)

        'MessageBoxHelper.ErrorMessage(errorMessage)
        textbox.Focus()

    End Sub

    Private Sub UsersForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            hintInfo.Dispose()
        Catch ex As Exception

        End Try

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        GeneralForm.listGeneralForm.Remove(Me.Name)

    End Sub

    Dim view_ID As Integer = Nothing

    Private Sub FillUserLevel()

        If _policyHelper.UseUserLevel Then

            UserLevelLabel.Visible = True
            UserLevelComboBox.Visible = True
            UserLevelDescriptionColumn.Visible = True

            UserPrivilegeLabel.Visible = False
            dgvPrivilege.Visible = False
        Else

            UserLevelLabel.Visible = False
            UserLevelComboBox.Visible = False
            UserLevelDescriptionColumn.Visible = False

            UserPrivilegeLabel.Visible = False
            dgvPrivilege.Visible = False

        End If

        UserLevelComboBox.DataSource = [Enum].GetValues(GetType(UserLevel)).Cast(Of UserLevel)().ToList

    End Sub

    Private Sub UsersForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FillUserLevel()
        fillPosition()
        FillUsers()

        view_ID = VIEW_privilege("Users", orgztnID)

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If formuserprivilege.Count = 0 Then

            btnNew.Visible = 0
            btnSave.Visible = 0
            btnDelete.Visible = 0
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    'ToolStripButton2.Visible = 0
                    btnNew.Visible = 0
                    btnSave.Visible = 0
                    btnDelete.Visible = 0
                    dontUpdate = 1
                    Exit For
                Else
                    If drow("Creates").ToString = "N" Then
                        btnNew.Visible = 0
                    Else
                        btnNew.Visible = 1
                    End If

                    If drow("Deleting").ToString = "N" Then
                        btnDelete.Visible = 0
                    Else
                        btnDelete.Visible = 1
                    End If

                    If drow("Updates").ToString = "N" Then
                        dontUpdate = 1
                    Else
                        dontUpdate = 0
                    End If

                End If

            Next

        End If

    End Sub

    Private Sub ToolStripButton12_Click(sender As Object, e As EventArgs) Handles ToolStripButton12.Click
        Me.Close()

    End Sub

    Private Sub dgvUserList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUserList.CellContentClick

    End Sub

    Private Sub dgvUserList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUserList.CellClick

        RemoveHandler txtPassword.TextChanged, AddressOf txtConfirmPassword_TextChanged

        RemoveHandler txtConfirmPassword.TextChanged, AddressOf txtConfirmPassword_TextChanged

        Try

            DisplayValue()
        Catch ex As Exception
        Finally

            'txtConfirmPassword.Tag = Nothing

            AddHandler txtPassword.TextChanged, AddressOf txtConfirmPassword_TextChanged

            AddHandler txtConfirmPassword.TextChanged, AddressOf txtConfirmPassword_TextChanged

        End Try

    End Sub

    Private Sub lblAddPosition_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblAddPosition.LinkClicked
        'Removed the old position form
        'When this button is visible again, add code to show the new add position form

    End Sub

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If MsgBox("Are you sure you want to remove this user?", MsgBoxStyle.YesNo, "Removing...") = MsgBoxResult.Yes Then
            btnDelete.Enabled = False

            Dim user = GetUserBoundItem()
            Dim dataService = MainServiceProvider.GetRequiredService(Of UserDataService)
            Await dataService.SoftDeleteAsync(user.RowID)

            dgvUserList.ClearSelection()

            Dim source = dataSource.Where(Function(u) Not u.RowID = user.RowID)
            dataSource = source.ToList()

            dgvUserList.DataSource = dataSource
            dgvUserList.Refresh()
        End If

        btnDelete.Enabled = True
    End Sub

    Private Sub dgvUserList_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvUserList.DataError
        e.ThrowException = False

    End Sub

    Private Sub cmbPosition_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbPosition.KeyPress

        e.Handled = True

    End Sub

    Private Sub cmbPosition_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPosition.SelectedIndexChanged

    End Sub

    Private Sub txtConfirmPassword_TextChanged(sender As Object, e As EventArgs) 'Handles txtConfirmPassword.TextChanged

        If txtConfirmPassword.Text.Trim = txtPassword.Text.Trim Then
            txtConfirmPassword.Tag = txtConfirmPassword.Text.Trim
        Else
            txtConfirmPassword.Tag = Nothing
        End If

    End Sub

    Private Sub btnNew_EnabledChanged(sender As Object, e As EventArgs) Handles btnNew.EnabledChanged
        isNew = Not btnNew.Enabled
    End Sub

    Private Class UserBoundItem

        Private ReadOnly _encryptor As IEncryption

        Public Sub New(u As User)

            _encryptor = MainServiceProvider.GetRequiredService(Of IEncryption)

            Dim userName As String = u.Username
            _UserID = Convert.ToString(_encryptor.Decrypt(userName))

            _PositionName = u.Position.Name
            _LastName = u.LastName
            _FirstName = u.FirstName
            _MiddleName = u.MiddleName
            _EmailAddress = u.EmailAddress
            _UserLevel = u.UserLevel
            _UserLevelIndex = UserLevelHelper.GetUserLevelDescription(u.UserLevel)
            _Password = u.Password
            _PositionID = u.PositionID
            _RowID = u.RowID
        End Sub

        Public Property UserID As String
        Public Property PositionName As String
        Public Property LastName As String
        Public Property FirstName As String
        Public Property MiddleName As String
        Public Property EmailAddress As String
        Public Property UserLevel As String
        Public Property UserLevelIndex As String
        Public Property Password As String
        Public Property PositionID As Integer
        Public Property RowID As Integer?
    End Class

End Class