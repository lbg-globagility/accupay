Option Strict On

Namespace AccuPay.Desktop.Helpers

    Public Class LookUpStringItem

        Public ReadOnly Property Item As String

        Public Sub New(item As String)
            Me.Item = item
        End Sub

        Public Shared Function Convert(Of T)(
            itemList As IEnumerable(Of T),
            itemPropertyName As String,
            Optional hasDefaultItem As Boolean = False,
            Optional nullDefaultItem As String = Nothing) As List(Of LookUpStringItem)

            Dim resultList As New List(Of LookUpStringItem)

            If hasDefaultItem Then

                resultList.Add(New LookUpStringItem(nullDefaultItem))

            End If

            If String.IsNullOrWhiteSpace(itemPropertyName) OrElse
                itemList Is Nothing OrElse
                Not itemList.Any() OrElse
                itemList(0).GetType().GetProperty(itemPropertyName) Is Nothing Then

                Return resultList

            End If

            For Each value In itemList

                Dim displayMember = value.GetType().GetProperty(itemPropertyName).GetValue(value)?.ToString()

                resultList.Add(New LookUpStringItem(displayMember))
            Next

            Return resultList

        End Function

        Public Shared Function Convert(
            itemList As IEnumerable(Of String),
            Optional hasDefaultItem As Boolean = False,
            Optional nullDefaultItem As String = Nothing) As List(Of LookUpStringItem)

            Dim resultList As New List(Of LookUpStringItem)

            If hasDefaultItem Then

                resultList.Add(New LookUpStringItem(nullDefaultItem))

            End If

            If itemList Is Nothing OrElse Not itemList.Any() Then

                Return resultList

            End If

            For Each value In itemList

                resultList.Add(New LookUpStringItem(value))
            Next

            Return resultList

        End Function

    End Class

End Namespace
