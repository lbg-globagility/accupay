Imports Femiani.Forms.UI.Input
Imports MySql.Data.MySqlClient

Public Class EmpPosition

    Public ReadOnly Property ViewIdentification As Object
        Get
            Return view_ID
        End Get
    End Property

    Public ShowMeAsDialog As Boolean = False

    Public q_employee As String = "SELECT e.RowID," &
        "e.EmployeeID 'Employee ID'," &
        "e.FirstName 'First Name'," &
        "e.MiddleName 'Middle Name'," &
        "e.LastName 'Last Name'," &
        "e.Surname," &
        "e.Nickname," &
        "e.MaritalStatus 'Marital Status'," &
        "COALESCE(e.NoOfDependents,0) 'No. Of Dependents'," &
        "COALESCE(DATE_FORMAT(e.Birthdate,'%m/%d/%Y'),'') 'Birthdate'," &
        "COALESCE(DATE_FORMAT(e.Startdate,'%m/%d/%Y'),'') 'Startdate'," &
        "e.JobTitle 'Job Title'," &
        "COALESCE(pos.PositionName,'') 'Position'," &
        "e.Salutation," &
        "e.TINNo 'TIN'," &
        "e.SSSNo 'SSS No.'," &
        "e.HDMFNo 'PAGIBIG No.'," &
        "e.PhilHealthNo 'PhilHealth No.'," &
        "e.WorkPhone 'Work Phone No.'," &
        "e.HomePhone 'Home Phone No.'," &
        "e.MobilePhone 'Mobile Phone No.'," &
        "e.HomeAddress 'Home address'," &
        "e.EmailAddress 'Email address'," &
        "COALESCE(IF(e.Gender='M','Male','Female'),'') 'Gender'," &
        "e.EmploymentStatus 'Employment Status'," &
        "IFNULL(pf.PayFrequencyType,'') 'Pay Frequency'," &
        "e.UndertimeOverride," &
        "e.OvertimeOverride," &
        "COALESCE(pos.RowID,'') 'PositionID'" &
        ",IFNULL(e.PayFrequencyID,'') 'PayFrequencyID'" &
        ",e.EmployeeType" &
        ",e.LeaveBalance" &
        ",e.SickLeaveBalance" &
        ",e.MaternityLeaveBalance" &
        ",e.LeaveAllowance" &
        ",e.SickLeaveAllowance" &
        ",e.MaternityLeaveAllowance" &
        ",e.LeavePerPayPeriod" &
        ",e.SickLeavePerPayPeriod" &
        ",e.MaternityLeavePerPayPeriod" &
        ",COALESCE(fstat.RowID,3) 'fstatRowID'" &
        ",'' 'Image'" &
        ",DATE_FORMAT(e.Created,'%m/%d/%Y') 'Creation Date'," &
        "CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2))) 'Created by'," &
        "COALESCE(DATE_FORMAT(e.LastUpd,'%m/%d/%Y'),'') 'Last Update'," &
        "(SELECT CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2)))  FROM user WHERE RowID=e.LastUpdBy) 'LastUpdate by'" &
        " " &
        "FROM employee e " &
        "LEFT JOIN user u ON e.CreatedBy=u.RowID " &
        "LEFT JOIN position pos ON e.PositionID=pos.RowID " &
        "LEFT JOIN payfrequency pf ON e.PayFrequencyID=pf.RowID " &
        "LEFT JOIN filingstatus fstat ON fstat.MaritalStatus=e.MaritalStatus AND fstat.Dependent=e.NoOfDependents " &
        "WHERE e.OrganizationID=" & orgztnID

    Dim positiontable As New DataTable

    Dim alphaposition As New DataTable

    Dim divisiontable As New DataTable

    Dim alphadivision As New DataTable

    Dim view_ID As Integer = VIEW_privilege("Position", orgztnID)

    Private Sub EmpPosition_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        InfoBalloon(, , lblforballoon, , , 1)

        WarnBalloon(, , cboDivis, , , 1)

        showAuditTrail.Close()

        If ShowMeAsDialog Then
            UsersForm.fillPosition()
        Else

            HRISForm.listHRISForm.Remove(Me.Name)

        End If

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        reload()

        For Each drow As DataRow In alphadivision.Rows

            Divisiontreeviewfiller(drow("RowID"), drow("Name"))

        Next

        tv2.ExpandAll()

        AddHandler tv2.AfterSelect, AddressOf tv2_AfterSelect

        If tv2.Nodes.Count <> 0 Then
            tv2_AfterSelect(sender, New TreeViewEventArgs(tv2.Nodes.Item(0)))
        End If

        If view_ID = Nothing Then
            view_ID = VIEW_privilege("Position", orgztnID)
        End If

        Dim formuserprivilege = position_view_table.Select("ViewID = " & view_ID)

        If formuserprivilege.Count = 0 Then

            tsbtnNewPosition.Visible = 0
            tsbtnSavePosition.Visible = 0
            tsbtnDeletePosition.Visible = 0

            dontUpdate = 1
        Else
            For Each drow In formuserprivilege
                If drow("ReadOnly").ToString = "Y" Then
                    'ToolStripButton2.Visible = 0
                    tsbtnNewPosition.Visible = 0
                    tsbtnSavePosition.Visible = 0
                    tsbtnDeletePosition.Visible = 0
                    dontUpdate = 1
                    Exit For
                Else
                    If drow("Creates").ToString = "N" Then
                        tsbtnNewPosition.Visible = 0
                    Else
                        tsbtnNewPosition.Visible = 1
                    End If

                    If drow("Deleting").ToString = "N" Then
                        tsbtnDeletePosition.Visible = 0
                    Else
                        tsbtnDeletePosition.Visible = 1
                    End If

                    If drow("Updates").ToString = "N" Then
                        dontUpdate = 1
                    Else
                        dontUpdate = 0
                    End If

                End If

            Next

        End If

        bgworkautcompsearch.RunWorkerAsync()

    End Sub

    Sub reload()

        positiontable = New SQLQueryToDatatable("SELECT p.*" &
                                    ",COALESCE((SELECT CONCAT('(',FirstName,IF(COALESCE(MiddleName,'')='','',CONCAT(' ',LEFT(MiddleName,1))),IF(LastName IS NULL,'',CONCAT(' ',LastName)),')') FROM employee WHERE OrganizationID=" & orgztnID & " AND PositionID=p.RowID AND TerminationDate IS NULL LIMIT 1),'(Open)') 'positionstats'" &
                                    ",d.RowID AS DivRowID" &
                                    " FROM position p" &
                                    " LEFT JOIN `division` d ON d.RowID=p.DivisionId" &
                                    " WHERE p.OrganizationID=" & orgztnID & ";").ResultTable

        alphaposition = New SQLQueryToDatatable("SELECT p.*" &
                                                ",d.RowID AS DivRowID" &
                                                " FROM position p" &
                                                " INNER JOIN `division` d ON d.RowID=p.DivisionId" &
                                                " WHERE p.OrganizationID=" & orgztnID & "" &
                                                " AND p.ParentPositionID IS NULL;").ResultTable

        divisiontable = New SQLQueryToDatatable("SELECT d.*" &
                                                ",dd.RowID AS DivLocID" &
                                                " FROM `division` d" &
                                                " INNER JOIN `division` dd ON dd.RowID=d.ParentDivisionID" &
                                                " WHERE d.OrganizationID=" & orgztnID & ";").ResultTable

        alphadivision = New SQLQueryToDatatable("SELECT * FROM division WHERE OrganizationID=" & orgztnID & " AND ParentDivisionID IS NULL;").ResultTable

        cboDivLoc.DisplayMember = alphadivision.Columns(1).ColumnName
        cboDivLoc.ValueMember = alphadivision.Columns(0).ColumnName
        cboDivLoc.DataSource = alphadivision

        tv2.Nodes.Clear()

    End Sub

    Sub Positiontreeviewfiller(Optional primkey As Object = Nothing,
                       Optional strval As Object = Nothing,
                       Optional trnod As TreeNode = Nothing,
                       Optional tree_view As TreeView = Nothing)

        Dim n_nod As TreeNode = Nothing

        If trnod Is Nothing Then
            If tree_view Is Nothing Then
                n_nod = tv2.Nodes.Add(primkey, strval)
            Else
                n_nod = tree_view.Nodes.Add(primkey, strval)
            End If
        Else
            n_nod = trnod.Nodes.Add(primkey, strval)
            Dim selpos = positiontable.Select("RowID=" & primkey)
            For Each posrow In selpos
                n_nod.Tag = posrow("DivRowID")
                Exit For
            Next
        End If

        Dim selchild = positiontable.Select("ParentPositionID=" & primkey)

        For Each drow In selchild
            Positiontreeviewfiller(drow("RowID"), drow("PositionName") & drow("positionstats"), n_nod)
        Next

    End Sub

    Sub Divisiontreeviewfiller(Optional primkey As Object = Nothing,
                       Optional strval As Object = Nothing,
                       Optional trnod As TreeNode = Nothing)

        Dim n_nod As TreeNode = Nothing

        If trnod Is Nothing Then
            strval = strval & "[Location]"
            n_nod = tv2.Nodes.Add(primkey, strval, 4)
        Else
            strval = strval & "[Department]"
            n_nod = trnod.Nodes.Add(primkey, strval, 4)
            Dim selrow = alphadivision.Select("RowID=" & trnod.Name)
            For Each nodrow In selrow
                n_nod.Tag = nodrow("RowID")
                Exit For
            Next
        End If

        Dim selchild = divisiontable.Select("ParentDivisionID=" & primkey)

        Dim selchildposition = positiontable.Select("DivisionId=" & primkey & " AND ParentPositionID IS NULL")

        For Each p_drow In selchildposition
            Positiontreeviewfiller(p_drow("RowID"), p_drow("PositionName") & p_drow("positionstats"), n_nod, tv2)

        Next

        For Each drow In selchild

            Divisiontreeviewfiller(drow("RowID"), drow("Name"), n_nod)

        Next

    End Sub

    Sub DivisonPosition(Optional primkey As Object = Nothing,
                       Optional strval As Object = Nothing,
                       Optional trnod As TreeNode = Nothing)

        Dim n_nod As TreeNode = Nothing

        If trnod Is Nothing Then
            n_nod = tv2.Nodes.Add(primkey, strval)
        Else
            n_nod = trnod.Nodes.Add(primkey, strval)
        End If

        Dim selchild = positiontable.Select("DivisionId=" & primkey)

        For Each drow In selchild
            DivisonPosition(drow("RowID"), drow("PositionName"), n_nod)

        Next

    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click

        RemoveHandler tv2.AfterSelect, AddressOf tv2_AfterSelect

        reload()

        For Each drow As DataRow In alphadivision.Rows

            Divisiontreeviewfiller(drow("RowID"), drow("Name"), )

        Next

        tv2.ExpandAll()

        AddHandler tv2.AfterSelect, AddressOf tv2_AfterSelect

        If tv2.Nodes.Count <> 0 Then
            tv2_AfterSelect(sender, New TreeViewEventArgs(tv2.Nodes.Item(0)))
        End If

    End Sub

    Private Sub tv2_BeforeSelect(sender As Object, e As TreeViewCancelEventArgs) Handles tv2.BeforeSelect

        Try
            currentNode = e.Node
        Catch ex As Exception
            currentNode = Nothing
            MsgBox(getErrExcptn(ex, Me.Name))
        Finally

        End Try

    End Sub

    Dim currentNode As TreeNode = Nothing

    Dim selPositionID As Object = Nothing

    Private Sub tv2_AfterSelect(sender As Object, e As TreeViewEventArgs) 'Handles tv2.AfterSelect
        cboDivis.SelectedIndex = -1
        cboParentPosit.Text = ""
        txtPositName.Text = ""

        selPositionID = Nothing
        dgvemployees.Rows.Clear()

        RemoveHandler cboDivis.SelectedIndexChanged, AddressOf cboDivis_SelectedIndexChanged

        tsbtnDeletePosition.Enabled = False

        If tv2.Nodes.Count = 0 Then
            currentNode = Nothing

            cboParentPosit.Items.Clear()

        ElseIf currentNode IsNot Nothing Then
            If currentNode.Text.Contains("[") Then 'Node is a division

                Dim usethisvalue = Nothing

                If currentNode.Tag = Nothing Then
                    usethisvalue = currentNode.Name
                Else
                    usethisvalue = currentNode.Tag
                End If

                cboDivLoc.SelectedValue = usethisvalue

                Dim parentposition = divisiontable.Select("ParentDivisionID = " & usethisvalue)

                parentposition = divisiontable.Select("RowID=" & currentNode.Name)

                For Each strval In parentposition
                    cboDivis.Text = strval("Name").ToString
                    Exit For
                Next
            Else '                                  'Node is a position

                Dim priornod = currentNode.Parent

                Dim prior_node As Integer = ValNoComma(priornod.Tag)

                cboDivLoc.SelectedValue = prior_node

                Dim sel_divisiontable = divisiontable.Select("ParentDivisionID = " & prior_node)

                tsbtnDeletePosition.Enabled = True

                Dim parentPositRowID = Nothing

                Dim parent_node = currentNode.Parent

                Dim notthisPosition = Nothing

                If parent_node.Text.Contains("[") = 0 Then 'Parent Position
                    notthisPosition = parent_node.Name
                Else
                    notthisPosition = currentNode.Name
                End If

                Dim parentposition = positiontable.Select("RowID<>" & notthisPosition, "PositionName ASC")

                If parentposition.Count <> 0 Then
                    cboParentPosit.Items.Clear()
                End If

                For Each strval In parentposition
                    cboParentPosit.Items.Add(strval("PositionName").ToString)
                Next

                If parent_node.Text.Contains("[") Then
                Else
                    parentposition = positiontable.Select("RowID=" & parent_node.Name)

                    parentPositRowID = Nothing
                    For Each strval In parentposition
                        cboParentPosit.Text = strval("PositionName").ToString
                        parentPositRowID = strval("RowID").ToString
                        Exit For
                    Next

                End If

                Dim selposition = positiontable.Select("RowID=" & currentNode.Name) 'Position

                For Each strval In selposition
                    txtPositName.Text = strval("PositionName").ToString

                    cboParentPosit.Items.Remove(Trim(txtPositName.Text))

                    selPositionID = strval("RowID").ToString

                    loademployee(selPositionID)

                    Exit For
                Next

                If cboParentPosit.Text = "" Then

                    Dim posit_divis = positiontable.Select("RowID = " & selPositionID)

                    Dim divisRowID = Nothing

                    For Each drow In posit_divis
                        divisRowID = drow("DivisionId").ToString
                    Next

                    If divisRowID = Nothing Then
                    Else
                        Dim divistab = divisiontable.Select("RowID = " & divisRowID)

                        For Each strval In divistab
                            cboDivis.Text = strval("Name").ToString
                            Exit For
                        Next

                    End If
                Else

                    If parentPositRowID = Nothing Then
                    Else
                        rootParentPosition(parentPositRowID)

                        parentPositRowID = position_rowID

                        Dim posit_divis = positiontable.Select("RowID = " & parentPositRowID)

                        Dim divisRowID = Nothing

                        For Each drow In posit_divis
                            rootParentDivision(drow("DivisionId").ToString)
                        Next

                        divisRowID = division_rowID

                        If divisRowID = Nothing Then
                        Else
                            Dim divistab = divisiontable.Select("RowID = " & divisRowID)

                            For Each strval In divistab
                                cboDivis.Text = strval("Name").ToString
                                Exit For
                            Next

                        End If

                    End If

                End If

            End If

        End If

        AddHandler cboDivis.SelectedIndexChanged, AddressOf cboDivis_SelectedIndexChanged

    End Sub

    Dim division_rowID As Object = Nothing

    Function rootParentDivision(Optional division_ID As Object = Nothing) As Object
        If division_ID = Nothing Then
        Else
            Dim divistab = divisiontable.Select("RowID = " & division_ID)

            For Each strval In divistab
                If IsDBNull(strval("ParentDivisionID")) Then
                    division_rowID = division_ID 'division_rowID
                    Exit For
                Else
                    division_rowID = division_ID 'strval("ParentDivisionID")
                    Exit For
                End If
            Next
        End If

        Return division_rowID
    End Function

    Dim position_rowID As Object = Nothing

    Function rootParentPosition(Optional position_ID As Object = Nothing) As Object

        If position_ID = Nothing Then
        Else
            Dim posittab = positiontable.Select("RowID = " & position_ID)

            For Each strval In posittab
                If IsDBNull(strval("ParentPositionID")) Then
                    position_rowID = position_ID
                    Exit For
                Else
                    position_rowID = strval("ParentPositionID")
                    rootParentPosition(strval("ParentPositionID"))
                End If
                Exit For
            Next
        End If

        Return position_rowID
    End Function

    Sub loademployee(Optional PositID As Object = Nothing)
        dgvRowAdder(q_employee & " AND e.PositionID='" & PositID & "' ORDER BY e.RowID DESC LIMIT " & pagination & ",100;", dgvemployees)
    End Sub

    Dim pagination As Integer = 0

    Private Sub Nxt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles Nxt.LinkClicked, Last.LinkClicked,
                                                                                              Prev.LinkClicked, First.LinkClicked
        Dim sendrname As String = DirectCast(sender, LinkLabel).Name

        If sendrname = "First" Then
            pagination = 0
        ElseIf sendrname = "Prev" Then
            Dim modcent = pagination Mod 100

            If modcent = 0 Then

                pagination -= 100
            Else

                pagination -= modcent

            End If

            If pagination < 0 Then

                pagination = 0

            End If

        ElseIf sendrname = "Nxt" Then
            Dim modcent = pagination Mod 100

            If modcent = 0 Then
                pagination += 100
            Else
                pagination -= modcent

                pagination += 100

            End If
        ElseIf sendrname = "Last" Then
            Dim lastpage = Val(EXECQUER("SELECT COUNT(RowID) / 100 FROM employee WHERE OrganizationID=" & orgztnID & ";"))
            Dim remender = lastpage Mod 100

            pagination = (lastpage - remender) * 100
        End If

        loademployee(selPositionID)
    End Sub

    Private Sub cboDivis_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboDivis.KeyPress
        Dim e_asc As String = Asc(e.KeyChar)

        If e_asc = 8 Then
            e.Handled = False
            cboDivis.Text = ""
        Else
            e.Handled = True 'TrapCharKey(e_asc)

        End If

    End Sub

    Private Sub cboDivis_SelectedIndexChanged(sender As Object, e As EventArgs) 'Handles cboDivis.SelectedIndexChanged
        If cboDivis.SelectedIndex = -1 Then
        Else
            cboParentPosit.Items.Clear()

            cboParentPosit.Text = ""

            Dim cboboxDivisionRowID = Nothing

            For Each datrow As DataRow In divisiontable.Rows
                If Trim(datrow("Name")) = Trim(cboDivis.Text) Then
                    cboboxDivisionRowID = datrow("RowID")
                    Exit For
                End If
            Next

            Dim selpositOfThisDivision = positiontable.Select("DivisionID=" & cboboxDivisionRowID)

            For Each drow In selpositOfThisDivision
                cboParentPosit.Items.Add(drow("PositionName"))

            Next
        End If
    End Sub

    Private Sub cboParentPosit_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboParentPosit.KeyPress
        Dim e_asc As String = Asc(e.KeyChar)
        If e_asc = 8 Then
            e.Handled = False
            cboParentPosit.Text = ""
        Else
            e.Handled = True 'TrapCharKey(e_asc)
        End If
    End Sub

    Private Sub tsbtnNewPosition_Click(sender As Object, e As EventArgs) Handles tsbtnNewPosition.Click
        tsbtnNewPosition.Enabled = 0

        cboDivis.Focus()

        cboDivis.SelectedIndex = -1
        cboDivis.Text = ""
        cboParentPosit.SelectedIndex = -1
        cboParentPosit.Text = ""
        txtPositName.Text = ""

        dgvemployees.Rows.Clear()

        cboParentPosit.Items.Clear()

        For Each drow As DataRow In positiontable.Rows
            cboParentPosit.Items.Add(drow("PositionName").ToString)
        Next

    End Sub

    Dim dontUpdate As SByte = 0

    Private Sub tsbtnSavePosition_Click(sender As Object, e As EventArgs) Handles tsbtnSavePosition.Click

        Dim str_tn_fullpath As String =
            tv2.SelectedNode.FullPath

        tbpPosition.Focus()

        RemoveHandler tv2.AfterSelect, AddressOf tv2_AfterSelect

        'positiontable
        Dim parentpositID As Object = Nothing

        Dim selposit = positiontable.Select("PositionName='" & Trim(cboParentPosit.Text) & "'")

        For Each drow In selposit
            parentpositID = drow("RowID")
            Exit For
        Next

        Dim divisID As Object = cboDivis.SelectedValue

        If divisID = Nothing Then
            WarnBalloon("Please select a Division Name.", "Invalid Division Name", cboDivis, cboDivis.Width - 17, -70)

            cboDivis.Focus()

            AddHandler tv2.AfterSelect, AddressOf tv2_AfterSelect

            Exit Sub

        End If

        If tsbtnNewPosition.Enabled = 0 Then

            Dim returnval = INSUPD_position(,
                Trim(txtPositName.Text),
                parentpositID,
                divisID)

            InfoBalloon("Position '" & txtPositName.Text & "' has successfully saved.", "Position save successful", lblforballoon, 0, -69)
        Else
            If dontUpdate = 1 Then
                Exit Sub
            End If

            If selPositionID = Nothing Then
            Else
                INSUPD_position(selPositionID,
                                Trim(txtPositName.Text),
                                parentpositID,
                                If(parentpositID = Nothing, divisID, Nothing))

                InfoBalloon("Position '" & txtPositName.Text & "' has successfully saved.", "Position save successful", lblforballoon, 0, -69)

            End If

        End If

        reload()

        For Each drow As DataRow In alphadivision.Rows

            Divisiontreeviewfiller(drow("RowID"), drow("Name"))

        Next

        tv2.ExpandAll()

        AddHandler tv2.AfterSelect, AddressOf tv2_AfterSelect

        tsbtnNewPosition.Enabled = 1

        '########################################################

        Dim node_names = str_tn_fullpath.Split("\")

        Dim selected_node =
            tv2.Nodes.Cast(Of TreeNode)()

        If selected_node.Count > 0 _
            Or node_names.Length > 0 Then

            For i = 0 To (node_names.Length - 1)

                selected_node =
                    SelectTreeNode(selected_node,
                                   node_names(i),
                                   i)
            Next

            For Each sel_nod In selected_node

                tv2.SelectedNode = sel_nod

            Next

        End If

    End Sub

    Private Sub tsbtnDeletePosition_Click(sender As Object, e As EventArgs) Handles tsbtnDeletePosition.Click

        If selPositionID = Nothing Then
        Else

            RemoveHandler tv2.AfterSelect, AddressOf tv2_AfterSelect
            EXECQUER(
                String.Concat("UPDATE employee SET PositionID=NULL,LastUpdBy=", z_User, " WHERE PositionID='", selPositionID, "' AND OrganizationID=", orgztnID, ";",
                              "DELETE FROM `position_view` WHERE PositionID='", selPositionID, "';",
                              "DELETE FROM position WHERE RowID='", selPositionID, "';",
                              "ALTER TABLE position AUTO_INCREMENT = 0;"))

            positiontable = retAsDatTbl("SELECT *" &
                                        ",COALESCE((SELECT CONCAT('(',FirstName,IF(MiddleName IS NULL,'',CONCAT(' ',LEFT(MiddleName,1))),IF(LastName IS NULL,'',CONCAT(' ',LEFT(LastName,1))),')') FROM employee WHERE OrganizationID=" & orgztnID & " AND PositionID=p.RowID AND TerminationDate IS NULL),'(Open)') 'positionstats'" &
                                        " FROM position p" &
                                        " WHERE p.OrganizationID=" & orgztnID & "" &
                                        " AND p.RowID NOT IN (SELECT PositionID FROM user WHERE OrganizationID=" & orgztnID & ");")

            alphaposition = retAsDatTbl("SELECT * FROM position WHERE OrganizationID=" & orgztnID & " AND ParentPositionID IS NULL" &
                                        " AND RowID NOT IN (SELECT PositionID FROM user WHERE OrganizationID=" & orgztnID & ");")

            For Each drow As DataRow In alphadivision.Rows

                Divisiontreeviewfiller(drow("RowID"), drow("Name"), )

            Next

            tv2.ExpandAll()

            AddHandler tv2.AfterSelect, AddressOf tv2_AfterSelect

            btnRefresh_Click(sender, e)

        End If

    End Sub

    Private Sub tsbtnCancel_Click(sender As Object, e As EventArgs) Handles tsbtnCancel.Click

        tsbtnNewPosition.Enabled = 1

        If tv2.Nodes.Count <> 0 Then
            If currentNode Is Nothing Then
            Else
                tv2_AfterSelect(sender, New TreeViewEventArgs(currentNode))
            End If
        End If

    End Sub

    Function INSUPD_position(Optional pos_RowID As Object = Nothing,
                             Optional pos_PositionName As Object = Nothing,
                             Optional pos_ParentPositionID As Object = Nothing,
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

                return_value = datread(0) 'INSUPD_position

            End With
        Catch ex As Exception
            MsgBox(ex.Message & " " & "INSUPD_position", , "Error")
        Finally
            conn.Close()
            cmd.Dispose()
        End Try
        Return return_value
    End Function

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Button3.Image.Tag = 1 Then
            Button3.Image = Nothing
            Button3.Image = My.Resources.r_arrow
            Button3.Image.Tag = 0

            TabControl1.Show()
            tv2.Width = 446

            'tvPosit_AfterSelect(sender, e)
        Else
            Button3.Image = Nothing
            Button3.Image = My.Resources.l_arrow
            Button3.Image.Tag = 1

            TabControl1.Hide()
            Dim pointX As Integer = Width_resolution - (Width_resolution * 0.15)

            tv2.Width = pointX
        End If

    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        Me.Close()
    End Sub

    Private Sub TabControl1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TabControl1.DrawItem
        TabControlColor(TabControl1, e)
    End Sub

    Private Sub tsbtnAudittrail_Click(sender As Object, e As EventArgs) Handles tsbtnAudittrail.Click
        showAuditTrail.Show()

        showAuditTrail.loadAudTrail(view_ID)

        showAuditTrail.BringToFront()

    End Sub

    Private Sub tbpPosition_Enter(sender As Object, e As EventArgs) Handles tbpPosition.Enter

        cboDivis.ContextMenu = New ContextMenu

        cboParentPosit.ContextMenu = New ContextMenu

        cboDivLoc.ContextMenu = New ContextMenu

    End Sub

    Private Sub cboDivLoc_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboDivLoc.KeyPress
        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 8 Then
            e.Handled = True
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub DivisionLocation_Changed(sender As Object, e As EventArgs) Handles cboDivLoc.SelectedIndexChanged

        Dim str_quer_sub_division As String =
            String.Concat("SELECT d.RowID",
                          ",d.Name",
                          " FROM division d",
                          " WHERE d.OrganizationID=", orgztnID,
                          " AND d.ParentDivisionID=", ValNoComma(cboDivLoc.SelectedValue), ";")

        Dim sql As New SQLQueryToDatatable(str_quer_sub_division)
        Dim dt As New DataTable
        dt = sql.ResultTable

        cboDivis.DisplayMember = dt.Columns(1).ColumnName
        cboDivis.ValueMember = dt.Columns(0).ColumnName
        cboDivis.DataSource = dt
    End Sub

    Private Sub bgworkautcompsearch_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkautcompsearch.DoWork
        For Each drow As DataRow In alphaposition.Rows
            Dim newarray = StringToArray(drow("PositionName"))
            autcomptxtposition.Items.Add(New AutoCompleteEntry(drow("PositionName"), newarray))
        Next
    End Sub

    Private Sub bgworkautcompsearch_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkautcompsearch.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            MsgBox("Error: " & vbNewLine & e.Error.Message)
        ElseIf e.Cancelled Then
        Else
            autcomptxtposition.Enabled = True
        End If
    End Sub

    Dim return_value As IEnumerable(Of TreeNode)

    Private Function SelectTreeNode(tn As IEnumerable(Of TreeNode),
                                    tn_name As String,
                                    indx As Integer) As IEnumerable(Of TreeNode)

        If indx = 0 Then

            Dim sel_nod =
                tn.Cast(Of TreeNode).Where(Function(tnod) tnod.Text = tn_name)

            return_value = sel_nod
        Else

            For Each t_node In tn

                Dim sel_nod =
                    t_node.Nodes.Cast(Of TreeNode).Where(Function(tnod) tnod.Text = tn_name)

                return_value = sel_nod

            Next

        End If

        Return return_value

    End Function

End Class
