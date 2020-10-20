Option Strict On

Imports System.IO
Imports AccuPay.Data.Interfaces
Imports AccuPay.Desktop.Helpers
Imports Microsoft.Extensions.DependencyInjection
Imports OfficeOpenXml

Public Class EmployeeProfilesReportProvider
    Implements IReportProvider

    Public Property Name As String = "Employee Personal Information" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    'Private basic_alphabet() As String =
    '    New String() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}

    Private _reportBuilder As IEmployeePersonalProfilesReportBuilder

    Public Sub New()
        _reportBuilder = MainServiceProvider.GetRequiredService(Of IEmployeePersonalProfilesReportBuilder)
    End Sub

    Public Async Sub Run() Implements IReportProvider.Run
        Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile("EmployeePersonalInfo", ".xlsx")

        If saveFileDialogHelperOutPut.IsSuccess = False Then
            Return
        End If

        Dim saveFilePath = saveFileDialogHelperOutPut.FileInfo.FullName

        Await _reportBuilder.CreateReport(z_OrganizationID, saveFilePath)

        Process.Start(saveFilePath)
    End Sub

End Class