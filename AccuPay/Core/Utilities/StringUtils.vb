Imports System.Globalization
Imports System.Text.RegularExpressions

Namespace Global.AccuPay.Utils

    Public Class StringUtils

        Private Sub New()
        End Sub

        Public Shared Function ToPascal(text As String) As String

            If text Is Nothing Then Return Nothing

            Dim newText = Regex.Replace(text, "([A-Z])", " $1")

            Dim info = CultureInfo.CurrentCulture.TextInfo
            Return info.ToTitleCase(newText).Replace(" ", String.Empty)
        End Function

        Public Shared Function Strip(text As String) As String

            If text Is Nothing Then Return Nothing

            Return text.Replace(" ", String.Empty)
        End Function

        Public Shared Function Normalize(text As String) As String
            Return text?.Trim()?.ToUpper()
        End Function

    End Class

End Namespace
