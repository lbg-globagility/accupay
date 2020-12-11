Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.AccuPay.Desktop.Helpers
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class LoanUserControl

    Private _currentLoan As LoanSchedule

    Private _statusList As List(Of String)

    Private _isNew As Boolean

    Private if_sysowner_is_benchmark As Boolean

    Private loanAmountBeforeTextChange As Decimal

    Private loanInterestPercentageBeforeTextChange As Decimal

    Private Async Sub LoanUserControl_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim systemOwnerService = MainServiceProvider.GetRequiredService(Of SystemOwnerService)
        if_sysowner_is_benchmark = systemOwnerService.GetCurrentSystemOwner() = SystemOwnerService.Benchmark

        _currentLoan = New LoanSchedule()

        LoadLoanStatus()

        Await LoadLoanTypes()

        Await LoadDeductionSchedules()
    End Sub

    Public Sub SetLoan(loan As LoanSchedule, isNew As Boolean)

        _currentLoan = loan
        _isNew = isNew

        PopulateStatustComboBox()

        ToggleLoanStatusComboboxVisibility(loan Is Nothing OrElse Not loan.IsUnEditable)

        CreateDataBindings()
    End Sub

    Public Function GetLoan() As LoanSchedule

        Return _currentLoan

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

    Private Async Sub lnlAddLoanType_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnlAddLoanType.LinkClicked
        Dim form As New AddLoanTypeForm()
        form.ShowDialog()

        If form.IsSaved Then

            Await LoadLoanTypes()

            If Me._currentLoan IsNot Nothing Then

                Dim loanTypes = CType(cboLoanType.DataSource, List(Of LookUpItem))

                Dim newLoanType = loanTypes.
                    Where(Function(l) l.Id IsNot Nothing AndAlso l.Id.Value = form.NewLoanType.RowID.Value).
                    FirstOrDefault()

                cboLoanType.SelectedIndex = loanTypes.IndexOf(newLoanType)

            End If

            MessageBoxHelper.Information("Loan Type Successfully Added", "Saved")
        End If
    End Sub

    Private Sub ToggleLoanStatusComboboxVisibility(showCombobox As Boolean)

        cmbLoanStatus.Visible = showCombobox
        txtLoanStatus.Visible = Not showCombobox

    End Sub

    Private Sub CreateDataBindings()

        If Me._currentLoan Is Nothing Then Return

        txtLoanNumber.DataBindings.Clear()
        txtLoanNumber.DataBindings.Add("Text", Me._currentLoan, "LoanNumber", True, DataSourceUpdateMode.OnPropertyChanged)

        txtRemarks.DataBindings.Clear()
        txtRemarks.DataBindings.Add("Text", Me._currentLoan, "Comments", True, DataSourceUpdateMode.OnPropertyChanged)

        txtTotalLoanAmount.DataBindings.Clear()
        txtTotalLoanAmount.DataBindings.Add("Text", Me._currentLoan, "TotalLoanAmount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")
        If Me._currentLoan.HasTransactions OrElse Me._currentLoan.TotalBalanceLeft < Me._currentLoan.TotalLoanAmount Then
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
        cboLoanType.DataBindings.Add("SelectedValue", Me._currentLoan, "LoanTypeID", True, DataSourceUpdateMode.OnPropertyChanged)

        txtLoanStatus.DataBindings.Clear()
        cmbLoanStatus.DataBindings.Clear()
        If Me._currentLoan.IsUnEditable Then

            txtLoanStatus.DataBindings.Add("Text", Me._currentLoan, "Status")
        Else
            cmbLoanStatus.DataBindings.Add("Text", Me._currentLoan, "Status", True, DataSourceUpdateMode.OnPropertyChanged)

        End If

        cmbDeductionSchedule.DataBindings.Clear()
        cmbDeductionSchedule.DataBindings.Add("SelectedValue", Me._currentLoan, "DeductionSchedule", True, DataSourceUpdateMode.OnPropertyChanged)

        ToggleLoanStatusComboboxVisibility(Not Me._currentLoan.IsUnEditable)
    End Sub

    Private Sub UpdateBalanceAndNumberOfPayPeriod()
        Me._currentLoan.TotalLoanAmount = AccuMath.CommercialRound(Me._currentLoan.TotalLoanAmount)
        Me._currentLoan.DeductionAmount = AccuMath.CommercialRound(Me._currentLoan.DeductionAmount)

        If Me._currentLoan.HasTransactions = False AndAlso Me._currentLoan.IsUnEditable = False Then

            Me._currentLoan.RecomputeTotalPayPeriod()

            Me._currentLoan.TotalBalanceLeft = Me._currentLoan.TotalLoanAmount

        End If

        Me._currentLoan.RecomputePayPeriodLeft()
    End Sub

    Private Sub LoadLoanStatus()

        Dim repository = MainServiceProvider.GetRequiredService(Of LoanRepository)
        _statusList = repository.GetStatusList()

        PopulateStatustComboBox()
    End Sub

    Private Sub PopulateStatustComboBox()

        Dim statusList = _statusList.ToList()

        If _isNew Then
            statusList.Remove(LoanSchedule.STATUS_CANCELLED)
        End If

        statusList.Remove(LoanSchedule.STATUS_COMPLETE)

        cmbLoanStatus.DataSource = statusList
    End Sub

    Private Async Function LoadLoanTypes() As Task

        Dim productRepository = MainServiceProvider.GetRequiredService(Of ProductRepository)

        Dim loanTypeList As List(Of Product)

        If if_sysowner_is_benchmark Then

            loanTypeList = New List(Of Product)(Await productRepository.GetGovernmentLoanTypesAsync(z_OrganizationID))
        Else
            loanTypeList = New List(Of Product)(Await productRepository.GetLoanTypesAsync(z_OrganizationID))

        End If

        Dim lookUpItems = LookUpItem.Convert(loanTypeList, "RowID", "PartNo", hasDefaultItem:=True)

        cboLoanType.ValueMember = "Id"
        cboLoanType.DisplayMember = "DisplayMember"
        cboLoanType.DataSource = lookUpItems.OrderBy(Function(i) i.DisplayMember).ToList()

    End Function

    Private Async Function LoadDeductionSchedules() As Task

        Dim listOfValueRepository = MainServiceProvider.GetRequiredService(Of ListOfValueRepository)

        Dim deductionSchedules = listOfValueRepository.
            ConvertToStringList(Await listOfValueRepository.GetDeductionSchedulesAsync())

        Dim deductionSchedulesList = LookUpStringItem.Convert(deductionSchedules, hasDefaultItem:=True)

        cmbDeductionSchedule.ValueMember = "Item"
        cmbDeductionSchedule.DisplayMember = "Item"
        cmbDeductionSchedule.DataSource = deductionSchedulesList

    End Function

End Class
