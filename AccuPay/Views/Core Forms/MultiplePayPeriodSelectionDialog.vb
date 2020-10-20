Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class MultiplePayPeriodSelectionDialog

    Public Property ReportIndex As Integer

    Public Property ShowLoanTypePanel As Boolean = False
    Public Property SHowPayrollSummaryPanel As Boolean = False
    Public Property ShowDeclaredOrActualOptionsPanel As Boolean = False

    Private _currentYear As Integer = CDate(dbnow).Year

    Private _currentlyWorkedOnPayPeriod As IPayPeriod

    Private ReadOnly _payPeriodRepository As PayPeriodRepository

    Private ReadOnly _productRepository As ProductRepository

    Private ReadOnly _policy As PolicyHelper

    Sub New()

        InitializeComponent()

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of PayPeriodRepository)

        _productRepository = MainServiceProvider.GetRequiredService(Of ProductRepository)

        _policy = MainServiceProvider.GetRequiredService(Of PolicyHelper)
    End Sub

    Public ReadOnly Property PayPeriodFromID As Integer?
        Get
            Dim startPayPeriod = GetStartPayPeriod()
            If startPayPeriod Is Nothing Then
                Return Nothing
            Else
                Return startPayPeriod.RowID
            End If
        End Get
    End Property

    Public ReadOnly Property PayPeriodToID As Integer?
        Get
            Dim endPayPeriod = GetEndPayPeriod()
            If endPayPeriod Is Nothing Then
                Return Nothing
            Else
                Return endPayPeriod.RowID
            End If
        End Get
    End Property

    Public ReadOnly Property DateFrom As Date?
        Get
            Dim startPayPeriod = GetStartPayPeriod()
            If startPayPeriod Is Nothing Then
                Return Nothing
            Else
                Return startPayPeriod.PayFromDate
            End If
        End Get
    End Property

    Public ReadOnly Property DateTo As Date?
        Get
            Dim endPayPeriod = GetEndPayPeriod()
            If endPayPeriod Is Nothing Then
                Return Nothing
            Else
                Return endPayPeriod.PayToDate
            End If
        End Get
    End Property

    Public ReadOnly Property IsActual As Boolean
        Get
            Return ActualRadioButton.Checked
        End Get
    End Property

    Public ReadOnly Property LoanTypeId As Integer?
        Get
            Return CInt(LoanTypeComboBox.SelectedValue)
        End Get
    End Property

    Private _selectedPayPeriods As List(Of PayPeriod)

#Region "Loan Types"

    Protected Overrides Async Sub OnLoad(e As EventArgs)

        Dim desiredHeight = Me.Height - LoanTypePanel.Height - PayrollSummaryPanel.Height - DeclaredOrActualPanel.Height

        If ShowLoanTypePanel Then

            Await LoadLoanTypesAsync()
            LoanTypePanel.Visible = True
            ReminderLabel.Visible = True

            desiredHeight += LoanTypePanel.Height

        End If

        If SHowPayrollSummaryPanel Then

            PayrollSummaryPanel.Visible = True
            ReminderLabel.Visible = True

            With SalaryDistributionComboBox
                .Visible = True
                .Items.Add(PayrollSummaryCategory.All)
                .Items.Add(PayrollSummaryCategory.Cash)
                .Items.Add(PayrollSummaryCategory.DirectDeposit)
            End With

            desiredHeight += PayrollSummaryPanel.Height

        End If

        If ShowDeclaredOrActualOptionsPanel Then

            DeclaredOrActualPanel.Visible = True

            If _policy.ShowActual = False Then

                DeclaredOrActualPanel.Visible = False
                DeclaredRadioButton.Checked = True
            End If

            desiredHeight += DeclaredOrActualPanel.Height

        End If

        Me.Height = desiredHeight

        MyBase.OnLoad(e)
    End Sub

    Private Async Function LoadLoanTypesAsync() As Task
        Dim loanTypes = Await _productRepository.GetLoanTypesAsync(z_OrganizationID)

        LoanTypeComboBox.DataSource = loanTypes.ToList()
    End Function

#End Region

    Private Async Sub PayrollSummaDateSelection_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        PayperiodsGridView.AutoGenerateColumns = False

        linkPrev.Text = "← " & (_currentYear - 1)
        linkNxt.Text = (_currentYear + 1) & " →"

        _currentlyWorkedOnPayPeriod = Await _payPeriodRepository.GetCurrentPayPeriodAsync(z_OrganizationID)

        DateFromTextLabel.Text = ""
        DateToTextLabel.Text = ""

        _selectedPayPeriods = New List(Of PayPeriod)
        Await LoadPayPeriods()
    End Sub

    Private Sub PayperiodsGridView_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles PayperiodsGridView.CellEndEdit
        UpdateDateDescription()

    End Sub

    Private Sub dgvpayperiod_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles PayperiodsGridView.CellContentClick

        If PayperiodsGridView.CurrentRow Is Nothing Then Return

        Dim selectedPayPeriod = CType(PayperiodsGridView.CurrentRow.DataBoundItem, PayPeriodWrapper)

        If selectedPayPeriod Is Nothing Then Return

        If e.ColumnIndex <> IsCheckedColumn.Index Then Return

        Dim checkBox As DataGridViewCheckBoxCell = CType(PayperiodsGridView.CurrentRow.Cells(IsCheckedColumn.Index), DataGridViewCheckBoxCell)
        Dim originalState = If(checkBox Is Nothing, False, (CType(checkBox.Value, Boolean)))

        If originalState = True Then

            _selectedPayPeriods.Remove(selectedPayPeriod.Model)
        Else

            'If the action is to check a cell and there currently two checked cells,
            'uncheck the earliest check cell so there will be always only 2 checked cells
            If _selectedPayPeriods.Count = 2 Then

                Dim earliestSelectedPayPeriod = _selectedPayPeriods(0)

                _selectedPayPeriods.Remove(earliestSelectedPayPeriod)

                Dim payPeriods = CType(PayperiodsGridView.DataSource, List(Of PayPeriodWrapper))
                Dim earliestSelectedPayPeriodWrapper = payPeriods.FirstOrDefault(Function(p) p.DateFrom = earliestSelectedPayPeriod.PayFromDate)

                If earliestSelectedPayPeriodWrapper IsNot Nothing Then

                    earliestSelectedPayPeriodWrapper.IsChecked = False
                End If

            End If

            _selectedPayPeriods.Add(selectedPayPeriod.Model)

        End If

        PayperiodsGridView.EndEdit()
        PayperiodsGridView.Refresh()
    End Sub

    Private Async Sub linkNxt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkNxt.LinkClicked
        _currentYear = _currentYear + 1
        Await ChangePageLinks()
    End Sub

    Private Async Sub linkPrev_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkPrev.LinkClicked
        _currentYear = _currentYear - 1
        Await ChangePageLinks()
    End Sub

    Private Async Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        PayperiodsGridView.EndEdit()

        If SalaryDistributionComboBox.Visible AndAlso SalaryDistributionComboBox.Text = String.Empty Then
            WarnBalloon("Please select a Salary Distribution", "Invalid Salary Distribution", SalaryDistributionComboBox, SalaryDistributionComboBox.Width - 17, -69)
            Return
        End If

        If _selectedPayPeriods.Any() AndAlso DateFrom.HasValue AndAlso DateTo.HasValue Then

            Await CreatePayPeriodIfNotExists(GetStartPayPeriod())
            Await CreatePayPeriodIfNotExists(GetEndPayPeriod())

            If Not PayPeriodFromID.HasValue OrElse Not PayPeriodToID.HasValue Then

                'log: cannot create new pay period'
                MessageBoxHelper.Warning("Cannot get pay period data. Close and reopen the dialog then try again.")
                Return
            End If
        Else

            WarnBalloon("Please select a pay period. ", "Invalid Pay Period", linkPrev, 30, -470)
            Return
        End If

        Me.DialogResult = DialogResult.OK
    End Sub

    Private Async Function CreatePayPeriodIfNotExists(currentPayPeriod As PayPeriod) As Task
        If currentPayPeriod.RowID Is Nothing Then

            Dim newlyCreatedId = Await CreateNewPayPeriod(currentPayPeriod)

            currentPayPeriod.RowID = newlyCreatedId

        End If
    End Function

    Private Shared Async Function CreateNewPayPeriod(startPayPeriod As PayPeriod) As Task(Of Integer?)
        Dim dataService = MainServiceProvider.GetRequiredService(Of PayPeriodDataService)
        Dim newPayPeriod = Await dataService.CreatesAsync(
            organizationId:=z_OrganizationID,
            month:=startPayPeriod.Month,
            year:=startPayPeriod.Year,
            isFirstHalf:=startPayPeriod.IsFirstHalf,
            createdByUserId:=z_User)

        Return newPayPeriod.RowID
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = Keys.Escape Then
            btnClose_Click(btnClose, New EventArgs)

            Return True
        Else
            Return MyBase.ProcessCmdKey(msg, keyData)
        End If
    End Function

    Private Sub ComboBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles SalaryDistributionComboBox.KeyPress
        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 8 Then
            SalaryDistributionComboBox.Text = String.Empty
            SalaryDistributionComboBox.SelectedIndex = -1
        End If
    End Sub

    Private Async Function LoadPayPeriods() As Task

        Dim payPeriods = (Await _payPeriodRepository.
            GetYearlyPayPeriodsAsync(
                organizationId:=z_OrganizationID,
                year:=Me._currentYear,
                currentUserId:=z_User)).
            Select(Function(p) New PayPeriodWrapper(p)).
            ToList()

        Dim focusedPayPeriod As PayPeriodWrapper = Nothing

        If _selectedPayPeriods.Any() Then

            Dim startPayPeriod = _selectedPayPeriods(0)
            focusedPayPeriod = CheckPayPeriodIfItIsInCurrentPage(startPayPeriod?.PayFromDate, payPeriods)

            If _selectedPayPeriods.Count > 1 Then

                Dim endPayPeriod = _selectedPayPeriods(1)
                CheckPayPeriodIfItIsInCurrentPage(endPayPeriod?.PayFromDate, payPeriods)
            End If
        Else
            focusedPayPeriod = CheckPayPeriodIfItIsInCurrentPage(_currentlyWorkedOnPayPeriod?.PayFromDate, payPeriods)

            If focusedPayPeriod IsNot Nothing Then

                _selectedPayPeriods.Add(focusedPayPeriod.Model)
            End If

        End If

        PayperiodsGridView.DataSource = payPeriods
        FocusPayPeriod(focusedPayPeriod, payPeriods)
        UpdateDateDescription()

    End Function

    Private Function CheckPayPeriodIfItIsInCurrentPage(payPeriodPayFromDate As Date?, payPeriods As List(Of PayPeriodWrapper)) As PayPeriodWrapper

        If payPeriodPayFromDate Is Nothing Then Return Nothing

        Dim currentPayPeriod = payPeriods.Where(Function(p) p.DateFrom = payPeriodPayFromDate.Value).FirstOrDefault()

        If currentPayPeriod Is Nothing Then Return Nothing

        currentPayPeriod.IsChecked = True

        Return currentPayPeriod

    End Function

    Private Sub FocusPayPeriod(currentPayPeriod As PayPeriodWrapper, payPeriods As List(Of PayPeriodWrapper))

        If currentPayPeriod IsNot Nothing Then

            Dim currentPayPeriodWrapperIndex = payPeriods.IndexOf(currentPayPeriod)
            If currentPayPeriodWrapperIndex > PayperiodsGridView.Rows.Count - 1 Then Return

            Dim currentPayPeriodRow = PayperiodsGridView.Rows(currentPayPeriodWrapperIndex)

            If currentPayPeriodRow IsNot Nothing Then

                'currentPayPeriodRow.Selected = True
                currentPayPeriodRow.Cells(DateFromColumn.Index).Selected = True

            End If

        End If
    End Sub

    Private Function GetStartPayPeriod() As PayPeriod
        If _selectedPayPeriods Is Nothing OrElse Not _selectedPayPeriods.Any Then Return Nothing
        Return _selectedPayPeriods.OrderBy(Function(p) p.PayFromDate).FirstOrDefault()
    End Function

    Private Function GetEndPayPeriod() As PayPeriod
        If _selectedPayPeriods Is Nothing OrElse Not _selectedPayPeriods.Any Then Return Nothing
        Return _selectedPayPeriods.OrderByDescending(Function(p) p.PayFromDate).FirstOrDefault()
    End Function

    Private Sub UpdateDateDescription()

        DateFromTextLabel.Text = FormatDateString(DateFrom)
        DateToTextLabel.Text = FormatDateString(DateTo)
    End Sub

    Private Shared Function FormatDateString(dateInput As Date?) As String
        Return If(dateInput Is Nothing, String.Empty, dateInput.Value.ToString("MMMM d, yyyy"))
    End Function

    Private Async Function ChangePageLinks() As Task
        linkPrev.Text = "← " & (_currentYear - 1)
        linkNxt.Text = (_currentYear + 1) & " →"

        Dim sel_tab_pg = TabControl1.SelectedTab

        Await LoadPayPeriods()
    End Function

    Private Class PayPeriodWrapper

        Public ReadOnly Property RowID As Integer?
            Get
                Return Model.RowID
            End Get
        End Property

        Public ReadOnly Property DateFrom As Date
            Get
                Return Model.PayFromDate
            End Get
        End Property

        Public ReadOnly Property DateTo As Date
            Get
                Return Model.PayToDate
            End Get
        End Property

        Public ReadOnly Model As PayPeriod
        Public Property IsChecked As Boolean

        Sub New(payPeriod As PayPeriod)

            Model = payPeriod

        End Sub

    End Class

End Class