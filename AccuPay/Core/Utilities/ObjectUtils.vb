Option Strict On

Namespace Global.AccuPay.Utils

    Public Class ObjectUtils

        Public Shared Function ToInteger(num As Object) As Integer
            If IsNothing(num) Then Return 0

            Dim output As Integer = 0

            Try
                output = Convert.ToInt32(num)
            Catch ex As Exception
                output = 0
            End Try

            Return output
        End Function

        Public Shared Function ToNullableInteger(num As Object) As Integer?
            If IsNothing(num) Then Return Nothing

            Dim output As Integer? = Nothing

            Try
                output = Convert.ToInt32(num)
            Catch ex As Exception
                output = Nothing
            End Try

            Return output
        End Function

        Public Shared Function ToDecimal(num As Object, Optional decimalPlace As Integer = 0) As Decimal
            If IsNothing(num) Then Return 0

            Dim output As Decimal = 0

            Try
                output = Convert.ToDecimal(num)
                If decimalPlace > 0 Then
                    AccuMath.CommercialRound(output, decimalPlace)
                End If
            Catch ex As Exception
                output = 0
            End Try

            Return output
        End Function

        Public Shared Function ToNullableDecimal(num As Object) As Decimal?
            If IsNothing(num) Then Return Nothing

            Dim output As Decimal? = Nothing

            Try
                output = Convert.ToDecimal(num)
            Catch ex As Exception
                output = Nothing
            End Try

            Return output
        End Function

        Public Shared Function ToDateTime(dateInput As Object) As Date
            If IsNothing(dateInput) Then Return Date.MinValue

            Dim output As Date = Date.MinValue

            Try
                output = Convert.ToDateTime(dateInput)
            Catch ex As Exception
                output = Date.MinValue
            End Try

            Return output
        End Function

        Public Shared Function ToNullableDateTime(dateInput As Object) As Date?
            If IsNothing(dateInput) Then Return Nothing

            Dim output As Date? = Nothing

            Try
                output = Convert.ToDateTime(dateInput)
            Catch ex As Exception
                output = Nothing
            End Try

            Return output
        End Function

        'Public Shared Function ToNullableBoolean(input As Object) As Boolean?
        '    If IsNothing(input) Then Return Nothing

        '    Dim output As Boolean? = Nothing

        '    Try
        '        output = Convert.ToBoolean(input)
        '    Catch ex As Exception
        '        output = Nothing
        '    End Try

        '    Return output
        'End Function

        Public Shared Function ToStringOrNull(text As Object) As String
            If IsNothing(text) Then Return Nothing
            Return text.ToString()
        End Function

        Public Shared Function ToStringOrEmpty(text As Object) As String
            If IsNothing(text) Then Return String.Empty
            Return text.ToString()
        End Function

    End Class
End Namespace