Option Strict On

Namespace Global.AccuPay.Utils


    Public Class MessageBoxHelper

        Public Shared Sub DefaultErrorMessage(Optional title As String = "Accupay")

            MsgBox("Something went wrong while loading the time logs. Please contact Globagility Inc. for assistance.",
                   MsgBoxStyle.OkOnly,
                   title)
        End Sub

        Public Shared Sub ErrorMessage(message As String, Optional title As String = "Accupay")

            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Sub
    End Class

End Namespace

