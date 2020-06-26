Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Exceptions
Imports AccuPay.Infrastructure.Services.Excel

Namespace Global.AccuPay.Desktop.Utilities

    Public Class FunctionUtils

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

        Public Shared Function TryCatchExcelParserReadFunction(
                                        action As Action,
                                       Optional messageTitle As String = "WorkSheet Parsing Error") As Boolean
            Try

                action()

                Return True
            Catch ex As WorkSheetRowParseValueException

                MessageBoxHelper.ErrorMessage(ex.Message, messageTitle)
            Catch ex As ExcelException

                MessageBoxHelper.ErrorMessage(ex.Message, messageTitle)
            Catch ex As Exception
                Debugger.Break()

                MessageBoxHelper.DefaultErrorMessage(messageTitle, ex)

            End Try

            Return False

        End Function

        Public Shared Async Function TryCatchExcelParserReadFunctionAsync(
                                        action As Func(Of Task),
                                        Optional messageTitle As String = "WorkSheet Parsing Error") As Task
            Try

                Await action()
            Catch ex As WorkSheetRowParseValueException

                MessageBoxHelper.ErrorMessage(ex.Message, messageTitle)
            Catch ex As ExcelException

                MessageBoxHelper.ErrorMessage(ex.Message, messageTitle)
            Catch ex As Exception
                Debugger.Break()

                MessageBoxHelper.DefaultErrorMessage(messageTitle, ex)

            End Try

        End Function

    End Class

End Namespace