Option Strict On

Imports CrystalDecisions.CrystalReports.Engine

Public Class LateUTAbsentSummaryReportProvider
    Implements IReportProvider

    Public Property Name As String = "Late, Undertime and Absent Summary" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Sub Run() Implements IReportProvider.Run

        Dim n_PayrollSummaDateSelection As New MultiplePayPeriodSelectionDialog

        If n_PayrollSummaDateSelection.ShowDialog = Windows.Forms.DialogResult.OK Then

            Dim date_from, date_to As Object

            date_from = n_PayrollSummaDateSelection.DateFrom
            date_to = n_PayrollSummaDateSelection.DateTo

            Dim sql_print_late_ut_absent_summary As _
                New SQL("CALL RPT_AttendanceDeduction(?og_rowid, ?date_f, ?date_t, NULL);",
                        New Object() {orgztnID, date_from, date_to})

            Try

                Dim dt As New DataTable

                dt = sql_print_late_ut_absent_summary.GetFoundRows.Tables(0)

                If sql_print_late_ut_absent_summary.HasError Then

                    Throw sql_print_late_ut_absent_summary.ErrorException
                Else

                    Dim rptdoc As New Late_Undertime_and_Absent_Summary

                    rptdoc.SetDataSource(dt)

                    Dim crvwr As New CrysRepForm

                    Dim objText As TextObject = Nothing

                    objText = DirectCast(rptdoc.ReportDefinition.Sections(1).ReportObjects("PeriodDate"), TextObject)

                    objText.Text =
                        String.Concat("Salary Date from ",
                                      DirectCast(date_from, Date).ToShortDateString,
                                       " to ",
                                      DirectCast(date_to, Date).ToShortDateString)

                    objText = DirectCast(rptdoc.ReportDefinition.Sections(1).ReportObjects("txtOrganizationName"), TextObject)

                    objText.Text = z_OrganizationName.ToUpper

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
