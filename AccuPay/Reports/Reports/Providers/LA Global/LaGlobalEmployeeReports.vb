Option Strict On

Imports AccuPay.Data
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services

Public Class LaGlobalEmployeeReports

    Private ReadOnly _context As PayrollContext

    Private ReadOnly _bpiInsuranceAmountReportDataService As BpiInsuranceAmountReportDataService

    Private ReadOnly _employeeRepository As EmployeeRepository

    Private ReadOnly _payPeriodRepository As PayPeriodRepository

    Private ReadOnly _employee As Employee

    Private reportProviders As New Dictionary(Of LaGlobalEmployeeReportName, ILaGlobalEmployeeReport) From {
        {LaGlobalEmployeeReportName.ActiveEmployeeChecklistReport, New ActiveEmployeeChecklistReportProvider(_context, _payPeriodRepository)},
        {LaGlobalEmployeeReportName.BpiInsurancePaymentReport, New BpiInsurancePaymentReportProvider(_bpiInsuranceAmountReportDataService)},
        {LaGlobalEmployeeReportName.EmploymentContractPage, New EmploymentContractReportProvider(_employeeRepository)},
        {LaGlobalEmployeeReportName.MonthlyEndofContractReport, New MonthlyEndofContractReportProvider(_context)},
        {LaGlobalEmployeeReportName.MonthlyBirthdayReport, New MonthlyBirthdayCelebrantsReportProvider(_context)},
        {LaGlobalEmployeeReportName.PayrollSummaryByBranch, New SmDeploymentEndorsementReportProvider()},
        {LaGlobalEmployeeReportName.SmDeploymentEndorsement, New SmDeploymentEndorsementReportProvider()},
        {LaGlobalEmployeeReportName.WorkOrder, New WorkOrderReportProvider()}
    }

    Public Sub New(context As PayrollContext,
                   bpiInsuranceAmountReportDataService As BpiInsuranceAmountReportDataService,
                   employeeRepository As EmployeeRepository,
                   payPeriodRepository As PayPeriodRepository,
                   employee As Employee)

        _context = context

        _bpiInsuranceAmountReportDataService = bpiInsuranceAmountReportDataService

        _employeeRepository = employeeRepository

        _payPeriodRepository = payPeriodRepository

        _employee = employee
    End Sub

    Public Sub Print(report As LaGlobalEmployeeReportName)
        Dim reportProvider As ILaGlobalEmployeeReport = reportProviders(report)
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