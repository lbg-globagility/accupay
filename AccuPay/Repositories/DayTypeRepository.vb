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

        Public Async Function Save(dayType As DayType) As Task
            Using context = New PayrollContext()
                If dayType.RowID Is Nothing Then
                    context.DayTypes.Add(dayType)
                Else
                    context.Entry(dayType).State = EntityState.Modified
                End If

                Await context.SaveChangesAsync()
            End Using
        End Function

    End Class

End Namespace
