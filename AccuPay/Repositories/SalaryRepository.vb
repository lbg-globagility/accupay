Imports System.Threading.Tasks
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Public Class SalaryRepository

    Public Async Function GetAllByCutOff(cutoffStart As Date) As Task(Of List(Of Salary))

        Using context As New PayrollContext

            Return Await context.Salaries.
                Where(Function(s) Nullable.Equals(s.OrganizationID, z_OrganizationID)).
                Where(Function(s) s.EffectiveFrom <= cutoffStart AndAlso cutoffStart <= If(s.EffectiveTo, cutoffStart)).
                ToListAsync

        End Using

    End Function

End Class