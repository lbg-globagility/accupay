Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports Microsoft.Extensions.DependencyInjection

Public Class LoanBreakdownDialog

    Private ReadOnly _paystubRepository As IPaystubRepository
    Private ReadOnly _paystubId As Integer

    Public Sub New(paystubId As Integer)
        InitializeComponent()

        _paystubRepository = MainServiceProvider.GetRequiredService(Of IPaystubRepository)

        _paystubId = paystubId
    End Sub

    Private Async Sub ViewLoanBreakdown_Load(sender As Object, e As EventArgs) Handles Me.Load
        LoanGridView.AutoGenerateColumns = False

        Await LoadLoanBreakdown()
    End Sub

    Private Async Function LoadLoanBreakdown() As Task
        Dim loanTransactionsList = Await _paystubRepository.
            GetLoanTransactionsAsync(_paystubId)

        Dim loanTransactions = loanTransactionsList.
            Select(Function(lt) New LoanTransactionDto(lt)).
            OrderBy(Function(b) b.LoanType).
            ThenBy(Function(b) b.DeductionSchedule).
            ThenBy(Function(b) b.LoanNumber).
            ThenBy(Function(b) b.TotalAmount).
            ThenBy(Function(b) b.Balance).
            ToList()

        LoanGridView.DataSource = loanTransactions
    End Function

    Private Class LoanTransactionDto

        Public Sub New(loanTransaction As LoanTransaction)
            _LoanNumber = loanTransaction.Loan.LoanNumber
            _LoanType = loanTransaction.Loan.LoanType.DisplayName
            _TotalAmount = loanTransaction.Loan.TotalLoanAmount
            _DeductionAmount = $"({FormatNumber(loanTransaction.DeductionAmount, 2)})"
            _Balance = loanTransaction.TotalBalance
            _DeductionSchedule = loanTransaction.Loan.DeductionSchedule
            _PayPeriodsLeft = loanTransaction.LoanPayPeriodLeft
        End Sub

        Public ReadOnly Property LoanNumber As String
        Public ReadOnly Property LoanType As String
        Public ReadOnly Property TotalAmount As Decimal
        Public ReadOnly Property DeductionAmount As String
        Public ReadOnly Property Balance As Decimal
        Public ReadOnly Property DeductionSchedule As String
        Public ReadOnly Property PayPeriodsLeft As Decimal
    End Class

End Class
