Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Services
Imports AccuPay.Data.ValueObjects
Imports CrystalDecisions.CrystalReports.Engine

Public Class FiledLeaveReportProvider
    Implements IReportProvider
    Public Property Name As String = "Filed Leave" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private ReadOnly _dataService As FiledLeaveReportDataService

    Private ReadOnly _payPeriodServive As PayPeriodService

    Sub New(dataService As FiledLeaveReportDataService, payPeriodServive As PayPeriodService)

        _dataService = dataService

        _payPeriodServive = payPeriodServive
    End Sub

    Public Async Sub Run() Implements IReportProvider.Run
        Dim dateSelector As New PayrollSummaDateSelection(_payPeriodServive)

        If Not dateSelector.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim dateFrom = dateSelector.DateFrom.Value
        Dim dateTo = dateSelector.DateTo.Value

        Dim datePeriod = New TimePeriod(dateFrom, dateTo)

        Dim data = Await CreateFiledLeaveDataTable(datePeriod)

        Dim report = New FiledLeaveReport()
        report.SetDataSource(data)

        Dim title = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtCutoffDate"), TextObject)
        Dim dateFromTitle = dateFrom.ToString("MMMM dd, yyyy")
        Dim dateToTitle = dateTo.ToString("MMMM dd, yyyy")
        title.Text = $"From {dateFromTitle} to {dateToTitle}"

        Dim txtCompanyName = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtCompanyName"), TextObject)
        txtCompanyName.Text = orgNam.ToUpper

        Dim txtAddress = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtAddress"), TextObject)
        txtAddress.Text = PayrollTools.GetOrganizationAddress()

        Dim viewer As New CrysRepForm()
        viewer.crysrepvwr.ReportSource = report
        viewer.Show()
    End Sub

    Public Async Function CreateFiledLeaveDataTable(timePeriod As TimePeriod) As Task(Of DataTable)

        Dim datatable As New DataTable

        datatable.Columns.Add("DatCol1")
        datatable.Columns.Add("DatCol2")
        datatable.Columns.Add("DatCol3")
        datatable.Columns.Add("DatCol11")
        datatable.Columns.Add("DatCol12")
        datatable.Columns.Add("DatCol13")
        datatable.Columns.Add("DatCol14")
        datatable.Columns.Add("DatCol15")
        datatable.Columns.Add("DatCol16")

        Dim leaveTransactions = Await _dataService.GetData(z_OrganizationID, timePeriod)

        For Each transaction In leaveTransactions

            Dim newRow = datatable.NewRow()

            newRow("DatCol1") = transaction.EmployeeID
            newRow("DatCol2") = transaction.Employee.EmployeeNo
            newRow("DatCol3") = transaction.Employee.FullNameWithMiddleInitialLastNameFirst
            newRow("DatCol11") = transaction.TransactionDate.ToString("dddd")
            newRow("DatCol12") = transaction.TransactionDate.ToString("MM/dd/yyyy")
            newRow("DatCol13") = transaction.LeaveLedger.Product.PartNo
            newRow("DatCol14") = transaction.Amount
            newRow("DatCol15") = transaction.Amount / Data.Helpers.PayrollTools.WorkHoursPerDay
            newRow("DatCol16") = transaction.Leave?.Reason

            datatable.Rows.Add(newRow)
        Next

        Return datatable

    End Function

End Class