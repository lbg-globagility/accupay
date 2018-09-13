Option Strict On

Imports AccuPay.Entity
Imports System.Collections.ReadOnlyCollectionBase
Imports System.Collections.Generic
Imports System.Collections.ObjectModel

Public Class ListOfValueCollection

    Private _values As IReadOnlyList(Of ListOfValue)

    Public Sub New(values As IEnumerable(Of ListOfValue))
        _values = New ReadOnlyCollection(Of ListOfValue)(values.ToList)
    End Sub

    Public Function Exists(lic As String) As Boolean
        Dim value = _values?.FirstOrDefault(Function(f) f.LIC = lic)
        Return value IsNot Nothing
    End Function

    Public Function GetValue(lic As String) As String
        Dim value = _values?.FirstOrDefault(Function(f) f.LIC = lic)
        Return value?.DisplayValue
    End Function

    Public Function GetValue(type As String, lic As String) As String
        Dim value = _values?.FirstOrDefault(Function(f) f.Type = type And f.LIC = lic)
        Return value?.DisplayValue
    End Function

    Public Function GetSublist(type As String) As ListOfValueCollection
        Return New ListOfValueCollection(_values.Where(Function(l) l.Type = type))
    End Function

    Public Function GetString(name As String, Optional [default] As String = "") As String
        Dim names = Split(name)
        Return GetStringValue(names.Item1, names.Item2)
    End Function

    Public Function GetStringOrNull(name As String) As String
        Dim names = Split(name)

        Return GetStringValue(names.Item1, names.Item2)
    End Function

    Public Function GetStringOrDefault(name As String, Optional [default] As String = "") As String
        Dim names = Split(name)

        Return If(GetStringValue(names.Item1, names.Item2), [default])
    End Function

    Private Function GetStringValue(type As String, lic As String) As String
        Dim value = _values?.FirstOrDefault(Function(f) f.LIC = lic And f.Type = type)
        Return value?.DisplayValue
    End Function

    Public Function GetEnum(Of T As {Structure})(name As String, Optional [default] As T = Nothing) As T
        Dim names = Split(name)
        Return GetEnum(Of T)(names.Item1, names.Item2, [default])
    End Function

    Public Function GetEnum(Of T As {Structure})(type As String, lic As String, Optional [default] As T = Nothing) As T
        Dim value = GetValue(type, lic)

        If value Is Nothing Then
            Return [default]
        End If

        Return Enums(Of T).Parse(value)
    End Function

    Public Function GetBoolean(name As String, Optional [default] As Boolean = False) As Boolean
        Dim names = Split(name)
        Return GetBoolean(names.Item1, names.Item2, [default])
    End Function

    Public Function GetBoolean(type As String, lic As String, Optional [default] As Boolean = False) As Boolean
        Dim value = GetListOfValue(type, lic)

        If String.IsNullOrEmpty(value?.DisplayValue) Then
            Return [default]
        End If

        If IsNumeric(value?.DisplayValue) Then
            Return Convert.ToBoolean(
                Convert.ToInt32(value?.DisplayValue))
        End If

        Return Convert.ToBoolean(value?.DisplayValue)
    End Function

    Public Function GetDecimal(lic As String, Optional [default] As Decimal = 0) As Decimal
        Return If(GetDecimalOrNull(lic), [default])
    End Function

    Public Function GetDecimalOrNull(lic As String) As Decimal?
        Dim value = GetValue(lic)
        Return If(value IsNot Nothing, Decimal.Parse(value), Nothing)
    End Function

    Private Function Split(name As String) As Tuple(Of String, String)
        Dim names = name.Split({"."}, 2, StringSplitOptions.RemoveEmptyEntries)
        Dim type = names(0)
        Dim lic = names(1)

        Return New Tuple(Of String, String)(type, lic)
    End Function

    Private Function GetListOfValue(type As String, lic As String) As ListOfValue
        Return _values?.FirstOrDefault(Function(f) f.Type = type And f.LIC = lic)
    End Function

End Class
