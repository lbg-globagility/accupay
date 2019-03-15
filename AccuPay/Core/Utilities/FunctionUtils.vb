Option Strict On
Imports System.Threading.Tasks

Namespace Global.AccuPay.Utils

    Public Class FunctionUtils

        Public Shared Async Function TryCatchFunctionAsync(messageTitle As String, action As Func(Of Task)) As Task

            Try

                Await action()

            Catch ex As ArgumentException

                MessageBoxHelper.ErrorMessage(ex.Message, messageTitle)

            Catch ex As Exception

                Debugger.Break()
                MessageBoxHelper.DefaultErrorMessage(messageTitle, ex)

            End Try

        End Function

    End Class

End Namespace
