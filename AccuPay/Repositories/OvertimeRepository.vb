Option Strict On

Imports System.Threading.Tasks
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class OvertimeRepository

        Public Function GetStatusList() As List(Of String)
            Return New List(Of String) From {
                    Overtime.StatusPending,
                    Overtime.StatusApproved
            }
        End Function

        Public Async Function GetByEmployeeAsync(
            employeeId As Integer?) As _
            Task(Of IEnumerable(Of Overtime))

            Using context = New PayrollContext()

                Return Await context.Overtimes.
                        Where(Function(l) l.EmployeeID.Value = employeeId.Value).
                        ToListAsync

            End Using

        End Function

    End Class

End Namespace