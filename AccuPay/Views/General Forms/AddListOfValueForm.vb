Public Class AddListOfValueForm

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        txtname.Clear()
        lblName.Text = ""
        Me.Close()

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        '        DisplayValue,
        'LIC,
        'Type,
        'ParentLIC,
        'Active,
        'Description,
        'Created,
        'CreatedBy,
        'LastUpd,
        'OrderBy,
        'LastUpdBy
        Dim dt As New DataTable

        If lblName.Text.Trim = "Organization Type" Then
            dt = getDataTableForSQL("Select * From ListOFVal where Type = 'Organization Type' And DisplayValue = '" & txtname.Text & "'")

            If dt.Rows.Count > 0 Then
                myBalloonWarn("Already Exist", "Duplicate Entry", txtname, , -65)

            Else
                sp_list(txtname.Text, txtname.Text, "Organization Type", "", "Yes", "", z_datetime, z_User, z_datetime, 1, z_User)
                OrganizationForm.fillorganizationtype()
                txtname.Clear()
                lblName.Text = ""
                Me.Close()
            End If

          
        ElseIf lblName.Text.Trim = "Type" Then
            dt = getDataTableForSQL("Select * From ListOFVal where Type = 'Type' And DisplayValue = '" & txtname.Text & "'")

            If dt.Rows.Count > 0 Then
                myBalloonWarn("Already Exist", "Duplicate Entry", txtname, , -65)

            Else
                sp_list(txtname.Text, txtname.Text, "Type", "", "Yes", "", z_datetime, z_User, z_datetime, 1, z_User)
                OrganizationForm.filltype()
                txtname.Clear()
                lblName.Text = ""
                Me.Close()
            End If
           
        ElseIf lblName.Text.Trim = "Status" Then
            dt = getDataTableForSQL("Select * From ListOFVal where Type = 'Status' And DisplayValue = '" & txtname.Text & "'")

            If dt.Rows.Count > 0 Then
                myBalloonWarn("Already Exist", "Duplicate Entry", txtname, , -65)

            Else
                sp_list(txtname.Text, txtname.Text, "Status", "", "Yes", "", z_datetime, z_User, z_datetime, 1, z_User)
                OrganizationForm.fillstatus()
                txtname.Clear()
                lblName.Text = ""
                Me.Close()
            End If
           
        ElseIf lblName.Text.Trim = "Personal Title" Then
            dt = getDataTableForSQL("Select * From ListOFVal where Type = 'Salutation' And DisplayValue = '" & txtname.Text & "'")

            If dt.Rows.Count > 0 Then
                myBalloonWarn("Already Exist", "Duplicate Entry", txtname, , -65)

            Else
                sp_list(txtname.Text, txtname.Text, "Salutation", "", "Yes", "", z_datetime, z_User, z_datetime, 1, z_User)
                OrganizationForm.fillpersonalstatus()
                txtname.Clear()
                lblName.Text = ""
                Me.Close()
            End If
        ElseIf lblName.Text = "Deduction Schedule" Then
            dt = getDataTableForSQL("Select * From ListOFVal where Type = 'Deduction Schedule' And DisplayValue = '" & txtname.Text & "'")

            If dt.Rows.Count > 0 Then
                myBalloonWarn("Already Exist", "Duplicate Entry", txtname, , -65)

            Else
                sp_list(txtname.Text, txtname.Text, "Deduction Schedule", "", "Yes", "", z_datetime, z_User, z_datetime, 1, z_User)
                LoanScheduleForm.filldedsched()
                txtname.Clear()
                lblName.Text = ""
                Me.Close()
            End If
        End If
      

    End Sub
End Class