Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports CrystalDecisions.CrystalReports.Engine
Imports Microsoft.Extensions.DependencyInjection

Public Class LeaveLedgerReportProvider
    Implements IReportProvider

    Public Property Name As String = "Leave Ledger" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Async Sub Run() Implements IReportProvider.Run
        Dim dateSelector As New PayrollSummaDateSelectionDialog()

        If Not dateSelector.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim dateFrom = dateSelector.DateFrom
        Dim dateTo = dateSelector.DateTo

        If dateFrom Is Nothing OrElse dateTo Is Nothing Then

            MessageBoxHelper.ErrorMessage("Please select a start date and end date.")
            Return
        End If

        Await NewReport(dateFrom, dateTo)

    End Sub

    Private Async Function NewReport(dateFrom As Date?, dateTo As Date?) As Task

        If dateFrom Is Nothing Or dateTo Is Nothing Then Throw New ArgumentException("Date From or Date To cannot be null")

        Dim dataService = MainServiceProvider.GetRequiredService(Of LeaveLedgerReportDataService)

        ' Note: There is actually no need to create and interface.
        ' As long as the models have the same name, crystal report can still map the data.
        ' The interface can create a contract though that the 2 models should adhere to.
        Dim leaveLedgerReportModels = Await dataService.GetData(
            z_OrganizationID,
            startDate:=dateFrom.Value,
            endDate:=dateTo.Value)

        Dim report As New New_Employee_Leave_Ledger()
        report.SetDataSource(leaveLedgerReportModels)

        Dim title = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtCutoffDate"), TextObject)
        Dim dateFromTitle = dateFrom?.ToString("MMM dd, yyyy")
        Dim dateToTitle = dateTo?.ToString("MMM dd, yyyy")
        title.Text = $"From {dateFromTitle} to {dateToTitle}"

        Dim viewer As New CrysRepForm()
        viewer.crysrepvwr.ReportSource = report
        viewer.Show()

    End Function

End Class