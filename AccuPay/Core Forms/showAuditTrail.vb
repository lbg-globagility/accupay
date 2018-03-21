Public Class showAuditTrail

    Private Sub showAuditTrail_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        MDIPrimaryForm.Enabled = True

        MDIPrimaryForm.BringToFront()

    End Sub

    Private Sub showAuditTrail_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        MDIPrimaryForm.Enabled = False

    End Sub

    Dim n_ViewRowID = Nothing

    Sub loadAudTrail(Optional ViewRowID As String = Nothing)
        n_ViewRowID = ViewRowID
        'dgvRowAdder("SELECT aut.RowID" & _
        '            ",aut.ViewID" & _
        '            ",aut.ChangedRowID" & _
        '            ",DATE_FORMAT(aut.Created,'%m/%d/%Y %h:%i %p') 'Created'" & _
        '            ",PROPERCASE(CONCAT(COALESCE(u.FirstName,''),' ',COALESCE(u.LastName,''))) 'CreatedBy'" & _
        '            ",v.ViewName" & _
        '            ",aut.FieldChanged" & _
        '            ",COALESCE(aut.OldValue,'') 'OldValue'" & _
        '            ",COALESCE(aut.NewValue,'') 'NewValue'" & _
        '            ",aut.ActionPerformed" & _
        '            " FROM audittrail aut" & _
        '            " LEFT JOIN user u ON u.RowID=aut.CreatedBy" & _
        '            " LEFT JOIN `view` v ON v.RowID = aut.ViewID" & _
        '            " WHERE aut.OrganizationID=" & orgztnID & _
        '            " AND aut.ViewID=" & ViewRowID & _
        '            " AND aut.CreatedBy=" & z_User & _
        '            " ORDER BY aut.Created DESC;", _
        '             dgvaudit)

        Dim params(3, 1) As Object

        params(0, 0) = "OrganizID"
        params(1, 0) = "View_ID"
        params(2, 0) = "UserID"
        params(3, 0) = "pagenumber"

        params(0, 1) = orgztnID
        params(1, 1) = ViewRowID
        params(2, 1) = z_User
        params(3, 1) = pagination

        Dim dt_audit As New DataTable

        dt_audit = callProcAsDatTab(params, _
                                    "VIEW_audittrail")

        'dt_audit = retAsDatTbl("CALL VIEW_audittrail('" & orgztnID & "','" & ViewRowID & "','" & z_User & "','" & pagination & "')")

        dgvaudit.Rows.Clear()

        If dt_audit IsNot Nothing Then

            Dim drowColCount = dt_audit.Columns.Count - 1

            For Each drow As DataRow In dt_audit.Rows

                Dim row_array = drow.ItemArray

                dgvaudit.Rows.Add(row_array)

                'Dim r = dgvaudit.Rows.Add()

                'For c = 0 To drowColCount

                '    Dim dr_val = If(IsDBNull(drow(c)), "", drow(c))

                '    dgvaudit.Item(c, r).Value = dr_val

                'Next

            Next

        End If

    End Sub

    Private Sub dgvaudit_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvaudit.CellContentClick

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            Me.Close()

            Return True

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Dim pagination = 0

    Private Sub PaginationEmployee(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles Prev.LinkClicked, Nxt.LinkClicked, _
                                                                                                 First.LinkClicked, Last.LinkClicked

        Panel1.Enabled = False

        Dim sender_linklabel = DirectCast(sender, LinkLabel)

        If sender_linklabel.Name = "Prev" Then
            'If pagination - 20 < 0 Then
            '    pagination = 0
            'Else
            '    pagination -= 20
            'End If

            Dim modcent = pagination Mod 20

            If modcent = 0 Then

                pagination -= 20

            Else

                pagination -= modcent

            End If

            If pagination < 0 Then

                pagination = 0

            End If

        ElseIf sender_linklabel.Name = "Nxt" Then

            Dim modcent = pagination Mod 20

            If modcent = 0 Then
                pagination += 20

            Else
                pagination -= modcent

                pagination += 20

            End If

        ElseIf sender_linklabel.Name = "First" Then
            pagination = 0

        ElseIf sender_linklabel.Name = "Last" Then
            'Dim lastpage = Val(EXECQUER("SELECT COUNT(RowID) / 20 FROM audittrail WHERE OrganizationID=" & orgztnID & ";"))
            Dim lastpage = Val(EXECQUER("SELECT COUNT(RowID) / 20 FROM audittrail WHERE OrganizationID=" & orgztnID & " AND ViewID='" & n_ViewRowID & "';"))



            Dim remender = lastpage Mod 1
            
            pagination = (lastpage - remender) * 20

            If pagination - 20 < 20 Then
                'pagination = 0

            End If

            'pagination = If(lastpage - 20 >= 20, _
            '                lastpage - 20, _
            '                lastpage)
        End If

        loadAudTrail(n_ViewRowID)

        Panel1.Enabled = True

    End Sub

End Class