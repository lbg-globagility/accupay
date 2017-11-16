Option Strict On

Imports Acupay

Public Class EmployeeOffenseReportProvider
    Implements ReportProvider

    Public Property Name As String = "Employee Offenses" Implements ReportProvider.Name

    Public Sub Run() Implements ReportProvider.Run
        Throw New NotImplementedException()
    End Sub

End Class
