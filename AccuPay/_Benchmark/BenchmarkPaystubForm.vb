Public Class BenchmarkPaystubForm

    Private Sub BenchmarkPaystubForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        PayrollForm.listPayrollForm.Remove(Me.Name)
    End Sub

End Class