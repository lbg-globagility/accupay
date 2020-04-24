Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Helpers
Imports AccuPay.Entity
Imports AccuPay.Loans
Imports AccuPay.Utilities
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class LoanScheduleRepository

        Public Shared ReadOnly STATUS_IN_PROGRESS As String = "In Progress"
        Public Shared ReadOnly STATUS_ON_HOLD As String = "On hold"
        Public Shared ReadOnly STATUS_CANCELLED As String = "Cancelled"
        Public Shared ReadOnly STATUS_COMPLETE As String = "Complete"

        Public Function GetStatusList() As List(Of String)
            Return New List(Of String) From {
                    STATUS_IN_PROGRESS,
                    STATUS_ON_HOLD,
                    STATUS_CANCELLED,
                    STATUS_COMPLETE
            }
        End Function

        Public Async Function GetByEmployeeAsync(
            employeeId As Integer?) As _
            Task(Of IEnumerable(Of LoanSchedule))

            Using context = New PayrollContext()

                Return Await context.LoanSchedules.
                        Where(Function(l) l.EmployeeID.Value = employeeId.Value).
                        ToListAsync

            End Using

        End Function

        Public Async Function GetByEmployeeAndStatusAsync(
            employeeId As Integer?,
            Optional inProgressChecked As Boolean = True,
            Optional onHoldChecked As Boolean = True,
            Optional cancelledChecked As Boolean = True,
            Optional completeChecked As Boolean = True) As _
            Task(Of IEnumerable(Of LoanSchedule))

            Using context = New PayrollContext()

                Return Await context.LoanSchedules.
                        Where(Function(l) l.EmployeeID.Value = employeeId.Value).
                        Where(Function(l) (inProgressChecked AndAlso l.Status = STATUS_IN_PROGRESS) OrElse
                                   (onHoldChecked AndAlso l.Status = STATUS_ON_HOLD) OrElse
                                   (cancelledChecked AndAlso l.Status = STATUS_CANCELLED) OrElse
                                   (completeChecked AndAlso l.Status = STATUS_COMPLETE)).
                        ToListAsync

            End Using

        End Function

        Public Async Function GetActiveLoansByLoanNameAsync(
            loanName As String,
            employeeId As Integer?) As _
            Task(Of IEnumerable(Of LoanSchedule))

            Using context = New PayrollContext()

                Return Await context.LoanSchedules.
                        Include(Function(l) l.LoanType).
                        Include(Function(l) l.LoanType.CategoryEntity).
                        Where(Function(l) l.LoanType.CategoryEntity.CategoryName = ProductConstant.LOAN_TYPE_CATEGORY).
                        Where(Function(l) l.LoanType.PartNo.ToUpper = loanName.ToUpper).
                        Where(Function(l) l.Status = STATUS_IN_PROGRESS).
                        Where(Function(l) l.EmployeeID.Value = employeeId.Value).
                        ToListAsync
            End Using

        End Function

        Public Async Function GetByIdAsync(loanScheduleId As Integer?) As Task(Of LoanSchedule)

            Using context = New PayrollContext()

                Return Await context.LoanSchedules.
                    FirstOrDefaultAsync(Function(l) l.RowID.Value = loanScheduleId.Value)

            End Using

        End Function

        Public Async Function GetLoanTransactionsWithPayPeriod(loanScheduleId As Integer?) As _
            Threading.Tasks.Task(Of IEnumerable(Of LoanTransaction))

            Using context = New PayrollContext()

                Return Await context.LoanTransactions.
                        Include(Function(l) l.PayPeriod).
                        Where(Function(l) l.LoanScheduleID = loanScheduleId.Value).
                        ToListAsync

            End Using

        End Function

        Public Async Function SaveManyAsync(currentLoanSchedules As List(Of LoanSchedule), loanTypes As IEnumerable(Of Product)) As Task

            Using context As New PayrollContext

                For Each loanSchedule In currentLoanSchedules

                    Await Me.SaveAsync(loanSchedule, loanTypes, context)

                    Await context.SaveChangesAsync()
                Next

            End Using

        End Function

        Public Async Function SaveAsync(
            loanSchedule As LoanSchedule,
            loanTypes As IEnumerable(Of Product),
            Optional passedContext As PayrollContext = Nothing) As Task

            'if completed yung loan, hindi pwede ma i-insert or update
            If loanSchedule.Status = STATUS_COMPLETE Then

                Throw New ArgumentException("Loan schedule is already completed!")

            End If

            If String.IsNullOrWhiteSpace(loanSchedule.LoanName) Then

                Dim loanName = loanTypes.FirstOrDefault(Function(l) l.RowID.Value = loanSchedule.LoanTypeID.Value)?.PartNo

                loanSchedule.LoanName = loanName

            End If

            Await ValidationForBenchmark(loanSchedule)

            'sanitize columns
            loanSchedule.TotalLoanAmount = AccuMath.CommercialRound(loanSchedule.TotalLoanAmount)
            loanSchedule.TotalBalanceLeft = AccuMath.CommercialRound(loanSchedule.TotalBalanceLeft)
            loanSchedule.DeductionAmount = AccuMath.CommercialRound(loanSchedule.DeductionAmount)
            loanSchedule.DeductionPercentage = AccuMath.CommercialRound(loanSchedule.DeductionPercentage)

            loanSchedule.NoOfPayPeriod = AccuMath.CommercialRound(loanSchedule.NoOfPayPeriod)
            loanSchedule.LoanPayPeriodLeft = CType(AccuMath.CommercialRound(ObjectUtils.ToInteger(loanSchedule.LoanPayPeriodLeft)), Integer)

            loanSchedule.OrganizationID = z_OrganizationID

            'add or update the loanSchedule
            If passedContext Is Nothing Then
                Using newContext As New PayrollContext
                    If loanSchedule.RowID Is Nothing OrElse loanSchedule.RowID = Integer.MinValue Then
                        Me.Insert(loanSchedule, loanTypes, newContext)
                    Else
                        Await Me.UpdateAsync(loanSchedule, newContext)
                    End If

                    Await newContext.SaveChangesAsync()
                End Using
            Else
                If loanSchedule.RowID Is Nothing OrElse loanSchedule.RowID = Integer.MinValue Then
                    Me.Insert(loanSchedule, loanTypes, passedContext)
                Else
                    Await Me.UpdateAsync(loanSchedule, passedContext)
                End If
            End If
        End Function

        Private Async Function ValidationForBenchmark(loanSchedule As LoanSchedule) As Task
            Dim sys_ownr As New SystemOwner
            If sys_ownr.CurrentSystemOwner = SystemOwner.Benchmark Then

                'IF benchmark
                '#1. Only Pagibig loan or SSS loan can be saved
                '#2. Only one active Pagibig or SSS loan is allowed.

                '#1
                If loanSchedule.LoanName <> ProductConstant.PAG_IBIG_LOAN AndAlso
                        loanSchedule.LoanName <> ProductConstant.SSS_LOAN Then

                    Throw New ArgumentException("Only PAGIBIG and SSS loan are allowed!")

                End If

                '#2
                If loanSchedule.Status = STATUS_IN_PROGRESS Then

                    Dim sameActiveLoans = Await GetActiveLoansByLoanNameAsync(loanSchedule.LoanName, loanSchedule.EmployeeID)

                    'if insert, check if there are any sameActiveLoans
                    'if update, check if there are any sameActiveLoans that is not the currently updated loan schedule
                    If (loanSchedule.RowID Is Nothing AndAlso sameActiveLoans.Any) OrElse
                            (loanSchedule.RowID.HasValue AndAlso sameActiveLoans.Where(Function(l) l.RowID.Value <> loanSchedule.RowID.Value).Any) Then

                        Throw New ArgumentException("Only one active PAGIBIG and one active SSS loan are allowed!")

                    End If
                End If
            End If
        End Function

        Public Async Function GetTotalLoanTransactions(loanSchedules As List(Of LoanSchedule)) As Task(Of Integer)

            If loanSchedules.Count > 0 Then
                Return 0
            End If

            Dim loanScheduleRowIdArray(loanSchedules.Count - 1) As Integer?

            For index = 0 To loanSchedules.Count - 1
                loanScheduleRowIdArray(index) = loanSchedules(index).RowID
            Next

            Using context As New PayrollContext
                Return Await context.LoanTransactions.
                        CountAsync(Function(l) loanScheduleRowIdArray.Contains(l.RowID))
            End Using
        End Function

        Public Async Function DeleteAsync(loanScheduleId As Integer?) As Task
            Using context = New PayrollContext()

                Dim loanSchedule = Await GetByIdAsync(loanScheduleId)

                context.Remove(loanSchedule)

                Await context.SaveChangesAsync()

            End Using
        End Function

        Public Function ComputeNumberOfPayPeriod(totalLoanAmount As Decimal, deductionAmount As Decimal) As Integer

            If deductionAmount = 0 Then Return 0

            If deductionAmount > totalLoanAmount Then Return 1

            Return CType(Math.Ceiling(totalLoanAmount / deductionAmount), Integer)

        End Function

        Private Sub Insert(
            loanSchedule As LoanSchedule,
            loanTypes As IEnumerable(Of Product),
            context As PayrollContext)

            loanSchedule.LoanPayPeriodLeft =
                    ComputeNumberOfPayPeriod(loanSchedule.TotalBalanceLeft, loanSchedule.DeductionAmount)

            If loanSchedule.LoanPayPeriodLeft < 1 Then
                loanSchedule.Status = STATUS_COMPLETE
            End If

            If loanSchedule.LoanNumber Is Nothing Then
                loanSchedule.LoanNumber = ""
            End If

            loanSchedule.Created = Date.Now
            loanSchedule.CreatedBy = z_User

            context.LoanSchedules.Add(loanSchedule)

        End Sub

        Private Async Function UpdateAsync(
            newLoanSchedule As LoanSchedule,
            context As PayrollContext) As Task

            Dim oldLoanSchedule = Await Me.GetByIdAsync(newLoanSchedule.RowID)
            Dim loanTransactionsCount = Await context.LoanTransactions.
                                        CountAsync(Function(l) l.LoanScheduleID = newLoanSchedule.RowID.Value)

            'if cancelled na yung loan, hindi pwede ma update
            If (oldLoanSchedule.Status = STATUS_CANCELLED) Then

                Throw New ArgumentException("Loan schedule is already cancelled!")

            End If

            If newLoanSchedule.TotalBalanceLeft = 0 Then
                newLoanSchedule.LoanPayPeriodLeft = 0
                newLoanSchedule.Status = STATUS_COMPLETE
            End If

            'if nag start ng magbawas ng loan, dapat hindi na pwede ma edit ang TotalLoanAmount
            If oldLoanSchedule.TotalBalanceLeft <> oldLoanSchedule.TotalLoanAmount OrElse
                loanTransactionsCount > 0 Then

                newLoanSchedule.TotalLoanAmount = oldLoanSchedule.TotalLoanAmount

                'recompute NoOfPayPeriod if TotalLoanAmount changed
                newLoanSchedule.NoOfPayPeriod =
                    ComputeNumberOfPayPeriod(newLoanSchedule.TotalLoanAmount, newLoanSchedule.DeductionAmount)
            End If

            If newLoanSchedule.TotalBalanceLeft > newLoanSchedule.TotalLoanAmount Then

                newLoanSchedule.TotalBalanceLeft = oldLoanSchedule.TotalLoanAmount
                'recompute LoanPayPeriodLeft if TotalBalanceLeft changed

                newLoanSchedule.LoanPayPeriodLeft =
                    ComputeNumberOfPayPeriod(newLoanSchedule.TotalBalanceLeft, newLoanSchedule.DeductionAmount)

            End If

            newLoanSchedule.LastUpdBy = z_User

            context.LoanSchedules.Attach(newLoanSchedule)
            context.Entry(newLoanSchedule).State = EntityState.Modified

        End Function

    End Class

End Namespace