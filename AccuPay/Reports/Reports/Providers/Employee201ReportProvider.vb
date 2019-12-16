Option Strict On

Imports CrystalDecisions.CrystalReports.Engine

Public Class Employee201ReportProvider
    Implements IReportProvider

    Public Property Name As String = "Employee 201" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private ReadOnly _employeeID As Integer?

    Public Sub New()
    End Sub

    Public Sub New(employeeID As Integer?)
        _employeeID = employeeID
    End Sub

    Public Sub Run() Implements IReportProvider.Run
        Dim params = New Object(,) {
            {"OrganizID", orgztnID},
            {"EmpRowID", _employeeID}
        }

        Dim data = DirectCast(callProcAsDatTab(params, "PRINT_employee_profile"), DataTable)

        Dim report = New EmployeeProfile()
        report.SetDataSource(data)

        Dim viewer As New CrysRepForm()
        viewer.crysrepvwr.ReportSource = report
        viewer.Show()
    End Sub

End Class