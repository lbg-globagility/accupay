Imports Acupay

Imports CrystalDecisions.CrystalReports.Engine

Public Class EmployeeIdentificationNumberReportProvider
    Implements IReportProvider

    Public Property Name As String = "Employee's Identification Number" Implements IReportProvider.Name

    Public Sub Run() Implements IReportProvider.Run

        Dim str_quer_address As String =
            String.Concat("SELECT CONCAT_WS(', '",
                          ", IF(LENGTH(TRIM(ad.StreetAddress1)) = 0, NULL, ad.StreetAddress1)",
                          ", IF(LENGTH(TRIM(ad.StreetAddress2)) = 0, NULL, ad.StreetAddress2)",
                          ", IF(LENGTH(TRIM(ad.Barangay)) = 0, NULL, ad.Barangay)",
                          ", IF(LENGTH(TRIM(ad.CityTown)) = 0, NULL, ad.CityTown)",
                          ", IF(LENGTH(TRIM(ad.Country)) = 0, NULL, ad.Country)",
                          ", IF(LENGTH(TRIM(ad.State)) = 0, NULL, ad.State)",
                          ") `Result`",
                          " FROM organization og",
                          " LEFT JOIN address ad ON ad.RowID = og.PrimaryAddressID",
                          " WHERE og.RowID = ", orgztnID, ";")

        Dim params =
            New Object() {orgztnID}

        Dim sql As New SQL("CALL RPT_employeeidhistory(?og_rowid);",
                           params)

        Try
            sql.ExecuteQuery()

            If sql.HasError Then
                Throw sql.ErrorException
            Else

                Dim dt As New DataTable
                dt = sql.GetFoundRows.Tables(0)

                Dim report = New Employees_Identification_Number

                Dim objText As TextObject = DirectCast(report.ReportDefinition.Sections(2).ReportObjects("txtOrgName"), TextObject)
                objText.Text = orgNam.ToUpper

                objText = DirectCast(report.ReportDefinition.Sections(2).ReportObjects("txtAddress"), TextObject)

                Dim _address As String =
                    Convert.ToString(New SQL(str_quer_address).GetFoundRow)
                objText.Text = _address

                report.SetDataSource(dt)

                Dim crvwr As New CrysRepForm
                crvwr.crysrepvwr.ReportSource = report
                crvwr.Show()

            End If

        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        End Try

    End Sub

End Class
