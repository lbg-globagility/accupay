Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class AllowanceRepository

        Public Function GetFrequencyList() As List(Of String)
            Return New List(Of String) From {
                    Allowance.FREQUENCY_ONE_TIME,
                    Allowance.FREQUENCY_DAILY,
                    Allowance.FREQUENCY_SEMI_MONTHLY,
                    Allowance.FREQUENCY_MONTHLY
            }
        End Function

        Public Async Function GetByEmployeeIncludesProductAsync(
            employeeId As Integer?) As _
            Task(Of IEnumerable(Of Allowance))

            Using context = New PayrollContext()

                Return Await context.Allowances.
                        Include(Function(p) p.Product).
                        Where(Function(l) Nullable.Equals(l.EmployeeID, employeeId)).
                        ToListAsync

            End Using

        End Function

        Public Async Function GetByIdAsync(allowanceId As Integer?) As Task(Of Allowance)

            Using context = New PayrollContext()

                Return Await context.Allowances.
                    FirstOrDefaultAsync(Function(l) Nullable.Equals(l.RowID, allowanceId))

            End Using

        End Function

        Public Async Function CheckIfAlreadyUsed(allowanceId As Integer?) As Task(Of Boolean)

            Using context = New PayrollContext()

                Return Await context.AllowanceItems.
                    AnyAsync(Function(a) Nullable.Equals(a.AllowanceID, allowanceId))

            End Using

        End Function

        Public Async Function DeleteAsync(allowanceId As Integer?) As Task
            Using context = New PayrollContext()

                Dim allowance = Await GetByIdAsync(allowanceId)

                context.Remove(allowance)

                Await context.SaveChangesAsync()

            End Using
        End Function

        Public Async Function SaveManyAsync(currentAllowances As List(Of Allowance)) As Task

            Using context As New PayrollContext

                For Each allowance In currentAllowances

                    Await Me.SaveAsync(allowance, context)

                    Await context.SaveChangesAsync()
                Next

            End Using

        End Function

        Public Async Function SaveAsync(
            allowance As Allowance,
            Optional passedContext As PayrollContext = Nothing) As Task

            'remove the product so it won't override the saving of ProductID
            Dim newAllowance = allowance.CloneJson()
            newAllowance.Product = Nothing

            newAllowance.OrganizationID = z_OrganizationID

            'add or update the allowance
            If passedContext Is Nothing Then
                Using newContext As New PayrollContext
                    If newAllowance.RowID Is Nothing Then
                        Me.Insert(newAllowance, newContext)
                    Else
                        Me.Update(newAllowance, newContext)
                    End If

                    Await newContext.SaveChangesAsync()
                End Using
            Else
                If newAllowance.RowID Is Nothing Then
                    Me.Insert(newAllowance, passedContext)
                Else
                    Me.Update(newAllowance, passedContext)
                End If
            End If
        End Function

        Public Function GetAllowancesWithPayPeriodBaseQuery(context As PayrollContext, _payDateFrom As Date, _payDateTo As Date) _
            As IQueryable(Of Allowance)

            ' Retrieve all allowances whose begin and end date spans the cutoff dates.
            Return context.Allowances.
                Include(Function(a) a.Product).
                Where(Function(a) a.OrganizationID.Value = z_OrganizationID).
                Where(Function(a) a.EffectiveStartDate <= _payDateTo).
                Where(Function(a) If(a.EffectiveEndDate Is Nothing, True, _payDateFrom <= a.EffectiveEndDate.Value))
        End Function

        Private Sub Insert(
            allowance As Allowance,
            context As PayrollContext)

            allowance.CreatedBy = z_User

            context.Allowances.Add(allowance)

        End Sub

        Private Sub Update(
            allowance As Allowance,
            context As PayrollContext)

            allowance.LastUpdBy = z_User

            context.Allowances.Attach(allowance)
            context.Entry(allowance).State = EntityState.Modified

        End Sub

    End Class

End Namespace