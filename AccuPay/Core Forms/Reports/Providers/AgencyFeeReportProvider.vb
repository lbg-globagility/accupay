Imports Acupay

Public Class AgencyFeeReportProvider
    Implements IReportProvider

    Public Property Name As String = "Agency Fee" Implements IReportProvider.Name

    Public Sub Run() Implements IReportProvider.Run
        Throw New NotImplementedException()
    End Sub

End Class
