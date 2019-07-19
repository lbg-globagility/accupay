Namespace Global.AccuPay.Extensions

    ''' <summary>
    ''' This exposes the extension modules to other projects. Currently used for unit testing from other projects.
    ''' </summary>
    Public Class ModuleClass

        Public Shared Function StringExtensions_ToNullableTimeSpan(input As String) As TimeSpan?

            Return input.ToNullableTimeSpan()

        End Function

        Public Shared Function StringExtensions_ToDecimal(input As String) As Decimal?

            Return input.ToDecimal()

        End Function

    End Class

End Namespace