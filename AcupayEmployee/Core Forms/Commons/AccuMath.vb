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

End Class
