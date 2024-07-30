Option Strict On
Imports AccuPay.CrystalReports
Imports Microsoft.Extensions.DependencyInjection
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Infrastructure.Data

Imports OfficeOpenXml
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Interfaces.Reports
Imports System.Security
Imports System.IO
Imports OfficeOpenXml.DataValidation

Public Class PBCOMReportProvider
    Implements IReportProvider


    Public Property Name As String = "PBCOM Report" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden



    Private Async Sub Run() Implements IReportProvider.Run
        Dim pbcomSelectionDialog = New PBCOMSelection()
        pbcomSelectionDialog.ShowDialog()
    End Sub
End Class
