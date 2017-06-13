Public Class TaxYearDialog
    ''' <summary>
    ''' The selected year to generate an alphalist for
    ''' </summary>
    ''' <remarks></remarks>
    Public Year As Integer

    Private currentYear As Integer

    Private Sub TaxYearDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        currentYear = Date.Today.Year

        ' Display the years past and after the current year
        For i As Integer = -10 To 10
            cboYear.Items.Add(currentYear + i)
        Next

        btnCreate.DialogResult = Windows.Forms.DialogResult.OK
        btnCancel.DialogResult = Windows.Forms.DialogResult.Cancel

        AcceptButton = btnCreate
        CancelButton = btnCancel
    End Sub

    Private Sub cboYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboYear.SelectedIndexChanged
        Year = cboYear.SelectedItem
    End Sub
End Class