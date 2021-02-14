Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Enums
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class SelectPayPeriodDialog

    Public Property SelectedPayPeriod As PayPeriod

    Private _currentYear As Integer = Date.Now.Year

    Private _currentlyWorkedOnPayPeriod As IPayPeriod

    Private ReadOnly _payPeriodRepository As IPayPeriodRepository

    Sub New()

        InitializeComponent()

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)
    End Sub

    Private Async Sub SelectPayPeriod_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PayPeriodGridView.AutoGenerateColumns = False

        _currentlyWorkedOnPayPeriod = Await _payPeriodRepository.GetCurrentPayPeriodAsync(
            organizationId:=z_OrganizationID,
            currentUserId:=z_User)

        Await UpdatePages()
    End Sub

    Async Function LoadPeriods(year As Integer) As Task

        Dim payPeriods = (Await _payPeriodRepository.
            GetYearlyPayPeriodsAsync(
                organizationId:=z_OrganizationID,
                year:=year,
                currentUserId:=z_User)).
            ToList()

        PayPeriodGridView.DataSource = payPeriods
        Dim index As Integer = 0
        For Each period In payPeriods
            HighlightOpenPayPeriod(index, period)

            index += 1
        Next

        Dim currentlyWorkedOnPayPeriodIndex As Integer = 0

        If _currentlyWorkedOnPayPeriod IsNot Nothing Then

            Dim currentlyWorkedOnPayPeriod = payPeriods.
                Where(Function(p) p.PayFromDate = _currentlyWorkedOnPayPeriod.PayFromDate).
                FirstOrDefault()

            If currentlyWorkedOnPayPeriod IsNot Nothing Then

                currentlyWorkedOnPayPeriodIndex = payPeriods.IndexOf(currentlyWorkedOnPayPeriod)
            End If

        End If

        If currentlyWorkedOnPayPeriodIndex > PayPeriodGridView.Rows.Count - 1 Then Return

        PayPeriodGridView.Rows(currentlyWorkedOnPayPeriodIndex).Selected = True

        PayPeriodGridView.Rows(currentlyWorkedOnPayPeriodIndex).Cells(Column15.Index).Selected = True

    End Function

    Private Sub HighlightOpenPayPeriod(index As Integer, payPeriod As PayPeriod)

        Dim currentRow = PayPeriodGridView.Rows(index)

        If payPeriod.Status = PayPeriodStatus.Closed Then
            PayPeriodGridView.Rows(index).DefaultCellStyle.ForeColor = Color.Black

        ElseIf payPeriod.Status = PayPeriodStatus.Open Then

            PayPeriodGridView.Rows(index).DefaultCellStyle.SelectionBackColor = Color.Green
            PayPeriodGridView.Rows(index).DefaultCellStyle.BackColor = Color.Yellow
        Else
            PayPeriodGridView.Rows(index).DefaultCellStyle.ForeColor = Color.Gray

        End If

    End Sub

    Private Async Sub linkNxt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkNxt.LinkClicked
        Await MoveNextYear()
    End Sub

    Private Async Sub linkPrev_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkPrev.LinkClicked
        Await MovePreviousYear()
    End Sub

    Private Async Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
        PayPeriodGridView.EndEdit()

        If PayPeriodGridView.RowCount <> 0 Then

            Dim payPeriod = DirectCast(PayPeriodGridView.CurrentRow.DataBoundItem, PayPeriod)

            If payPeriod IsNot Nothing Then

                Dim payPeriodService = MainServiceProvider.GetRequiredService(Of IPayPeriodDataService)

                If payPeriod.RowID Is Nothing Then
                    payPeriod = Await payPeriodService.CreateAsync(
                        organizationId:=z_OrganizationID,
                        month:=payPeriod.Month,
                        year:=payPeriod.Year,
                        isFirstHalf:=payPeriod.IsFirstHalf,
                        currentUserId:=z_User)
                End If

                Dim validate = Await payPeriodService.ValidatePayPeriodActionAsync(payPeriod.RowID)

                If validate = FunctionResult.Failed Then

                    MessageBoxHelper.Warning(validate.Message)
                    Return
                End If

                SelectedPayPeriod = Await _payPeriodRepository.GetByIdAsync(payPeriod.RowID.Value)

            End If

        End If

        DialogResult = DialogResult.OK
    End Sub

    Private Sub dgvpaypers_SelectionChanged(sender As Object, e As EventArgs) Handles PayPeriodGridView.SelectionChanged
        UpdateStatusLabel()
    End Sub

    Private Async Function MoveNextYear() As Task
        _currentYear += 1
        Await UpdatePages()
    End Function

    Private Async Function MovePreviousYear() As Task
        _currentYear -= 1
        Await UpdatePages()
    End Function

    Private Async Function UpdatePages() As Task
        Panel2.Enabled = False
        linkNxt.Text = (_currentYear + 1) & " →"
        linkPrev.Text = "← " & (_currentYear - 1)

        Await LoadPeriods(_currentYear)

        If PayPeriodGridView.RowCount <> 0 Then
            UpdateStatusLabel()
        Else
            lblpapyperiodval.Text = ""
            ResetPayPeriodStatusLabel()
        End If

        Panel2.Enabled = True
    End Function

    Private Sub UpdateStatusLabel()
        If PayPeriodGridView.CurrentRow IsNot Nothing AndAlso PayPeriodGridView.CurrentRow.DataBoundItem IsNot Nothing Then

            Dim currentPayPeriod = DirectCast(PayPeriodGridView.CurrentRow.DataBoundItem, PayPeriod)

            If currentPayPeriod Is Nothing Then Return

            Dim dateFrom = currentPayPeriod.PayFromDate.ToShortDateString()
            Dim dateTo = currentPayPeriod.PayToDate.ToShortDateString()

            lblpapyperiodval.Text = ": from " & dateFrom & " to " & dateTo

            UpdatePayPeriodStatusLabel(currentPayPeriod)
        Else
            lblpapyperiodval.Text = ""
            ResetPayPeriodStatusLabel()
        End If
    End Sub

    Private Sub UpdatePayPeriodStatusLabel(currentPayPeriod As PayPeriod)

        If currentPayPeriod Is Nothing Then Return

        Select Case currentPayPeriod.Status
            Case PayPeriodStatus.Pending
                PayPeriodStatusLabel.Text = "PENDING"
                PayPeriodStatusLabel.BackColor = Color.Blue
                PayPeriodStatusLabel.ForeColor = Color.White
            Case PayPeriodStatus.Open
                PayPeriodStatusLabel.Text = "OPEN"
                PayPeriodStatusLabel.BackColor = Color.Green
                PayPeriodStatusLabel.ForeColor = Color.White
            Case PayPeriodStatus.Closed
                PayPeriodStatusLabel.Text = "CLOSED"
                PayPeriodStatusLabel.BackColor = Color.Gray
                PayPeriodStatusLabel.ForeColor = Color.Black
        End Select
    End Sub

    Private Sub ResetPayPeriodStatusLabel()
        PayPeriodStatusLabel.ForeColor = Color.Black
        PayPeriodStatusLabel.BackColor = Color.White
        PayPeriodStatusLabel.ResetText()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        Dim n_link As New LinkLabel.Link

        If keyData = Keys.Enter Then

            OkButton.PerformClick()

            Return True

        ElseIf keyData = Keys.Escape And PayPeriodGridView.IsCurrentCellInEditMode = False Then

            CloseButton.PerformClick()

            Return True

        ElseIf keyData = Keys.Left Then

            n_link.Name = "linkPrev"

            linkPrev_LinkClicked(linkPrev, New LinkLabelLinkClickedEventArgs(n_link))

            Return True

        ElseIf keyData = Keys.Right Then

            n_link.Name = "linkNxt"

            linkNxt_LinkClicked(linkNxt, New LinkLabelLinkClickedEventArgs(n_link))

            Return True
        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If
    End Function

End Class
