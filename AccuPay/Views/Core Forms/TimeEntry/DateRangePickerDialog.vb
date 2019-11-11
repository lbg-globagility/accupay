Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Data
Imports Microsoft.EntityFrameworkCore

Public Class DateRangePickerDialog

    Private _payFrequencyId As Integer

    Private _currentPayperiod As PayPeriod

    Private _payperiodModels As IList(Of PayperiodModel)

    Private _payperiods As IList(Of PayPeriod)

    Public Year As Integer = Date.Today.Year

    Private _start As Date

    Private _end As Date

    Private _rowId As Integer

    Private _passedPayPeriod As IPayPeriod

    Sub New(Optional passedPayPeriod As IPayPeriod = Nothing)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _passedPayPeriod = passedPayPeriod

    End Sub

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

    Public ReadOnly Property Id As Integer
        Get
            Return _rowId
        End Get
    End Property

    Private Async Sub DateRangePickerDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PayperiodsDataGridView.AutoGenerateColumns = False

        _payFrequencyId = PayrollTools.PayFrequencyMonthlyId

        Await LoadPayPeriods()

        Await ChooseSelectedDefaultPayPeriod()

        lblYear.Text = Convert.ToString(Year)

        btnDecrementYear.Text = String.Concat("← ", (Year - 1))
        btnIncrementYear.Text = String.Concat((Year + 1), " →")
    End Sub

    Private Async Function ChooseSelectedDefaultPayPeriod() As Task

        Dim currentPayPeriod As IPayPeriod

        If _passedPayPeriod Is Nothing Then

            currentPayPeriod = Await PayrollTools.GetCurrentlyWorkedOnPayPeriodByCurrentYear(New List(Of IPayPeriod)(_payperiods))
        Else
            currentPayPeriod = _passedPayPeriod

        End If

        Dim currentPayPeriodModel = _payperiodModels.
                                Where(Function(p) Nullable.Equals(p.RowID, currentPayPeriod.RowID)).
                                LastOrDefault

        If currentPayPeriodModel Is Nothing Then Return

        Dim currentPayPeriodIndex = _payperiodModels.IndexOf(currentPayPeriodModel)

        If currentPayPeriodIndex > PayperiodsDataGridView.Rows.Count - 1 Then Return

        PayperiodsDataGridView.ClearSelection()

        PayperiodsDataGridView.Rows(currentPayPeriodIndex).Selected = True

        PayperiodsDataGridView.Rows(currentPayPeriodIndex).Cells(0).Selected = True

        PayperiodsDataGridView.CurrentCell = PayperiodsDataGridView.Rows(currentPayPeriodIndex).Cells(0)

        UpdateCurrentPayPeriod(currentPayPeriodModel)

    End Function

    Private Async Function LoadPayPeriods() As Task
        Using context = New PayrollContext()
            _payperiods = Await context.PayPeriods.
                Where(Function(p) p.Year = Year).
                Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                Where(Function(p) Nullable.Equals(p.PayFrequencyID, _payFrequencyId)).
                ToListAsync()
        End Using

        Dim payPeriodsWithPaystubCount = PayPeriodStatusData.GetPeriodsWithPaystubCount()

        _payperiodModels = _payperiods.Select(Function(p) New PayperiodModel(p)).ToList()

        PayperiodsDataGridView.DataSource = _payperiodModels

        Dim index = 0
        For Each payperiod In _payperiodModels

            If payperiod.IsClosed Then
                PayperiodsDataGridView.Rows(index).DefaultCellStyle.ForeColor = Color.Black
                payperiod.Status = PayPeriodStatusData.PayPeriodStatus.Closed
            Else
                'check if this open payperiod is already modified
                If payPeriodsWithPaystubCount.Any(Function(p) p.RowID.Value = payperiod.RowID) Then

                    PayperiodsDataGridView.Rows(index).DefaultCellStyle.SelectionBackColor = Color.Green
                    PayperiodsDataGridView.Rows(index).DefaultCellStyle.BackColor = Color.Yellow

                    payperiod.Status = PayPeriodStatusData.PayPeriodStatus.Processing
                Else
                    PayperiodsDataGridView.Rows(index).DefaultCellStyle.ForeColor = Color.Gray

                End If

            End If

            index += 1

        Next

    End Function

    Private Sub PayperiodsDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles PayperiodsDataGridView.SelectionChanged
        Dim payperiod = DirectCast(PayperiodsDataGridView.CurrentRow.DataBoundItem, PayperiodModel)

        UpdateCurrentPayPeriod(payperiod)
    End Sub

    Private Sub UpdateCurrentPayPeriod(payperiod As PayperiodModel)
        _currentPayperiod = payperiod.PayPeriod

        _rowId = payperiod.RowID

        _start = payperiod.PayFromDate
        _end = payperiod.PayToDate
    End Sub

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If Await PayrollTools.
                    ValidatePayPeriodAction(_currentPayperiod.RowID) = False Then Return

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

        Public Property Status As PayPeriodStatusData.PayPeriodStatus

        Public ReadOnly Property RowID As Integer
            Get
                Return PayPeriod.RowID.Value
            End Get
        End Property

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

        Public ReadOnly Property IsClosed As Boolean
            Get
                Return PayPeriod.IsClosed
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

    Private Async Sub lblDecrementIncrementYear_ClickedAsync(sender As Object, e As EventArgs) _
        Handles btnDecrementYear.Click, btnIncrementYear.Click

        Dim linkLabel = DirectCast(sender, Button)
        Dim factor = Convert.ToInt32(linkLabel.Tag)

        Year = Year + factor

        lblYear.Text = Convert.ToString(Year)

        Await LoadPayPeriods()

        btnDecrementYear.Text = String.Concat("← ", (Year - 1))
        btnIncrementYear.Text = String.Concat((Year + 1), " →")
    End Sub

End Class