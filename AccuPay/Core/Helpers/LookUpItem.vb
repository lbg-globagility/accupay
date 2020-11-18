Option Strict On

Imports AccuPay.Utilities

Namespace AccuPay.Desktop.Helpers

    Public Class LookUpItem

        Public ReadOnly Property Id As Integer?
        Public ReadOnly Property DisplayMember As String

        Public Sub New(id As Integer?, displayMember As String)
            Me.Id = id
            Me.DisplayMember = displayMember
        End Sub

        Public Shared Function Convert(Of T)(itemList As IEnumerable(Of T), idPropertyName As String, displayMemberPropertyName As String, Optional nullDefaultItem As String = Nothing) As List(Of LookUpItem)

            Dim resultList As New List(Of LookUpItem)

            If Not String.IsNullOrWhiteSpace(nullDefaultItem) Then

                resultList.Add(New LookUpItem(Nothing, nullDefaultItem))

            End If

            If String.IsNullOrWhiteSpace(idPropertyName) OrElse
                String.IsNullOrWhiteSpace(displayMemberPropertyName) OrElse
                itemList Is Nothing OrElse
                Not itemList.Any() Then

                Return resultList

            End If

            If itemList(0).GetType().GetProperty(idPropertyName) Is Nothing OrElse
                itemList(0).GetType().GetProperty(displayMemberPropertyName) Is Nothing Then

                Return resultList

            End If

            For Each item In itemList

                Dim id = ObjectUtils.ToNullableInteger(item.GetType().GetProperty(idPropertyName).GetValue(item))
                Dim displayMember = item.GetType().GetProperty(displayMemberPropertyName).GetValue(item)?.ToString()

                resultList.Add(New LookUpItem(id, displayMember))
            Next

            Return resultList

        End Function

    End Class

End Namespace
