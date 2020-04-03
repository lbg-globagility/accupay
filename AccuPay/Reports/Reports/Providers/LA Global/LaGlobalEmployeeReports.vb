Imports AccuPay.Entity

Public Class LaGlobalEmployeeReports
    Private ReadOnly _employee As Employee

    Private reportProviders = New Dictionary(Of LaGlobalEmployeeReportName, ILaGlobalEmployeeReport) From {
        {LaGlobalEmployeeReportName.SmDeploymentEndorsement, New SmDeploymentEndorsementReportProvider()}
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
    EmploymentContractPage1
    EmploymentContractPage2
    EndofContractReport
    MonthlyBirthdayReport
    PayrollSummaryByBranch
    SmDeploymentEndorsement
    WorkOrder
End Enum
