Public Class DepartmentMinWages

    Dim deptRowID As Object = Nothing

    Sub New(ByVal DivisionRowID As Object)

        deptRowID = DivisionRowID

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        Dim n_ReadSQLProcedureToDatatable As _
            New ReadSQLProcedureToDatatable("VIEW_divisionminimumwage",
                                            orgztnID,
                                            deptRowID)

        Dim catchdt As New DataTable

        catchdt = n_ReadSQLProcedureToDatatable.ResultTable

        dgvMinimumWages.Rows.Clear()

        For Each drow As DataRow In catchdt.Rows

            Dim row_array = drow.ItemArray

            dgvMinimumWages.Rows.Add(row_array)

        Next

        MyBase.OnLoad(e)

    End Sub

    Dim isShowAsDialog As Boolean = False

    Public Overloads Function ShowDialog(ByVal someValue As String) As DialogResult

        With Me

            isShowAsDialog = True

            .Text = someValue

        End With

        Return Me.ShowDialog

    End Function

    Private Sub DepartmentMinWages_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub tsbtnSaveMinimum_Click(sender As Object, e As EventArgs) Handles tsbtnSaveMinimum.Click

        dgvMinimumWages.EndEdit(True)

        tsbtnSaveMinimum.Enabled = False

        For Each dgvrow As DataGridViewRow In dgvMinimumWages.Rows

            If dgvrow.IsNewRow Then

                Continue For
            Else

                With dgvrow

                    Dim n_ReadSQLFunction As _
                        New ReadSQLFunction("INSUPD_divisionminimumwage",
                                            "returnvalue",
                                            .Cells("dmw_RowID").Value,
                                            orgztnID,
                                            z_User,
                                            deptRowID,
                                            ValNoComma(.Cells("dmw_Amount").Value),
                                            MYSQLDateFormat(CDate(.Cells("dmw_EffectiveDateFrom").Value)),
                                            MYSQLDateFormat(CDate(.Cells("dmw_EffectiveDateTo").Value)))

                    If .Cells("dmw_RowID").Value = Nothing _
                        And ValNoComma(n_ReadSQLFunction.ReturnValue) > 0 Then
                        .Cells("dmw_RowID").Value = n_ReadSQLFunction.ReturnValue
                    End If

                End With

            End If

        Next

        tsbtnSaveMinimum.Enabled = True

    End Sub

    Private Sub tsbtnCancelMinimum_Click(sender As Object, e As EventArgs) Handles tsbtnCancelMinimum.Click

        tsbtnCancelMinimum.Enabled = False

        OnLoad(New EventArgs)

        tsbtnCancelMinimum.Enabled = True

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape _
            And dgvMinimumWages.IsCurrentCellInEditMode = False Then

            Me.Close()

            Return True
        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub dgvMinimumWages_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvMinimumWages.CellBeginEdit

    End Sub

    Private Sub dgvMinimumWages_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvMinimumWages.CellContentClick

    End Sub

    Private Sub dgvMinimumWages_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvMinimumWages.CellEndEdit

    End Sub

    Private Sub tsbtnDelMinimumWage_Click(sender As Object, e As EventArgs) Handles tsbtnDelMinimumWage.Click

        dgvMinimumWages.Focus()

        Dim dgv_currRow = dgvMinimumWages.CurrentRow

        If dgv_currRow.IsNewRow Then
        Else

            Dim prompt = MessageBox.Show("Are you sure you want to delete this item ?",
                                         "Delete minimum wage",
                                         MessageBoxButtons.YesNoCancel,
                                         MessageBoxIcon.Information)

            If prompt = Windows.Forms.DialogResult.Yes Then

                Dim n_ExecuteQuery As _
                    New ExecuteQuery("DELETE FROM divisionminimumwage WHERE RowID='" & dgv_currRow.Cells("dmw_RowID").Value & "';")

                If n_ExecuteQuery.HasError = False Then

                    dgvMinimumWages.Rows.Remove(dgv_currRow)

                End If

            End If

        End If

    End Sub

End Class