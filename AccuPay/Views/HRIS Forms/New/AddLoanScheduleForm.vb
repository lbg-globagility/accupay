Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Utilities
Imports AccuPay.Utils

Public Class AddLoanScheduleForm

    Dim sys_ownr As New SystemOwner

    Private if_sysowner_is_benchmark As Boolean

    Private _currentEmployee As Employee

    Private _newLoanSchedule As New LoanSchedule

    Private _productRepository As New ProductRepository

    Private _listOfValueRepository As New ListOfValueRepository

    Private _loanScheduleRepository As New LoanScheduleRepository

    Private _loanTypeList As List(Of Product)

    Private _deductionSchedulesList As List(Of String)

    Public Property NewLoanTypes As List(Of Product)

    Public Property IsSaved As Boolean

    Public Property ShowBalloonSuccess As Boolean

    Sub New(employee As Data.Entities.Employee)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _currentEmployee = employee

        Me.IsSaved = False

        Me.NewLoanTypes = New List(Of Data.Entities.Product)

        if_sysowner_is_benchmark = sys_ownr.CurrentSystemOwner = SystemOwner.Benchmark

    End Sub

    Private Async Sub AddLoanScheduleForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        PopulateEmployeeData()

        LoadLoanStatus()

        Await LoadLoanTypes()

        Await LoadDeductionSchedules()

        ResetForm()

    End Sub

    Private Sub ResetForm()
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

        txtEmployeeFirstName.Text = _currentEmployee?.FullNameWithMiddleInitial

        txtEmployeeNumber.Text = _currentEmployee?.EmployeeIdWithPositionAndEmployeeType

        pbEmployeePicture.Image = ConvByteToImage(_currentEmployee.Image)

    End Sub

    Private Sub CreateDataBindings()

        txtLoanNumber.DataBindings.Clear()
        txtLoanNumber.DataBindings.Add("Text", Me._newLoanSchedule, "LoanNumber")

        txtRemarks.DataBindings.Clear()
        txtRemarks.DataBindings.Add("Text", Me._newLoanSchedule, "Comments")

        txtTotalLoanAmount.DataBindings.Clear()
        txtTotalLoanAmount.DataBindings.Add("Text", Me._newLoanSchedule, "TotalLoanAmount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        txtLoanBalance.DataBindings.Clear()
        txtLoanBalance.DataBindings.Add("Text", Me._newLoanSchedule, "TotalBalanceLeft", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        dtpDateFrom.DataBindings.Clear()
        dtpDateFrom.DataBindings.Add("Value", Me._newLoanSchedule, "DedEffectiveDateFrom")

        txtNumberOfPayPeriod.DataBindings.Clear()
        txtNumberOfPayPeriod.DataBindings.Add("Text", Me._newLoanSchedule, "NoOfPayPeriod", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N0")

        txtNumberOfPayPeriodLeft.DataBindings.Clear()
        txtNumberOfPayPeriodLeft.DataBindings.Add("Text", Me._newLoanSchedule, "LoanPayPeriodLeft", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N0")

        txtDeductionAmount.DataBindings.Clear()
        txtDeductionAmount.DataBindings.Add("Text", Me._newLoanSchedule, "DeductionAmount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        txtLoanInterestPercentage.DataBindings.Clear()
        txtLoanInterestPercentage.DataBindings.Add("Text", Me._newLoanSchedule, "DeductionPercentage", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        cboLoanType.DataBindings.Clear()
        cboLoanType.DataBindings.Add("Text", Me._newLoanSchedule, "LoanName")

        cmbLoanStatus.DataBindings.Clear()
        cmbLoanStatus.DataBindings.Add("Text", Me._newLoanSchedule, "Status")

        cmbDeductionSchedule.DataBindings.Clear()
        cmbDeductionSchedule.DataBindings.Add("Text", Me._newLoanSchedule, "DeductionSchedule")
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub cboLoanType_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboLoanType.SelectedValueChanged
        If Me._newLoanSchedule IsNot Nothing Then
            Dim selectedLoanType = Me._loanTypeList.FirstOrDefault(Function(l) l.PartNo = cboLoanType.Text)

            If selectedLoanType Is Nothing Then

                Me._newLoanSchedule.LoanTypeID = Nothing
            Else

                Me._newLoanSchedule.LoanTypeID = selectedLoanType.RowID

            End If
        End If
    End Sub

    Private Async Sub AddLoanScheduleButtonClicked(sender As Object, e As EventArgs) _
        Handles btnAddAndNew.Click, btnAddAndClose.Click

        ForceLoanScheduleDataBindingsCommit()

        Dim confirmMessage = ""
        Dim messageTitle = "New Loan"

        If Me._newLoanSchedule.TotalLoanAmount = 0 AndAlso Me._newLoanSchedule.DeductionAmount = 0 Then
            confirmMessage = "You did not enter a value for Total Loan Amount and Deduction Amount. Do you want to save the new loan?"

        ElseIf Me._newLoanSchedule.TotalLoanAmount = 0 Then
            confirmMessage = "You did not enter a value for Total Loan Amount. Do you want to save the new loan?"

        ElseIf Me._newLoanSchedule.DeductionAmount = 0 Then
            confirmMessage = "You did not enter a value for Deduction Amount. Do you want to save the new loan?"

        End If

        If String.IsNullOrWhiteSpace(confirmMessage) = False Then

            If MessageBoxHelper.Confirm(Of Boolean) _
                (confirmMessage, messageTitle, messageBoxIcon:=MessageBoxIcon.Warning) = False Then Return

        End If

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Await _loanScheduleRepository.SaveAsync(Me._newLoanSchedule,
                                                        Me._loanTypeList,
                                                        organizationId:=z_OrganizationID,
                                                        userId:=z_User)

                Dim repo As New UserActivityRepository
                repo.RecordAdd(z_User, "Loan", Me._newLoanSchedule.RowID, z_OrganizationID)

                Me.IsSaved = True

                If sender Is btnAddAndNew Then
                    ShowBalloonInfo("Loan Successfully Added", "Saved")

                    ResetForm()
                Else

                    Me.ShowBalloonSuccess = True
                    Me.Close()
                End If
            End Function)

    End Sub

    Private loanAmountBeforeTextChange As Decimal

    Private loanInterestPercentageBeforeTextChange As Decimal

    Private Sub txtLoanInterestPercentage_Enter(sender As Object, e As EventArgs) Handles txtLoanInterestPercentage.Enter

        If Me._newLoanSchedule Is Nothing Then Return

        loanInterestPercentageBeforeTextChange = Me._newLoanSchedule.DeductionPercentage

    End Sub

    Private Sub txtLoanInterestPercentage_Leave(sender As Object, e As EventArgs) Handles txtLoanInterestPercentage.Leave

        If Me._newLoanSchedule Is Nothing Then Return

        If loanInterestPercentageBeforeTextChange = Me._newLoanSchedule.DeductionPercentage Then Return

        Dim totalPlusInterestRate As Decimal = 1 + (Me._newLoanSchedule.DeductionPercentage * 0.01D)

        Me._newLoanSchedule.TotalLoanAmount = Me._newLoanSchedule.TotalLoanAmount * totalPlusInterestRate

        UpdateBalanceAndNumberOfPayPeriod()

    End Sub

    Private Sub txtTotalLoanAmount_Enter(sender As Object, e As EventArgs) Handles txtTotalLoanAmount.Enter

        If Me._newLoanSchedule Is Nothing Then Return

        loanAmountBeforeTextChange = Me._newLoanSchedule.TotalLoanAmount

    End Sub

    Private Sub txtTotalLoanAmount_Leave(sender As Object, e As EventArgs) _
        Handles txtTotalLoanAmount.Leave, txtDeductionAmount.Leave

        If Me._newLoanSchedule Is Nothing Then Return

        If sender Is txtTotalLoanAmount Then
            If loanAmountBeforeTextChange <> Me._newLoanSchedule.TotalLoanAmount Then
                Me._newLoanSchedule.DeductionPercentage = 0
            End If
        End If

        UpdateBalanceAndNumberOfPayPeriod()

    End Sub

    Private Sub UpdateBalanceAndNumberOfPayPeriod()
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

            Me._loanTypeList.Add(form.NewLoanType)

            Me.NewLoanTypes.Add(form.NewLoanType)

            PopulateLoanTypeCombobox()

            If Me._newLoanSchedule IsNot Nothing Then

                Me._newLoanSchedule.LoanTypeID = form.NewLoanType.RowID
                Me._newLoanSchedule.LoanName = form.NewLoanType.PartNo

                Dim orderedLoanTypeList = Me._loanTypeList.OrderBy(Function(p) p.PartNo).ToList

                cboLoanType.SelectedIndex = orderedLoanTypeList.IndexOf(form.NewLoanType)

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

        If if_sysowner_is_benchmark Then

            Me._loanTypeList = New List(Of Data.Entities.Product)(Await _productRepository.GetGovernmentLoanTypes(z_OrganizationID))
        Else
            Me._loanTypeList = New List(Of Data.Entities.Product)(Await _productRepository.GetLoanTypes(z_OrganizationID))

        End If

        PopulateLoanTypeCombobox()

    End Function

    Private Sub PopulateLoanTypeCombobox()
        Dim loanTypes = _productRepository.ConvertToStringList(Me._loanTypeList)

        cboLoanType.DataSource = loanTypes
    End Sub

    Private Async Function LoadDeductionSchedules() As Task

        Me._deductionSchedulesList = _listOfValueRepository.
            ConvertToStringList(Await _listOfValueRepository.GetDeductionSchedulesAsync())

        cmbDeductionSchedule.DataSource = Me._deductionSchedulesList

    End Function

    Private Sub ForceLoanScheduleDataBindingsCommit()
        'Workaround. Focus other control to lose focus on current control
        pbEmployeePicture.Focus()
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, EmployeeInfoTabLayout, 400)
    End Sub

#End Region

End Class