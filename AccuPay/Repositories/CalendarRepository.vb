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
        ''' Gets all days of a calendar that is part of a given year
        ''' </summary>
        ''' <param name="calendarID"></param>
        ''' <param name="year"></param>
        ''' <returns></returns>
        Public Async Function GetCalendarDays(calendarID As Integer,
                                              year As Integer) As Task(Of ICollection(Of CalendarDay))
            Dim firstDayOfYear = New Date(year, 1, 1)
            Dim lastDayOfYear = New Date(year, 12, 31)

            Return Await GetCalendarDays(calendarID, firstDayOfYear, lastDayOfYear)
        End Function

        ''' <summary>
        ''' Gets all days of a calendar that is from and to the given date range
        ''' </summary>
        ''' <param name="calendarID"></param>
        ''' <param name="from"></param>
        ''' <param name="[to]"></param>
        ''' <returns></returns>
        Public Async Function GetCalendarDays(calendarID As Integer,
                                              from As Date,
                                              [to] As Date) As Task(Of ICollection(Of CalendarDay))
            Using context = New PayrollContext()
                Return Await context.CalendarDays.
                    Include(Function(t) t.DayType).
                    Where(Function(t) from <= t.Date AndAlso t.Date <= [to]).
                    Where(Function(t) t.CalendarID.Value = calendarID).
                    ToListAsync()
            End Using
        End Function

        ''' <summary>
        ''' Gets all days of a calendar
        ''' </summary>
        ''' <param name="calendarID"></param>
        ''' <returns></returns>
        Public Async Function GetCalendarDays(calendarID As Integer) As Task(Of ICollection(Of CalendarDay))
            Using context = New PayrollContext()
                Return Await context.CalendarDays.
                    Include(Function(t) t.DayType).
                    Where(Function(t) t.CalendarID.Value = calendarID).
                    ToListAsync()
            End Using
        End Function

        Public Async Function Create(calendar As PayCalendar, copiedCalendar As PayCalendar) As Task
            Using context = New PayrollContext()

                Dim transaction = Await context.Database.BeginTransactionAsync()

                Try
                    context.Calendars.Add(calendar)
                    Await context.SaveChangesAsync()

                    Dim copiedDays = Await GetCalendarDays(copiedCalendar.RowID.Value)

                    Dim newDays = New Collection(Of CalendarDay)
                    For Each copiedDay In copiedDays
                        Dim day = New CalendarDay()
                        day.CalendarID = calendar.RowID
                        day.Date = copiedDay.Date
                        day.DayTypeID = copiedDay.DayTypeID
                        day.Description = copiedDay.Description

                        newDays.Add(day)
                    Next

                    context.CalendarDays.AddRange(newDays)
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
