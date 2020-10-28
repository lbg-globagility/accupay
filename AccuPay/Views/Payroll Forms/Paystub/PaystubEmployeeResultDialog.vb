Option Strict On

Imports AccuPay.Data.Enums
Imports AccuPay.Data.Services

Public Class PaystubEmployeeResultDialog

    Private _results As IList(Of PaystubEmployeeResult)
    Public ReadOnly Property _generationDescription As String
    Public ReadOnly Property _entityDescription As String

    Public Sub New(results As IList(Of PaystubEmployeeResult), title As String, generationDescription As String, entityDescription As String)
        InitializeComponent()

        _results = results.OrderBy(Function(r) r.FullName).ToList()

        Me.Text = title

        _generationDescription = generationDescription
        _entityDescription = entityDescription
    End Sub

    Private Sub PayrollResultDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgvSuccess.AutoGenerateColumns = False
        dgvFailed.AutoGenerateColumns = False

        Dim successes = _results.
            Where(Function(r) r.Status = ResultStatus.Success).
            ToList()

        Dim failures = _results.
            Where(Function(r) r.Status = ResultStatus.Error).
            ToList()

        dgvSuccess.DataSource = successes
        dgvFailed.DataSource = failures

        tbpSuccess.Text = $"Success ({successes.Count})"
        tbpFailed.Text = $"Failed ({failures.Count})"

        lblMessage.Text = $"{_generationDescription} is done. Successful {_entityDescription}: {successes.Count}; failed {_entityDescription}: {failures.Count}."
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        DialogResult = DialogResult.OK
    End Sub

End Class
