Option Strict On

Imports log4net

Namespace Global.AccuPay.Desktop.Utilities

    Public Class MessageBoxHelper

        Private Const MessageTitle As String = "AccuPay"

        Private Shared _logger As ILog = LogManager.GetLogger("TimeLogsLogger")

        Public Shared Sub DefaultErrorMessage(Optional title As String = MessageTitle, Optional exception As Exception = Nothing, Optional errorMessageTitle As String = "DefaultErrorMessage")

            If exception IsNot Nothing Then
                _logger.Error(errorMessageTitle, exception)
            End If

            MsgBox("Something went wrong while executing the last task. Please contact Globagility Inc. for assistance.",
                   MsgBoxStyle.OkOnly,
                   title)
        End Sub

        Public Shared Sub DefaultUnauthorizedFormMessage(ByVal Optional title As String = MessageTitle, ByVal Optional messageBoxButtons As MessageBoxButtons = MessageBoxButtons.OK)

            MessageBox.Show("You are not authorized to access that form.", title, messageBoxButtons, MessageBoxIcon.Warning)

        End Sub

        Public Shared Sub DefaultUnauthorizedActionMessage(ByVal Optional title As String = MessageTitle, ByVal Optional messageBoxButtons As MessageBoxButtons = MessageBoxButtons.OK)

            MessageBox.Show("You are not authorized to perform that action.", title, messageBoxButtons, MessageBoxIcon.Warning)

        End Sub

        Public Shared Sub ErrorMessage(message As String, Optional title As String = MessageTitle)

            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Sub

        Public Shared Sub Warning(ByVal message As String, ByVal Optional title As String = MessageTitle, ByVal Optional messageBoxButtons As MessageBoxButtons = MessageBoxButtons.OK)

            MessageBox.Show(message, title, messageBoxButtons, MessageBoxIcon.Warning)

        End Sub

        Public Shared Sub Information(ByVal message As String, ByVal Optional title As String = MessageTitle, ByVal Optional messageBoxButtons As MessageBoxButtons = MessageBoxButtons.OK)

            MessageBox.Show(message, title, messageBoxButtons, MessageBoxIcon.Information)

        End Sub

        ''' <summary>
        ''' Displays a messasage box with specified text, caption, buttons And icons.
        ''' This Is a wrapper method to System.Windows.Forms.MessageBox.Show().
        ''' Only bool And DialogResult type parameter Is allowed. Otherwise this will throw an exception
        ''' </summary>
        ''' <typeparam name="T">The data type you want the method to return</typeparam>
        ''' <param name="text">The text to display in the message box</param>
        ''' <param name="caption">The text to display in the title bar of the message box</param>
        ''' <param name="messageBoxButton">One of the System.Windows.Forms.MessageBoxButtons values
        ''' that specifies which buttons to display in the message box</param>
        ''' <param name="messageBoxIcon">One of the System.Windows.Forms.MessageBoxIcon values
        ''' that specifies which icon to display in the message box</param>
        ''' <returns></returns>
        ''' <exception cref="ArgumentException">Only type of bool And DialogResult have implementations</exception>
        Public Shared Function Confirm(Of T)(ByVal text As String, ByVal Optional caption As String = "", ByVal Optional messageBoxButton As MessageBoxButtons = MessageBoxButtons.YesNo, ByVal Optional messageBoxIcon As MessageBoxIcon = MessageBoxIcon.Question) As T
            If GetType(T) = GetType(Boolean) Then
                Return CType(Convert.ChangeType(MessageBox.Show(text, caption, messageBoxButton, messageBoxIcon) = DialogResult.Yes, GetType(T)), T)
            ElseIf GetType(T) = GetType(DialogResult) Then
                Return CType(Convert.ChangeType(MessageBox.Show(text, caption, messageBoxButton, messageBoxIcon), GetType(T)), T)
            Else
                Throw New ArgumentException()
            End If
        End Function

    End Class

End Namespace