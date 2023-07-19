Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class AdvanceValeForm

    Private ReadOnly ADVVALE_LOAN_NAMES As List(Of String) = New List(Of String) From {
        String.Join(" - ", ProductConstant.ADVANCE_VALE_LOAN_MORNINGSUN, ProductConstant.HDMF_LOAN_MORNINGSUN),
        String.Join(" - ", ProductConstant.ADVANCE_VALE_LOAN_MORNINGSUN, ProductConstant.PHILHEALTH_LOAN_MORNINGSUN),
        String.Join(" - ", ProductConstant.ADVANCE_VALE_LOAN_MORNINGSUN, ProductConstant.SSS_LOAN_MORNINGSUN)
    }

    Private ReadOnly _loanDataService As ILoanDataService
    Private ReadOnly _productRepository As IProductRepository
    Private ReadOnly _payPeriodRepository As IPayPeriodRepository
    Private ReadOnly _organizationRepository As IOrganizationRepository
    Private ReadOnly _loanRepository As ILoanRepository
    Private _selectedMonthDate As Date

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _loanDataService = MainServiceProvider.GetRequiredService(Of ILoanDataService)

        _productRepository = MainServiceProvider.GetRequiredService(Of IProductRepository)

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)

        _organizationRepository = MainServiceProvider.GetRequiredService(Of IOrganizationRepository)

        _loanRepository = MainServiceProvider.GetRequiredService(Of ILoanRepository)
    End Sub

    Private Sub AdvanceValeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GovernmentTypeLoansDataGrid.AutoGenerateColumns = False
        LoansDataGrid.AutoGenerateColumns = False

    End Sub

    Private Async Sub SelectSourceMonthToolStripButton_Click(sender As Object, e As EventArgs) Handles SelectSourceMonthToolStripButton.Click

        Dim selectMonthForm As New selectMonth
        If Not selectMonthForm.ShowDialog() = DialogResult.OK Then Return
        _selectedMonthDate = CType(selectMonthForm.MonthValue, Date)

        Dim loans = (Await _loanDataService.GetLoansByMonthPeriodAsync(organizationId:=z_OrganizationID, dateTime:=_selectedMonthDate)).
            OrderBy(Function(l) l.FullNameLastNameFirst).
                ThenBy(Function(l) l.DedEffectiveDateFrom).
            ToList()

        GovernmentTypeLoansDataGrid.DataSource = loans
        LoansDataGrid.DataSource = Enumerable.Empty(Of Loan).ToList()

        SelectAllCheckBox.Checked = True
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        If Not LoansDataGrid.Rows.Count > 0 Then Return

        Dim convertedAdvValeLoans = LoansDataGrid.Rows.
            OfType(Of DataGridViewRow).
            Select(Function(r) CType(r.DataBoundItem, Loan)).
            ToList()

        Dim prompt = MessageBox.Show(text:=$"Are you sure you want to save ({convertedAdvValeLoans.Count()}) ADV. Vale?",
            caption:="Save ADV. Vale Loans",
            buttons:=MessageBoxButtons.YesNoCancel,
            icon:=MessageBoxIcon.Question,
            defaultButton:=MessageBoxDefaultButton.Button2)

        If Not prompt = DialogResult.Yes Then Return

        Await FunctionUtils.TryCatchFunctionAsync(String.Empty,
            Async Function()
                Dim parentLoans = Await _loanRepository.
                    GetManyByIdsAsTrackableAsync(ids:=convertedAdvValeLoans.Select(Function(l) l.ParentLoanId.Value).ToArray())

                For Each parentLoan In parentLoans
                    Dim convertedAdvValeLoan = convertedAdvValeLoans.FirstOrDefault(Function(l) l.ParentLoanId.Value = parentLoan.RowID.Value)
                    If convertedAdvValeLoan Is Nothing Then Continue For
                    'parentLoan.TotalLoanAmount -= convertedAdvValeLoan.TotalLoanAmount
                    parentLoan.TotalBalanceLeft -= convertedAdvValeLoan.TotalBalanceLeft
                    parentLoan.RecomputeTotalPayPeriod()
                    parentLoan.RecomputePayPeriodLeft()
                Next

                Dim parentAndChildLoans = convertedAdvValeLoans.Concat(parentLoans).ToList()

                Await _loanDataService.SaveManyAsync(entities:=parentAndChildLoans, currentlyLoggedInUserId:=z_User)

            End Function)
    End Sub

    Private Async Sub ConvertToAdvValeButton_Click(sender As Object, e As EventArgs) Handles ConvertToAdvValeButton.Click
        Dim organization = Await _organizationRepository.GetByIdWithAddressAsync(z_OrganizationID)
        Dim succeedingMonthDate = _selectedMonthDate.AddMonths(1)
        Dim selectedYear = succeedingMonthDate.Date.Year
        Dim selectedMonth = succeedingMonthDate.Date.Month
        Dim payPeriods = (Await _payPeriodRepository.GetYearlyPayPeriodsAsync(organizationId:=z_OrganizationID,
                year:=selectedYear,
                currentUserId:=z_User)).
            Where(Function(p) p.Month = selectedMonth).
            ToList()
        If organization.IsWeekly Then
            payPeriods = (Await _payPeriodRepository.GetYearlyPayPeriodsOfWeeklyAsync(organization:=organization,
                year:=selectedYear,
                currentUserId:=z_User)).
            Where(Function(p) p.Month = selectedMonth).
            ToList()
        End If

        Dim firstPayPeriod = payPeriods.
            Where(Function(p) Not p.IsClosed).
            OrderBy(Function(p) p.PayFromDate).
            FirstOrDefault()

        Dim advValeGovtLoanTypeNames = New List(Of (String, Core.Enums.GovernmentDeductionTypeEnum)) From {
            (ADVVALE_LOAN_NAMES.FirstOrDefault(), Core.Enums.GovernmentDeductionTypeEnum.Hdmf),
            (ADVVALE_LOAN_NAMES(1), Core.Enums.GovernmentDeductionTypeEnum.PhilHealth),
            (ADVVALE_LOAN_NAMES.LastOrDefault(), Core.Enums.GovernmentDeductionTypeEnum.Sss)
        }
        Dim advValeProducts = Await _productRepository.AddManyLoanTypeAsync(loanTypeItems:=advValeGovtLoanTypeNames,
            organizationId:=z_OrganizationID,
            userId:=z_User)

        Dim loans = GovernmentTypeLoansDataGrid.Rows.OfType(Of DataGridViewRow).
            Where(Function(r) CType(r.Cells(Column1.Name).Value, Boolean) = True).
            Select(Function(r) CType(r.DataBoundItem, Loan)).
            ToList()

        ' HDMF
        Dim hdmfLoanType = advValeProducts.
            FirstOrDefault(Function(p) p.PartNo = ADVVALE_LOAN_NAMES.FirstOrDefault())
        Dim hdmfLoanTypeId = hdmfLoanType.RowID.Value

        Dim groupedHdmfLoans = loans.
            Where(Function(l) l.IsHDMFLoanOfMorningSun).
            GroupBy(Function(l) l.EmployeeID).
            ToList()
        Dim hdmfLoans = New List(Of Loan)
        For Each item In groupedHdmfLoans
            Dim loans1 = item.Select(Function(l) l).
                OrderBy(Function(l) l.DedEffectiveDateFrom).
                ToList()
            Dim counter = 0
            For Each loanItem In loans1
                Dim l = loanItem
                Dim newLoan = Loan.NewLoan(organizationId:=z_OrganizationID,
                     userId:=z_User,
                     employeeId:=l.EmployeeID.Value,
                     loanTypeId:=hdmfLoanTypeId,
                     totalLoanAmount:=l.TotalBalanceLeft,
                     totalBalance:=l.TotalBalanceLeft,
                     effectiveFrom:=payPeriods(counter).PayFromDate,
                     deductionAmount:=l.DeductionAmount,
                     status:=Loan.STATUS_IN_PROGRESS,
                     deductionSchedule:=l.DeductionSchedule,
                     parentLoanId:=l.RowID)
                newLoan.SetEmployee(l.Employee)
                newLoan.SetLoanType(hdmfLoanType)
                hdmfLoans.Add(newLoan)
                counter += 1
            Next
        Next

        ' PhilHealth
        Dim philHealthLoanType = advValeProducts.
            FirstOrDefault(Function(p) p.PartNo = ADVVALE_LOAN_NAMES(1))
        Dim philHealthLoanTypeId = philHealthLoanType.RowID.Value

        Dim groupedPhilHealthLoans = loans.
            Where(Function(l) l.IsPhilHealthLoanOfMorningSun).
            GroupBy(Function(l) l.EmployeeID).
            ToList()
        Dim philHealthLoans = New List(Of Loan)
        For Each item In groupedPhilHealthLoans
            Dim loans1 = item.Select(Function(l) l).
                OrderBy(Function(l) l.DedEffectiveDateFrom).
                ToList()
            Dim counter = 0
            For Each loanItem In loans1
                Dim l = loanItem
                Dim newLoan = Loan.NewLoan(organizationId:=z_OrganizationID,
                     userId:=z_User,
                     employeeId:=l.EmployeeID.Value,
                     loanTypeId:=philHealthLoanTypeId,
                     totalLoanAmount:=l.TotalBalanceLeft,
                     totalBalance:=l.TotalBalanceLeft,
                     effectiveFrom:=payPeriods(counter).PayFromDate,
                     deductionAmount:=l.DeductionAmount,
                     status:=Loan.STATUS_IN_PROGRESS,
                     deductionSchedule:=l.DeductionSchedule,
                     parentLoanId:=l.RowID)
                newLoan.SetEmployee(l.Employee)
                newLoan.SetLoanType(philHealthLoanType)
                philHealthLoans.Add(newLoan)
                counter += 1
            Next
        Next

        ' SSS
        Dim sssLoanType = advValeProducts.
            FirstOrDefault(Function(p) p.PartNo = ADVVALE_LOAN_NAMES.LastOrDefault())
        Dim sssLoanTypeId = sssLoanType.RowID.Value

        Dim groupedSssLoans = loans.
            Where(Function(l) l.IsSssLoanOfMorningSun).
            GroupBy(Function(l) l.EmployeeID).
            ToList()
        Dim sssLoans = New List(Of Loan)
        For Each item In groupedSssLoans
            Dim loans1 = item.Select(Function(l) l).
                OrderBy(Function(l) l.DedEffectiveDateFrom).
                ToList()
            Dim counter = 0
            For Each loanItem In loans1
                Dim l = loanItem
                Dim newLoan = Loan.NewLoan(organizationId:=z_OrganizationID,
                     userId:=z_User,
                     employeeId:=l.EmployeeID.Value,
                     loanTypeId:=sssLoanTypeId,
                     totalLoanAmount:=l.TotalBalanceLeft,
                     totalBalance:=l.TotalBalanceLeft,
                     effectiveFrom:=payPeriods(counter).PayFromDate,
                     deductionAmount:=l.DeductionAmount,
                     status:=Loan.STATUS_IN_PROGRESS,
                     deductionSchedule:=l.DeductionSchedule,
                     parentLoanId:=l.RowID)
                newLoan.SetEmployee(l.Employee)
                newLoan.SetLoanType(sssLoanType)
                sssLoans.Add(newLoan)
                counter += 1
            Next
        Next

        Dim loanEntities = New List(Of Loan)
        loanEntities.AddRange(hdmfLoans)
        loanEntities.AddRange(philHealthLoans)
        loanEntities.AddRange(sssLoans)

        LoansDataGrid.DataSource = loanEntities.
            OrderBy(Function(l) l.FullNameLastNameFirst).
                ThenBy(Function(l) l.DedEffectiveDateFrom).
            ToList()
        TabControl1.SelectedTab = TabPage2
    End Sub

    Private Sub SelectAllCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles SelectAllCheckBox.CheckedChanged
        Dim rows = GovernmentTypeLoansDataGrid.Rows.OfType(Of DataGridViewRow).ToList()
        For Each item In rows
            item.Cells(Column1.Name).Value = SelectAllCheckBox.Checked
        Next
    End Sub

End Class
