Option Strict On

Namespace Global.AccuPay.Helpers

    Public Class NameHelper

        Public Shared Function FullNameWithMiddleNameInitialLastNameFirst(firstName As String, middleName As String, lastName As String) As String

            If String.IsNullOrWhiteSpace(firstName) AndAlso String.IsNullOrWhiteSpace(middleName) Then

                Return lastName

            Else

                Dim middleInitial = If(String.IsNullOrWhiteSpace(middleName), "", middleName(0) & ".")

                Return $"{lastName}, {If(String.IsNullOrWhiteSpace(firstName), "", firstName & " ")}{middleInitial}"

            End If

        End Function

    End Class

End Namespace

