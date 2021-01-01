Option Strict On

Imports AccuPay.Core.Interfaces

Public Class EmployeeResultsDialog

    Private _results As List(Of IEmployeeResult)
    Public ReadOnly Property _generationDescription As String
    Public ReadOnly Property _entityDescription As String

    Public Sub New(results As IEnumerable(Of IEmployeeResult), title As String, generationDescription As String, entityDescription As String)
        InitializeComponent()

        _results = results.OrderBy(Function(r) r.EmployeeFullName).ToList()

        Me.Text = title

        _generationDescription = generationDescription
        _entityDescription = entityDescription
    End Sub

    Private Sub PayrollResultDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgvSuccess.AutoGenerateColumns = False
        dgvFailed.AutoGenerateColumns = False

        Dim successes = _results.
            Where(Function(r) r.IsSuccess).
            ToList()

        Dim failures = _results.
            Where(Function(r) r.IsError).
            ToList()

        dgvSuccess.DataSource = successes
        dgvFailed.DataSource = failures

        tbpSuccess.Text = $"Success ({successes.Count})"
        tbpFailed.Text = $"Failed ({failures.Count})"

        lblMessage.Text = $"{_generationDescription} is done. Successful {_entityDescription}: {successes.Count}; failed {_entityDescription}: {failures.Count}."
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub dgvSuccess_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvSuccess.CellFormatting

        If e.ColumnIndex = SuccessResultColumn.Index Then

            FormatStatusCell(dgvSuccess, e)
        End If

    End Sub

    Private Sub dgvFailed_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvFailed.CellFormatting

        If e.ColumnIndex = FailedResultColumn.Index Then

            FormatStatusCell(dgvFailed, e)
        End If

    End Sub

    Private Sub FormatStatusCell(sender As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs)
        Dim row = CType(sender.Rows(e.RowIndex).DataBoundItem, IEmployeeResult)

        If row Is Nothing Then Return

        e.Value = If(row.IsSuccess, "Success", "Error")
        e.FormattingApplied = True
    End Sub

End Class
