Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.AccuPay.Desktop.Helpers
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class LoanUserControl

    Private _currentLoan As LoanModel

    Private _statusList As List(Of String)

    Private _isNew As Boolean

    Private if_sysowner_is_benchmark As Boolean

    Private Async Sub LoanUserControl_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim systemOwnerService = MainServiceProvider.GetRequiredService(Of SystemOwnerService)
        Dim policy = MainServiceProvider.GetRequiredService(Of IPolicyHelper)
        if_sysowner_is_benchmark = systemOwnerService.GetCurrentSystemOwner() = SystemOwnerService.Benchmark

        txtLoanInterestPercentage.Visible = policy.UseGoldwingsLoanInterest
        lblLoanInterestPercentage.Visible = policy.UseGoldwingsLoanInterest

        LoadLoanStatus()

        Await LoadLoanTypes()

        Await LoadDeductionSchedules()
    End Sub

    Public Sub SetLoan(loan As LoanModel, isNew As Boolean)

        _currentLoan = loan

        _isNew = isNew

        If _isNew Then
            txtLoanBalance.Visible = False
            lblLoanBalance.Visible = False
            lblLoanBalancePesoSign.Visible = False
        End If

        PopulateStatustComboBox()

        ToggleLoanStatusComboboxVisibility(_currentLoan Is Nothing OrElse Not _currentLoan.IsUnEditable)

        CreateDataBindings()
    End Sub

    Public Function GetLoan() As LoanSchedule

        Return _currentLoan?.CreateLoan()

    End Function

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
        dtpDateFrom.DataBindings.Add("Value", Me._currentLoan, "EffectiveFrom")

        txtDeductionAmount.DataBindings.Clear()
        txtDeductionAmount.DataBindings.Add("Text", Me._currentLoan, "DeductionAmount", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        txtLoanInterestPercentage.DataBindings.Clear()
        txtLoanInterestPercentage.DataBindings.Add("Text", Me._currentLoan, "InterestPercentage", True, DataSourceUpdateMode.OnPropertyChanged, Nothing, "N2")

        cboLoanType.DataBindings.Clear()
        cboLoanType.DataBindings.Add("SelectedValue", Me._currentLoan, "LoanType", True, DataSourceUpdateMode.OnPropertyChanged)

        If Not String.IsNullOrWhiteSpace(Me._currentLoan.LoanTypeName) Then
            cboLoanType.Text = Me._currentLoan.LoanTypeName
        End If

        txtLoanStatus.DataBindings.Clear()
        cmbLoanStatus.DataBindings.Clear()
        If Me._currentLoan.IsUnEditable Then

            txtLoanStatus.DataBindings.Add("Text", Me._currentLoan, "Status")
        Else
            cmbLoanStatus.DataBindings.Add("SelectedValue", Me._currentLoan, "Status", True, DataSourceUpdateMode.OnPropertyChanged)

        End If

        cmbDeductionSchedule.DataBindings.Clear()
        cmbDeductionSchedule.DataBindings.Add("SelectedValue", Me._currentLoan, "DeductionSchedule", True, DataSourceUpdateMode.OnPropertyChanged)

        ToggleLoanStatusComboboxVisibility(Not Me._currentLoan.IsUnEditable)
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

        Dim statusLookUpList = LookUpStringItem.Convert(statusList, hasDefaultItem:=True)

        cmbLoanStatus.ValueMember = "Item"
        cmbLoanStatus.DisplayMember = "Item"

        txtLoanStatus.DataBindings.Clear()
        cmbLoanStatus.DataBindings.Clear()

        cmbLoanStatus.DataSource = statusLookUpList
    End Sub

    Private Async Function LoadLoanTypes() As Task

        Dim productRepository = MainServiceProvider.GetRequiredService(Of ProductRepository)

        Dim loanTypeList As List(Of Product)

        If if_sysowner_is_benchmark Then

            loanTypeList = New List(Of Product)(Await productRepository.GetGovernmentLoanTypesAsync(z_OrganizationID))
        Else
            loanTypeList = New List(Of Product)(Await productRepository.GetLoanTypesAsync(z_OrganizationID))

        End If

        cboLoanType.DisplayMember = "PartNo"
        cboLoanType.DataSource = loanTypeList.OrderBy(Function(i) i.PartNo).ToList()

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
