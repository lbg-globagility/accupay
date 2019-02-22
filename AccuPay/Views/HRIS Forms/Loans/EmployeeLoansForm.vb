Option Strict On

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

    Private _loanTypeList As IEnumerable(Of Product)

    Private _employees As New List(Of Simplified.Employee)
    Private _currentLoanSchedule As New LoanSchedule

    Private _currentloanSchedules As New List(Of LoanSchedule)

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
        txtEmployeeNumber.Text = currentEmployee.EmployeeID

        Await LoadLoanSchedules(currentEmployee)

    End Sub

    Private Async Sub loanSchedulesDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles loanSchedulesDataGridView.SelectionChanged
        ResetLoanScheduleForm()

        DetailsTabControl.Enabled = False

        If loanSchedulesDataGridView.CurrentRow Is Nothing Then Return

        Dim currentLoanSchedule As LoanSchedule = GetSelectedLoanSchedule()

        Dim currentEmployee = GetSelectedEmployee()
        If currentLoanSchedule IsNot Nothing AndAlso currentEmployee IsNot Nothing AndAlso
           Nullable.Equals(currentLoanSchedule.EmployeeID, currentEmployee.RowID) Then
            PopulateLoanScheduleForm(currentLoanSchedule)

            Await LoadLoanTransactions(currentLoanSchedule)

            DetailsTabControl.Enabled = True
        End If

    End Sub

    Private Sub txtTotalLoanAmount_Leave(sender As Object, e As EventArgs) _
        Handles txtTotalLoanAmount.Leave, txtDeductionAmount.Leave

        Dim totalLoanAmount = AccuMath.CommercialRound(Me._currentLoanSchedule.TotalLoanAmount)
        Dim deductionAmount = AccuMath.CommercialRound(Me._currentLoanSchedule.DeductionAmount)
        Dim totalBalanceLeft = AccuMath.CommercialRound(Me._currentLoanSchedule.TotalBalanceLeft)

        Dim numberOfPayPeriod, numberOfPayPeriodLeft As Integer

        If deductionAmount = 0 Then
            numberOfPayPeriod = 0
            numberOfPayPeriodLeft = 0
        Else
            numberOfPayPeriod = _loanScheduleRepository.ComputeNumberOfPayPeriod(totalLoanAmount, deductionAmount)
            numberOfPayPeriodLeft = _loanScheduleRepository.ComputeNumberOfPayPeriodLeft(totalBalanceLeft, deductionAmount)
        End If

        Me._currentLoanSchedule.NoOfPayPeriod = numberOfPayPeriod
        Me._currentLoanSchedule.LoanPayPeriodLeft = numberOfPayPeriodLeft
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

            MessageBoxHelper.DefaultErrorMessage(messageTitle)

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

        ForceLoanScheduleGridViewCommit()
    End Sub

    Private Async Sub tsbtnNewLoan_Click(sender As Object, e As EventArgs) Handles tsbtnNewLoan.Click

        Dim employee As Simplified.Employee = GetSelectedEmployee()

        If employee Is Nothing Then
            MessageBox.Show("No employee selected!")
        End If

        Dim form As New AddLoanScheduleForm(employee)
        form.ShowDialog()

        If form.IsSaved Then
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
        cmbLoanStatus.DataSource = _loanScheduleRepository.GetStatusList()
    End Sub

    Private Async Function LoadEmployees(Optional searchValue As String = "") As Threading.Tasks.Task
        Dim filteredEmployees As New List(Of Simplified.Employee)

        If String.IsNullOrEmpty(searchValue) Then
            employeesDataGridView.DataSource = Me._employees
        Else
            employeesDataGridView.DataSource =
                Await _employeeRepository.SearchSimpleLocal(Me._employees, searchValue)
        End If
    End Function

    Dim _unchangedLoanSchedules As New List(Of LoanSchedule)

    Private Async Function LoadLoanSchedules(currentEmployee As Simplified.Employee) As Threading.Tasks.Task
        If currentEmployee Is Nothing Then Return

        Dim loanSchedules = Await _loanScheduleRepository.GetByEmployeeAsync(currentEmployee.RowID)

        Me._currentloanSchedules = loanSchedules.ToList

        Me._unchangedLoanSchedules.Clear()

        For Each loanSchedule In Me._currentloanSchedules
            Me._unchangedLoanSchedules.Add(loanSchedule.CloneJson())
        Next

        LoanScheduleBindingSource.DataSource = Me._currentloanSchedules

        loanSchedulesDataGridView.DataSource = LoanScheduleBindingSource

    End Function

    Private Async Function LoadLoanTransactions(currentLoanSchedule As LoanSchedule) As Threading.Tasks.Task

        loanHistoryGridView.DataSource =
                Await _loanScheduleRepository.GetLoanTransactionsWithPayPeriod(currentLoanSchedule.RowID)
    End Function

    Private Async Function LoadLoanTypes() As Threading.Tasks.Task

        Me._loanTypeList = Await _productRepository.GetLoanTypes()
        Dim loanTypes = _productRepository.ConvertToStringList(_loanTypeList)

        cboLoanType.DataSource = loanTypes

    End Function

    Private Async Function LoadDeductionSchedules() As Threading.Tasks.Task

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

        txtLoanNumber.DataBindings.Add("Text", Me._currentLoanSchedule, "LoanNumber")

        txtRemarks.DataBindings.Add("Text", Me._currentLoanSchedule, "Comments")

        txtTotalLoanAmount.DataBindings.Add("Text", Me._currentLoanSchedule, "TotalLoanAmount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        txtLoanBalance.DataBindings.Add("Text", Me._currentLoanSchedule, "TotalBalanceLeft", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        dtpDateFrom.DataBindings.Add("Value", Me._currentLoanSchedule, "DedEffectiveDateFrom")

        txtNumberOfPayPeriod.DataBindings.Add("Text", Me._currentLoanSchedule, "NoOfPayPeriod", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N0")

        txtNumberOfPayPeriodLeft.DataBindings.Add("Text", Me._currentLoanSchedule, "LoanPayPeriodLeft", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N0")

        txtDeductionAmount.DataBindings.Add("Text", Me._currentLoanSchedule, "DeductionAmount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        txtLoanInterestPercentage.DataBindings.Add("Text", Me._currentLoanSchedule, "DeductionPercentage", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        cboLoanType.DataBindings.Add("Text", Me._currentLoanSchedule, "LoanName")

        cmbLoanStatus.DataBindings.Add("Text", Me._currentLoanSchedule, "Status")

        cmbDeductionSchedule.DataBindings.Add("Text", Me._currentLoanSchedule, "DeductionSchedule")

    End Sub

    Private Sub ResetLoanScheduleForm()

        Me._currentLoanSchedule = Nothing

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

        cmbLoanStatus.SelectedIndex = -1
        cmbLoanStatus.DataBindings.Clear()


        cmbDeductionSchedule.SelectedIndex = -1
        cmbDeductionSchedule.DataBindings.Clear()

    End Sub

    Private Function GetSelectedLoanSchedule() As LoanSchedule
        Return CType(loanSchedulesDataGridView.CurrentRow.DataBoundItem, LoanSchedule)
    End Function

    Private Sub ForceLoanScheduleGridViewCommit()
        'Workaround. Focus other control to lose focus on current control
        pbEmpPicLoan.Focus()
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