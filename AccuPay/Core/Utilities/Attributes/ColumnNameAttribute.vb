Namespace Global.AccuPay.Attributes

    Friend Class ColumnNameAttribute
        Inherits Attribute

        Property Value As String

        Sub New(value As String)
            Me.Value = value
        End Sub

    End Class

End Namespace
