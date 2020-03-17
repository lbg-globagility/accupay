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

        Public Async Function GetPayRates(calendarID As Integer,
                                          from As Date,
                                          [to] As Date) As Task(Of ICollection(Of PayRate))
            Using context = New PayrollContext()
                Return Await context.PayRates.
                    Where(Function(p) from <= p.Date AndAlso p.Date <= [to]).
                    Where(Function(p) p.OrganizationID.Value = calendarID).
                    ToListAsync()
            End Using
        End Function

    End Class

End Namespace
