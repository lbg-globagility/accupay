Imports Acupay

Public Class PostEmploymentClearanceReportProvider
    Implements ReportProvider

    Public Property Name As String = "Post Employment Clearance" Implements ReportProvider.Name

    Public Sub Run() Implements ReportProvider.Run
        Throw New NotImplementedException("No decision yet what employment clearance would look like.")
    End Sub

End Class
