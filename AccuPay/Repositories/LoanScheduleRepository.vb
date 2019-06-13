Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Loans
Imports AccuPay.Utils
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
                        Where(Function(l) Nullable.Equals(l.EmployeeID, employeeId)).
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
                        Where(Function(l) Nullable.Equals(l.EmployeeID, employeeId)).
                        Where(Function(l) (inProgressChecked AndAlso l.Status = STATUS_IN_PROGRESS) OrElse
                                   (onHoldChecked AndAlso l.Status = STATUS_ON_HOLD) OrElse
                                   (cancelledChecked AndAlso l.Status = STATUS_CANCELLED) OrElse
                                   (completeChecked AndAlso l.Status = STATUS_COMPLETE)).
                        ToListAsync

            End Using

        End Function

        Public Async Function GetByIdAsync(loanScheduleId As Integer?) As Task(Of LoanSchedule)

            Using context = New PayrollContext()

                Return Await context.LoanSchedules.
                    FirstOrDefaultAsync(Function(l) Nullable.Equals(l.RowID, loanScheduleId))

            End Using

        End Function

        Public Async Function GetLoanTransactionsWithPayPeriod(loanScheduleId As Integer?) As _
            Threading.Tasks.Task(Of IEnumerable(Of LoanTransaction))

            Using context = New PayrollContext()

                Return Await context.LoanTransactions.
                        Include(Function(l) l.PayPeriod).
                        Where(Function(l) Nullable.Equals(l.LoanScheduleID, loanScheduleId)).
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
                    If loanSchedule.RowID Is Nothing Then
                        Me.Insert(loanSchedule, loanTypes, newContext)
                    Else
                        Await Me.UpdateAsync(loanSchedule, newContext)
                    End If

                    Await newContext.SaveChangesAsync()
                End Using
            Else
                If loanSchedule.RowID Is Nothing Then
                    Me.Insert(loanSchedule, loanTypes, passedContext)
                Else
                    Await Me.UpdateAsync(loanSchedule, passedContext)
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

            If String.IsNullOrWhiteSpace(loanSchedule.LoanName) Then

                Dim loanName = loanTypes.FirstOrDefault(Function(l) Nullable.Equals(l.RowID, loanSchedule.LoanTypeID))?.PartNo

                loanSchedule.LoanName = loanName

            End If

            loanSchedule.CreatedBy = z_User

            context.LoanSchedules.Add(loanSchedule)

        End Sub

        Private Async Function UpdateAsync(
            newLoanSchedule As LoanSchedule,
            context As PayrollContext) As Task

            Dim oldLoanSchedule = Await Me.GetByIdAsync(newLoanSchedule.RowID)
            Dim loanTransactionsCount = Await context.LoanTransactions.
                                        CountAsync(Function(l) Nullable.Equals(l.LoanScheduleID, newLoanSchedule.RowID))

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