Imports AccuPay.Core

Public Class PayPeriod
    Implements IPayPeriod

    Sub New(isFirstHalf As Boolean)
        Me.IsFirstHalf = isFirstHalf
    End Sub

    Public Property RowID As Integer? Implements IPayPeriod.RowID
    Public Property PayFromDate As Date Implements IPayPeriod.PayFromDate
    Public Property PayToDate As Date Implements IPayPeriod.PayToDate
    Public Property Month As Integer Implements IPayPeriod.Month
    Public Property Year As Integer Implements IPayPeriod.Year
    Public ReadOnly Property IsFirstHalf As Boolean Implements IPayPeriod.IsFirstHalf

End Class
