Imports AccuPay.Data.Services
Imports AccuPay.Utils

Public Class UsersForm
    Dim isNew As Integer = 0
    Dim rowid As Integer

    Protected Overrides Sub OnLoad(e As EventArgs)

        OjbAssignNoContextMenu(cmbPosition)

        MyBase.OnLoad(e)

    End Sub

    Private Sub fillUsers()

        Try
            Dim dt As New DataTable
            dt = getDataTableForSQL("Select u.RowID, u.UserID, p.PositionName, u.LastName, u.Firstname, u.MiddleName, u.RowID, u.EmailAddress, u.UserLevel from User u " &
                                    " inner join Position p on u.PositionID = p.RowID WHERE u.Status <> 'Inactive' ORDER BY u.Rowid ASC;")

            'Where u.OrganizationID = '" & Z_OrganizationID & "' And Status = 'Active'

            dgvUserList.Rows.Clear()
            If dt.Rows.Count > 0 Then
                For Each drow As DataRow In dt.Rows
                    Dim n As Integer = dgvUserList.Rows.Add()
                    With drow
                        Try
                            dgvUserList.Rows.Item(n).Cells(c_userid.Index).Value = DecrypedData(.Item("UserID").ToString)
                            dgvUserList.Rows.Item(n).Cells(c_Position.Index).Value = .Item("PositionName").ToString
                            dgvUserList.Rows.Item(n).Cells(c_lname.Index).Value = .Item("LastName").ToString
                            dgvUserList.Rows.Item(n).Cells(c_fname.Index).Value = .Item("FirstName").ToString
                            dgvUserList.Rows.Item(n).Cells(c_Mname.Index).Value = .Item("MiddleName").ToString
                            dgvUserList.Rows.Item(n).Cells(c_rowid.Index).Value = .Item("RowID").ToString
                            dgvUserList.Rows.Item(n).Cells(c_emailadd.Index).Value = .Item("EmailAddress").ToString
                            dgvUserList.Rows.Item(n).Cells(UserLevelColumn.Index).Value = .Item("UserLevel")
                            dgvUserList.Rows.Item(n).Cells(UserLevelDescriptionColumn.Index).Value = UserLevelHelper.GetUserLevelDescription(.Item("UserLevel"))
                            'dgvUserList.Rows.Item(n).Cells(5).Value = .Item("PositionID").ToString
                        Catch ex As Exception

                        End Try

                    End With
                Next
            End If

            If dgvUserList.Rows.Count = 0 Then
            Else
                DisplayValue(dgvUserList.CurrentRow.Cells(c_rowid.Index).Value)

            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error code fillUsers")
        End Try
    End Sub

    Private Function DisplayValue(ByVal UserID As Integer) As Boolean
        Try
            txtPassword.Clear()
            txtConfirmPassword.Clear()
            If dgvUserList.Rows.Count = 0 Then
            Else
                Dim dt As New DataTable
                dt = getDataTableForSQL("select * from user where RowID = " & UserID & ";") ' And OrganizationID = " & z_OrganizationID & "
                For Each drow As DataRow In dt.Rows
                    With drow
                        Dim posID As Integer = .Item("PositionID").ToString

                        Dim n_SQLQueryToDatatable As _
                            New SQLQueryToDatatable(
                                String.Concat("CALL USER_dropdownposition(", orgztnID, ",", UserID, ");"))
                        cboxposition.ValueMember = n_SQLQueryToDatatable.ResultTable.Columns(0).ColumnName
                        cboxposition.DisplayMember = n_SQLQueryToDatatable.ResultTable.Columns(1).ColumnName
                        cboxposition.DataSource = n_SQLQueryToDatatable.ResultTable

                        Dim postname As String = getStringItem("Select PositionName from position where rowID = '" & posID & "'")
                        Dim getpostname As String = postname
                        txtUserName.Text = DecrypedData(.Item("UserID").ToString)
                        txtPassword.Text = DecrypedData(.Item("Password").ToString)
                        txtConfirmPassword.Text = txtPassword.Text
                        txtConfirmPassword.Tag = txtConfirmPassword.Text
                        txtLastName.Text = .Item("LastName").ToString
                        txtFirstName.Text = .Item("FirstName").ToString
                        txtMiddleName.Text = .Item("Middlename").ToString
                        txtEmailAdd.Text = .Item("EmailAddress").ToString
                        cmbPosition.Text = getpostname
                        cboxposition.SelectedValue = posID

                        UserLevelComboBox.SelectedIndex = .Item("UserLevel")

                        rowid = .Item("RowID")

                    End With

                Next

            End If

            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error code DisplayValue")
        End Try
        Return True
    End Function

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
        isNew = 1

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        cleartextbox()
        btnNew.Enabled = True
        dgvUserList.Enabled = True
        Z_ErrorProvider.Dispose()
    End Sub

    Dim dontUpdate As SByte = 0

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If isNew = 1 Then
            Z_ErrorProvider.Dispose()

            If txtLastName.Text.Trim.Length = 0 Or txtFirstName.Text.Trim.Length = 0 Or txtUserName.Text.Trim.Length = 0 Or txtPassword.Text.Trim.Length = 0 _
                Or txtConfirmPassword.Text.Trim.Length = 0 Or cboxposition.Text.Trim.Length = 0 Then
                If Not SetWarningIfEmpty(txtLastName) And SetWarningIfEmpty(txtFirstName) And SetWarningIfEmpty(txtUserName) And
                    SetWarningIfEmpty(txtPassword) And SetWarningIfEmpty(txtConfirmPassword) And SetWarningIfEmpty(cboxposition) Then

                End If
            Else

                Dim position As String = getStringItem("Select RowID From Position Where PositionName = '" & cboxposition.Text & "' And OrganizationID = " & z_OrganizationID & "")
                Dim getposition As Integer = Val(position) 'ValNoComma(cboxposition.SelectedValue)

                Dim status As String = "Active"
                Dim userid As String = getStringItem("Select UserID from user Where UserID = '" & EncrypedData(txtUserName.Text) & "' AND OrganizationID = '" & z_OrganizationID & "'")
                Dim getuserid As String = userid
                If getuserid = EncrypedData(txtUserName.Text) Then
                    SetWarning(txtUserName, "User ID Already exist.")
                    'myBalloonWarn("User ID Already exist.", "Duplicate", txtUserName, , -65)
                Else
                    If txtPassword.Text = txtConfirmPassword.Text Then
                        I_UsersProc(txtLastName.Text,
                                    txtFirstName.Text,
                                    txtMiddleName.Text,
                               EncrypedData(txtUserName.Text),
                                    EncrypedData(txtConfirmPassword.Text),
                               z_OrganizationID,
                                    getposition,
                                    Date.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                               z_User,
                                    z_User,
                                    Date.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                    status,
                                    txtEmailAdd.Text,
                                    UserLevelComboBox.SelectedIndex)
                        'audittrail(

                        myBalloon("Successfully Save", "Saved", lblSaveMsg, , -100)
                        fillUsers()
                        btnNew.Enabled = True
                        dgvUserList.Enabled = True
                        isNew = 0
                    Else
                        SetWarning(txtConfirmPassword, "Password does not match.")
                        'myBalloonWarn("Password does not match", "Not Match", txtConfirmPassword, , -65)
                    End If

                End If

            End If
        Else

            If txtConfirmPassword.Tag = Nothing Then
                SetWarning(txtConfirmPassword, "Password mismatch.")
                'myBalloonWarn("Password mismatch", "Incorrect data", txtConfirmPassword, , -65)
            Else

                Dim enc_userid = New EncryptData(txtUserName.Text.Trim).ResultValue

                Dim enc_pword = New EncryptData(txtConfirmPassword.Tag).ResultValue

                If dontUpdate = 1 Then
                    Exit Sub
                End If

                Dim position As String = getStringItem("SELECT RowID FROM Position WHERE PositionName = '" & cmbPosition.Text & "' AND OrganizationID = " & z_OrganizationID & ";")

                Dim getposition As Integer = 0 'Val(position)

                'If getposition = 0 Then

                '    Dim position_count As String = getStringItem("Select COUNT(RowID) From Position Where PositionName = '" & cmbPosition.Text & "';")

                '    If position_count <> 0 Then

                '        If position_count = 1 Then

                '            getposition = getStringItem("Select RowID From Position Where PositionName = '" & cmbPosition.Text & "';")

                '        ElseIf position_count > 1 Then

                '            getposition = getStringItem("SELECT PositionID FROM user WHERE RowID = '" & z_User & "';")

                '        End If

                '    End If

                'End If
                getposition = cboxposition.SelectedValue
                Dim status As String = "Active"
                U_UsersProc(Val(dgvUserList.CurrentRow.Cells(c_rowid.Index).Value),
                                        txtLastName.Text,
                                        txtFirstName.Text,
                                        txtMiddleName.Text,
                                        getposition,
                                        Today.Date,
                                        z_User,
                                        z_User,
                                        Today.Date,
                                        status,
                                        txtEmailAdd.Text,
                                        enc_userid,
                                        enc_pword,
                                        UserLevelComboBox.SelectedIndex)

                'SetBalloonTip("Updated", "Successfully Save.")
                myBalloon("Successfully Save", "Updated", lblSaveMsg, , -100)
                fillUsers()

            End If

        End If

    End Sub

    Private Sub SetWarning(textbox As Control, errorMessage As String)

        MessageBoxHelper.ErrorMessage(errorMessage)
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

        'If FormLeft.Contains("Users") Then
        '    FormLeft.Remove("Users")
        'End If

        'If FormLeft.Count = 0 Then
        '    MDIPrimaryForm.Text = "Welcome"
        'Else
        '    MDIPrimaryForm.Text = "Welcome to " & FormLeft.Item(FormLeft.Count - 1)
        'End If
        GeneralForm.listGeneralForm.Remove(Me.Name)

    End Sub

    Dim view_ID As Integer = Nothing

    Private Sub FillUserLevel()

        Dim settings = ListOfValueCollection.Create()

        If settings.GetBoolean("User Policy.UseUserLevel", False) Then

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
        fillUsers()

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

    Private Sub dgvUserList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUserList.CellClick

        RemoveHandler txtPassword.TextChanged, AddressOf txtConfirmPassword_TextChanged

        RemoveHandler txtConfirmPassword.TextChanged, AddressOf txtConfirmPassword_TextChanged

        Try

            DisplayValue(dgvUserList.CurrentRow.Cells(c_rowid.Index).Value)
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

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try

            If MsgBox("Are you sure you want to remove this user?", MsgBoxStyle.YesNo, "Removing...") = MsgBoxResult.Yes Then

                DirectCommand("Update user Set Status = 'Inactive' Where RowID = '" & dgvUserList.CurrentRow.Cells(c_rowid.Index).Value & "'")
                fillUsers()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error code btnDelete_Click")
        End Try
    End Sub

    Private Sub tsAuditTrail_Click(sender As Object, e As EventArgs) Handles tsAuditTrail.Click
        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(view_ID)

        showAuditTrail.BringToFront()

    End Sub

    Private Sub cmbPosition_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbPosition.KeyPress

        e.Handled = True

    End Sub

    Private Sub cmbPosition_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPosition.SelectedIndexChanged

    End Sub

    Private Sub dgvUserList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUserList.CellContentClick

    End Sub

    Private Sub txtConfirmPassword_TextChanged(sender As Object, e As EventArgs) 'Handles txtConfirmPassword.TextChanged

        If txtConfirmPassword.Text.Trim = txtPassword.Text.Trim Then
            txtConfirmPassword.Tag = txtConfirmPassword.Text.Trim
        Else
            txtConfirmPassword.Tag = Nothing
        End If

    End Sub

End Class