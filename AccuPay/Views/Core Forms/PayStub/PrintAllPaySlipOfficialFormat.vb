Namespace Global.AccuPay.Views.Payroll

    Class PrintAllPaySlipOfficialFormat

        Private n_PayPeriodRowID As Object = Nothing

        Private n_IsPrintingAsActual As SByte = 0

        Sub New(PayPeriodRowID As Object,
            IsPrintingAsActual As SByte)

            n_PayPeriodRowID = PayPeriodRowID

            n_IsPrintingAsActual = IsPrintingAsActual

            DoProcess()

        End Sub

        Const customDateFormat As String = "'%c/%e/%Y'"

        Private crvwr As New CrysRepForm

        Private catchdt As New DataTable

        Private sys_ownr As New SystemOwner

        Sub DoProcess()

            Dim rptdoc As Object = Nothing

            Static current_system_owner As String = sys_ownr.CurrentSystemOwner

            Dim some_systemowners = New String() {SystemOwner.Goldwings, SystemOwner.DefaultOwner}

            If some_systemowners.Contains(current_system_owner) Then

                Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("CALL paystub_payslip(" & orgztnID & "," & n_PayPeriodRowID & "," & n_IsPrintingAsActual & ");")

                catchdt = n_SQLQueryToDatatable.ResultTable

                rptdoc = New OfficialPaySlipFormat

                With rptdoc.ReportDefinition.Sections(2)
                    Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("txtOrganizName")
                    objText.Text = orgNam.ToUpper

                    objText = .ReportObjects("txtPayPeriod")

                    If ValNoComma(n_PayPeriodRowID) > 0 Then
                        objText.Text =
                New ExecuteQuery("SELECT CONCAT(DATE_FORMAT(PayFromDate," & customDateFormat & "),' to ',DATE_FORMAT(PayToDate," & customDateFormat & ")) `Result`" &
                                 " FROM payperiod WHERE RowID=" & ValNoComma(n_PayPeriodRowID) & ";").Result
                    End If
                End With

            ElseIf SystemOwner.Hyundai = current_system_owner Then

                Dim params =
                New Object() {orgztnID, n_PayPeriodRowID}

                Dim _sql As New SQL("CALL `HyundaiPayslip`(?og_rowid, ?pp_rowid, TRUE, NULL);",
                               params)

                catchdt = _sql.GetFoundRows.Tables(0)

                rptdoc = New HyundaiPayslip1

                Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgname")
                objText.Text = orgNam.ToUpper

            ElseIf SystemOwner.Cinema2000 = current_system_owner Then

                Dim params =
                New Object() {orgztnID, n_PayPeriodRowID}

                Dim str_query As String = String.Concat(
                "CALL `RPT_payslip`(?og_rowid, ?pp_rowid, TRUE, NULL);")

                Dim _sql As New SQL(str_query,
                                params)

                catchdt = _sql.GetFoundRows.Tables(0)

                rptdoc = New TwoEmpIn1PaySlip

                Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("payperiod")
                objText.Text =
                Convert.ToString(
                New SQL(String.Concat("SELECT",
                                      " CONCAT('Payroll period ', DATE_FORMAT(pp.PayFromDate, '%c/%e/%Y'), ' to ', DATE_FORMAT(pp.PayToDate, '%c/%e/%Y')) `Result`",
                                      " FROM payperiod pp WHERE pp.RowID=", n_PayPeriodRowID, ";")).GetFoundRow)

                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("OrgContact")
                objText.Text = String.Empty

                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("OrgName")
                objText.Text = orgNam.ToUpper

                objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("OrgAddress")
                objText.Text =
                Convert.ToString(New SQL(String.Concat("SELECT CONCAT_WS(', ',",
                                                       "IF(LENGTH(TRIM(ad.StreetAddress1)) > 0, ad.StreetAddress1, NULL)",
                                                       ",IF(LENGTH(TRIM(ad.StreetAddress2)) > 0, ad.StreetAddress2, NULL)",
                                                       ",IF(LOCATE('city', ad.Barangay) > 0, IF(LENGTH(TRIM(ad.Barangay)) > 0, ad.Barangay, NULL), CONCAT('Brgy. ', TRIM(ad.Barangay)))",
                                                       ",IF(LOCATE('city', ad.CityTown) > 0, IF(LENGTH(TRIM(ad.CityTown)) > 0, ad.CityTown, NULL), CONCAT(TRIM(ad.CityTown), ' city'))",
                                                       ",IF(LENGTH(TRIM(ad.Country)) > 0, ad.Country, NULL)",
                                                       ",IF(LENGTH(TRIM(ad.State)) > 0, ad.State, NULL)",
                                                       ",IF(LENGTH(TRIM(ad.ZipCode)) > 0, ad.ZipCode, NULL)",
                                                       ") `AddressText`",
                                                       " FROM organization og",
                                                       " INNER JOIN address ad ON ad.RowID=og.PrimaryAddressID",
                                                       " WHERE og.RowID = ", orgztnID,
                                                       ";")).GetFoundRow)

            End If

            rptdoc.SetDataSource(catchdt)

            crvwr.crysrepvwr.ReportSource = rptdoc

            crvwr.Show()

        End Sub

    End Class

End Namespace
