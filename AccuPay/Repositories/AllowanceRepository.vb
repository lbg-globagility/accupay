Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Utilities.Extensions
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

        Public Async Function GetByIdAsync(id As Integer?) As Task(Of Allowance)

            Using context = New PayrollContext()

                Return Await context.Allowances.
                    FirstOrDefaultAsync(Function(l) l.RowID.Value = id.Value)

            End Using

        End Function

        Public Async Function CheckIfAlreadyUsed(id As Integer?) As Task(Of Boolean)

            Using context = New PayrollContext()

                Return Await context.AllowanceItems.
                    AnyAsync(Function(a) Nullable.Equals(a.AllowanceID, id))

            End Using

        End Function

        Public Async Function DeleteAsync(id As Integer?) As Task
            Using context = New PayrollContext()

                Dim allowance = Await GetByIdAsync(id)

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
                    Await SaveAsyncFunction(newAllowance, newContext)
                End Using
            Else
                Await SaveAsyncFunction(newAllowance, passedContext)
            End If
        End Function

        Private Async Function SaveAsyncFunction(newAllowance As Allowance, context As PayrollContext) As Task

            If newAllowance.ProductID Is Nothing Then
                Throw New ArgumentException("Allowance type cannot be empty.")
            End If

            Dim product = Await context.Products.
                                    Where(Function(p) p.RowID.Value = newAllowance.ProductID.Value).
                                    FirstOrDefaultAsync()

            If product Is Nothing Then
                Throw New ArgumentException("The selected allowance type no longer exists. Please close then reopen the form to view the latest data.")
            End If

            If newAllowance.IsMonthly AndAlso Not product.Fixed Then
                Throw New ArgumentException("Only fixed allowance type are allowed for Monthly allowances.")
            End If

            If newAllowance.RowID Is Nothing Then
                Me.Insert(newAllowance, context)
            Else
                Me.Update(newAllowance, context)
            End If

            Await context.SaveChangesAsync()
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