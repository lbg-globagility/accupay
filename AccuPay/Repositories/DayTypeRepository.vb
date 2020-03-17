Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class DayTypeRepository

        Public Async Function GetAll() As Task(Of ICollection(Of DayType))
            Using context = New PayrollContext()
                Return Await context.DayTypes.ToListAsync()
            End Using
        End Function

    End Class

End Namespace
