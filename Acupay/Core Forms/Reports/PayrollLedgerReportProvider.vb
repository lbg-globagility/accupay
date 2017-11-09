Option Strict On

Public Class PayrollLedgerReportProvider
    Implements ReportProvider

    Public Property Name As String = "Payroll Ledger" Implements ReportProvider.Name

    Public Sub Run() Implements ReportProvider.Run
        Throw New NotImplementedException()
    End Sub

End Class
