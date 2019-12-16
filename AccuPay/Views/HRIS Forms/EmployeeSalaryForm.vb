Public Class EmployeeSalaryForm
    Dim isNew As Integer = 0

    Dim payid, filingid, sssid, philID As Integer

    Private Sub fillemployeelist()
        Dim dt As New DataTable
        dt = getDataTableForSQL("select * from employee Where OrganizationID = '" & z_OrganizationID & "'")

        dgvEmployeeList.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvEmployeeList.Rows.Add()
            With drow
                Dim ln, fn, mn, name As String
                ln = .Item("Lastname").ToString
                fn = .Item("Firstname").ToString
                mn = .Item("Middlename").ToString
                name = fn + " " + mn + " " + ln
                dgvEmployeeList.Rows.Item(n).Cells(c_empid.Index).Value = .Item("EmployeeID").ToString
                dgvEmployeeList.Rows.Item(n).Cells(c_Empname.Index).Value = name
            End With
        Next
    End Sub
    Private Sub fillsearchemployeelist()
        Dim empid, empname As String

        If txtSearchEmpID.Text = Nothing Then
            empid = ""
        Else
            empid = " And EmployeeID = '" & txtSearchEmpID.Text & "' "
        End If

        If txtSearchEmpName.Text = Nothing Then
            empname = ""
        Else
            empname = "  And concat(FirstName, ' ', MiddleName, ' ', LastName) = '" & txtSearchEmpName.Text & "' "
        End If

        Dim dt As New DataTable
        dt = getDataTableForSQL("select * from employee where OrganizationID = '" & z_OrganizationID & "'" & empid & empname)

        dgvEmployeeList.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvEmployeeList.Rows.Add()
            With drow
                Dim ln, fn, mn, name As String
                ln = .Item("Lastname").ToString
                fn = .Item("Firstname").ToString
                mn = .Item("Middlename").ToString
                name = fn + " " + mn + " " + ln
                dgvEmployeeList.Rows.Item(n).Cells(c_empid.Index).Value = .Item("EmployeeID").ToString
                dgvEmployeeList.Rows.Item(n).Cells(c_Empname.Index).Value = name
            End With
        Next
    End Sub
    Private Function fillEmpSalaryList(ByVal EmpID As Integer) As Boolean

        Try
            Dim dt As New DataTable
            dt = getDataTableForSQL("select * from employeesalary es inner join employee ee on es.EmployeeID = ee.RowID Where es.EmployeeID = '" & EmpID & "' And ee.OrganizationID = '" & z_OrganizationID & "'")
            dgvemployeesalary.Rows.Clear()

            For Each drow As DataRow In dt.Rows
                Dim n As Integer = dgvemployeesalary.Rows.Add()
                With drow
                    Dim payssID As Integer = .Item("PaySocialSecurityID")
                    Dim phealthID As Integer = .Item("PayPhilhealthID")
                    Dim payfrequencyID As Integer = .Item("PayFrequencyID")
                    Dim pfID As String = getStringItem("Select PayFrequencyType from PayFrequency where RowID = '" & payfrequencyID & "'")
                    Dim getpfID As String = pfID
                    Dim filingID As Integer = .Item("FilingStatusID")
                    Dim fsID As String = getStringItem("Select `FilingStatus` from FilingStatus where RowID = '" & filingID & "'")
                    Dim getfsID As String = fsID
                    Dim psID As String = getStringItem("Select EmployeeContributionAmount from PaySocialSecurity where RowID = '" & payssID & "'")
                    Dim getpsID As String = psID
                    'EmployeeShare
                    Dim phID As String = getStringItem("Select EmployeeShare from PayPhilhealth where RowID = '" & phealthID & "'")
                    Dim getphID As String = phID
                    Dim ln, fn, mn, name As String
                    ln = .Item("Lastname").ToString
                    fn = .Item("Firstname").ToString
                    mn = .Item("Middlename").ToString
                    name = fn + " " + mn + " " + ln

                    txtBasicrate.Text = FormatNumber(.Item("BasicPay"), 2)

                    Dim datefrom, dateto As String

                    If Not IsDBNull(.Item("EffectiveDateFrom")) Then
                        datefrom = CDate(.Item("EffectiveDateFrom")).ToString("MMM dd, yyyy")
                    Else
                        datefrom = Nothing
                    End If
                    If Not IsDBNull(.Item("EffectiveDateTo")) Then
                        dateto = CDate(.Item("EffectiveDateTo")).ToString("MMM dd, yyyy")
                    Else
                        dateto = Nothing
                    End If
                    dgvemployeesalary.Rows.Item(n).Cells(c_empIDList.Index).Value = .Item("EmployeeID").ToString
                    dgvemployeesalary.Rows.Item(n).Cells(c_empNameList.Index).Value = name
                    dgvemployeesalary.Rows.Item(n).Cells(c_paytype.Index).Value = getpfID
                    dgvemployeesalary.Rows.Item(n).Cells(c_filingStatus.Index).Value = getfsID
                    dgvemployeesalary.Rows.Item(n).Cells(c_MaritalStatus.Index).Value = .Item("MaritalStatus").ToString
                    dgvemployeesalary.Rows.Item(n).Cells(c_NoOfDependents.Index).Value = .Item("NoOfDependents").ToString
                    dgvemployeesalary.Rows.Item(n).Cells(c_basicpay.Index).Value = .Item("Basicpay").ToString
                    dgvemployeesalary.Rows.Item(n).Cells(c_PagIbig.Index).Value = .Item("HDMFAmount").ToString
                    dgvemployeesalary.Rows.Item(n).Cells(c_philHealth.Index).Value = getphID
                    dgvemployeesalary.Rows.Item(n).Cells(c_sssno.Index).Value = getpsID
                    dgvemployeesalary.Rows.Item(n).Cells(c_effecdatefrom.Index).Value = datefrom
                    dgvemployeesalary.Rows.Item(n).Cells(c_effecDateto.Index).Value = dateto
                    dgvemployeesalary.Rows.Item(n).Cells(c_rowid.Index).Value = .Item("RowID")
                End With
            Next

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error Code fillEmpSalaryList")
        End Try


        Return True

    End Function

    Private Function fillselectempsalary(ByVal empID As Integer) As Boolean

        Dim dt As New DataTable
        dt = getDataTableForSQL("select * from employeesalary es inner join employee ee on es.EmployeeID = ee.RowID where es.RowID = '" & empID & "' And ee.OrganizationID = '" & z_OrganizationID & "'")

        'cleartext()
        For Each drow As DataRow In dt.Rows
            With drow
                Dim payfrequencyID As Integer = .Item("PayFrequencyID")
                Dim pfID As String = getStringItem("Select PayFrequencyType from PayFrequency where RowID = '" & payfrequencyID & "'")
                Dim getpfID As String = pfID
                txtpaytype.Text = getpfID
                'FilingStatus
                Dim filingID As Integer = .Item("FilingStatusID")
                Dim fsID As String = getStringItem("Select `FilingStatus` from FilingStatus where RowID = '" & filingID & "'")
                Dim getfsID As String = fsID
                cmbFilingStatus.Text = getfsID
                cmbMaritalStatus.Text = .Item("MaritalStatus")
                ComboBox1.Text = .Item("NoofDependents")
                txtEmpID.Text = .Item("EmployeeID").ToString





                txtBasicrate.Text = FormatNumber(.Item("BasicPay"), 2)

                txtPagibig.Text = FormatNumber(.Item("HDMFAmount"), 2)
                dptFrom.Value = CDate(.Item("EffectiveDateFrom")).ToString("MM/dd/yyyy")
                dtpTo.Value = CDate(.Item("EffectiveDateTo")).ToString("MM/dd/yyyy")
            End With
        Next

        Return True

    End Function

    Private Function fillEnterEmpID(ByVal empID As Integer) As Boolean

        Dim dt As New DataTable
        dt = getDataTableForSQL("select * from employeesalary es inner join employee ee on es.EmployeeID = ee.RowID where es.EmployeeID = '" & empID & "' And ee.OrganizationID = '" & z_OrganizationID & "'")

        'cleartext()
        If dt.Rows.Count > 0 Then
            For Each drow As DataRow In dt.Rows
                With drow
                    Dim ln, fn, mn, name As String
                    ln = .Item("Lastname").ToString
                    fn = .Item("Firstname").ToString
                    mn = .Item("Middlename").ToString
                    name = fn + " " + mn + " " + ln
                    txtEmpname.Text = name
                    Dim payfrequencyID As Integer = .Item("PayFrequencyID")
                    Dim pfID As String = getStringItem("Select PayFrequencyType from PayFrequency where RowID = '" & payfrequencyID & "'")
                    Dim getpfID As String = pfID
                    txtpaytype.Text = getpfID
                    'FilingStatus
                    Dim filingID As Integer = .Item("FilingStatusID")
                    Dim fsID As String = getStringItem("Select `FilingStatus` from FilingStatus where RowID = '" & filingID & "'")
                    Dim getfsID As String = fsID
                    cmbFilingStatus.Text = getfsID
                    cmbMaritalStatus.Text = .Item("MaritalStatus")
                    ComboBox1.Text = .Item("NoofDependents")
                    txtEmpID.Text = .Item("EmployeeID").ToString
                    txtBasicrate.Text = FormatNumber(.Item("BasicPay"), 2)
                    txtPagibig.Text = FormatNumber(.Item("HDMFAmount"), 2)
                    dptFrom.Value = CDate(.Item("EffectiveDateFrom")).ToString("MM/dd/yyyy")
                    dtpTo.Value = CDate(.Item("EffectiveDateTo")).ToString("MM/dd/yyyy")
                End With
            Next
        Else

            dt = getDataTableForSQL("Select * From employee where EmployeeID = '" & empID & "' And OrganizationID = '" & z_OrganizationID & "'")
            If dt.Rows.Count > 0 Then
                For Each drow As DataRow In dt.Rows
                    With drow
                        Dim ln, fn, mn, name As String
                        ln = .Item("Lastname").ToString
                        fn = .Item("Firstname").ToString
                        mn = .Item("Middlename").ToString
                        name = fn + " " + mn + " " + ln
                        txtEmpname.Text = name
                        Dim paytype As Integer = .Item("PayFrequencyID").ToString

                        Dim pt As String = getStringItem("Select PayFrequencyType From PayFrequency where RowID = '" & paytype & "'")
                        Dim getpt As String = pt
                        txtpaytype.Text = getpt


                    End With
                Next
            Else
                txtEmpname.Clear()
            End If
        End If
      

        Return True

    End Function
    Private Sub fillmaritalfiling()
        Dim strQuery As String = "select `FilingStatus` from FilingStatus"
        cmbFilingStatus.Items.Clear()
        cmbFilingStatus.Items.Add("")
        cmbFilingStatus.Items.AddRange(CType(SQL_ArrayList(strQuery).ToArray(GetType(String)), String()))
        cmbFilingStatus.SelectedIndex = 0

        Dim strQuery1 As String = "select Distinct(MaritalStatus) from FilingStatus"
        cmbMaritalStatus.Items.Clear()
        cmbMaritalStatus.Items.Add("")
        cmbMaritalStatus.Items.AddRange(CType(SQL_ArrayList(strQuery1).ToArray(GetType(String)), String()))
        cmbMaritalStatus.SelectedIndex = 0
    End Sub


    Private Sub EmployeeSalaryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        AutoSearchEmpName(txtSearchEmpName)
        AutoSearchEmpID(txtSearchEmpID)
        fillemployeelist()
        fillmaritalfiling()
    End Sub


    Private Sub txtBasicrate_TextChanged(sender As Object, e As EventArgs) Handles txtBasicrate.TextChanged

        ComputeEmpSalary(txtBasicrate, txtSSS, txtPhilHealth)

        Dim basicrate As Double

        basicrate = CDec(IIf(Decimal.TryParse(txtBasicrate.Text, Nothing), txtBasicrate.Text, "0"))

        Dim dtemp As New DataTable
        dtemp = getDataTableForSQL("Select * From employee ee inner join employeedependents ed on ee.RowID = ed.ParentEmployeeID  " & _
                                   "where ee.EmployeeID = '" & dgvEmployeeList.CurrentRow.Cells(c_empid.Index).Value & "' And ee.OrganizationID = '" & z_OrganizationID & "'")

        Dim noofdepd As Integer
        Dim mStat As String

        'If dtemp.Rows.Count > 0 Then
        noofdepd = dtemp.Rows(0)("NoOfDependents").ToString
        mStat = dtemp.Rows(0)("MaritalStatus").ToString


        ' End If

        Dim fsid As String = getStringItem("Select FilingStatus From filingstatus where MaritalStatus = '" & mStat & "' And Dependent = '" & noofdepd & "'")
        Dim getfsid As String = fsid



        Dim dt2 As New DataTable
        dt2 = getDataTableForSQL("select * from paywithholdingtax ph inner join payfrequency pf on ph.PayFrequencyID = pf.RowID " & _
                                 "inner join filingstatus fs on ph.FilingStatusID = fs.RowID " & _
                                 "where fs.FilingStatus = '" & getfsid & "' And fs.MaritalStatus = '" & mStat & "' And pf.PayFrequencyType = '" & txtpaytype.Text & "' " & _
                                 "And ph.TaxableIncomeFromAmount <= '" & basicrate & "' And ph.TaxableIncomeToAmount >= '" & basicrate & "'")

        'txtTAX.Clear()
        If dt2.Rows.Count > 0 Then
            For Each drow As DataRow In dt2.Rows
                With drow
                    Dim exemptionamt, totalexcess, grandtotal, excessandexemption, exin As Integer
                    Dim excessamt As Double
                    excessamt = .Item("ExemptionInExcessAmount")
                    exemptionamt = .Item("TaxableIncomeFromAmount")
                    exin = .Item("ExemptionAmount")
                    payid = .Item("payfrequencyID")
                    filingid = .Item("FilingStatusID")
                    totalexcess = CDec(txtBasicrate.Text) - Val(exemptionamt)
                    excessandexemption = Val(totalexcess) * Val(excessamt)

                    grandtotal = Val(exin) + Val(excessandexemption)



                End With
            Next
        End If



    End Sub
    Private Sub cleartext()
        txtBasicrate.Clear()
        txtEmpID.Clear()
        txtEmpname.Clear()

        txtPagibig.Clear()
        txtpaytype.Clear()
        txtPhilHealth.Clear()
        txtSSS.Clear()

        cmbFilingStatus.SelectedIndex = -1
        cmbMaritalStatus.SelectedIndex = -1
        ComboBox1.SelectedIndex = -1
    End Sub


    Private Sub txtEmpID_TextChanged(sender As Object, e As EventArgs) Handles txtEmpID.TextChanged
    
        fillEnterEmpID(Val(txtEmpID.Text))

    End Sub

    Private Sub txtpaytype_TextChanged(sender As Object, e As EventArgs) Handles txtpaytype.TextChanged

    End Sub


    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        isNew = 1


        grpbasicsalaryaddeduction.Enabled = True
        btnSave.Enabled = True
        btnNew.Enabled = False

        cleartext()

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click


        If isNew = 1 Then
            SP_EmployeeSalary(1, 1, z_datetime, z_datetime, filingid, cmbMaritalStatus.Text, ComboBox1.Text, 2, txtEmpID.Text, sssid, philID, txtPagibig.Text, txtBasicrate.Text, CDate(dptFrom.Value).ToString("yyyy-MM-dd"), CDate(dtpTo.Value).ToString("yyyy-MM-dd"))
        Else
            Dim dt As New DataTable
            dt = getDataTableForSQL("select * from employeesalary es inner join employee ee on es.EmployeeID = ee.RowID Where es.RowID = '" & Val(dgvemployeesalary.CurrentRow.Cells(c_rowid.Index).Value) & "'")
            If dt.Rows.Count > 0 Then
                Dim rowid As Integer = dt.Rows(0)("RowID")
                DirectCommand("UPDATE employeesalary SET filingstatusID = '" & filingid & "', MaritalStatus = '" & cmbMaritalStatus.Text & "', NoofDependents = '" & ComboBox1.Text & "', " & _
                              "BasicPay = '" & CDec(txtBasicrate.Text) & "', HDMFAmount = '" & txtPagibig.Text & "', PayPhilHealthID = '" & z_phID & "', PaySocialSecurityID = '" & z_ssid & "', " & _
                              "EffectiveDateFrom = '" & CDate(dptFrom.Value).ToString("yyyy-MM-dd") & "', EffectiveDateTo = '" & CDate(dtpTo.Value).ToString("yyyy-MM-dd") & "' Where RowID = '" & rowid & "'")
            End If
        End If

        'fillemployeelist()
        fillEmpSalaryList(txtEmpID.Text)
        myBalloon("Successfully Save", "Saved", lblSaveMSg, , -100)
        btnSave.Enabled = False
        btnNew.Enabled = True

        grpbasicsalaryaddeduction.Enabled = False
        isNew = 0
    End Sub

    Private Sub dgvEmployeeList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmployeeList.CellClick
        fillEmpSalaryList(dgvEmployeeList.CurrentRow.Cells(c_empid.Index).Value)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        cleartext()
        btnSave.Enabled = False
        btnNew.Enabled = True
        grpbasicsalaryaddeduction.Enabled = False
        isNew = 0
    End Sub

    Private Sub dgvemployeesalary_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvemployeesalary.CellClick
        Try
            fillselectempsalary(dgvemployeesalary.CurrentRow.Cells(c_rowid.Index).Value)
            btnSave.Enabled = True
            grpbasicsalaryaddeduction.Enabled = True
        Catch ex As Exception

        End Try

    End Sub

    Private Sub txtSearchEmpID_TextChanged(sender As Object, e As EventArgs) Handles txtSearchEmpID.TextChanged
        fillsearchemployeelist()
    End Sub

    Private Sub txtSearchEmpName_TextChanged(sender As Object, e As EventArgs) Handles txtSearchEmpName.TextChanged
        fillsearchemployeelist()
    End Sub

    Private Sub dgvemployeesalary_KeyUp(sender As Object, e As KeyEventArgs) Handles dgvemployeesalary.KeyUp
        Try
            fillselectempsalary(dgvemployeesalary.CurrentRow.Cells(c_rowid.Index).Value)
            btnSave.Enabled = True
            grpbasicsalaryaddeduction.Enabled = True
        Catch ex As Exception

        End Try
    End Sub


    Private Sub dgvemployeesalary_Paint(sender As Object, e As PaintEventArgs) Handles dgvemployeesalary.Paint
        dgvemployeesalary.RowHeadersDefaultCellStyle.BackColor = Color.Blue
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()

    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

    End Sub

    Private Sub dgvEmployeeList_KeyUp(sender As Object, e As KeyEventArgs) Handles dgvEmployeeList.KeyUp
        fillEmpSalaryList(dgvEmployeeList.CurrentRow.Cells(c_empid.Index).Value)
    End Sub
End Class