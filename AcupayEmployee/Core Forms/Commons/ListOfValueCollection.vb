Option Strict On

Imports AccuPay.Entity

Public Class ListOfValueCollection

    Private _values As ICollection(Of ListOfValue)

    Public Sub New(values As ICollection(Of ListOfValue))
        _values = values
    End Sub

    Public Function Exists(lic As String) As Boolean
        Dim value = _values?.FirstOrDefault(Function(f) f.LIC = lic)
        Return value IsNot Nothing
    End Function

    Public Function GetValue(lic As String) As String
        Dim value = _values?.FirstOrDefault(Function(f) f.LIC = lic)
        Return value?.DisplayValue
    End Function

    Public Function GetString(type As String, lic As String) As String
        Dim value = _values?.FirstOrDefault(Function(f) f.LIC = lic And f.Type = type)
        Return value?.DisplayValue
    End Function

    Public Function GetBoolean(type As String, lic As String) As Boolean
        Dim value = GetListOfValue(type, lic)

        Return If(
            String.IsNullOrEmpty(value?.DisplayValue),
            False,
            Boolean.Parse(value?.DisplayValue)
        )
    End Function

    Public Function GetString(lic As String) As String
        Return GetValue(lic)
    End Function

    Public Function GetDecimal(lic As String) As Decimal?
        Dim value = GetValue(lic)
        Return If(value IsNot Nothing, Decimal.Parse(value), Nothing)
    End Function

    Private Function GetListOfValue(type As String, lic As String) As ListOfValue
        Return _values?.FirstOrDefault(Function(f) f.Type = type And f.LIC = lic)
    End Function

End Class
