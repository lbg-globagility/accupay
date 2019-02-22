
Imports System.Runtime.CompilerServices
Imports Newtonsoft.Json

Namespace Global.AccuPay.Extensions

    Module ObjectExtensions

        <Extension()>
        Public Function CloneJson(Of T)(ByVal source As T) As T
            If Object.ReferenceEquals(source, Nothing) Then
                Return Nothing
            End If

            Dim deserializeSettings = New JsonSerializerSettings With {
                .ObjectCreationHandling = ObjectCreationHandling.Replace
            }
            Return JsonConvert.DeserializeObject(Of T)(JsonConvert.SerializeObject(source), deserializeSettings)
        End Function

    End Module

End Namespace
