Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports log4net
Imports Microsoft.EntityFrameworkCore

Namespace Benchmark

    Public Class BenchmarkPayrollHelper

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

            Using context As New PayrollContext

                'get category
                Dim loanTypeCategoryId As Integer? = Await GetCategoryId(ProductConstant.LOAN_TYPE_CATEGORY, context)

                'get Ids from product table

                _pagibigLoanId = (Await context.Products.
                                        Where(Function(p) CBool(p.CategoryID.Value = loanTypeCategoryId)).
                                        Where(Function(p) p.PartNo = ProductConstant.PAG_IBIG_LOAN).
                                        FirstOrDefaultAsync)?.RowID

                _sssLoanId = (Await context.Products.
                                        Where(Function(p) CBool(p.CategoryID.Value = loanTypeCategoryId)).
                                        Where(Function(p) p.PartNo = ProductConstant.SSS_LOAN).
                                        FirstOrDefaultAsync)?.RowID

                If _pagibigLoanId Is Nothing OrElse _sssLoanId Is Nothing Then

                    logger.Error("Pagibig or SSS loan Id were not found in the database.")

                    Return False

                End If

            End Using

            Return True

        End Function

        Private Async Function InitializeAdjustmentsLists() As Task(Of Boolean)

            Dim adjustmentTypeCategory = "Adjustment Type"

            Using context As New PayrollContext

                Dim loanTypeCategoryId As Integer? = Await GetCategoryId(adjustmentTypeCategory, context)

                If loanTypeCategoryId Is Nothing Then Return False

                _deductionList = Await context.Products.
                                        Where(Function(p) p.CategoryID.Value = loanTypeCategoryId.Value).
                                        Where(Function(p) p.Description = ProductConstant.ADJUSTMENT_TYPE_DEDUCTION).
                                        ToListAsync

                _incomeList = Await context.Products.
                                        Where(Function(p) p.CategoryID.Value = loanTypeCategoryId.Value).
                                        Where(Function(p) p.Description = ProductConstant.ADJUSTMENT_TYPE_ADDITION).
                                        ToListAsync

                Return True

            End Using

        End Function

        Private Shared Async Function GetCategoryId(loanTypeCategory As String, context As PayrollContext) As Task(Of Integer?)
            Return (Await context.Categories.
                                                           Where(Function(c) c.CategoryName = loanTypeCategory).
                                                           Where(Function(c) c.OrganizationID = z_OrganizationID).
                                                           FirstOrDefaultAsync)?.RowID
        End Function

    End Class

End Namespace