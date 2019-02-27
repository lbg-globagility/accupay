
Imports System.Runtime.CompilerServices
Imports Newtonsoft.Json

Namespace Global.AccuPay.Extensions

    Module ObjectExtensions

        <Extension()>
        Public Function CloneJson(Of T)(ByVal source As T) As T
            Return CloneObject(source)
        End Function

        <Extension()>
        Public Function CloneListJson(Of T)(ByVal source As List(Of T)) As List(Of T)

            Dim newList As New List(Of T)

            For Each item In source
                newList.Add(item.CloneJson())
            Next

            Return newList

        End Function

#Region "Private Functions"

        Private Function CloneObject(Of T)(source As T) As T
            If Object.ReferenceEquals(source, Nothing) Then
                Return Nothing
            End If

            Dim deserializeSettings = New JsonSerializerSettings With {
                .ObjectCreationHandling = ObjectCreationHandling.Replace
            }
            Return JsonConvert.DeserializeObject(Of T)(JsonConvert.SerializeObject(source), deserializeSettings)
        End Function

    End Module
#End Region

End Namespace
