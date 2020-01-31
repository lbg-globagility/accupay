Public Class TrialForm

    Protected Overrides Sub OnDeactivate(e As EventArgs)

        MyBase.OnDeactivate(e)

    End Sub

    Private Sub DataGridViewX1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewX1.CellContentClick

        If e.ColumnIndex > -1 _
            And e.RowIndex > -1 Then

            'MsgBox(Convert.ToString(DataGridViewX1.Item(e.ColumnIndex, e.RowIndex).Tag))

        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim fsdfsd = CStr("update the application").ToUpper

        Dim except_this_string() As String = {"CALL", "UPDATE"}

        'If fsdfsd.Contains(fsdfsd) Then
        '    MsgBox(CStr(except_this_string.Contains(fsdfsd)))
        'End If

        MsgBox(CStr(FindingWordsInString(fsdfsd,
                                        except_this_string)))

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim n_ShiftTemplater As New ShiftTemplater

        If n_ShiftTemplater.ShowDialog = Windows.Forms.DialogResult.OK Then

        End If

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Dim n_BranchForm As New BranchForm

        If n_BranchForm.ShowDialog = Windows.Forms.DialogResult.OK Then

        End If

    End Sub

    Private Sub TrialForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'TreeViewAdv1.Model = New SortedTreeModel()

    End Sub

    Sub PrintPayslip(sender As Object, e As EventArgs) Handles ToolStripButton2.Click

        Dim params =
            New Object() {3, 1077}

        Dim sql As New SQL("CALL `HyundaiPayslip`(?og_rowid, ?pp_rowid, TRUE, NULL);",
                           params)

        Dim dt As New DataTable
        dt = sql.GetFoundRows.Tables(0)

        Dim rptdoc As New HyundaiPayslip
        rptdoc.SetDataSource(dt)

        Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

        objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgname")
        objText.Text = "HYUNDAI PAMPANGA INC."

        Dim crvwr As New CrysVwr

        crvwr.CrystalReportViewer1.ReportSource = rptdoc
        crvwr.Show()

    End Sub

End Class