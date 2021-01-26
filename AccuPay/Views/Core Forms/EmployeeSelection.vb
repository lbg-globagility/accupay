Imports Femiani.Forms.UI.Input

Public Class EmployeeSelection

    Dim loadEmpQuery = "SELECT" &
                        " emp.RowID" &
                        ",emp.EmployeeID" &
                        ",emp.FirstName" &
                        ",emp.MiddleName" &
                        ",emp.LastName" &
                        ",emp.Surname" &
                        ",emp.Nickname" &
                        ",emp.TINNo" &
                        ",emp.SSSNo" &
                        ",emp.HDMFNo" &
                        ",emp.PhilHealthNo" &
                        ",COALESCE(pos.PositionName,'') 'PositionName'" &
                        ",COALESCE(emp.PositionID,'') 'PositionID'" &
                        ",emp.EmploymentStatus" &
                        ",emp.HomeAddress" &
                        ",IF(emp.Gender='M','Male','Female') 'Gender'" &
                        ",emp.MaritalStatus" &
                        ",emp.NoOfDependents" &
                        ",COALESCE(DATE_FORMAT(emp.Birthdate,'%m/%d%Y'),'') 'Birthdate'" &
                        ",COALESCE(DATE_FORMAT(emp.StartDate,'%m/%d/%Y'),'') 'StartDate'" &
                        ",payf.PayFrequencyType 'PayFrequency'" &
                        ",emp.PayFrequencyID" &
                        ",IFNULL(pos.DivisionId,'') 'DivisionId'" &
                        ",emp.Image" &
                        " FROM employee emp" &
                        " LEFT JOIN position pos ON pos.RowID=emp.PositionID" &
                        " LEFT JOIN payfrequency payf ON payf.RowID=emp.PayFrequencyID"

    Dim n_ERowID As String = String.Empty

    Property ERowIDValue As String

        Get
            Return n_ERowID

        End Get

        Set(value As String)
            n_ERowID = value

        End Set

    End Property

    Dim n_EmployeeID As String = String.Empty

    Property EmployeeIDValue As String

        Get
            Return n_EmployeeID

        End Get

        Set(value As String)
            n_EmployeeID = value

        End Set

    End Property

    Dim n_EmpLastName As String = String.Empty

    Property EmpLastNameValue As String

        Get
            Return n_EmpLastName

        End Get

        Set(value As String)
            n_EmpLastName = value

        End Set

    End Property

    Dim n_EmpFirstName As String = String.Empty

    Property EmpFirstNameValue As String

        Get
            Return n_EmpFirstName

        End Get

        Set(value As String)
            n_EmpFirstName = value

        End Set

    End Property

    Dim n_EmpMiddleName As String = String.Empty

    Property EmpMiddleNameValue As String

        Get
            Return n_EmpMiddleName

        End Get

        Set(value As String)
            n_EmpMiddleName = value

        End Set

    End Property

    Dim n_EmpPositionID As Object = Nothing

    Property EmpPositionIDValue As Object

        Get
            Return n_EmpPositionID

        End Get

        Set(value As Object)
            n_EmpPositionID = value

        End Set

    End Property

    Dim n_EmpDivisionID As Object = Nothing

    Property EmpDivisionIDValue As Object

        Get
            Return n_EmpDivisionID

        End Get

        Set(value As Object)
            n_EmpDivisionID = value

        End Set

    End Property

    Protected Overrides Sub OnLoad(e As EventArgs)

        bgworkSearchAutoComp.RunWorkerAsync()

        MyBase.OnLoad(e)

    End Sub

    Private Sub EmployeeSelection_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

    End Sub

    Private Sub SelectFromEmployee_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        LoadEmployees()

        dgvEmployee_SelectionChanged(sender, e)

        AddHandler dgvEmployee.SelectionChanged, AddressOf dgvEmployee_SelectionChanged

    End Sub

    Dim empTable As New DataTable

    Dim pagination As Integer = 0

    Sub LoadEmployees(Optional SearchQuery As String = Nothing)

        Static searchString As String = Nothing

        Static samesearchquery As String = Nothing

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable(loadEmpQuery & " WHERE emp.OrganizationID=" & orgztnID & " ORDER BY emp.RowID DESC LIMIT " & pagination & ",20;")

        If SearchQuery = Nothing Then
            empTable = n_SQLQueryToDatatable.ResultTable

            searchString = Nothing
            samesearchquery = Nothing
        Else

            If searchString <> SearchQuery Then
                searchString = SearchQuery

                pagination = 0

                n_SQLQueryToDatatable = New SQLQueryToDatatable(loadEmpQuery & " WHERE " & SearchQuery & " AND emp.OrganizationID=" & orgztnID & " ORDER BY emp.RowID DESC LIMIT " & pagination & ",20;")

                empTable = n_SQLQueryToDatatable.ResultTable

                samesearchquery = loadEmpQuery & " WHERE " & SearchQuery & " AND emp.OrganizationID=" & orgztnID & " ORDER BY emp.RowID DESC LIMIT "
            Else

                n_SQLQueryToDatatable = New SQLQueryToDatatable(samesearchquery & pagination & ",20;")

                empTable = n_SQLQueryToDatatable.ResultTable

            End If

        End If

        dgvEmployee.Rows.Clear()

        For Each drow As DataRow In empTable.Rows

            Dim rowarray = drow.ItemArray

            dgvEmployee.Rows.Add(rowarray)

        Next

    End Sub

    Private Sub dgvEmployee_DoubleClick(sender As Object, e As EventArgs) Handles dgvEmployee.DoubleClick

    End Sub

    Private Sub dgvEmployee_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmployee.CellContentClick

    End Sub

    Private Sub dgvEmployee_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvEmployee.SelectionChanged
        pbEmpPic.Image = Nothing

        n_ERowID = String.Empty
        n_EmployeeID = String.Empty
        n_EmpLastName = String.Empty
        n_EmpFirstName = String.Empty
        n_EmpMiddleName = String.Empty

        n_EmpPositionID = Nothing

        n_EmpDivisionID = Nothing

        If dgvEmployee.RowCount <> 0 Then

            With dgvEmployee.CurrentRow

                n_ERowID = .Cells("ColRowID").Value
                n_EmployeeID = .Cells("Column1").Value
                n_EmpLastName = .Cells("Column4").Value
                n_EmpFirstName = .Cells("Column2").Value
                n_EmpMiddleName = If(IsDBNull(.Cells("Column3").Value), "", .Cells("Column3").Value)

                n_EmpPositionID = .Cells("Column12").Value

                n_EmpDivisionID = .Cells("DivisionRowID").Value

                If IsDBNull(.Cells("Column22").Value) Then
                    pbEmpPic.Image = Nothing
                Else
                    pbEmpPic.Image = ConvByteToImage(DirectCast(.Cells("Column22").Value, Byte()))
                End If

            End With
        Else

        End If

    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        btnSearch.Enabled = False

        Dim FinSearchQuery As String = Nothing

        If theSearchQuery1 <> Nothing Then
            FinSearchQuery = " " & theSearchQuery1
        End If

        If theSearchQuery1 <> Nothing And theSearchQuery2 <> Nothing Then
            FinSearchQuery &= " AND " & theSearchQuery2
        Else
            If theSearchQuery2 <> Nothing Then
                FinSearchQuery = " " & theSearchQuery2
            End If

        End If

        If (theSearchQuery1 <> Nothing Or theSearchQuery2 <> Nothing) And theSearchQuery3 <> Nothing Then
            FinSearchQuery &= " AND " & theSearchQuery3
        Else

            If theSearchQuery3 <> Nothing Then
                'If FinSearchQuery.Length >= 1 Then
                '    FinSearchQuery &= " AND " & theSearchQuery3
                'Else
                FinSearchQuery &= " " & theSearchQuery3
                'End If
            End If

        End If

        If (theSearchQuery1 <> Nothing Or theSearchQuery2 <> Nothing Or theSearchQuery3 <> Nothing) And theSearchQuery4 <> Nothing Then
            FinSearchQuery &= " AND " & theSearchQuery4
        Else

            If theSearchQuery4 <> Nothing Then
                'If FinSearchQuery.Length >= 1 Then
                '    FinSearchQuery &= " AND " & theSearchQuery4
                'Else
                FinSearchQuery &= " " & theSearchQuery4
                'End If
            End If

        End If

        If (theSearchQuery1 <> Nothing Or theSearchQuery2 <> Nothing Or theSearchQuery3 <> Nothing Or theSearchQuery4 <> Nothing) And theSearchQuery5 <> Nothing Then
            FinSearchQuery &= " AND " & theSearchQuery5
        Else

            If theSearchQuery5 <> Nothing Then
                'If FinSearchQuery.Length >= 1 Then
                '    FinSearchQuery &= " AND " & theSearchQuery5
                'Else
                FinSearchQuery &= " " & theSearchQuery5
                'End If
            End If

        End If

        'MsgBox(FinSearchQuery & vbNewLine & _
        '       Trim(FinSearchQuery).Length)

        'Trim(FinSearchQuery)

        If autcomptxtagency.Text.Trim.Length = 0 Then

            LoadEmployees()
        Else

            LoadEmployees("(EmployeeID ='" & autcomptxtagency.Text.Trim & "' OR FirstName ='" & autcomptxtagency.Text.Trim & "' OR LastName='" & autcomptxtagency.Text.Trim & "')")

        End If

        previousSearch = Trim(FinSearchQuery)

        dgvEmployee_SelectionChanged(dgvEmployee, New EventArgs)

        btnSearch.Enabled = True

    End Sub

    Private Sub btnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click

        Me.DialogResult = Windows.Forms.DialogResult.OK

        Me.Close()

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Me.Close()

    End Sub

    Private Sub EnterKeyPressForSearch(sender As Object, e As KeyPressEventArgs) Handles autcomptxtagency.KeyPress
        'txtEmpID.KeyPress, txtFName.KeyPress, txtMName.KeyPress, _
        'txtLName.KeyPress, txtSName.KeyPress

        Dim e_asc As String = Asc(e.KeyChar)

        If e_asc = 13 Then
            btnSearch_Click(sender, e)

        End If

    End Sub

    Dim theSearchQuery1 As String = Nothing

    Dim theSearchQuery2 As String = Nothing

    Dim theSearchQuery3 As String = Nothing

    Dim theSearchQuery4 As String = Nothing

    Dim theSearchQuery5 As String = Nothing

    Private Sub cbosearch1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbosearch1.KeyPress, cbosearch2.KeyPress, cbosearch3.KeyPress,
                                                                                      cbosearch4.KeyPress, cbosearch5.KeyPress
        Dim e_asc As String = Asc(e.KeyChar)

        If e_asc = 8 Then
            e.Handled = False
        Else
            If e_asc = 13 Then
                e.Handled = False

                btnSearch_Click(sender, e)
            Else
                e.Handled = True
            End If
        End If

    End Sub

    Private Sub cbosearch1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbosearch1.SelectedIndexChanged, cbosearch2.SelectedIndexChanged, cbosearch3.SelectedIndexChanged,
                                                                                          cbosearch4.SelectedIndexChanged, cbosearch5.SelectedIndexChanged,
                                                                                          cbosearch1.SelectedValueChanged, cbosearch2.SelectedValueChanged, cbosearch3.SelectedValueChanged,
                                                                                          cbosearch4.SelectedValueChanged, cbosearch5.SelectedValueChanged
        Dim senderSelIndx = DirectCast(sender, ComboBox)

        'theSearchQuery1 = Nothing
        'theSearchQuery2 = Nothing
        'theSearchQuery3 = Nothing
        'theSearchQuery4 = Nothing
        'theSearchQuery5 = Nothing

        Select Case senderSelIndx.SelectedIndex
            Case -1
                If senderSelIndx.Name = "cbosearch1" Then
                    If Trim(txtEmpID.Text) = "" Then
                        theSearchQuery1 = Nothing
                    Else
                        theSearchQuery1 = "emp.EmployeeID='" & Trim(txtEmpID.Text) & "'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch2" Then
                    If Trim(txtFName.Text) = "" Then
                        theSearchQuery2 = Nothing
                    Else
                        theSearchQuery2 = "emp.FirstName='" & Trim(txtFName.Text) & "'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch3" Then
                    If Trim(txtMName.Text) = "" Then
                        theSearchQuery3 = Nothing
                    Else
                        theSearchQuery3 = "emp.MiddleName='" & Trim(txtMName.Text) & "'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch4" Then
                    If Trim(txtLName.Text) = "" Then
                        theSearchQuery4 = Nothing
                    Else
                        theSearchQuery4 = "emp.LastName='" & Trim(txtLName.Text) & "'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch5" Then
                    If Trim(txtSName.Text) = "" Then
                        theSearchQuery5 = Nothing
                    Else
                        theSearchQuery5 = "emp.Surname='" & Trim(txtSName.Text) & "'"
                    End If
                End If

            Case 0 'starts with

                If senderSelIndx.Name = "cbosearch1" Then
                    If Trim(txtEmpID.Text) = "" Then
                        theSearchQuery1 = Nothing
                    Else
                        theSearchQuery1 = "emp.EmployeeID LIKE '" & Trim(txtEmpID.Text) & "%'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch2" Then
                    If Trim(txtFName.Text) = "" Then
                        theSearchQuery2 = Nothing
                    Else
                        theSearchQuery2 = "emp.FirstName LIKE '" & Trim(txtFName.Text) & "%'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch3" Then
                    If Trim(txtMName.Text) = "" Then
                        theSearchQuery3 = Nothing
                    Else
                        theSearchQuery3 = "emp.MiddleName LIKE '" & Trim(txtMName.Text) & "%'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch4" Then
                    If Trim(txtLName.Text) = "" Then
                        theSearchQuery4 = Nothing
                    Else
                        theSearchQuery4 = "emp.LastName LIKE '" & Trim(txtLName.Text) & "%'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch5" Then
                    If Trim(txtSName.Text) = "" Then
                        theSearchQuery5 = Nothing
                    Else
                        theSearchQuery5 = "emp.Surname LIKE '" & Trim(txtSName.Text) & "%'"
                    End If
                End If

            Case 1 'contains like

                If senderSelIndx.Name = "cbosearch1" Then
                    If Trim(txtEmpID.Text) = "" Then
                        theSearchQuery1 = Nothing
                    Else
                        theSearchQuery1 = "emp.EmployeeID LIKE '%" & Trim(txtEmpID.Text) & "%'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch2" Then
                    If Trim(txtFName.Text) = "" Then
                        theSearchQuery2 = Nothing
                    Else
                        theSearchQuery2 = "emp.FirstName LIKE '%" & Trim(txtFName.Text) & "%'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch3" Then
                    If Trim(txtMName.Text) = "" Then
                        theSearchQuery3 = Nothing
                    Else
                        theSearchQuery3 = "emp.MiddleName LIKE '%" & Trim(txtMName.Text) & "%'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch4" Then
                    If Trim(txtLName.Text) = "" Then
                        theSearchQuery4 = Nothing
                    Else
                        theSearchQuery4 = "emp.LastName LIKE '%" & Trim(txtLName.Text) & "%'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch5" Then
                    If Trim(txtSName.Text) = "" Then
                        theSearchQuery5 = Nothing
                    Else
                        theSearchQuery5 = "emp.Surname LIKE '%" & Trim(txtSName.Text) & "%'"
                    End If
                End If

            Case 2 'is exactly

                If senderSelIndx.Name = "cbosearch1" Then
                    If Trim(txtEmpID.Text) = "" Then
                        theSearchQuery1 = Nothing
                    Else
                        theSearchQuery1 = "emp.EmployeeID='" & Trim(txtEmpID.Text) & "'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch2" Then
                    If Trim(txtFName.Text) = "" Then
                        theSearchQuery2 = Nothing
                    Else
                        theSearchQuery2 = "emp.FirstName='" & Trim(txtFName.Text) & "'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch3" Then
                    If Trim(txtMName.Text) = "" Then
                        theSearchQuery3 = Nothing
                    Else
                        theSearchQuery3 = "emp.MiddleName='" & Trim(txtMName.Text) & "'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch4" Then
                    If Trim(txtLName.Text) = "" Then
                        theSearchQuery4 = Nothing
                    Else
                        theSearchQuery4 = "emp.LastName='" & Trim(txtLName.Text) & "'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch5" Then
                    If Trim(txtSName.Text) = "" Then
                        theSearchQuery5 = Nothing
                    Else
                        theSearchQuery5 = "emp.Surname='" & Trim(txtSName.Text) & "'"
                    End If
                End If

            Case 3 'does not contain

                If senderSelIndx.Name = "cbosearch1" Then
                    If Trim(txtEmpID.Text) = "" Then
                        theSearchQuery1 = Nothing
                    Else
                        theSearchQuery1 = "emp.EmployeeID NOT LIKE '%" & Trim(txtEmpID.Text) & "%'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch2" Then
                    If Trim(txtEmpID.Text) = "" Then
                        theSearchQuery2 = Nothing
                    Else
                        theSearchQuery2 = "emp.FirstName NOT LIKE '%" & Trim(txtFName.Text) & "%'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch3" Then
                    If Trim(txtMName.Text) = "" Then
                        theSearchQuery3 = Nothing
                    Else
                        theSearchQuery3 = "emp.MiddleName NOT LIKE '%" & Trim(txtMName.Text) & "%'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch4" Then
                    If Trim(txtLName.Text) = "" Then
                        theSearchQuery4 = Nothing
                    Else
                        theSearchQuery4 = "emp.LastName NOT LIKE '%" & Trim(txtLName.Text) & "%'"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch5" Then
                    If Trim(txtSName.Text) = "" Then
                        theSearchQuery5 = Nothing
                    Else
                        theSearchQuery5 = "emp.Surname NOT LIKE '%" & Trim(txtSName.Text) & "%'"
                    End If
                End If

            Case 4 'is empty null

                If senderSelIndx.Name = "cbosearch1" Then
                    If Trim(txtEmpID.Text) = "" Then
                        theSearchQuery1 = Nothing
                    Else
                        theSearchQuery1 = "emp.EmployeeID IS NULL"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch2" Then
                    If Trim(txtFName.Text) = "" Then
                        theSearchQuery2 = Nothing
                    Else
                        theSearchQuery2 = "emp.FirstName IS NULL"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch3" Then
                    If Trim(txtMName.Text) = "" Then
                        theSearchQuery3 = Nothing
                    Else
                        theSearchQuery3 = "emp.MiddleName IS NULL"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch4" Then
                    If Trim(txtLName.Text) = "" Then
                        theSearchQuery4 = Nothing
                    Else
                        theSearchQuery4 = "emp.LastName IS NULL"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch5" Then
                    If Trim(txtSName.Text) = "" Then
                        theSearchQuery5 = Nothing
                    Else
                        theSearchQuery5 = "emp.Surname IS NULL"
                    End If
                End If

            Case 5 'is not empty

                If senderSelIndx.Name = "cbosearch1" Then
                    If Trim(txtEmpID.Text) = "" Then
                        theSearchQuery1 = Nothing
                    Else
                        theSearchQuery1 = "emp.EmployeeID IS NOT NULL"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch2" Then
                    If Trim(txtFName.Text) = "" Then
                        theSearchQuery2 = Nothing
                    Else
                        theSearchQuery2 = "emp.FirstName IS NOT NULL"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch3" Then
                    If Trim(txtMName.Text) = "" Then
                        theSearchQuery3 = Nothing
                    Else
                        theSearchQuery3 = "emp.MiddleName IS NOT NULL"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch4" Then
                    If Trim(txtLName.Text) = "" Then
                        theSearchQuery4 = Nothing
                    Else
                        theSearchQuery4 = "emp.LastName IS NOT NULL"
                    End If
                ElseIf senderSelIndx.Name = "cbosearch5" Then
                    If Trim(txtSName.Text) = "" Then
                        theSearchQuery5 = Nothing
                    Else
                        theSearchQuery5 = "emp.Surname IS NOT NULL"
                    End If
                End If

        End Select

    End Sub

    Private Sub txtEmpID_TextChanged(sender As Object, e As EventArgs) Handles txtEmpID.TextChanged
        If Trim(txtEmpID.Text) = "" Then
            theSearchQuery1 = Nothing
        Else
            cbosearch1_SelectedIndexChanged(cbosearch1, e)
        End If

    End Sub

    Private Sub txtFName_TextChanged(sender As Object, e As EventArgs) Handles txtFName.TextChanged
        If Trim(txtFName.Text) = "" Then
            theSearchQuery2 = Nothing
        Else
            cbosearch1_SelectedIndexChanged(cbosearch2, e)
        End If

    End Sub

    Private Sub txtMName_TextChanged(sender As Object, e As EventArgs) Handles txtMName.TextChanged
        If Trim(txtMName.Text) = "" Then
            theSearchQuery3 = Nothing
        Else
            cbosearch1_SelectedIndexChanged(cbosearch3, e)
        End If

    End Sub

    Private Sub txtLName_TextChanged(sender As Object, e As EventArgs) Handles txtLName.TextChanged
        If Trim(txtLName.Text) = "" Then
            theSearchQuery4 = Nothing
        Else
            cbosearch1_SelectedIndexChanged(cbosearch4, e)
        End If

    End Sub

    Private Sub txtSName_TextChanged(sender As Object, e As EventArgs) Handles txtSName.TextChanged
        If Trim(txtSName.Text) = "" Then
            theSearchQuery5 = Nothing
        Else
            cbosearch1_SelectedIndexChanged(cbosearch5, e)
        End If

    End Sub

    Dim previousSearch As String = Nothing

    Private Sub PaginationEmployee(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles Prev.LinkClicked, Nxt.LinkClicked,
                                                                                                 First.LinkClicked, Last.LinkClicked

        Dim sender_linklabel = DirectCast(sender, LinkLabel)

        If sender_linklabel.Name = "Prev" Then
            'If pagination - 20 < 0 Then
            '    pagination = 0
            'Else
            '    pagination -= 20
            'End If

            Dim modcent = pagination Mod 20

            If modcent = 0 Then

                pagination -= 20
            Else

                pagination -= modcent

            End If

            If pagination < 0 Then

                pagination = 0

            End If

        ElseIf sender_linklabel.Name = "Nxt" Then

            Dim modcent = pagination Mod 20

            If modcent = 0 Then
                pagination += 20
            Else
                pagination -= modcent

                pagination += 20

            End If
        ElseIf sender_linklabel.Name = "First" Then
            pagination = 0
        ElseIf sender_linklabel.Name = "Last" Then
            Dim lastpage = Val(EXECQUER("SELECT COUNT(RowID) / 20 FROM employee WHERE OrganizationID=" & orgztnID & ";"))

            Dim remender = lastpage Mod 1

            pagination = (lastpage - remender) * 20

            If pagination - 20 < 20 Then
                'pagination = 0

            End If

            'pagination = If(lastpage - 20 >= 20, _
            '                lastpage - 20, _
            '                lastpage)

        End If

        LoadEmployees(previousSearch)

    End Sub

    Private Sub dgvEmployee_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvEmployee.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        ElseIf e.KeyCode = Keys.Enter Then
            btnSelect_Click(sender, e)
        End If

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            btnClose_Click(btnClose, New EventArgs)

            Return True

            'ElseIf keyData = Keys.Up Then

            '    dgvEmployee.Focus()

            '    'My.Computer.Keyboard.SendKeys("{UP}")
            '    'SendKeys.Send("{UP}")

            '    Return MyBase.ProcessCmdKey(msg, keyData)

            'ElseIf keyData = Keys.Down Then

            '    dgvEmployee.Focus()

            '    'My.Computer.Keyboard.SendKeys("{DOWN}")
            '    'SendKeys.Send("{DOWN}")

            '    Return MyBase.ProcessCmdKey(msg, keyData)
        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub bgworkSearchAutoComp_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgworkSearchAutoComp.DoWork

        Dim EmpIDFNameLName As New DataTable

        Dim n_SQLQueryToDatatable As New SQLQueryToDatatable("SELECT EmployeeID, FirstName, LastName FROM employee WHERE OrganizationID='" & orgztnID & "' AND RevealInPayroll='1';")

        EmpIDFNameLName = n_SQLQueryToDatatable.ResultTable

        For Each drow As DataRow In EmpIDFNameLName.Rows

            With autcomptxtagency

                .Items.Add(New AutoCompleteEntry(CStr(drow("EmployeeID")), StringToArray(CStr(drow("EmployeeID")))))

                .Items.Add(New AutoCompleteEntry(CStr(drow("FirstName")), StringToArray(CStr(drow("FirstName")))))

                .Items.Add(New AutoCompleteEntry(CStr(drow("LastName")), StringToArray(CStr(drow("LastName")))))

            End With

        Next

        EmpIDFNameLName.Dispose()

    End Sub

    Private Sub bgworkSearchAutoComp_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgworkSearchAutoComp.ProgressChanged

    End Sub

    Private Sub bgworkSearchAutoComp_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgworkSearchAutoComp.RunWorkerCompleted

        If e.Error IsNot Nothing Then

            MessageBox.Show("Error: " & e.Error.Message)

        ElseIf e.Cancelled Then

            MessageBox.Show("Background work cancelled.")
        Else

        End If

        autcomptxtagency.Enabled = True
        autcomptxtagency.Focus()

    End Sub

    Private Sub autcomptxtagency_TextChanged(sender As Object, e As EventArgs) Handles autcomptxtagency.TextChanged

    End Sub

End Class