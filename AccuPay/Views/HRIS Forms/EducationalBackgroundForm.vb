Public Class EducationalBackgroundForm
    Dim isNew As Integer = 0

    Private Sub cleartextbox()
        txtCourse.Clear()
        txtDegree.Clear()
        cmbEducType.SelectedIndex = -1

        txtMinor.Clear()

        txtRemarks.Clear()
        txtSchool.Clear()

    End Sub

    Private Sub fillemplyeelist()
        'If dgvEmplist.Rows.Count = 0 Then
        'Else
        Dim dt As New DataTable
        dt = getDataTableForSQL("Select concat(Lastname, ' ', Firstname, ' ', MiddleName) as name, EmployeeID, RowID from employee where organizationID = '" & z_OrganizationID & "'")

        dgvEmplist.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvEmplist.Rows.Add()
            With drow

                dgvEmplist.Rows.Item(n).Cells(c_EmpID.Index).Value = .Item("EmployeeID").ToString
                dgvEmplist.Rows.Item(n).Cells(c_empname.Index).Value = .Item("Name").ToString
                dgvEmplist.Rows.Item(n).Cells(c_rowID.Index).Value = .Item("RowID").ToString
            End With
        Next
        'End If

    End Sub

    Private Sub filleducback()
        If dgvEmplist.Rows.Count = 0 Then
        Else
            Dim dt As New DataTable
            dt = getDataTableForSQL("Select * from employeeeducation ed inner join employee ee on ed.EmployeeID = ee.RowID " & _
                                    "where ee.OrganizationID = '" & z_OrganizationID & "' and ee.EmployeeID = '" & dgvEmplist.CurrentRow.Cells(c_EmpID.Index).Value & "'")

            dgvEducback.Rows.Clear()
            For Each drow As DataRow In dt.Rows
                Dim n As Integer = dgvEducback.Rows.Add()
                With drow
                    dgvEducback.Rows.Item(n).Cells(c_EmplyeeID.Index).Value = .Item("EmployeeID").ToString
                    dgvEducback.Rows.Item(n).Cells(c_name.Index).Value = .Item("Name").ToString
                    dgvEducback.Rows.Item(n).Cells(c_school.Index).Value = .Item("School").ToString
                    dgvEducback.Rows.Item(n).Cells(c_degree.Index).Value = .Item("Degree").ToString
                    dgvEducback.Rows.Item(n).Cells(c_course.Index).Value = .Item("Course").ToString
                    dgvEducback.Rows.Item(n).Cells(c_minor.Index).Value = .Item("Minor").ToString
                    dgvEducback.Rows.Item(n).Cells(c_EducationalType.Index).Value = .Item("EducationType").ToString
                    dgvEducback.Rows.Item(n).Cells(c_Datefrom.Index).Value = CDate(.Item("DateFrom")).ToString("MM/dd/yyyy")
                    dgvEducback.Rows.Item(n).Cells(c_Dateto.Index).Value = CDate(.Item("DateTo")).ToString("MM/dd/yyyy")
                    dgvEducback.Rows.Item(n).Cells(c_Remarks.Index).Value = .Item("Remarks").ToString
                    dgvEducback.Rows.Item(n).Cells(c_RowID1.Index).Value = .Item("RowID").ToString
                End With
            Next
        End If
        
    End Sub
    Private Sub fillselectRowID()
       
            Dim dt As New DataTable
            dt = getDataTableForSQL("Select * from employeeeducation ed inner join employee ee on ed.EmployeeID = ee.RowID " & _
                                    "where ee.OrganizationID = '" & z_OrganizationID & "' And ee.RowID = '" & dgvEmplist.CurrentRow.Cells(c_rowID.Index).Value & "'")

            dgvEducback.Rows.Clear()
            For Each drow As DataRow In dt.Rows
                Dim n As Integer = dgvEducback.Rows.Add()
                With drow
                    dgvEducback.Rows.Item(n).Cells(c_EmplyeeID.Index).Value = .Item("EmployeeID").ToString
                    dgvEducback.Rows.Item(n).Cells(c_name.Index).Value = .Item("Name").ToString
                    dgvEducback.Rows.Item(n).Cells(c_school.Index).Value = .Item("School").ToString
                    dgvEducback.Rows.Item(n).Cells(c_degree.Index).Value = .Item("Degree").ToString
                    dgvEducback.Rows.Item(n).Cells(c_course.Index).Value = .Item("Course").ToString
                    dgvEducback.Rows.Item(n).Cells(c_minor.Index).Value = .Item("Minor").ToString
                    dgvEducback.Rows.Item(n).Cells(c_EducationalType.Index).Value = .Item("EducationType").ToString
                    dgvEducback.Rows.Item(n).Cells(c_Datefrom.Index).Value = CDate(.Item("DateFrom")).ToString("MM/dd/yyyy")
                    dgvEducback.Rows.Item(n).Cells(c_Dateto.Index).Value = CDate(.Item("DateTo")).ToString("MM/dd/yyyy")
                    dgvEducback.Rows.Item(n).Cells(c_Remarks.Index).Value = .Item("Remarks").ToString
                    dgvEducback.Rows.Item(n).Cells(c_RowID1.Index).Value = .Item("RowID").ToString
                End With
            Next

        
    End Sub
    Private Sub fillselecteducback()
        If dgvEducback.Rows.Count = 0 Then
        Else
            Dim dt As New DataTable
            dt = getDataTableForSQL("Select * from employeeeducation ed inner join employee ee on ed.EmployeeID = ee.RowID " & _
                                    "where ee.OrganizationID = '" & z_OrganizationID & "' And ed.RowID = '" & dgvEducback.CurrentRow.Cells(c_RowID1.Index).Value & "'")
            cleartextbox()
            For Each drow As DataRow In dt.Rows
                With drow


                    txtSchool.Text = .Item("School").ToString
                    txtDegree.Text = .Item("Degree").ToString
                    txtCourse.Text = .Item("Course").ToString
                    txtMinor.Text = .Item("Minor").ToString
                    cmbEducType.Text = .Item("EducationType").ToString
                    dtpFrom.Value = CDate(.Item("DateFrom")).ToString("MM/dd/yyyy")
                    dtpto.Value = CDate(.Item("DateTo")).ToString("MM/dd/yyyy")
                    txtRemarks.Text = .Item("Remarks").ToString

                End With
            Next
        End If
        
    End Sub


    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        btnNew.Enabled = False
        cleartextbox()
        isNew = 1
        btnDelete.Enabled = False
        dgvEducback.Enabled = False

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If isNew = 1 Then

            SP_EducBackGround(z_datetime, z_User, z_datetime, z_User, z_OrganizationID, dgvEmplist.CurrentRow.Cells(c_rowID.Index).Value, _
                              dtpFrom.Value.ToString("MM/dd/yyyy"), dtpto.Value.ToString("MM/dd/yyyy"), _
                              txtCourse.Text, txtSchool.Text, txtDegree.Text, txtMinor.Text, cmbEducType.Text, txtRemarks.Text)

            myBalloon("Successfully Save", "Saved", lblSaveMsg, , -100)
            filleducback()
            btnSave.Enabled = False
            isNew = 0
            btnNew.Enabled = True
            dgvEducback.Enabled = True

        Else
            SP_employeeeducationUpdate(dtpFrom.Value.ToString("MM/dd/yyyy"), dtpto.Value.ToString("MM/dd/yyyy"), _
                          txtCourse.Text, txtSchool.Text, txtDegree.Text, txtMinor.Text, cmbEducType.Text, txtRemarks.Text, _
                          dgvEducback.CurrentRow.Cells(c_RowID1.Index).Value)

            myBalloon("Successfully Save", "Saved", lblSaveMsg, , -100)
            filleducback()
            btnSave.Enabled = False
            btnNew.Enabled = True
            isNew = 0
            dgvEducback.Enabled = True
        End If
    End Sub

    Private Sub EducationalBackgroundForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        cmbEducType.Text = "College"
        fillemplyeelist()
        filleducback()
        fillselecteducback()

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()

    End Sub

    Private Sub dgvEmplist_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmplist.CellClick
        fillselectRowID()
        fillselecteducback()

    End Sub

    Private Sub dgvEducback_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEducback.CellClick
        btnSave.Enabled = True
        btnDelete.Enabled = True
        fillselecteducback()
    End Sub

    Private Sub cmbEducType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEducType.SelectedIndexChanged
        lblSchool.Text = cmbEducType.Text
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

    End Sub

End Class