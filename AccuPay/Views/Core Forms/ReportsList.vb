Option Strict On

Imports System.Collections.ObjectModel
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports Microsoft.Extensions.DependencyInjection

Public Class ReportsList

    Private ReadOnly curr_sys_owner_name As String

    Private ReadOnly _systemOwnerService As SystemOwnerService

    Private ReadOnly _listOfValueRepository As ListOfValueRepository

    Sub New()

        InitializeComponent()

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of SystemOwnerService)

        _listOfValueRepository = MainServiceProvider.GetRequiredService(Of ListOfValueRepository)

        curr_sys_owner_name = _systemOwnerService.GetCurrentSystemOwner()
    End Sub

    Private Async Sub ReportsList_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'This has errors
        'New EmployeeOffenseReportProvider(),

        Dim providers = New Collection(Of IReportProvider) From {
            New SalaryIncreaseHistoryReportProvider(),
            New EmploymentRecordReportProvider(),
            New EmployeeProfilesReportProvider(),
            New LeaveLedgerReportProvider(),
            New FiledLeaveReportProvider(),
            New LoanSummaryByEmployeeReportProvider(),
            New LoanSummaryByTypeReportProvider(),
            New SSSMonthlyReportProvider(),
            New PhilHealthReportProvider(),
            New PagIBIGMonthlyReportProvider(),
            New TaxReportProvider(),
            New ThirteenthMonthPayReportProvider(),
            New ThirteenthMonthSummaryReportProvider(),
            New AttendanceSheetReportProvider(),
            New LateUTAbsentSummaryReportProvider(),
            New PayrollLedgerExcelFormatReportProvider(),
            New LoanLedgerReportProvider(),
            New Cinema2000TardinessReportProvider()
        }
        'New PayrollLedgerReportProvider(),

        If curr_sys_owner_name = SystemOwnerService.Benchmark Then
            providers = GetBenchmarkReports()
        End If

        Dim allowedProviders = Await _listOfValueRepository.GetDutyReportProvidersAsync()

        For Each provider In providers

            If provider.IsHidden Then Continue For

            Dim type = provider.GetType().Name

            If allowedProviders.Any(Function(p) p.DisplayValue = type) Then

                Dim newListItem = New ListViewItem(provider.Name)
                newListItem.Tag = provider

                lvMainMenu.Items.Add(newListItem)
            End If
        Next

        If curr_sys_owner_name = SystemOwnerService.Benchmark Then

            'Payroll Summary
            Dim reportProvider As New PayrollSummaryExcelFormatReportProvider()
            lvMainMenu.Items.Add(CreateNewListViewItem(reportProvider, reportProvider.Name))

            'Payslip
            Dim payslipProvider As New DefaultPayslipFullOvertimeBreakdownProvider()
            lvMainMenu.Items.Add(CreateNewListViewItem(payslipProvider, payslipProvider.Name))
        End If
    End Sub

    Private Shared Function CreateNewListViewItem(reportProvider As IReportProvider, reportName As String) As ListViewItem
        Dim listItem = New ListViewItem(reportName)
        listItem.Tag = reportProvider
        Return listItem
    End Function

    Private Shared Function GetBenchmarkReports() As Collection(Of IReportProvider)
        Return New Collection(Of IReportProvider) From {
            New SalaryIncreaseHistoryReportProvider(),
            New EmployeeProfilesReportProvider(),
            New EmployeeIdentificationNumberReportProvider(),
            New LoanSummaryByEmployeeReportProvider(),
            New LoanSummaryByTypeReportProvider(),
            New SSSMonthlyReportProvider(),
            New PhilHealthReportProvider(),
            New PagIBIGMonthlyReportProvider(),
            New TaxReportProvider(),
            New ThirteenthMonthSummaryReportProvider(),
            New LoanLedgerReportProvider(),
            New PayrollSummaryExcelFormatReportProvider()
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
        If lvMainMenu.SelectedItems.Count = 0 Then Return

        Dim listviewitem As New ListViewItem
        Try
            listviewitem = lvMainMenu.SelectedItems(0)
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
            Exit Sub
        End Try

        If TypeOf listviewitem.Tag Is IReportProvider Then
            Dim provider = DirectCast(listviewitem.Tag, IReportProvider)

            Try
                provider.Run()
            Catch ex As NotImplementedException
                MsgBox($"Report Is Not Yet Done: {ex.Message}", MsgBoxStyle.OkOnly)
            End Try
        End If
    End Sub

End Class