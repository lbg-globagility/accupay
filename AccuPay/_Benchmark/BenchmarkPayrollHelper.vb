Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Repository
Imports log4net

Namespace Benchmark

    Public Class BenchmarkPayrollHelper

        Private _productRepository As ProductRepository

        Private _pagibigLoanId As Integer?
        Private _sssLoanId As Integer?

        Private _deductionList As List(Of Product)

        Public ReadOnly Property DeductionList() As List(Of Product)
            Get
                Return _deductionList
            End Get
        End Property

        Private _incomeList As List(Of Product)

        Public ReadOnly Property IncomeList() As List(Of Product)
            Get
                Return _incomeList
            End Get
        End Property

        Private Sub New()

            _productRepository = New ProductRepository

        End Sub

        Public Shared Async Function GetInstance(logger As ILog) As Task(Of BenchmarkPayrollHelper)

            Dim helper = New BenchmarkPayrollHelper()

            If Await helper.Initialize(logger) = False Then

                Return Nothing

            End If

            Return helper

        End Function

        ''' <summary>
        ''' Makes sure that Accupay does not have unneccesary data for benchmark.
        ''' </summary>
        Public Async Function CleanEmployee(employeeId As Integer) As Task

            Using context As New PayrollContext

                'delete all loans that are not HDMF or SSS
                'only HDMF or SSS loans are supported in benchmark
                context.LoanSchedules.
                        RemoveRange(context.LoanSchedules.
                                    Where(Function(l) l.EmployeeID.Value = employeeId).
                                    Where(Function(l) l.LoanTypeID.Value <> _pagibigLoanId.Value).
                                    Where(Function(l) l.LoanTypeID.Value <> _sssLoanId.Value))

                Await context.SaveChangesAsync
            End Using

        End Function

        Public Async Function Initialize(logger As ILog) As Task(Of Boolean)

            If Await InitializeLoanIds(logger) = False OrElse
                Await InitializeAdjustmentsLists() = False Then

                Return False

            End If

            Return True

        End Function

        Private Async Function InitializeLoanIds(logger As ILog) As Task(Of Boolean)

            Dim govermentLoans = Await _productRepository.GetGovernmentLoanTypes()

            _pagibigLoanId = govermentLoans.FirstOrDefault(Function(l) l.IsPagibigLoan)?.RowID
            _sssLoanId = govermentLoans.FirstOrDefault(Function(l) l.IsSssLoan)?.RowID

            If _pagibigLoanId Is Nothing OrElse _sssLoanId Is Nothing Then

                logger.Error("Pagibig or SSS loan Id were not found in the database.")

                Return False

            End If

            Return True

        End Function

        Private Async Function InitializeAdjustmentsLists() As Task(Of Boolean)

            _deductionList = New List(Of Product)(Await _productRepository.GetDeductionAdjustmentTypes())
            _incomeList = New List(Of Product)(Await _productRepository.GetAdditionAdjustmentTypes())

            If _deductionList Is Nothing OrElse _incomeList Is Nothing Then Return False

            Return True

        End Function

    End Class

End Namespace