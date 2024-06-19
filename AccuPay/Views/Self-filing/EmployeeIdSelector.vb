Public Class EmployeeIdSelector

    Public Sub New(subInfo As List(Of SelfFilingForm.EmployeeSubInfo))

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DataGridView1.AutoGenerateColumns = False
        DataGridView1.DataSource = subInfo
    End Sub

    Public Function GetSelectedSubInfo() As SelfFilingForm.EmployeeSubInfo
        If DataGridView1 Is Nothing AndAlso DataGridView1.CurrentRow Is Nothing Then Return Nothing
        Return DirectCast(DataGridView1.CurrentRow.DataBoundItem, SelfFilingForm.EmployeeSubInfo)
    End Function

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_KeyDown(sender As Object, e As KeyEventArgs) Handles DataGridView1.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            DialogResult = DialogResult.OK
        End If
    End Sub

End Class