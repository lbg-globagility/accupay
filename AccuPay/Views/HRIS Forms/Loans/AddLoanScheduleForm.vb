Imports System.Threading.Tasks
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

    Private _loanTypeList As List(Of Product)

    Private _deductionSchedulesList As List(Of String)

    Public Property NewLoanTypes As List(Of Product)

    Public Property IsSaved As Boolean

    Public Property ShowBalloonSuccess As Boolean

    Sub New(employee As Simplified.Employee)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _currentEmployee = employee

        Me.IsSaved = False

        Me.NewLoanTypes = New List(Of Product)

    End Sub

    Private Async Sub AddLoanScheduleForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        PopulateEmployeeData()

        LoadLoanStatus()

        Await LoadLoanTypes()

        Await LoadDeductionSchedules()

        Me._newLoanSchedule = New LoanSchedule
        Me._newLoanSchedule.EmployeeID = _currentEmployee.RowID
        Me._newLoanSchedule.DedEffectiveDateFrom = Date.Now

        Me._newLoanSchedule.Status = LoanScheduleRepository.STATUS_IN_PROGRESS

        Dim firstLoanType = Me._loanTypeList.FirstOrDefault()

        If firstLoanType IsNot Nothing Then
            Me._newLoanSchedule.LoanTypeID = firstLoanType.RowID
            Me._newLoanSchedule.LoanName = firstLoanType.PartNo
        End If

        If _deductionSchedulesList IsNot Nothing Then
            Me._newLoanSchedule.DeductionSchedule = _deductionSchedulesList.FirstOrDefault
        End If

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

        dtpDateFrom.DataBindings.Add("Value", Me._newLoanSchedule, "DedEffectiveDateFrom")

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

        Me._newLoanSchedule.TotalBalanceLeft = totalLoanAmount

        Dim numberOfPayPeriod = _loanScheduleRepository.ComputeNumberOfPayPeriod(totalLoanAmount, deductionAmount)

        Me._newLoanSchedule.NoOfPayPeriod = numberOfPayPeriod

        Me._newLoanSchedule.LoanPayPeriodLeft = numberOfPayPeriod


    End Sub

    Private Sub lnlAddLoanType_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnlAddLoanType.LinkClicked
        Dim form As New AddLoanTypeForm()
        form.ShowDialog()

        If form.IsSaved Then

            Me._loanTypeList.Add(form.NewProduct)

            Me.NewLoanTypes.Add(form.NewProduct)

            PopulateLoanTypeCombobox()

            If Me._newLoanSchedule IsNot Nothing Then

                Me._newLoanSchedule.LoanTypeID = form.NewProduct.RowID
                Me._newLoanSchedule.LoanName = form.NewProduct.PartNo

                Dim orderedLoanTypeList = Me._loanTypeList.OrderBy(Function(p) p.PartNo).ToList

                cboLoanType.SelectedIndex = orderedLoanTypeList.IndexOf(form.NewProduct)

            End If

            ShowBalloonInfo("Loan Type Successfully Added", "Saved")
        End If
    End Sub

#Region "Private Functions"

    Private Sub LoadLoanStatus()

        Dim statusList = _loanScheduleRepository.GetStatusList()

        statusList.Remove(LoanScheduleRepository.STATUS_CANCELLED)
        statusList.Remove(LoanScheduleRepository.STATUS_COMPLETE)

        cmbLoanStatus.DataSource = statusList
    End Sub

    Private Async Function LoadLoanTypes() As Task

        Me._loanTypeList = New List(Of Product)(Await _productRepository.GetLoanTypes())
        PopulateLoanTypeCombobox()

    End Function

    Private Sub PopulateLoanTypeCombobox()
        Dim loanTypes = _productRepository.ConvertToStringList(Me._loanTypeList)

        cboLoanType.DataSource = loanTypes
    End Sub

    Private Async Function LoadDeductionSchedules() As Task

        Me._deductionSchedulesList = _listOfValueRepository.
            ConvertToStringList(Await _listOfValueRepository.GetDeductionSchedules())

        cmbDeductionSchedule.DataSource = Me._deductionSchedulesList

    End Function

    Private Sub ForceLoanScheduleGridViewCommit()
        'Workaround. Focus other control to lose focus on current control
        pbEmpPicLoan.Focus()
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, txtEmployeeFirstName, 400)
    End Sub
#End Region
End Class