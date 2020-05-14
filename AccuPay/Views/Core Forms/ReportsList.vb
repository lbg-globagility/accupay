Option Strict On

Imports System.Collections.ObjectModel
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports Microsoft.Extensions.DependencyInjection

Public Class ReportsList

    Private Const ActualDescription As String = "(Actual)"

    Private ReadOnly curr_sys_owner_name As String

    Private ReadOnly _systemOwnerService As SystemOwnerService

    Sub New()

        InitializeComponent()

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of SystemOwnerService)

        curr_sys_owner_name = _systemOwnerService.GetCurrentSystemOwner()
    End Sub

    Private ReadOnly _adjustmentService As AdjustmentService

    Private ReadOnly _cinemaTardinessReportDataService As CinemaTardinessReportDataService

    Private ReadOnly _filedLeaveReportDataService As FiledLeaveReportDataService

    Private ReadOnly _leaveLedgerReportDataService As LeaveLedgerReportDataService

    Private ReadOnly _listOfValueService As ListOfValueService

    Private ReadOnly _payPeriodService As PayPeriodService

    Private ReadOnly _paystubPayslipModelDataService As PaystubPayslipModelDataService

    Private ReadOnly _employeeRepository As EmployeeRepository

    Private ReadOnly _payPeriodRepository As PayPeriodRepository

    Sub New(adjustmentService As AdjustmentService,
            cinemaTardinessReportDataService As CinemaTardinessReportDataService,
            filedLeaveReportDataService As FiledLeaveReportDataService,
            leaveLedgerReportDataService As LeaveLedgerReportDataService,
            listOfValueService As ListOfValueService,
            PayPeriodService As PayPeriodService,
            paystubPayslipModelDataService As PaystubPayslipModelDataService,
            systemOwnerService As SystemOwnerService,
            employeeRepository As EmployeeRepository,
            payPeriodRepository As PayPeriodRepository)

        InitializeComponent()
        _adjustmentService = adjustmentService

        _cinemaTardinessReportDataService = cinemaTardinessReportDataService

        _filedLeaveReportDataService = filedLeaveReportDataService

        _leaveLedgerReportDataService = leaveLedgerReportDataService

        _listOfValueService = listOfValueService

        _payPeriodService = PayPeriodService

        _paystubPayslipModelDataService = paystubPayslipModelDataService

        _systemOwnerService = systemOwnerService

        _employeeRepository = employeeRepository

        _payPeriodRepository = payPeriodRepository

        curr_sys_owner_name = _systemOwnerService.GetCurrentSystemOwner()
    End Sub

    Private Sub ReportsList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim providers = New Collection(Of IReportProvider) From {
            New SalaryIncreaseHistoryReportProvider(_payPeriodService),
            New EmploymentRecordReportProvider(),
            New EmployeeProfilesReportProvider(),
            New PostEmploymentClearanceReportProvider(),
            New EmployeeIdentificationNumberReportProvider(),
            New EmployeeOffenseReportProvider(_payPeriodService),
            New LeaveLedgerReportProvider(_employeeRepository, _leaveLedgerReportDataService, _payPeriodService) With {.IsNewReport = True},
            New FiledLeaveReportProvider(_filedLeaveReportDataService, _payPeriodService),
            New LoanSummaryByEmployeeReportProvider(_payPeriodService),
            New LoanSummaryByTypeReportProvider(_payPeriodService),
            New SSSMonthlyReportProvider(),
            New PhilHealthReportProvider(),
            New PagIBIGMonthlyReportProvider(),
            New TaxReportProvider(_payPeriodRepository),
            New ThirteenthMonthPayReportProvider(_payPeriodService),
            New ThirteenthMonthSummaryReportProvider(_payPeriodService),
            New AttendanceSheetReportProvider(_payPeriodService),
            New LateUTAbsentSummaryReportProvider(_payPeriodService),
            New AgencyFeeReportProvider(),
            New PayrollLedgerExcelFormatReportProvider(_payPeriodService),
            New LoanLedgerReportProvider(_payPeriodService),
            New Cinema2000TardinessReportProvider(_cinemaTardinessReportDataService)
        }
        'New PayrollLedgerReportProvider(),

        If _systemOwnerService.GetCurrentSystemOwner() = SystemOwnerService.Benchmark Then
            providers = GetBenchmarkReports()
        End If

        For Each provider In providers

            If provider.IsHidden Then Continue For

            Dim dataTable = New SqlToDataTable($"
                SELECT l.DisplayValue
                FROM listofval l
                WHERE l.`Type` = 'ReportProviders'
            ").Read()

            Dim type = provider.GetType().Name
            Dim found = dataTable.Select($"DisplayValue = '{type}'").Count >= 1

            If found Then

                Dim newListItem = New ListViewItem(provider.Name)
                newListItem.Tag = provider

                lvMainMenu.Items.Add(newListItem)
            End If
        Next

        If _systemOwnerService.GetCurrentSystemOwner() = SystemOwnerService.Benchmark Then

            lvMainMenu.Items.Add(CreatePayrollSummaryListViewItem("(Declared)"))
            lvMainMenu.Items.Add(CreatePayrollSummaryListViewItem(ActualDescription))

            'Payslip
            Dim payslipProvider As New DefaultPayslipFullOvertimeBreakdownProvider(_payPeriodRepository,
                                                                                   _payPeriodService,
                                                                                   _paystubPayslipModelDataService)
            lvMainMenu.Items.Add(CreateNewListViewItem(payslipProvider, payslipProvider.Name))
        End If
    End Sub

    Private Function CreatePayrollSummaryListViewItem(suffix As String) As ListViewItem

        Dim summaryProvider As New PayrollSummaryExcelFormatReportProvider(_payPeriodRepository,
                                                                           _adjustmentService,
                                                                           _listOfValueService,
                                                                           _payPeriodService,
                                                                           _systemOwnerService)

        Dim reportName = summaryProvider.Name & " " & suffix
        Return CreateNewListViewItem(summaryProvider, reportName)
    End Function

    Private Shared Function CreateNewListViewItem(
                                reportProvider As IReportProvider,
                                reportName As String) As ListViewItem

        Dim listItem = New ListViewItem(reportName)
        listItem.Tag = reportProvider
        Return listItem
    End Function

    Private Function GetBenchmarkReports() As Collection(Of IReportProvider)
        Return New Collection(Of IReportProvider) From {
                    New SalaryIncreaseHistoryReportProvider(_payPeriodService),
                    New EmployeeProfilesReportProvider(),
                    New EmployeeIdentificationNumberReportProvider(),
                    New LoanSummaryByEmployeeReportProvider(_payPeriodService),
                    New LoanSummaryByTypeReportProvider(_payPeriodService),
                    New SSSMonthlyReportProvider(),
                    New PhilHealthReportProvider(),
                    New PagIBIGMonthlyReportProvider(),
                    New TaxReportProvider(_payPeriodRepository),
                    New ThirteenthMonthSummaryReportProvider(_payPeriodService),
                    New LoanLedgerReportProvider(_payPeriodService),
                    New PayrollSummaryExcelFormatReportProvider(_payPeriodRepository,
                                                                _adjustmentService,
                                                                _listOfValueService,
                                                                _payPeriodService,
                                                                _systemOwnerService)
                }
    End Function

    Private Sub lvMainMenu_KeyDown(sender As Object, e As KeyEventArgs) Handles lvMainMenu.KeyDown
        If lvMainMenu.Items.Count <> 0 Then
            If e.KeyCode = Keys.Enter Then
                report_maker()
            End If
        End If
    End Sub

    Private Sub lvMainMenu_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lvMainMenu.MouseDoubleClick
        If lvMainMenu.Items.Count > 0 Then
            report_maker()
        End If
    End Sub

    Sub report_maker()
        Dim n_listviewitem As New ListViewItem

        Try
            n_listviewitem = lvMainMenu.SelectedItems(0)
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
            Exit Sub
        End Try

        If TypeOf n_listviewitem.Tag Is IReportProvider Then
            Dim provider = DirectCast(n_listviewitem.Tag, IReportProvider)

            Try
                If _systemOwnerService.GetCurrentSystemOwner() = SystemOwnerService.Benchmark AndAlso
                    TypeOf provider Is PayrollSummaryExcelFormatReportProvider Then

                    Dim payrollSummary = DirectCast(provider, PayrollSummaryExcelFormatReportProvider)

                    Dim isActual = n_listviewitem.Text.EndsWith(ActualDescription)

                    payrollSummary.IsActual = isActual

                    payrollSummary.Run()
                Else

                    provider.Run()
                End If
            Catch ex As NotImplementedException
                MsgBox($"Report Is Not Yet Done: {ex.Message}", MsgBoxStyle.OkOnly)
            End Try
        End If
    End Sub

End Class