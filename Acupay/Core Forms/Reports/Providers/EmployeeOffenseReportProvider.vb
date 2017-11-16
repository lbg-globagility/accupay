Option Strict On

Imports Acupay

Public Class EmployeeOffenseReportProvider
    Implements IReportProvider

    Public Property Name As String = "Employee Offenses" Implements IReportProvider.Name

    Public Sub Run() Implements IReportProvider.Run
        Throw New NotImplementedException()
    End Sub

End Class
