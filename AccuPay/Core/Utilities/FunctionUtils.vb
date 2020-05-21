Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Exceptions

Namespace Global.AccuPay.Utils

    Public Class FunctionUtils

        Public Shared Sub TryCatchFunction(
                                        messageTitle As String,
                                        action As [Delegate],
                                        Optional baseExceptionErrorMessage As String = Nothing)

            Try

                action.DynamicInvoke()
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

        End Sub

        Public Shared Async Function TryCatchFunctionAsync(
                                        messageTitle As String,
                                        action As Func(Of Task),
                                        Optional baseExceptionErrorMessage As String = Nothing) As Task

            Try

                Await action()
            Catch ex As ArgumentException
                MessageBoxHelper.ErrorMessage(ex.Message, messageTitle)
            Catch ex As BusinessLogicException
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

        Public Shared Async Function TryCatchFunctionAsync(
                                        messageTitle As String,
                                        action As Func(Of Task(Of Boolean)),
                                        Optional baseExceptionErrorMessage As String = Nothing) As Task(Of Boolean)

            Try

                Return Await action()
            Catch ex As ArgumentException
                MessageBoxHelper.ErrorMessage(ex.Message, messageTitle)
            Catch ex As BusinessLogicException
                MessageBoxHelper.ErrorMessage(ex.Message, messageTitle)
            Catch ex As Exception
                Debugger.Break()

                If baseExceptionErrorMessage Is Nothing Then

                    MessageBoxHelper.DefaultErrorMessage(messageTitle, ex)
                Else

                    MessageBoxHelper.ErrorMessage(baseExceptionErrorMessage, messageTitle)
                End If

            End Try

            Return False

        End Function

        Public Shared Function TryCatchExcelParserReadFunctionAsync(
                                        action As Action,
                                       Optional messageTitle As String = "WorkSheet Parsing Error") As Boolean

            Try

                action()

                Return True
            Catch ex As WorkSheetNotFoundException

                MessageBoxHelper.ErrorMessage(ex.Message)
            Catch ex As WorkSheetIsEmptyException

                MessageBoxHelper.ErrorMessage(ex.Message)
            Catch ex As WorkSheetRowParseValueException

                MessageBoxHelper.ErrorMessage(ex.Message)
            End Try

            Return False

        End Function

    End Class

End Namespace