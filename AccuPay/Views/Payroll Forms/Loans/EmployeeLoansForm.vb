Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Enums
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class EmployeeLoansForm

    Private _currentLoan As LoanModel

    Private _employees As List(Of Employee)

    Private _allEmployees As List(Of Employee)

    Private _currentloans As List(Of LoanModel)

    Private _changedLoans As List(Of LoanModel)

    Private _currentLoanTransactions As List(Of LoanTransaction)

    Private ReadOnly _employeeRepository As IEmployeeRepository

    Private ReadOnly _textBoxDelayedAction As DelayedAction(Of Boolean)

    Private Const LOAN_HISTORY_TAB_TEXT As String = "Loan History"

    Private _currentRolePermission As RolePermission

    Private ReadOnly _policyHelper As IPolicyHelper
    Private ReadOnly _loanTypeGrouping As LoanTypeGroupingEnum

    Sub New(loanTypeGrouping As LoanTypeGroupingEnum)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        _employees = New List(Of Employee)

        _allEmployees = New List(Of Employee)

        _currentloans = New List(Of LoanModel)

        _changedLoans = New List(Of LoanModel)

        _currentLoanTransactions = New List(Of LoanTransaction)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

        _textBoxDelayedAction = New DelayedAction(Of Boolean)

        _policyHelper = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

        _loanTypeGrouping = loanTypeGrouping

        LoanUserControl1.LoanTypeGrouping = loanTypeGrouping

        If loanTypeGrouping = LoanTypeGroupingEnum.Government Then
            lblFormTitle.Text = "Government Premium"
        End If
    End Sub

    Sub New()

        InitializeComponent()

        _employees = New List(Of Employee)

        _allEmployees = New List(Of Employee)

        _currentloans = New List(Of LoanModel)

        _changedLoans = New List(Of LoanModel)

        _currentLoanTransactions = New List(Of LoanTransaction)

        _employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

        _textBoxDelayedAction = New DelayedAction(Of Boolean)

        _policyHelper = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

        _loanTypeGrouping = LoanTypeGroupingEnum.All
    End Sub

    Private Async Sub EmployeeLoansForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        InitializeComponentSettings()

        Await CheckRolePermissions()

        Await LoadEmployees()
        Await ShowEmployeeList()

        AddHandler SearchTextBox.TextChanged, AddressOf SearchTextBox_TextChanged

        lnkBonusPayment.Visible = _policyHelper.UseLoanDeductFromBonus
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

        Dim currentLoan As LoanModel = GetSelectedLoan()

        Dim currentEmployee = GetSelectedEmployee()
        If currentLoan IsNot Nothing AndAlso currentEmployee IsNot Nothing AndAlso
           Nullable.Equals(currentLoan.EmployeeId, currentEmployee.RowID) Then

            Await UpdateSelectedLoanData()

            tbpHistory.Text = $"{LOAN_HISTORY_TAB_TEXT} ({Me._currentLoanTransactions.Count})"

            DetailsTabControl.Enabled = True
        End If

    End Function

    Private Async Sub SaveToolStripButton_Click(sender As Object, e As EventArgs) Handles SaveToolStripButton.Click
        ForceLoanGridViewCommit()

        Dim changedLoans As New List(Of Loan)

        Dim messageTitle = "Update Loans"

        For Each item In Me._currentloans

            If CheckIfLoanIsChanged(item) Then
                changedLoans.Add(item.CreateLoan())
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
                Dim dataService = MainServiceProvider.GetRequiredService(Of ILoanDataService)
                Await dataService.SaveManyAsync(changedLoans, z_User)

                ShowBalloonInfo($"{changedLoans.Count} Loan(s) Successfully Updated.", messageTitle)

                Dim currentEmployee = GetSelectedEmployee()

                If currentEmployee Is Nothing Then Return

                Await LoadLoans(currentEmployee)
            End Function)
    End Sub

    Private Async Sub NewToolStripButton_Click(sender As Object, e As EventArgs) Handles NewToolStripButton.Click

        Dim employee As Employee = GetSelectedEmployee()

        If employee Is Nothing Then
            MessageBoxHelper.Warning("No employee selected!")

            Return
        End If

        Dim form As New AddLoanForm(employee, loanTypeGrouping:=_loanTypeGrouping)
        form.ShowDialog()

        If form.IsSaved Then

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

        Me._currentloans = Me._changedLoans.
            Select(Function(l) l.Clone()).
            ToList()

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
            Me._currentLoan.Id Is Nothing Then
            MessageBoxHelper.Warning("No loan selected!")

            Return
        End If

        Dim repository = MainServiceProvider.GetRequiredService(Of ILoanRepository)
        Dim currentLoan = Await repository.GetByIdAsync(Me._currentLoan.Id.Value)

        If currentLoan Is Nothing Then

            MessageBoxHelper.Warning("Loan not found in database! Please close this form the open again.")

            Return
        End If

        Dim loanNumber = Me._currentLoan.LoanNumber

        Dim loanNumberString = If(String.IsNullOrWhiteSpace(loanNumber), "", $": {loanNumber}")

        If MessageBoxHelper.Confirm(Of Boolean) _
        ($"Are you sure you want to delete loan{loanNumberString}?", "Confirm Deletion") = False Then

            Return
        End If

        If currentLoan.Status = Loan.STATUS_COMPLETE Then

            MessageBoxHelper.Warning("Cannot delete a completed loan.")

            Return
        Else

            Dim loanTransactions = Await repository.
                GetLoanTransactionsWithPayPeriodAsync(Me._currentLoan.Id.Value)

            If loanTransactions.Count > 0 Then

                MessageBoxHelper.Warning("This loan has already started and therefore cannot be deleted. Try changing its status to ""On Hold"" or ""Cancelled"" instead.")

                Return
            End If

        End If

        Await DeleteLoan(currentEmployee, messageTitle, loanNumberString)

    End Sub

    Private Async Function DeleteLoan(
        currentEmployee As Employee,
        messageTitle As String,
        loanNumberString As String) As Task

        If Me._currentLoan Is Nothing OrElse
            Me._currentLoan.Id Is Nothing Then
            MessageBoxHelper.Warning("No loan selected!")

            Return
        End If

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Dim dataService = MainServiceProvider.GetRequiredService(Of ILoanDataService)
                Await dataService.DeleteAsync(
                    id:=Me._currentLoan.Id.Value,
                    currentlyLoggedInUserId:=z_User)

                Await LoadLoans(currentEmployee)

                ShowBalloonInfo($"Loan{If(String.IsNullOrWhiteSpace(loanNumberString), "", loanNumberString)} Successfully Deleted.", messageTitle)
            End Function)
    End Function

    Private Async Sub ImportToolStripButton_Click(sender As Object, e As EventArgs) Handles ImportToolStripButton.Click
        If _loanTypeGrouping = LoanTypeGroupingEnum.Government Then
            Using form = New ImportGovernmentPremiumForm()
                form.ShowDialog()

                If form.IsSaved Then

                    Dim currentEmployee = GetSelectedEmployee()

                    If currentEmployee IsNot Nothing Then
                        Await LoadLoans(currentEmployee)
                    End If

                    ShowBalloonInfo("Government premium successfully imported", "Import Government Premium")

                End If

            End Using

            Return
        End If

        Using form = New ImportLoansForm()
            form.ShowDialog()

            If form.IsSaved Then

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

    Private Async Function FilterEmployees(Optional searchValue As String = "") As Task

        RemoveHandler EmployeesDataGridView.SelectionChanged, AddressOf EmployeesDataGridView_SelectionChanged

        chkInProgressFilter.Text = Loan.STATUS_IN_PROGRESS
        chkOnHoldFilter.Text = Loan.STATUS_ON_HOLD
        chkCancelledFilter.Text = Loan.STATUS_CANCELLED
        chkCompleteFilter.Text = Loan.STATUS_COMPLETE

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

        Dim repository = MainServiceProvider.GetRequiredService(Of ILoanRepository)
        Dim loans = Await repository.GetByEmployeeAsync(currentEmployee.RowID.Value, _loanTypeGrouping)

        Dim loanModels = loans.
            Select(Function(l) LoanModel.Create(l)).
            OrderByDescending(Function(l) l.EffectiveFrom).
            ThenByDescending(Function(l) l.LoanTypeName).
            ToList()

        Dim statusFilter = CreateStatusFilter()

        If statusFilter Is Nothing Then
            Me._currentloans = New List(Of LoanModel)
        Else
            Me._currentloans = loanModels.Where(statusFilter).ToList()

        End If

        Me._changedLoans = Me._currentloans.
            Select(Function(l) l.Clone()).
            ToList()

        chkInProgressFilter.Text = $"{Loan.STATUS_IN_PROGRESS} ({loans.Where(Function(l) l.Status = Loan.STATUS_IN_PROGRESS).Count()})"
        chkOnHoldFilter.Text = $"{Loan.STATUS_ON_HOLD} ({loans.Where(Function(l) l.Status = Loan.STATUS_ON_HOLD).Count()})"
        chkCancelledFilter.Text = $"{Loan.STATUS_CANCELLED} ({loans.Where(Function(l) l.Status = Loan.STATUS_CANCELLED).Count()})"
        chkCompleteFilter.Text = $"{Loan.STATUS_COMPLETE} ({loans.Where(Function(l) l.Status = Loan.STATUS_COMPLETE).Count()})"

        Await PopulateLoanGridView()

    End Function

    Private Function CreateStatusFilter() As Func(Of LoanModel, Boolean)

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
            Function(loan As LoanModel) As Boolean

                Return (inProgressChecked AndAlso loan.Status = Core.Entities.Loan.STATUS_IN_PROGRESS) OrElse
                    (onHoldChecked AndAlso loan.Status = Core.Entities.Loan.STATUS_ON_HOLD) OrElse
                    (cancelledChecked AndAlso loan.Status = Core.Entities.Loan.STATUS_CANCELLED) OrElse
                    (completeChecked AndAlso loan.Status = Core.Entities.Loan.STATUS_COMPLETE)

            End Function

        Return whereFunction
    End Function

    Private Async Function UpdateSelectedLoanData() As Task

        Dim currentLoan As LoanModel = GetSelectedLoan()
        If currentLoan Is Nothing Then Return

        Dim repository = MainServiceProvider.GetRequiredService(Of ILoanRepository)
        Me._currentLoanTransactions = New List(Of LoanTransaction) _
            (Await repository.GetLoanTransactionsWithPayPeriodAsync(currentLoan.Id.Value))

        LoanHistoryGridView.DataSource = Me._currentLoanTransactions

        Dim LoanHistoryCount As Integer = LoanHistoryGridView.Rows.Count

        If LoanHistoryCount > 0 Then

            LoanHistoryGridView.CurrentCell = LoanHistoryGridView.Rows(LoanHistoryCount - 1).Cells(0)

        End If

        PopulateLoanForm(currentLoan)

    End Function

    Private Function GetSelectedEmployee() As Employee
        If EmployeesDataGridView.CurrentRow Is Nothing Then Return Nothing

        Return CType(EmployeesDataGridView.CurrentRow.DataBoundItem, Employee)
    End Function

    Private Sub PopulateLoanForm(loan As LoanModel)
        Me._currentLoan = loan

        Dim originalLoan = Me._changedLoans.
            FirstOrDefault(Function(l) Nullable.Equals(l.Id, Me._currentLoan.Id))

        If _currentRolePermission.Update Then

            DetailsTabLayout.Enabled = Me._currentLoan IsNot Nothing AndAlso Not Me._currentLoan.IsUnEditable
        End If

        LoanUserControl1.SetLoan(Me._currentLoan, isNew:=False)
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

        tbpHistory.Text = LOAN_HISTORY_TAB_TEXT

        LoanUserControl1.SetLoan(Me._currentLoan, isNew:=False)

    End Sub

    Private Function GetSelectedLoan() As LoanModel
        Return CType(LoanGridView.CurrentRow.DataBoundItem, LoanModel)
    End Function

    Private Sub ForceLoanGridViewCommit()
        'Workaround. Focus other control to lose focus on current control
        EmployeeInfoTabLayout.Focus()
    End Sub

    Private Function CheckIfLoanIsChanged(newLoan As LoanModel) As Boolean

        Dim oldLoan =
            Me._changedLoans.
                FirstOrDefault(Function(l) Nullable.Equals(l.Id, newLoan.Id))

        If oldLoan Is Nothing Then Return False

        Dim hasChanged = False

        If _
            Nullable.Equals(newLoan.LoanType?.RowID, oldLoan.LoanType?.RowID) = False OrElse
            newLoan.LoanNumber <> oldLoan.LoanNumber OrElse
            newLoan.TotalLoanAmount <> oldLoan.TotalLoanAmount OrElse
            newLoan.TotalBalanceLeft <> oldLoan.TotalBalanceLeft OrElse
            newLoan.EffectiveFrom <> oldLoan.EffectiveFrom OrElse
            newLoan.DeductionAmount <> oldLoan.DeductionAmount OrElse
            newLoan.Status <> oldLoan.Status OrElse
            newLoan.InterestPercentage <> oldLoan.InterestPercentage OrElse
            newLoan.Comments <> oldLoan.Comments OrElse
            newLoan.DeductionSchedule <> oldLoan.DeductionSchedule Then

            hasChanged = True
        End If

        Return hasChanged

    End Function

    Private Async Function LoadEmployees() As Task

        Me._allEmployees = (Await _employeeRepository.GetAllByOrganizationWithPositionAsync(z_OrganizationID)).
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
        Dim formEntityName As String = "Loan"
        Dim userActivity As New UserActivityForm(formEntityName)
        userActivity.ShowDialog()
    End Sub

    Private Async Sub lnkBonusPayment_Click(sender As Object, e As EventArgs) Handles lnkBonusPayment.Click
        Dim repository = MainServiceProvider.GetRequiredService(Of ILoanRepository)

        Dim currentLoan = GetSelectedLoan().CreateLoan()

        Dim loan = Await repository.GetByIdAsync(currentLoan.RowID.Value)

        Dim loanTransactions = Await repository.GetLoanTransactionsWithPayPeriodAsync(currentLoan.RowID.Value)

        Dim from As New AssignBonusToLoanForm(loan, loanTransactions:=loanTransactions)
        If from.ShowDialog() = DialogResult.OK Then
            EmployeesDataGridView_SelectionChanged(EmployeesDataGridView, New EventArgs)
        End If
    End Sub

    Private Sub AdvValeToolStripButton_Click(sender As Object, e As EventArgs) Handles AdvValeToolStripButton.Click
        Dim form = New AdvanceValeForm
        If Not form.ShowDialog() = DialogResult.OK Then Return

    End Sub

#End Region

End Class
