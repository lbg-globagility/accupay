Public Class EmployeePrevEmployerForm
    Dim isNew As Integer = 0
    Private Sub fillemplist()
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


                dgvEmpList.Rows.Item(n).Cells(c_EmpID.Index).Value = .Item("EmployeeID").ToString
                dgvEmpList.Rows.Item(n).Cells(c_EmpName.Index).Value = name
                dgvEmpList.Rows.Item(n).Cells(c_LRowID.Index).Value = .Item("RowID").ToString
            End With
        Next

    End Sub

    Private Sub fillemployerlist()
        If dgvEmpList.Rows.Count = 0 Then
        Else
            Dim dt As New DataTable
            dt = getDataTableForSQL("Select * From employeepreviousemployer where EmployeeID = '" & dgvEmpList.CurrentRow.Cells(c_EmpID.Index).Value & "' " & _
                                    "And OrganizationID = '" & z_OrganizationID & "' ")

            dgvListCompany.Rows.Clear()

            For Each drow As DataRow In dt.Rows
                Dim n As Integer = dgvListCompany.Rows.Add()
                With drow

                    dgvListCompany.Rows.Item(n).Cells(c_compname.Index).Value = .Item("Name").ToString
                    dgvListCompany.Rows.Item(n).Cells(c_trade.Index).Value = .Item("TradeName").ToString
                    dgvListCompany.Rows.Item(n).Cells(c_contname.Index).Value = .Item("ContactName").ToString
                    dgvListCompany.Rows.Item(n).Cells(c_mainphone.Index).Value = .Item("MainPHone").ToString
                    dgvListCompany.Rows.Item(n).Cells(c_altphone.Index).Value = .Item("AltPhone").ToString
                    dgvListCompany.Rows.Item(n).Cells(c_faxno.Index).Value = .Item("FaxNumber").ToString
                    dgvListCompany.Rows.Item(n).Cells(c_emailaddr.Index).Value = .Item("EmailAddress").ToString
                    dgvListCompany.Rows.Item(n).Cells(c_altemailaddr.Index).Value = .Item("AltEmailAddress").ToString
                    dgvListCompany.Rows.Item(n).Cells(c_url.Index).Value = .Item("URL").ToString
                    dgvListCompany.Rows.Item(n).Cells(c_tinno.Index).Value = .Item("TINNo").ToString
                    dgvListCompany.Rows.Item(n).Cells(c_jobtitle.Index).Value = .Item("JobTitle").ToString
                    dgvListCompany.Rows.Item(n).Cells(c_jobfunction.Index).Value = .Item("JobFunction").ToString
                    dgvListCompany.Rows.Item(n).Cells(c_orgtype.Index).Value = .Item("OrganizationType").ToString
                    dgvListCompany.Rows.Item(n).Cells(c_experience.Index).Value = .Item("ExperienceFromTo").ToString
                    dgvListCompany.Rows.Item(n).Cells(c_compaddr.Index).Value = .Item("BusinessAddress").ToString
                    dgvListCompany.Rows.Item(n).Cells(c_rowID.Index).Value = .Item("RowID").ToString

                End With
            Next
        End If
        
    End Sub


    Private Sub fillemployerOneByone()
        If dgvListCompany.Rows.Count = 0 Then
        Else
            Dim dt As New DataTable
            dt = getDataTableForSQL("Select * From employeepreviousemployer where RowID = '" & dgvListCompany.CurrentRow.Cells(c_rowID.Index).Value & "' " & _
                                    "And OrganizationID = '" & z_OrganizationID & "'")
            If dt.Rows.Count > 0 Then
                cleartextbox()
                For Each drow As DataRow In dt.Rows
                    With drow

                        txtCompanyName.Text = .Item("Name").ToString
                        txtTradeName.Text = .Item("TradeName").ToString
                        txtContactName.Text = .Item("ContactName").ToString
                        txtMainPhone.Text = .Item("MainPHone").ToString
                        txtAltPhone.Text = .Item("AltPhone").ToString
                        txtFaxNo.Text = .Item("FaxNumber").ToString
                        txtEmailAdd.Text = .Item("EmailAddress").ToString
                        txtAltEmailAdd.Text = .Item("AltEmailAddress").ToString
                        txtUrl.Text = .Item("URL").ToString
                        txtTinNo.Text = .Item("TINNo").ToString
                        txtJobTitle.Text = .Item("JobTitle").ToString
                        txtJobFunction.Text = .Item("JobFunction").ToString
                        txtOrganizationType.Text = .Item("OrganizationType").ToString
                        txtExfromto.Text = .Item("ExperienceFromTo").ToString
                        txtCompAddr.Text = .Item("BusinessAddress").ToString
                    End With
                Next
            Else
                cleartextbox()
            End If
            'dgvListCompany.Rows.Clear()
            
        End If
        
    End Sub
    Private Sub cleartextbox()
        txtAltEmailAdd.Clear()
        txtAltPhone.Clear()
        txtCompAddr.Clear()
        txtCompanyName.Clear()
        txtContactName.Clear()
        txtEmailAdd.Clear()
        txtFaxNo.Clear()
        txtJobFunction.Clear()
        txtJobTitle.Clear()
        txtMainPhone.Clear()
        txtOrganizationType.Clear()
        txtTinNo.Clear()
        txtTradeName.Clear()
        txtUrl.Clear()
        txtExfromto.Clear()


    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()

    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        isNew = 1
        cleartextbox()
        btnSave.Enabled = True
        btnNew.Enabled = False
        grpDetails.Enabled = True
        dgvListCompany.Enabled = False


    End Sub

    Private Sub EmployeePrevEmployerForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        fillemplist()
        dgvEmpList.Focus()

        fillemployerlist()
        fillemployerOneByone()

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If isNew = 1 Then
            Z_ErrorProvider.Dispose()

            If txtCompanyName.Text = Nothing Or txtContactName.Text = Nothing Or txtMainPhone.Text = Nothing _
                Or txtTinNo.Text = Nothing Or txtCompAddr.Text = Nothing Or txtEmailAdd.Text = Nothing Then
                If Not SetWarningIfEmpty(txtCompanyName) And SetWarningIfEmpty(txtContactName) And SetWarningIfEmpty(txtTinNo) _
                    And SetWarningIfEmpty(txtCompAddr) And SetWarningIfEmpty(txtEmailAdd) Then

                End If
            Else
                SP_employeepreviousemployer(txtCompanyName.Text, txtTradeName.Text, z_OrganizationID, txtMainPhone.Text, txtFaxNo.Text, txtJobTitle.Text, _
                                       txtExfromto.Text, txtCompAddr.Text, txtContactName.Text, txtEmailAdd.Text, txtAltEmailAdd.Text, txtAltPhone.Text, _
                                      txtUrl.Text, txtTinNo.Text, txtJobFunction.Text, z_datetime, z_User, z_datetime, z_User, txtOrganizationType.Text, _
                                      dgvEmpList.CurrentRow.Cells(c_LRowID.Index).Value)
                fillemployerlist()

                myBalloon("Successfully Save", "Saved", lblSaveMsg, , -100)
                dgvListCompany.Enabled = True
                btnNew.Enabled = True
                btnSave.Enabled = False
                isNew = 0
            End If
        Else
            Z_ErrorProvider.Dispose()
            If txtCompanyName.Text = Nothing Or txtContactName.Text = Nothing Or txtMainPhone.Text = Nothing _
               Or txtTinNo.Text = Nothing Or txtCompAddr.Text = Nothing Or txtEmailAdd.Text = Nothing Then
                If Not SetWarningIfEmpty(txtCompanyName) And SetWarningIfEmpty(txtContactName) And SetWarningIfEmpty(txtTinNo) _
                    And SetWarningIfEmpty(txtCompAddr) And SetWarningIfEmpty(txtEmailAdd) Then

                End If
            Else
                SP_EmployeePreviousEmployerUpdate(txtCompanyName.Text, txtTradeName.Text, txtMainPhone.Text, txtFaxNo.Text, txtJobTitle.Text, _
                                txtExfromto.Text, txtCompAddr.Text, txtContactName.Text, txtEmailAdd.Text, txtAltEmailAdd.Text, txtAltPhone.Text, _
                               txtUrl.Text, txtTinNo.Text, txtJobFunction.Text, txtOrganizationType.Text, _
                               dgvListCompany.CurrentRow.Cells(c_rowID.Index).Value)
                fillemployerlist()
                btnNew.Enabled = True
                btnSave.Enabled = False
                myBalloon("Successfully Save", "Saved", lblSaveMsg, , -100)
            End If



        End If
    End Sub





    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If MsgBox("Are you sure you want to remove this employer " & txtCompanyName.Text & "?", MsgBoxStyle.YesNo, "Removing...") = MsgBoxResult.Yes Then
            DirectCommand("Delete From employeepreviousemployer where RowID = '" & dgvListCompany.CurrentRow.Cells(c_rowID.Index).Value & "'")
            fillemployerlist()
            btnDelete.Enabled = False
            btnNew.Enabled = True
            btnSave.Enabled = False
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        cleartextbox()
        btnSave.Enabled = False
        btnDelete.Enabled = False
        dgvListCompany.Enabled = True
        btnNew.Enabled = True
        dgvEmpList.Focus()

    End Sub

    Private Sub dgvListCompany_CellClick1(sender As Object, e As DataGridViewCellEventArgs) Handles dgvListCompany.CellClick
        fillemployerOneByone()
        btnSave.Enabled = True
        'btnNew.Enabled = False
        btnDelete.Enabled = True
    End Sub

    Private Sub dgvEmplist_CellClick1(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmplist.CellClick
        fillemployerlist()
        fillemployerOneByone()
    End Sub

End Class