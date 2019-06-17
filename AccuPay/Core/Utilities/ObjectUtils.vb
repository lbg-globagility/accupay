Option Strict On

Imports System.Globalization

Namespace Global.AccuPay.Utils

    Public Class ObjectUtils

        Public Shared Function ToInteger(num As Object) As Integer

            Dim defaultOutput As Integer = 0

            If IsNothing(num) Then Return defaultOutput

            Dim output As Integer = defaultOutput

            Try
                output = Convert.ToInt32(num)
            Catch ex As Exception
                output = defaultOutput
            End Try

            Return output
        End Function

        Public Shared Function ToNullableInteger(num As Object) As Integer?

            Dim defaultOutput As Integer? = Nothing

            If IsNothing(num) Then Return defaultOutput

            Dim output As Integer? = defaultOutput

            Try
                output = Convert.ToInt32(num)
            Catch ex As Exception
                output = defaultOutput
            End Try

            Return output
        End Function

        Public Shared Function ToDecimal(num As Object, Optional decimalPlace As Integer = 0) As Decimal

            Dim defaultOutput As Decimal = 0

            If IsNothing(num) Then Return defaultOutput

            Dim output As Decimal = defaultOutput

            Try
                output = Convert.ToDecimal(num)
                If decimalPlace > defaultOutput Then
                    AccuMath.CommercialRound(output, decimalPlace)
                End If
            Catch ex As Exception
                output = defaultOutput
            End Try

            Return output
        End Function

        Public Shared Function ToNullableDecimal(num As Object) As Decimal?

            Dim defaultOutput As Decimal? = Nothing

            If IsNothing(num) Then Return defaultOutput

            Dim output As Decimal? = defaultOutput

            Try
                output = Convert.ToDecimal(num)
            Catch ex As Exception
                output = defaultOutput
            End Try

            Return output
        End Function

        Public Shared Function ToNullableDouble(num As Object) As Double?

            Dim defaultOutput As Double? = Nothing

            If IsNothing(num) Then Return defaultOutput

            Dim output As Double? = defaultOutput

            Try
                output = Convert.ToDouble(num)
            Catch ex As Exception
                output = defaultOutput
            End Try

            Return output
        End Function

        Public Shared Function ToDateTime(dateInput As Object) As Date

            Dim defaultOutput As Date = Date.MinValue

            If IsNothing(dateInput) Then Return defaultOutput

            Dim output As Date = defaultOutput

            Try
                output = Convert.ToDateTime(dateInput)
            Catch ex As Exception
                output = defaultOutput
            End Try

            Return output
        End Function

        Public Shared Function ToNullableDateTime(dateInput As Object) As Date?

            Dim defaultOutput As Date? = Nothing

            If IsNothing(dateInput) Then Return defaultOutput

            Dim output As Date? = defaultOutput

            Try
                output = Convert.ToDateTime(dateInput)
            Catch ex As Exception
                output = defaultOutput
            End Try

            Return output
        End Function

        Public Shared Function ToTimeSpan(input As Object) As TimeSpan

            Dim defaultOutput As TimeSpan = TimeSpan.Zero

            If IsNothing(input) Then Return defaultOutput

            Dim output As TimeSpan = defaultOutput

            Dim dt As Date

            If (Date.TryParseExact(input.ToString(), "HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, dt)) Then

                output = dt.TimeOfDay
            End If

            Return output
        End Function

        Public Shared Function ToNullableTimeSpan(input As Object) As TimeSpan?

            If IsNothing(input) Then Return Nothing

            Dim output As TimeSpan

            Dim dt As Date

            If (Date.TryParse(input.ToString(),
                CultureInfo.InvariantCulture, DateTimeStyles.None, dt)) Then

                output = dt.TimeOfDay
            End If

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