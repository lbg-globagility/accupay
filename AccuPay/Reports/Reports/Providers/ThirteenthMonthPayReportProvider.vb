Option Strict On

Imports AccuPay.DB
Imports CrystalDecisions.CrystalReports.Engine

Public Class ThirteenthMonthPayReportProvider
    Implements IReportProvider

    Public Property Name As String = "Thirteenth Month Pay (Detailed)" Implements IReportProvider.Name

    Public Sub Run() Implements IReportProvider.Run

        Dim n_PayrollSummaDateSelection As New PayrollSummaDateSelection

        If n_PayrollSummaDateSelection.ShowDialog = Windows.Forms.DialogResult.OK Then

            Dim date_from, date_to As Object

            date_from = n_PayrollSummaDateSelection.DateFrom
            date_to = n_PayrollSummaDateSelection.DateTo

            Dim sql_print_13thmonth_pay_detailedsummary As _
                New SQL("CALL RPT_13thmonthpayDetailed(?og_rowid, ?date_from, ?date_to);",
                        New Object() {orgztnID, date_from, date_to})

            Try

                Dim dt As New DataTable

                dt = sql_print_13thmonth_pay_detailedsummary.GetFoundRows.Tables(0)

                If sql_print_13thmonth_pay_detailedsummary.HasError Then

                    Throw sql_print_13thmonth_pay_detailedsummary.ErrorException
                Else

                    Dim rptdoc As New ThirteenthMonthPayDetailSummary

                    rptdoc.SetDataSource(dt)

                    Dim crvwr As New CrysRepForm

                    Dim objText As TextObject = Nothing

                    objText = DirectCast(rptdoc.ReportDefinition.Sections(1).ReportObjects("PeriodDate"), TextObject)

                    objText.Text =
                        String.Concat("Salary from ",
                                      DirectCast(date_from, Date).ToShortDateString,
                                       " to ",
                                      DirectCast(date_to, Date).ToShortDateString)

                    objText = DirectCast(rptdoc.ReportDefinition.Sections(1).ReportObjects("txtOrganizationName"), TextObject)

                    objText.Text = orgNam.ToUpper

                    crvwr.crysrepvwr.ReportSource = rptdoc

                    crvwr.Show()

                End If
            Catch ex As Exception

                MsgBox(getErrExcptn(ex, Me.Name))
            Finally

            End Try

        End If

    End Sub

End Class