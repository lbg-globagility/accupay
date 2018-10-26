Public Class AccuMath

    ''' <summary>
    ''' Truncates and not round the fractional portion of the decimal to n places.
    ''' </summary>
    ''' <param name="value">The value to truncate</param>
    ''' <param name="places">The number of fractional places to leave</param>
    ''' <returns>The truncated decimal value</returns>
    Public Shared Function Truncate(value As Decimal, places As Integer) As Decimal
        Dim x = Math.Pow(10, places)
        Return Math.Truncate(value * x) / x
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

End Class
