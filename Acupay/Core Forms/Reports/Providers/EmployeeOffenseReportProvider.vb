Option Strict On

Imports CrystalDecisions.CrystalReports.Engine

Public Class EmployeeOffenseReportProvider
    Implements IReportProvider

    Public Property Name As String = "Employee Offenses" Implements IReportProvider.Name

    Public Sub Run() Implements IReportProvider.Run
        Dim payperiodSelector = New PayrollSummaDateSelection()

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
