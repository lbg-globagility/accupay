Option Strict On
Imports System.Threading.Tasks

Namespace Global.AccuPay.Utils

    Public Class FunctionUtils

        Public Shared Async Function TryCatchFunctionAsync(
                                        messageTitle As String,
                                        action As Func(Of Task),
                                        Optional baseExceptionErrorMessage As String = Nothing) As Task

            Try

                Await action()

            Catch ex As ArgumentException

                MessageBoxHelper.ErrorMessage(ex.Message, messageTitle)

            Catch ex As Exception

                Debugger.Break()

                If baseExceptionErrorMessage Is Nothing Then

                    MessageBoxHelper.DefaultErrorMessage(messageTitle, ex)
                Else

                    MessageBoxHelper.ErrorMessage(baseExceptionErrorMessage, messageTitle)
                End If

            End Try

        End Function

    End Class

End Namespace
