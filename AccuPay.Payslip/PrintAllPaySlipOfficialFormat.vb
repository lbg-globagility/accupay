Imports System.IO
Imports Accupay.Data
Imports Accupay.DB
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports PdfSharp.Pdf
Imports PdfSharp.Pdf.IO
Imports PdfSharp.Pdf.Security

Public Class PayslipCreator

    Const customDateFormat As String = "'%c/%e/%Y'"

    Private sys_ownr As New Reference.BaseSystemOwner

    Private _currentPayPeriod As IPayPeriod

    Private _payPeriodId As Integer?

    Private _isActual As SByte = 0

    Private _reportDocument As ReportClass

    Private _pdfFile As String

    Private _payslipDatatable As DataTable

    Sub New(payPeriod As IPayPeriod, isActual As SByte)

        _currentPayPeriod = payPeriod

        _payPeriodId = _currentPayPeriod?.RowID

        _isActual = isActual
    End Sub

    Public Function GetReportDocument() As ReportClass
        Return _reportDocument
    End Function

    Public Function GetPDF() As String
        Return _pdfFile
    End Function

    Public Function GetPayslipDatatable() As DataTable
        Return _payslipDatatable
    End Function

    Public Function GetFirstEmployee() As DataRow
        Return If(_payslipDatatable.Rows.Count > 0, _payslipDatatable(0), Nothing)
    End Function

    Public Function CreateReportDocument(
                        orgztnID As Integer,
                        nextPayPeriod As IPayPeriod,
                        Optional employeeIds As Integer() = Nothing) As PayslipCreator

        'filter employees, print and email payslip is tested on cinema only
        'test this before deploying
        Dim rptdoc As Object = Nothing

        Dim organizationName = New SQL($"SELECT Name FROM organization WHERE RowID = {orgztnID}").
                                    GetFoundRow?.ToString

        Static current_system_owner As String = sys_ownr.CurrentSystemOwner

        If Reference.BaseSystemOwner.Goldwings = current_system_owner Then

            Dim n_SQLQueryToDatatable As _
                   New SQLQueryToDatatable("CALL paystub_payslip(" & orgztnID & "," & _payPeriodId & "," & _isActual & ");")

            _payslipDatatable = n_SQLQueryToDatatable.ResultTable

            'rptdoc = New OfficialPaySlipFormat

            With rptdoc.ReportDefinition.Sections(2)
                Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("txtOrganizName")
                objText.Text = organizationName.ToUpper

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

            _payslipDatatable = _sql.GetFoundRows.Tables(0)

            Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgname")
            objText.Text = organizationName.ToUpper

        ElseIf Reference.BaseSystemOwner.Cinema2000 = current_system_owner Then

            Dim params =
            New Object() {orgztnID, _payPeriodId}

            Dim str_query As String = String.Concat(
            "CALL `RPT_payslip`(?og_rowid, ?pp_rowid, TRUE, NULL);")

            Dim _sql As New SQL(str_query,
                            params)

            _payslipDatatable = _sql.GetFoundRows.Tables(0)

            If employeeIds IsNot Nothing AndAlso employeeIds.Count > 0 Then

                _payslipDatatable = _payslipDatatable.AsEnumerable().
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
            objText.Text = organizationName.ToUpper

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
            New SQLQueryToDatatable("CALL PrintDefaultPayslip(" & orgztnID & "," & _payPeriodId & "," & _isActual & ");")

            _payslipDatatable = n_SQLQueryToDatatable.ResultTable

            Dim rptPayslip As New TwoEmpIn1PaySlip

            With rptPayslip.Section2
                Dim objText As TextObject = .ReportObjects("txtOrganizName")
                objText.Text = organizationName.ToUpper

                objText = .ReportObjects("txtPayPeriod")

                If _payPeriodId > 0 Then
                    objText.Text =
                    New ExecuteQuery("SELECT CONCAT(DATE_FORMAT(PayFromDate," & customDateFormat & "),' to ',DATE_FORMAT(PayToDate," & customDateFormat & ")) `Result`" &
                                     " FROM payperiod WHERE RowID=" & _payPeriodId & ";").Result
                End If
            End With

            rptdoc = rptPayslip
        End If

        rptdoc.SetDataSource(_payslipDatatable)

        _reportDocument = rptdoc

        Return Me

    End Function

    Public Function GeneratePDF(saveFolderPath As String, fileName As String) As PayslipCreator

        Dim pdfFullPath As String = Path.Combine(saveFolderPath, fileName)

        Dim CrExportOptions As CrystalDecisions.[Shared].ExportOptions
        Dim CrDiskFileDestinationOptions As DiskFileDestinationOptions = New DiskFileDestinationOptions()
        Dim CrFormatTypeOptions As PdfRtfWordFormatOptions = New PdfRtfWordFormatOptions()
        CrDiskFileDestinationOptions.DiskFileName = pdfFullPath
        CrExportOptions = _reportDocument.ExportOptions

        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile
        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat
        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions
        CrExportOptions.FormatOptions = CrFormatTypeOptions

        _reportDocument.Export()

        _pdfFile = pdfFullPath

        Return Me
    End Function

    Public Function AddPdfPassword(password As String) As PayslipCreator
        Dim document As PdfDocument = PdfReader.Open(_pdfFile)
        Dim securitySettings As PdfSecuritySettings = document.SecuritySettings
        securitySettings.UserPassword = password
        securitySettings.OwnerPassword = password
        securitySettings.PermitAccessibilityExtractContent = False
        securitySettings.PermitAnnotations = False
        securitySettings.PermitAssembleDocument = False
        securitySettings.PermitExtractContent = False
        securitySettings.PermitFormsFill = True
        securitySettings.PermitFullQualityPrint = False
        securitySettings.PermitModifyDocument = False
        securitySettings.PermitPrint = True
        document.Save(_pdfFile)

        Return Me
    End Function

End Class