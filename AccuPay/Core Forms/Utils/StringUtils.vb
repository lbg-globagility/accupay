Imports System.Globalization
Imports System.Text.RegularExpressions

Namespace Global.AccuPay.Utils

    Public Class StringUtils

        Private Sub New()
        End Sub

        Public Shared Function ToPascal(text As String) As String
            Dim newText = Regex.Replace(text, "([A-Z])", " $1")

            Dim info = CultureInfo.CurrentCulture.TextInfo
            Return info.ToTitleCase(newText).Replace(" ", String.Empty)
        End Function

        Public Shared Function Strip(text As String) As String
            Return text.Replace(" ", String.Empty)
        End Function

    End Class

End Namespace
