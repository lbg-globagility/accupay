Option Strict On

Imports AccuPay.Loans
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class LoanRepository

        Public Async Function GetByEmployee(employeeId As Integer?) As _
            Threading.Tasks.Task(Of IEnumerable(Of LoanSchedule))

            Using context = New PayrollContext()
                Dim list As New List(Of LoanSchedule)

                list = Await context.LoanSchedules.
                        Where(Function(l) Nullable.Equals(l.EmployeeID, employeeId)).
                        ToListAsync

                Return list
            End Using

        End Function

    End Class

End Namespace
