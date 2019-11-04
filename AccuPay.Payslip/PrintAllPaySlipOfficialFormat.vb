Imports Accupay.Data
Imports Accupay.DB
Imports CrystalDecisions.CrystalReports.Engine

Public Class PrintAllPaySlipOfficialFormat

    Private _payPeriodId As Object = Nothing

    Private n_IsPrintingAsActual As SByte = 0

    Sub New(payPeriodId As Integer,
        IsPrintingAsActual As SByte)

        _payPeriodId = payPeriodId

        n_IsPrintingAsActual = IsPrintingAsActual

    End Sub

    Const customDateFormat As String = "'%c/%e/%Y'"

    Private catchdt As New DataTable

    Private sys_ownr As New Reference.BaseSystemOwner

    Public Function GetReportDocument(
                        orgNam As String,
                        orgztnID As Integer,
                        nextPayPeriod As IPayPeriod,
                        Optional employeeIds As Integer() = Nothing) As ReportClass

        'filter employees, print and email payslip is tested on cinema only
        'test this before deploying

        Dim rptdoc As Object = Nothing

        Static current_system_owner As String = sys_ownr.CurrentSystemOwner

        If Reference.BaseSystemOwner.Goldwings = current_system_owner Then

            Dim n_SQLQueryToDatatable As _
                   New SQLQueryToDatatable("CALL paystub_payslip(" & orgztnID & "," & _payPeriodId & "," & n_IsPrintingAsActual & ");")

            catchdt = n_SQLQueryToDatatable.ResultTable

            'rptdoc = New OfficialPaySlipFormat

            With rptdoc.ReportDefinition.Sections(2)
                Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("txtOrganizName")
                objText.Text = orgNam.ToUpper

                objText = .ReportObjects("txtPayPeriod")

                If _payPeriodId > 0 Then
                    objText.Text =
                    New ExecuteQuery("SELECT CONCAT(DATE_FORMAT(PayFromDate," & customDateFormat & "),' to ',DATE_FORMAT(PayToDate," & customDateFormat & ")) `Result`" &
                                     " FROM payperiod WHERE RowID=" & _payPeriodId & ";").Result
                End If
            End With

        ElseIf Reference.BaseSystemOwner.Hyundai = current_system_owner Then

            Dim params =
            New Object() {orgztnID, _payPeriodId}

            Dim _sql As New SQL("CALL `HyundaiPayslip`(?og_rowid, ?pp_rowid, TRUE, NULL);",
                           params)

            catchdt = _sql.GetFoundRows.Tables(0)

            Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgname")
            objText.Text = orgNam.ToUpper

        ElseIf Reference.BaseSystemOwner.Cinema2000 = current_system_owner Then

            Dim params =
            New Object() {orgztnID, _payPeriodId}

            Dim str_query As String = String.Concat(
            "CALL `RPT_payslip`(?og_rowid, ?pp_rowid, TRUE, NULL);")

            Dim _sql As New SQL(str_query,
                            params)

            catchdt = _sql.GetFoundRows.Tables(0)

            If employeeIds IsNot Nothing AndAlso employeeIds.Count > 0 Then

                catchdt = catchdt.AsEnumerable().
                    Where(Function(r) employeeIds.Contains(r.Field(Of Integer)("EmployeeRowID"))).
                    CopyToDataTable

            End If

            'rptdoc = New HyundaiPayslip1

            rptdoc = New TwoEmpIn1PaySlip

            Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("payperiod")

            If nextPayPeriod IsNot Nothing Then

                objText.Text = $"Payroll period {nextPayPeriod.PayFromDate.ToShortDateString}  to {nextPayPeriod.PayToDate.ToShortDateString}"

            End If

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("OrgContact")
            objText.Text = String.Empty

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("OrgName")
            objText.Text = orgNam.ToUpper

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("OrgAddress")
            objText.Text = Convert.ToString(New SQL(String.Concat("SELECT CONCAT_WS(', ',",
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
        Else

            Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("CALL PrintDefaultPayslip(" & orgztnID & "," & _payPeriodId & "," & n_IsPrintingAsActual & ");")

            catchdt = n_SQLQueryToDatatable.ResultTable

            Dim rptPayslip As New TwoEmpIn1PaySlip

            With rptPayslip.Section2
                Dim objText As TextObject = .ReportObjects("txtOrganizName")
                objText.Text = orgNam.ToUpper

                objText = .ReportObjects("txtPayPeriod")

                If _payPeriodId > 0 Then
                    objText.Text =
                    New ExecuteQuery("SELECT CONCAT(DATE_FORMAT(PayFromDate," & customDateFormat & "),' to ',DATE_FORMAT(PayToDate," & customDateFormat & ")) `Result`" &
                                     " FROM payperiod WHERE RowID=" & _payPeriodId & ";").Result
                End If
            End With

            rptdoc = rptPayslip
        End If

        rptdoc.SetDataSource(catchdt)

        Return rptdoc

    End Function

End Class