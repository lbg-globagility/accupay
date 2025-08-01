Option Strict On

Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports Microsoft.Extensions.DependencyInjection

Public Class SelectPayPeriodSimpleDialog

    Private _currentYear As Integer

    Public Property RowID As Integer

    Public Property PayFromDate As Date

    Public Property PayToDate As Date

    Private ReadOnly _payPeriodRepository As IPayPeriodRepository

    Sub New()

        InitializeComponent()

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)

    End Sub

    Private Sub SelectPayPeriodSimple_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        gridPeriods.AutoGenerateColumns = False

        _currentYear = Date.UtcNow.Year

        UpdateDetails()

        LoadPeriods()
    End Sub

    Private Sub UpdateDetails()
        labelCurrentYear.Text = _currentYear.ToString()

        linkPreviousYear.Text = $"← {_currentYear - 1}"
        linkNextYear.Text = $"{_currentYear + 1} →"
    End Sub

    Private Async Sub LoadPeriods()

        Dim periods = (Await _payPeriodRepository.GetByYearAndPayPrequencyAsync(
            organizationId:=z_OrganizationID,
            year:=_currentYear,
            payFrequencyId:=PayrollTools.PayFrequencySemiMonthlyId)).
        ToList()

        Dim ascOrder = periods.OrderBy(Function(p) p.OrdinalValue).ToList()

        Dim source = ascOrder.
            Select(Function(p) New PeriodDataSource With {
                .Month = $"{Format(New Date(p.Year, p.Month, 1), "MMM").ToUpper()} {If(p.IsFirstHalf, "1st half", "2nd half")}",
                .Period = $"{p.PayFromDate.Date.ToShortDateString()} to {p.PayToDate.Date.ToShortDateString()}",
                .RowID = p.RowID.Value,
                .PayFromDate = p.PayFromDate.Date,
                .PayToDate = p.PayToDate.Date}).
            ToList()

        gridPeriods.DataSource = source
    End Sub

    Private Sub MoveYear(sender As Object, e As EventArgs) Handles linkPreviousYear.Click, linkNextYear.Click

        Dim linkLabel = CType(sender, LinkLabel)

        Dim addend = 0
        If linkLabel.Name = linkPreviousYear.Name Then
            addend = -1
        ElseIf linkLabel.Name = linkNextYear.Name Then
            addend = 1
        End If

        _currentYear += addend

        UpdateDetails()

        LoadPeriods()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim currentRow = gridPeriods.CurrentRow
        If currentRow Is Nothing Then Return

        Dim dataSource = DirectCast(currentRow.DataBoundItem, PeriodDataSource)

        _RowID = dataSource.RowID
        _PayFromDate = dataSource.PayFromDate
        _PayToDate = dataSource.PayToDate
    End Sub

    Private Class PeriodDataSource
        Public Property RowID As Integer
        Public Property Month As String
        Public Property Period As String
        Public Property PayFromDate As Date
        Public Property PayToDate As Date
    End Class

End Class
