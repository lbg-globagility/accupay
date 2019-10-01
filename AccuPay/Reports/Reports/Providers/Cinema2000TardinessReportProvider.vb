Imports CrystalDecisions.CrystalReports.Engine

Public Class Cinema2000TardinessReportProvider
    Implements IReportProvider

    Public Property Name As String = "Tardiness Report" Implements IReportProvider.Name

    Public Sub Run() Implements IReportProvider.Run

        'Dim params =
        '    New Object() {orgztnID}

        'Dim sql As New SQL("CALL RPT_employeeidhistory(?og_rowid);",
        '                   params)

        Try
            'sql.ExecuteQuery()

            'If sql.HasError Then
            '    Throw sql.ErrorException
            'Else

            'Dim dt As New DataTable
            '    dt = sql.GetFoundRows.Tables(0)

            Dim report = New Cinemas_Tardiness_Report

            Dim objText As TextObject = DirectCast(report.ReportDefinition.Sections(2).ReportObjects("txtOrgName"), TextObject)
            objText.Text = orgNam.ToUpper

            objText = DirectCast(report.ReportDefinition.Sections(2).ReportObjects("txtAddress"), TextObject)

            objText.Text = PayrollTools.GetOrganizationAddress()

            'report.SetDataSource(dt)

            Dim crvwr As New CrysRepForm
            crvwr.crysrepvwr.ReportSource = report
            crvwr.Show()

            'End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        End Try

    End Sub

End Class