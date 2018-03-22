Imports Femiani.Forms.UI
Imports Femiani.Forms.UI.Input

Public Class SelectFromEmployee

    Dim loadEmpQuery = "SELECT" & _
                        " emp.EmployeeID" & _
                        ",emp.FirstName" & _
                        ",emp.MiddleName" & _
                        ",emp.LastName" & _
                        ",emp.Surname" & _
                        ",emp.Nickname" & _
                        ",emp.TINNo" & _
                        ",emp.SSSNo" & _
                        ",emp.HDMFNo" & _
                        ",emp.PhilHealthNo" & _
                        ",COALESCE(pos.PositionName,'') 'PositionName'" & _
                        ",COALESCE(emp.PositionID,'') 'PositionID'" & _
                        ",emp.EmploymentStatus" & _
                        ",emp.HomeAddress" & _
                        ",IF(emp.Gender='M','Male','Female') 'Gender'" & _
                        ",emp.MaritalStatus" & _
                        ",emp.NoOfDependents" & _
                        ",COALESCE(DATE_FORMAT(emp.Birthdate,'%m/%d%Y'),'') 'Birthdate'" & _
                        ",COALESCE(DATE_FORMAT(emp.StartDate,'%m/%d/%Y'),'') 'StartDate'" & _
                        ",payf.PayFrequencyType 'PayFrequency'" & _
                        ",emp.PayFrequencyID" & _
                        ",emp.Image" & _
                        ",CONCAT(emp.LastName,',',emp.FirstName, IF(emp.MiddleName='','',','),INITIALS(emp.MiddleName,'. ','1')) AS Fullname" &
                        " FROM employee emp" & _
                        " LEFT JOIN position pos ON pos.RowID=emp.PositionID" & _
                        " LEFT JOIN payfrequency payf ON payf.RowID=emp.PayFrequencyID"

    Private Sub SelectFromEmployee_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        LoadEmployees()


        dgvEmployee_SelectionChanged(sender, e)

        AddHandler dgvEmployee.SelectionChanged, AddressOf dgvEmployee_SelectionChanged

        bgautocomplete.RunWorkerAsync()

    End Sub

    Dim empTable As New DataTable

    Dim pagination As Integer = 0

    Sub LoadEmployees(Optional SearchQuery As String = Nothing)

        Static searchString As String = Nothing

        Static samesearchquery As String = Nothing

        If SearchQuery = Nothing Then
            empTable = retAsDatTbl(loadEmpQuery & " WHERE emp.OrganizationID=" & orgztnID & " ORDER BY emp.RowID DESC LIMIT " & pagination & ",100;")

            searchString = Nothing
            samesearchquery = Nothing
        Else

            If searchString <> SearchQuery Then
                searchString = SearchQuery

                pagination = 0

                empTable = retAsDatTbl(loadEmpQuery & " WHERE " & SearchQuery & " AND emp.OrganizationID=" & orgztnID & " ORDER BY emp.RowID DESC LIMIT " & pagination & ",100;")

                samesearchquery = loadEmpQuery & " WHERE " & SearchQuery & " emp.OrganizationID=" & orgztnID & " ORDER BY emp.RowID DESC LIMIT "
            Else

                empTable = retAsDatTbl(samesearchquery & pagination & ",100;")

            End If


        End If

        dgvEmployee.Rows.Clear()

        For Each drow As DataRow In empTable.Rows

            dgvEmployee.Rows.Add(drow(0), _
                                  drow(1), _
                                  drow(2), _
                                  drow(3), _
                                  drow(4), _
                                  drow(5), _
                                  drow(6), _
                                  drow(7), _
                                  drow(8), _
                                  drow(9), _
                                  drow(10), _
                                  drow(11), _
                                  drow(12), _
                                  drow(13), _
                                  drow(14), _
                                  drow(15), _
                                  drow(16), _
                                  drow(17), _
                                  drow(18), _
                                  drow(19), _
                                  drow(20), _
                                  drow(21))

        Next

    End Sub

    Private Sub dgvEmployee_DoubleClick(sender As Object, e As EventArgs) Handles dgvEmployee.DoubleClick
        btnSelect_Click(sender, e)

    End Sub

    Private Sub dgvEmployee_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmployee.CellContentClick

    End Sub

    Private Sub dgvEmployee_SelectionChanged(sender As Object, e As EventArgs) 'Handles dgvEmployee.SelectionChanged
        pbEmpPic.Image = Nothing

        If dgvEmployee.RowCount <> 0 Then
            With dgvEmployee.CurrentRow








                If IsDBNull(.Cells("Column22").Value) Then
                Else
                    pbEmpPic.Image = ConvByteToImage(DirectCast(.Cells("Column22").Value, Byte()))
                End If

            End With

        Else

        End If

    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

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

        LoadEmployees(Trim(FinSearchQuery))

        previousSearch = Trim(FinSearchQuery)
    End Sub

    Private Sub btnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click

        If dgvEmployee.RowCount = 0 Then
        Else
            Dim NewRowIndx As Integer = Nothing

            Dim firstIndx = TimeLogsForm.dgvetentdet.CurrentRow.Index

            Dim SecondIndx = (TimeLogsForm.originalTimeEntryCount) '- 1)

            If firstIndx >= SecondIndx Then
                NewRowIndx = TimeLogsForm.dgvetentdet.CurrentRow.Index

                If TimeLogsForm.dgvetentdet.CurrentRow.IsNewRow Then
                    TimeLogsForm.dgvetentdet.Rows.Add()
                End If

            Else
                For Each dgvrow As DataGridViewRow In TimeLogsForm.dgvetentdet.Rows
                    If dgvrow.IsNewRow Then
                        NewRowIndx = dgvrow.Index
                        TimeLogsForm.dgvetentdet.Rows.Add()
                        Exit For

                    End If
                Next
            End If

            With dgvEmployee.CurrentRow
                'EmpTimeDetail.dgvetentdet.Item("Column7", NewRowIndx).Value = 1
                TimeLogsForm.dgvetentdet.Item("Column2", NewRowIndx).Selected = True

                TimeLogsForm.dgvetentdet.Item("Column2", NewRowIndx).Value = .Cells("Column1").Value

                Dim employeefullname As String = .Cells("Column2").Value

                If Trim(.Cells("Column3").Value) = "" Then
                Else
                    employeefullname &= " " & Microsoft.VisualBasic.Left(Trim(.Cells("Column3").Value), 1) & "."
                End If

                If Trim(.Cells("Column4").Value) = "" Then
                Else
                    employeefullname &= " " & Trim(.Cells("Column4").Value)
                End If

                If Trim(.Cells("Column5").Value) = "" Then
                Else
                    employeefullname &= "-" & Trim(.Cells("Column5").Value)
                End If

                employeefullname = .Cells("EmpFullname").Value

                TimeLogsForm.dgvetentdet.Item("Column11", NewRowIndx).Value = StrConv(employeefullname, VbStrConv.ProperCase) '.Cells("Column2").Value

            End With

            Me.Close()
        End If

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub EnterKeyPressForSearch(sender As Object, e As KeyPressEventArgs) Handles txtEmpID.KeyPress, txtFName.KeyPress, txtMName.KeyPress, _
                                                                                         txtLName.KeyPress, txtSName.KeyPress
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

    Private Sub cbosearch1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbosearch1.KeyPress, cbosearch2.KeyPress, cbosearch3.KeyPress, _
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

    Private Sub cbosearch1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbosearch1.SelectedIndexChanged, cbosearch2.SelectedIndexChanged, cbosearch3.SelectedIndexChanged, _
                                                                                          cbosearch4.SelectedIndexChanged, cbosearch5.SelectedIndexChanged, _
                                                                                          cbosearch1.SelectedValueChanged, cbosearch2.SelectedValueChanged, cbosearch3.SelectedValueChanged, _
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

    Private Sub PaginationEmployee(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles Prev.LinkClicked, Nxt.LinkClicked, _
                                                                                                 First.LinkClicked, Last.LinkClicked

        Dim sender_linklabel = DirectCast(sender, LinkLabel)

        If sender_linklabel.Name = "Prev" Then
            'If pagination - 100 < 0 Then
            '    pagination = 0
            'Else
            '    pagination -= 100
            'End If

            Dim modcent = pagination Mod 100

            If modcent = 0 Then

                pagination -= 100

            Else

                pagination -= modcent

            End If

            If pagination < 0 Then

                pagination = 0

            End If

        ElseIf sender_linklabel.Name = "Nxt" Then

            Dim modcent = pagination Mod 100

            If modcent = 0 Then
                pagination += 100

            Else
                pagination -= modcent

                pagination += 100

            End If
        ElseIf sender_linklabel.Name = "First" Then
            pagination = 0
        ElseIf sender_linklabel.Name = "Last" Then
            Dim lastpage = Val(EXECQUER("SELECT COUNT(RowID) / 100 FROM employee WHERE OrganizationID=" & orgztnID & ";"))



            Dim remender = lastpage Mod 1
            
            pagination = (lastpage - remender) * 100

            If pagination - 100 < 100 Then
                'pagination = 0

            End If

            'pagination = If(lastpage - 100 >= 100, _
            '                lastpage - 100, _
            '                lastpage)

        End If

        LoadEmployees(previousSearch)

    End Sub

    Private Sub dgvEmployee_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvEmployee.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnSelect_Click(sender, e)
        End If

    End Sub

    Private Sub CoolTextBox1_Load(sender As Object, e As EventArgs) Handles autcoEmpID.Load

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            Me.Close()

            Return True

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Dim dattbl As New DataTable

    Private Sub bgautocomplete_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgautocomplete.DoWork

        dattbl = retAsDatTbl("SELECT EmployeeID" & _
                             ",FirstName" & _
                             ",MiddleName" & _
                             ",LastName" & _
                             ",Surname" & _
                             " FROM employee" & _
                             " WHERE OrganizationID='" & orgztnID & "'" & _
                             " AND EmploymentStatus NOT IN ('Resigned','Terminated');")

        If dattbl IsNot Nothing Then

            For Each drow As DataRow In dattbl.Rows

                If CStr(drow("EmployeeID")) <> Nothing Then
                    'autcoEmpID.Items.Add(New AutoCompleteEntry(CStr(drow("EmployeeID"))))

                    autcoEmpID.Items.Add(New AutoCompleteEntry(CStr(drow("EmployeeID")), StringToArray(CStr(drow("EmployeeID")))))

                End If
                
            Next

            dattbl.Dispose()

        End If

    End Sub

    Private Sub bgautocomplete_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgautocomplete.ProgressChanged

    End Sub

    Private Sub bgautocomplete_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgautocomplete.RunWorkerCompleted

        If e.Error IsNot Nothing Then

            MessageBox.Show("Error: " & e.Error.Message)

        ElseIf e.Cancelled Then

            MessageBox.Show("Background work cancelled.")

        Else

        End If

        txtEmpID.Enabled = True
        txtFName.Enabled = True
        txtMName.Enabled = True
        txtLName.Enabled = True
        txtSName.Enabled = True

        autcoEmpID.Text = String.Empty

        autcoEmpID.Enabled = True

    End Sub

End Class