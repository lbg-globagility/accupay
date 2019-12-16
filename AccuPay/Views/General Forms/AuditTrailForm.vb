Public Class AuditTrailForm

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        Try
            Dim action, userid As String

            If cbAction.Text = "All" Then
                action = ""
            Else
                action = " And aud.ActionPerformed = '" & cbAction.Text & "' "
            End If

            If cbUserID.Text = "All" Then
                userid = ""
            Else
                userid = " And usr.UserID = '" & EncrypedData(cbUserID.Text) & "' "
            End If
            Dim dt As New DataTable
            dt = getDataTableForSQL("Select * From AuditTrail aud inner join user usr on aud.LastUpdBy = usr.RowID " & _
                                    "inner join view vw on aud.ViewID = vw.RowID Where aud.OrganizationID = '" & z_OrganizationID & "' " & _
                                    "And aud.LastUpd Between '" & dtpFrom.Value.ToString("yyyy-MM-dd") & " 00:00:00' " & _
                                    "And '" & dtpTo.Value.ToString("yyyy-MM-dd") & " 23:59:00'" & userid & action & " order by aud.RowID desc")
            If dt.Rows.Count = 0 Then

                MsgBox("No Record found", MsgBoxStyle.Information, "No Record")

            Else
                dgvAuditList.Rows.Clear()

                For Each dr As DataRow In dt.Rows
                    Dim n As Integer = dgvAuditList.Rows.Add()
                    With dr
                        dgvAuditList.Rows.Item(n).Cells(0).Value = CDate(.Item("lastupd")).ToString
                        dgvAuditList.Rows.Item(n).Cells(1).Value = DecrypedData(.Item("UserID"))
                        dgvAuditList.Rows.Item(n).Cells(2).Value = .Item("FieldChanged")
                        dgvAuditList.Rows.Item(n).Cells(3).Value = .Item("ChangedRowID")
                        dgvAuditList.Rows.Item(n).Cells(4).Value = .Item("OldValue")
                        dgvAuditList.Rows.Item(n).Cells(5).Value = .Item("NewValue")
                        dgvAuditList.Rows.Item(n).Cells(6).Value = .Item("ActionPerformed")
                        dgvAuditList.Rows.Item(n).Cells(7).Value = .Item("ViewName")
                    End With
                    Dim companyname, companyaddr As String
                    companyname = z_CompanyName
                    companyaddr = z_CompanyAddr
                    'rpt.AddAuditTrailRow(companyname, companyaddr, dr.Item("Lastupd").ToString, dr.Item("UserID").ToString, _
                    '                     dr.Item("FieldChanged").ToString, dr.Item("OldValue").ToString, dr.Item("NewValue").ToString, _
                    '                     dr.Item("ActionPerformed").ToString, dr.Item("ViewName"))
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error code Show Button.")
        End Try

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()

    End Sub
End Class