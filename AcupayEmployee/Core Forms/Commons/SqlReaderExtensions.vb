Imports System.Runtime.CompilerServices

Public Class SqlReaderConversionException
    Inherits InvalidCastException

    Public Property Value As Object

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(value As Object)
        Me.Value = value

    End Sub
End Class

Module SqlReaderExtensions
    Public Function ConvertToType(Of T)(value As Object) As T
        Try
            If IsDBNull(value) Then
                Return CType(Nothing, T)
            End If

            Dim innerType = Nullable.GetUnderlyingType(GetType(T))

            If innerType Is Nothing Then
                Return CType(value, T)
            End If

            If value Is Nothing Then
                Return Nothing
            Else
                Return CTypeDynamic(value, innerType)
            End If
        Catch exception As InvalidCastException
            Throw New SqlReaderConversionException(value)
        End Try
    End Function

    <Extension()>
    Public Function GetValue(Of T)(reader As IDataReader, name As String) As T
        Return ConvertToType(Of T)(reader(name))
    End Function
End Module

