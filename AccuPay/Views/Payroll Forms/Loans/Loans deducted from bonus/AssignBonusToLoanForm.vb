Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Data.ValueObjects
Imports Microsoft.Extensions.DependencyInjection

Public Class AssignBonusToLoanForm

    Private ReadOnly _loanSchedule As LoanSchedule
    Private ReadOnly _bonusRepository As BonusRepository

    Private _bonuses As IEnumerable(Of Bonus)
    Private _loanPaymentFromBonusModels As List(Of LoanPaymentFromBonusModel)
    Public Property ChangedBonus As Bonus

    Public Sub New(loanSchedule As LoanSchedule)

        InitializeComponent()

        _loanSchedule = loanSchedule

        _bonusRepository = MainServiceProvider.GetRequiredService(Of BonusRepository)

        DisplayLoanScheduleDetails(loanSchedule)
    End Sub

    Private Async Sub AssignBonusToLoan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await LoadLoanPaymentFromBonus()
    End Sub

    Private Async Function LoadLoanPaymentFromBonus() As Task
        _loanPaymentFromBonusModels = New List(Of LoanPaymentFromBonusModel)

        _bonuses = Await _bonusRepository.GetByEmployeeAndPayPeriodForLoanPaymentAsync(
            organizationId:=z_OrganizationID,
            employeeId:=_loanSchedule.EmployeeID.Value,
            timePeriod:=New TimePeriod(_loanSchedule.DedEffectiveDateFrom, _loanSchedule.DedEffectiveDateFrom.AddYears(1)))

        Dim hasChangedBonus = ChangedBonus IsNot Nothing

        For Each bonus In _bonuses
            Dim paramBonus = bonus
            If hasChangedBonus AndAlso bonus.RowID = ChangedBonus.RowID Then
                paramBonus = ChangedBonus
            End If
            _loanPaymentFromBonusModels.Add(New LoanPaymentFromBonusModel(bonus:=paramBonus, loanSchedule:=_loanSchedule))
        Next

        dgvBonuses.AutoGenerateColumns = False
        dgvBonuses.DataSource = _loanPaymentFromBonusModels.
            OrderBy(Function(l) l.EffectiveStartDate).
            ThenByDescending(Function(l) l.BonusAmount).
            ToList()
    End Function

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
        Dim models = GetModels().
            Where(Function(m) m.IsEditable).
            Where(Function(m) m.HasChanged)

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

            If model.IsExcessivePayment Then model.IsFullPayment = True

            currentRow.Cells(colAmountPayment.Index).ReadOnly = model.IsFullPayment

            UpdatedLoanDisplayDetails()

            UpdateSaveButton()

            dgvBonuses.Refresh()
        End If
    End Sub

    Private Sub UpdateSaveButton()
        Dim changedCount = GetChangesCount()
        Dim isValid = changedCount > 0

        btnSave.Enabled = isValid
        btnSave.Text = If(isValid, $"&Save ({changedCount})", "&Save")
    End Sub

    Private Function GetChangesCount() As Integer
        If _loanSchedule.Status = LoanSchedule.STATUS_COMPLETE Then Return 0

        Dim totalPayment = GetTotalPayment()
        Dim overPays = totalPayment > _loanSchedule.TotalBalanceLeft
        If overPays Then
            MessageBox.Show(
                $"Total Payment Amount exceeds Loan Balance({_loanSchedule.TotalBalanceLeft.ToString("N")}).{Environment.NewLine}Please correct the Payment.",
                "Payment exceed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Stop)
            Return 0
        Else
            Dim hasChangedList = GetModels().Where(Function(t) t.HasChanged)
            Dim hasChanges = hasChangedList.Any()

            Dim payAtLeastDeductionAmount = totalPayment = 0 OrElse totalPayment >= _loanSchedule.DeductionAmount

            If hasChanges AndAlso payAtLeastDeductionAmount Then
                Return hasChangedList.Count()
            Else
                Return 0
            End If
        End If
    End Function

    Private Function GetTotalPayment() As Decimal
        Dim models = GetModels()
        Return models.Sum(Function(m) m.AmountPayment)
    End Function

    Private Sub UpdatedLoanDisplayDetails()
        Dim models = GetModels()
        If Not models.Any() Then Return
        Dim totalAmountPayment As Decimal = GetTotalPayment()

        Dim newTotalBalanceLeft = _loanSchedule.TotalBalanceLeft
        Dim newLoanPayPeriodLeft = _loanSchedule.LoanPayPeriodLeft

        If totalAmountPayment >= _loanSchedule.TotalBalanceLeft Then
            newTotalBalanceLeft = 0
            newLoanPayPeriodLeft = 0

        ElseIf totalAmountPayment >= _loanSchedule.DeductionAmount _
            And totalAmountPayment < _loanSchedule.TotalBalanceLeft Then
            newTotalBalanceLeft = _loanSchedule.TotalBalanceLeft - totalAmountPayment

            Dim payPeriodLeft = newTotalBalanceLeft / _loanSchedule.DeductionAmount
            If payPeriodLeft > 0 And payPeriodLeft < 1 Then
                newLoanPayPeriodLeft = 1
            ElseIf payPeriodLeft > -1 Then
                newLoanPayPeriodLeft = CInt(Math.Round(payPeriodLeft, 2))
            Else
                newLoanPayPeriodLeft = 0
            End If
        End If

        lblTotalAmountPayment.Text = totalAmountPayment.ToString("N")
        lblTotalBalanceLeft.Text = newTotalBalanceLeft.ToString("N")
        lblLoanPayPeriodLeft.Text = newLoanPayPeriodLeft.ToString()
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

    Public Function GetModels() As IEnumerable(Of LoanPaymentFromBonusModel)
        Return dgvBonuses.Rows.OfType(Of DataGridViewRow).Select(Function(r) GetModel(r)).ToList()
    End Function

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        dgvBonuses.EndEdit()

        If GetChangesCount() = 0 Then
            Return
        End If

        Dim saveList = GetModels().
            Where(Function(t) t.HasChanged).
            Select(Function(t) t.Export(organizationId:=z_OrganizationID, userId:=z_User)).
            ToList()

        btnSave.Enabled = False

        Dim loanPaymentFromBonusRepo = MainServiceProvider.GetRequiredService(Of LoanPaymentFromBonusRepository)

        Await loanPaymentFromBonusRepo.SaveManyAsync(saveList)

        DialogResult = DialogResult.OK
        btnSave.Enabled = True
    End Sub

End Class
