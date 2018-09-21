Option Strict On

Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Public Class DateRangePickerDialog

    Private _payFrequencyId As Integer = 1

    Private _currentPayperiod As PayPeriod

    Private _payperiods As IList(Of PayPeriod)

    Private _year As Integer = 2018

    Private _start As Date

    Private _end As Date

    Public ReadOnly Property Start As Date
        Get
            Return _start
        End Get
    End Property

    Public ReadOnly Property [End] As Date
        Get
            Return _end
        End Get
    End Property

    Private Async Sub DateRangePickerDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PayperiodsDataGridView.AutoGenerateColumns = False

        Using context = New PayrollContext()
            _payperiods = Await context.PayPeriods.
                Where(Function(p) p.Year = _year).
                Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                Where(Function(p) Nullable.Equals(p.PayFrequencyID, _payFrequencyId)).
                ToListAsync()
        End Using

        PayperiodsDataGridView.DataSource = _payperiods
    End Sub

    Private Sub PayperiodsDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles PayperiodsDataGridView.SelectionChanged
        Dim payperiod = DirectCast(PayperiodsDataGridView.CurrentRow.DataBoundItem, PayPeriod)

        _currentPayperiod = payperiod

        _start = payperiod.PayFromDate
        _end = payperiod.PayToDate
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        _start = DateTimePicker1.Value.Date
    End Sub

    Private Sub DateTimePicker2_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker2.ValueChanged
        _end = DateTimePicker2.Value.Date
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DialogResult = DialogResult.OK
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DialogResult = DialogResult.Cancel
    End Sub

End Class
