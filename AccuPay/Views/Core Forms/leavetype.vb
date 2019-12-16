Public Class leavtyp

    Private Sub leavetype_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        lstbxleavtyp.Items.Clear()
        For Each strval In EmployeeForm.leavetype
            lstbxleavtyp.Items.Add(strval)
        Next

    End Sub

    Private Sub ListBox1_Button1_Event(sender As Object, e As EventArgs) Handles lstbxleavtyp.DoubleClick, _
                                                                                 Button1.Click
        If lstbxleavtyp.Items.Count <> 0 Then

            With EmployeeForm
                If .dgvempleave.RowCount <> 1 Then
                    If .dgvempleave.CurrentRow.IsNewRow Then
                        .dgvempleave.Rows.Add()

                        With .dgvempleave.Rows(.dgvempleave.RowCount - 2)
                            .Cells("elv_Type").Value = lstbxleavtyp.SelectedItem.ToString
                            .Cells("elv_Type").Selected = True
                        End With
                    Else
                        .dgvempleave.CurrentRow.Cells("elv_Type").Value = lstbxleavtyp.SelectedItem.ToString
                    End If
                Else
                    .dgvempleave.Rows.Add()

                    With .dgvempleave.Rows(.dgvempleave.RowCount - 2)
                        .Cells("elv_Type").Value = lstbxleavtyp.SelectedItem.ToString
                        .Cells("elv_Type").Selected = True
                    End With
                End If
            End With

            Me.Hide()
        End If

    End Sub

    Private Sub ListBox1_Button1_KeyDown(sender As Object, e As KeyEventArgs) Handles Button1.KeyDown, Me.KeyDown, _
                                                                                      lstbxleavtyp.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Hide()
        ElseIf e.KeyCode = Keys.Enter Then
            ListBox1_Button1_Event(sender, e)
        End If
    End Sub

    Private Sub leavtyp_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        Me.Hide()
    End Sub

End Class