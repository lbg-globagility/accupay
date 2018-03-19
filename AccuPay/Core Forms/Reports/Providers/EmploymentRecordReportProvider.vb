Imports Acupay

Public Class EmploymentRecordReportProvider
    Implements IReportProvider

    Public Property Name As String = "Employees' Employment Record" Implements IReportProvider.Name

    Public Sub Run() Implements IReportProvider.Run

        Dim sql_print_employment_history As _
            New SQL("CALL RPT_employment_record(?og_rowid);",
                    New Object() {orgztnID})

        Try

            Dim dt As New DataTable

            dt = sql_print_employment_history.GetFoundRows.Tables(0)

            If sql_print_employment_history.HasError Then

                Throw sql_print_employment_history.ErrorException
            Else

                Dim rptdoc As New Employees_Employment_Record

                rptdoc.SetDataSource(dt)

                Dim crvwr As New CrysRepForm

                crvwr.crysrepvwr.ReportSource = rptdoc

                crvwr.Show()

            End If
        Catch ex As Exception

            MsgBox(getErrExcptn(ex, Me.Name))
        Finally

        End Try

    End Sub

End Class
