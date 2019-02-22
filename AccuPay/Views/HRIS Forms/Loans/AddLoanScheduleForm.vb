Imports AccuPay.Entity
Imports AccuPay.Loans
Imports AccuPay.Repository
Imports Simplified = AccuPay.SimplifiedEntities.GridView

Public Class AddLoanScheduleForm

    Private _currentEmployee As Simplified.Employee

    Private _newLoanSchedule As New LoanSchedule

    Private _productRepository As New ProductRepository

    Private _listOfValueRepository As New ListOfValueRepository

    Private _loanScheduleRepository As New LoanScheduleRepository

    Private _loanTypeList As IEnumerable(Of Product)

    Public Property IsSaved As Boolean

    Public Property ShowBalloonSuccess As Boolean

    Sub New(employee As Simplified.Employee)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _currentEmployee = employee

        Me.IsSaved = False

    End Sub

    Private Async Sub AddLoanScheduleForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        PopulateEmployeeData()

        LoadLoanStatus()

        Await LoadLoanTypes()

        Await LoadDeductionSchedules()

        Me._newLoanSchedule = New LoanSchedule
        Me._newLoanSchedule.EmployeeID = _currentEmployee.RowID

        CreateDataBindings()

    End Sub

    Private Sub PopulateEmployeeData()

        txtEmployeeFirstName.Text = _currentEmployee.FullNameWithMiddleNameInitial

        txtEmployeeNumber.Text = _currentEmployee.EmployeeID

    End Sub

    Private Sub CreateDataBindings()

        txtLoanNumber.DataBindings.Add("Text", Me._newLoanSchedule, "LoanNumber")

        txtRemarks.DataBindings.Add("Text", Me._newLoanSchedule, "Comments")

        txtTotalLoanAmount.DataBindings.Add("Text", Me._newLoanSchedule, "TotalLoanAmount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        txtLoanBalance.DataBindings.Add("Text", Me._newLoanSchedule, "TotalBalanceLeft", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        dtpDateFrom.DataBindings.Add("Value", Me._newLoanSchedule, "DedEffectiveDateFrom", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        txtNumberOfPayPeriod.DataBindings.Add("Text", Me._newLoanSchedule, "NoOfPayPeriod", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N0")

        txtNumberOfPayPeriodLeft.DataBindings.Add("Text", Me._newLoanSchedule, "LoanPayPeriodLeft", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N0")

        txtDeductionAmount.DataBindings.Add("Text", Me._newLoanSchedule, "DeductionAmount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        txtLoanInterestPercentage.DataBindings.Add("Text", Me._newLoanSchedule, "DeductionPercentage", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        cboLoanType.DataBindings.Add("Text", Me._newLoanSchedule, "LoanName")

        cmbLoanStatus.DataBindings.Add("Text", Me._newLoanSchedule, "Status")

        cmbDeductionSchedule.DataBindings.Add("Text", Me._newLoanSchedule, "DeductionSchedule")
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub cboLoanType_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboLoanType.SelectedValueChanged
        If sender Is cboLoanType AndAlso Me._newLoanSchedule IsNot Nothing Then
            Dim selectedLoanType = Me._loanTypeList.FirstOrDefault(Function(l) l.PartNo = cboLoanType.Text)

            If selectedLoanType Is Nothing Then

                Me._newLoanSchedule.LoanTypeID = Nothing

            Else

                Me._newLoanSchedule.LoanTypeID = selectedLoanType.RowID

            End If
        End If
    End Sub

    Private Async Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ForceLoanScheduleGridViewCommit()

        Await _loanScheduleRepository.SaveAsync(Me._newLoanSchedule, Me._loanTypeList)

        Me.IsSaved = True

        Me.ShowBalloonSuccess = True

        Me.Close()
    End Sub

    Private Sub txtTotalLoanAmount_Leave(sender As Object, e As EventArgs) _
        Handles txtTotalLoanAmount.Leave, txtDeductionAmount.Leave

        Dim totalLoanAmount = AccuMath.CommercialRound(Me._newLoanSchedule.TotalLoanAmount)
        Dim deductionAmount = AccuMath.CommercialRound(Me._newLoanSchedule.DeductionAmount)
        Dim totalBalanceLeft = AccuMath.CommercialRound(Me._newLoanSchedule.TotalBalanceLeft)

        Dim numberOfPayPeriod, numberOfPayPeriodLeft As Integer

        If deductionAmount = 0 Then
            numberOfPayPeriod = 0
            numberOfPayPeriodLeft = 0
        Else
            numberOfPayPeriod = _loanScheduleRepository.ComputeNumberOfPayPeriod(totalLoanAmount, deductionAmount)
            numberOfPayPeriodLeft = _loanScheduleRepository.ComputeNumberOfPayPeriodLeft(totalBalanceLeft, deductionAmount)
        End If

        Me._newLoanSchedule.NoOfPayPeriod = numberOfPayPeriod
        Me._newLoanSchedule.LoanPayPeriodLeft = numberOfPayPeriodLeft

        Me._newLoanSchedule.TotalBalanceLeft = totalLoanAmount
    End Sub

#Region "Private Functions"


    Private Sub LoadLoanStatus()
        cmbLoanStatus.DataSource = _loanScheduleRepository.GetStatusList()
    End Sub

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

    Private Sub ForceLoanScheduleGridViewCommit()
        'Workaround. Focus other control to lose focus on current control
        pbEmpPicLoan.Focus()
    End Sub
#End Region
End Class