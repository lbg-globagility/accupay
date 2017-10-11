Public Class viewtotallow
    Dim categallowID As Object = Nothing

    Dim allowance_type As New AutoCompleteStringCollection
    Private Sub viewtotallow_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        categallowID = EXECQUER("SELECT RowID FROM category WHERE OrganizationID=" & orgztnID & " AND CategoryName='" & "Allowance Type" & "' LIMIT 1;")

        If Val(categallowID) = 0 Then
            categallowID = INSUPD_category(, "Allowance Type")
        End If

        enlistTheLists("SELECT CONCAT(COALESCE(PartNo,''),'@',RowID) FROM product WHERE CategoryID='" & categallowID & "' AND OrganizationID=" & orgztnID & ";", _
                       allowance_type) 'cboallowtype

        'For Each strval In allowance_type
        '    'cboallowtype.Items.Add(getStrBetween(strval, "", "@"))
        '    eall_Type.Items.Add(getStrBetween(strval, "", "@"))
        'Next

    End Sub

    Sub VIEW_employeeallowance_indate(Optional eallow_EmployeeID As Object = Nothing, _
                               Optional datefrom As Object = Nothing, _
                               Optional dateto As Object = Nothing, _
                               Optional num_weekdays As Object = Nothing, _
                               Optional AllowanceExcept As String = Nothing)

        Dim param(4, 2) As Object

        'param(0, 0) = "eallow_EmployeeID"
        'param(1, 0) = "eallow_OrganizationID"
        'param(2, 0) = "effectivedatefrom"
        'param(3, 0) = "effectivedateto"
        'param(4, 0) = "numweekdays"

        param(0, 0) = "eallow_EmployeeID"
        param(1, 0) = "eallow_OrganizationID"
        param(2, 0) = "effective_datefrom"
        param(3, 0) = "effective_dateto"
        param(4, 0) = "ExceptThisAllowance"

        param(0, 1) = eallow_EmployeeID
        param(1, 1) = orgztnID
        param(2, 1) = datefrom
        param(3, 1) = If(dateto = Nothing, DBNull.Value, dateto)
        param(4, 1) = If(AllowanceExcept = Nothing, String.Empty, AllowanceExcept)

        'param(4, 1) = Val(num_weekdays)

        EXEC_VIEW_PROCEDURE(param, _
                           "VIEW_employeeallowances", _
                           dgvempallowance, , 1) 'VIEW_employeeallowance_indate

    End Sub

    Private Sub dgvempallowance_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempallowance.CellContentClick

    End Sub

    Private Sub dgvempallowance_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvempallowance.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

End Class