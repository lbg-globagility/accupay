Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Utilities
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Utils

Public Class EmployeeLoansForm

    Dim sys_ownr As New SystemOwnerService()

    Private sysowner_is_benchmark As Boolean

    Private _employeeRepository As New EmployeeRepository()

    Private _productRepository As New ProductRepository()

    Private _listOfValueRepository As New ListOfValueRepository()

    Private _loanScheduleRepository As New LoanScheduleRepository()

    Private _loanTypeList As List(Of Product)

    Private _allEmployees As New List(Of Employee)

    Private _employees As New List(Of Employee)

    Private _currentLoanSchedule As LoanSchedule

    Private _currentloanSchedules As New List(Of LoanSchedule)

    Private _changedLoanSchedules As New List(Of LoanSchedule)

    Private _currentLoanTransactions As New List(Of LoanTransaction)

    Private _textBoxDelayedAction As New DelayedAction(Of Boolean)

    Private Const LOAN_HISTORY_TAB_TEXT As String = "Loan History"

    Private Const FormEntityName As String = "Loan"

    Private Async Sub EmployeeLoansForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        sysowner_is_benchmark = sys_ownr.GetCurrentSystemOwner() = SystemOwnerService.Benchmark

        If sysowner_is_benchmark Then

            RemoveHandler searchTextBox.TextChanged, AddressOf searchTextBox_TextChanged
            AddHandler searchTextBox.KeyPress, AddressOf SearchTextBox_KeyPress
        Else
            AddHandler searchTextBox.TextChanged, AddressOf searchTextBox_TextChanged
            RemoveHandler searchTextBox.KeyPress, AddressOf SearchTextBox_KeyPress
        End If

        AddHandler loanSchedulesDataGridView.SelectionChanged, AddressOf loanSchedulesDataGridView_SelectionChanged

        DetailsTabControl.Enabled = False

        InitializeComponentSettings()

        LoadLoanStatus()
        Await LoadLoanTypes()
        Await LoadDeductionSchedules()

        Await LoadEmployees()
        Await ShowEmployeeList()

        ResetLoanScheduleForm()

    End Sub

    Private Sub searchTextBox_TextChanged(sender As Object, e As EventArgs)

        _textBoxDelayedAction.ProcessAsync(Async Function()
                                               Await FilterEmployees(searchTextBox.Text.ToLower())

                                               Return True
                                           End Function)

    End Sub

    Private Sub SearchTextBox_KeyPress(sender As Object, e As KeyPressEventArgs)
        Dim e_asc = Asc(e.KeyChar)

        If e_asc = 13 Then

            _textBoxDelayedAction.ProcessAsync(Async Function()
                                                   Await FilterEmployees(searchTextBox.Text.ToLower())

                                                   Return True
                                               End Function)
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Async Sub employeesDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles employeesDataGridView.SelectionChanged

        ResetLoanScheduleForm()
        Me._currentloanSchedules.Clear()
        Me.LoanScheduleBindingSource.Clear()
        loanSchedulesDataGridView.DataSource = LoanScheduleBindingSource

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then Return

        txtEmployeeFirstName.Text = currentEmployee.FullNameWithMiddleInitial
        txtEmployeeNumber.Text = currentEmployee.EmployeeIdWithPositionAndEmployeeType

        pbEmployeePicture.Image = ConvByteToImage(currentEmployee.Image)

        Await LoadLoanSchedules(currentEmployee)

    End Sub

    Private Async Sub loanSchedulesDataGridView_SelectionChanged(sender As Object, e As EventArgs)

        RemoveHandler loanSchedulesDataGridView.SelectionChanged, AddressOf loanSchedulesDataGridView_SelectionChanged

        ResetLoanScheduleForm()

        DetailsTabControl.Enabled = False

        If loanSchedulesDataGridView.CurrentRow Is Nothing Then

            AddHandler loanSchedulesDataGridView.SelectionChanged, AddressOf loanSchedulesDataGridView_SelectionChanged
            Return

        End If

        Dim currentLoanSchedule As LoanSchedule = GetSelectedLoanSchedule()

        Dim currentEmployee = GetSelectedEmployee()
        If currentLoanSchedule IsNot Nothing AndAlso currentEmployee IsNot Nothing AndAlso
           Nullable.Equals(currentLoanSchedule.EmployeeID, currentEmployee.RowID) Then

            Await UpdateSelectedLoanScheduleData()

            tbpHistory.Text = $"{LOAN_HISTORY_TAB_TEXT} ({Me._currentLoanTransactions.Count})"

            DetailsTabControl.Enabled = True
        End If

        AddHandler loanSchedulesDataGridView.SelectionChanged, AddressOf loanSchedulesDataGridView_SelectionChanged

    End Sub

    Private loanAmountBeforeTextChange As Decimal

    Private loanInterestPercentageBeforeTextChange As Decimal

    Private Sub txtLoanInterestPercentage_Enter(sender As Object, e As EventArgs) Handles txtLoanInterestPercentage.Enter

        If Me._currentLoanSchedule Is Nothing Then Return

        loanInterestPercentageBeforeTextChange = Me._currentLoanSchedule.DeductionPercentage

    End Sub

    Private Sub txtLoanInterestPercentage_Leave(sender As Object, e As EventArgs) Handles txtLoanInterestPercentage.Leave

        If Me._currentLoanSchedule Is Nothing Then Return

        If loanInterestPercentageBeforeTextChange = Me._currentLoanSchedule.DeductionPercentage Then Return

        Dim totalPlusInterestRate As Decimal = 1 + (Me._currentLoanSchedule.DeductionPercentage * 0.01D)

        Me._currentLoanSchedule.TotalLoanAmount = Me._currentLoanSchedule.TotalLoanAmount * totalPlusInterestRate

        UpdateBalanceAndNumberOfPayPeriod()

    End Sub

    Private Sub txtTotalLoanAmount_Enter(sender As Object, e As EventArgs) Handles txtTotalLoanAmount.Enter

        If Me._currentLoanSchedule Is Nothing Then Return

        loanAmountBeforeTextChange = Me._currentLoanSchedule.TotalLoanAmount

    End Sub

    Private Sub txtTotalLoanAmount_Leave(sender As Object, e As EventArgs) _
        Handles txtTotalLoanAmount.Leave, txtDeductionAmount.Leave

        If Me._currentLoanSchedule Is Nothing Then Return

        If sender Is txtTotalLoanAmount Then
            If loanAmountBeforeTextChange <> Me._currentLoanSchedule.TotalLoanAmount Then
                Me._currentLoanSchedule.DeductionPercentage = 0
            End If
        End If

        UpdateBalanceAndNumberOfPayPeriod()

    End Sub

    Private Sub UpdateBalanceAndNumberOfPayPeriod()
        Dim totalLoanAmount = AccuMath.CommercialRound(Me._currentLoanSchedule.TotalLoanAmount)
        Dim deductionAmount = AccuMath.CommercialRound(Me._currentLoanSchedule.DeductionAmount)

        If Me._currentLoanTransactions.Count = 0 AndAlso
            (Me._currentLoanSchedule.Status IsNot LoanScheduleRepository.STATUS_CANCELLED OrElse
            Me._currentLoanSchedule.Status IsNot LoanScheduleRepository.STATUS_COMPLETE) Then

            Dim numberOfPayPeriod = _loanScheduleRepository.ComputeNumberOfPayPeriod(totalLoanAmount, deductionAmount)

            Me._currentLoanSchedule.TotalBalanceLeft = totalLoanAmount
            Me._currentLoanSchedule.NoOfPayPeriod = numberOfPayPeriod

        End If

        Dim totalBalanceLeft = AccuMath.CommercialRound(Me._currentLoanSchedule.TotalBalanceLeft)

        Me._currentLoanSchedule.LoanPayPeriodLeft = _loanScheduleRepository.ComputeNumberOfPayPeriod(totalBalanceLeft, deductionAmount)
    End Sub

    Private Sub lnlAddLoanType_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnlAddLoanType.LinkClicked

        Dim form As New AddLoanTypeForm()
        form.ShowDialog()

        If form.IsSaved Then

            Me._loanTypeList.Add(form.NewLoanType)

            PopulateLoanTypeCombobox()

            If Me._currentLoanSchedule IsNot Nothing Then

                Me._currentLoanSchedule.LoanTypeID = form.NewLoanType.RowID
                Me._currentLoanSchedule.LoanName = form.NewLoanType.PartNo

                Dim orderedLoanTypeList = Me._loanTypeList.OrderBy(Function(p) p.PartNo).ToList

                cboLoanType.SelectedIndex = orderedLoanTypeList.IndexOf(form.NewLoanType)

            End If

            ShowBalloonInfo("Loan Type Successfully Added", "Saved")
        End If

    End Sub

    Private Async Sub tsbtnSaveLoan_Click(sender As Object, e As EventArgs) Handles tsbtnSaveLoan.Click
        ForceLoanScheduleGridViewCommit()

        Dim changedLoanSchedules As New List(Of LoanSchedule)

        Dim messageTitle = "Update Loans"

        For Each loanSchedule In Me._currentloanSchedules

            If CheckIfLoanScheduleIsChanged(loanSchedule) Then
                loanSchedule.LastUpdBy = z_User
                changedLoanSchedules.Add(loanSchedule)
            End If

        Next

        If changedLoanSchedules.Count < 1 Then

            MessageBoxHelper.Warning("No changed loans!", messageTitle)
            Return

        ElseIf changedLoanSchedules.Count > 1 AndAlso MessageBoxHelper.Confirm(Of Boolean) _
            ($"You are about to update multiple loans. Do you want to proceed?", "Confirm Multiple Updates") = False Then

            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function() As Task
                Await _loanScheduleRepository.SaveManyAsync(changedLoanSchedules, Me._loanTypeList)

                For Each item In changedLoanSchedules
                    RecordUpdate(item)
                Next

                ShowBalloonInfo($"{changedLoanSchedules.Count} Loan(s) Successfully Updated.", messageTitle)

                Dim currentEmployee = GetSelectedEmployee()

                If currentEmployee Is Nothing Then Return

                Await LoadLoanSchedules(currentEmployee)
            End Function)
    End Sub

    Private Sub cboLoanType_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboLoanType.SelectedValueChanged
        UpdateLoanTypeID()

    End Sub

    Private Sub UpdateLoanTypeID()
        If Me._currentLoanSchedule IsNot Nothing Then
            Dim selectedLoanType = Me._loanTypeList.FirstOrDefault(Function(l) l.PartNo = cboLoanType.Text)

            If selectedLoanType Is Nothing Then

                Me._currentLoanSchedule.LoanTypeID = Nothing
            Else

                Me._currentLoanSchedule.LoanTypeID = selectedLoanType.RowID

            End If
        End If
    End Sub

    Private Async Sub tsbtnNewLoan_Click(sender As Object, e As EventArgs) Handles tsbtnNewLoan.Click

        Dim employee As Employee = GetSelectedEmployee()

        If employee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")

            Return
        End If

        Dim form As New AddLoanScheduleForm(employee)
        form.ShowDialog()

        If form.IsSaved Then

            If form.NewLoanTypes.Count > 0 Then

                For Each loanType In form.NewLoanTypes
                    Me._loanTypeList.Add(loanType)
                Next

                PopulateLoanTypeCombobox()
            End If

            Await LoadLoanSchedules(employee)

            If form.ShowBalloonSuccess Then
                ShowBalloonInfo("Loan Successfully Added", "Saved")
            End If
        End If

    End Sub

    Private Sub LoanScheduleBindingSource_CurrentItemChanged(sender As Object, e As EventArgs) Handles LoanScheduleBindingSource.CurrentItemChanged

        Dim currentRow = loanSchedulesDataGridView.CurrentRow

        If currentRow Is Nothing Then Return

        If Me._currentLoanSchedule Is Nothing Then Return

        Dim hasChanged = CheckIfLoanScheduleIsChanged(Me._currentLoanSchedule)

        If hasChanged Then
            currentRow.DefaultCellStyle.BackColor = Color.Yellow
            currentRow.DefaultCellStyle.SelectionForeColor = Color.Red
        Else
            currentRow.DefaultCellStyle.BackColor = Color.White
            currentRow.DefaultCellStyle.SelectionForeColor = Color.Black

        End If

    End Sub

    Private Sub tsbtnCancelLoan_Click(sender As Object, e As EventArgs) Handles tsbtnCancelLoan.Click

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")
            Return
        End If

        If Me._currentloanSchedules Is Nothing Then
            MessageBoxHelper.Warning("No loan schedules!")
            Return
        End If

        Me._currentloanSchedules = Me._changedLoanSchedules.CloneListJson()

        LoanScheduleBindingSource.DataSource = Me._currentloanSchedules

        loanSchedulesDataGridView.DataSource = LoanScheduleBindingSource

    End Sub

    Private Async Sub tsbtnDeleteLoanScheduleButton_Click(sender As Object, e As EventArgs) Handles DeleteLoanScheduleButton.Click

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")
            Return
        End If

        Const messageTitle As String = "Delete Loan"

        If Me._currentLoanSchedule Is Nothing OrElse
            Me._currentLoanSchedule.RowID Is Nothing Then
            MessageBoxHelper.Warning("No loan selected!")

            Return
        End If

        Dim currentLoanSchedule = Await _loanScheduleRepository.
                                            GetByIdAsync(Me._currentLoanSchedule.RowID.Value)

        If currentLoanSchedule Is Nothing Then

            MessageBoxHelper.Warning("Loan not found in database! Please close this form the open again.")

            Return
        End If

        Dim loanNumber = Me._currentLoanSchedule.LoanNumber

        Dim loanNumberString = If(String.IsNullOrWhiteSpace(loanNumber), "", $": {loanNumber} ")

        If MessageBoxHelper.Confirm(Of Boolean) _
        ($"Are you sure you want to delete loan{loanNumberString}?", "Confirm Deletion") = False Then

            Return
        End If

        If currentLoanSchedule.Status = LoanScheduleRepository.STATUS_COMPLETE Then

            If MessageBoxHelper.Confirm(Of Boolean) _
        ("Loan schedule is already completed. Deleting this might affect previous cutoffs. Do you want to proceed deletion?", "Confirm Deletion",
            messageBoxIcon:=MessageBoxIcon.Warning) = False Then

                Return
            End If
        Else

            Dim loanTransactions = Await _loanScheduleRepository.
                GetLoanTransactionsWithPayPeriodAsync(Me._currentLoanSchedule.RowID.Value)

            If loanTransactions.Count > 0 Then

                If MessageBoxHelper.Confirm(Of Boolean) _
        ("This loan has already started. Deleting this might affect previous cutoffs. Do you want to proceed deletion?", "Confirm Deletion",
            messageBoxIcon:=MessageBoxIcon.Warning) = False Then

                    Return
                End If

            End If
        End If

        Await DeleteLoanSchedule(currentEmployee, messageTitle, loanNumberString)

    End Sub

    Private Async Function DeleteLoanSchedule(currentEmployee As Employee,
                                              messageTitle As String,
                                              loanNumberString As String) As Task

        If Me._currentLoanSchedule Is Nothing OrElse
            Me._currentLoanSchedule.RowID Is Nothing Then
            MessageBoxHelper.Warning("No loan selected!")

            Return
        End If

        Try

            Await _loanScheduleRepository.DeleteAsync(Me._currentLoanSchedule.RowID.Value)

            Dim repo As New UserActivityRepository
            repo.RecordDelete(z_User, FormEntityName, CInt(Me._currentLoanSchedule.RowID), z_OrganizationID)

            Await LoadLoanSchedules(currentEmployee)

            ShowBalloonInfo($"Loan {If(String.IsNullOrWhiteSpace(loanNumberString), " ", loanNumberString)} Successfully Deleted.", messageTitle)
        Catch ex As ArgumentException

            MessageBoxHelper.ErrorMessage(ex.Message, messageTitle)
        Catch ex As Exception

            MessageBoxHelper.DefaultErrorMessage(messageTitle, ex)

        End Try
    End Function

    Private Async Sub tsbtnImportLoans_Click(sender As Object, e As EventArgs) Handles tsbtnImportLoans.Click

        Using form = New ImportLoansForm()
            form.ShowDialog()

            If form.IsSaved Then

                Await LoadLoanTypes()

                Dim currentEmployee = GetSelectedEmployee()

                If currentEmployee IsNot Nothing Then
                    Await LoadLoanSchedules(currentEmployee)
                End If

                ShowBalloonInfo("Loans Successfully Imported", "Import Loans")

            End If

        End Using
    End Sub

    Private Sub EmployeeLoansForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        PayrollForm.listPayrollForm.Remove(Me.Name)
        myBalloon(, , lblFormTitle, , , 1)

    End Sub

    Private Async Sub checkboxFilter_CheckedChanged(sender As Object, e As EventArgs) Handles chkOnHoldFilter.CheckedChanged, chkInProgressFilter.CheckedChanged, chkCompleteFilter.CheckedChanged, chkCancelledFilter.CheckedChanged

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then Return

        Await LoadLoanSchedules(currentEmployee)
    End Sub

    Private Sub loanHistoryGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles loanHistoryGridView.DataError
        e.Cancel = True
    End Sub

    Private Async Sub cbShowAll_CheckedChanged(sender As Object, e As EventArgs) Handles cbShowAll.CheckedChanged
        Await ShowEmployeeList()
    End Sub

#Region "Private Functions"

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, lblFormTitle, 400)
    End Sub

    Private Sub InitializeComponentSettings()
        employeesDataGridView.AutoGenerateColumns = False
        loanSchedulesDataGridView.AutoGenerateColumns = False
        loanHistoryGridView.AutoGenerateColumns = False
    End Sub

    Private Sub LoadLoanStatus()

        Dim statusList = _loanScheduleRepository.GetStatusList()

        statusList.Remove(LoanScheduleRepository.STATUS_COMPLETE)

        cmbLoanStatus.DataSource = statusList

    End Sub

    Private Async Function FilterEmployees(Optional searchValue As String = "") As Task

        chkInProgressFilter.Text = LoanScheduleRepository.STATUS_IN_PROGRESS
        chkOnHoldFilter.Text = LoanScheduleRepository.STATUS_ON_HOLD
        chkCancelledFilter.Text = LoanScheduleRepository.STATUS_CANCELLED
        chkCompleteFilter.Text = LoanScheduleRepository.STATUS_COMPLETE

        ResetLoanScheduleForm()
        Me._currentloanSchedules.Clear()
        Me.LoanScheduleBindingSource.Clear()
        loanSchedulesDataGridView.DataSource = LoanScheduleBindingSource

        If String.IsNullOrEmpty(searchValue) Then
            employeesDataGridView.DataSource = Me._employees
        Else
            employeesDataGridView.DataSource =
                Await _employeeRepository.SearchSimpleLocal(Me._employees, searchValue)
        End If

        If employeesDataGridView.Rows.Count = 0 Then

            Me._currentloanSchedules.Clear()
            Me.LoanScheduleBindingSource.Clear()
            loanSchedulesDataGridView.DataSource = LoanScheduleBindingSource

            loanSchedulesDataGridView.Rows.Clear()

        End If

    End Function

    Private Async Function LoadLoanSchedules(currentEmployee As Employee) As Task
        If currentEmployee?.RowID Is Nothing Then Return

        Dim inProgressChecked = chkInProgressFilter.Checked
        Dim onHoldChecked = chkOnHoldFilter.Checked
        Dim cancelledChecked = chkCancelledFilter.Checked
        Dim completeChecked = chkCompleteFilter.Checked

        Dim loanSchedules = Await _loanScheduleRepository.
                                    GetByEmployeeAsync(currentEmployee.RowID.Value)

        Dim statusFilter = CreateStatusFilter()

        If statusFilter Is Nothing Then
            Me._currentloanSchedules = New List(Of LoanSchedule)
        Else
            Me._currentloanSchedules = loanSchedules.Where(statusFilter).ToList

        End If

        Me._changedLoanSchedules = Me._currentloanSchedules.CloneListJson()

        LoanScheduleBindingSource.DataSource = Me._currentloanSchedules

        loanSchedulesDataGridView.DataSource = LoanScheduleBindingSource

        chkInProgressFilter.Text = $"{LoanScheduleRepository.STATUS_IN_PROGRESS} ({loanSchedules.Count(Function(l) l.Status = LoanScheduleRepository.STATUS_IN_PROGRESS)})"
        chkOnHoldFilter.Text = $"{LoanScheduleRepository.STATUS_ON_HOLD} ({loanSchedules.Count(Function(l) l.Status = LoanScheduleRepository.STATUS_ON_HOLD)})"
        chkCancelledFilter.Text = $"{LoanScheduleRepository.STATUS_CANCELLED} ({loanSchedules.Count(Function(l) l.Status = LoanScheduleRepository.STATUS_CANCELLED)})"
        chkCompleteFilter.Text = $"{LoanScheduleRepository.STATUS_COMPLETE} ({loanSchedules.Count(Function(l) l.Status = LoanScheduleRepository.STATUS_COMPLETE)})"

    End Function

    Private Function CreateStatusFilter() As Func(Of LoanSchedule, Boolean)

        Dim inProgressChecked = chkInProgressFilter.Checked
        Dim onHoldChecked = chkOnHoldFilter.Checked
        Dim cancelledChecked = chkCancelledFilter.Checked
        Dim completeChecked = chkCompleteFilter.Checked

        If Not inProgressChecked AndAlso
                Not onHoldChecked AndAlso
                Not cancelledChecked AndAlso
                Not completeChecked Then
            Return Nothing
        End If

        Dim whereFunction = Function(loanSchedule As LoanSchedule) As Boolean

                                Return (inProgressChecked AndAlso loanSchedule.Status = LoanScheduleRepository.STATUS_IN_PROGRESS) OrElse
                                   (onHoldChecked AndAlso loanSchedule.Status = LoanScheduleRepository.STATUS_ON_HOLD) OrElse
                                   (cancelledChecked AndAlso loanSchedule.Status = LoanScheduleRepository.STATUS_CANCELLED) OrElse
                                   (completeChecked AndAlso loanSchedule.Status = LoanScheduleRepository.STATUS_COMPLETE)

                            End Function

        Return whereFunction
    End Function

    Private Async Function UpdateSelectedLoanScheduleData() As Task

        Dim currentLoanSchedule As LoanSchedule = GetSelectedLoanSchedule()
        If currentLoanSchedule Is Nothing Then Return

        Me._currentLoanTransactions = New List(Of LoanTransaction) _
            (Await _loanScheduleRepository.GetLoanTransactionsWithPayPeriodAsync(currentLoanSchedule.RowID.Value))

        loanHistoryGridView.DataSource = Me._currentLoanTransactions

        Dim loanHistoryCount As Integer = loanHistoryGridView.Rows.Count

        If loanHistoryCount > 0 Then

            loanHistoryGridView.CurrentCell = loanHistoryGridView.Rows(loanHistoryCount - 1).Cells(0)

        End If

        PopulateLoanScheduleForm(currentLoanSchedule)

    End Function

    Private Async Function LoadLoanTypes() As Task

        If sysowner_is_benchmark Then

            Me._loanTypeList = New List(Of Product)(Await _productRepository.GetGovernmentLoanTypesAsync(z_OrganizationID))
        Else
            Me._loanTypeList = New List(Of Product)(Await _productRepository.GetLoanTypesAsync(z_OrganizationID))

        End If

        PopulateLoanTypeCombobox()

    End Function

    Private Sub PopulateLoanTypeCombobox()
        Dim loanTypes = _productRepository.ConvertToStringList(Me._loanTypeList)

        cboLoanType.DataSource = loanTypes
    End Sub

    Private Async Function LoadDeductionSchedules() As Task

        Dim deductionSchedules = _listOfValueRepository.
                    ConvertToStringList(Await _listOfValueRepository.GetDeductionSchedulesAsync())

        cmbDeductionSchedule.DataSource = deductionSchedules
    End Function

    Private Function GetSelectedEmployee() As Employee
        If employeesDataGridView.CurrentRow Is Nothing Then Return Nothing

        Return CType(employeesDataGridView.CurrentRow.DataBoundItem, Employee)
    End Function

    Private Sub PopulateLoanScheduleForm(loanSchedule As LoanSchedule)
        Me._currentLoanSchedule = loanSchedule

        Dim originalLoanSchedule = Me._changedLoanSchedules.
            FirstOrDefault(Function(l) Nullable.Equals(l.RowID, Me._currentLoanSchedule.RowID))

        Dim isUneditable As Boolean = False

        If originalLoanSchedule IsNot Nothing Then
            isUneditable = originalLoanSchedule.Status = LoanScheduleRepository.STATUS_CANCELLED OrElse
            originalLoanSchedule.Status = LoanScheduleRepository.STATUS_COMPLETE
        End If

        tbpDetails.Enabled = Not isUneditable

        txtLoanNumber.DataBindings.Clear()
        txtLoanNumber.DataBindings.Add("Text", Me._currentLoanSchedule, "LoanNumber")

        txtRemarks.DataBindings.Clear()
        txtRemarks.DataBindings.Add("Text", Me._currentLoanSchedule, "Comments")

        txtTotalLoanAmount.DataBindings.Clear()
        txtTotalLoanAmount.DataBindings.Add("Text", Me._currentLoanSchedule, "TotalLoanAmount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")
        If Me._currentLoanTransactions IsNot Nothing AndAlso Me._currentLoanTransactions.Count > 0 Then
            txtTotalLoanAmount.Enabled = False
            txtLoanInterestPercentage.Enabled = False
        Else
            txtTotalLoanAmount.Enabled = True
            txtLoanInterestPercentage.Enabled = True
        End If

        txtLoanBalance.DataBindings.Clear()
        txtLoanBalance.DataBindings.Add("Text", Me._currentLoanSchedule, "TotalBalanceLeft", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        dtpDateFrom.DataBindings.Clear()
        dtpDateFrom.DataBindings.Add("Value", Me._currentLoanSchedule, "DedEffectiveDateFrom")

        txtNumberOfPayPeriod.DataBindings.Clear()
        txtNumberOfPayPeriod.DataBindings.Add("Text", Me._currentLoanSchedule, "NoOfPayPeriod", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N0")

        txtNumberOfPayPeriodLeft.DataBindings.Clear()
        txtNumberOfPayPeriodLeft.DataBindings.Add("Text", Me._currentLoanSchedule, "LoanPayPeriodLeft", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N0")

        txtDeductionAmount.DataBindings.Clear()
        txtDeductionAmount.DataBindings.Add("Text", Me._currentLoanSchedule, "DeductionAmount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        txtLoanInterestPercentage.DataBindings.Clear()
        txtLoanInterestPercentage.DataBindings.Add("Text", Me._currentLoanSchedule, "DeductionPercentage", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        cboLoanType.DataBindings.Clear()
        cboLoanType.DataBindings.Add("Text", Me._currentLoanSchedule, "LoanName")
        cboLoanType.Text = Me._currentLoanSchedule.LoanName
        UpdateLoanTypeID()

        txtLoanStatus.DataBindings.Clear()
        cmbLoanStatus.DataBindings.Clear()
        If isUneditable Then

            txtLoanStatus.DataBindings.Add("Text", Me._currentLoanSchedule, "Status")
        Else
            cmbLoanStatus.DataBindings.Add("Text", Me._currentLoanSchedule, "Status")

        End If

        cmbDeductionSchedule.DataBindings.Clear()
        cmbDeductionSchedule.DataBindings.Add("Text", Me._currentLoanSchedule, "DeductionSchedule")

        ToggleLoanStatusComboboxVisibility(Not isUneditable)

    End Sub

    Private Sub ResetLoanScheduleForm()

        Me._currentLoanSchedule = Nothing

        Me._currentLoanTransactions.Clear()

        txtLoanNumber.Clear()
        txtLoanNumber.DataBindings.Clear()

        txtRemarks.Clear()
        txtRemarks.DataBindings.Clear()

        txtTotalLoanAmount.Clear()
        txtTotalLoanAmount.DataBindings.Clear()

        txtLoanBalance.Clear()
        txtLoanBalance.DataBindings.Clear()

        dtpDateFrom.ResetText()
        dtpDateFrom.DataBindings.Clear()

        txtNumberOfPayPeriod.Clear()
        txtNumberOfPayPeriod.DataBindings.Clear()

        txtNumberOfPayPeriodLeft.Clear()
        txtNumberOfPayPeriodLeft.DataBindings.Clear()

        txtDeductionAmount.Clear()
        txtDeductionAmount.DataBindings.Clear()

        txtLoanInterestPercentage.Clear()
        txtLoanInterestPercentage.DataBindings.Clear()

        cboLoanType.SelectedIndex = -1
        cboLoanType.DataBindings.Clear()
        txtLoanStatus.Clear()
        txtLoanStatus.DataBindings.Clear()

        cmbLoanStatus.SelectedIndex = -1
        cmbLoanStatus.DataBindings.Clear()

        ToggleLoanStatusComboboxVisibility(True)

        cmbDeductionSchedule.SelectedIndex = -1
        cmbDeductionSchedule.DataBindings.Clear()

        tbpHistory.Text = LOAN_HISTORY_TAB_TEXT

    End Sub

    Private Sub ToggleLoanStatusComboboxVisibility(showCombobox As Boolean)

        cmbLoanStatus.Visible = showCombobox
        txtLoanStatus.Visible = Not showCombobox

    End Sub

    Private Function GetSelectedLoanSchedule() As LoanSchedule
        Return CType(loanSchedulesDataGridView.CurrentRow.DataBoundItem, LoanSchedule)
    End Function

    Private Sub ForceLoanScheduleGridViewCommit()
        'Workaround. Focus other control to lose focus on current control
        EmployeeInfoTabLayout.Focus()
    End Sub

    Private Function CheckIfLoanScheduleIsChanged(newLoanSchedule As LoanSchedule) As Boolean

        Dim oldLoanSchedule =
            Me._changedLoanSchedules.
                FirstOrDefault(Function(l) Nullable.Equals(l.RowID, newLoanSchedule.RowID))

        If oldLoanSchedule Is Nothing Then Return False

        Dim hasChanged = False

        If _
            Nullable.Equals(newLoanSchedule.LoanTypeID, oldLoanSchedule.LoanTypeID) = False OrElse
            newLoanSchedule.LoanNumber <> oldLoanSchedule.LoanNumber OrElse
            newLoanSchedule.TotalLoanAmount <> oldLoanSchedule.TotalLoanAmount OrElse
            newLoanSchedule.TotalBalanceLeft <> oldLoanSchedule.TotalBalanceLeft OrElse
            newLoanSchedule.DedEffectiveDateFrom <> oldLoanSchedule.DedEffectiveDateFrom OrElse
            newLoanSchedule.NoOfPayPeriod <> oldLoanSchedule.NoOfPayPeriod OrElse
            newLoanSchedule.LoanPayPeriodLeft <> oldLoanSchedule.LoanPayPeriodLeft OrElse
            newLoanSchedule.DeductionAmount <> oldLoanSchedule.DeductionAmount OrElse
            newLoanSchedule.Status <> oldLoanSchedule.Status OrElse
            newLoanSchedule.DeductionPercentage <> oldLoanSchedule.DeductionPercentage OrElse
            newLoanSchedule.Comments <> oldLoanSchedule.Comments OrElse
            newLoanSchedule.DeductionSchedule <> oldLoanSchedule.DeductionSchedule Then

            hasChanged = True
        End If

        Return hasChanged

    End Function

    Private Function RecordUpdate(newLoanSchedule As LoanSchedule) As Boolean

        Dim oldLoanSchedule =
            Me._changedLoanSchedules.
                FirstOrDefault(Function(l) Nullable.Equals(l.RowID, newLoanSchedule.RowID))

        If oldLoanSchedule Is Nothing Then Return False

        Dim changes = New List(Of UserActivityItem)

        Dim entityName = FormEntityName.ToLower()

        If newLoanSchedule.LoanName <> oldLoanSchedule.LoanName Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldLoanSchedule.RowID),
                        .Description = $"Updated {entityName} type from '{oldLoanSchedule.LoanName}' to '{newLoanSchedule.LoanName}'."
                        })
        End If
        If newLoanSchedule.LoanNumber <> oldLoanSchedule.LoanNumber Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldLoanSchedule.RowID),
                        .Description = $"Updated {entityName} number from '{oldLoanSchedule.LoanNumber}' to '{newLoanSchedule.LoanNumber}'."
                        })
        End If
        If newLoanSchedule.TotalLoanAmount <> oldLoanSchedule.TotalLoanAmount Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldLoanSchedule.RowID),
                        .Description = $"Updated {entityName} total amount from '{oldLoanSchedule.TotalLoanAmount.ToString}' to '{newLoanSchedule.TotalLoanAmount.ToString}'."
                        })
        End If
        If newLoanSchedule.DedEffectiveDateFrom <> oldLoanSchedule.DedEffectiveDateFrom Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldLoanSchedule.RowID),
                        .Description = $"Updated {entityName} date from '{oldLoanSchedule.DedEffectiveDateFrom.ToShortDateString}' to '{newLoanSchedule.DedEffectiveDateFrom.ToShortDateString}'."
                        })
        End If
        If newLoanSchedule.DeductionAmount <> oldLoanSchedule.DeductionAmount Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldLoanSchedule.RowID),
                        .Description = $"Updated {entityName} deduction amount from '{oldLoanSchedule.DeductionAmount.ToString}' to '{newLoanSchedule.DeductionAmount.ToString}'."
                        })
        End If
        If newLoanSchedule.Status <> oldLoanSchedule.Status Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldLoanSchedule.RowID),
                        .Description = $"Updated {entityName} status from '{oldLoanSchedule.Status}' to '{newLoanSchedule.Status}'."
                        })
        End If
        If newLoanSchedule.DeductionPercentage <> oldLoanSchedule.DeductionPercentage Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldLoanSchedule.RowID),
                        .Description = $"Updated {entityName} interest percentage from '{oldLoanSchedule.DeductionPercentage.ToString}' to '{newLoanSchedule.DeductionPercentage.ToString}'."
                        })
        End If
        If newLoanSchedule.DeductionSchedule <> oldLoanSchedule.DeductionSchedule Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldLoanSchedule.RowID),
                        .Description = $"Updated {entityName} deduction schedule from '{oldLoanSchedule.DeductionSchedule}' to '{newLoanSchedule.DeductionSchedule}'."
                        })
        End If
        If newLoanSchedule.Comments <> oldLoanSchedule.Comments Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldLoanSchedule.RowID),
                        .Description = $"Updated {entityName} comments from '{oldLoanSchedule.Comments}' to '{newLoanSchedule.Comments}'."
                        })
        End If

        Dim repo = New UserActivityRepository
        repo.CreateRecord(z_User, FormEntityName, z_OrganizationID, UserActivityRepository.RecordTypeEdit, changes)

        Return True
    End Function

    Private Async Function LoadEmployees() As Task

        Me._allEmployees = (Await _employeeRepository.GetAllWithPositionAsync(z_OrganizationID)).
                            OrderBy(Function(e) e.LastName).
                            ToList

    End Function

    Private Async Function ShowEmployeeList() As Task

        If cbShowAll.Checked Then

            Me._employees = Me._allEmployees
        Else

            Me._employees = Me._allEmployees.Where(Function(e) e.IsActive).ToList

        End If

        Await FilterEmployees()
    End Function

    Private Sub UserActivityToolStripButton_Click(sender As Object, e As EventArgs) Handles UserActivityToolStripButton.Click
        Dim userActivity As New UserActivityForm(FormEntityName)
        userActivity.ShowDialog()
    End Sub

#End Region

End Class