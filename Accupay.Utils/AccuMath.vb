Option Strict On

Public Class AccuMath

    ''' <summary>
    ''' Truncates and not round the fractional portion of the decimal to n places.
    ''' </summary>
    ''' <param name="value">The value to truncate</param>
    ''' <param name="places">The number of fractional places to leave</param>
    ''' <returns>The truncated decimal value</returns>
    Public Shared Function Truncate(value As Decimal, places As Integer) As Decimal
        Dim x = Math.Pow(10, places)
        Return CDec(Math.Truncate(value * x) / x)
    End Function

    ''' <summary>
    ''' Perform a commercial rounding away from zero.
    '''
    ''' ex:
    ''' 1.284 -> 1.28
    ''' 1.285 -> 1.29
    ''' 1.286 -> 1.29
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="places"></param>
    ''' <returns></returns>
    Public Shared Function CommercialRound(value As Decimal, Optional places As Integer = 2) As Decimal
        Return Math.Round(value, places, MidpointRounding.AwayFromZero)
    End Function

    Public Shared Function CommercialRound(value As Decimal?, Optional places As Integer = 2) As Decimal

        If value Is Nothing Then Return 0D

        Return CommercialRound(CDec(value), places)
    End Function

    Public Shared Function CommercialRound(value As Double, Optional places As Integer = 2) As Decimal
        Return CommercialRound(CDec(value), places)
    End Function

    ''' <summary>
    ''' This method is used as a workaround on VB.Net's fuck up ternary operator using nullable decimal. Example of built-in ternary operator: If(value, 0, Nothing). If it's true, it return 0. If it's false, it should return Nothing but VB.Net is a fuck up PL and returns zero instead. This is due to VB.Net's "Nothing" actually a closer match to C#'s default(T) instead of null. And the value of default(Integer) is 0.
    ''' </summary>
    ''' <param name="condition"></param>
    ''' <param name="trueOutput">The output when the condition is True.</param>
    ''' <param name="falseOutput">The output when the condition is False.</param>
    ''' <returns>A nullable decimal.</returns>
    Public Shared Function NullableDecimalTernaryOperator(condition As Boolean, trueOutput As Decimal?, falseOutput As Decimal?) As Decimal?

        If condition Then

            Return trueOutput
        Else

            Return falseOutput

        End If

    End Function

End Class
