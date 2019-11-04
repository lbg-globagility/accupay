Imports Accupay.Payslip

Public Class Form1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim n_PrintAllPaySlipOfficialFormat As _
            New PrintAllPaySlipOfficialFormat(619,
                                              IsPrintingAsActual:=False)

        Dim nextPayPeriod As New PayPeriod With {
        .RowID = 620,
        .PayFromDate = New Date(2019, 10, 1),
        .PayToDate = New Date(2019, 10, 15)
        }

        Dim reportDocument = n_PrintAllPaySlipOfficialFormat.GetReportDocument("Cinema 2000s", 2, nextPayPeriod)

        Dim crvwr As New CrystalReportsFormViewer
        crvwr.CrystalReportViewer1.ReportSource = reportDocument
        crvwr.Show()

    End Sub

End Class