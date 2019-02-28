Option Strict On

Imports System.Threading.Tasks
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class CategoryRepository

        Public Async Function GetLoanTypeId() As Task(Of Integer?)

            Return Await GetCategoryId("Loan Type")

        End Function

#Region "Private Functions"
        Private Async Function GetCategoryId(categoryName As String) As Task(Of Integer?)

            Using context = New PayrollContext()

                Dim category = Await context.Categories.
                                Where(Function(c) Nullable.Equals(c.OrganizationID, z_OrganizationID)).
                                FirstOrDefaultAsync

                Return category?.RowID

            End Using

        End Function
#End Region

    End Class

End Namespace
