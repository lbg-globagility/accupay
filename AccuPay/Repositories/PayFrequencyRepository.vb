Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class PayFrequencyRepository

        Public Async Function GetAllAsync() As Task(Of List(Of PayFrequency))

            Using context As New PayrollContext

                Return Await context.PayFrequencies.ToListAsync

            End Using

        End Function




    End Class

End Namespace
