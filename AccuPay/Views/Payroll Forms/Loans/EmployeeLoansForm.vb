Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class EmployeeLoansForm

    Private sysowner_is_benchmark As Boolean

    Private _currentLoan As LoanSchedule

    Private _employees As List(Of Employee)

    Private _allEmployees As List(Of Employee)

    Private _loanTypeList As List(Of Product)

    Private _currentloans As List(Of LoanSchedule)

    Private _changedLoans As List(Of LoanSchedule)

    Private _currentLoanTransactions As List(Of LoanTransaction)

    Private ReadOnly _employeeRepository As EmployeeRepository

    Private ReadOnly _productRepository As ProductRepository

    Private ReadOnly _listOfValueRepository As ListOfValueRepository

    Private ReadOnly _userActivityRepository As UserActivityRepository

    Private ReadOnly _textBoxDelayedAction As DelayedAction(Of Boolean)

    Private Const LOAN_HISTORY_TAB_TEXT As String = "Loan History"

    Private Const FormEntityName As String = "Loan"

    Private loanAmountBeforeTextChange As Decimal

    Private loanInterestPercentageBeforeTextChange As Decimal

    Private _currentRolePermission As RolePermission

    Sub New()

        InitializeComponent()

        _employees = New List(Of Employee)

        _allEmployees = New List(Of Employee)

        _currentloans = New List(Of LoanSchedule)

        _changedLoans = New List(Of LoanSchedule)

        _currentLoanTransactions = New List(Of LoanTransaction)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)

        _productRepository = MainServiceProvider.GetRequiredService(Of ProductRepository)

        _listOfValueRepository = MainServiceProvider.GetRequiredService(Of ListOfValueRepository)

        _userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)

        _textBoxDelayedAction = New DelayedAction(Of Boolean)
    End Sub

    Private Async Sub EmployeeLoansForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        InitializeComponentSettings()

        Await CheckRolePermissions()

        LoadLoanStatus()
        Await LoadLoanTypes()
        Await LoadDeductionSchedules()

        Await LoadEmployees()
        Await ShowEmployeeList()

        AddHandler SearchTextBox.TextChanged, AddressOf SearchTextBox_TextChanged

        Dim checker = FeatureListChecker.Instance
        lnkBonusPayment.Visible = checker.HasAccess(Feature.LoanDeductFromBonus)
    End Sub

    Private Async Function CheckRolePermissions() As Task
        Dim role = Await PermissionHelper.GetRoleAsync(PermissionConstant.LOAN)

        NewToolStripButton.Visible = False
        ImportToolStripButton.Visible = False
        SaveToolStripButton.Visible = False
        CancelToolStripButton.Visible = False
        DeleteToolStripButton.Visible = False
        DetailsTabLayout.Enabled = False

        If role.Success Then

            _currentRolePermission = role.RolePermission

            If _currentRolePermission.Create Then
                NewToolStripButton.Visible = True
                ImportToolStripButton.Visible = True

            End If

            If _currentRolePermission.Update Then
                SaveToolStripButton.Visible = True
                CancelToolStripButton.Visible = True
                DetailsTabLayout.Enabled = True
            End If

            If _currentRolePermission.Delete Then
                DeleteToolStripButton.Visible = True

            End If

        End If
    End Function

    Private Sub SearchTextBox_TextChanged(sender As Object, e As EventArgs)

        _textBoxDelayedAction.ProcessAsync(
            Async Function()
                Await FilterEmployees(SearchTextBox.Text.ToLower())

                Return True
            End Function)

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Async Sub EmployeesDataGridView_SelectionChanged(sender As Object, e As EventArgs)

        Await ShowEmployeeLoans()

    End Sub

    Private Async Function ShowEmployeeLoans() As Task

        ResetForm()

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then Return

        EmployeeNameTextBox.Text = currentEmployee.FullNameWithMiddleInitial
        EmployeeNumberTextBox.Text = currentEmployee.EmployeeIdWithPositionAndEmployeeType

        EmployeePictureBox.Image = ConvByteToImage(currentEmployee.Image)

        Await LoadLoans(currentEmployee)

    End Function

    Private Async Sub LoanGridView_SelectionChanged(sender As Object, e As EventArgs)

        Await ShowLoanDetails()

    End Sub

    Private Async Function ShowLoanDetails() As Task
        If LoanGridView.CurrentRow Is Nothing Then Return

        Dim currentLoan As LoanSchedule = GetSelectedLoan()

        Dim currentEmployee = GetSelectedEmployee()
        If currentLoan IsNot Nothing AndAlso currentEmployee IsNot Nothing AndAlso
           Nullable.Equals(currentLoan.EmployeeID, currentEmployee.RowID) Then

            Await UpdateSelectedLoanScheduleData()

            tbpHistory.Text = $"{LOAN_HISTORY_TAB_TEXT} ({Me._currentLoanTransactions.Count})"

            DetailsTabControl.Enabled = True
        End If

    End Function

    Private Sub txtLoanInterestPercentage_Enter(sender As Object, e As EventArgs) Handles txtLoanInterestPercentage.Enter

        If Me._currentLoan Is Nothing Then Return

        loanInterestPercentageBeforeTextChange = Me._currentLoan.DeductionPercentage

    End Sub

    Private Sub txtLoanInterestPercentage_Leave(sender As Object, e As EventArgs) Handles txtLoanInterestPercentage.Leave

        If Me._currentLoan Is Nothing Then Return

        If loanInterestPercentageBeforeTextChange = Me._currentLoan.DeductionPercentage Then Return

        Dim totalPlusInterestRate As Decimal = 1 + (Me._currentLoan.DeductionPercentage * 0.01D)

        Me._currentLoan.TotalLoanAmount = Me._currentLoan.TotalLoanAmount * totalPlusInterestRate

        UpdateBalanceAndNumberOfPayPeriod()

    End Sub

    Private Sub txtTotalLoanAmount_Enter(sender As Object, e As EventArgs) Handles txtTotalLoanAmount.Enter

        If Me._currentLoan Is Nothing Then Return

        loanAmountBeforeTextChange = Me._currentLoan.TotalLoanAmount

    End Sub

    Private Sub txtTotalLoanAmount_Leave(sender As Object, e As EventArgs) _
        Handles txtTotalLoanAmount.Leave, txtDeductionAmount.Leave

        If Me._currentLoan Is Nothing Then Return

        If sender Is txtTotalLoanAmount Then
            If loanAmountBeforeTextChange <> Me._currentLoan.TotalLoanAmount Then
                Me._currentLoan.DeductionPercentage = 0
            End If
        End If

        UpdateBalanceAndNumberOfPayPeriod()

    End Sub

    Private Sub UpdateBalanceAndNumberOfPayPeriod()
        Me._currentLoan.TotalLoanAmount = AccuMath.CommercialRound(Me._currentLoan.TotalLoanAmount)
        Me._currentLoan.DeductionAmount = AccuMath.CommercialRound(Me._currentLoan.DeductionAmount)

        If Me._currentLoanTransactions.Count = 0 AndAlso IsUnEditable() = False Then

            Me._currentLoan.RecomputeTotalPayPeriod()

            Me._currentLoan.TotalBalanceLeft = Me._currentLoan.TotalLoanAmount

        End If

        Me._currentLoan.RecomputePayPeriodLeft()
    End Sub

    Private Function IsUnEditable() As Boolean

        Return Me._currentLoan.Status = LoanSchedule.STATUS_CANCELLED OrElse
                    Me._currentLoan.Status = LoanSchedule.STATUS_COMPLETE
    End Function

    Private Sub lnlAddLoanType_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnlAddLoanType.LinkClicked

        Dim form As New AddLoanTypeForm()
        form.ShowDialog()

        If form.IsSaved Then

            Me._loanTypeList.Add(form.NewLoanType)

            PopulateLoanTypeCombobox()

            If Me._currentLoan IsNot Nothing Then

                Me._currentLoan.LoanTypeID = form.NewLoanType.RowID
                Me._currentLoan.LoanName = form.NewLoanType.PartNo

                Dim orderedLoanTypeList = Me._loanTypeList.OrderBy(Function(p) p.PartNo).ToList

                cboLoanType.SelectedIndex = orderedLoanTypeList.IndexOf(form.NewLoanType)

            End If

            ShowBalloonInfo("Loan Type Successfully Added", "Saved")
        End If

    End Sub

    Private Async Sub SaveToolStripButton_Click(sender As Object, e As EventArgs) Handles SaveToolStripButton.Click
        ForceLoanScheduleGridViewCommit()

        Dim changedLoans As New List(Of LoanSchedule)

        Dim messageTitle = "Update Loans"

        For Each loan In Me._currentloans

            If CheckIfLoanIsChanged(loan) Then
                loan.LastUpdBy = z_User
                changedLoans.Add(loan)
            End If

        Next

        If changedLoans.Count < 1 Then

            MessageBoxHelper.Warning("No changed loans!", messageTitle)
            Return

        ElseIf changedLoans.Count > 1 AndAlso MessageBoxHelper.Confirm(Of Boolean) _
            ($"You are about to update multiple loans. Do you want to proceed?", "Confirm Multiple Updates") = False Then

            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of LoanDataService)
                Await dataService.SaveManyAsync(changedLoans, z_User)

                For Each item In changedLoans
                    RecordUpdate(item)
                Next

                ShowBalloonInfo($"{changedLoans.Count} Loan(s) Successfully Updated.", messageTitle)

                Dim currentEmployee = GetSelectedEmployee()

                If currentEmployee Is Nothing Then Return

                Await LoadLoans(currentEmployee)
            End Function)
    End Sub

    Private Sub cboLoanType_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboLoanType.SelectedValueChanged
        UpdateLoanTypeID()

    End Sub

    Private Sub UpdateLoanTypeID()
        If Me._currentLoan IsNot Nothing Then
            Dim selectedLoanType = Me._loanTypeList.FirstOrDefault(Function(l) l.PartNo = cboLoanType.Text)

            If selectedLoanType Is Nothing Then

                Me._currentLoan.LoanTypeID = Nothing
            Else

                Me._currentLoan.LoanTypeID = selectedLoanType.RowID

            End If
        End If
    End Sub

    Private Async Sub NewToolStripButton_Click(sender As Object, e As EventArgs) Handles NewToolStripButton.Click

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

            Await LoadLoans(employee)

            If form.ShowBalloonSuccess Then
                ShowBalloonInfo("Loan Successfully Added", "Saved")
            End If
        End If

    End Sub

    Private Sub LoanListBindingSource_CurrentItemChanged(sender As Object, e As EventArgs) Handles LoanListBindingSource.CurrentItemChanged

        Dim currentRow = LoanGridView.CurrentRow

        If currentRow Is Nothing Then Return

        If Me._currentLoan Is Nothing Then Return

        Dim hasChanged = CheckIfLoanIsChanged(Me._currentLoan)

        If hasChanged Then
            currentRow.DefaultCellStyle.BackColor = Color.Yellow
            currentRow.DefaultCellStyle.SelectionForeColor = Color.Red
        Else
            currentRow.DefaultCellStyle.BackColor = Color.White
            currentRow.DefaultCellStyle.SelectionForeColor = Color.Black

        End If

    End Sub

    Private Async Sub CancelToolStripButton_Click(sender As Object, e As EventArgs) Handles CancelToolStripButton.Click

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")
            Return
        End If

        If Me._currentloans Is Nothing Then
            MessageBoxHelper.Warning("No loan schedules!")
            Return
        End If

        Me._currentloans = Me._changedLoans.CloneListJson()

        Await PopulateLoanGridView()
    End Sub

    Private Async Function PopulateLoanGridView() As Task

        RemoveHandler LoanGridView.SelectionChanged, AddressOf LoanGridView_SelectionChanged

        LoanListBindingSource.DataSource = Me._currentloans

        LoanGridView.DataSource = LoanListBindingSource

        Await ShowLoanDetails()

        AddHandler LoanGridView.SelectionChanged, AddressOf LoanGridView_SelectionChanged

    End Function

    Private Async Sub DeleteToolStripButton_Click(sender As Object, e As EventArgs) Handles DeleteToolStripButton.Click

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")
            Return
        End If

        Const messageTitle As String = "Delete Loan"

        If Me._currentLoan Is Nothing OrElse
            Me._currentLoan.RowID Is Nothing Then
            MessageBoxHelper.Warning("No loan selected!")

            Return
        End If

        Dim repository = MainServiceProvider.GetRequiredService(Of LoanRepository)
        Dim currentLoanSchedule = Await repository.GetByIdAsync(Me._currentLoan.RowID.Value)

        If currentLoanSchedule Is Nothing Then

            MessageBoxHelper.Warning("Loan not found in database! Please close this form the open again.")

            Return
        End If

        Dim loanNumber = Me._currentLoan.LoanNumber

        Dim loanNumberString = If(String.IsNullOrWhiteSpace(loanNumber), "", $": {loanNumber} ")

        If MessageBoxHelper.Confirm(Of Boolean) _
        ($"Are you sure you want to delete loan{loanNumberString}?", "Confirm Deletion") = False Then

            Return
        End If

        If currentLoanSchedule.Status = LoanSchedule.STATUS_COMPLETE Then

            MessageBoxHelper.Warning("Cannot delete a completed loan.")

            Return
        Else

            Dim loanTransactions = Await repository.
                GetLoanTransactionsWithPayPeriodAsync(Me._currentLoan.RowID.Value)

            If loanTransactions.Count > 0 Then

                MessageBoxHelper.Warning("This loan has already started and therefore cannot be deleted. Try changing its status to ""On Hold"" or ""Cancelled"" instead.")

                Return
            End If

        End If

        Await DeleteLoanSchedule(currentEmployee, messageTitle, loanNumberString)

    End Sub

    Private Async Function DeleteLoanSchedule(
        currentEmployee As Employee,
        messageTitle As String,
        loanNumberString As String) As Task

        If Me._currentLoan Is Nothing OrElse
            Me._currentLoan.RowID Is Nothing Then
            MessageBoxHelper.Warning("No loan selected!")

            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of LoanDataService)
                Await dataService.DeleteAsync(
                    id:=Me._currentLoan.RowID.Value,
                    changedByUserId:=z_User)

                Await LoadLoans(currentEmployee)

                ShowBalloonInfo($"Loan {If(String.IsNullOrWhiteSpace(loanNumberString), " ", loanNumberString)} Successfully Deleted.", messageTitle)
            End Function)
    End Function

    Private Async Sub ImportToolStripButton_Click(sender As Object, e As EventArgs) Handles ImportToolStripButton.Click

        Using form = New ImportLoansForm()
            form.ShowDialog()

            If form.IsSaved Then

                Await LoadLoanTypes()

                Dim currentEmployee = GetSelectedEmployee()

                If currentEmployee IsNot Nothing Then
                    Await LoadLoans(currentEmployee)
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

        Await LoadLoans(currentEmployee)
    End Sub

    Private Sub LoanHistoryGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles LoanHistoryGridView.DataError
        e.Cancel = True
    End Sub

    Private Async Sub ShowAllCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ShowAllCheckBox.CheckedChanged

        RemoveHandler SearchTextBox.TextChanged, AddressOf SearchTextBox_TextChanged

        SearchTextBox.Clear()
        Await ShowEmployeeList()

        AddHandler SearchTextBox.TextChanged, AddressOf SearchTextBox_TextChanged
    End Sub

#Region "Private Functions"

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, lblFormTitle, 400)
    End Sub

    Private Sub InitializeComponentSettings()
        EmployeesDataGridView.AutoGenerateColumns = False
        LoanGridView.AutoGenerateColumns = False
        LoanHistoryGridView.AutoGenerateColumns = False
    End Sub

    Private Sub LoadLoanStatus()

        Dim repository = MainServiceProvider.GetRequiredService(Of LoanRepository)
        Dim statusList = repository.GetStatusList()

        statusList.Remove(LoanSchedule.STATUS_COMPLETE)

        cmbLoanStatus.DataSource = statusList

    End Sub

    Private Async Function FilterEmployees(Optional searchValue As String = "") As Task

        RemoveHandler EmployeesDataGridView.SelectionChanged, AddressOf EmployeesDataGridView_SelectionChanged

        chkInProgressFilter.Text = LoanSchedule.STATUS_IN_PROGRESS
        chkOnHoldFilter.Text = LoanSchedule.STATUS_ON_HOLD
        chkCancelledFilter.Text = LoanSchedule.STATUS_CANCELLED
        chkCompleteFilter.Text = LoanSchedule.STATUS_COMPLETE

        If String.IsNullOrEmpty(searchValue) Then
            EmployeesDataGridView.DataSource = Me._employees
        Else
            EmployeesDataGridView.DataSource =
                Await _employeeRepository.SearchSimpleLocal(Me._employees, searchValue)
        End If

        Await ShowEmployeeLoans()

        AddHandler EmployeesDataGridView.SelectionChanged, AddressOf EmployeesDataGridView_SelectionChanged

    End Function

    Private Async Function LoadLoans(currentEmployee As Employee) As Task
        If currentEmployee?.RowID Is Nothing Then Return

        Dim inProgressChecked = chkInProgressFilter.Checked
        Dim onHoldChecked = chkOnHoldFilter.Checked
        Dim cancelledChecked = chkCancelledFilter.Checked
        Dim completeChecked = chkCompleteFilter.Checked

        Dim repository = MainServiceProvider.GetRequiredService(Of LoanRepository)
        Dim loanSchedules = Await repository.GetByEmployeeAsync(currentEmployee.RowID.Value)

        loanSchedules = loanSchedules.
            OrderByDescending(Function(l) l.DedEffectiveDateFrom).
            ThenBy(Function(l) l.LoanName).
            ToList()

        Dim statusFilter = CreateStatusFilter()

        If statusFilter Is Nothing Then
            Me._currentloans = New List(Of LoanSchedule)
        Else
            Me._currentloans = loanSchedules.Where(statusFilter).ToList

        End If

        Me._changedLoans = Me._currentloans.CloneListJson()
        Me._changedLoans.ForEach(
            Sub(loan)
                loan.RecomputePayPeriodLeft()
                loan.RecomputeTotalPayPeriod()
            End Sub)

        chkInProgressFilter.Text = $"{LoanSchedule.STATUS_IN_PROGRESS} ({loanSchedules.Where(Function(l) l.Status = LoanSchedule.STATUS_IN_PROGRESS).Count()})"
        chkOnHoldFilter.Text = $"{LoanSchedule.STATUS_ON_HOLD} ({loanSchedules.Where(Function(l) l.Status = LoanSchedule.STATUS_ON_HOLD).Count()})"
        chkCancelledFilter.Text = $"{LoanSchedule.STATUS_CANCELLED} ({loanSchedules.Where(Function(l) l.Status = LoanSchedule.STATUS_CANCELLED).Count()})"
        chkCompleteFilter.Text = $"{LoanSchedule.STATUS_COMPLETE} ({loanSchedules.Where(Function(l) l.Status = LoanSchedule.STATUS_COMPLETE).Count()})"

        Await PopulateLoanGridView()

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

        Dim whereFunction =
            Function(loanSchedule As LoanSchedule) As Boolean

                Return (inProgressChecked AndAlso loanSchedule.Status = LoanSchedule.STATUS_IN_PROGRESS) OrElse
                    (onHoldChecked AndAlso loanSchedule.Status = LoanSchedule.STATUS_ON_HOLD) OrElse
                    (cancelledChecked AndAlso loanSchedule.Status = LoanSchedule.STATUS_CANCELLED) OrElse
                    (completeChecked AndAlso loanSchedule.Status = LoanSchedule.STATUS_COMPLETE)

            End Function

        Return whereFunction
    End Function

    Private Async Function UpdateSelectedLoanScheduleData() As Task

        Dim currentLoanSchedule As LoanSchedule = GetSelectedLoan()
        If currentLoanSchedule Is Nothing Then Return

        Dim repository = MainServiceProvider.GetRequiredService(Of LoanRepository)
        Me._currentLoanTransactions = New List(Of LoanTransaction) _
            (Await repository.GetLoanTransactionsWithPayPeriodAsync(currentLoanSchedule.RowID.Value))

        LoanHistoryGridView.DataSource = Me._currentLoanTransactions

        Dim LoanHistoryCount As Integer = LoanHistoryGridView.Rows.Count

        If LoanHistoryCount > 0 Then

            LoanHistoryGridView.CurrentCell = LoanHistoryGridView.Rows(LoanHistoryCount - 1).Cells(0)

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
        If EmployeesDataGridView.CurrentRow Is Nothing Then Return Nothing

        Return CType(EmployeesDataGridView.CurrentRow.DataBoundItem, Employee)
    End Function

    Private Sub PopulateLoanScheduleForm(loanSchedule As LoanSchedule)
        Me._currentLoan = loanSchedule

        Dim originalLoanSchedule = Me._changedLoans.
            FirstOrDefault(Function(l) Nullable.Equals(l.RowID, Me._currentLoan.RowID))

        Dim unEditable As Boolean = False

        If originalLoanSchedule IsNot Nothing Then
            unEditable = IsUnEditable()
        End If

        If _currentRolePermission.Update Then

            DetailsTabLayout.Enabled = Not unEditable
        End If

        txtLoanNumber.DataBindings.Clear()
        txtLoanNumber.DataBindings.Add("Text", Me._currentLoan, "LoanNumber")

        txtRemarks.DataBindings.Clear()
        txtRemarks.DataBindings.Add("Text", Me._currentLoan, "Comments")

        txtTotalLoanAmount.DataBindings.Clear()
        txtTotalLoanAmount.DataBindings.Add("Text", Me._currentLoan, "TotalLoanAmount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")
        If (Me._currentLoanTransactions IsNot Nothing AndAlso Me._currentLoanTransactions.Count > 0) OrElse
            Me._currentLoan.TotalBalanceLeft < Me._currentLoan.TotalLoanAmount Then
            txtTotalLoanAmount.Enabled = False
            txtLoanInterestPercentage.Enabled = False
            cboLoanType.Enabled = False
        Else
            txtTotalLoanAmount.Enabled = True
            txtLoanInterestPercentage.Enabled = True
            cboLoanType.Enabled = True
        End If

        txtLoanBalance.DataBindings.Clear()
        txtLoanBalance.DataBindings.Add("Text", Me._currentLoan, "TotalBalanceLeft", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        dtpDateFrom.DataBindings.Clear()
        dtpDateFrom.DataBindings.Add("Value", Me._currentLoan, "DedEffectiveDateFrom")

        txtNumberOfPayPeriodLeft.DataBindings.Clear()
        txtNumberOfPayPeriodLeft.DataBindings.Add("Text", Me._currentLoan, "LoanPayPeriodLeft", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N0")

        txtDeductionAmount.DataBindings.Clear()
        txtDeductionAmount.DataBindings.Add("Text", Me._currentLoan, "DeductionAmount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        txtLoanInterestPercentage.DataBindings.Clear()
        txtLoanInterestPercentage.DataBindings.Add("Text", Me._currentLoan, "DeductionPercentage", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        cboLoanType.DataBindings.Clear()
        cboLoanType.DataBindings.Add("Text", Me._currentLoan, "LoanName")
        cboLoanType.Text = Me._currentLoan.LoanName
        UpdateLoanTypeID()

        txtLoanStatus.DataBindings.Clear()
        cmbLoanStatus.DataBindings.Clear()
        If unEditable Then

            txtLoanStatus.DataBindings.Add("Text", Me._currentLoan, "Status")
        Else
            cmbLoanStatus.DataBindings.Add("Text", Me._currentLoan, "Status")

        End If

        cmbDeductionSchedule.DataBindings.Clear()
        cmbDeductionSchedule.DataBindings.Add("Text", Me._currentLoan, "DeductionSchedule")

        ToggleLoanStatusComboboxVisibility(Not unEditable)

    End Sub

    Private Sub ResetForm()

        'employee details

        EmployeeNameTextBox.Text = String.Empty
        EmployeeNumberTextBox.Text = String.Empty

        EmployeePictureBox.Image = Nothing

        'loan grid view
        RemoveHandler LoanGridView.SelectionChanged, AddressOf LoanGridView_SelectionChanged

        LoanGridView.DataSource = Nothing

        LoanHistoryGridView.DataSource = Nothing

        AddHandler LoanGridView.SelectionChanged, AddressOf LoanGridView_SelectionChanged

        'loan details
        Me._currentLoan = Nothing

        DetailsTabLayout.Enabled = False

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

    Private Function GetSelectedLoan() As LoanSchedule
        Return CType(LoanGridView.CurrentRow.DataBoundItem, LoanSchedule)
    End Function

    Private Sub ForceLoanScheduleGridViewCommit()
        'Workaround. Focus other control to lose focus on current control
        EmployeeInfoTabLayout.Focus()
    End Sub

    Private Function CheckIfLoanIsChanged(newLoanSchedule As LoanSchedule) As Boolean

        Dim oldLoanSchedule =
            Me._changedLoans.
                FirstOrDefault(Function(l) Nullable.Equals(l.RowID, newLoanSchedule.RowID))

        If oldLoanSchedule Is Nothing Then Return False

        Dim hasChanged = False

        If _
            Nullable.Equals(newLoanSchedule.LoanTypeID, oldLoanSchedule.LoanTypeID) = False OrElse
            newLoanSchedule.LoanNumber <> oldLoanSchedule.LoanNumber OrElse
            newLoanSchedule.TotalLoanAmount <> oldLoanSchedule.TotalLoanAmount OrElse
            newLoanSchedule.TotalBalanceLeft <> oldLoanSchedule.TotalBalanceLeft OrElse
            newLoanSchedule.DedEffectiveDateFrom <> oldLoanSchedule.DedEffectiveDateFrom OrElse
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
            Me._changedLoans.
                FirstOrDefault(Function(l) Nullable.Equals(l.RowID, newLoanSchedule.RowID))

        If oldLoanSchedule Is Nothing Then Return False

        Dim changes = New List(Of UserActivityItem)

        Dim suffixIdentifier = $"of loan with type '{oldLoanSchedule.LoanName}' and start date '{oldLoanSchedule.DedEffectiveDateFrom.ToShortDateString()}'."

        If newLoanSchedule.LoanName <> oldLoanSchedule.LoanName Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldLoanSchedule.RowID.Value,
                .Description = $"Updated type from '{oldLoanSchedule.LoanName}' to '{newLoanSchedule.LoanName}' {suffixIdentifier}",
                .ChangedEmployeeId = oldLoanSchedule.EmployeeID.Value
            })
        End If
        If newLoanSchedule.LoanNumber <> oldLoanSchedule.LoanNumber Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldLoanSchedule.RowID.Value,
                .Description = $"Updated loan number from '{oldLoanSchedule.LoanNumber}' to '{newLoanSchedule.LoanNumber}' {suffixIdentifier}",
                .ChangedEmployeeId = oldLoanSchedule.EmployeeID.Value
            })
        End If
        If newLoanSchedule.TotalLoanAmount <> oldLoanSchedule.TotalLoanAmount Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldLoanSchedule.RowID.Value,
                .Description = $"Updated total amount from '{oldLoanSchedule.TotalLoanAmount}' to '{newLoanSchedule.TotalLoanAmount}' {suffixIdentifier}",
                .ChangedEmployeeId = oldLoanSchedule.EmployeeID.Value
            })
        End If
        If newLoanSchedule.DedEffectiveDateFrom <> oldLoanSchedule.DedEffectiveDateFrom Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldLoanSchedule.RowID.Value,
                .Description = $"Updated start date '{oldLoanSchedule.DedEffectiveDateFrom.ToShortDateString()}' to '{newLoanSchedule.DedEffectiveDateFrom.ToShortDateString()}' {suffixIdentifier}",
                .ChangedEmployeeId = oldLoanSchedule.EmployeeID.Value
            })
        End If
        If newLoanSchedule.DeductionAmount <> oldLoanSchedule.DeductionAmount Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldLoanSchedule.RowID.Value,
                .Description = $"Updated deduction amount from '{oldLoanSchedule.DeductionAmount}' to '{newLoanSchedule.DeductionAmount}' {suffixIdentifier}",
                .ChangedEmployeeId = oldLoanSchedule.EmployeeID.Value
            })
        End If
        If newLoanSchedule.Status <> oldLoanSchedule.Status Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldLoanSchedule.RowID.Value,
                .Description = $"Updated status from '{oldLoanSchedule.Status}' to '{newLoanSchedule.Status}' {suffixIdentifier}",
                .ChangedEmployeeId = oldLoanSchedule.EmployeeID.Value
            })
        End If
        If newLoanSchedule.DeductionPercentage <> oldLoanSchedule.DeductionPercentage Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldLoanSchedule.RowID.Value,
                .Description = $"Updated interest percentage from '{oldLoanSchedule.DeductionPercentage}' to '{newLoanSchedule.DeductionPercentage}' {suffixIdentifier}",
                .ChangedEmployeeId = oldLoanSchedule.EmployeeID.Value
            })
        End If
        If newLoanSchedule.DeductionSchedule <> oldLoanSchedule.DeductionSchedule Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldLoanSchedule.RowID.Value,
                .Description = $"Updated deduction schedule from '{oldLoanSchedule.DeductionSchedule}' to '{newLoanSchedule.DeductionSchedule}' {suffixIdentifier}",
                .ChangedEmployeeId = oldLoanSchedule.EmployeeID.Value
            })
        End If
        If newLoanSchedule.Comments <> oldLoanSchedule.Comments Then
            changes.Add(New UserActivityItem() With
            {
                .EntityId = oldLoanSchedule.RowID.Value,
                .Description = $"Updated comments from '{oldLoanSchedule.Comments}' to '{newLoanSchedule.Comments}' {suffixIdentifier}",
                .ChangedEmployeeId = oldLoanSchedule.EmployeeID.Value
            })
        End If

        If changes.Any() Then

            _userActivityRepository.CreateRecord(z_User, FormEntityName, z_OrganizationID, UserActivityRepository.RecordTypeEdit, changes)
        End If

        Return True
    End Function

    Private Async Function LoadEmployees() As Task

        Me._allEmployees = (Await _employeeRepository.GetAllWithPositionAsync(z_OrganizationID)).
            OrderBy(Function(e) e.LastName).
            ToList

    End Function

    Private Async Function ShowEmployeeList() As Task

        If ShowAllCheckBox.Checked Then

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

    Private Sub lnkBonusPayment_Click(sender As Object, e As EventArgs) Handles lnkBonusPayment.Click
        Dim from As New AssignBonusToLoanForm(GetSelectedLoan())
        If from.ShowDialog() = DialogResult.OK Then
            EmployeesDataGridView_SelectionChanged(EmployeesDataGridView, New EventArgs)
        End If
    End Sub

#End Region

End Class
