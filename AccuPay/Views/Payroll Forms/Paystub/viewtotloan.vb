Public Class viewtotloan

    Private Sub viewtotloan_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Sub VIEW_employeeloan_indate(Optional eloan_EmployeeID As Object = Nothing, _
                               Optional datefrom As Object = Nothing, _
                               Optional dateto As Object = Nothing)

        Dim params(3, 2) As Object

        params(0, 0) = "eloan_EmployeeID"
        params(1, 0) = "eloan_OrganizationID"
        params(2, 0) = "effectivedatefrom"
        params(3, 0) = "effectivedateto"

        params(0, 1) = eloan_EmployeeID
        params(1, 1) = orgztnID
        params(2, 1) = datefrom
        params(3, 1) = dateto

        EXEC_VIEW_PROCEDURE(params, _
                             "VIEW_employeeloan_indate", _
                             dgvLoanList)

    End Sub

    Private Sub dgvLoanList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvLoanList.CellContentClick

    End Sub

    Private Sub dgvLoanList_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvLoanList.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class