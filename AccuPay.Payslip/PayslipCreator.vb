Imports System.IO
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.DB
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports PdfSharp.Pdf
Imports PdfSharp.Pdf.IO
Imports PdfSharp.Pdf.Security

Public Class PayslipCreator

    Const customDateFormat As String = "M/d/yyyy"

    Private ReadOnly _organizationRepository As OrganizationRepository

    Private ReadOnly _payPeriodRepository As PayPeriodRepository

    Private ReadOnly _addressRepository As AddressRepository

    Private _systemOwnerService As SystemOwnerService

    Private _reportDocument As ReportClass

    Private _pdfFile As String

    Private _payslipDatatable As DataTable

    Sub New(organizationRepository As OrganizationRepository,
            payPeriodRepository As PayPeriodRepository,
            addressRepository As AddressRepository,
            systemOwnerService As SystemOwnerService)

        _organizationRepository = organizationRepository

        _payPeriodRepository = payPeriodRepository

        _addressRepository = addressRepository

        _systemOwnerService = systemOwnerService
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
                        organizationId As Integer,
                        payPeriodId As Integer,
                        isActual As SByte,
                        Optional employeeIds As Integer() = Nothing) As PayslipCreator

        Dim _isActual = isActual

        'filter employees, print and email payslip is tested on cinema only
        'test this before deploying
        Dim rptdoc As Object = Nothing

        Dim organization = _organizationRepository.GetById(organizationId)
        Dim payperiod = _payPeriodRepository.GetById(payPeriodId)
        Dim address As Address = If(organization?.PrimaryAddressId Is Nothing,
                                                Nothing,
                                                _addressRepository.
                                                    GetById(organization?.PrimaryAddressId))

        Dim organizationName = organization.Name

        Static current_system_owner As String = _systemOwnerService.GetCurrentSystemOwner()

        If SystemOwnerService.Goldwings = current_system_owner Then

            Dim query As New SQLQueryToDatatable("CALL paystub_payslip(" & organizationId & "," & payPeriodId & "," & _isActual & ");")
            _payslipDatatable = query.ResultTable

            rptdoc = New OfficialPaySlipFormat

            With rptdoc.ReportDefinition.Sections(2)
                Dim objText As TextObject = .ReportObjects("txtOrganizName")
                objText.Text = organizationName.ToUpper

                objText = .ReportObjects("txtPayPeriod")

                objText.Text = $"{payperiod.PayFromDate.ToString(customDateFormat)} to {payperiod.PayToDate.ToString(customDateFormat)}"
            End With

            'ElseIf Reference.BaseSystemOwner.Hyundai = current_system_owner Then

            '    Dim params =
            '    New Object() {orgztnID, _payPeriodId}

            '    Dim _sql As New SQL("CALL `HyundaiPayslip`(?og_rowid, ?pp_rowid, TRUE, NULL);",
            '                   params)

            '    _payslipDatatable = _sql.GetFoundRows.Tables(0)

            '    Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

            '    objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgname")
            '    objText.Text = organizationName.ToUpper

        ElseIf SystemOwnerService.Cinema2000 = current_system_owner Then

            Dim query As New SQLQueryToDatatable("CALL RPT_payslip(" & organizationId & "," & payPeriodId & ", TRUE, NULL);")
            _payslipDatatable = query.ResultTable

            If employeeIds IsNot Nothing AndAlso employeeIds.Count > 0 Then

                _payslipDatatable = _payslipDatatable.AsEnumerable().
                    Where(Function(r) employeeIds.Contains(r.Field(Of Integer)("EmployeeRowID"))).
                    CopyToDataTable

            End If

            rptdoc = New TwoEmpIn1PaySlip

            Dim objText As TextObject = Nothing

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("payperiod")

            Dim nextPayPeriod = _payPeriodRepository.GetNextPayPeriod(payPeriodId)

            If nextPayPeriod IsNot Nothing Then

                objText.Text = $"Payroll period {nextPayPeriod.PayFromDate.ToShortDateString}  to {nextPayPeriod.PayToDate.ToShortDateString}"

            End If

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("OrgContact")
            objText.Text = String.Empty

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("OrgName")
            objText.Text = organizationName.ToUpper

            objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("OrgAddress")
            objText.Text = If(address?.FullAddress, "")
        Else

            Dim n_SQLQueryToDatatable As _
            New SQLQueryToDatatable("CALL PrintDefaultPayslip(" & organizationId & "," & payPeriodId & "," & _isActual & ");")

            _payslipDatatable = n_SQLQueryToDatatable.ResultTable

            Dim rptPayslip As New DefaultPayslipFormat

            With rptPayslip.Section2
                Dim objText As TextObject = .ReportObjects("txtOrganizName")
                objText.Text = organizationName.ToUpper

                objText = .ReportObjects("txtPayPeriod")

                objText.Text = $"{payperiod.PayFromDate.ToString(customDateFormat)} to {payperiod.PayToDate.ToString(customDateFormat)}"
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