Imports AccuPay.Entity

Public Class LaGlobalEmployeeReports
    Private ReadOnly _employee As Employee

    Private reportProviders = New Dictionary(Of LaGlobalEmployeeReportName, ILaGlobalEmployeeReport) From {
        {LaGlobalEmployeeReportName.ActiveEmployeeChecklistReport, New ActiveEmployeeChecklistReportProvider()},
        {LaGlobalEmployeeReportName.BpiInsurancePaymentReport, New BpiInsurancePaymentReportProvider()},
        {LaGlobalEmployeeReportName.EmploymentContractPage, New EmploymentContractReportProvider()},
        {LaGlobalEmployeeReportName.MonthlyEndofContractReport, New MonthlyEndofContractReportProvider()},
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
    BpiInsurancePaymentReport
    EmploymentContractPage
    MonthlyEndofContractReport
    MonthlyBirthdayReport
    PayrollSummaryByBranch
    SmDeploymentEndorsement
    WorkOrder
End Enum
