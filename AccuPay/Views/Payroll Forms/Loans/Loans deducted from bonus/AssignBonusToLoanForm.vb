Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Data.ValueObjects
Imports Microsoft.Extensions.DependencyInjection

Public Class AssignBonusToLoanForm

    Private ReadOnly _loanSchedule As LoanSchedule
    Private ReadOnly _loanTransactions As ICollection(Of LoanTransaction)
    Private ReadOnly _bonusDataService As BonusDataService
    Private ReadOnly _payPeriodRepository As PayPeriodRepository
    Private _bonuses As IEnumerable(Of Bonus)
    Private _lastSelectedCellIndex1 As Integer
    Private _lastSelectedRowIndex1 As Integer
    Private _lastSelectedCellIndex2 As Integer
    Private _lastSelectedRowIndex2 As Integer
    Public Property ChangedBonus As Bonus

    Public Sub New(loanSchedule As LoanSchedule, loanTransactions As ICollection(Of LoanTransaction))

        InitializeComponent()

        _loanSchedule = loanSchedule

        _loanTransactions = loanTransactions

        _bonusDataService = MainServiceProvider.GetRequiredService(Of BonusDataService)

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of PayPeriodRepository)

        DisplayLoanScheduleDetails(loanSchedule)
    End Sub

    Private Async Sub AssignBonusToLoan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgvBonuses.AutoGenerateColumns = False
        DataGridView1.AutoGenerateColumns = False
        DataGridViewX1.AutoGenerateColumns = False

        btnSave.Visible = _loanSchedule.Status <> LoanSchedule.STATUS_COMPLETE

        Await LoadLoanPaymentFromBonus()
    End Sub

    Private Async Function LoadLoanPaymentFromBonus() As Task
        Dim loanPaymentFromBonusModels = New List(Of LoanPaymentFromBonusData)

        Dim loanCoveredPeriods = Await GetLoanCoveredPeriods(_loanSchedule)

        Dim loanTimerPeriod = New TimePeriod(
            _loanSchedule.DedEffectiveDateFrom,
            loanCoveredPeriods.Max(Function(pp) pp.PayToDate))

        _bonuses = Await _bonusDataService.GetByEmployeeAndPayPeriodForLoanPaymentAsync(
            organizationId:=z_OrganizationID,
            employeeId:=_loanSchedule.EmployeeID.Value,
            timePeriod:=loanTimerPeriod)

        Dim hasChangedBonus = ChangedBonus IsNot Nothing

        For Each bonus In _bonuses
            Dim paramBonus = bonus
            If hasChangedBonus AndAlso bonus.RowID = ChangedBonus.RowID Then
                paramBonus = ChangedBonus
            End If
            loanPaymentFromBonusModels.Add(New LoanPaymentFromBonusData(bonus:=paramBonus, loanSchedule:=_loanSchedule))
        Next

        dgvBonuses.DataSource = loanPaymentFromBonusModels.
            OrderBy(Function(l) l.EffectiveStartDate).
            ThenByDescending(Function(l) l.BonusAmount).
            ToList()
    End Function

    Private Async Function GetLoanCoveredPeriods(loanSchedule As LoanSchedule) As Task(Of ICollection(Of PayPeriod))
        Return Await _payPeriodRepository.
            GetLoanScheduleRemainingPayPeriodsAsync(loanSchedule)
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
        If dgvBonuses.Rows.Count() > 0 Then
            TabPage2.Text = $"Preview {GetModel(dgvBonuses.CurrentRow).BonusType}"
        Else
            TabPage2.Text = "Preview"
        End If
    End Sub

    Private Sub dgvBonuses_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBonuses.CellEndEdit
        dgvBonuses.EndEdit()

        Dim currentRow = dgvBonuses.Rows(e.RowIndex)
        If currentRow Is Nothing Then Return

        If {colIsFullAmount.Index, colAmountPayment.Index}.Contains(e.ColumnIndex) Then
            Dim model = GetModel(currentRow)

            If model.IsExcessivePayment Then model.IsFullPayment = True

            If colIsFullAmount.Index = e.ColumnIndex Then
                If model.IsFullPayment Then
                    model.SetValidAmountPayment()
                Else
                    model.SetNoAmountPayment()
                End If
            End If

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

    Private Function GetModel(currentRow As DataGridViewRow) As LoanPaymentFromBonusData
        Return DirectCast(currentRow.DataBoundItem, LoanPaymentFromBonusData)
    End Function

    Public Function GetModels() As IEnumerable(Of LoanPaymentFromBonusData)
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

    Private Async Sub TabPage2_Enter(sender As Object, e As EventArgs) Handles TabPage2.Enter
        If dgvBonuses.Rows.Count() = 0 Then Return

        Dim model = GetModel(dgvBonuses.CurrentRow)

        Dim models = New List(Of LoanPaymentFromBonusData) From {model}

        Dim leastEffectiveDate = {models.Min(Function(m) m.EffectiveEndDate)}.Min()

        Dim coveredPeriod = Await GetLoanCoveredPeriods(_loanSchedule)

        Dim loanBalance = _loanSchedule.TotalLoanAmount

        Dim dataSource As List(Of PreviewLoanPayment) = Await LoadPreviewLoanPayments(models, coveredPeriod, loanBalance)

        DataGridViewX1.DataSource = dataSource

        If _lastSelectedRowIndex1 > -1 AndAlso
            _lastSelectedCellIndex1 > -1 AndAlso
            dataSource.Any() AndAlso
            _lastSelectedRowIndex1 <= dataSource.Count() Then

            DataGridViewX1.CurrentCell = DataGridViewX1.Item(_lastSelectedCellIndex1, _lastSelectedRowIndex1)
            AddHandler DataGridViewX1.SelectionChanged, AddressOf DataGridViewX1_SelectionChanged
        End If
    End Sub

    Private Async Sub TabPage3_Enter(sender As Object, e As EventArgs) Handles TabPage3.Enter
        If dgvBonuses.Rows.Count() = 0 Then Return

        Dim models = GetModels()

        ', _loanSchedule.DedEffectiveDateFrom
        Dim leastEffectiveDate = {models.Min(Function(m) m.EffectiveEndDate)}.Min()

        Dim coveredPeriod = Await GetLoanCoveredPeriods(_loanSchedule)

        Dim loanBalance = _loanSchedule.TotalLoanAmount

        Dim dataSource As List(Of PreviewLoanPayment) = Await LoadPreviewLoanPayments(models, coveredPeriod, loanBalance)

        'If(d.IsPeriod, earliestEffectiveBonusDate <= d.PeriodTimePeriod.Start, d.IsWithinPeriod1)
        'Dim gridviewDataSource = dataSource.
        '    Where(Function(d) leastEffectiveDate <= d.PeriodTimePeriod.Start).
        '    ToList()
        'If gridviewDataSource.Any() Then
        '    DataGridView1.DataSource = gridviewDataSource
        'Else
        DataGridView1.DataSource = dataSource
        'End If

        If _lastSelectedRowIndex2 > -1 AndAlso
            _lastSelectedCellIndex2 > -1 AndAlso
            dataSource.Any() AndAlso
            _lastSelectedRowIndex1 < dataSource.Count() Then

            DataGridView1.CurrentCell = DataGridView1.Item(_lastSelectedCellIndex2, _lastSelectedRowIndex2)
            AddHandler DataGridView1.SelectionChanged, AddressOf DataGridView1_SelectionChanged
        End If
    End Sub

    Private Async Function LoadPreviewLoanPayments(
            models As IEnumerable(Of LoanPaymentFromBonusData),
            coveredPeriod As ICollection(Of PayPeriod),
            loanBalance As Decimal) As Task(Of List(Of PreviewLoanPayment))
        Dim dataSource = New List(Of PreviewLoanPayment)

        Dim totalPayment = 0D

        Dim currentLoanBalance = 0D

        For Each period In coveredPeriod
            Dim periodicTotalPayment = 0D
            Dim periodicTotalBonus = 0D

            For Each model In models
                'Dim isWithinPeriod = model.EffectiveStartDate <= period.PayToDate AndAlso
                '    period.PayFromDate <= model.EffectiveEndDate
                Dim isWithinPeriod = PreviewLoanPayment.IsWithinPeriod(
                    bonusTimePeriod:=New TimePeriod(model.EffectiveStartDate, model.EffectiveEndDate),
                    cutoffTimePeriod:=New TimePeriod(period.PayFromDate, period.PayToDate))

                If Not isWithinPeriod Then Continue For

                totalPayment += model.AmountPayment

                periodicTotalPayment += model.AmountPayment

                periodicTotalBonus += model.BonusAmount

                dataSource.Add(New PreviewLoanPayment(model, period:=period))
            Next

            If periodicTotalPayment = 0 Then
                Dim loanTransaction = _loanTransactions.
                    FirstOrDefault(Function(lt) CBool(lt.PayPeriodID = period.RowID.Value))

                If loanTransaction IsNot Nothing Then
                    totalPayment += loanTransaction.DeductionAmount
                    periodicTotalPayment = loanTransaction.DeductionAmount
                Else
                    totalPayment += _loanSchedule.DeductionAmount
                    periodicTotalPayment = _loanSchedule.DeductionAmount
                End If
            End If

            Dim updatedLoanBalance = loanBalance - totalPayment

            If updatedLoanBalance < 0 Then
                totalPayment += updatedLoanBalance
                periodicTotalPayment += updatedLoanBalance
                updatedLoanBalance -= updatedLoanBalance
            End If

            Dim bonusBalance = If(periodicTotalBonus - periodicTotalPayment < 0,
                0,
                periodicTotalBonus - periodicTotalPayment)

            Dim periodDataRow = New PreviewLoanPayment(
                period,
                bonusBalance:=periodicTotalBonus,
                payment:=periodicTotalPayment,
                loanBalance:=updatedLoanBalance)
            dataSource.Add(periodDataRow)

            currentLoanBalance = updatedLoanBalance

            If updatedLoanBalance = 0 Then Exit For
        Next

        If currentLoanBalance > 0 Then
            Dim extendedCoveredPeriods = Await _payPeriodRepository.GetLoanScheduleRemainingPayPeriodsAsync(
                organizationId:=z_OrganizationID,
                startDate:=dataSource.Max(Function(d) d.PeriodTimePeriod.End).AddDays(1),
                frequencySchedule:=_loanSchedule.DeductionSchedule,
                count:=CInt(Math.Round(currentLoanBalance / _loanSchedule.DeductionAmount, 0)))

            Dim extendedDataSource = Await LoadPreviewLoanPayments(
                models,
                extendedCoveredPeriods,
                currentLoanBalance)
            For Each extDataSource In extendedDataSource
                dataSource.Add(extDataSource)
            Next
        End If

        Return dataSource
    End Function

    Private Sub DataGridView1_DataSourceChanged(sender As Object, e As EventArgs) Handles DataGridView1.DataSourceChanged
        ApplyGridRowCellStyle(dataGrid:=DataGridView1)
    End Sub

    Private Sub DataGridViewX1_DataSourceChanged(sender As Object, e As EventArgs) Handles DataGridViewX1.DataSourceChanged
        ApplyGridRowCellStyle(dataGrid:=DataGridViewX1)
    End Sub

    Private Sub ApplyGridRowCellStyle(dataGrid As DevComponents.DotNetBar.Controls.DataGridViewX)
        For Each row In dataGrid.Rows.OfType(Of DataGridViewRow)
            Dim data = DirectCast(row.DataBoundItem, PreviewLoanPayment)
            If data.IsPeriod Then
                Dim cellStyle = row.DefaultCellStyle
                cellStyle.Font = New Font("Segoe UI", 8.25!, FontStyle.Bold, GraphicsUnit.Point, CType(0, Byte))

                row.DefaultCellStyle.ApplyStyle(cellStyle)
            End If
        Next
    End Sub

    Private Sub DataGridViewX1_SelectionChanged(sender As Object, e As EventArgs)
        _lastSelectedCellIndex1 = DataGridViewX1.CurrentCell.ColumnIndex
        _lastSelectedRowIndex1 = DataGridViewX1.CurrentRow.Index
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs)
        _lastSelectedCellIndex2 = DataGridView1.CurrentCell.ColumnIndex
        _lastSelectedRowIndex2 = DataGridView1.CurrentRow.Index
    End Sub

    Private Sub TabPage1_Enter(sender As Object, e As EventArgs) Handles TabPage1.Enter
        RemoveHandler DataGridView1.SelectionChanged, AddressOf DataGridView1_SelectionChanged
        RemoveHandler DataGridViewX1.SelectionChanged, AddressOf DataGridViewX1_SelectionChanged
    End Sub

    Private Class PreviewLoanPayment

        Public Sub New(model As LoanPaymentFromBonusData, period As PayPeriod)
            BonusType = model.BonusType
            BonusAmount = model.BonusAmount
            Payment = model.AmountPayment
            PeriodTimePeriod = New TimePeriod(period.PayFromDate, period.PayToDate)
        End Sub

        Public Sub New(payPeriod As PayPeriod, bonusBalance As Decimal, payment As Decimal, loanBalance As Decimal)
            Me.Payment = payment
            Me.LoanBalance = loanBalance
            BonusAmount = bonusBalance
            PeriodTimePeriod = New TimePeriod(payPeriod.PayFromDate, payPeriod.PayToDate)
            IsPeriod = True
        End Sub

        Public ReadOnly Property IsPeriod As Boolean
        Public ReadOnly Property BonusType As String
        Public ReadOnly Property BonusAmount As Decimal?
        Public ReadOnly Property PeriodTimePeriod As TimePeriod

        Public ReadOnly Property PayPeriodFrom As Date?
            Get
                If IsPeriod Then Return PeriodTimePeriod.Start
            End Get
        End Property

        Public ReadOnly Property PayPeriodTo As Date?
            Get
                If IsPeriod Then Return PeriodTimePeriod.End
            End Get
        End Property

        Public ReadOnly Property Payment As Decimal?
        Public ReadOnly Property LoanBalance As Decimal?

        Friend Shared Function IsWithinPeriod(bonusTimePeriod As TimePeriod, cutoffTimePeriod As TimePeriod) As Boolean
            Return bonusTimePeriod.Start <= cutoffTimePeriod.End AndAlso
                cutoffTimePeriod.Start <= bonusTimePeriod.End
        End Function

    End Class

End Class
