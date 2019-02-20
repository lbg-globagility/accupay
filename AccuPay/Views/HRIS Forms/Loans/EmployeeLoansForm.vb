Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Loans
Imports AccuPay.Repository
Imports AccuPay.Utils
Imports Microsoft.EntityFrameworkCore
Imports Simplified = AccuPay.SimplifiedEntities.GridView

Public Class EmployeeLoansForm

    Private _employeeRepository As EmployeeRepository
    Private _loanRepository As LoanRepository
    Private _productRepository As ProductRepository
    Private _listOfValueRepository As ListOfValueRepository
    Private _loanScheduleRepository As LoanScheduleRepository

    Private _employees As List(Of Simplified.Employee)
    Private _deductionScheduleList As IEnumerable(Of ListOfValue)
    Private _loanTypeList As IEnumerable(Of Product)

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _employeeRepository = New EmployeeRepository
        _loanRepository = New LoanRepository
        _productRepository = New ProductRepository
        _listOfValueRepository = New ListOfValueRepository
        _loanScheduleRepository = New LoanScheduleRepository

        _employees = New List(Of Simplified.Employee)
        _deductionScheduleList = New List(Of ListOfValue)
        _loanTypeList = New List(Of Product)
    End Sub

    Private Async Sub EmployeeLoansForm_Load(sender As Object, e As EventArgs) Handles Me.Load

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

        Await LoadLoanSchedules(currentEmployee)

    End Sub

    Private Sub loanSchedulesDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles loanSchedulesDataGridView.SelectionChanged
        ResetLoanScheduleForm()

        If loanSchedulesDataGridView.CurrentRow Is Nothing Then Return

        Dim currentLoanSchedule As LoanSchedule = GetSelectedLoanSchedule()

        Dim currentEmployee = GetSelectedEmployee()
        If currentLoanSchedule IsNot Nothing AndAlso currentEmployee IsNot Nothing AndAlso
           Nullable.Equals(currentLoanSchedule.EmployeeID, currentEmployee.RowID) Then
            PopulateLoanScheduleForm(currentLoanSchedule)
        End If

    End Sub

    Private Sub txtTotalLoanAmount_Leave(sender As Object, e As EventArgs) _
        Handles txtTotalLoanAmount.Leave, txtDeductionAmount.Leave

        'Dim totalLoanAmount = ObjectUtils.ToDecimal(txtTotalLoanAmount.Text)
        'Dim deductionAmount = ObjectUtils.ToDecimal(txtDeductionAmount.Text)

        'Dim numberOfPayPeriod As Integer

        'numberOfPayPeriod = CType(Math.Round(totalLoanAmount / deductionAmount, 0), Integer)

        'txtNumberOfPayPeriod.Text = numberOfPayPeriod.ToString()
    End Sub

    Private Async Sub tsbtnSaveLoan_Click(sender As Object, e As EventArgs) Handles tsbtnSaveLoan.Click
        Dim currentLoanSchedule As LoanSchedule = GetSelectedLoanSchedule()

        Using context As New PayrollContext
            Dim loan = Await context.LoanSchedules.Where(Function(l) Nullable.Equals(l.RowID, currentLoanSchedule.RowID)).
                                                    FirstOrDefaultAsync()

            Dim loanSchedule As New LoanSchedule With {
                .LoanNumber = txtLoanNumber.Text,
                .DedEffectiveDateFrom = dtpDateFrom.Value,
                .TotalLoanAmount = ObjectUtils.ToDecimal(txtTotalLoanAmount.Text),
                .DeductionSchedule = cmbDeductionSchedule.Text,
                .DeductionAmount = ObjectUtils.ToDecimal(txtDeductionAmount.Text),
                .Status = cmbLoanStatus.Text,
                .DeductionPercentage = ObjectUtils.ToDecimal(txtLoanInterestPercentage.Text),
                .NoOfPayPeriod = ObjectUtils.ToDecimal(txtNumberOfPayPeriod.Text),
                .LoanTypeID = ObjectUtils.ToNullableInteger(cboLoanType.SelectedValue)
            }

            'repo.Add(repo)
            'repo.Save()
        End Using
    End Sub

#Region "Private Functions"

    Private _loanSchedule As New LoanSchedule


    Private Sub InitializeComponentSettings()
        Using context As New PayrollContext
            _loanSchedule = context.LoanSchedules.FirstOrDefault
        End Using

        employeesDataGridView.AutoGenerateColumns = False
        loanSchedulesDataGridView.AutoGenerateColumns = False
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

    Private Async Function LoadLoanSchedules(currentEmployee As Simplified.Employee) As Threading.Tasks.Task
        If currentEmployee Is Nothing Then Return

        loanSchedulesDataGridView.DataSource =
            Await _loanRepository.GetByEmployee(currentEmployee.RowID)

    End Function

    Private Async Function LoadLoanTypes() As Threading.Tasks.Task

        Me._loanTypeList = Await _productRepository.GetLoanTypes()
        Dim loanTypes = _productRepository.ConvertToStringList(_loanTypeList)

        cboLoanType.DataSource = loanTypes

    End Function

    Private Async Function LoadDeductionSchedules() As Threading.Tasks.Task

        Me._deductionScheduleList = Await _listOfValueRepository.GetDeductionSchedules()
        Dim deductionSchedules = _listOfValueRepository.ConvertToStringList(Me._deductionScheduleList)

        cmbDeductionSchedule.DataSource = deductionSchedules
    End Function

    Private Function GetSelectedEmployee() As Simplified.Employee
        If employeesDataGridView.CurrentRow Is Nothing Then Return Nothing

        Return CType(employeesDataGridView.CurrentRow.DataBoundItem, Simplified.Employee)
    End Function


    Private Sub PopulateLoanScheduleForm(loanSchedule As LoanSchedule)
        Me._loanSchedule = loanSchedule

        txtLoanNumber.DataBindings.Add("Text", Me._loanSchedule, "LoanNumber", True, DataSourceUpdateMode.OnPropertyChanged)

        txtTotalLoanAmount.DataBindings.Add("Text", Me._loanSchedule, "TotalLoanAmount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "0.00")

        txtLoanBalance.DataBindings.Add("Text", Me._loanSchedule, "TotalBalanceLeft", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "0.00")

        dtpDateFrom.DataBindings.Add("Value", Me._loanSchedule, "DedEffectiveDateFrom")

        txtNumberOfPayPeriod.DataBindings.Add("Text", Me._loanSchedule, "NoOfPayPeriod", True, DataSourceUpdateMode.OnPropertyChanged)

        txtNumberOfPayPeriodLeft.DataBindings.Add("Text", Me._loanSchedule, "LoanPayPeriodLeft", True, DataSourceUpdateMode.OnPropertyChanged)

        txtDeductionAmount.DataBindings.Add("Text", Me._loanSchedule, "DeductionAmount", True, DataSourceUpdateMode.OnPropertyChanged)

        txtLoanInterestPercentage.DataBindings.Add("Text", Me._loanSchedule, "DeductionPercentage", True, DataSourceUpdateMode.OnPropertyChanged)

        cboLoanType.DataBindings.Add("Text", Me._loanSchedule, "LoanName")

        cmbLoanStatus.DataBindings.Add("Text", Me._loanSchedule, "Status")

        cmbDeductionSchedule.DataBindings.Add("Text", Me._loanSchedule, "DeductionSchedule")

        'Dim currentLoanType = _loanTypeList.
        '        FirstOrDefault(Function(l) Nullable.Equals(l.RowID, loanSchedule.LoanTypeID))

        'If currentLoanType IsNot Nothing Then
        '    cboLoanType.SelectedItem = currentLoanType
        'End If

        'txtLoanNumber.Text = loanSchedule.LoanNumber
        'txtTotalLoanAmount.Text = loanSchedule.TotalLoanAmount.ToString()
        'txtLoanBalance.Text = loanSchedule.TotalBalanceLeft.ToString()
        'dtpDateFrom.Value = loanSchedule.DedEffectiveDateFrom
        'txtNumberOfPayPeriod.Text = loanSchedule.NoOfPayPeriod.ToString()
        'txtNumberOfPayPeriodLeft.Text = loanSchedule.LoanPayPeriodLeft.ToString()
        'txtDeductionAmount.Text = loanSchedule.DeductionAmount.ToString()
        'cmbLoanStatus.SelectedItem = loanSchedule.Status
        'txtLoanInterestPercentage.Text = loanSchedule.DeductionPercentage.ToString()

        'Dim currentDeductionSchedule = _deductionScheduleList.
        '        FirstOrDefault(Function(l) Nullable.Equals(l.DisplayValue, loanSchedule.DeductionSchedule))

        'If currentDeductionSchedule IsNot Nothing Then
        '    cmbDeductionSchedule.SelectedItem = currentDeductionSchedule
        'End If

    End Sub

    Private Sub ResetLoanScheduleForm()

        Me._loanSchedule = Nothing

        txtLoanNumber.Clear()
        txtLoanNumber.DataBindings.Clear()

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

    Private Sub txtTotalLoanAmount_TextChanged(sender As Object, e As EventArgs) Handles txtTotalLoanAmount.TextChanged

    End Sub

#End Region

End Class