Public Class EmpSalaryForm
    Dim isNew As Integer = 0
    Dim noofdepd As Integer
    Dim mStat As String
    Dim payid, filingid, sssid, philID As Integer

    Private Sub fillempployeeList()
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select * From employee where OrganizationID = '" & z_OrganizationID & "'")

        dgvEmpList.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvEmpList.Rows.Add()
            With drow
                Dim ln, fn, mn, name As String
                ln = .Item("Lastname").ToString
                fn = .Item("Firstname").ToString
                mn = .Item("Middlename").ToString
                name = fn + " " + mn + " " + ln
                dgvEmpList.Rows.Item(n).Cells(c_EmployeeID.Index).Value = .Item("EmployeeID").ToString
                dgvEmpList.Rows.Item(n).Cells(c_EmployeeName.Index).Value = name
                dgvEmpList.Rows.Item(n).Cells(c_ID.Index).Value = .Item("RowID").ToString
            End With
        Next
    End Sub
    Private Sub cleartext()
        txtBasicrate.Clear()
        txtPagibig.Clear()
        txtPhilHealth.Clear()
        txtSSS.Clear()
    End Sub

    Private Function fillSelectedEmpSalaryList(ByVal EmpID As Integer) As Boolean

        Try
            Dim dt As New DataTable
            dt = getDataTableForSQL("select * from employeesalary es inner join employee ee on es.EmployeeID = ee.RowID Where es.EmployeeID = '" & EmpID & "' And ee.OrganizationID = '" & z_OrganizationID & "' Order By es.RowID ASC")
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
                    dgvemployeesalary.Rows.Item(n).Cells(c_empID.Index).Value = .Item("EmployeeID").ToString
                    dgvemployeesalary.Rows.Item(n).Cells(c_empName.Index).Value = name
                    dgvemployeesalary.Rows.Item(n).Cells(c_PayType.Index).Value = getpfID
                    dgvemployeesalary.Rows.Item(n).Cells(c_filingstatus.Index).Value = getfsID
                    dgvemployeesalary.Rows.Item(n).Cells(c_maritalStatus.Index).Value = .Item("MaritalStatus").ToString
                    dgvemployeesalary.Rows.Item(n).Cells(c_noofdepd.Index).Value = .Item("NoOfDependents").ToString
                    dgvemployeesalary.Rows.Item(n).Cells(c_basicpay.Index).Value = .Item("Basicpay").ToString
                    dgvemployeesalary.Rows.Item(n).Cells(c_pagibig.Index).Value = .Item("HDMFAmount").ToString
                    dgvemployeesalary.Rows.Item(n).Cells(c_philhealth.Index).Value = getphID
                    dgvemployeesalary.Rows.Item(n).Cells(c_sss.Index).Value = getpsID
                    dgvemployeesalary.Rows.Item(n).Cells(c_fromdate.Index).Value = datefrom
                    dgvemployeesalary.Rows.Item(n).Cells(c_todate.Index).Value = dateto
                    dgvemployeesalary.Rows.Item(n).Cells(c_rowid.Index).Value = .Item("RowID")
                End With
            Next

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error Code fillEmpSalaryList")
        End Try


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
                    'cmbFilingStatus.Text = getfsID
                    'cmbMaritalStatus.Text = .Item("MaritalStatus")
                    'ComboBox1.Text = .Item("NoofDependents")
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
    Private Function fillseletedEnterEmpID(ByVal RowID As Integer) As Boolean

        Dim dt As New DataTable
        dt = getDataTableForSQL("select * from employeesalary es inner join employee ee on es.EmployeeID = ee.RowID where es.RowID = '" & RowID & "' And ee.OrganizationID = '" & z_OrganizationID & "'")

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
                    Dim payph, paysss As Integer
                    payph = .Item("PayPhilHealthID").ToString
                    paysss = .Item("PaySocialSecurityID").ToString

                    Dim fsID As String = getStringItem("Select `FilingStatus` from FilingStatus where RowID = '" & filingID & "'")
                    Dim getfsID As String = fsID
                    Dim payphID As String = getStringItem("Select `EmployeeShare` from PayPhilHealth where RowID = '" & payph & "'")
                    Dim getpayphID As String = payphID
                    Dim paysssID As String = getStringItem("Select `EmployeeContributionAmount` from PaySocialSecurity where RowID = '" & paysss & "'")

                    Dim getpaysssID As String = paysssID


                    txtSSS.Text = FormatNumber(getpaysssID, 2)
                    txtPhilHealth.Text = FormatNumber(getpayphID, 2)
                    'cmbFilingStatus.Text = getfsID
                    'cmbMaritalStatus.Text = .Item("MaritalStatus")
                    'ComboBox1.Text = .Item("NoofDependents")
                    txtEmpID.Text = .Item("EmployeeID").ToString
                    txtBasicrate.Text = FormatNumber(.Item("BasicPay"), 2)
                    txtPagibig.Text = FormatNumber(.Item("HDMFAmount"), 2)

                    dptFrom.Value = CDate(.Item("EffectiveDateFrom")).ToString("MM/dd/yyyy")
                    dtpTo.Value = CDate(.Item("EffectiveDateTo")).ToString("MM/dd/yyyy")
                End With
            Next

        End If


        Return True

    End Function

    Private Sub EmpSalaryForm_Load(sender As Object, e As EventArgs) Handles Me.Load
       
        

        fillempployeeList()
        fillSelectedEmpSalaryList(dgvEmpList.CurrentRow.Cells(c_EmployeeID.Index).Value)
        fillseletedEnterEmpID(dgvemployeesalary.CurrentRow.Cells(c_rowid.Index).Value)
        If lbllinkbasicpay.Text = "yes" Then
            If Not dgvEmpList.Rows.Count = 0 Then
                Dim NoRow As Integer = 1
                dgvEmpList.CurrentCell = dgvEmpList(0, NoRow)
            End If
        End If
    End Sub

    Private Sub dgvEmpList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmpList.CellClick
        fillSelectedEmpSalaryList(dgvEmpList.CurrentRow.Cells(c_ID.Index).Value)
        'fillseletedEnterEmpID(dgvemployeesalary.CurrentRow.Cells(c_rowid.Index).Value)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtBasicrate_TextChanged(sender As Object, e As EventArgs) Handles txtBasicrate.TextChanged
        'fillEnterEmpID(Val(txtEmpID.Text))

        ComputeEmpSalary(txtBasicrate, txtSSS, txtPhilHealth)

        Dim basicrate As Double

        basicrate = CDec(IIf(Decimal.TryParse(txtBasicrate.Text, Nothing), txtBasicrate.Text, "0"))

        Dim dtemp As New DataTable
        dtemp = getDataTableForSQL("Select * From employee ee inner join employeedependents ed on ee.RowID = ed.ParentEmployeeID  " & _
                                   "where ee.EmployeeID = '" & dgvEmpList.CurrentRow.Cells(c_empID.Index).Value & "' And ee.OrganizationID = '" & z_OrganizationID & "'")

     
        noofdepd = 0
        mStat = 0
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

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        isNew = 1


        grpbasicsalaryaddeduction.Enabled = True
        btnSave.Enabled = True
        btnNew.Enabled = False

        cleartext()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If isNew = 1 Then
            SP_EmployeeSalary(1, 1, z_datetime, z_datetime, filingid, mStat, noofdepd, 2, txtEmpID.Text, z_ssid, z_phID, txtPagibig.Text, txtBasicrate.Text, CDate(dptFrom.Value).ToString("yyyy-MM-dd"), CDate(dtpTo.Value).ToString("yyyy-MM-dd"))
        Else
            Dim dt As New DataTable
            dt = getDataTableForSQL("select * from employeesalary es inner join employee ee on es.EmployeeID = ee.RowID Where es.RowID = '" & Val(dgvemployeesalary.CurrentRow.Cells(c_rowid.Index).Value) & "'")
            If dt.Rows.Count > 0 Then
                Dim rowid As Integer = dt.Rows(0)("RowID")
                DirectCommand("UPDATE employeesalary SET filingstatusID = '" & filingid & "', MaritalStatus = '" & mStat & "', NoofDependents = '" & noofdepd & "', " & _
                              "BasicPay = '" & CDec(txtBasicrate.Text) & "', HDMFAmount = '" & txtPagibig.Text & "', PayPhilHealthID = '" & z_phID & "', PaySocialSecurityID = '" & z_ssid & "', " & _
                              "EffectiveDateFrom = '" & CDate(dptFrom.Value).ToString("yyyy-MM-dd") & "', EffectiveDateTo = '" & CDate(dtpTo.Value).ToString("yyyy-MM-dd") & "' Where RowID = '" & rowid & "'")
            End If
        End If

        'fillemployeelist()
        fillSelectedEmpSalaryList(dgvEmpList.CurrentRow.Cells(c_EmployeeID.Index).Value)
        myBalloon("Successfully Save", "Saved", lblSaveMSg, , -100)
        ' btnSave.Enabled = False
        btnNew.Enabled = True

        grpbasicsalaryaddeduction.Enabled = False
        isNew = 0
    End Sub

    Private Sub dgvemployeesalary_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvemployeesalary.CellClick
        Try
            fillseletedEnterEmpID(dgvemployeesalary.CurrentRow.Cells(c_rowid.Index).Value)
            grpbasicsalaryaddeduction.Enabled = True
            btnDelete.Enabled = True

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If MsgBox("Are you sure you want to remove this employe salary No." & dgvemployeesalary.CurrentRow.Cells(c_rowid.Index).Value & "?", MsgBoxStyle.YesNo, "Removing...") = MsgBoxResult.Yes Then
            DirectCommand("DELETE FROM employeesalary where RowID = '" & dgvemployeesalary.CurrentRow.Cells(c_rowid.Index).Value & "'")


            fillSelectedEmpSalaryList(dgvEmpList.CurrentRow.Cells(c_EmployeeID.Index).Value)
            fillseletedEnterEmpID(dgvemployeesalary.CurrentRow.Cells(c_rowid.Index).Value)
            btnDelete.Enabled = False
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        'cleartext()
        fillSelectedEmpSalaryList(dgvEmpList.CurrentRow.Cells(c_EmployeeID.Index).Value)
        fillseletedEnterEmpID(dgvemployeesalary.CurrentRow.Cells(c_rowid.Index).Value)
        btnSave.Enabled = False
        btnNew.Enabled = True
        grpbasicsalaryaddeduction.Enabled = False
        isNew = 0
    End Sub

    Private Sub dgvEmpList_KeyUp(sender As Object, e As KeyEventArgs) Handles dgvEmpList.KeyUp
        fillSelectedEmpSalaryList(dgvEmpList.CurrentRow.Cells(c_EmployeeID.Index).Value)
        fillseletedEnterEmpID(dgvemployeesalary.CurrentRow.Cells(c_rowid.Index).Value)
    End Sub

    Private Sub dgvemployeesalary_KeyUp(sender As Object, e As KeyEventArgs) Handles dgvemployeesalary.KeyUp
        Try
            fillseletedEnterEmpID(dgvemployeesalary.CurrentRow.Cells(c_rowid.Index).Value)
            grpbasicsalaryaddeduction.Enabled = True
            btnDelete.Enabled = True

        Catch ex As Exception

        End Try
    End Sub




    Private Sub dgvemployeesalary_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvemployeesalary.CellContentClick

        'fillseletedEnterEmpID(dgvemployeesalary.CurrentRow.Cells(c_rowid.Index).Value)
    End Sub

    Private Sub dgvEmpList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmpList.CellContentClick

    End Sub

End Class