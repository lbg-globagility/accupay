Option Strict On

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

    Private Const customDateFormat As String = "M/d/yyyy"

    Private Const GoldwingsEmployeeIdColumn As String = "RowID"

    Private Const CinemaEmployeeIdColumn As String = "EmployeeRowID"

    Private Const DefaultEmployeeIdColumn As String = "RowID"

    Private ReadOnly _addressRepository As AddressRepository

    Private ReadOnly _organizationRepository As OrganizationRepository

    Private ReadOnly _payPeriodRepository As PayPeriodRepository

    Private _systemOwnerService As SystemOwnerService

    Private _reportDocument As ReportClass

    Private _pdfFile As String

    Private _payslipDatatable As DataTable

    Private ReadOnly _currentSystemOwner As String

    Sub New(addressRepository As AddressRepository,
            organizationRepository As OrganizationRepository,
            payPeriodRepository As PayPeriodRepository,
            systemOwnerService As SystemOwnerService)

        _addressRepository = addressRepository

        _organizationRepository = organizationRepository

        _payPeriodRepository = payPeriodRepository

        _systemOwnerService = systemOwnerService

        _currentSystemOwner = _systemOwnerService.GetCurrentSystemOwner()
    End Sub

    Public Function GetReportDocument() As ReportClass
        Return _reportDocument
    End Function

    Public Function GetPDF() As String
        Return _pdfFile
    End Function

    Public Function CheckIfEmployeeExists(employeeId As Integer) As Boolean

        Dim payslipList = _payslipDatatable.AsEnumerable()

        Select Case _currentSystemOwner
            Case SystemOwnerService.Goldwings
                Return payslipList.
                        Where(Function(x) x.Field(Of Integer)(GoldwingsEmployeeIdColumn) = employeeId).
                        Any()

            Case SystemOwnerService.Cinema2000
                Return payslipList.
                        Where(Function(x) x.Field(Of Integer)(CinemaEmployeeIdColumn) = employeeId).
                        Any()

            Case Else
                Return payslipList.
                        Where(Function(x) x.Field(Of Integer)(DefaultEmployeeIdColumn) = employeeId).
                        Any()

        End Select

        Return False
    End Function

    Public Function GetFirstEmployee() As DataRow
        Return If(_payslipDatatable.Rows.Count > 0, _payslipDatatable(0), Nothing)
    End Function

    Public Function CreateReportDocument(
                        organizationId As Integer,
                        payPeriodId As Integer,
                        isActual As SByte,
                        Optional employeeIds As Integer() = Nothing) As PayslipCreator

        'Use dapper or entity framework for procedure calls

        Dim organization = _organizationRepository.GetById(organizationId)
        Dim payPeriod = _payPeriodRepository.GetById(payPeriodId)

        Dim organizationName = organization.Name

        If _currentSystemOwner = SystemOwnerService.Goldwings Then

            _payslipDatatable = CreateGoldWingsDataSource(organization, payPeriod, isActual, employeeIds)
            _reportDocument = CreateGoldWingsReport(organization, payPeriod)
            'ElseIf Reference.BaseSystemOwner.Hyundai = current_system_owner Then

            '    Dim params =
            '    New Object() {orgztnID, _payPeriodId}

            '    Dim _sql As New SQL("CALL `HyundaiPayslip`(?og_rowid, ?pp_rowid, TRUE, NULL);",
            '                   params)

            '    _payslipDatatable = _sql.GetFoundRows.Tables(0)

            '    Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = Nothing

            '    objText = rptdoc.ReportDefinition.Sections(2).ReportObjects("txtorgname")
            '    objText.Text = organizationName.ToUpper

        ElseIf _currentSystemOwner = SystemOwnerService.Cinema2000 Then

            _payslipDatatable = CreateCinemaDataSource(organization, payPeriodId, employeeIds)
            _reportDocument = CreateCinemaReport(organization, payPeriodId)
        Else

            _payslipDatatable = CreateDefaultDataSource(organization, payPeriod, isActual, employeeIds)
            _reportDocument = CreateDefaultReport(organization, payPeriod)
        End If

        _reportDocument.SetDataSource(_payslipDatatable)

        Return Me

    End Function

    Private Function CreateGoldWingsReport(organization As Organization,
                                           payPeriod As PayPeriod) As ReportClass
        Dim rptdoc As New OfficialPaySlipFormat

        Dim txtOrganizName = DirectCast(rptdoc.ReportDefinition.Sections(2).ReportObjects("txtOrganizName"), TextObject)
        txtOrganizName.Text = organization.Name.ToUpper()

        Dim txtPayPeriod = DirectCast(rptdoc.ReportDefinition.Sections(2).ReportObjects("txtPayPeriod"), TextObject)
        txtPayPeriod.Text = $"{payPeriod.PayFromDate.ToString(customDateFormat)} to {payPeriod.PayToDate.ToString(customDateFormat)}"

        Return rptdoc
    End Function

    Private Function CreateGoldWingsDataSource(organization As Organization,
                                               payPeriod As PayPeriod,
                                               isActual As SByte,
                                               employeeIds() As Integer) _
                                                As DataTable

        Dim procedureCall = "CALL paystub_payslip(" & organization.RowID & "," & payPeriod.RowID & "," & isActual & ");"

        Return CreateDataSource(procedureCall, GoldwingsEmployeeIdColumn, employeeIds)

    End Function

    Private Function CreateCinemaReport(organization As Organization,
                                        payPeriodId As Integer) As ReportClass

        Dim address As Address = If(organization?.PrimaryAddressId Is Nothing,
                                                Nothing,
                                                _addressRepository.
                                                    GetById(organization.PrimaryAddressId.Value))

        Dim rptdoc = New TwoEmpIn1PaySlip

        Dim nextPayPeriod = _payPeriodRepository.GetNextPayPeriod(payPeriodId)

        If nextPayPeriod IsNot Nothing Then

            Dim txtPayperiod = DirectCast(rptdoc.ReportDefinition.Sections(2).ReportObjects("payperiod"), TextObject)
            txtPayperiod.Text = $"Payroll period {nextPayPeriod.PayFromDate.ToShortDateString}  to {nextPayPeriod.PayToDate.ToShortDateString}"

        End If

        Dim txtOrgContact = DirectCast(rptdoc.ReportDefinition.Sections(2).ReportObjects("OrgContact"), TextObject)
        txtOrgContact.Text = String.Empty

        Dim txtOrgName = DirectCast(rptdoc.ReportDefinition.Sections(2).ReportObjects("OrgName"), TextObject)
        txtOrgName.Text = organization.Name.ToUpper()

        Dim txtOrgAddress = DirectCast(rptdoc.ReportDefinition.Sections(2).ReportObjects("OrgAddress"), TextObject)
        txtOrgAddress.Text = If(address?.FullAddress, "")

        Return rptdoc
    End Function

    Private Function CreateCinemaDataSource(organization As Organization,
                                            payPeriodId As Integer,
                                            Optional employeeIds As Integer() = Nothing) _
                                            As DataTable

        Dim procedureCall = "CALL RPT_payslip(" & organization.RowID & "," & payPeriodId & ", TRUE, NULL);"

        Return CreateDataSource(procedureCall, CinemaEmployeeIdColumn, employeeIds)
    End Function

    Private Function CreateDefaultReport(organization As Organization,
                                         payperiod As PayPeriod) As ReportClass

        Dim rptdoc As New DefaultPayslipFormat

        Dim txtOrganizName = DirectCast(rptdoc.Section2.ReportObjects("txtOrganizName"), TextObject)
        txtOrganizName.Text = organization.Name.ToUpper()

        Dim txtPayPeriod = DirectCast(rptdoc.Section2.ReportObjects("txtPayPeriod"), TextObject)
        txtPayPeriod.Text = $"{payperiod.PayFromDate.ToString(customDateFormat)} to {payperiod.PayToDate.ToString(customDateFormat)}"

        Return rptdoc
    End Function

    Private Function CreateDefaultDataSource(organization As Organization,
                                             payperiod As PayPeriod,
                                             isActual As SByte,
                                             Optional employeeIds() As Integer = Nothing) _
                                             As DataTable

        Dim procedureCall = "CALL PrintDefaultPayslip(" & organization.RowID & "," & payperiod.RowID & "," & isActual & ");"

        Return CreateDataSource(procedureCall, DefaultEmployeeIdColumn, employeeIds)
    End Function

    Private Function CreateDataSource(procedureCall As String,
                                      employeeIdColumn As String,
                                      Optional employeeIds() As Integer = Nothing) _
                                      As DataTable

        Dim query As New SQLQueryToDatatable(procedureCall)

        If employeeIds IsNot Nothing AndAlso employeeIds.Count > 0 Then

            Return query.ResultTable.AsEnumerable().
                Where(Function(r) employeeIds.Contains(r.Field(Of Integer)(employeeIdColumn))).
                CopyToDataTable
        Else
            Return query.ResultTable
        End If
    End Function

    Public Function GeneratePDF(saveFolderPath As String, fileName As String) As PayslipCreator

        Dim pdfFullPath As String = Path.Combine(saveFolderPath, fileName)

        Dim CrExportOptions As ExportOptions
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