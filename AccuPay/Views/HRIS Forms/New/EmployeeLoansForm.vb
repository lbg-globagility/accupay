Option Strict On
Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports AccuPay.Loans
Imports AccuPay.Repository
Imports AccuPay.Utils
Imports Simplified = AccuPay.SimplifiedEntities.GridView

Public Class EmployeeLoansForm

    Private _employeeRepository As New EmployeeRepository
    Private _productRepository As New ProductRepository
    Private _listOfValueRepository As New ListOfValueRepository
    Private _loanScheduleRepository As New LoanScheduleRepository

    Private _loanTypeList As List(Of Product)

    Private _employees As New List(Of Simplified.Employee)
    Private _currentLoanSchedule As New LoanSchedule

    Private _currentloanSchedules As New List(Of LoanSchedule)

    Private _unchangedLoanSchedules As New List(Of LoanSchedule)

    Private _currentLoanTransactions As New List(Of LoanTransaction)


    Private str_ms_excel_file_extension As String =
        String.Concat("Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|",
                      "Microsoft Excel Documents 97-2003 (*.xls)|*.xls")

    Private threadArrayList As New List(Of Thread)

    Private Async Sub EmployeeLoansForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        DetailsTabControl.Enabled = False

        InitializeComponentSettings()

        Dim list = Await _employeeRepository.GetAll(Of Simplified.Employee)()

        Me._employees = CType(list, List(Of Simplified.Employee))

        ResetLoanScheduleForm()

        LoadLoanStatus()
        Await LoadLoanTypes()
        Await LoadDeductionSchedules()
        Await LoadEmployees()

    End Sub

    Private Async Sub searchTextBox_TextChanged(sender As Object, e As EventArgs) Handles searchTextBox.TextChanged
        Dim searchValue = searchTextBox.Text.ToLower()

        Await LoadEmployees(searchValue)

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Async Sub employeesDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles employeesDataGridView.SelectionChanged
        ResetLoanScheduleForm()

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then Return

        txtEmployeeFirstName.Text = currentEmployee.FullNameWithMiddleNameInitial
        txtEmployeeNumber.Text = currentEmployee.EmployeeNo

        pbEmployeePicture.Image = ConvByteToImage(currentEmployee.Image)

        Await LoadLoanSchedules(currentEmployee)

    End Sub

    Private Async Sub loanSchedulesDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles loanSchedulesDataGridView.SelectionChanged
        ResetLoanScheduleForm()

        DetailsTabControl.Enabled = False

        Dim loanHistoryTabText = "Loan History"

        tbpHistory.Text = loanHistoryTabText

        If loanSchedulesDataGridView.CurrentRow Is Nothing Then Return

        Dim currentLoanSchedule As LoanSchedule = GetSelectedLoanSchedule()

        Dim currentEmployee = GetSelectedEmployee()
        If currentLoanSchedule IsNot Nothing AndAlso currentEmployee IsNot Nothing AndAlso
           Nullable.Equals(currentLoanSchedule.EmployeeID, currentEmployee.RowID) Then

            Await LoadLoanTransactions(currentLoanSchedule)

            PopulateLoanScheduleForm(currentLoanSchedule)

            tbpHistory.Text = $"{loanHistoryTabText} ({Me._currentLoanTransactions.Count})"

            DetailsTabControl.Enabled = True
        End If



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
                changedLoanSchedules.Add(loanSchedule)
            End If

        Next

        If changedLoanSchedules.Count < 1 Then

            MessageBoxHelper.ErrorMessage("No unchanged loans!", messageTitle)
            Return
        End If

        Try

            Await _loanScheduleRepository.SaveManyAsync(changedLoanSchedules, Me._loanTypeList)

            ShowBalloonInfo($"{changedLoanSchedules.Count} Loan(s) Successfully Updated.", messageTitle)

            Dim currentEmployee = GetSelectedEmployee()

            If currentEmployee Is Nothing Then Return

            Await LoadLoanSchedules(currentEmployee)

        Catch ex As ArgumentException

            Dim errorMessage = "One of the updated loans has an error:" & Environment.NewLine & ex.Message

            MessageBoxHelper.ErrorMessage(errorMessage, messageTitle)

        Catch ex As Exception

            MessageBoxHelper.DefaultErrorMessage(messageTitle, ex)

        End Try
    End Sub

    Private Sub combobox_SelectedValueChanged(sender As Object, e As EventArgs) _
        Handles cmbLoanStatus.SelectedValueChanged, cmbDeductionSchedule.SelectedValueChanged,
                cboLoanType.SelectedValueChanged


        If sender Is cboLoanType AndAlso Me._currentLoanSchedule IsNot Nothing Then
            Dim selectedLoanType = Me._loanTypeList.FirstOrDefault(Function(l) l.PartNo = cboLoanType.Text)

            If selectedLoanType Is Nothing Then

                Me._currentLoanSchedule.LoanTypeID = Nothing

            Else

                Me._currentLoanSchedule.LoanTypeID = selectedLoanType.RowID

            End If
        End If

    End Sub

    Private Async Sub tsbtnNewLoan_Click(sender As Object, e As EventArgs) Handles tsbtnNewLoan.Click

        Dim employee As Simplified.Employee = GetSelectedEmployee()

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

        Me._currentloanSchedules = Me._unchangedLoanSchedules.CloneListJson()

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

        Dim currentLoanSchedule = Await _loanScheduleRepository.GetByIdAsync(Me._currentLoanSchedule.RowID)

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
        ("Loan schedule is already completed. Deleting this might affect previous cutoffs. Do you want to proceed deletion?", "Confirm Deletion") = False Then

                Return
            End If

        Else

            Dim loanTransactions = Await _loanScheduleRepository.
                GetLoanTransactionsWithPayPeriod(Me._currentLoanSchedule.RowID)

            If loanTransactions.Count > 0 Then

                If MessageBoxHelper.Confirm(Of Boolean) _
        ("This loan has already started. Deleting this might affect previous cutoffs. Do you want to proceed deletion?", "Confirm Deletion") = False Then

                    Return
                End If

            End If
        End If

        Try

            Await _loanScheduleRepository.DeleteAsync(Me._currentLoanSchedule.RowID)

            Await LoadLoanSchedules(currentEmployee)

            ShowBalloonInfo($"Loan{If(String.IsNullOrWhiteSpace(loanNumberString), " ", loanNumberString)}Successfully Deleted.", messageTitle)

        Catch ex As ArgumentException

            MessageBoxHelper.ErrorMessage(ex.Message, messageTitle)

        Catch ex As Exception

            MessageBoxHelper.DefaultErrorMessage(messageTitle, ex)

        End Try

    End Sub

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

        HRISForm.listHRISForm.Remove(Me.Name)

    End Sub

    Private Async Sub checkboxFilter_CheckedChanged(sender As Object, e As EventArgs) Handles chkOnHoldFilter.CheckedChanged, chkInProgressFilter.CheckedChanged, chkCompleteFilter.CheckedChanged, chkCancelledFilter.CheckedChanged

        Dim currentEmployee = GetSelectedEmployee()

        If currentEmployee Is Nothing Then Return

        Await LoadLoanSchedules(currentEmployee)
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

    Private Async Function LoadEmployees(Optional searchValue As String = "") As Task
        Dim filteredEmployees As New List(Of Simplified.Employee)

        If String.IsNullOrEmpty(searchValue) Then
            employeesDataGridView.DataSource = Me._employees
        Else
            employeesDataGridView.DataSource =
                Await _employeeRepository.SearchSimpleLocal(Me._employees, searchValue)
        End If
    End Function

    Private Async Function LoadLoanSchedules(currentEmployee As Simplified.Employee) As Task
        If currentEmployee Is Nothing Then Return

        Dim inProgressChecked = chkInProgressFilter.Checked
        Dim onHoldChecked = chkOnHoldFilter.Checked
        Dim cancelledChecked = chkCancelledFilter.Checked
        Dim completeChecked = chkCompleteFilter.Checked


        Dim loanSchedules = Await _loanScheduleRepository.GetByEmployeeAsync(currentEmployee.RowID)

        Dim statusFilter = CreateStatusFilter()

        Me._currentloanSchedules = loanSchedules.Where(statusFilter).ToList


        chkInProgressFilter.Text = $"In Progress ({loanSchedules.Count(Function(l) l.Status = LoanScheduleRepository.STATUS_IN_PROGRESS)})"
        chkOnHoldFilter.Text = $"On Hold ({loanSchedules.Count(Function(l) l.Status = LoanScheduleRepository.STATUS_ON_HOLD)})"
        chkCancelledFilter.Text = $"Cancelled ({loanSchedules.Count(Function(l) l.Status = LoanScheduleRepository.STATUS_CANCELLED)})"
        chkCompleteFilter.Text = $"Complete ({loanSchedules.Count(Function(l) l.Status = LoanScheduleRepository.STATUS_COMPLETE)})"

        Me._unchangedLoanSchedules = Me._currentloanSchedules.CloneListJson()

        LoanScheduleBindingSource.DataSource = Me._currentloanSchedules

        loanSchedulesDataGridView.DataSource = LoanScheduleBindingSource

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

    Private Async Function LoadLoanTransactions(currentLoanSchedule As LoanSchedule) As Task

        Me._currentLoanTransactions = New List(Of LoanTransaction) _
            (Await _loanScheduleRepository.GetLoanTransactionsWithPayPeriod(currentLoanSchedule.RowID))

        loanHistoryGridView.DataSource = Me._currentLoanTransactions

    End Function

    Private Async Function LoadLoanTypes() As Task

        Me._loanTypeList = New List(Of Product)(Await _productRepository.GetLoanTypes())
        PopulateLoanTypeCombobox()

    End Function

    Private Sub PopulateLoanTypeCombobox()
        Dim loanTypes = _productRepository.ConvertToStringList(Me._loanTypeList)

        cboLoanType.DataSource = loanTypes
    End Sub

    Private Async Function LoadDeductionSchedules() As Task

        Dim deductionSchedules = _listOfValueRepository.
                    ConvertToStringList(Await _listOfValueRepository.GetDeductionSchedules())

        cmbDeductionSchedule.DataSource = deductionSchedules
    End Function

    Private Function GetSelectedEmployee() As Simplified.Employee
        If employeesDataGridView.CurrentRow Is Nothing Then Return Nothing

        Return CType(employeesDataGridView.CurrentRow.DataBoundItem, Simplified.Employee)
    End Function

    Private Sub PopulateLoanScheduleForm(loanSchedule As LoanSchedule)
        Me._currentLoanSchedule = loanSchedule

        Dim originalLoanSchedule = Me._unchangedLoanSchedules.
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

        Me._currentLoanTransactions.CLear()

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
            Me._unchangedLoanSchedules.
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

#End Region

End Class