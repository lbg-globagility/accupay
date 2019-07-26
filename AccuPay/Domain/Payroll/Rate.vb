Option Strict On

Public Class Rate

    Public Property Name As String
    Public Property Rate As Decimal
    Public Property BaseRate As Rate

    Sub New(name As String, rate As Decimal, Optional baseRate As Rate = Nothing)

        Me.Name = name
        Me.Rate = rate
        Me.BaseRate = baseRate

    End Sub

End Class