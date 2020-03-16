Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class CalendarRepository

        Public Async Function GetAll() As Task(Of ICollection(Of PayCalendar))
            Using context = New PayrollContext()
                Return Await context.Calendars.ToListAsync()
            End Using
        End Function

    End Class

End Namespace
