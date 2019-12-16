Public Class EmployeePromotionForm
    Dim IsNew As Integer
    Dim rowid As Integer
    Private Sub controltrue()
        cmbfrom.Enabled = True
        cmbto.Enabled = True
        cmbflg.Enabled = True
        dtpEffectivityDate.Enabled = True
        txtbasicpay.Enabled = True
    End Sub
    Private Sub controlclear()
        cmbfrom.SelectedIndex = -1
        cmbto.SelectedIndex = -1
        cmbflg.SelectedIndex = -1
        dtpEffectivityDate.Value = Date.Now
        txtbasicpay.Clear()

    End Sub

    Private Sub controlfalse()
        cmbfrom.Enabled = False
        cmbto.Enabled = False
        cmbflg.Enabled = False
        dtpEffectivityDate.Enabled = False
        txtbasicpay.Enabled = False
    End Sub
    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        IsNew = 1
        btnNew.Enabled = False
        dgvEmpList.Enabled = False
        controlclear()
        controltrue()

    End Sub
    Private Sub fillemplyeelist()
        'If dgvEmplist.Rows.Count = 0 Then
        'ElseCOALESCE(StreetAddress1,' ')
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select concat(COALESCE(Lastname, ' '),' ', COALESCE(Firstname, ' '), ' ', COALESCE(MiddleName, ' ')) as name, EmployeeID, RowID from employee where organizationID = '" & z_OrganizationID & "'")

        dgvEmpList.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvEmpList.Rows.Add()
            With drow

                dgvEmpList.Rows.Item(n).Cells(c_EmployeeID.Index).Value = .Item("EmployeeID").ToString
                'txtempid.Text = .Item("EmployeeID").ToString
                dgvEmpList.Rows.Item(n).Cells(c_EmployeeName.Index).Value = .Item("Name").ToString
                dgvEmpList.Rows.Item(n).Cells(c_ID.Index).Value = .Item("RowID").ToString
            End With
        Next
        'End If

    End Sub

    Private Sub fillemplyeelistselected()
        'If dgvEmplist.Rows.Count = 0 Then
        'ElseCOALESCE(StreetAddress1,' ')
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select concat(COALESCE(Lastname, ' '),' ', COALESCE(Firstname, ' '), ' ', COALESCE(MiddleName, ' ')) as name, EmployeeID, RowID from employee where organizationID = '" & z_OrganizationID & "' And RowID = '" & dgvEmpList.CurrentRow.Cells(c_ID.Index).Value & "'")

        For Each drow As DataRow In dt.Rows

            With drow

                txtEmpID.Text = .Item("EmployeeID").ToString
                'txtempid.Text = .Item("EmployeeID").ToString
                txtEmpName.Text = .Item("Name").ToString
                rowid = .Item("RowID").ToString
            End With
        Next
        'End If

    End Sub
    Private Sub fillpromotions()
        If Not dgvEmpList.Rows.Count = 0 Then
            Dim dt As New DataTable
            dt = getDataTableForSQL("select concat(COALESCE(e.Lastname, ' '),' ', COALESCE(e.Firstname, ' '), ' ', COALESCE(e.MiddleName, ' ')) as name " & _
                                    ",ep.EffectiveDate, ep.CompensationChange, es.BasicPay, ep.PositionFrom, ep.PositionTo, e.EmployeeID, ep.RowID " & _
                                    "from employeepromotions ep inner join employee e on ep.EmployeeID = e.RowID " & _
                                    "inner join employeesalary es on ep.EmployeeSalaryID = es.RowID " & _
                                    "where ep.OrganizationID = '" & z_OrganizationID & "' And e.RowID = '" & rowid & "'")
            dgvPromotionList.Rows.Clear()
            If dt.Rows.Count > 0 Then

                For Each drow As DataRow In dt.Rows
                    Dim n As Integer = dgvPromotionList.Rows.Add()
                    With drow
                        Dim flg As Integer = .Item("CompensationChange").ToString
                        Dim getflg As String
                        If flg = 1 Then
                            getflg = "Yes"
                        Else
                            getflg = "No"
                        End If


                        dgvPromotionList.Rows.Item(n).Cells(c_empID2.Index).Value = .Item("EmployeeID").ToString
                        dgvPromotionList.Rows.Item(n).Cells(c_empname2.Index).Value = .Item("Name").ToString
                        dgvPromotionList.Rows.Item(n).Cells(c_basicpay.Index).Value = .Item("BasicPay").ToString
                        dgvPromotionList.Rows.Item(n).Cells(c_compensation.Index).Value = getflg
                        dgvPromotionList.Rows.Item(n).Cells(c_PostionFrom.Index).Value = .Item("PositionFrom").ToString
                        dgvPromotionList.Rows.Item(n).Cells(c_positionto.Index).Value = .Item("PositionTo").ToString
                        dgvPromotionList.Rows.Item(n).Cells(c_rowid.Index).Value = .Item("RowID").ToString
                        dgvPromotionList.Rows.Item(n).Cells(c_effecDate.Index).Value = CDate(.Item("EffectiveDate")).ToString("MM/dd/yyyy")
                    End With
                Next
            End If
        End If

       

    End Sub
    Private Sub fillselectedpromotions()
        If Not dgvPromotionList.Rows.Count = 0 Then
            Dim dt As New DataTable
            dt = getDataTableForSQL("select concat(COALESCE(e.Lastname, ' '),' ', COALESCE(e.Firstname, ' '), ' ', COALESCE(e.MiddleName, ' ')) as name " & _
                                    ",ep.EffectiveDate, ep.CompensationChange, es.BasicPay, ep.PositionFrom, ep.PositionTo, e.EmployeeID, ep.RowID, " & _
                                    "concat('Php', ' ', Format(es.BasicPay,2), ' ', DATE_FORMAT(es.EffectiveDatefrom, '%m/%d/%Y'), ' ', 'To', DATE_FORMAT(es.EffectiveDateTo, '%m/%d/%Y')) As SalaryDate " & _
                                    "from employeepromotions ep inner join employee e on ep.EmployeeID = e.RowID " & _
                                    "inner join employeesalary es on ep.EmployeeSalaryID = es.RowID " & _
                                    "where ep.OrganizationID = '" & z_OrganizationID & "' And e.RowID = '" & rowid & "' And ep.RowID = '" & dgvPromotionList.CurrentRow.Cells(c_rowid.Index).Value & "'")
            controlclear()
            If dt.Rows.Count > 0 Then

                For Each drow As DataRow In dt.Rows

                    With drow
                        Dim flg As Integer = .Item("CompensationChange").ToString
                        Dim getflg As String
                        If flg = 1 Then
                            getflg = "Yes"
                        Else
                            getflg = "No"
                        End If

                        txtbasicpay.Text = .Item("BasicPay").ToString
                        cmbflg.Text = getflg
                        cmbfrom.Text = .Item("PositionFrom").ToString
                        cmbto.Text = .Item("PositionTo").ToString
                        dtpEffectivityDate.Text = CDate(.Item("EffectiveDate")).ToString("MM/dd/yyyy")
                        cmbSalaryChanged.Text = .Item("SalaryDate").ToString
                    End With
                Next
            End If
        End If



    End Sub


    Private Sub fillPositionFrom()
        Dim strQuery As String = "select PositionName from Position Where OrganizationID = '" & z_OrganizationID & "'"
        cmbfrom.Items.Clear()
        cmbfrom.Items.Add("")
        cmbfrom.Items.AddRange(CType(SQL_ArrayList(strQuery).ToArray(GetType(String)), String()))
        cmbfrom.SelectedIndex = 0
    End Sub

   

    Private Sub fillSalaryDate()
        Dim strQuery As String = "Select concat('Php', ' ', Format(BasicPay,2), ' ', DATE_FORMAT(EffectiveDatefrom, '%m/%d/%Y'), ' ', 'To', DATE_FORMAT(EffectiveDateTo, '%m/%d/%Y')) as salarydate from employeesalary " & _
                                 "where EmployeeID = '" & rowid & "' And OrganizationID = '" & z_OrganizationID & "'"
        cmbSalaryChanged.Items.Clear()
        cmbSalaryChanged.Items.Add("-Please Select One-")
        cmbSalaryChanged.Items.AddRange(CType(SQL_ArrayList(strQuery).ToArray(GetType(String)), String()))
        cmbSalaryChanged.SelectedIndex = 0
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If IsNew = 1 Then
            If cmbflg.Text = "Yes" Then
                If cmbSalaryChanged.Text = "-Please Select One-" Then
                    myBalloonWarn("Please select one", "System Message", cmbSalaryChanged, , -65)
                    Exit Sub
                End If
            End If
          
            Dim sID As String = getStringItem("Select ROwID from employeesalary " & _
                                              "where concat('Php', ' ', Format(BasicPay,2), ' ', DATE_FORMAT(EffectiveDatefrom, '%m/%d/%Y'), ' ', " & _
                                              "'To', DATE_FORMAT(EffectiveDateTo, '%m/%d/%Y')) = '" & cmbSalaryChanged.Text & "' " & _
                                              "And EmployeeID = '" & rowid & "' And OrganizationID = '" & z_OrganizationID & "'")
            Dim getsID As Integer = Val(sID)


            Dim flg As Integer
            If cmbflg.Text = "Yes" Then
                flg = 1
            Else
                flg = 0
            End If
            sp_promotion(z_datetime, z_User, z_datetime, z_OrganizationID, z_User, dtpEffectivityDate.Value.ToString("yyyy-MM-dd"), cmbfrom.Text, cmbto.Text, _
                         getsID, flg, rowid)
            DirectCommand("UPDATE employeesalary SET PaysocialSecurityID = '" & z_ssid & "', PayPhilHealthID = '" & z_phID & "', BasicPay = '" & txtbasicpay.Text & "' Where RowID = '" & getsID & "'")

            fillpromotions()
            dgvEmpList.Enabled = True
            myBalloon("Successfully Save", "Saving...", lblSaveMsg, , -100)
        Else
           
            If cmbflg.Text = "Yes" Then
                If cmbSalaryChanged.Text = "-Please Select One-" Then
                    myBalloonWarn("Please select one", "System Message", cmbSalaryChanged, , -65)
                    Exit Sub
                End If
                Dim flg As Integer
                If cmbflg.Text = "Yes" Then
                    flg = 1
                Else
                    flg = 0
                End If
                Dim sID As String = getStringItem("Select ROwID from employeesalary " & _
                                           "where concat('Php', ' ', Format(BasicPay,2), ' ', DATE_FORMAT(EffectiveDatefrom, '%m/%d/%Y'), ' ', " & _
                                           "'To', DATE_FORMAT(EffectiveDateTo, '%m/%d/%Y')) = '" & cmbSalaryChanged.Text & "' " & _
                                           "And EmployeeID = '" & rowid & "' And OrganizationID = '" & z_OrganizationID & "'")
                Dim getsID As Integer = Val(sID)
                Dim basicpay As Double
                If Double.TryParse(txtbasicpay.Text, basicpay) Then
                    basicpay = CDec(txtbasicpay.Text)
                Else
                    basicpay = 0

                End If
                DirectCommand("UPDATE employeepromotions SET Effectivedate = '" & dtpEffectivityDate.Value.ToString("yyyy-MM-dd") & "', EmployeeSalaryID = '" & getsID & "', " & _
                              "LastUpd = '" & z_datetime & "', lastupdby = '" & z_User & "', PositionFrom = '" & cmbfrom.Text & "', PositionTo = '" & cmbto.Text & "', CompensationChange = '" & flg & "' Where RowID = '" & dgvPromotionList.CurrentRow.Cells(c_rowid.Index).Value & "'")

            Else

                Dim flg As Integer
                If cmbflg.Text = "Yes" Then
                    flg = 1
                Else
                    flg = 0
                End If
                DirectCommand("UPDATE employeepromotions SET Effectivedate = '" & dtpEffectivityDate.Value.ToString("yyyy-MM-dd") & "', LastUpd = '" & z_datetime & "', lastupdby = '" & z_User & "'," & _
                       "PositionFrom = '" & cmbfrom.Text & "', PositionTo = '" & cmbto.Text & "', CompensationChange = '" & flg & "' Where RowID = '" & dgvPromotionList.CurrentRow.Cells(c_rowid.Index).Value & "'")

            End If
            fillpromotions()
            dgvEmpList.Enabled = True
            myBalloon("Successfully Save", "Saving...", lblSaveMsg, , -100)
        End If
        
        controlfalse()
        btnNew.Enabled = True
        dgvEmpList.Enabled = True


    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()

    End Sub

    Private Sub EmployeePromotionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbSalaryChanged.Visible = False
        fillPositionFrom()
        fillemplyeelist()
        fillemplyeelistselected()
        fillpromotions()
        fillselectedpromotions()
    End Sub

    Private Sub dgvPromotionList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPromotionList.CellClick
        Try
            'fillpromotions()
            controltrue()
            fillselectedpromotions()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgvEmpList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmpList.CellClick
        Try
            controltrue()
            fillemplyeelistselected()
            fillpromotions()
            fillselectedpromotions()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub cmbfrom_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbfrom.SelectedIndexChanged
        Try
            cmbto.Items.Clear()
            fillCombobox("select PositionName from Position Where OrganizationID = '" & z_OrganizationID & "' And PositionName != '" & cmbfrom.Text & "'", cmbto)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtbasicpay_TextChanged(sender As Object, e As EventArgs) Handles txtbasicpay.TextChanged
        Try
            ComputeEmpSalary(txtbasicpay, lblsss, lblphilhealth)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmbflg_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbflg.SelectedIndexChanged

        If cmbflg.Text = "Yes" Then
            cmbSalaryChanged.Visible = True
            fillSalaryDate()
        Else
            cmbSalaryChanged.Visible = False
        End If
    End Sub

    Private Sub cmbSalaryChanged_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSalaryChanged.SelectedIndexChanged
        Try
            Dim sID As String = getStringItem("Select basicpay from employeesalary " & _
                                         "where concat('Php', ' ', Format(BasicPay,2), ' ', DATE_FORMAT(EffectiveDatefrom, '%m/%d/%Y'), ' ', " & _
                                         "'To', DATE_FORMAT(EffectiveDateTo, '%m/%d/%Y')) = '" & cmbSalaryChanged.Text & "' " & _
                                         "And EmployeeID = '" & rowid & "' And OrganizationID = '" & z_OrganizationID & "'")
            Dim getsID As Double = sID

            txtbasicpay.Text = FormatNumber(getsID, 2)

        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgvPromotionList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPromotionList.CellContentClick
        If e.ColumnIndex = c_basicpay.Index Then
            EmpSalaryForm.lbllinkbasicpay.Text = "yes"
            MainForm.ChangeForm(EmpSalaryForm)

            'MessageBox.Show(dgvPromotionList.Item(e.ColumnIndex, e.RowIndex).Value.ToString)
        ElseIf e.ColumnIndex = c_empname2.Index Then
            MessageBox.Show(dgvPromotionList.Item(e.ColumnIndex, e.RowIndex).Value.ToString)
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            IsNew = 0
            btnNew.Enabled = True
            dgvEmpList.Enabled = True
            controlclear()
            controlfalse()
            fillemplyeelistselected()
            fillpromotions()
            fillselectedpromotions()
        Catch ex As Exception

        End Try
      
    End Sub
End Class