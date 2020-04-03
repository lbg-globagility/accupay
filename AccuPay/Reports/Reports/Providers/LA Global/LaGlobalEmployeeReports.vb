Imports AccuPay.Entity

Public Class LaGlobalEmployeeReports
    Private ReadOnly _employee As Employee

    Private reportProviders = New Dictionary(Of LaGlobalEmployeeReportName, ILaGlobalEmployeeReport) From {
        {LaGlobalEmployeeReportName.ActiveEmployeeChecklistReport, New SmDeploymentEndorsementReportProvider()},
        {LaGlobalEmployeeReportName.BpiInsuranceAmountReport, New SmDeploymentEndorsementReportProvider()},
        {LaGlobalEmployeeReportName.EmploymentContractPage, New SmDeploymentEndorsementReportProvider()},
        {LaGlobalEmployeeReportName.EndofContractReport, New SmDeploymentEndorsementReportProvider()},
        {LaGlobalEmployeeReportName.MonthlyBirthdayReport, New SmDeploymentEndorsementReportProvider()},
        {LaGlobalEmployeeReportName.PayrollSummaryByBranch, New SmDeploymentEndorsementReportProvider()},
        {LaGlobalEmployeeReportName.SmDeploymentEndorsement, New SmDeploymentEndorsementReportProvider()},
        {LaGlobalEmployeeReportName.WorkOrder, New SmDeploymentEndorsementReportProvider()}
    }

    Public Sub New(employee As Employee)
        _employee = employee
    End Sub

    Public Sub Print(report As LaGlobalEmployeeReportName)
        Dim reportProvider = reportProviders(report)
        reportProvider.Employee = _employee
        reportProvider.Output()
    End Sub
End Class

Public Enum LaGlobalEmployeeReportName
    ActiveEmployeeChecklistReport
    BpiInsuranceAmountReport
    EmploymentContractPage
    EndofContractReport
    MonthlyBirthdayReport
    PayrollSummaryByBranch
    SmDeploymentEndorsement
    WorkOrder
End Enum
