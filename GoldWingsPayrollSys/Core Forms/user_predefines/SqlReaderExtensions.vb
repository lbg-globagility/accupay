Imports System.Runtime.CompilerServices

Module SqlReaderExtensions
    Public Function ConvertToType(Of T)(value As Object) As T
        If IsDBNull(value) Then
            Return CType(Nothing, T)
        End If

        Dim innerType = Nullable.GetUnderlyingType(GetType(T))

        If innerType Is Nothing Then
            Return CType(value, T)
        Else
            Return CTypeDynamic(value, innerType)
        End If
    End Function

    <Extension()>
    Public Function GetValue(Of T)(reader As IDataReader, name As String) As T
        Return ConvertToType(Of T)(reader(name))
    End Function
End Module

