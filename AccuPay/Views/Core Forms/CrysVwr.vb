Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class CrysVwr

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        'cboxExportTypeOptn.DisplayMember = "Text"
        'cboxExportTypeOptn.ValueMember = "Value"

        cboxExportTypeOptn.DataSource = Nothing

        MyBase.OnLoad(e)

    End Sub

    Private Sub CrysVwr_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        If CrystalReportViewer1.ReportSource IsNot Nothing Then
            CrystalReportViewer1.ReportSource.Dispose()
        End If
    End Sub

    Private Sub CrysVwr_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub CrysVwr_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown, CrystalReportViewer1.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.W Then
            Me.Close()
        ElseIf e.Control AndAlso e.KeyCode = Keys.P Then
            CrystalReportViewer1.PrintReport()

        End If

    End Sub
    Private Sub CrysVwr_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim result = MessageBox.Show("Do you want to Close " & Me.Text & " ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
        If result = DialogResult.No Then
            e.Cancel = True
        ElseIf result = DialogResult.Yes Then
            e.Cancel = False
            CrystalReportViewer1.ReportSource.Dispose()

        End If

    End Sub

    Dim n_ShowSubControls As Boolean = False

    Property ShowSubControls As Boolean

        Get
            Return n_ShowSubControls

        End Get

        Set(value As Boolean)
            n_ShowSubControls = value

            Panel1.Visible = n_ShowSubControls

        End Set

    End Property

    Private cryRpt As ReportDocument

    Private Sub btnExclExport_Click(sender As Object, e As EventArgs) Handles btnExclExport.Click

        Dim err_msg As String = String.Empty

        Dim sfd As New SaveFileDialog With {.DefaultExt = ".xls",
                                            .Filter = "Excel 97-2003 Workbook (.xls)|*.xls",
                                            .RestoreDirectory = True,
                                            .Title = "Export as Excel 97-2003 Workbook"}

        Dim str_path As String = String.Empty

        If sfd.ShowDialog = Windows.Forms.DialogResult.OK Then

            Try

                cryRpt = New ReportDocument

                cryRpt = CrystalReportViewer1.ReportSource

                Dim CrDiskFileDestinationOptions = New DiskFileDestinationOptions()

                Dim CrExportOptions As New ExportOptions

                Dim CrFormatTypeOptions = New ExcelFormatOptions()

                str_path = sfd.FileName

                CrDiskFileDestinationOptions.DiskFileName = str_path '"D:\csharp_net_informations.xls"

                CrExportOptions = cryRpt.ExportOptions

                With CrExportOptions

                    .ExportDestinationType = ExportDestinationType.DiskFile

                    .ExportFormatType = ExportFormatType.ExcelWorkbook

                    .DestinationOptions = CrDiskFileDestinationOptions

                    .FormatOptions = CrFormatTypeOptions

                End With

                cryRpt.Export()

            Catch ex As Exception

                err_msg = getErrExcptn(ex, Me.Name)

            Finally

                If err_msg.Length > 0 Then

                    MsgBox(err_msg)

                Else

                    MsgBox("Done exporting" & Environment.NewLine &
                           "see file in " & str_path)

                End If

            End Try

        End If

    End Sub

End Class