Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports Microsoft.Extensions.DependencyInjection

Public Class AssignBonusToLoanForm

    Private ReadOnly _loanSchedule As LoanSchedule
    Private ReadOnly _bonusRepository As BonusRepository
    Private ReadOnly _loanPaymentFromBonusRepo As LoanPaymentFromBonusRepository
    Private ReadOnly _loanService As LoanDataService

    Private _loanPaymentFromBonus As IEnumerable(Of LoanPaymentFromBonus)
    Private _bonuses As IEnumerable(Of Bonus)
    Private _loanPaymentFromBonusModels As List(Of LoanPaymentFromBonusModel)
    Private newTotalBalanceLeft As Decimal
    Private newLoanPayPeriodLeft As Integer

    Public Sub New(loanSchedule As LoanSchedule)

        InitializeComponent()

        _loanSchedule = loanSchedule

        _bonusRepository = MainServiceProvider.GetRequiredService(Of BonusRepository)
        _loanPaymentFromBonusRepo = MainServiceProvider.GetRequiredService(Of LoanPaymentFromBonusRepository)
        _loanService = MainServiceProvider.GetRequiredService(Of LoanDataService)

        newTotalBalanceLeft = _loanSchedule.TotalBalanceLeft
        newLoanPayPeriodLeft = _loanSchedule.LoanPayPeriodLeft

        DisplayLoanScheduleDetails(loanSchedule)

        LoadLoanPaymentFromBonus()
    End Sub

    Private Sub AssignBonusToLoan_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Async Sub LoadLoanPaymentFromBonus()
        _loanPaymentFromBonusModels = New List(Of LoanPaymentFromBonusModel)

        _loanPaymentFromBonus = Await _loanPaymentFromBonusRepo.GetByLoanIdAsync(_loanSchedule.RowID.Value)

        Dim hasLoanPaymentFromBonus = _loanPaymentFromBonus.Any()
        If hasLoanPaymentFromBonus Then _loanPaymentFromBonusModels = _loanPaymentFromBonus.Select(Function(l) New LoanPaymentFromBonusModel(l)).ToList()

        If hasLoanPaymentFromBonus Then
            Dim bonusIds = _loanPaymentFromBonus.Select(Function(l) l.BonusId).ToArray()

            _bonuses = Await _bonusRepository.GetBetweenDatesNotFromIDs(_loanSchedule.EmployeeID.Value, _loanSchedule.DedEffectiveDateFrom, bonusIds)
        Else
            _bonuses = Await _bonusRepository.GetBetweenDatesByEmployeeId(_loanSchedule.EmployeeID.Value, _loanSchedule.DedEffectiveDateFrom)

        End If

        For Each bonus In _bonuses
            _loanPaymentFromBonusModels.Add(LoanPaymentFromBonusModel.Convert(bonus, _loanSchedule))
        Next

        dgvBonuses.AutoGenerateColumns = False
        dgvBonuses.DataSource = _loanPaymentFromBonusModels.ToList()
    End Sub

    Private Sub DisplayLoanScheduleDetails(loanSchedule As LoanSchedule)
        'loanSchedule = loanSchedule

        'Dim originalLoanSchedule = Me._unchangedLoanSchedules.
        '    FirstOrDefault(Function(l) Nullable.Equals(l.RowID, loanSchedule.RowID))

        'Dim isUneditable As Boolean = False

        'If originalLoanSchedule IsNot Nothing Then
        '    isUneditable = originalLoanSchedule.Status = LoanScheduleRepository.STATUS_CANCELLED OrElse
        '    originalLoanSchedule.Status = LoanScheduleRepository.STATUS_COMPLETE
        'End If

        'tbpDetails.Enabled = Not isUneditable

        txtLoanNumber.DataBindings.Clear()
        txtLoanNumber.DataBindings.Add("Text", loanSchedule, "LoanNumber")

        txtRemarks.DataBindings.Clear()
        txtRemarks.DataBindings.Add("Text", loanSchedule, "Comments")

        txtTotalLoanAmount.DataBindings.Clear()
        txtTotalLoanAmount.DataBindings.Add("Text", loanSchedule, "TotalLoanAmount", True, DataSourceUpdateMode.Never, Nothing, "N2")
        'If Me._currentLoanTransactions IsNot Nothing AndAlso Me._currentLoanTransactions.Count > 0 Then
        '    txtTotalLoanAmount.Enabled = False
        '    txtLoanInterestPercentage.Enabled = False
        'Else
        '    txtTotalLoanAmount.Enabled = True
        '    txtLoanInterestPercentage.Enabled = True
        'End If

        txtLoanBalance.DataBindings.Clear()
        txtLoanBalance.DataBindings.Add("Text", loanSchedule, "TotalBalanceLeft", True, DataSourceUpdateMode.Never, Nothing, "N2")
        lblTotalBalanceLeft.Text = loanSchedule.TotalBalanceLeft.ToString("N")

        dtpDateFrom.DataBindings.Clear()
        dtpDateFrom.DataBindings.Add("Text", loanSchedule, "DedEffectiveDateFrom", True, DataSourceUpdateMode.Never)

        txtNumberOfPayPeriod.DataBindings.Clear()
        txtNumberOfPayPeriod.DataBindings.Add("Text", loanSchedule, "TotalPayPeriod", True, DataSourceUpdateMode.Never, Nothing, "N0")

        txtNumberOfPayPeriodLeft.DataBindings.Clear()
        txtNumberOfPayPeriodLeft.DataBindings.Add("Text", loanSchedule, "LoanPayPeriodLeft", True, DataSourceUpdateMode.Never, Nothing, "N0")
        lblNoOfPayPeriodLeft.Text = loanSchedule.LoanPayPeriodLeft.ToString()

        txtDeductionAmount.DataBindings.Clear()
        txtDeductionAmount.DataBindings.Add("Text", loanSchedule, "DeductionAmount", True, DataSourceUpdateMode.Never, Nothing, "N2")

        txtLoanInterestPercentage.DataBindings.Clear()
        txtLoanInterestPercentage.DataBindings.Add("Text", loanSchedule, "DeductionPercentage", True, DataSourceUpdateMode.Never, Nothing, "N2")

        cboLoanType.DataBindings.Clear()
        cboLoanType.DataBindings.Add("Text", loanSchedule, "LoanName")

        txtLoanStatus.DataBindings.Clear()
        cmbLoanStatus.DataBindings.Clear()
        'If isUneditable Then

        txtLoanStatus.DataBindings.Add("Text", loanSchedule, "Status")
        'Else
        cmbLoanStatus.DataBindings.Add("Text", loanSchedule, "Status")

        'End If

        cmbDeductionSchedule.DataBindings.Clear()
        cmbDeductionSchedule.DataBindings.Add("Text", loanSchedule, "DeductionSchedule")

        'ToggleLoanStatusComboboxVisibility(Not isUneditable)

        'LoanPaymentFromBonusModel.conver

    End Sub

    Private Sub dgvBonuses_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBonuses.CellContentClick
        If IsFullAmount.Index = e.ColumnIndex Then
            tbpDetails.Focus()
            dgvBonuses.CurrentCell = dgvBonuses.Item(e.ColumnIndex, e.RowIndex)
        End If

    End Sub

    Private Sub dgvBonuses_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBonuses.CellValueChanged
        Dim currentRow = dgvBonuses.CurrentRow
        If currentRow Is Nothing Then Return

        Dim model = GetModel(dgvBonuses.CurrentRow)

        If model.IsFullPayment Then newTotalBalanceLeft = _loanSchedule.TotalBalanceLeft

        model.Scrutinize(newTotalBalanceLeft)

        dgvBonuses.Refresh()

        UpdatedLoanDisplayDetails()

        UpdateSaveButton()

        dgvBonuses.Refresh()

    End Sub

    Private Sub UpdateSaveButton()
        Dim models = GetModels()
        Dim invalidAmountPayments = models.Where(Function(m) m.InvalidAmountPayment)
        If invalidAmountPayments.Any() Then
            btnSave.Enabled = False
            'btnSave.Text = "Save"
            Return
        End If

        Dim hasChangedList = models.Where(Function(t) t.HasChanged).ToList()
        Dim hasChanges = hasChangedList.Any()
        btnSave.Enabled = hasChanges
        'btnSave.Text = $"Save ({hasChangedList.Count()})"
    End Sub

    Private Sub UpdatedLoanDisplayDetails()
        Dim models = GetModels()
        If Not models.Any() Then Return
        Dim totalAmountPayment As Decimal = models.
            Where(Function(m) Not m.PaystubId.HasValue).
            Sum(Function(m) m.AmountPayment)

        Dim invalidAmountPayments = models.Where(Function(m) m.InvalidAmountPayment)
        If invalidAmountPayments.Any() Then
            Dim errorMessage = $"Payment should be divisible by {_loanSchedule.DeductionAmount.ToString("N")}"
            With ToolTip1
                .ToolTipTitle = "Invalid Amount Payment"
                .ToolTipIcon = ToolTipIcon.Warning
                .Show(errorMessage, Label4, 0, 0, 3000)
            End With

            Dim bonusIds = invalidAmountPayments.Select(Function(m) m.BonusId).ToArray()

            For Each drow In dgvBonuses.Rows.OfType(Of DataGridViewRow)

                Dim model = GetModel(drow)

                If bonusIds.Contains(model.BonusId) Then
                    drow.Cells(AmountPayment.Name).ErrorText = errorMessage
                Else
                    Continue For
                End If
            Next
            Return
        Else
            For Each drow In dgvBonuses.Rows.OfType(Of DataGridViewRow)
                drow.Cells(AmountPayment.Name).ErrorText = String.Empty
                'dgvBonuses.UpdateCellErrorText(AmountPayment.Index, drow.Index)
            Next
        End If

        newTotalBalanceLeft = _loanSchedule.TotalBalanceLeft
        newLoanPayPeriodLeft = _loanSchedule.LoanPayPeriodLeft

        If totalAmountPayment >= _loanSchedule.TotalBalanceLeft Then
            newTotalBalanceLeft = 0
            newLoanPayPeriodLeft = 0
            totalAmountPayment = _loanSchedule.TotalBalanceLeft

        ElseIf totalAmountPayment >= _loanSchedule.DeductionAmount _
            And totalAmountPayment < _loanSchedule.TotalBalanceLeft Then
            newTotalBalanceLeft = _loanSchedule.TotalBalanceLeft - totalAmountPayment

            Dim payPeriodLeft = newTotalBalanceLeft / _loanSchedule.DeductionAmount
            If payPeriodLeft > 0 And payPeriodLeft < 1 Then
                newLoanPayPeriodLeft = 1
            ElseIf payPeriodLeft > -1 Then
                newLoanPayPeriodLeft = CInt(Math.Floor(payPeriodLeft))
            Else
                newLoanPayPeriodLeft = 0
            End If

        End If

        lblTotalAmountPayment.Text = totalAmountPayment.ToString("N")
        lblTotalBalanceLeft.Text = newTotalBalanceLeft.ToString("N")
        lblNoOfPayPeriodLeft.Text = newLoanPayPeriodLeft.ToString()

    End Sub

    Private Sub dgvBonuses_DataSourceChanged(sender As Object, e As EventArgs) Handles dgvBonuses.DataSourceChanged
        UpdatedLoanDisplayDetails()

        Dim models = GetModels()
        If Not models.Any() Then Return

        Dim uneditables = models.Where(Function(m) m.PaystubId.HasValue).ToList()

        Dim loanIdsAndBonusIds = uneditables.Select(Function(m) New Dictionary(Of Integer, Integer) From {{m.LoanId, m.BonusId}}).ToList()

        For Each row In dgvBonuses.Rows.OfType(Of DataGridViewRow)
            Dim model = GetModel(row)

            Dim foundSatisfied = loanIdsAndBonusIds.Where(Function(m) m.ContainsKey(model.LoanId)).
                Where(Function(m) m.ContainsValue(model.BonusId)).Any()

            row.ReadOnly = foundSatisfied
        Next
    End Sub

    Private Sub dgvBonuses_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvBonuses.DataError
        'e.ThrowException = False
    End Sub

    Private Function GetModel(currentRow As DataGridViewRow) As LoanPaymentFromBonusModel
        Return DirectCast(currentRow.DataBoundItem, LoanPaymentFromBonusModel)
    End Function

    Private Function GetModels() As IEnumerable(Of LoanPaymentFromBonusModel)
        Dim rows = dgvBonuses.Rows.OfType(Of DataGridViewRow).Select(Function(r) GetModel(r)).ToList()
        Return rows
    End Function

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim saveList = GetModels().
            Where(Function(t) t.HasChanged).
            Select(Function(t) t.Export()).
            ToList()

        If Not saveList.Any() Then Return

        btnSave.Enabled = False

        Dim models = GetModels()
        Dim totalAmountPayment As Decimal = models.Sum(Function(m) m.AmountPayment)
        If totalAmountPayment <= 0 Then
            _loanSchedule.DeductionAmount = saveList.FirstOrDefault().DeductionAmount
        Else
            _loanSchedule.DeductionAmount = totalAmountPayment
        End If

        Await _loanService.SaveAsync(_loanSchedule, z_User)

        Await _loanPaymentFromBonusRepo.SaveManyAsync(saveList)

        DialogResult = DialogResult.OK
        btnSave.Enabled = True
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
    End Sub

End Class
