Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.ValueObjects
Imports Microsoft.Extensions.DependencyInjection

Public Class AssignBonusToLoanForm

    Private ReadOnly _loanSchedule As LoanSchedule
    Private ReadOnly _bonusRepository As BonusRepository
    Private _loanPaymentFromBonusRepo As LoanPaymentFromBonusRepository

    Private _bonuses As IEnumerable(Of Bonus)
    Private _loanPaymentFromBonusModels As List(Of LoanPaymentFromBonusModel)
    Private _newTotalBalanceLeft As Decimal
    Private _newLoanPayPeriodLeft As Integer

    Public Sub New(loanSchedule As LoanSchedule)

        InitializeComponent()

        _loanSchedule = loanSchedule

        _bonusRepository = MainServiceProvider.GetRequiredService(Of BonusRepository)

        _newTotalBalanceLeft = _loanSchedule.TotalBalanceLeft
        _newLoanPayPeriodLeft = _loanSchedule.LoanPayPeriodLeft

        DisplayLoanScheduleDetails(loanSchedule)

        LoadLoanPaymentFromBonus()
    End Sub

    Private Sub AssignBonusToLoan_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Async Sub LoadLoanPaymentFromBonus()
        _loanPaymentFromBonusModels = New List(Of LoanPaymentFromBonusModel)

        _bonuses = Await _bonusRepository.GetByEmployeeAndPayPeriodForLoanPaymentAsync(
            organizationId:=z_OrganizationID,
            employeeId:=_loanSchedule.EmployeeID.Value,
            timePeriod:=New TimePeriod(_loanSchedule.DedEffectiveDateFrom, _loanSchedule.DedEffectiveDateFrom.AddYears(1)))

        For Each bonus In _bonuses
            _loanPaymentFromBonusModels.Add(New LoanPaymentFromBonusModel(bonus:=bonus, loanSchedule:=_loanSchedule))
        Next

        dgvBonuses.AutoGenerateColumns = False
        dgvBonuses.DataSource = _loanPaymentFromBonusModels.
            OrderBy(Function(l) l.EffectiveDate).
            ThenByDescending(Function(l) l.BonusAmount).
            ToList()
    End Sub

    Private Sub DisplayLoanScheduleDetails(loanSchedule As LoanSchedule)
        txtLoanNumber.DataBindings.Clear()
        txtLoanNumber.DataBindings.Add("Text", loanSchedule, "LoanNumber")

        txtRemarks.DataBindings.Clear()
        txtRemarks.DataBindings.Add("Text", loanSchedule, "Comments")

        txtTotalLoanAmount.DataBindings.Clear()
        txtTotalLoanAmount.DataBindings.Add("Text", loanSchedule, "TotalLoanAmount", True, DataSourceUpdateMode.Never, Nothing, "N2")

        txtLoanBalance.DataBindings.Clear()
        txtLoanBalance.DataBindings.Add("Text", loanSchedule, "TotalBalanceLeft", True, DataSourceUpdateMode.Never, Nothing, "N2")
        lblTotalBalanceLeft.Text = loanSchedule.TotalBalanceLeft.ToString("N")

        dtpDateFrom.DataBindings.Clear()
        dtpDateFrom.DataBindings.Add("Text", loanSchedule, "DedEffectiveDateFrom", True, DataSourceUpdateMode.Never)

        txtNumberOfPayPeriod.DataBindings.Clear()
        txtNumberOfPayPeriod.DataBindings.Add("Text", loanSchedule, "TotalPayPeriod", True, DataSourceUpdateMode.Never, Nothing, "N0")

        txtNumberOfPayPeriodLeft.DataBindings.Clear()
        txtNumberOfPayPeriodLeft.DataBindings.Add("Text", loanSchedule, "LoanPayPeriodLeft", True, DataSourceUpdateMode.Never, Nothing, "N0")
        lblLoanPayPeriodLeft.Text = loanSchedule.LoanPayPeriodLeft.ToString()

        txtDeductionAmount.DataBindings.Clear()
        txtDeductionAmount.DataBindings.Add("Text", loanSchedule, "DeductionAmount", True, DataSourceUpdateMode.Never, Nothing, "N2")

        txtLoanInterestPercentage.DataBindings.Clear()
        txtLoanInterestPercentage.DataBindings.Add("Text", loanSchedule, "DeductionPercentage", True, DataSourceUpdateMode.Never, Nothing, "N2")

        cboLoanType.DataBindings.Clear()
        cboLoanType.DataBindings.Add("Text", loanSchedule, "LoanName")

        txtLoanStatus.DataBindings.Clear()
        cmbLoanStatus.DataBindings.Clear()

        txtLoanStatus.DataBindings.Add("Text", loanSchedule, "Status")

        cmbLoanStatus.DataBindings.Add("Text", loanSchedule, "Status")

        cmbDeductionSchedule.DataBindings.Clear()
        cmbDeductionSchedule.DataBindings.Add("Text", loanSchedule, "DeductionSchedule")
    End Sub

    Private Sub dgvBonuses_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvBonuses.CellBeginEdit
        Dim models = GetModels().Where(Function(m) m.IsEditable).Where(Function(m) m.HasChanged)
        If models.Any(Function(m) m.IsFulfilled) Then
            Dim idList = models.Select(Function(m) m.BonusId).ToArray()

            Dim currentRow = dgvBonuses.Rows(e.RowIndex)
            If currentRow Is Nothing Then Return

            Dim model = GetModel(currentRow)
            e.Cancel = Not idList.Contains(model.BonusId)
        End If
    End Sub

    Private Sub dgvBonuses_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBonuses.CellValueChanged

    End Sub

    Private Sub dgvBonuses_SelectionChanged(sender As Object, e As EventArgs) Handles dgvBonuses.SelectionChanged

    End Sub

    Private Sub dgvBonuses_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBonuses.CellEndEdit
        dgvBonuses.EndEdit()

        Dim currentRow = dgvBonuses.Rows(e.RowIndex)
        If currentRow Is Nothing Then Return

        If {colIsFullAmount.Index, colAmountPayment.Index}.Contains(e.ColumnIndex) Then
            Dim model = GetModel(currentRow)

            currentRow.Cells(colAmountPayment.Index).ReadOnly = model.IsFullPayment

            UpdatedLoanDisplayDetails()

            UpdateSaveButton()

            dgvBonuses.Refresh()
        End If
    End Sub

    Private Sub UpdateSaveButton()
        Dim models = GetModels()
        Dim totalPayment = models.Sum(Function(m) m.AmountPayment)
        Dim overPays = totalPayment > _loanSchedule.TotalBalanceLeft
        If overPays Then
            btnSave.Enabled = False
            btnSave.Text = "&Save"
            MessageBox.Show(
                $"Total Payment Amount exceeds Loan Balance({_loanSchedule.TotalBalanceLeft.ToString("N")}). Please correct the Payment.",
                "Payment exceed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Stop)
        Else
            Dim hasChangedList = models.Where(Function(t) t.HasChanged)
            Dim hasChanges = hasChangedList.Any()

            Dim remainder = totalPayment Mod _loanSchedule.DeductionAmount
            Dim payExact = remainder = 0

            Dim saveAbility = hasChanges AndAlso payExact

            btnSave.Enabled = saveAbility
            btnSave.Text = If(saveAbility, $"&Save ({hasChangedList.Count()})", "&Save")
        End If
    End Sub

    Private Sub UpdatedLoanDisplayDetails()
        Dim models = GetModels()
        If Not models.Any() Then Return
        Dim totalAmountPayment As Decimal = models.Sum(Function(m) m.AmountPayment)

        _newTotalBalanceLeft = _loanSchedule.TotalBalanceLeft
        _newLoanPayPeriodLeft = _loanSchedule.LoanPayPeriodLeft

        If totalAmountPayment >= _loanSchedule.TotalBalanceLeft Then
            _newTotalBalanceLeft = 0
            _newLoanPayPeriodLeft = 0
            'totalAmountPayment = _loanSchedule.TotalBalanceLeft

        ElseIf totalAmountPayment >= _loanSchedule.DeductionAmount _
            And totalAmountPayment < _loanSchedule.TotalBalanceLeft Then
            _newTotalBalanceLeft = _loanSchedule.TotalBalanceLeft - totalAmountPayment

            Dim payPeriodLeft = _newTotalBalanceLeft / _loanSchedule.DeductionAmount
            If payPeriodLeft > 0 And payPeriodLeft < 1 Then
                _newLoanPayPeriodLeft = 1
            ElseIf payPeriodLeft > -1 Then
                _newLoanPayPeriodLeft = CInt(Math.Round(payPeriodLeft, 2))
            Else
                _newLoanPayPeriodLeft = 0
            End If
        End If

        lblTotalAmountPayment.Text = totalAmountPayment.ToString("N")
        lblTotalBalanceLeft.Text = _newTotalBalanceLeft.ToString("N")
        lblLoanPayPeriodLeft.Text = _newLoanPayPeriodLeft.ToString()
    End Sub

    Private Sub dgvBonuses_DataSourceChanged(sender As Object, e As EventArgs) Handles dgvBonuses.DataSourceChanged
        UpdatedLoanDisplayDetails()

        For Each row In dgvBonuses.Rows.OfType(Of DataGridViewRow)
            Dim model = GetModel(row)
            row.ReadOnly = Not model.IsEditable
        Next
    End Sub

    Private Sub dgvBonuses_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvBonuses.DataError
        'e.ThrowException = False
    End Sub

    Private Function GetModel(currentRow As DataGridViewRow) As LoanPaymentFromBonusModel
        Return DirectCast(currentRow.DataBoundItem, LoanPaymentFromBonusModel)
    End Function

    Private Function GetModels() As IEnumerable(Of LoanPaymentFromBonusModel)
        Return dgvBonuses.Rows.OfType(Of DataGridViewRow).Select(Function(r) GetModel(r)).ToList()
    End Function

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        dgvBonuses.EndEdit()

        Dim saveList = GetModels().
            Where(Function(t) t.HasChanged).
            Select(Function(t) t.Export()).
            ToList()

        If Not saveList.Any() Then Return

        btnSave.Enabled = False

        _loanPaymentFromBonusRepo = MainServiceProvider.GetRequiredService(Of LoanPaymentFromBonusRepository)

        Await _loanPaymentFromBonusRepo.SaveManyAsync(saveList)

        DialogResult = DialogResult.OK
        btnSave.Enabled = True
    End Sub

End Class
