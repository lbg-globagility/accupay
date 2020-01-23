Option Strict On

Imports System.Threading.Tasks
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class OfficialBusinessRepository

        Public Async Function GetByEmployeeAsync(
            employeeId As Integer?) As _
            Task(Of IEnumerable(Of OfficialBusiness))

            Using context = New PayrollContext()

                Return Await context.OfficialBusinesses.
                        Where(Function(l) l.EmployeeID.Value = employeeId.Value).
                        ToListAsync

            End Using

        End Function

    End Class

End Namespace