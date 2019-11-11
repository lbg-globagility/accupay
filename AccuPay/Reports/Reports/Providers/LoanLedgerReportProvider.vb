Option Strict On

Imports AccuPay.DB
Imports CrystalDecisions.CrystalReports.Engine

Public Class LoanLedgerReportProvider
    Implements IReportProvider

    Public Property Name As String = "Loan Ledger" Implements IReportProvider.Name

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
        Dim payPeriodSelector = New PayrollSummaDateSelection() With {.ShowLoanType = True}

        If payPeriodSelector.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Dim periodFromId = payPeriodSelector.PayPeriodFromID
        Dim periodToId = payPeriodSelector.PayPeriodToID
        Dim loanTypeId As Object = DBNull.Value
        If payPeriodSelector.LoanTypeId IsNot Nothing Then
            loanTypeId = payPeriodSelector.LoanTypeId
        End If

        Dim params = New Object() {z_OrganizationID, periodFromId, periodToId, loanTypeId}

        Dim ds = New SQL("CALL `RPT_LoanLedger`(?orgId, ?payPeriodFromId, ?payPeriodToId, ?loanTypeId);",
                         params).GetFoundRows
        Dim data = ds.Tables(0)

        Dim report = New LoanLedgerReport()
        report.SetDataSource(data)

        Dim sql As New SQL(orgNameAddressSql,
                           New Object() {z_OrganizationID})
        Dim result = Convert.ToString(sql.GetFoundRow)
        Dim splitValues = Split(result, Chr(10))

        Dim txtCompanyName = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtOrgName"), TextObject)
        txtCompanyName.Text = splitValues.First

        Dim txtAddress = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtAddress"), TextObject)
        txtAddress.Text = splitValues.Last

        Dim viewer As New CrysRepForm()
        viewer.crysrepvwr.ReportSource = report
        viewer.Show()
    End Sub

End Class