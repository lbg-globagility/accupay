Imports AccuPay.Data

Public Class PayPeriod
    Implements IPayPeriod

    Public Property RowID As Integer? Implements IPayPeriod.RowID
    Public Property PayFromDate As Date Implements IPayPeriod.PayFromDate
    Public Property PayToDate As Date Implements IPayPeriod.PayToDate
End Class