Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Utils
Imports CrystalDecisions.CrystalReports.Engine

Public Class LeaveLedgerReportProvider
    Implements IReportProvider

    Public Property Name As String = "Leave Ledger" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Property IsNewReport As Boolean = True

    Private ReadOnly _dataService As LeaveLedgerReportDataService

    Private ReadOnly _payPeriodServive As PayPeriodService

    Sub New(employeeRepository As EmployeeRepository,
            dataService As LeaveLedgerReportDataService,
            payPeriodServive As PayPeriodService)

        _dataService = dataService
    End Sub

    Public Async Sub Run() Implements IReportProvider.Run
        Dim dateSelector As New PayrollSummaDateSelection(_payPeriodServive)

        If Not dateSelector.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim dateFrom = dateSelector.DateFrom
        Dim dateTo = dateSelector.DateTo

        If dateFrom Is Nothing OrElse dateTo Is Nothing Then

            MessageBoxHelper.ErrorMessage("Please select a start date and end date.")
            Return
        End If

        If IsNewReport Then
            Await NewReport(dateFrom, dateTo)
        Else
            OldReport(dateSelector, dateFrom, dateTo)
        End If

    End Sub

    Private Async Function NewReport(dateFrom As Date?, dateTo As Date?) As Task

        If dateFrom Is Nothing Or dateTo Is Nothing Then Throw New ArgumentException("Date From or Date To cannot be null")

        ' Note: There is actually no need to create and interface.
        ' As long as the models have the same name, crystal report can still map the data.
        ' The interface can create a contract though that the 2 models should adhere to.
        Dim leaveLedgerReportModels = Await _dataService.GetData(z_OrganizationID,
                                                                dateFrom.Value,
                                                                dateTo.Value)

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

    Private Shared Sub OldReport(dateSelector As PayrollSummaDateSelection, dateFrom As Date?, dateTo As Date?)
        Dim params = New Object(,) {
                    {"OrganizID", orgztnID},
                    {"paramDateFrom", dateFrom},
                    {"paramDateTo", dateTo},
                    {"PayPeriodDateFromID", dateSelector.PayPeriodFromID},
                    {"PayPeriodDateToID", dateSelector.PayPeriodToID}
                }

        Dim data = DirectCast(callProcAsDatTab(params, "RPT_leave_ledger"), DataTable)

        Dim report = New Employee_Leave_Ledger()

        Dim title = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtCutoffDate"), TextObject)
        Dim dateFromTitle = dateFrom?.ToString("MMM dd, yyyy")
        Dim dateToTitle = dateTo?.ToString("MMM dd, yyyy")
        title.Text = $"From {dateFromTitle} to {dateToTitle}"

        Dim viewer As New CrysRepForm()
        viewer.crysrepvwr.ReportSource = report
        viewer.Show()
    End Sub

End Class