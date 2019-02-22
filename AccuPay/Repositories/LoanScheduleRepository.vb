Option Strict On
Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Loans
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class LoanScheduleRepository

        Public Shared STATUS_IN_PROGRESS As String = "In Progress"
        Public Shared STATUS_ON_HOLD As String = "On hold"
        Public Shared STATUS_CANCELLED As String = "Cancelled"
        Public Shared STATUS_COMPLETE As String = "Complete"


        Public Function GetStatusList() As List(Of String)
            Return New List(Of String) From {
                    STATUS_IN_PROGRESS,
                    STATUS_ON_HOLD,
                    STATUS_CANCELLED,
                    STATUS_COMPLETE
            }
        End Function

        Public Async Function GetByEmployeeAsync(employeeId As Integer?) As _
            Threading.Tasks.Task(Of IEnumerable(Of LoanSchedule))

            Using context = New PayrollContext()

                Return Await context.LoanSchedules.
                        Where(Function(l) Nullable.Equals(l.EmployeeID, employeeId)).
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

        Private Shared Function GetLoanTypeId(loanTypes As IEnumerable(Of Product), status As String) As Integer?
            Return loanTypes.
                            FirstOrDefault(Function(l) l.PartNo.
                                            Equals(status, StringComparison.InvariantCultureIgnoreCase))?.
                                            RowID
        End Function

        Public Async Function SaveManyAsync(currentLoanSchedules As List(Of LoanSchedule), loanTypes As IEnumerable(Of Product)) As Task

            Using context As New PayrollContext

                For Each loanSchedule In currentLoanSchedules

                    Await Me.SaveAsync(loanSchedule, loanTypes)

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

                Dim exception As New ArgumentException("Loan schedule is already completed!")
                Throw exception
                'has error serializable
                'exception.Data("loanSchedule") = loanSchedule

            End If

            'sanitize columns
            loanSchedule.TotalLoanAmount = AccuMath.CommercialRound(loanSchedule.TotalLoanAmount)
            loanSchedule.TotalBalanceLeft = AccuMath.CommercialRound(loanSchedule.TotalBalanceLeft)
            loanSchedule.DeductionAmount = AccuMath.CommercialRound(loanSchedule.DeductionAmount)
            loanSchedule.DeductionPercentage = AccuMath.CommercialRound(loanSchedule.DeductionPercentage)

            loanSchedule.NoOfPayPeriod = AccuMath.CommercialRound(loanSchedule.NoOfPayPeriod)
            loanSchedule.LoanPayPeriodLeft = CType(AccuMath.CommercialRound(loanSchedule.LoanPayPeriodLeft), Integer)

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

        Public Function ComputeNumberOfPayPeriod(totalLoanAmount As Decimal, deductionAmount As Decimal) As Integer

            Return CType(AccuMath.CommercialRound(totalLoanAmount / deductionAmount), Integer)

        End Function

        Public Function ComputeNumberOfPayPeriodLeft(totalBalanceLeft As Decimal, deductionAmount As Decimal) As Integer

            Return CType(AccuMath.CommercialRound(totalBalanceLeft / deductionAmount), Integer)

        End Function


        Private Sub Insert(
            loanSchedule As LoanSchedule,
            loanTypes As IEnumerable(Of Product),
            context As PayrollContext)

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


            loanSchedule.Created = Date.Now
            loanSchedule.CreatedBy = z_User

            context.LoanSchedules.Add(loanSchedule)

        End Sub

        Private Async Function UpdateAsync(
            newLoanSchedule As LoanSchedule,
            context As PayrollContext) As Task

            'if cancelled na yung loan, hindi pwede ma update
            If (newLoanSchedule.Status = STATUS_CANCELLED) Then

                Throw New ArgumentException("Loan schedule is already cancelled!")

            End If

            If newLoanSchedule.TotalBalanceLeft = 0 Then
                newLoanSchedule.LoanPayPeriodLeft = 0
                newLoanSchedule.Status = STATUS_COMPLETE
            End If

            Dim oldLoanSchedule = Await Me.GetByIdAsync(newLoanSchedule.RowID)
            Dim loanTransactionsCount = Await context.LoanTransactions.
                                        CountAsync(Function(l) Nullable.Equals(l.LoanScheduleID, newLoanSchedule.RowID))

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
                    ComputeNumberOfPayPeriodLeft(newLoanSchedule.TotalBalanceLeft, newLoanSchedule.DeductionAmount)

            End If

            newLoanSchedule.LastUpd = Date.Now
            newLoanSchedule.LastUpdBy = z_User

            context.LoanSchedules.Attach(newLoanSchedule)
            context.Entry(newLoanSchedule).State = EntityState.Modified

        End Function
    End Class

End Namespace

