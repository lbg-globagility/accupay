Option Strict On

Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports Microsoft.Extensions.DependencyInjection
Imports Remotion.Linq.Clauses

Public Class ReportsList

    Private ReadOnly curr_sys_owner_name As String

    Private ReadOnly _systemOwnerService As ISystemOwnerService

    Private ReadOnly _listOfValueRepository As IListOfValueRepository

    Sub New()

        InitializeComponent()

        _systemOwnerService = MainServiceProvider.GetRequiredService(Of ISystemOwnerService)

        _listOfValueRepository = MainServiceProvider.GetRequiredService(Of IListOfValueRepository)

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
            New ThirteenthMonthDetailedReportProvider(),
            New ThirteenthMonthSummaryReportProvider(),
            New AttendanceSheetReportProvider(),
            New LateUTAbsentSummaryReportProvider(),
            New PayrollLedgerExcelFormatReportProvider(),
            New LoanLedgerReportProvider(),
            New Cinema2000TardinessReportProvider(),
            New AlphalistExcelFormatReportProvider(),
            New BankFileReportProvider(z_OrganizationID)
        }
        'New PayrollLedgerReportProvider(),

        If curr_sys_owner_name = SystemOwner.Benchmark Then
            providers = GetBenchmarkReports()
        End If

        If curr_sys_owner_name = SystemOwner.LAGlobal Then providers.Add(New LaGlobalAlphaListReportProvider())

        Await Cinema2000BankFileReportProviders(providers)

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

        If curr_sys_owner_name = SystemOwner.Benchmark Then

            'Payroll Summary
            Dim reportProvider As New PayrollSummaryExcelFormatReportProvider()
            lvMainMenu.Items.Add(CreateNewListViewItem(reportProvider, reportProvider.Name))

            'Payslip
            Dim payslipProvider As New DefaultPayslipFullOvertimeBreakdownProvider()
            lvMainMenu.Items.Add(CreateNewListViewItem(payslipProvider, payslipProvider.Name))
        End If
    End Sub

    Private Async Function Cinema2000BankFileReportProviders(providers As Collection(Of IReportProvider)) As Task
        If Not curr_sys_owner_name = SystemOwner.Cinema2000 Then Return

        Dim defaultBankFileReportProvider = providers.FirstOrDefault(Function(t) t.Name = BankFileReportProvider.BANK_FILE_TEXT)
        If defaultBankFileReportProvider IsNot Nothing Then providers.Remove(defaultBankFileReportProvider)

        Dim bankFileSecurityBankPolicyText = BankFileTextFormatSecurityBankForm.POLICY_TYPE_NAME
        Dim bankFileSimpleExcelPolicyText = BankFileSimpleExcelForm.POLICY_TYPE_NAME
        Dim bankFileEasyExcelPolicyText = BankFileEasyExcelForm.POLICY_TYPE_NAME
        Dim types = {bankFileSecurityBankPolicyText, bankFileSimpleExcelPolicyText, bankFileEasyExcelPolicyText}

        Dim bankFilePolicies = (Await _listOfValueRepository.GetAllAsync()).
            Where(Function(t) types.Contains(t.Type)).
            ToList()

        If If(bankFilePolicies?.Any(Function(t) t.Type = bankFileSecurityBankPolicyText And t.DisplayValue.Split({","c}).Contains($"{z_OrganizationID}")), False) Then
            providers.Add(New BankFileSecurityBankReportProvider(z_OrganizationID, userId:=z_User))
        End If

        If If(bankFilePolicies?.Any(Function(t) t.Type = bankFileSimpleExcelPolicyText And t.DisplayValue.Split({","c}).Contains($"{z_OrganizationID}")), False) Then
            providers.Add(New BankFileSimpleExcelReportProvider(z_OrganizationID, userId:=z_User))
        End If

        If If(bankFilePolicies?.Any(Function(t) t.Type = bankFileEasyExcelPolicyText And t.DisplayValue.Split({","c}).Contains($"{z_OrganizationID}")), False) Then
            providers.Add(New BankFileEasyExcelReportProvider(z_OrganizationID, userId:=z_User))
        End If

    End Function

    Private Shared Function CreateNewListViewItem(reportProvider As IReportProvider, reportName As String) As ListViewItem
        Dim listItem = New ListViewItem(reportName)
        listItem.Tag = reportProvider
        Return listItem
    End Function

    Private Shared Function GetBenchmarkReports() As Collection(Of IReportProvider)
        Return New Collection(Of IReportProvider) From {
            New SalaryIncreaseHistoryReportProvider(),
            New EmployeeProfilesReportProvider(),
            New LoanSummaryByEmployeeReportProvider(),
            New LoanSummaryByTypeReportProvider(),
            New SSSMonthlyReportProvider(),
            New PhilHealthReportProvider(),
            New PagIBIGMonthlyReportProvider(),
            New TaxReportProvider(),
            New ThirteenthMonthDetailedReportProvider(),
            New ThirteenthMonthSummaryReportProvider(),
            New LoanLedgerReportProvider(),
            New PayrollSummaryExcelFormatReportProvider(),
            New BenchmarkAlphalistReportProvider(),
            New PayrollLedgerExcelFormatReportProvider()
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
