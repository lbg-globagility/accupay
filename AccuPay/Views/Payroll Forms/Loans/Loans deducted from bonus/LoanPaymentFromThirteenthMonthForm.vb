Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class LoanPaymentFromThirteenthMonthForm
    Private ReadOnly _currentPayPeriod As PayPeriod
    Private ReadOnly _paystubDataService As IPaystubDataService
    Private ReadOnly _loanDataService As ILoanDataService
    Private _loans As ICollection(Of Loan)

    Public Sub New(currentPayPeriod As PayPeriod)

        InitializeComponent()

        _currentPayPeriod = currentPayPeriod

        _paystubDataService = MainServiceProvider.GetRequiredService(Of IPaystubDataService)

        _loanDataService = MainServiceProvider.GetRequiredService(Of ILoanDataService)
    End Sub

    Public ReadOnly Property HasChanges As Boolean

    Private Async Function LoadLoans(paystubs As List(Of Paystub)) As Task(Of ICollection(Of Loan))
        Return Await _loanDataService.GetCurrentPayrollLoansAsync(z_OrganizationID, _currentPayPeriod, paystubs)
    End Function

    Private Async Function LoadPaystubs() As Task(Of ICollection(Of Paystub))
        Return Await _paystubDataService.GetByPaystubsForLoanPaymentFrom13thMonthAsync(_currentPayPeriod.RowID.Value)
    End Function

    Private Sub PopulateEmployees(paystubs As List(Of Paystub))
        Dim employeeModels As New List(Of ThirteenthMonthEmployeeModel)

        For Each paystub In paystubs
            Dim model = New ThirteenthMonthEmployeeModel(paystub)
            Dim thirteenthMonthAdjustment = paystub.Adjustments.
                Where(Function(i) i.Is13thMonthPay).
                FirstOrDefault()

            Dim has13thMonthPay = thirteenthMonthAdjustment IsNot Nothing

            If has13thMonthPay Then model.UpdateThirteenthMonthPayAmount(thirteenthMonthAdjustment.Amount, 0)

            model.IsSelected = has13thMonthPay

            employeeModels.Add(model)
        Next

        DataGridViewX1.DataSource = employeeModels.
            Where(Function(m) m.IsSelected).
            ToList()
    End Sub

    Private Function GetModel(Of T)(gridRow As DataGridViewRow) As T
        Return DirectCast(gridRow.DataBoundItem, T)
    End Function

    Private Async Sub LoanPaymentFromThirteenthMonthForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridViewX1.AutoGenerateColumns = False
        DataGridViewX2.AutoGenerateColumns = False

        Column9.HeaderText = $"Loan Balance as of {_currentPayPeriod.PayToDate.ToShortDateString()}"
        Column8.HeaderText = $"Loan Deduction as of {_currentPayPeriod.PayToDate.ToShortDateString()}"

        Await StartLoad()
    End Sub

    Private Async Function StartLoad(Optional isAddHandler As Boolean = True) As Task

        Await FunctionUtils.TryCatchFunctionAsync("",
            Async Function()
                Dim paystubs = Await LoadPaystubs()

                PopulateEmployees(paystubs.ToList())

                _loans = Await LoadLoans(paystubs.ToList())

                If isAddHandler Then AddHandler DataGridViewX1.SelectionChanged, AddressOf DataGridViewX1_SelectionChanged

                DataGridViewX1_SelectionChanged(DataGridViewX1, New EventArgs)
            End Function)
    End Function

    Private Sub DataGridViewX1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewX1.CellContentClick

    End Sub

    Private Sub DataGridViewX1_SelectionChanged(sender As Object, e As EventArgs)
        If _loans Is Nothing AndAlso DataGridViewX1.CurrentRow Is Nothing Then Return

        Dim model = GetModel(Of ThirteenthMonthEmployeeModel)(DataGridViewX1.CurrentRow)

        Dim loans = _loans.
            Where(Function(l) CBool(l.EmployeeID = model.EmployeeId)).
            ToList()

        Dim loanModels As New List(Of LoanModel)
        For Each loan In loans
            Dim loanModel = New LoanModel(model, loan:=loan, payperiodId:=_currentPayPeriod.RowID.Value)
            loanModels.Add(loanModel)
        Next

        DataGridViewX2.DataSource = loanModels

        Update13thMonthPayBalance(model, loanModels)

        If loanModels.Any() Then
            TabPage2.Text = $"Loans ({loanModels.Count()})"
        Else
            TabPage2.Text = "Loans"
        End If

        UpdateSaveButton()
    End Sub

    Private Sub Update13thMonthPayBalance(model As ThirteenthMonthEmployeeModel, loanModels As List(Of LoanModel))
        Dim totalLoanPayment = loanModels.Sum(Function(l) l.AmountPayment)

        Dim currentThirteenthMonthBalance =
            If(model.ThirteenthMonthAmount - totalLoanPayment < 0, 0, model.ThirteenthMonthAmount - totalLoanPayment)

        UpdateLabel(totalLoanPayment, currentThirteenthMonthBalance)
    End Sub

    Private Sub UpdateLabel(totalLoanPayment As Decimal, currentThirteenthMonthBalance As Decimal)
        Label1.Text = $"13th Month Pay Balance: {currentThirteenthMonthBalance:N}{Environment.NewLine}Total Loan Payment: {totalLoanPayment:N}"
    End Sub

    Private Sub DataGridViewX2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewX2.CellContentClick

    End Sub

    Private Sub DataGridViewX2_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewX2.CellEndEdit
        Dim eModel = GetModel(Of ThirteenthMonthEmployeeModel)(DataGridViewX1.CurrentRow)

        Dim loanModels = GetGridRows(DataGridViewX2).
            Select(Function(r) GetModel(Of LoanModel)(r)).
            ToList()

        Dim currLoanModel = GetModel(Of LoanModel)(DataGridViewX2.CurrentRow)

        Dim totalLoanPayment = loanModels.
            Where(Function(l) l.LoanId <> currLoanModel.LoanId).
            Sum(Function(l) l.AmountPayment)

        Dim currentThirteenthMonthBalance =
            If(eModel.ThirteenthMonthAmount - totalLoanPayment < 0, 0, eModel.ThirteenthMonthAmount - totalLoanPayment)

        currLoanModel.UpdateCurrentThirteenthMonthBalance(currentThirteenthMonthBalance)

        If currLoanModel.IsExcessivePayment OrElse currLoanModel.IsFulfilled Then currLoanModel.SetValidAmountPayment()

        UpdateSaveButton()

        DataGridViewX2.Refresh()

        Update13thMonthPayBalance(eModel, loanModels)
    End Sub

    Private Sub UpdateSaveButton()
        Dim changedCount = GetChangesCount()
        Dim isValid = changedCount > 0

        Button1.Enabled = isValid
        Button1.Text = If(isValid, $"Save ({changedCount})", "Save")
    End Sub

    Private Function GetChangesCount() As Integer
        Dim models = GetGridRows(DataGridViewX2).
            Select(Function(r) GetModel(Of LoanModel)(r)).
            ToList()

        Return models.
            Where(Function(m) m.HasChanges).
            Count()
    End Function

    Private Sub TabPage2_Enter(sender As Object, e As EventArgs) Handles TabPage2.Enter

    End Sub

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Await FunctionUtils.TryCatchFunctionAsync("",
            Async Function()
                Dim repository = MainServiceProvider.GetRequiredService(Of ILoanPaymentFromThirteenthMonthPayDataService)

                Dim results = GetGridRows(DataGridViewX2).
                    Select(Function(r) GetModel(Of LoanModel)(r).Export()).
                    ToList()

                Await repository.SaveManyAsync(_currentPayPeriod, results)

                _HasChanges = True

                Await StartLoad()
            End Function)
    End Sub

    Private Function GetGridRows(grid As DataGridView) As List(Of DataGridViewRow)
        Dim gridRows = grid.Rows.
            OfType(Of DataGridViewRow).
            ToList()

        Return gridRows
    End Function

    Private Class LoanModel
        Private ReadOnly _employeeModel As ThirteenthMonthEmployeeModel
        Private ReadOnly _paystub As Paystub
        Private ReadOnly _loan As Loan
        Private ReadOnly _originalAmountPayment As Decimal

        Public Sub New(
            employeeModel As ThirteenthMonthEmployeeModel,
            loan As Loan,
            payperiodId As Integer)

            _employeeModel = employeeModel
            _paystub = _employeeModel.PaystubObject
            _loan = loan

            LoanId = _loan.RowID.Value
            LoanNumber = _loan.LoanNumber
            TotalAmount = _loan.TotalLoanAmount
            LoanType = _loan.LoanType.PartNo

            Dim loanPayment13thMonth = _employeeModel.PaystubObject.
                LoanPaymentFromThirteenthMonthPays.
                Where(Function(l) l.PaystubId = _paystub.RowID.Value).
                Where(Function(l) l.LoanId = _loan.RowID.Value).
                FirstOrDefault()

            'Dim loanPaymentBonus = ??

            Dim loanTransaction = _employeeModel.PaystubObject.
                LoanTransactions.
                Where(Function(lt) CBool(lt.PayPeriodID = payperiodId)).
                Where(Function(lt) lt.LoanID = _loan.RowID.Value).
                FirstOrDefault()

            Dim loanTransactionDeductionAmount = If(loanTransaction?.DeductionAmount, _loan.DeductionAmount)
            DeductionAmount = loanTransactionDeductionAmount

            LoanDeductionAmount = _loan.DeductionAmount

            Balance = If(loanTransaction?.TotalBalance, _loan.TotalBalanceLeft)

            DeductionSchedule = _loan.DeductionSchedule
            PayPeriodsLeft = _loan.LoanPayPeriodLeft

            ThirteenthMonthAmount = _employeeModel.ThirteenthMonthAmount

            LoanPayment = loanPayment13thMonth

            If Not IsNew Then
                AmountPayment = loanPayment13thMonth.AmountPayment

                'If AmountPayment > 0 AndAlso loanTransactionDeductionAmount > AmountPayment Then DeductionAmount = DeductionAmount Mod AmountPayment
            End If

            _originalAmountPayment = AmountPayment
        End Sub

        Public ReadOnly Property LoanId As Integer
        Public ReadOnly Property LoanNumber As String
        Public ReadOnly Property LoanType As String
        Public ReadOnly Property TotalAmount As Decimal
        Public ReadOnly Property DeductionAmount As Decimal
        Public ReadOnly Property LoanDeductionAmount As Decimal
        Public ReadOnly Property Balance As Decimal
        Public ReadOnly Property DeductionSchedule As String
        Public ReadOnly Property PayPeriodsLeft As Decimal
        Public ReadOnly Property ThirteenthMonthAmount As Decimal
        Public Property AmountPayment As Decimal
        Public ReadOnly Property LoanPayment As LoanPaymentFromThirteenthMonthPay

        Public ReadOnly Property IsNew As Boolean
            Get
                Return LoanPayment Is Nothing
            End Get
        End Property

        Public ReadOnly Property TotalPayment As Decimal
            Get
                If IsNew Then
                    Return DeductionAmount + AmountPayment
                Else
                    Return DeductionAmount + (AmountPayment - LoanPayment.AmountPayment)
                End If
            End Get
        End Property

        Public ReadOnly Property IsFulfilled As Boolean
            Get
                Return Balance <= TotalPayment
            End Get
        End Property

        Public ReadOnly Property ExclusiveCurrent13thMonthAmount As Decimal

        Public ReadOnly Property IsExcessivePayment As Boolean
            Get
                Return IsExcessivePaymentOver13thMonthPay OrElse IsExcessiveBalancePayment
            End Get
        End Property

        Public ReadOnly Property IsExcessivePaymentOver13thMonthPay As Boolean
            Get
                Return AmountPayment > ExclusiveCurrent13thMonthAmount
            End Get
        End Property

        Public ReadOnly Property IsExcessiveBalancePayment As Boolean
            Get
                Return AmountPayment > Balance
            End Get
        End Property

        Public ReadOnly Property MaxAvailablePayment As Decimal
            Get
                Dim totalBalanceLeft = Balance

                Dim insufficientToPayMinimumDeductionAmount = ExclusiveCurrent13thMonthAmount < LoanDeductionAmount
                If (insufficientToPayMinimumDeductionAmount) Then Return ExclusiveCurrent13thMonthAmount

                Dim sufficientToPayBalance = ExclusiveCurrent13thMonthAmount - totalBalanceLeft > -1
                If (sufficientToPayBalance) Then
                    Return totalBalanceLeft
                Else
                    Return ExclusiveCurrent13thMonthAmount
                End If
            End Get
        End Property

        Public Sub SetValidAmountPayment()
            AmountPayment = ModDivision(MaxAvailablePayment, LoanDeductionAmount)
        End Sub

        Private Function ModDivision(dividend As Decimal, divisor As Decimal) As Decimal
            Dim remainder = dividend Mod divisor
            If dividend - remainder = 0 Then
                Return dividend
            Else
                Return dividend - remainder
            End If
        End Function

        Public ReadOnly Property HasChanges As Boolean
            Get
                Return _originalAmountPayment <> AmountPayment
            End Get
        End Property

        Public Function Export() As LoanPaymentFromThirteenthMonthPay
            If Not IsNew Then
                LoanPayment.AmountPayment = AmountPayment
                LoanPayment.LastUpdBy = z_User
                Return LoanPayment
            Else
                Return New LoanPaymentFromThirteenthMonthPay() With {
                    .AmountPayment = AmountPayment,
                    .CreatedBy = z_User,
                    .LoanId = _loan.RowID.Value,
                    .OrganizationID = z_OrganizationID,
                    .PaystubId = _paystub.RowID.Value}
            End If
        End Function

        Friend Sub UpdateCurrentThirteenthMonthBalance(amount As Decimal)
            _ExclusiveCurrent13thMonthAmount = amount
        End Sub

    End Class

End Class
