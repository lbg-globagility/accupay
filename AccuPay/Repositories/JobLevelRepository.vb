Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.JobLevels
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class JobLevelRepository

        Public Async Function GetAllAsync() As Task(Of List(Of JobLevel))

            Using context As New PayrollContext

                Return Await context.JobLevels.
                            Where(Function(j) Nullable.Equals(j.OrganizationID, z_OrganizationID)).
                            ToListAsync

            End Using

        End Function

    End Class

End Namespace
