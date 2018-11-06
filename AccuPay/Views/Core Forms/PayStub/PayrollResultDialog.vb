Option Strict On

Public Class PayrollResultDialog

    Private _results As IList(Of PayrollGeneration.Result)

    Public Sub New(results As IList(Of PayrollGeneration.Result))
        InitializeComponent()

        _results = results.OrderBy(Function(r) r.FullName).ToList()
    End Sub

    Private Sub PayrollResultDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgvSuccess.AutoGenerateColumns = False
        dgvFailed.AutoGenerateColumns = False

        Dim successes = _results.
            Where(Function(r) r.Status = PayrollGeneration.ResultStatus.Success).
            ToList()

        Dim failures = _results.
            Where(Function(r) r.Status = PayrollGeneration.ResultStatus.Error).
            ToList()

        dgvSuccess.DataSource = successes
        dgvFailed.DataSource = failures

        tbpSuccess.Text = $"Success ({successes.Count})"
        tbpFailed.Text = $"Failed ({failures.Count})"

        lblMessage.Text = $"Payroll generation is done. Successful paystubs: {successes.Count}; failed paystubs: {failures.Count}."
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        DialogResult = DialogResult.OK
    End Sub

End Class
