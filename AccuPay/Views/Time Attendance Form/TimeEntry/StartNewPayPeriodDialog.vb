Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Enums
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class StartNewPayPeriodDialog

    Private _currentPayperiod As PayPeriod

    Private _payperiodModels As IList(Of PayperiodModel)

    Private _payPeriods As IList(Of PayPeriod)

    Public Year As Integer = Date.Today.Year

    Private _start As Date

    Private _end As Date

    Private _rowId As Integer?

    Private _passedPayPeriod As IPayPeriod

    Private ReadOnly _payPeriodRepository As IPayPeriodRepository

    Sub New(Optional passedPayPeriod As IPayPeriod = Nothing)

        InitializeComponent()

        _passedPayPeriod = passedPayPeriod

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)

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

    Public ReadOnly Property Id As Integer?
        Get
            Return _rowId
        End Get
    End Property

    Private Async Sub DateRangePickerDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PayperiodsGridView.AutoGenerateColumns = False

        Await LoadPayPeriods()

        Await ChooseSelectedDefaultPayPeriod()

        lblYear.Text = Convert.ToString(Year)

        btnDecrementYear.Text = String.Concat("← ", (Year - 1))
        btnIncrementYear.Text = String.Concat((Year + 1), " →")
    End Sub

    Private Async Function ChooseSelectedDefaultPayPeriod() As Task

        Dim currentPayPeriod As IPayPeriod

        If _passedPayPeriod Is Nothing Then

            currentPayPeriod = Await _payPeriodRepository.GetCurrentPayPeriodAsync(
                organizationId:=z_OrganizationID,
                currentUserId:=z_User)
        Else
            currentPayPeriod = _passedPayPeriod

        End If

        Dim currentPayPeriodModel = _payperiodModels.
            Where(Function(p) p.PayFromDate = currentPayPeriod.PayFromDate).
            LastOrDefault

        If currentPayPeriodModel Is Nothing Then Return

        Dim currentPayPeriodIndex = _payperiodModels.IndexOf(currentPayPeriodModel)

        If currentPayPeriodIndex > PayperiodsGridView.Rows.Count - 1 Then Return

        PayperiodsGridView.ClearSelection()

        PayperiodsGridView.Rows(currentPayPeriodIndex).Selected = True

        PayperiodsGridView.Rows(currentPayPeriodIndex).Cells(0).Selected = True

        PayperiodsGridView.CurrentCell = PayperiodsGridView.Rows(currentPayPeriodIndex).Cells(0)

        UpdateCurrentPayPeriod(currentPayPeriodModel)

    End Function

    Private Async Function LoadPayPeriods() As Task

        _payPeriods = (Await _payPeriodRepository.
            GetYearlyPayPeriodsAsync(
                organizationId:=z_OrganizationID,
                year:=Me.Year,
                currentUserId:=z_User)).
            ToList()

        _payperiodModels = _payPeriods.Select(Function(p) New PayperiodModel(p)).ToList()

        PayperiodsGridView.DataSource = _payperiodModels

        Dim index = 0
        For Each payperiod In _payperiodModels

            If payperiod.IsClosed Then
                PayperiodsGridView.Rows(index).DefaultCellStyle.ForeColor = Color.Black

            ElseIf payperiod.IsOpen Then
                PayperiodsGridView.Rows(index).DefaultCellStyle.SelectionBackColor = Color.Green
                PayperiodsGridView.Rows(index).DefaultCellStyle.BackColor = Color.Yellow
            Else
                PayperiodsGridView.Rows(index).DefaultCellStyle.ForeColor = Color.Gray

            End If

            index += 1

        Next

    End Function

    Private Sub PayperiodsDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles PayperiodsGridView.SelectionChanged
        Dim payperiod = DirectCast(PayperiodsGridView.CurrentRow.DataBoundItem, PayperiodModel)

        UpdateCurrentPayPeriod(payperiod)
    End Sub

    Private Sub UpdateCurrentPayPeriod(payperiod As PayperiodModel)
        _currentPayperiod = payperiod.PayPeriod

        _rowId = payperiod.RowID

        _start = payperiod.PayFromDate
        _end = payperiod.PayToDate
    End Sub

    Private Async Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click

        Await FunctionUtils.TryCatchFunctionAsync("Start New Payroll",
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of PayPeriodDataService)

                Me.Cursor = Cursors.WaitCursor

                Await dataService.StartStatusAsync(
                    organizationId:=z_OrganizationID,
                    month:=_currentPayperiod.Month,
                    year:=_currentPayperiod.Year,
                    isFirstHalf:=_currentPayperiod.IsFirstHalf,
                    currentUserId:=z_User)

                Await TimeEntrySummaryForm.LoadPayPeriods()

                Await PayStubForm.VIEW_payperiodofyear()

                DialogResult = DialogResult.OK
            End Function)

        Me.Cursor = Cursors.Default
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DialogResult = DialogResult.Cancel
    End Sub

    Private Class PayperiodModel

        Public ReadOnly PayPeriod As PayPeriod

        Public Sub New(payperiod As PayPeriod)
            Me.PayPeriod = payperiod
        End Sub

        Public ReadOnly Property RowID As Integer?
            Get
                Return PayPeriod.RowID
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
                Return PayPeriod.Status = PayPeriodStatus.Closed
            End Get
        End Property

        Public ReadOnly Property IsOpen As Boolean
            Get
                Return PayPeriod.Status = PayPeriodStatus.Open
            End Get
        End Property

        Public ReadOnly Property IsPending As Boolean
            Get
                Return PayPeriod.Status = PayPeriodStatus.Pending
            End Get
        End Property

        Public ReadOnly Property Period As String
            Get
                If PayPeriod.IsSemiMonthly Then
                    Dim month = New Date(PayPeriod.Year, PayPeriod.Month, 1)
                    Dim halfNo = String.Empty

                    If PayPeriod.IsFirstHalf Then
                        halfNo = "1st Half"
                    ElseIf PayPeriod.IsEndOfTheMonth Then
                        halfNo = "2nd Half"
                    End If

                    Return $"{month:MMM} {halfNo}"
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

        Year += factor

        lblYear.Text = Convert.ToString(Year)

        Await LoadPayPeriods()

        btnDecrementYear.Text = String.Concat("← ", (Year - 1))
        btnIncrementYear.Text = String.Concat((Year + 1), " →")
    End Sub

End Class
