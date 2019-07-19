Option Strict On

Public Class Rate

    Public Property Name As String

    Public Property Rate As Decimal

    Sub New(name As String, rate As Decimal)

        Me.Name = name
        Me.Rate = rate

    End Sub

End Class