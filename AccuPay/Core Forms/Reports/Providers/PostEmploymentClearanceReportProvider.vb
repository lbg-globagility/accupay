Imports Acupay

Public Class PostEmploymentClearanceReportProvider
    Implements IReportProvider

    Public Property Name As String = "Post Employment Clearance" Implements IReportProvider.Name

    Public Sub Run() Implements IReportProvider.Run
        Throw New NotImplementedException("No decision yet what employment clearance would look like.")
    End Sub

End Class
