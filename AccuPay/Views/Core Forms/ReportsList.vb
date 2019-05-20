Option Strict On

Imports System.Collections.ObjectModel

Public Class ReportsList

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
    End Sub

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
                provider.Run()
            Catch ex As NotImplementedException
                MsgBox($"Report Is Not Yet Done: {ex.Message}", MsgBoxStyle.OkOnly)
            End Try
        End If
    End Sub

End Class
