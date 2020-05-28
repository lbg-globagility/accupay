Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports Microsoft.Extensions.DependencyInjection

Namespace Global.AccuPay

    Public Class ViewLeaveLedgerDialog

        Private ReadOnly _employee As Employee

        Private _leaveLedgers As ICollection(Of LeaveLedger)

        Private _leaveTypes As ICollection(Of Product)

        Private _currentLeaveType As Product

        Private _leaveLedgerRepository As LeaveLedgerRepository

        Private _productRepository As ProductRepository

        Public Sub New(employee As Employee)
            InitializeComponent()
            _employee = employee

            _leaveLedgerRepository = MainServiceProvider.GetRequiredService(Of LeaveLedgerRepository)
            _productRepository = MainServiceProvider.GetRequiredService(Of ProductRepository)
        End Sub

        Private Async Sub ViewLeaveLedgerDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            TransactionsDataGridView.AutoGenerateColumns = False

            Try
                _leaveTypes = (Await _productRepository.GetLeaveTypesAsync(z_OrganizationID)).
                    OrderByDescending(Function(t) t.PartNo).
                    ToList()

                Dim trackedLeaves = {ProductConstant.SICK_LEAVE, ProductConstant.VACATION_LEAVE}
                _leaveTypes = _leaveTypes.Where(Function(l) trackedLeaves.Contains(l.PartNo)).ToList()

                ViewLeaveLedgerTypeSelector.LeaveTypes = _leaveTypes

                _leaveLedgers = Await _leaveLedgerRepository.GetAllByEmployee(_employee.RowID)

                LoadTransactions()
            Catch ex As Exception

            End Try
        End Sub

        Private Sub ViewLeaveLedgerTypeSelector_SelectionChanged(leaveType As Product) Handles ViewLeaveLedgerTypeSelector.SelectionChanged
            _currentLeaveType = leaveType
            LoadTransactions()
        End Sub

        Private Async Sub LoadTransactions()
            Dim ledger = _leaveLedgers?.FirstOrDefault(Function(l) CBool(l.ProductID.Value = _currentLeaveType.RowID))

            If ledger Is Nothing Then
                Return
            End If

            Dim leaveTransactions = Await _leaveLedgerRepository.GetTransactionsByLedger(ledger.RowID)
            Dim models = leaveTransactions.
                Select(Function(t)
                           Dim model = New LeaveTransactionModel With {
                               .TransactionDate = t.TransactionDate,
                               .Description = t.Description,
                               .Balance = t.Balance
                           }

                           model.Credit = If(t.Type = LeaveTransactionType.Credit, t.Amount, 0)
                           model.Debit = If(t.Type = LeaveTransactionType.Debit, -t.Amount, 0)

                           Return model
                       End Function).
                ToList()

            TransactionsDataGridView.DataSource = models
        End Sub

        Private Class LeaveTransactionModel

            Public Property TransactionDate As Date

            Public Property Description As String

            Public Property Credit As Decimal

            Public Property Debit As Decimal

            Public Property Balance As Decimal

        End Class

    End Class

End Namespace