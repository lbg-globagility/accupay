Option Strict On

Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Public Class DateRangePickerDialog

    Private _payFrequencyId As Integer = 1

    Private _currentPayperiod As PayPeriod

    Private _payperiodModels As IList(Of PayperiodModel)

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

        _payperiodModels = _payperiods.Select(Function(p) New PayperiodModel(p)).ToList()

        PayperiodsDataGridView.DataSource = _payperiodModels
    End Sub

    Private Sub PayperiodsDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles PayperiodsDataGridView.SelectionChanged
        Dim payperiod = DirectCast(PayperiodsDataGridView.CurrentRow.DataBoundItem, PayperiodModel)

        _currentPayperiod = payperiod.PayPeriod

        _start = payperiod.PayFromDate
        _end = payperiod.PayToDate
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DialogResult = DialogResult.OK
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DialogResult = DialogResult.Cancel
    End Sub

    Private Class PayperiodModel

        Public ReadOnly PayPeriod As PayPeriod

        Public Sub New(payperiod As PayPeriod)
            Me.PayPeriod = payperiod
        End Sub

        Public ReadOnly Property PayFromDate As Date
            Get
                Return PayPeriod.PayFromDate
            End Get
        End Property

        Public ReadOnly Property PayToDate As Date
            Get
                Return PayPeriod.PayToDate
            End Get
        End Property

        Public ReadOnly Property Period As String
            Get
                If PayPeriod.IsMonthly Then
                    Dim month = New Date(PayPeriod.Year, PayPeriod.Month, 1)
                    Dim halfNo = String.Empty

                    If PayPeriod.IsFirstHalf Then
                        halfNo = "1st Half"
                    ElseIf PayPeriod.IsEndOfTheMonth Then
                        halfNo = "2nd Half"
                    End If

                    Return $"{month.ToString("MMM")} {halfNo}"
                ElseIf PayPeriod.IsWeekly Then
                    ' Not implemented yet
                    Return String.Empty
                Else
                    Return String.Empty
                End If
            End Get
        End Property

    End Class

End Class
