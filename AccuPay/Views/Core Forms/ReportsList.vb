Option Strict On

Imports System.Collections.ObjectModel

Public Class ReportsList

    Dim sys_ownr As New SystemOwner

    Private curr_sys_owner_name As String = sys_ownr.CurrentSystemOwner

    Private Const ActualDescription As String = "(Actual)"

    Private Sub ReportsList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim providers = New Collection(Of IReportProvider) From {
            New SalaryIncreaseHistoryReportProvider(),
            New EmploymentRecordReportProvider(),
            New EmployeeProfilesReportProvider(),
            New PostEmploymentClearanceReportProvider(),
            New EmployeeIdentificationNumberReportProvider(),
            New EmployeeOffenseReportProvider(),
            New LeaveLedgerReportProvider() With {.IsNewReport = True},
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
            New AgencyFeeReportProvider(),
            New PayrollLedgerExcelFormatReportProvider(),
            New LoanLedgerReportProvider()
        }
        'New PayrollLedgerReportProvider(),

        If sys_ownr.CurrentSystemOwner = SystemOwner.Benchmark Then
            providers = GetBenchmarkReports()
        End If

        For Each provider In providers
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

        If sys_ownr.CurrentSystemOwner = SystemOwner.Benchmark Then

            lvMainMenu.Items.Add(CreateNewListViewItem("(Declared)"))
            lvMainMenu.Items.Add(CreateNewListViewItem(ActualDescription))
        End If
    End Sub

    Private Shared Function CreateNewListViewItem(suffix As String) As ListViewItem
        Dim summaryProvider As New PayrollSummaryExcelFormatReportProvider()

        Dim summaryListItem = New ListViewItem(summaryProvider.Name & " " & suffix)
        summaryListItem.Tag = summaryProvider
        Return summaryListItem
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
                If sys_ownr.CurrentSystemOwner = SystemOwner.Benchmark AndAlso
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