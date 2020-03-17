Option Strict On

Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class CalendarRepository

        Public Async Function GetById(calendarID As Integer) As Task(Of PayCalendar)
            Using context = New PayrollContext()
                Return Await context.Calendars.
                    Where(Function(c) c.RowID.Value = calendarID).
                    FirstOrDefaultAsync()
            End Using
        End Function

        Public Async Function GetAll() As Task(Of ICollection(Of PayCalendar))
            Using context = New PayrollContext()
                Return Await context.Calendars.ToListAsync()
            End Using
        End Function

        ''' <summary>
        ''' Gets all payrates of a calendar that is part of a given year
        ''' </summary>
        ''' <param name="calendarID"></param>
        ''' <param name="year"></param>
        ''' <returns></returns>
        Public Async Function GetPayRates(calendarID As Integer,
                                          year As Integer) As Task(Of ICollection(Of PayRate))
            Dim firstDayOfYear = New Date(year, 1, 1)
            Dim lastDayOfYear = New Date(year, 12, 31)

            Return Await GetPayRates(calendarID, firstDayOfYear, lastDayOfYear)
        End Function

        ''' <summary>
        ''' Gets all payrates of a calendar that is from and to the given date range
        ''' </summary>
        ''' <param name="calendarID"></param>
        ''' <param name="from"></param>
        ''' <param name="[to]"></param>
        ''' <returns></returns>
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

        ''' <summary>
        ''' Gets all payrates of a calendar
        ''' </summary>
        ''' <param name="calendarID"></param>
        ''' <returns></returns>
        Public Async Function GetPayRates(calendarID As Integer) As Task(Of ICollection(Of PayRate))
            Using context = New PayrollContext()
                Return Await context.PayRates.
                    Where(Function(p) p.OrganizationID.Value = calendarID).
                    ToListAsync()
            End Using
        End Function

        Public Async Function Create(calendar As PayCalendar, copiedCalendar As PayCalendar) As Task
            Using context = New PayrollContext()

                Dim transaction = Await context.Database.BeginTransactionAsync()

                Try
                    context.Calendars.Add(calendar)
                    Await context.SaveChangesAsync()

                    Dim copiedPayrates = Await GetPayRates(copiedCalendar.RowID.Value)

                    Dim newPayrates = New Collection(Of PayRate)
                    For Each copiedPayrate In copiedPayrates
                        Dim payrate = New PayRate()
                        payrate.CalendarID = calendar.RowID
                        payrate.Date = copiedPayrate.Date
                        payrate.PayType = copiedPayrate.PayType
                        payrate.Description = copiedPayrate.Description
                        payrate.DayBefore = copiedPayrate.DayBefore

                        newPayrates.Add(payrate)
                    Next

                    context.PayRates.AddRange(newPayrates)
                    Await context.SaveChangesAsync()

                    transaction.Commit()
                Catch ex As Exception
                    transaction.Rollback()
                    Throw
                End Try
            End Using
        End Function

    End Class

End Namespace
