Imports MySql.Data.MySqlClient

Public Class newPostion

    Protected Overrides Sub OnLoad(e As EventArgs)

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT '' AS RowID, '' AS PositionName" & _
                                                             " UNION" & _
                                                             " SELECT RowID,PositionName" & _
                                                             " FROM position" & _
                                                             " WHERE OrganizationID='" & orgztnID & "';")

        With n_SQLQueryToDatatable.ResultTable

            cboParentPosit.ValueMember = .Columns(0).ColumnName

            cboParentPosit.DisplayMember = .Columns(1).ColumnName

            cboParentPosit.DataSource = n_SQLQueryToDatatable.ResultTable

        End With

        n_SQLQueryToDatatable = New SQLQueryToDatatable("SELECT '' AS RowID, '' AS Name" & _
                                                             " UNION" & _
                                                             " SELECT RowID,Name" & _
                                                             " FROM `division`" & _
                                                             " WHERE OrganizationID='" & orgztnID & "';")


        With n_SQLQueryToDatatable.ResultTable

            cboDivis.ValueMember = .Columns(0).ColumnName

            cboDivis.DisplayMember = .Columns(1).ColumnName

            cboDivis.DataSource = n_SQLQueryToDatatable.ResultTable

        End With

        MyBase.OnLoad(e)

    End Sub

    Private Sub newPostion_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'dbconn() ': Me.Parent = MDIParent1

        cboParentPosit.ContextMenu = New ContextMenu

        txtPositName.ContextMenu = New ContextMenu

        'cboParentPosit.Items.Clear() ': Employee.Enabled = False

        'For Each r In Employee.positn
        '    cboParentPosit.Items.Add(StrReverse(getStrBetween(StrReverse(r), "", "@")))
        'Next



        'cboDivis.Items.Clear()

        'enlistToCboBox("SELECT Name FROM division WHERE OrganizationID='" & orgztnID & "' ORDER BY Name;", _
        '               cboDivis)

    End Sub

    Dim n_PositionRowID = String.Empty

    Property PositionRowID As String

        Get
            Return n_PositionRowID

        End Get

        Set(value As String)
            n_PositionRowID = value

        End Set

    End Property

    Dim n_PositionName = String.Empty

    Property PositionName As String

        Get
            Return n_PositionName

        End Get

        Set(value As String)
            n_PositionName = value

        End Set

    End Property

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        txtPositName.Text = Employee.strTrimProper(txtPositName.Text)

        n_PositionName = txtPositName.Text

        If cboDivis.SelectedIndex < 0 Then
            txtPositName.Focus()
            WarnBalloon("Please input a Division Name.", "Invalid Division Name", cboDivis, cboDivis.Width - 16, -69) : Exit Sub
        ElseIf txtPositName.Text = "" Then
            txtPositName.Focus()
            WarnBalloon("Please input a Position Name.", "Invalid Position Name", txtPositName, txtPositName.Width - 16, -69) : Exit Sub
        ElseIf EXECQUER("SELECT EXISTS(SELECT RowID FROM position WHERE PositionName='" & txtPositName.Text & "' AND OrganizationID=" & orgztnID & ")") = 1 Then
            txtPositName.Focus()
            WarnBalloon("Position Name '" & txtPositName.Text & "' has already exists. Please try another.", "Invalid Position Name", txtPositName, txtPositName.Width - 16, -69) : Exit Sub

        End If


        Dim _rowID = ""

        'If Trim(cboParentPosit.Text) = "" Then
        '    n_PositionRowID = INS_position(txtPositName.Text)
        'Else
        '    Dim i = cboParentPosit.SelectedIndex
        '    'Dim parposID = getStrBetween(Employee.positn.Item(i), "", "@")
        n_PositionRowID = INSUPD_position(,
                                        txtPositName.Text,
                                        cboParentPosit.SelectedValue,
                                        cboDivis.SelectedValue)
        'End If

        'Employee.positn.Add(_rowID & "@" & Trim(txtPositName.Text))
        'Employee.cboPosit.Items.Add(Trim(txtPositName.Text))
        'Employee.cboPosit.Text = Trim(txtPositName.Text)

        Button2_Click(Button2, New EventArgs)

    End Sub

    Function INSUPD_position(Optional pos_RowID As Object = Nothing, _
                             Optional pos_PositionName As Object = Nothing, _
                             Optional pos_ParentPositionID As Object = Nothing, _
                             Optional pos_DivisionId As Object = Nothing) As Object
        Dim return_value = Nothing
        Try
            If conn.State = ConnectionState.Open Then : conn.Close() : End If

            cmd = New MySqlCommand("INSUPD_position", conn)

            conn.Open()

            With cmd
                .Parameters.Clear()

                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("positID", MySqlDbType.Int32)

                .Parameters.AddWithValue("pos_RowID", If(pos_RowID = Nothing, DBNull.Value, pos_RowID))
                .Parameters.AddWithValue("pos_PositionName", Trim(pos_PositionName))
                .Parameters.AddWithValue("pos_CreatedBy", z_User)
                .Parameters.AddWithValue("pos_OrganizationID", orgztnID)
                .Parameters.AddWithValue("pos_LastUpdBy", z_User)
                .Parameters.AddWithValue("pos_ParentPositionID", If(pos_ParentPositionID = Nothing, DBNull.Value, pos_ParentPositionID))
                .Parameters.AddWithValue("pos_DivisionId", If(pos_DivisionId = Nothing, DBNull.Value, pos_DivisionId))

                .Parameters("positID").Direction = ParameterDirection.ReturnValue

                Dim datread As MySqlDataReader

                datread = .ExecuteReader()
                'INSUPD_position
                return_value = datread(0)

            End With
        Catch ex As Exception
            MsgBox(ex.Message & " " & "INSUPD_position", , "Error")
        Finally
            conn.Close()
            cmd.Dispose()
        End Try
        Return return_value
    End Function

    Private Sub newPostion_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If ValNoComma(n_PositionRowID) = 0 Then

            Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Else

            Me.DialogResult = Windows.Forms.DialogResult.OK

        End If

        WarnBalloon(, , txtPositName, , , 1) : Employee.Enabled = True

        WarnBalloon(, , cboDivis, , , 1)

    End Sub

    Private Sub cboParentPosit_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboParentPosit.KeyPress
        Dim e_asc As String = Asc(e.KeyChar)
        If e_asc = 8 Then
            e.Handled = False
            cboParentPosit.SelectedIndex = -1
        Else : e.Handled = True
        End If
    End Sub

    Private Sub cboParentPosit_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboParentPosit.SelectedIndexChanged

    End Sub

    Private Sub txtPositName_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPositName.KeyDown, cboParentPosit.KeyDown, Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button1_Click(sender, e)
        ElseIf e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Dim divisionRowID = Nothing

    Private Sub cboDivis_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDivis.SelectedIndexChanged

        If cboDivis.SelectedIndex = -1 Then

            divisionRowID = Nothing

        Else

            divisionRowID = EXECQUER("SELECT RowID FROM division WHERE OrganizationID='" & orgztnID & "' ORDER BY Name LIMIT " & cboDivis.SelectedIndex & ",1;")

        End If

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            Button2_Click(Button2, New EventArgs)

            Return True

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Dim isShowAsDialog = False

    Public Overloads Function ShowDialog(ByVal someValue As String) As DialogResult
        ' ...do something with "someValue"...
        With Me
            isShowAsDialog = True

            .Text = someValue

            .FormBorderStyle = Windows.Forms.FormBorderStyle.FixedDialog

            .StartPosition = FormStartPosition.CenterScreen

            .MinimizeBox = False

            .MaximizeBox = False

            '.Width = 375

            '.Height += 30

            'dgvAddress.Anchor = AnchorStyles.None

            'dgvAddress.Location = New Point(25, 173)

            'dgvAddress.Height -= 30

            '398,368

        End With

        Return Me.ShowDialog

    End Function

End Class