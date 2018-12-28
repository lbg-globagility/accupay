Option Strict On

Imports CrystalDecisions.CrystalReports.Engine

Public Class FiledLeaveReportProvider
    Implements IReportProvider

    Public Property Name As String = "Filed Leave" Implements IReportProvider.Name

    Private orgNameAddressSql As String =
        "SELECT
        CONCAT_WS('\n'
        , UCASE(og.`Name`)
        , CONCAT_WS(', '
                , a.StreetAddress1
                , a.StreetAddress2
                , a.Barangay
                , a.CityTown
                , a.Country
                , a.State
                , a.ZipCode)
		        )
        `FullAddress`
        FROM organization og
        LEFT JOIN (SELECT RowID
			        , IF(LENGTH(TRIM(StreetAddress1)) = 0, NULL, TRIM(StreetAddress1)) `StreetAddress1`
			        , IF(LENGTH(TRIM(StreetAddress2)) = 0, NULL, TRIM(StreetAddress2)) `StreetAddress2`
			        , IF(LENGTH(TRIM(Barangay)) = 0, NULL, TRIM(Barangay)) `Barangay`
			        , IF(LENGTH(TRIM(CityTown)) = 0, NULL, TRIM(CityTown)) `CityTown`
			        , IF(LENGTH(TRIM(Country)) = 0, NULL, TRIM(Country)) `Country`
			        , IF(LENGTH(TRIM(State)) = 0, NULL, TRIM(State)) `State`
			        , IF(LENGTH(TRIM(ZipCode)) = 0, NULL, TRIM(ZipCode)) `ZipCode`

			        FROM address) a ON a.RowID=og.PrimaryAddressID
        WHERE og.RowID = ?ogId;"

    Public Sub Run() Implements IReportProvider.Run
        Dim dateSelector As New PayrollSummaDateSelection()

        If Not dateSelector.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim dateFrom = dateSelector.DateFrom
        Dim dateTo = dateSelector.DateTo

        Dim params = New Object(,) {
            {"OrganizationID", orgztnID},
            {"PayDateFrom", dateSelector.DateFrom},
            {"PayDateTo", dateSelector.DateTo}
        }

        Dim data = DirectCast(callProcAsDatTab(params, "RPT_FiledLeave"), DataTable)

        Dim report = New FiledLeaveReport()
        report.SetDataSource(data)

        Dim title = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtCutoffDate"), TextObject)
        Dim dateFromTitle = dateFrom?.ToString("MMMM dd, yyyy")
        Dim dateToTitle = dateTo?.ToString("MMMM dd, yyyy")
        title.Text = $"From {dateFromTitle} to {dateToTitle}"


        Dim sql As New SQL(orgNameAddressSql,
                           New Object() {orgztnID})
        Dim result = Convert.ToString(sql.GetFoundRow)
        Dim splitValues = Split(result, Chr(10))

        Dim txtCompanyName = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtCompanyName"), TextObject)
        txtCompanyName.Text = splitValues.First

        Dim txtAddress = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtAddress"), TextObject)
        txtAddress.Text = splitValues.Last

        Dim viewer As New CrysRepForm()
        viewer.crysrepvwr.ReportSource = report
        viewer.Show()
    End Sub

End Class
