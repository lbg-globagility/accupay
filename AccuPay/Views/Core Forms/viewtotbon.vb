Public Class viewtotbon

    Dim categBonusID As String = Nothing

    Dim bonus_type As New AutoCompleteStringCollection

    Private Sub viewtotbon_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        categBonusID = EXECQUER("SELECT RowID FROM category WHERE OrganizationID=" & orgztnID & " AND CategoryName='" & "Bonus" & "' LIMIT 1;")

        If Val(categBonusID) = 0 Then
            categBonusID = INSUPD_category(, "Bonus")
        End If

        enlistTheLists("SELECT CONCAT(COALESCE(PartNo,''),'@',RowID) FROM product WHERE CategoryID='" & categBonusID & "' AND OrganizationID=" & orgztnID & ";", _
                       bonus_type) 'cboallowtype

        'For Each strval In bonus_type
        '    bon_Type.Items.Add(getStrBetween(strval, "", "@"))
        'Next

    End Sub

    Sub VIEW_employeebonus_indate(Optional ebon_EmployeeID As Object = Nothing, _
                               Optional datefrom As Object = Nothing, _
                               Optional dateto As Object = Nothing)

        Dim params(3, 2) As Object

        params(0, 0) = "ebon_EmployeeID"
        params(1, 0) = "ebon_OrganizationID"
        params(2, 0) = "effectivedatefrom"
        params(3, 0) = "effectivedateto"

        params(0, 1) = ebon_EmployeeID
        params(1, 1) = orgztnID
        params(2, 1) = datefrom
        params(3, 1) = dateto

        EXEC_VIEW_PROCEDURE(params, _
                             "VIEW_employeebonus_indate", _
                             dgvempbon)

    End Sub

    Private Sub dgvempbon_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvempbon.CellContentClick

    End Sub

    Private Sub dgvempbon_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvempbon.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class