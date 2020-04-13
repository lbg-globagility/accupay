Imports AccuPay.Entity

Public Class LaGlobalEmployeeReports
    Private ReadOnly _employee As Employee

    Private reportProviders = New Dictionary(Of LaGlobalEmployeeReportName, ILaGlobalEmployeeReport) From {
        {LaGlobalEmployeeReportName.ActiveEmployeeChecklistReport, New SmDeploymentEndorsementReportProvider()},
        {LaGlobalEmployeeReportName.BpiInsuranceAmountReport, New BpiInsurancePaymentReportProvider()},
        {LaGlobalEmployeeReportName.EmploymentContractPage, New EmploymentContractReportProvider()},
        {LaGlobalEmployeeReportName.EndofContractReport, New SmDeploymentEndorsementReportProvider()},
        {LaGlobalEmployeeReportName.MonthlyBirthdayReport, New MonthlyBirthdayCelebrantsReportProvider()},
        {LaGlobalEmployeeReportName.PayrollSummaryByBranch, New SmDeploymentEndorsementReportProvider()},
        {LaGlobalEmployeeReportName.SmDeploymentEndorsement, New SmDeploymentEndorsementReportProvider()},
        {LaGlobalEmployeeReportName.WorkOrder, New WorkOrderReportProvider()}
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
