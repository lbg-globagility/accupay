Imports AccuPay

Public Class AgencyFeeReportProvider
    Implements IReportProvider

    Public Property Name As String = "Agency Fee" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = True Implements IReportProvider.IsHidden

    Public Sub Run() Implements IReportProvider.Run
        Throw New NotImplementedException()
    End Sub

End Class