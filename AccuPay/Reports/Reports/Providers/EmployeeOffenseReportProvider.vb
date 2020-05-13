Option Strict On

Imports AccuPay.Data.Services
Imports CrystalDecisions.CrystalReports.Engine

Public Class EmployeeOffenseReportProvider
    Implements IReportProvider
    Public Property Name As String = "Employee Offenses" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private ReadOnly _payPeriodService As PayPeriodService

    Sub New(payPeriodService As PayPeriodService)

        _payPeriodService = payPeriodService

    End Sub

    Public Sub Run() Implements IReportProvider.Run
        Dim payperiodSelector = New PayrollSummaDateSelection(_payPeriodService)

        If Not payperiodSelector.ShowDialog() = DialogResult.OK Then
            Return
        End If

        Dim dateFrom = CDate(payperiodSelector.DateFrom)
        Dim dateTo = CDate(payperiodSelector.DateTo)

        Dim params = New Object(,) {
            {"organizationID", orgztnID},
            {"dateFrom", dateFrom},
            {"dateTo", dateTo}
        }

        Dim data = DirectCast(callProcAsDatTab(params, "RPT_EmployeeOffenses"), DataTable)

        Dim report = New Employees_Offenses()
        report.SetDataSource(data)

        Dim reportDialog As New CrysRepForm()
        reportDialog.crysrepvwr.ReportSource = report
        reportDialog.Show()
    End Sub

End Class