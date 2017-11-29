Imports System.Xml.XPath
Imports System.Collections.ObjectModel
Imports CrystalDecisions.CrystalReports.Engine
Imports System.IO
Imports System.Text.RegularExpressions

Public Class ReportsList

    Private Sub ReportsList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim providers = New Collection(Of IReportProvider) From {
            New AttendanceSheetReportProvider(),
            New SalaryIncreaseHistoryReportProvider(),
            New EmploymentRecordReportProvider(),
            New EmployeeIdentificationNumberReportProvider(),
            New EmployeeOffenseReportProvider(),
            New PayrollLedgerReportProvider(),
            New LeaveLedgerReportProvider(),
            New PagIBIGMonthlyReportProvider(),
            New ThirteenthMonthPayReportProvider(),
            New ThirteenthMonthSummaryReportProvider(),
            New LoanSummaryReportProvider(),
            New LoanSummaryByTypeReportProvider(),
            New EmployeeProfilesReportProvider(),
            New PhilHealthReportProvider(),
            New SSSMonthlyReportProvider(),
            New TaxReportProvider(),
            New PostEmploymentClearanceReportProvider(),
            New AgencyFeeReportProvider()
        }

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
        If lvMainMenu.Items.Count > 0 And e.Button = Windows.Forms.MouseButtons.Left Then
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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Button1.Enabled = False

        Dim rptdoc As ReportDocument

        rptdoc = New Crystal_Report11601C_BIR

        Static n_dt As New DataTable

        Static once As SByte = 0

        If once = 0 Then

            For ii = 1 To 120

                Dim n_dcol As New DataColumn

                n_dcol.ColumnName = "COL" & ii

                n_dt.Columns.Add(n_dcol)

            Next

        End If

        rptdoc.SetDataSource(n_dt)

        Dim crvwr As New CrysRepForm

        crvwr.crysrepvwr.ReportSource = rptdoc

        Dim papy_string = ""

        crvwr.Text = papy_string

        crvwr.Show()

        Button1.Enabled = True

    End Sub

    Private Sub Open_Remote_Connection(Optional strComputer As String = "GLOBAL-A-PC\Users\Public\Downloads\Test1.txt",
                                       Optional strUsername As String = Nothing,
                                       Optional strPassword As String = Nothing)
        '//====================================================================================
        '//using NET USE to open a connection to the remote computer
        '//with the specified credentials. if we dont do this first, File.Copy will fail
        '//====================================================================================
        Dim ProcessStartInfo As New System.Diagnostics.ProcessStartInfo
        ProcessStartInfo.FileName = "net"
        ProcessStartInfo.Arguments = "use \\" & strComputer & "\c$ /USER:" & strUsername & " " & strPassword
        ProcessStartInfo.WindowStyle = ProcessWindowStyle.Maximized 'Hidden
        System.Diagnostics.Process.Start(ProcessStartInfo)

        '//============================================================================
        '//wait 2 seconds to let the above command complete or the copy will still fail
        '//============================================================================
        System.Threading.Thread.Sleep(2000)

    End Sub

End Class
